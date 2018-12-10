using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CrossSell;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.OffersV1;
using Carwale.Entity.Offers;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CrossSell;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Template;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Carwale.Interfaces.NewCars;

namespace Carwale.BL.PriceQuote
{
    public class PQAdapterAndroidV1 : IPQAdapter
    {
        private readonly IPQRepository _pqRepo;
        private readonly ICarVersionCacheRepository _versionCachedRepo;
        private readonly IDealerSponsoredAdRespository _dealerRepo;
        private readonly IPQGeoLocationBL _cityRepo;
        private readonly IGeoCitiesCacheRepository _cityCachedRepo;
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly IPaidCrossSell _paidCrossSell;
        private readonly IHouseCrossSell _houseCrossSell;
        private readonly ICampaign _campaign;
        private readonly ICarModels _carModelsBL;
        private readonly IPrices _pricesBL;
        private readonly INewCarDealersCache _newCarDealersCacheRepo;
        private readonly ICustomerTracking _trackingBL;
        private readonly ICarPriceQuoteAdapter _priceQuote;
        private readonly ITemplate _campaignTemplate;
        private readonly IPriceQuoteBL _priceQuoteBL;
        private readonly IOffersAdapter _offersAdapter;

        public PQAdapterAndroidV1(IUnityContainer container, IPQRepository pqRepo, ICarVersionCacheRepository versionCachedRepo,
            IDealerSponsoredAdRespository dealerRepo, IPQGeoLocationBL cityRepo, IGeoCitiesCacheRepository cityCachedRepo,
            ITemplatesCacheRepository tempCache, ICampaign campaign, ICarModels carModelsBL, IPrices pricesBL,
            ICustomerTracking trackingBL, ICarPriceQuoteAdapter priceQuote, ITemplate campainTemplate, IPaidCrossSell paidCrossSell,
            IHouseCrossSell houseCrossSell, IPriceQuoteBL priceQuoteBL, IOffersAdapter offersAdapter)
        {
            _pqRepo = pqRepo;
            _versionCachedRepo = versionCachedRepo;
            _dealerRepo = dealerRepo;
            _cityRepo = cityRepo;
            _cityCachedRepo = cityCachedRepo;
            _tempCache = tempCache;
            _paidCrossSell = paidCrossSell;
            _houseCrossSell = houseCrossSell;
            _campaign = campaign;
            _carModelsBL = carModelsBL;
            _pricesBL = pricesBL;
            _trackingBL = trackingBL;
            _priceQuote = priceQuote;
            _newCarDealersCacheRepo = container.Resolve<INewCarDealersCache>();
            _campaignTemplate = campainTemplate;
            _priceQuoteBL = priceQuoteBL;
            _offersAdapter = offersAdapter;
        }

        public List<T> GetPQ<T>(PQInput pqInputes, string userIdentifier)
        {

            var pqList = new List<PQAndroid>();
            var pq = GetNewPQ(pqInputes, userIdentifier);
            if (pq == null)
            {
                return null;
            }

            var mainCarDataTrackingEntity = _priceQuoteBL.GetBasicTrackingObject(pqInputes);
            mainCarDataTrackingEntity.OnRoadPrice = pq.onRoadPrice;
            mainCarDataTrackingEntity.CampaignType = (int)CampaignAdType.Pq;
            mainCarDataTrackingEntity.PageId = CustomParser.parseIntObject(pqInputes.PageId);
            _trackingBL.TrackPqImpression(mainCarDataTrackingEntity, pq.sponsoredDealer, pq.priceQuoteList);
            int originalPageId = CustomParser.parseIntObject(pqInputes.PageId);
            pqList.Add(pq);

            try
            {
                var crossSellCampaign = new CrossSellDetail();

                if (!pqInputes.IsSponsoredCarShowed && pq.sponsoredDealer.DealerId < 1)
                {
                    crossSellCampaign = _paidCrossSell.GetPaidCrossSell(pqInputes.CarVersionId,
                        new Location { CityId = pqInputes.CityId, ZoneId = CustomParser.parseIntObject(pqInputes.ZoneId) });

                    pqInputes.PageId = SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString();

                    if (crossSellCampaign == null || crossSellCampaign.CampaignDetail.Id == 0)
                    {
                        crossSellCampaign = _houseCrossSell.GetHouseCrossSell(pqInputes.CarVersionId,
                            new Location { CityId = pqInputes.CityId, ZoneId = CustomParser.parseIntObject(pqInputes.ZoneId) }, pqInputes.SourceId);
                        pqInputes.PageId = SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString();
                    }

                    if (crossSellCampaign == null)
                    {
                        return (List<T>)Convert.ChangeType(pqList, typeof(List<T>));
                    }

                    pqInputes.CarVersionId = crossSellCampaign.CarVersionDetail.VersionId;
                    pqInputes.CampaignId = crossSellCampaign.CampaignDetail.Id;
                    pqInputes.CarModelId = crossSellCampaign.CarVersionDetail.ModelId;
                    var CrossSellpq = GetNewPQ(pqInputes, userIdentifier);

                    var sponsoredDealer = Mapper.Map<Campaign, SponsoredDealer>(crossSellCampaign.CampaignDetail);

                    // TODO : send mail when campaign id is 0

                    if (String.IsNullOrWhiteSpace(sponsoredDealer.DealerMobile))
                    {
                        sponsoredDealer.DealerMobile = CWConfiguration.tollFreeNumber;
                    }

                    CrossSellpq.sponsoredDealer = _campaignTemplate.CampaignDealerInfo(sponsoredDealer, pqInputes.PageId, pqInputes.SourceId);

                    if (pqInputes.PageId == SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString())
                    {
                        CrossSellpq.sponsoredDealer.LeadSource = new List<LeadSource>();
                        CrossSellpq.sponsoredDealer.LeadSource.Add(Campaigns.LeadSource.GetLeadSource("Link", (short)LeadSources.PaidCrossSellPQ, pqInputes.SourceId));
                        CrossSellpq.sponsoredDealer.LeadSource.Add(Campaigns.LeadSource.GetLeadSource("Button", (short)LeadSources.PaidCrossSellPQ, pqInputes.SourceId));
                    }
                    else if (pqInputes.PageId == SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString())
                    {
                        CrossSellpq.sponsoredDealer.LeadSource = new List<LeadSource>();
                        CrossSellpq.sponsoredDealer.LeadSource.Add(Campaigns.LeadSource.GetLeadSource("Link", (short)LeadSources.HouseCrossSellPQ, pqInputes.SourceId));
                        CrossSellpq.sponsoredDealer.LeadSource.Add(Campaigns.LeadSource.GetLeadSource("Button", (short)LeadSources.HouseCrossSellPQ, pqInputes.SourceId));
                    }

                    if (CrossSellpq != null)
                    {
                        pqList.Add(CrossSellpq);
                    }

                    var carDataTrackingEntity = _priceQuoteBL.GetBasicTrackingObject(pqInputes);
                    carDataTrackingEntity.OnRoadPrice = CrossSellpq.onRoadPrice;
                    carDataTrackingEntity.CampaignType = (int)CampaignAdType.PaidCrossSell;
                    carDataTrackingEntity.PageId = originalPageId;
                    _trackingBL.TrackPqImpression(carDataTrackingEntity, CrossSellpq.sponsoredDealer, CrossSellpq.priceQuoteList);
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "PQAdapterAndroidV1.GetPQ()");
                exception.LogException();
            }

            return (List<T>)Convert.ChangeType(pqList, typeof(List<T>));
        }
        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on customer datails
        /// </summary>
        /// <param name="pqInputes">Customer Details</param>
        /// <returns>A  List ot type PQ</returns>
        private PQAndroid GetNewPQ(PQInput pqInputes, string userIdentifier)
        {
            var pqList = new PQAndroid();
            var pq = new PQ();
            CarPriceQuoteDTO carPriceQuote = null;
            try
            {
                //getting and assigning User Ip
                pqInputes.ClientIp = UserTracker.GetUserIp();

                if (pqInputes.CarVersionId < 1)
                    pqInputes.CarVersionId = _versionCachedRepo.GetDefaultVersionId(pqInputes.CityId, pqInputes.CarModelId);

                if (pqInputes.CarVersionId < 1)
                {
                    return null;
                }

                //getting cardetais based on version id
                var carDetail = _versionCachedRepo.GetVersionDetailsById(pqInputes.CarVersionId);

                //getting price quote based on city and version id
                if (pqInputes.CarModelId < 1)
                {
                    pqInputes.CarModelId = carDetail.ModelId;
                }
                carPriceQuote = _priceQuote.GetModelPriceQuote(carDetail.ModelId, pqInputes.CityId, true, false, true);
                if (carPriceQuote != null && carPriceQuote.VersionPricesList != null)
                {
                    pq.PriceQuoteList = _pricesBL.MapFromVersionPriceQuoteDTO(carPriceQuote.VersionPricesList.Where(x => x.VersionId == pqInputes.CarVersionId), pqInputes.SourceId);
                }
                else
                {
                    pq.PriceQuoteList = new List<PQItem>();
                }

                // check for sponsored car price
                if (pq.PriceQuoteList.Count == 0
                                && pqInputes.PageId == SponsoredCarPageId.PQSponsorCar.GetHashCode().ToString())
                { return null; }

                //getting cust citydetails based on cityid and zone id
                var cityDetail = _cityRepo.GetCityDetailsV2(pqInputes.CityId, CustomParser.parseIntObject(pqInputes.ZoneId), pqInputes.AreaId);

                // getting sponsored campaign
                SponsoredDealer dealerSponsoredDetails = new SponsoredDealer();

                var cwOffers = new PQOfferEntity();

                if (pq.PriceQuoteList.Count > 0)
                {
                    if (pqInputes.PageId != SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString() && pqInputes.PageId != SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString())
                    {
                        dealerSponsoredDetails = _campaign.GetSponsorDealerAd(carDetail.ModelId, pqInputes.SourceId,
                            new Location { CityId = cityDetail.CityId, ZoneId = CustomParser.parseIntObject(cityDetail.ZoneId), AreaId = cityDetail.AreaId });

                        if (dealerSponsoredDetails != null && dealerSponsoredDetails.DealerId > 0)
                        {
                            int templateId = dealerSponsoredDetails.AssignedTemplateId;
                            var template = _tempCache.GetById(templateId);
                            _campaignTemplate.ResolveTemplate(template, ref dealerSponsoredDetails);
                            dealerSponsoredDetails.LeadSource = new List<LeadSource>();
                            dealerSponsoredDetails.LeadSource.Add(BL.Campaigns.LeadSource.GetLeadSource("Link", (short)LeadSources.PQ, pqInputes.SourceId));
                            dealerSponsoredDetails.LeadSource.Add(BL.Campaigns.LeadSource.GetLeadSource("Button", (short)LeadSources.PQ, pqInputes.SourceId));
                        }
                    }
                    //offers
                    cwOffers = new PQOfferEntity();
                }

                var AssigneddealerList = (_newCarDealersCacheRepo.GetNCSDealers(carDetail.ModelId, cityDetail.CityId)).ToList();

                var specialZones = new List<PuneThaneZones>();

                pqList.priceQuoteList = pq.PriceQuoteList;
                pqList.makeId = carDetail.MakeId;
                pqList.MakeName = carDetail.MakeName;
                pqList.modelId = carDetail.ModelId;
                pqList.modelName = carDetail.ModelName;
                pqList.versionId = carDetail.VersionId;
                pqList.versionName = carDetail.VersionName;
                pqList.maskingName = carDetail.MaskingName;
                pqList.largePicUrl = carDetail.ModelImageLarge;
                pqList.smallPicUrl = carDetail.ModelImageSmall;
                pqList.HostUrl = carDetail.HostURL;
                pqList.OriginalImgPath = carDetail.OriginalImgPath;
                pqList.carName = carDetail.MakeName + " " + carDetail.ModelName + " " + carDetail.VersionName;
                pqList.reviewRate = carDetail.ReviewRate;
                pqList.specsSummery = string.IsNullOrWhiteSpace(carDetail.SpecSummery) ? "" : carDetail.SpecSummery.Replace("|", "");
                pqList.cityId = cityDetail.CityId;
                pqList.ZoneId = cityDetail.ZoneId;
                pqList.AreaId = cityDetail.AreaId;
                pqList.cityName = cityDetail.CityName;
                pqList.zoneName = cityDetail.ZoneName;
                pqList.AreaName = cityDetail.AreaName;
                pqList.sponsoredDealer = dealerSponsoredDetails;
                pqList.otherVersions = _versionCachedRepo.GetCarVersionsByType("new", carDetail.ModelId);
                pqList.otherCities = Mapper.Map<List<Carwale.Entity.Geolocation.City>, List<PriceQuoteCityDTO>>(_cityCachedRepo.GetPQCitiesByModelId(carDetail.ModelId));
                pqList.alternativeCars = Mapper.Map<List<SimilarCarModelsDTO>, List<SimilarCarsAndroidDTO>>(_carModelsBL.GetSimilarCarsForApp(carDetail.ModelId, userIdentifier));
                pqList.modelDetailUrl = GetAPIHostUrl() + "modeldetails/?budget=" + -1 + "&fuelTypes=" + -1 + "&bodyTypes=" + -1 + "&transmission=" + -1
                                        + "&seatingCapacity=" + -1 + "&enginePower=" + -1 + "&importantFeatures=" + -1 + "&modelId=" + carDetail.ModelId;
                pqList.versionDetailUrl = GetAPIHostUrl() + "Versiondetails?versionId=" + carDetail.VersionId;
                pqList.otherCityUrl = GetAPIHostUrl() + "Pricequote?versionId=" + carDetail.VersionId + "&preferenceId=" + 10000 + "&name=" + pqInputes.Name
                                        + "&emailId=" + pqInputes.Email + "&mobileNo=" + pqInputes.Mobile;
                pqList.formatedonRoadPrice = string.IsNullOrEmpty(pq.OnRoadPrice.ToString()) || pq.OnRoadPrice == 0 ? "" : Utility.Format.Numeric(pq.OnRoadPrice.ToString());
                pqList.onRoadPrice = pq.OnRoadPrice;
                pqList.offers = cwOffers;
                pqList.dealerList = AssigneddealerList;
                pqList.SpecialZones = specialZones;
                pqList.Zones = Mapper.Map<List<CityZones>, List<Carwale.DTOs.Geolocation.CityZonesDTO>>(_cityRepo.GetPQCityZonesList(carDetail.ModelId));
                pqList.IsSponsoredCar = pqInputes.PageId == SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString() || pqInputes.PageId == SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString()
                                                                    ? true : false;
                pqList.OfferV1 = GetOffer(carDetail, cityDetail);
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "PQAdapterAndroidV1.GetNewPQ()");
                exception.LogException();
            }
            return pqList;
        }

        private OfferDto GetOffer(CarVersionDetails carDetails, LocationV2 locationDetails)
        {
            var offer = new OfferDto();
            try
            {
                var offerInput = Mapper.Map<CarVersionDetails, OfferInput>(carDetails);
                Mapper.Map<LocationV2, OfferInput>(locationDetails, offerInput);
                offerInput.ApplicationId = (int)Application.CarWale;

                offer = _offersAdapter.GetOffers(offerInput);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PQAdapterAndroidV1.GetOffer() : modelId : " + carDetails.ModelId + " VersionId : " + carDetails.VersionId + " City : " + locationDetails.CityId);
            }
            return offer;
        }

        private string GetAPIHostUrl()
        {
            return ConfigurationManager.AppSettings["WebApiHostUrl"];
        }



        public List<T> GetPQByIds<T>(List<ulong> pqIdList) where T : new()
        {
            throw new NotImplementedException();
        }


        public SponsoredDealer GetSponsorDealerAd(int modelId, int cityId, string zoneId)
        {
            throw new NotImplementedException();
        }

        public List<PQOnRoadPrice> GetPQ(int cityId, int versionId)
        {
            throw new NotImplementedException();
        }


        public List<T> GetPQByIds<T>(List<string> pqIdList, string userIdentifier) where T : new()
        {
            throw new NotImplementedException();
        }
    }

}
using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.Offers;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Offers;
using Carwale.Entity.OffersV1;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CrossSell;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.NewCars;
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

namespace Carwale.BL.PriceQuote
{
    public class PQAdapterAndroidV2 : IPQAdapter
    {
        private readonly IPQRepository _pqRepo;
        private readonly ICarVersionCacheRepository _versionCachedRepo;
        private readonly IDealerSponsoredAdRespository _dealerRepo;
        private readonly IGeoCitiesCacheRepository _cityCachedRepo;
        private readonly IPriceQuoteBL _priceQuoteBL;
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
        private readonly IPQGeoLocationBL _pqGeoLocationBl;
        private readonly IOffersAdapter _offersAdapter;

        public PQAdapterAndroidV2(IUnityContainer container, IPQRepository pqRepo, ICarVersionCacheRepository versionCachedRepo,
            IDealerSponsoredAdRespository dealerRepo, IGeoCitiesCacheRepository cityCachedRepo, ITemplatesCacheRepository tempCache,
            IPriceQuoteBL priceQuoteBL, ICampaign campaign, ICarModels carModelsBL, IPrices pricesBL, ICustomerTracking trackingBL,
            ICarPriceQuoteAdapter priceQuote, ITemplate campainTemplate, IPaidCrossSell paidCrossSell, IHouseCrossSell houseCrossSell,
            IPQGeoLocationBL pqGeoLocationBl, IOffersAdapter offersAdapter)
        {
            _pqRepo = pqRepo;
            _versionCachedRepo = versionCachedRepo;
            _dealerRepo = dealerRepo;
            _cityCachedRepo = cityCachedRepo;
            _tempCache = tempCache;
            _priceQuoteBL = priceQuoteBL;
            _paidCrossSell = paidCrossSell;
            _houseCrossSell = houseCrossSell;
            _campaign = campaign;
            _carModelsBL = carModelsBL;
            _pricesBL = pricesBL;
            _trackingBL = trackingBL;
            _priceQuote = priceQuote;
            _newCarDealersCacheRepo = container.Resolve<INewCarDealersCache>();
            _campaignTemplate = campainTemplate;
            _pqGeoLocationBl = pqGeoLocationBl;
            _offersAdapter = offersAdapter;
        }

        public List<T> GetPQ<T>(PQInput pqInputes, string userIdentifier)
        {
            var pqList = new List<PQAndroidV2>();
            var pq = GetNewPQ(pqInputes, userIdentifier);

            if (pq == null)
            {
                return null;
            }

            var mainCarDataTrackingEntity = _priceQuoteBL.GetBasicTrackingObject(pqInputes);
            mainCarDataTrackingEntity.OnRoadPrice = pq.OnRoadPrice;
            mainCarDataTrackingEntity.CampaignType = (int)CampaignAdType.Pq;
            mainCarDataTrackingEntity.PageId = CustomParser.parseIntObject(pqInputes.PageId);
            _trackingBL.TrackPqImpression(mainCarDataTrackingEntity, pq.SponsoredDealer, pq.PriceQuoteList);
            int originalPageId = CustomParser.parseIntObject(pqInputes.PageId);
            pqList.Add(pq);

            try
            {
                if (!pqInputes.IsSponsoredCarShowed && pq.SponsoredDealer.DealerId < 1)
                {
                    var crossSellCampaign = _paidCrossSell.GetPaidCrossSell(pqInputes.CarVersionId,
                         new Location { CityId = pqInputes.CityId, ZoneId = CustomParser.parseIntObject(pqInputes.ZoneId) });

                    pqInputes.PageId = SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString();

                    if (crossSellCampaign == null || crossSellCampaign.CampaignDetail.Id == 0)
                    {
                        crossSellCampaign = _houseCrossSell.GetHouseCrossSell(pqInputes.CarVersionId,
                            new Location { CityId = pqInputes.CityId, ZoneId = CustomParser.parseIntObject(pqInputes.ZoneId) }, pqInputes.SourceId);
                        pqInputes.PageId = SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString();
                    }

                    if (crossSellCampaign == null)
                        return (List<T>)Convert.ChangeType(pqList, typeof(List<T>));

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

                    CrossSellpq.SponsoredDealer = Mapper.Map<SponsoredDealer, SponsoredDealerDTO>(_campaignTemplate.CampaignDealerInfo(sponsoredDealer, pqInputes.PageId, pqInputes.SourceId));

                    if (pqInputes.PageId == SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString())
                    {
                        CrossSellpq.SponsoredDealer.LeadSource = new List<LeadSourceDTO>();
                        CrossSellpq.SponsoredDealer.LeadSource.Add(
                                    Mapper.Map<LeadSource, LeadSourceDTO>(Campaigns.LeadSource.GetLeadSource("Link", (short)LeadSources.PaidCrossSellPQ, pqInputes.SourceId)));
                        CrossSellpq.SponsoredDealer.LeadSource.Add(
                                    Mapper.Map<LeadSource, LeadSourceDTO>(Campaigns.LeadSource.GetLeadSource("Button", (short)LeadSources.PaidCrossSellPQ, pqInputes.SourceId)));
                    }
                    else if (pqInputes.PageId == SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString())
                    {
                        CrossSellpq.SponsoredDealer.LeadSource = new List<LeadSourceDTO>();
                        CrossSellpq.SponsoredDealer.LeadSource.Add(
                                    Mapper.Map<LeadSource, LeadSourceDTO>(BL.Campaigns.LeadSource.GetLeadSource("Link", (short)LeadSources.HouseCrossSellPQ, pqInputes.SourceId)));
                        CrossSellpq.SponsoredDealer.LeadSource.Add(
                                    Mapper.Map<LeadSource, LeadSourceDTO>(BL.Campaigns.LeadSource.GetLeadSource("Button", (short)LeadSources.HouseCrossSellPQ, pqInputes.SourceId)));
                    }

                    if (CrossSellpq != null)
                    {
                        pqList[0].LinkedSponsoredCar = CrossSellpq.InquiryId;
                        pqList.Add(CrossSellpq);
                    }
                    var carDataTrackingEntity = _priceQuoteBL.GetBasicTrackingObject(pqInputes);
                    carDataTrackingEntity.OnRoadPrice = CrossSellpq.OnRoadPrice;
                    carDataTrackingEntity.CampaignType = (int)CampaignAdType.PaidCrossSell;
                    carDataTrackingEntity.PageId = originalPageId;
                    _trackingBL.TrackPqImpression(carDataTrackingEntity, CrossSellpq.SponsoredDealer, CrossSellpq.PriceQuoteList);
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "PQAdapterAndroidV2.GetPQ()");
                exception.LogException();
            }

            return (List<T>)Convert.ChangeType(pqList, typeof(List<T>));
        }

        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on customer datails
        /// </summary>
        /// <param name="pqInputes">Customer Details</param>
        /// <returns>A  List ot type PQ</returns>
        private PQAndroidV2 GetNewPQ(PQInput pqInputes, string userIdentifier)
        {
            var pqList = new PQAndroidV2();
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

                //getting cardetails based on version id
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
                var cityDetail = _pqGeoLocationBl.GetCityDetailsV2(pqInputes.CityId, CustomParser.parseIntObject(pqInputes.ZoneId), pqInputes.AreaId);

                //This is a hack to handle a case when wrong area is sent along with city
                if (pqInputes.AreaId > 0 && cityDetail.CityId != pqInputes.CityId)
                {
                    pqInputes.AreaId = 0;
                    cityDetail = _pqGeoLocationBl.GetCityDetailsV2(pqInputes.CityId, CustomParser.parseIntObject(pqInputes.ZoneId), pqInputes.AreaId);
                }

                // getting sponsored campaign
                SponsoredDealer dealerSponsoredDetails = new SponsoredDealer();

                var cwOffers = new PQOfferEntity();

                if (pq.PriceQuoteList.Count > 0)
                {
                    if (pqInputes.PageId != SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString() && pqInputes.PageId != SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString())
                    {
                        dealerSponsoredDetails = _campaign.GetSponsorDealerAd(carDetail.ModelId,
                            pqInputes.SourceId,
                            new Location { CityId = cityDetail.CityId, ZoneId = CustomParser.parseIntObject(cityDetail.ZoneId), AreaId = cityDetail.AreaId });

                        if (dealerSponsoredDetails.DealerId > 0)
                        {
                            int templateId = dealerSponsoredDetails.AssignedTemplateId;
                            var template = _tempCache.GetById(templateId);

                            _campaignTemplate.ResolveTemplate(template, ref dealerSponsoredDetails);
                            dealerSponsoredDetails.LeadSource = new List<LeadSource>();
                            dealerSponsoredDetails.LeadSource.Add(Campaigns.LeadSource.GetLeadSource("Link", (short)LeadSources.PQ, pqInputes.SourceId));
                            dealerSponsoredDetails.LeadSource.Add(Campaigns.LeadSource.GetLeadSource("Button", (short)LeadSources.PQ, pqInputes.SourceId));
                        }
                    }
                }

                var AssigneddealerList = _newCarDealersCacheRepo.GetNCSDealers(carDetail.ModelId, cityDetail.CityId).ToList();

                var specialZones = new List<PuneThaneZones>();

                if (cityDetail.CityId == 12 || cityDetail.CityId == 646 || cityDetail.CityId == 647)
                {
                    specialZones = _priceQuoteBL.GetSpecialPuneZones();
                }
                else if (cityDetail.CityId == 40 || cityDetail.CityId == 645 || cityDetail.ZoneId == "16")
                {
                    specialZones = _priceQuoteBL.GetSpecialThaneZones();
                }

                pqList.PriceQuoteList = Mapper.Map<List<PQItem>, List<PQItemDTO>>(pq.PriceQuoteList);

                var carDetails = new CarDetailsDTO();
                carDetails.CarMake = Mapper.Map<CarVersionDetails, CarMakesDTO>(carDetail);
                carDetails.CarModel = Mapper.Map<CarVersionDetails, CarModelsDTO>(carDetail);
                carDetails.CarVersion = Mapper.Map<CarVersionDetails, PQCarVersionDTO>(carDetail);
                carDetails.CarImageBase = Mapper.Map<CarVersionDetails, CarImageBaseDTO>(carDetail);
                carDetails.ModelDetailUrl = GetAPIHostUrl() + "modeldetails/?budget=" + -1 + "&fuelTypes=" + -1 + "&bodyTypes=" + -1 + "&transmission=" + -1
                                            + "&seatingCapacity=" + -1 + "&enginePower=" + -1 + "&importantFeatures=" + -1 + "&modelId=" + carDetail.ModelId;
                carDetails.VersionDetailUrl = GetAPIHostUrl() + "Versiondetails?versionId=" + carDetail.VersionId;
                carDetails.CarName = string.Format("{0} {1} {2}", carDetail.MakeName, carDetail.ModelName, carDetail.VersionName);
                carDetails.CarVersion.SpecsSummary = carDetails.CarVersion.SpecsSummary != null ? carDetails.CarVersion.SpecsSummary.Replace("|", string.Empty) : null;

                pqList.CarDetails = carDetails;
                pqList.CityDetails = Mapper.Map<LocationV2, PQCustLocationDTO>(cityDetail);
                pqList.OtherVersions = Mapper.Map<List<CarVersionEntity>, List<Versions>>(_versionCachedRepo.GetCarVersionsByType("new", carDetail.ModelId));
                pqList.SponsoredDealer = Mapper.Map<SponsoredDealer, SponsoredDealerDTO>(dealerSponsoredDetails);
                pqList.AlternativeCars = _carModelsBL.GetSimilarCarsForApp(carDetail.ModelId, userIdentifier);
                pqList.FormatedonRoadPrice = string.IsNullOrEmpty(pq.OnRoadPrice.ToString()) || pq.OnRoadPrice == 0 ? "" : Utility.Format.Numeric(pq.OnRoadPrice.ToString());
                pqList.OnRoadPrice = pq.OnRoadPrice;
                pqList.Offers = Mapper.Map<PQOfferEntity, PQOffersDTO>(cwOffers);
                pqList.DealerList = Mapper.Map<List<NewCarDealersList>, List<DealerDTO>>(AssigneddealerList);
                pqList.SpecialZones = Mapper.Map<List<PuneThaneZones>, List<PuneThaneZonesDTO>>(specialZones);
                pqList.IsSponsoredCar = (pqInputes.PageId == SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString()
                                            || pqInputes.PageId == SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString());
                pqList.OfferV1 = GetOffer(carDetail, cityDetail);
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "PQAdapterAndroidV2.GetNewPQ()");
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
                Logger.LogException(ex, "PQAdapterAndroidV2.GetOffer() : modelId : " + carDetails.ModelId + " VersionId : " + carDetails.VersionId + " City : " + locationDetails.CityId);
            }
            return offer;
        }

        private static string GetAPIHostUrl()
        {
            return ConfigurationManager.AppSettings["WebApiHostUrl"];
        }

        public List<T> GetPQByIds<T>(List<ulong> pqIdList) where T : new()
        {
            throw new NotImplementedException();
        }

        public List<PQOnRoadPrice> GetPQ(int cityId, int versionId)
        {
            throw new NotImplementedException();
        }

        public SponsoredDealer GetSponsorDealerAd(int modelId, int cityId, string zoneId)
        {
            throw new NotImplementedException();
        }


        public List<T> GetPQByIds<T>(List<string> pqIdList, string userIdentifier) where T : new()
        {
            throw new NotImplementedException();
        }
    }
}

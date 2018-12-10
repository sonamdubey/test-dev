using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CrossSell;
using Carwale.Entity.Dealers;
using Carwale.Entity.Deals;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Insurance;
using Carwale.Entity.OffersV1;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CrossSell;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Template;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace Carwale.BL.PriceQuote
{
    public class PQAdapterDesktopV1 : IPQAdapter
    {
        private readonly IPQRepository _pqRepo;
        private readonly IDealerSponsoredAdRespository _dealerRepo;
        private readonly IPQCacheRepository _pqCachedRepo;
        private readonly ICarVersionCacheRepository _versionCachedRepo;
        private readonly IGeoCitiesCacheRepository _cityCachedRepo;
        private readonly ICarModels _carModelsBL;
        private readonly INewCarDealers _newCarDealersRepo;
        private readonly IPriceQuoteBL _priceQuoteBL;
        private readonly IDeals _carDeals;
        private readonly IPaidCrossSell _paidCrossSell;
        private readonly IHouseCrossSell _houseCrossSell;
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly IPQGeoLocationBL _cityBL;
        private readonly IPrices _prices;
        private readonly ICampaign _campaign;
        private readonly ICacheManager _cacheCore;
        private readonly ICarPriceQuoteAdapter _priceQuote;
        private readonly ICustomerTracking _trackingBL;
        private readonly ITemplate _campaignTemplate;
        private readonly IOffersAdapter _offersAdapter;
        public string dealerCookie = "";

        public PQAdapterDesktopV1(IUnityContainer container, IPQRepository pqRepo, IDealerSponsoredAdRespository dealerRepo,
            IPQCacheRepository pqCachedRepo, ICarVersionCacheRepository versionCachedRepo, IGeoCitiesCacheRepository cityCachedRepo,
            ICarModels carModelsBL, INewCarDealers newCarDealersRepo,
            IPriceQuoteBL priceQuoteBL, IDeals carDeals, ITemplatesCacheRepository tempCache,
            IPQGeoLocationBL cityBL, IPrices prices, ICampaign campaign, ICacheManager cacheCore,
            ICarPriceQuoteAdapter priceQuote, ITemplate campaignTemplate, IPaidCrossSell paidCrossSell, IHouseCrossSell houseCrossSell, ICustomerTracking trackingBL, IOffersAdapter offersAdapter)
        {
            _pqRepo = pqRepo;
            _pqCachedRepo = pqCachedRepo;
            _dealerRepo = dealerRepo;
            _versionCachedRepo = versionCachedRepo;
            _cityCachedRepo = cityCachedRepo;
            _carModelsBL = carModelsBL;
            _newCarDealersRepo = newCarDealersRepo;
            _priceQuoteBL = priceQuoteBL;
            _carDeals = carDeals;
            _tempCache = tempCache;
            _cityBL = cityBL;
            _campaign = campaign;
            _prices = prices;
            _cacheCore = cacheCore;
            _priceQuote = priceQuote;
            _campaignTemplate = campaignTemplate;
            _paidCrossSell = paidCrossSell;
            _houseCrossSell = houseCrossSell;
            _trackingBL = trackingBL;
            _offersAdapter = offersAdapter;
        }

        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on multiple pqids
        /// </summary>
        /// <param name="pqIds">comma separated pqIds(Inquiry Ids)</param>
        /// <returns>A  List ot type PQ</returns>
        /// WrittrnBy : ASHISH Verma on 15/06/2014
        public List<T> GetPQByIds<T>(List<string> enIdList, string userIdentifier) where T : new()
        {
            var priceQuotes = new List<PQDesktop>();
            var pq = new PQ();
            CarPriceQuoteDTO carPriceQuote = null;

            try
            {
                foreach (var enId in enIdList)
                {
                    pq = _priceQuoteBL.GetPQDetails(enId);

                    if (pq.CityId > 0 && pq.VersionId > 0)
                    {
                        var pqCarDetails = Mapper.Map<CarVersionDetails, PQCarDetails>(_versionCachedRepo.GetVersionDetailsById(pq.VersionId));

                        var inputCityDetails = _cityBL.GetCityDetailsV2(pq.CityId, CustomParser.parseIntObject(pq.ZoneId), pq.AreaId);

                        if (inputCityDetails == null)
                        {
                            continue;
                        }

                        var cityDetail = Mapper.Map<LocationV2, CustLocation>(inputCityDetails);

                        carPriceQuote = _priceQuote.GetModelPriceQuote(pqCarDetails.ModelId, pq.CityId, true, false, true);
                        if (carPriceQuote != null && carPriceQuote.VersionPricesList != null)
                        {
                            var currentVersions = carPriceQuote.VersionPricesList.Where(x => x.VersionId == pq.VersionId);
                            pq.PriceQuoteList = _prices.MapFromVersionPriceQuoteDTO(currentVersions, (int)Platform.CarwaleDesktop);
                        }
                        else
                        {
                            pq.PriceQuoteList = new List<PQItem>();
                        }

                        pq.ModelId = pqCarDetails.ModelId;
                        pq.MakeId = pqCarDetails.MakeId;

                        var dealerShowroomDetails = new DealerShowroomDetails();
                        var dealerSponsoredDetails = new SponsoredDealer();
                        var insuranceDiscount = new InsuranceDiscount();
                        List<CrossSellCampaign> crossSellCampaign = new List<CrossSellCampaign>();
                        DiscountSummary discountSummary = new DiscountSummary();

                        if (pq.PriceQuoteList.Count > 0)
                        {
                            //poco SponsoredDealer // display ad on basis of cookies
                            dealerSponsoredDetails = _campaign.GetSponsorDealerAd(pq.ModelId, (int)Platform.CarwaleDesktop, new Location
                            {
                                CityId = pq.CityId,
                                ZoneId = CustomParser.parseIntObject(pq.ZoneId),
                                AreaId = pq.AreaId
                            });

                            if (dealerSponsoredDetails.DealerId != 0)
                            {
                                int templateId = _campaignTemplate.GetCampaignGroupTemplateId(dealerSponsoredDetails.AssignedTemplateId,
                                                                            dealerSponsoredDetails.AssignedGroupId, (int)Platform.CarwaleDesktop);
                                dealerSponsoredDetails.MakeName = pqCarDetails.MakeName;
                                var template = _tempCache.GetById(templateId);
                                dealerSponsoredDetails.TemplateName = template.UniqueName;
                                dealerSponsoredDetails.TemplateHtml = template.Html;
                                dealerSponsoredDetails.TemplateHtml = _campaignTemplate.GetRendredContent(dealerSponsoredDetails.TemplateName,
                                    dealerSponsoredDetails.TemplateHtml, dealerSponsoredDetails);
                                if (dealerSponsoredDetails.DealerLeadBusinessType == DealerShowroomBusinessTypeEnum.LeadBusinessType.GetHashCode())
                                {
                                    DealerShowroomDetails dealerDetails = _newCarDealersRepo.GetDealerDetails(dealerSponsoredDetails.ActualDealerId,
                                                                                                                dealerSponsoredDetails.DealerId, pq.CityId, pq.MakeId);

                                    if (!string.IsNullOrEmpty(dealerDetails.objDealerDetails.Name))
                                        dealerShowroomDetails = dealerDetails;
                                }
                            }
                            else
                            {
                                discountSummary = _carDeals.IsShowDeals(cityDetail.CityId, false) ? _carDeals.GetDealsSummaryByModelandCity(pqCarDetails.ModelId, cityDetail.CityId) : null;
                                crossSellCampaign = GetCrossSellCampaigns(discountSummary,
                                        new Location { CityId = cityDetail.CityId, ZoneId = CustomParser.parseIntObject(cityDetail.ZoneId), AreaId = cityDetail.AreaId }, pqCarDetails.VersionId);
                            }
                        }

                        var pqDesktopObj = new PQDesktop
                        {
                            PriceQuoteList = pq.PriceQuoteList,
                            carDetails = pqCarDetails,
                            cityDetail = cityDetail,
                            IsSponsoredCar = pq.PageId == SponsoredCarPageId.PQSponsorCar.GetHashCode(),
                            CarVersions = _priceQuoteBL.GetCarVersionDetails(pq.ModelId, cityDetail.CityId),
                            PQCities = Mapper.Map<List<City>, List<PriceQuoteCityDTO>>(_cityCachedRepo.GetPQCitiesByModelId(pq.ModelId)),
                            PQZones = Mapper.Map<List<CityZones>, List<CityZonesDTO>>(_cityBL.GetPQCityZonesList(pq.ModelId)),
                            SponsoredDealer = dealerSponsoredDetails,
                            InsuranceDiscount = insuranceDiscount,
                            Offers = null,
                            DealerShowroom = dealerShowroomDetails,
                            CrossSellCampaignList = crossSellCampaign,
                            discountSummary = discountSummary,
                            EnId = enId,
                            ShowSellCarLink = PresentationLogic.NewCar.ShowSellCarLinkOnPq(cityDetail.CityId)
                        };
                        GetOffer(pqDesktopObj);
                        pqDesktopObj.AlternateCars = _carModelsBL.GetSimilarCarsWithPriceOverview(pq.ModelId, pq.CityId, userIdentifier);
                        pqDesktopObj.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(GetCampaignTemplatesInput(pq.CityId, pqCarDetails.ModelId, pq.MakeId), pqDesktopObj);
                        priceQuotes.Add(pqDesktopObj);
                    }
                    else
                    {
                        priceQuotes = null;
                    }
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return null;
            }

            return (List<T>)Convert.ChangeType(priceQuotes, typeof(List<T>));
        }

        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on customer datails
        /// </summary>
        /// <param name="pqInputes">Customer Details</param>
        /// <returns>A  List ot type PQ</returns>
        /// /// WrittrnBy : Ashish Verma on 15/06/2014
        public List<T> GetPQ<T>(PQInput pqInputes, string userIdentifier)
        {
            var pqList = new List<PQDesktop>();
            var pq = GetNewPQ(pqInputes, userIdentifier);

            if (pq != null)
            {
                pqList.Add(pq);
            }

            var mainCarDataTrackingEntity = _priceQuoteBL.GetBasicTrackingObject(pqInputes);
            mainCarDataTrackingEntity.OnRoadPrice = _priceQuoteBL.CalculateOnRoadPrice(pq.PriceQuoteList);
            mainCarDataTrackingEntity.CampaignType = (int)CampaignAdType.Pq;
            if (pq.CrossSellCampaignList != null && pq.CrossSellCampaignList.Count > 0)
            {
                mainCarDataTrackingEntity.CampaignType = (int)CampaignAdType.PaidCrossSell;
            }
            mainCarDataTrackingEntity.PageId = pqInputes != null ? CustomParser.parseIntObject(pqInputes.PageId) : 0;
            _trackingBL.TrackPqImpression(mainCarDataTrackingEntity, pq.SponsoredDealer, pq.PriceQuoteList);

            return (List<T>)Convert.ChangeType(pqList, typeof(List<T>));
        }

        private PQDesktop GetNewPQ(PQInput pqInputes, string userIdentifier)
        {
            var priceQuote = new PQDesktop();
            priceQuote.InsuranceDiscount = new InsuranceDiscount();
            Mapper.CreateMap<CarVersionDetails, PQCarDetails>()
                .ForMember(x => x.LargePic, o => o.MapFrom(s => s.ModelImageLarge))
                .ForMember(x => x.SmallPic, o => o.MapFrom(s => s.ModelImageSmall))
                .ForMember(x => x.XtraLargePic, o => o.MapFrom(s => s.ModelImageXtraLarge));
            CarPriceQuoteDTO carPriceQuote = null;
            try
            {
                pqInputes.ClientIp = UserTracker.GetUserIp();

                if (pqInputes.CarVersionId < 1)
                    pqInputes.CarVersionId = _versionCachedRepo.GetDefaultVersionId(pqInputes.CityId, pqInputes.CarModelId);

                // get details about car
                var pqCarDetails = Mapper.Map<CarVersionDetails, PQCarDetails>(_versionCachedRepo.GetVersionDetailsById(pqInputes.CarVersionId));
                pqCarDetails.VersionId = pqInputes.CarVersionId;

                // get prices of every fields
                var pq = new PQ();
                carPriceQuote = _priceQuote.GetModelPriceQuote(pqCarDetails.ModelId, pqInputes.CityId, true, false, true);
                if (carPriceQuote != null && carPriceQuote.VersionPricesList != null)
                {
                    pq.PriceQuoteList = _prices.MapFromVersionPriceQuoteDTO(carPriceQuote.VersionPricesList.Where(x => x.VersionId == pqInputes.CarVersionId), (int)Platform.CarwaleDesktop);
                }
                else
                {
                    pq.PriceQuoteList = new List<PQItem>();
                }

                if (pq.PriceQuoteList.Count == 0
                    && pqInputes.PageId == SponsoredCarPageId.PQSponsorCar.GetHashCode().ToString())
                { return null; } // check for sponsored car price

                CustLocation cityDetail = null;
                var inputCityDetail = _cityBL.GetCityDetailsV2(pqInputes.CityId, CustomParser.parseIntObject(pqInputes.ZoneId), pqInputes.AreaId);

                if (inputCityDetail == null)
                {
                    return null;
                }

                cityDetail = Mapper.Map<LocationV2, CustLocation>(inputCityDetail);

                // create json object
                priceQuote.PriceQuoteList = pq.PriceQuoteList;
                priceQuote.carDetails = pqCarDetails;
                priceQuote.cityDetail = cityDetail;
                priceQuote.CarVersions = _priceQuoteBL.GetCarVersionDetails(pqCarDetails.ModelId, cityDetail.CityId);
                priceQuote.PQCities = Mapper.Map<List<City>, List<PriceQuoteCityDTO>>(_cityCachedRepo.GetPQCitiesByModelId(pqCarDetails.ModelId));
                priceQuote.PQZones = Mapper.Map<List<CityZones>, List<CityZonesDTO>>(_cityBL.GetPQCityZonesList(pqCarDetails.ModelId));
                priceQuote.AlternateCars = _carModelsBL.GetSimilarCarsWithPriceOverview(pqCarDetails.ModelId, pqInputes.CityId, userIdentifier);
                priceQuote.IsSponsoredCar = pqInputes.PageId == SponsoredCarPageId.PQSponsorCar.GetHashCode().ToString();
                priceQuote.EnId = HttpUtility.UrlEncode(CustomTripleDES.EncryptTripleDES(pqCarDetails.VersionId + "~" + cityDetail.CityId + "~" +
                    cityDetail.ZoneId + "~" + pqInputes.PageId + "~" + cityDetail.AreaId));
                GetOffer(priceQuote);

                if (pq.PriceQuoteList.Count > 0 &&
                    pqInputes.PageId != SponsoredCarPageId.PQNearByCity.GetHashCode().ToString())
                // if onRoad price is available then show Dealer Ad, Dealer Showroom, Insurance Link and Offers.
                {
                    priceQuote.Offers = null;
                    // get sponsore dealer Ad campaign
                    var dealerSponsoredDetails = _campaign.GetSponsorDealerAd(pqCarDetails.ModelId,
                        (int)Platform.CarwaleDesktop, new Location { CityId = cityDetail.CityId, ZoneId = CustomParser.parseIntObject(cityDetail.ZoneId), AreaId = cityDetail.AreaId });

                    // poco SponsoredDealer (assigning parsed html to Poco)
                    DealerShowroomDetails dealerShowroomDetails = new DealerShowroomDetails();
                    if (dealerSponsoredDetails.DealerId != 0)
                    {
                        int templateId = _campaignTemplate.GetCampaignGroupTemplateId(dealerSponsoredDetails.AssignedTemplateId, dealerSponsoredDetails.AssignedGroupId, (int)Platform.CarwaleDesktop);

                        dealerSponsoredDetails.MakeName = pqCarDetails.MakeName;
                        var template = _tempCache.GetById(templateId);
                        dealerSponsoredDetails.TemplateName = template.UniqueName;
                        dealerSponsoredDetails.TemplateHtml = template.Html;
                        dealerSponsoredDetails.TemplateHtml = _campaignTemplate.GetRendredContent(dealerSponsoredDetails.TemplateName,
                                                                                                    dealerSponsoredDetails.TemplateHtml,
                                                                                                    dealerSponsoredDetails);
                        if (dealerSponsoredDetails.DealerLeadBusinessType == DealerShowroomBusinessTypeEnum.LeadBusinessType.GetHashCode())
                        {
                            DealerShowroomDetails dealerDetails = _newCarDealersRepo.GetDealerDetails(dealerSponsoredDetails.ActualDealerId, dealerSponsoredDetails.DealerId,
                                pqInputes.CityId, pqCarDetails.MakeId);

                            if (!string.IsNullOrEmpty(dealerDetails.objDealerDetails.Name))
                            {
                                dealerShowroomDetails = dealerDetails;
                            }
                        }
                    }
                    else
                    {
                        priceQuote.discountSummary = _carDeals.IsShowDeals(cityDetail.CityId, false) ? _carDeals.GetDealsSummaryByModelandCity(pqCarDetails.ModelId, cityDetail.CityId) : null;
                        priceQuote.CrossSellCampaignList = GetCrossSellCampaigns(priceQuote.discountSummary,
                            new Location { CityId = cityDetail.CityId, ZoneId = CustomParser.parseIntObject(cityDetail.ZoneId), AreaId = cityDetail.AreaId }, pqInputes.CarVersionId);
                    }

                    // get Dealer Showroom Details
                    priceQuote.DealerShowroom = dealerShowroomDetails;
                    // get Sponsor Dealer Ad
                    priceQuote.SponsoredDealer = dealerSponsoredDetails;

                    priceQuote.ShowSellCarLink = PresentationLogic.NewCar.ShowSellCarLinkOnPq(cityDetail.CityId);
                }
                priceQuote.CampaignTemplates = _campaignTemplate.GetTemplatesByPage(GetCampaignTemplatesInput(pqInputes.CityId, pqInputes.CarModelId, pqCarDetails.MakeId), priceQuote);
                SetCwcCookie(pqCarDetails.ModelId);
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            return priceQuote;
        }

        /// <summary>
        /// For Getting Client Info object
        /// </summary>
        /// <param name="pqInputes">Customer Details and PQid</param>
        /// <returns>Client info object of type PQUserInfoTrackEntity</returns>
        /// /// WrittrnBy : Ashish Verma on 19/09/2014
        private PQUserInfoTrackEntity GetClientInfo(PQInput pqInputes)
        {
            var userTrackingObj = new PQUserInfoTrackEntity()
            {
                ClientIp = pqInputes.ClientIp,
                AspSessoinId = UserTracker.GetAspSessionCookie(),
                CWCookievalue = UserTracker.GetSessionCookie(),
                EntryDate = DateTime.Now.ToString()
            };
            return userTrackingObj;
        }

        /// <summary>
        /// For Getting PriceQuote For Sponsored car based on city id and version id (this function not inserts new entry into NewcarPurchageInquries Table)
        /// Written By : Ashish Verma on 29/9/2014
        /// </summary>
        /// <param name="PQId">cityid,versionid,zoneid</param>
        /// <returns>Price Quote For Sponsored Car </returns>
        public List<PQOnRoadPrice> GetPQ(int cityId, int versionId)
        {
            var pqList = new List<PQOnRoadPrice>();

            var pq = _prices.FilterPrices(cityId, versionId);

            pqList.Add(new PQOnRoadPrice()
            {
                PriceQuoteList = pq.PriceQuoteList,
                OnRoadPrice = pq.OnRoadPrice
            });

            return pqList;
        }

        public int FetchDealerOnCookie(int modelId, int cityId, string zoneId)
        {
            try
            {
                string currModelId = modelId.ToString();
                string currCityId = cityId.ToString();
                string currZoneId = zoneId;
                string temp = dealerCookie;
                var arrOfDealerInfo = temp.Split('!');
                var len = arrOfDealerInfo.Length;
                for (int i = 0; i < len - 1; i++)
                {
                    var cookieModel = arrOfDealerInfo[i].Split('~')[2];
                    var cookieCity = arrOfDealerInfo[i].Split('~')[1];
                    var cookieZone = arrOfDealerInfo[i].Split('~')[3];
                    if (currCityId == cookieCity && currModelId == cookieModel && currZoneId == cookieZone)
                    {
                        return CustomParser.parseIntObject(arrOfDealerInfo[i].Split('~')[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "PQAdapterDesktop.FetchDealerOnCookie()");
                objErr.SendMail();
            }
            return 0;
        }

        /// <summary>
        /// Author:Chetan Thambad on <05/05/2016>
        /// Desc: Get final of list CrossSell DTO to be bind on UI 
        /// </summary>
        private List<CrossSellCampaign> getCrossSellDTO(List<CrossSellDetail> crossSellCampaigns, int cityId, int adType)
        {
            if (crossSellCampaigns != null && crossSellCampaigns.Count > 0)
            {
                var crossSellCampaignList = Mapper.Map<List<CrossSellDetail>, List<CrossSellCampaign>>(crossSellCampaigns);
                foreach (var crossSell in crossSellCampaignList)
                {
                    var price = _prices.FilterPrices(cityId, crossSell.VersionId);
                    crossSell.OnRoadPrice = price.OnRoadPrice;
                    crossSell.LeadSource = new List<LeadSource>();
                    crossSell.LeadSource.Add(BL.Campaigns.LeadSource.GetLeadSource("Button", adType, (short)Platform.CarwaleDesktop));
                    crossSell.LeadSource.Add(BL.Campaigns.LeadSource.GetLeadSource("Recommendation", adType, (short)Platform.CarwaleDesktop));
                }
                return crossSellCampaignList;
            }
            return null;
        }

        /// <summary>
        /// Author:Chetan Thambad on <05/05/2016>
        /// Desc: Get the list of Paid or House CrossSell campaigns
        /// </summary>
        private List<CrossSellCampaign> GetCrossSellCampaigns(DiscountSummary discountSummary, Location loc, int versionId)
        {
            List<CrossSellCampaign> crossSellCampaign = null;

            var paidCrossSellCampaigns = _paidCrossSell.GetPaidCrossSellList(versionId, loc);

            if (paidCrossSellCampaigns != null)
                crossSellCampaign = getCrossSellDTO(paidCrossSellCampaigns, loc.CityId, (int)LeadSources.PaidCrossSellPQ);

            if (discountSummary == null && paidCrossSellCampaigns == null)
            {
                var houseCrossSellCampaigns = _houseCrossSell.GetHouseCrossSellList(versionId, loc, (int)Platform.CarwaleDesktop);
                if (houseCrossSellCampaigns != null)
                    crossSellCampaign = getCrossSellDTO(houseCrossSellCampaigns, loc.CityId, (int)LeadSources.HouseCrossSellPQ);
            }
            return crossSellCampaign;
        }

        private void SetCwcCookie(int modelId)
        {
            try
            {
                string cwcCookie = string.Empty;
                var cwcCookieVal = new List<int>();

                if (HttpContext.Current.Request.Cookies["CWC"] != null && HttpContext.Current.Request.Cookies["CWC"].Values.ToString() != "")
                    cwcCookie = WebUtility.UrlDecode(HttpContext.Current.Request.Cookies["CWC"].Values.ToString());
                else
                    return;

                var userModelHistory = _cacheCore.GetFromCache<List<int>>(string.Format("cwc-cookie-{0}", cwcCookie));
                _cacheCore.ExpireCache(string.Format("cwc-cookie-{0}", cwcCookie));

                if (userModelHistory != null && userModelHistory.Count != 0)
                    cwcCookieVal.AddRange(userModelHistory);

                cwcCookieVal.Add(modelId);
                _pqCachedRepo.StoreUserPQTakenModels(string.Format("cwc-cookie-{0}", cwcCookie), cwcCookieVal.Distinct().ToList());
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, "PQAdapterDesktop SetCwcCookie()");
                exception.LogException();
            }
        }

        public List<T> GetPQByIds<T>(List<ulong> pqIdList) where T : new()
        {
            throw new NotImplementedException();
        }

        private CampaignInputv2 GetCampaignTemplatesInput(int cityId, int modelId, int makeId)
        {
            return new CampaignInputv2
            {
                ModelId = modelId,
                PlatformId = (short)Platform.CarwaleDesktop,
                ApplicationId = (short)Application.CarWale,
                CityId = cityId,
                PageId = (short)CwPages.QuotationDesktop,
                MakeId = makeId
            };
        }

        private void GetOffer(PQDesktop priceQuoteDesktop)
        {
            priceQuoteDesktop.OfferAndDealerAd = new OfferAndDealerAdDTO();
            if (priceQuoteDesktop.carDetails != null && priceQuoteDesktop.cityDetail != null && priceQuoteDesktop.PriceQuoteList.Count > 0)
            {
                priceQuoteDesktop.OfferAndDealerAd.CarDetails = Mapper.Map<CarDetailsDTO>(priceQuoteDesktop.carDetails);
                SetOffers(priceQuoteDesktop);
            }
        }

        private void SetOffers(PQDesktop priceQuoteDesktop)
        {
            try
            {
                var offerInput = Mapper.Map<PQCarDetails, OfferInput>(priceQuoteDesktop.carDetails);
                offerInput.CityId = priceQuoteDesktop.cityDetail.CityId;
                priceQuoteDesktop.OfferAndDealerAd.Offer = _offersAdapter.GetOffers(offerInput);
            }
            catch (Exception ex)
            {
                var exception = new ExceptionHandler(ex, "PQAdapterDesktop SetOffers()");
                exception.LogException();
            }
        }
	}// class
}

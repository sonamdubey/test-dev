using System;
using System.Collections.Generic;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Geolocation;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.CarData;
using System.Configuration;
using Carwale.Entity.Enum;
using Carwale.Utility;
using Carwale.DAL.PriceQuote;
using Carwale.Cache.PriceQuote;
using Carwale.DAL.CarData;
using Carwale.Cache.CarData;
using Carwale.DAL.Dealers;
using Carwale.Cache.Dealers;
using AEPLCore.Cache;
using Carwale.Cache.Geolocation;
using Carwale.DAL.Geolocation;
using Microsoft.Practices.Unity;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Offers;
using System.Linq;
using Carwale.Interfaces.SponsoredCar;
using Carwale.DAL.SponsoredCar;
using Carwale.Interfaces.Campaigns;
using Carwale.BL.GeoLocation;
using Carwale.BL.Campaigns;
using Carwale.DAL.Campaigns;
using Carwale.Cache.Campaigns;
using Carwale.Interfaces.Template;
using Carwale.Cache.Template;
using Carwale.DAL.Template;
using AutoMapper;
using Carwale.DTOs.Geolocation;
using Carwale.BL.CarData;
using AEPLCore.Cache.Interfaces;

namespace Carwale.BL.PriceQuote
{
    public class PQAdapterAndroid : IPQAdapter
    {
        private readonly IPQRepository _pqRepo;
        private readonly IPQCacheRepository _pqCachedRepo;
        private readonly ICarVersionCacheRepository _versionCachedRepo;
        private readonly ICarModelCacheRepository _carModelsCachedRepo;
        private readonly IDealerSponsoredAdRespository _dealerRepo;
        private readonly IDealerSponsoredAdCache _dealerCachedRepo;
        private readonly ISponsoredCar _sponsoredcarRepo;
        private readonly IPQGeoLocationBL _cityRepo;
        private readonly IGeoCitiesCacheRepository _cityCachedRepo;
        private readonly IPriceQuoteBL _priceQuoteBL;
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly IPrices _prices;
        private readonly ICampaign _campaign;
        private readonly ICarModels _carModelsBL;
        private readonly INewCarDealersCache _newCarDealersCacheRepo;
        private readonly ITemplate _campaignTemplate;

        public PQAdapterAndroid(IUnityContainer container)
        {
            container.RegisterType<IPQRepository, PQRepository>()
                .RegisterType<IPQCacheRepository, PQCacheRepository>()

                .RegisterType<ICarVersionRepository, CarVersionsRepository>()
                .RegisterType<ICarVersionCacheRepository, CarVersionsCacheRepository>()

                .RegisterType<ICampaign, CampaignBL>()
                .RegisterType<ITemplate, Carwale.BL.Campaigns.Template>()
                .RegisterType<ICampaignRepository, CampaignRepository>()
                .RegisterType<ICampaignCacheRepository, CampaignCacheRepository>()
                .RegisterType<ITemplatesCacheRepository, TemplatesCacheRepository>()
                .RegisterType<ITemplatesRepository, TemplatesRepository>()

                .RegisterType<IDealerSponsoredAdRespository, DealerSponsoredAdRespository>()
                .RegisterType<IDealerSponsoredAdCache, DealerSponsoredAdCache>()
                .RegisterType<ISponsoredCar, SponsoredCarRepository>()
                .RegisterType<ICacheManager, CacheManager>()

                .RegisterType<IGeoCitiesRepository, GeoCitiesRepository>()
                .RegisterType<IGeoCitiesCacheRepository, GeoCitiesCacheRepository>()
                .RegisterType<IPQGeoLocationBL, PQGeoLocationBL>()
                .RegisterType<ICarModelRepository, CarModelsRepository>()
                .RegisterType<ICarModelCacheRepository, CarModelsCacheRepository>()
                .RegisterType<IPriceQuoteBL, PriceQuoteBL>()
                .RegisterType<ICarModels, CarModelsBL>();

            _pqRepo = container.Resolve<IPQRepository>();
            _pqCachedRepo = container.Resolve<IPQCacheRepository>();

            _dealerRepo = container.Resolve<IDealerSponsoredAdRespository>();
            _dealerCachedRepo = container.Resolve<IDealerSponsoredAdCache>();

            _versionCachedRepo = container.Resolve<ICarVersionCacheRepository>();

            _cityCachedRepo = container.Resolve<IGeoCitiesCacheRepository>();

            _carModelsCachedRepo = container.Resolve<ICarModelCacheRepository>();

            _cityRepo = container.Resolve<IPQGeoLocationBL>();

            _priceQuoteBL = container.Resolve<IPriceQuoteBL>();
            _sponsoredcarRepo = container.Resolve<ISponsoredCar>();
            _tempCache = container.Resolve<ITemplatesCacheRepository>();
            _campaign = container.Resolve<ICampaign>();
            _prices = container.Resolve<IPrices>();
            _carModelsBL = container.Resolve<ICarModels>();
            _newCarDealersCacheRepo = container.Resolve<INewCarDealersCache>();
            _campaignTemplate = container.Resolve<ITemplate>();
        }


        /// <summary>
        /// For getting PriceQuote,All CarVersions,Dealers deatails And All carDetails based on customer datails
        /// </summary>
        /// <param name="pqInputes">Customer Details</param>
        /// <returns>A  List ot type PQ</returns>
        public List<T> GetPQ<T>(PQInput pqInputes, string userIdentifier)
        {
            var pqList = new List<PQAndroid>();
            var pq = GetNewPQ(pqInputes, userIdentifier);
            pqList.Add(pq);
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
            //getting and assigning User Ip
            pqInputes.ClientIp = UserTracker.GetUserIp();
            //saving lead to newcarpurchageinquiries and newpurchagecities table

            if (pqInputes.CarVersionId < 1)
                pqInputes.CarVersionId = _versionCachedRepo.GetDefaultVersionId(pqInputes.CityId, pqInputes.CarModelId);

            //getting price quote based on city and version id
            pq = _prices.FilterPrices(pqInputes.CityId, pqInputes.CarVersionId);

            if (pq.OnRoadPrice <= 0
                            && pqInputes.PageId == SponsoredCarPageId.PQSponsorCar.GetHashCode().ToString())
            { return null; } // check for sponsored car price

            //getting cardetais based on version id
            var carDetail = _versionCachedRepo.GetVersionDetailsById(pqInputes.CarVersionId);
            //getting cust citydetails based on cityid and zone id
            var cityDetail = _cityCachedRepo.GetCustLocation(pqInputes.CityId, pqInputes.ZoneId);
            // getting sponsored campaign
            SponsoredDealer dealerSponsoredDetails = new SponsoredDealer();
            var cwOffers = new PQOfferEntity();

            if (pq.OnRoadPrice > 0)
            {
                dealerSponsoredDetails = _campaign.GetSponsorDealerAd(carDetail.ModelId, pqInputes.SourceId,
                    new Location { CityId = cityDetail.CityId, ZoneId = CustomParser.parseIntObject(cityDetail.ZoneId) });
                dealerSponsoredDetails.TemplateHeight = Convert.ToInt32(ConfigurationManager.AppSettings["TemplateHeight"]);
                if (dealerSponsoredDetails.DealerId != 0)
                {
                    int templateId = dealerSponsoredDetails.AssignedTemplateId;
                    var template = _tempCache.GetById(templateId);
                    dealerSponsoredDetails.TemplateName = template.UniqueName;
                    dealerSponsoredDetails.TemplateHtml = template.Html;
                    dealerSponsoredDetails.TemplateHtml = _campaignTemplate.GetRendredContent(dealerSponsoredDetails.TemplateName, dealerSponsoredDetails.TemplateHtml, dealerSponsoredDetails);
                }
                //offers
                cwOffers = new PQOfferEntity();
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
            pqList.carName = carDetail.MakeName + " " + carDetail.ModelName + " " + carDetail.VersionName;
            pqList.reviewRate = carDetail.ReviewRate;
            pqList.specsSummery = string.IsNullOrWhiteSpace(carDetail.SpecSummery) ? "" : carDetail.SpecSummery.Replace("|", "");
            pqList.cityId = cityDetail.CityId;
            pqList.ZoneId = cityDetail.ZoneId;
            pqList.cityName = cityDetail.CityName;
            pqList.zoneName = cityDetail.ZoneName;
            pqList.sponsoredDealer = dealerSponsoredDetails;
            pqList.otherVersions = _versionCachedRepo.GetCarVersionsByType("new", carDetail.ModelId);
            pqList.otherCities = Mapper.Map<List<Carwale.Entity.Geolocation.City>, List<PriceQuoteCityDTO>>(_cityCachedRepo.GetPQCitiesByModelId(carDetail.ModelId));
            pqList.alternativeCars = Mapper.Map<List<SimilarCarModelsDTO>, List<SimilarCarsAndroidDTO>>(_carModelsBL.GetSimilarCarsForApp(carDetail.ModelId, userIdentifier));
            pqList.modelDetailUrl = GetAPIHostUrl() + "modeldetails/?budget=" + -1 + "&fuelTypes=" + -1 + "&bodyTypes=" + -1 + "&transmission=" + -1 + "&seatingCapacity=" + -1 + "&enginePower=" + -1 + "&importantFeatures=" + -1 + "&modelId=" + carDetail.ModelId;
            pqList.versionDetailUrl = GetAPIHostUrl() + "Versiondetails?versionId=" + carDetail.VersionId;
            pqList.otherCityUrl = GetAPIHostUrl() + "Pricequote?versionId=" + carDetail.VersionId + "&preferenceId=" + 10000 + "&name=" + pqInputes.Name + "&emailId=" + pqInputes.Email + "&mobileNo=" + pqInputes.Mobile;
            pqList.formatedonRoadPrice = string.IsNullOrEmpty(pq.OnRoadPrice.ToString()) || pq.OnRoadPrice == 0 ? "" : Utility.Format.Numeric(pq.OnRoadPrice.ToString());
            pqList.onRoadPrice = pq.OnRoadPrice;
            pqList.offers = cwOffers;
            pqList.dealerList = AssigneddealerList;
            pqList.SpecialZones = specialZones;
            pqList.Zones = Mapper.Map<List<CityZones>, List<Carwale.DTOs.Geolocation.CityZonesDTO>>(_cityRepo.GetPQCityZonesList(carDetail.ModelId));

            return pqList;
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

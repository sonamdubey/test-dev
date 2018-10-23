using Bikewale.Entities;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Device.Location;
using Microsoft.Practices.Unity;
using Bikewale.Models.PriceInCity;

namespace Bikewale.Cache.PriceQuote
{
    /// Created By : Vivek Gupta on 20-05-2016
    /// Modified By : Monika Korrapati on 27 Sept 2018
    /// Description : DealerPriceQuoteRepository of DAL layer resolving instead of BAL.
    public class PriceQuoteCache : IPriceQuoteCache
    {
        private readonly ICacheManager _cache = null;
        private readonly IPriceQuote _obPriceQuote = null;
        private readonly Bikewale.Interfaces.BikeBooking.IDealerPriceQuote _dealerPriceQuoteRepository ;
        private readonly Bikewale.Interfaces.AutoBiz.IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IPriceQuoteCacheHelper _priceQuoteCacheHelper;
        private readonly static IUnityContainer _container;

        static PriceQuoteCache()
        {
            _container = new UnityContainer();
            _container.RegisterType<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote, Bikewale.DAL.BikeBooking.DealerPriceQuoteRepository>();
        }

        public PriceQuoteCache(ICacheManager cache, IPriceQuote obPriceQuote,  Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objDealerPriceQuote, IPriceQuoteCacheHelper priceQuoteCacheHelper)
        {
            _cache = cache;
            _obPriceQuote = obPriceQuote;
            _objDealerPriceQuote = objDealerPriceQuote;
            _priceQuoteCacheHelper = priceQuoteCacheHelper;
            _dealerPriceQuoteRepository = _container.Resolve<Bikewale.Interfaces.BikeBooking.IDealerPriceQuote>();
        }

        /// <summary>
        /// Created By : Vivek Gupta on 20-05-2016
        /// Description : top city prices
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<Bikewale.Entities.PriceQuote.PriceQuoteOfTopCities> FetchPriceQuoteOfTopCitiesCache(uint modelId, uint topCount)
        {
            IEnumerable<Bikewale.Entities.PriceQuote.PriceQuoteOfTopCities> prices = null;
            string key = String.Format("BW_TopCitiesPrice_{0}_{1}", modelId, topCount);
            try
            {
                prices = _cache.GetFromCache<IEnumerable<Bikewale.Entities.PriceQuote.PriceQuoteOfTopCities>>(key, new TimeSpan(1, 0, 0), () => _obPriceQuote.FetchPriceQuoteOfTopCities(modelId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PriceQuoteCache.FetchPriceQuoteOfTopCitiesCache");

            }
            return prices;
        }


        /// <summary>
        /// Written BY : Ashish G. Kamble 
        /// Summary : Function get the prices of the given model in the nearest cities of the given city. Function gets data from BAL and cache it.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<Entities.PriceQuote.PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount)
        {
            IEnumerable<PriceQuoteOfTopCities> prices = null;

            string key = String.Format("BW_PriceInNearestCities_m_{0}_c_{1}_t_{2}", modelId, cityId, topCount);

            try
            {
                prices = _cache.GetFromCache<IEnumerable<PriceQuoteOfTopCities>>(key, new TimeSpan(1, 0, 0), () => _obPriceQuote.GetModelPriceInNearestCities(modelId, cityId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PriceQuoteCache.GetModelPriceInNearestCities");
            }
            return prices;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 3 October 2018
        /// Description : returns model top version price in nearest cities
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public PriceInTopCitiesWidgetVM GetModelPriceInNearestCities(uint modelId, uint cityId)
        {
            DateTime dt1 = DateTime.Now, dt2;
            PriceInTopCitiesWidgetVM priceInTopCities = null;
            string nearestCityKey = String.Format("BW_PriceInNC_M_{0}_C_{1}", modelId, cityId);
            try
            {
                IEnumerable<PriceQuoteOfTopCities> priceInNearestCities = _cache.GetFromCache<IEnumerable<PriceQuoteOfTopCities>>(nearestCityKey, new TimeSpan(30, 0, 0, 0), () => _priceQuoteCacheHelper.GetModelPriceInNearestCities(cityId, modelId));

                if (priceInNearestCities != null && priceInNearestCities.Any())
                {
                    priceInTopCities = new PriceInTopCitiesWidgetVM
                    {
                        PriceQuoteList = priceInNearestCities,
                        BikeName = string.Format("{0} {1}", priceInNearestCities.FirstOrDefault().Make, priceInNearestCities.FirstOrDefault().Model)
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.PriceQuote.PriceQuoteCache.GetModelPriceInNearestCities(modelId = {0}, cityId = {1})", modelId, cityId));
            }
            finally
            {
                dt2 = DateTime.Now;
                ThreadContext.Properties["ModelPriceInNearestCities_FetchTime"] = (dt2 - dt1).TotalMilliseconds;
            }
            return priceInTopCities;
        }
        
        /// <summary>
        /// Modified by  : Rajan Chauhan on 28 September 2018
        /// Description  : Increased cached duration to 7 days
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<OtherVersionInfoEntity> GetOtherVersionsPrices(uint modelId, uint cityId)
        {
            IEnumerable<OtherVersionInfoEntity> versions = null;

            string key = String.Format("BW_VersionPrices_M_{0}_C_{1}", modelId, cityId);
            try
            {
                versions = _cache.GetFromCache<IEnumerable<OtherVersionInfoEntity>>(key, new TimeSpan(7, 0, 0, 0), () => _obPriceQuote.GetOtherVersionsPrices(modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PriceQuoteCache.GetOtherVersionsPrices");
            }
            return versions;
        }

        /// <summary>
        /// Gets the manufacturer dealers.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>
        /// Created by : Sangram Nandkhile on 10-May-2017
        /// Modified by : Rajan Chauhan on 16 August 2018
        /// Description : Added check on dictionary for cityId existence
        /// </returns>
        public IEnumerable<ManufacturerDealer> GetManufacturerDealers(uint cityId, uint dealerId)
        {
            IEnumerable<ManufacturerDealer> dealerInCity = null;
            IDictionary<uint, List<ManufacturerDealer>> dealer = null;
            string key = "BW_Manufacturer_Dealer_"+dealerId;
            try
            {
                dealer = _cache.GetFromCache<IDictionary<uint, List<ManufacturerDealer>>>(key, new TimeSpan(24, 0, 0), () => _obPriceQuote.GetManufacturerDealers(dealerId));
                
                if (dealer != null && dealer.ContainsKey(cityId))
                {
                    dealerInCity = dealer[cityId]; 
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PriceQuoteCache.GetManufacturerDealers");
            }
            return dealerInCity;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Mar 2018
        /// Description :   Get GetDefaultPriceQuoteVersion by calling DAL
        /// Modified by :   Monika Korrapati on 28 Sept 2018
        /// Description :   Increased cache timing to 30 days
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId)
        {
            uint versionId = 0;
            string key = String.Format("BW_DefaultPQVersion_{0}_{1}", modelId, cityId);
            try
            {
                versionId = _cache.GetFromCache<uint>(key, new TimeSpan(30, 0, 0, 0), () => _dealerPriceQuoteRepository.GetDefaultPriceQuoteVersion(modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PriceQuoteCache.GetDefaultPriceQuoteVersion({0},{1})", modelId, cityId));
            }  
            return versionId;
        }


        public Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerPriceQuoteByPackageV2(Entities.BikeBooking.PQParameterEntity objParams)
        {
            try
            {
                if (objParams != null)
                {
                    string key = String.Format("BW_DPQ_{0}_{1}_{2}_{3}", objParams.CityId, objParams.VersionId, objParams.DealerId, objParams.AreaId);
                    Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealerQuotation = _cache.GetFromCache<Entities.PriceQuote.v2.DetailedDealerQuotationEntity>(key, new TimeSpan(0, 30, 0), () => _objDealerPriceQuote.GetDealerPriceQuoteByPackageV2(objParams));

                    return dealerQuotation;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PriceQuoteCache.GetDealerPriceQuoteByPackageV2({0},{1},{2},{3})", objParams.DealerId, objParams.VersionId, objParams.CityId, objParams.AreaId));
            }
            return null;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2018
        /// Description :   Get Manufacturer Campaign Mobile Rendered Template from memcache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="leadCampaign"></param>
        /// <returns></returns>
        public string GetManufacturerCampaignMobileRenderedTemplate(string key, Entities.manufacturecampaign.ManufactureCampaignLeadEntity leadCampaign)
        {
            try
            {
                string htmlTemplate = _cache.GetFromCache<string>(key + "_v1", new TimeSpan(24, 0, 0), () => GetRenderMobileTemplate(leadCampaign));
                return htmlTemplate;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetManufacturerCampaignMobileRenderedTemplate()", leadCampaign.CampaignId));
            }
            return null;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 19 JUne 2018
        /// Description : used new entity for ManufactureCampaignLeadEntity
        /// </summary>
        /// <param name="key"></param>
        /// <param name="leadCampaign"></param>
        /// <returns></returns>
        public string GetManufacturerCampaignMobileRenderedTemplateV2(string key, Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity leadCampaign)
        {
            try
            {
                string htmlTemplate = _cache.GetFromCache<string>(key + "_v1.1", new TimeSpan(24, 0, 0), () => GetRenderMobileTemplateV2(leadCampaign));
                return htmlTemplate;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetManufacturerCampaignMobileRenderedTemplateV2({0})", leadCampaign.CampaignId));
            }
            return null;
        }

        /// <summary>
        /// Created by : Rajan Chauhan on 28 September 2018
        /// Description : Created GetVersionPricesByModelId to get Bikewale VersionPrices
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId)
        {
            IEnumerable<BikeQuotationEntity> versionPrices = null;
            try
            {
                string key = String.Format("BW_Version_PQ_M_{0}_C_{1}", modelId, cityId);
                bool hasArea = false;
                versionPrices = _cache.GetFromCache<IEnumerable<BikeQuotationEntity>>(key, new TimeSpan(7, 0, 0, 0), () => _obPriceQuote.GetVersionPricesByModelId(modelId, cityId, out hasArea));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PriceQuoteCache.GetVersionPricesByModelId( {0}, {1})", modelId, cityId));
            }
            return versionPrices;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2018
        /// Description :   GetRenderMobileTemplate using MvcHelper class
        /// Modified by :   Sumit Kate on 30 Mar 2018
        /// Description :   Changed the template name
        /// </summary>
        /// <param name="leadCampaign"></param>
        /// <returns></returns>
        private string GetRenderMobileTemplate(Entities.manufacturecampaign.ManufactureCampaignLeadEntity leadCampaign)
        {
            try
            {
                string template = MvcHelper.Render(string.Format("LeadCampaign_Android_{0}", leadCampaign.CampaignId), leadCampaign, leadCampaign.LeadsHtmlMobile);
                return template;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetManufacturerCampaignMobileRenderedTemplate()", leadCampaign.CampaignId));
            }
            return null;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 19 June 2018
        /// Description : used new entity for ManufactureCampaignLeadEntity
        /// </summary>
        /// <param name="leadCampaign"></param>
        /// <returns></returns>
        private string GetRenderMobileTemplateV2(Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity leadCampaign)
        {
            try
            {
                string template = MvcHelper.Render(string.Format("LeadCampaign_Android_{0}", leadCampaign.CampaignId), leadCampaign, leadCampaign.LeadsHtmlMobile);
                return template;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetManufacturerCampaignMobileRenderedTemplateV2()", leadCampaign.CampaignId));
            }
            return null;
        }
    }
}

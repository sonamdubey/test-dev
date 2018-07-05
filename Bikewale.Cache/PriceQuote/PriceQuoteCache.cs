using Bikewale.Entities;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Cache.PriceQuote
{
    /// Created By : Vivek Gupta on 20-05-2016
    public class PriceQuoteCache : IPriceQuoteCache
    {
        private readonly ICacheManager _cache = null;
        private readonly IPriceQuote _obPriceQuote = null;
        private readonly Bikewale.Interfaces.BikeBooking.IDealerPriceQuote _dealerPQRepository = null;
        private readonly Bikewale.Interfaces.AutoBiz.IDealerPriceQuote _objDealerPriceQuote = null;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(PriceQuoteCache));
        public PriceQuoteCache(ICacheManager cache, IPriceQuote obPriceQuote, Bikewale.Interfaces.BikeBooking.IDealerPriceQuote dealerPQRepository, Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objDealerPriceQuote)
        {
            _cache = cache;
            _obPriceQuote = obPriceQuote;
            _dealerPQRepository = dealerPQRepository;
            _objDealerPriceQuote = objDealerPriceQuote;
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


        public IEnumerable<OtherVersionInfoEntity> GetOtherVersionsPrices(uint modelId, uint cityId)
        {
            IEnumerable<OtherVersionInfoEntity> versions = null;

            string key = String.Format("BW_VersionPrices_M_{0}_C_{1}", modelId, cityId);
            try
            {
                versions = _cache.GetFromCache<IEnumerable<OtherVersionInfoEntity>>(key, new TimeSpan(6, 0, 0), () => _obPriceQuote.GetOtherVersionsPrices(modelId, cityId));
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
        /// </returns>
        public IEnumerable<ManufacturerDealer> GetManufacturerDealers(uint cityId, uint dealerId)
        {
            IEnumerable<ManufacturerDealer> dealers = null;
            string key = "BW_Manufacturer_Dealers_All";
            try
            {
                dealers = _cache.GetFromCache<IEnumerable<ManufacturerDealer>>(key, new TimeSpan(24, 0, 0), () => _obPriceQuote.GetManufacturerDealers());
                if (dealers != null && dealers.Any())
                {
                    dealers = (from d in dealers where d.CityId == cityId && d.DealerId == dealerId select d).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PriceQuoteCache.GetManufacturerDealers");
            }
            return dealers;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Mar 2018
        /// Description :   Get GetDefaultPriceQuoteVersion by calling DAL
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
                versionId = _cache.GetFromCache<uint>(key, new TimeSpan(24, 0, 0), () => _dealerPQRepository.GetDefaultPriceQuoteVersion(modelId, cityId));
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
                ErrorClass.LogError(ex, String.Format("GetManufacturerCampaignMobileRenderedTemplateV2()", leadCampaign.CampaignId));
            }
            return null;
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

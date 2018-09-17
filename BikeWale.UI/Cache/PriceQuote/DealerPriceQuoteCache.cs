

using Bikewale.Interfaces.AutoBiz;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using BikeWale.Entities.AutoBiz;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Cache.PriceQuote
{
    public class DealerPriceQuoteCache : IDealerPriceQuoteCache
    {
        private readonly ICacheManager _cache;
        private readonly IDealerPriceQuote _objDealerPQ;

        public DealerPriceQuoteCache(ICacheManager cache, IDealerPriceQuote objDealerPQ)
        {
            _cache = cache;
            _objDealerPQ = objDealerPQ;
        }
        public IEnumerable<PQ_VersionPrice> GetDealerPriceQuotesByModelCity(uint cityId, uint modelId, uint dealerId)
        {
            IEnumerable<PQ_VersionPrice> objDealerPrice = null;
            string key = string.Format("BW_DealerPriceQuotes_{0}_{1}_{2}", cityId, modelId, dealerId);
            try
            {
                objDealerPrice = _cache.GetFromCache<IEnumerable<PQ_VersionPrice>>(key, new TimeSpan(1, 0, 0), () => _objDealerPQ.GetDealerPriceQuotesByModelCity(cityId, modelId, dealerId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.PriceQuote.GetDealerPriceQuotesByModelCity");
            }
            return objDealerPrice;
        }
		/// <summary>
		/// Created By : Prabhu Puredla on 18 july 2018
		/// Description : Get All active mla combinations
		/// </summary>		
		/// <returns></returns>
		public IEnumerable<string> GetMLAMakeCities()
		{
			IEnumerable<string> mlaMakeCities = null;
			string key = "MlaMakeCities";
			try
			{
				mlaMakeCities = _cache.GetFromCache<IEnumerable<string>>(key, new TimeSpan(7,0,0,0), () => _objDealerPQ.GetMLAMakeCities());
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.Cache.PriceQuote.GetMLAMakeCities");
			}
			return mlaMakeCities;
		}
    }
}

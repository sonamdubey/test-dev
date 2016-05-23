using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.PriceQuote
{
    /// Created By : Vivek Gupta on 20-05-2016
    public class PriceQuoteCache : IPriceQuoteCache
    {
        private readonly ICacheManager _cache = null;
        private readonly IPriceQuote _obPriceQuote = null;

        public PriceQuoteCache(ICacheManager cache, IPriceQuote obPriceQuote)
        {
            _cache = cache;
            _obPriceQuote = obPriceQuote;
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
            string key = String.Format("BW_TopCityPrices_{0}_{1}", modelId, topCount);
            try
            {
                prices = _cache.GetFromCache<IEnumerable<Bikewale.Entities.PriceQuote.PriceQuoteOfTopCities>>(key, new TimeSpan(1, 0, 0), () => _obPriceQuote.FetchPriceQuoteOfTopCities(modelId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "PriceQuoteCache.FetchPriceQuoteOfTopCitiesCache");
                objErr.SendMail();
            }
            return prices;
        }
    }
}

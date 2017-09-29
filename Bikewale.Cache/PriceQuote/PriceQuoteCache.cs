﻿using Bikewale.Entities;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
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
            string key = String.Format("BW_TopCitiesPrice_{0}_{1}", modelId, topCount);
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
                ErrorClass objErr = new ErrorClass(ex, "PriceQuoteCache.GetModelPriceInNearestCities");
            }
            return prices;
        }


        public IEnumerable<OtherVersionInfoEntity> GetOtherVersionsPrices(uint modelId, uint cityId)
        {
            IEnumerable<OtherVersionInfoEntity> versions = null;

            string key = String.Format("BW_VersionPrices_{0}_C_{1}", modelId, cityId);
            try
            {
                versions = _cache.GetFromCache<IEnumerable<OtherVersionInfoEntity>>(key, new TimeSpan(6, 0, 0), () => _obPriceQuote.GetOtherVersionsPrices(modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "PriceQuoteCache.GetOtherVersionsPrices");
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
                ErrorClass objErr = new ErrorClass(ex, "PriceQuoteCache.GetManufacturerDealers");
            }
            return dealers;
        }
    }
}

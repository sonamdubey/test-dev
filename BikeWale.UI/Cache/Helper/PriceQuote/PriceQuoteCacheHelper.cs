

using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Device.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;

namespace Bikewale.Cache.Helper.PriceQuote
{
    public class PriceQuoteCacheHelper : IPriceQuoteCacheHelper
    {
        private readonly ICacheManager _cache;
        public PriceQuoteCacheHelper(ICacheManager cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 3 October 2018
        /// Description : return model price in NearBy cities based on haversine distance
        /// </summary>
        /// <param name="modelPrices"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(ModelTopVersionPrices modelPrices, uint cityId, uint modelId)
        {
            IList<PriceQuoteOfTopCities> priceInNearestCities = null;
            try
            {
                if (modelPrices != null && modelPrices.CityPrice != null)
                {
                    CityPriceEntity currentCity = modelPrices.CityPrice.Where(x => x.CityId == cityId).FirstOrDefault();
                    if(currentCity != null)
                    {
                        Distance distObj = new Distance();
                        //get nearest citites where the model price is available based on haversine distance formula
                        IEnumerable<CityPriceEntity> cityObj = modelPrices.CityPrice.Where(t => t.OnRoadPrice > 0 && t.CityId != cityId)
                                                            .OrderBy(x => distObj.GetDistanceBetweenTwoLocations(currentCity.Latitude, currentCity.Longitude, x.Latitude, x.Longitude)).Take(9);
                        if (cityObj != null)
                        {
                            priceInNearestCities = new List<PriceQuoteOfTopCities>();
                            foreach (var city in cityObj)
                            {
                                string key = string.Format("BW_NearByCityKeyMapping_C_{0}", city.CityId);
                                //cache price key with nearBy city
                                SaveCityKeyMapping(key, string.Format("BW_PriceInNearestCities_M_{0}_C_{1}", modelId, cityId));

                                priceInNearestCities.Add(new PriceQuoteOfTopCities
                                {
                                    CityId = city.CityId,
                                    CityName = city.CityName,
                                    CityMaskingName = city.CityMaskingName,
                                    OnRoadPrice = city.OnRoadPrice,
                                    Make = modelPrices.BikeMake.MakeName != null ? modelPrices.BikeMake.MakeName : string.Empty,
                                    MakeMaskingName = modelPrices.BikeMake.MakeMaskingName != null ? modelPrices.BikeMake.MakeMaskingName : string.Empty,
                                    Model = modelPrices.BikeModel.ModelName != null ? modelPrices.BikeModel.ModelName : string.Empty,
                                    ModelMaskingName = modelPrices.BikeModel.MaskingName != null ? modelPrices.BikeModel.MaskingName : string.Empty
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Helper.PriceQuote.GetModelPriceInNearestCities");
            }
            return priceInNearestCities;
        }
        /// <summary>
        /// Created by  : Pratibha Verma on 4 October 2018
        /// Description : Method to cache priceKeys mapped with cityKey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newKey"></param>
        private void SaveCityKeyMapping(string cityKey, string priceKey)
        {
            HashSet<string> oldKeyData;
            try
            {
                oldKeyData = _cache.GetFromCache<HashSet<string>>(cityKey, new TimeSpan(30, 0, 0, 0), () => new HashSet<string>());
                if (oldKeyData != null)
                {
                    oldKeyData.Add(priceKey);
                }
                else
                {
                    oldKeyData = new HashSet<string>();
                    oldKeyData.Add(priceKey);
                }
                _cache.RefreshCache(cityKey);
                _cache.GetFromCache<HashSet<string>>(cityKey, new TimeSpan(30, 0, 0, 0), () => oldKeyData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.PriceQuote.PriceQuoteCache.GetCityKeyMapping(cityKey = {0}, priceKey = {1})", cityKey, priceKey));
            }
        }
    }
}
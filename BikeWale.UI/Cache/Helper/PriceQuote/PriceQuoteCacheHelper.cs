

using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using System.Web.Hosting;

namespace Bikewale.Cache.Helper.PriceQuote
{
    public class PriceQuoteCacheHelper : IPriceQuoteCacheHelper
    {
        private readonly ICacheManager _cache;
        private readonly IPriceQuote _objPriceQuote = null;
        public PriceQuoteCacheHelper(ICacheManager cache, IPriceQuote objPriceQuote)
        {
            _cache = cache;
            _objPriceQuote = objPriceQuote;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 3 October 2018
        /// Description : return model price in NearBy cities based on haversine distance
        /// </summary>
        /// <param name="modelPrices"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint cityId, uint modelId)
        {
            IList<PriceQuoteOfTopCities> priceInNearestCities = null;
            try
            {
                ModelTopVersionPrices modelPrices = GetTopVersionPriceInCities(modelId);
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
                            //reverse key mapping of nearest cities
                            string priceKey = string.Format("BW_PriceInNC_M_{0}_C_{1}", modelId, cityId);
                            HostingEnvironment.QueueBackgroundWorkItem(f => SaveCityKeyMapping(modelId, priceKey, cityObj));

                            priceInNearestCities = new List<PriceQuoteOfTopCities>();
                            foreach (var city in cityObj)
                            {
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
                ErrorClass.LogError(ex, "Bikewale.Cache.Helper.PriceQuote.PriceQuoteCacheHelper.GetModelPriceInNearestCities");
            }
            return priceInNearestCities;
        }
        /// <summary>
        /// Created by  : Pratibha Verma on 4 October 2018
        /// Description : Method to cache priceKeys mapped with cityKey(reverse key mapping)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newKey"></param>
        private void SaveCityKeyMapping(uint modelId,string priceKey, IEnumerable<CityPriceEntity> cityObj)
        {
            try
            {
                foreach (var city in cityObj)
                {
                    string cityKey = String.Format("BW_NearByCityKeyMapping_C_{0}", city.CityId);
                    Dictionary<uint, HashSet<string>> oldKeyData = _cache.GetFromCache<Dictionary<uint, HashSet<string>>>(cityKey, new TimeSpan(30, 0, 0, 0), () => new Dictionary<uint, HashSet<string>>());

                    if (oldKeyData != null)
                    {
                        if (oldKeyData.ContainsKey(modelId))
                        {
                            oldKeyData[modelId].Add(priceKey);
                        }
                        else
                        {
                            oldKeyData.Add(modelId, new HashSet<string> { priceKey });
                        }
                    }
                    else
                    {
                        oldKeyData = new Dictionary<uint, HashSet<string>>();
                        oldKeyData.Add(modelId, new HashSet<string> { priceKey });
                    }
                    _cache.RefreshCache(cityKey);
                    _cache.GetFromCache<Dictionary<uint, HashSet<string>>>(cityKey, new TimeSpan(30, 0, 0, 0), () => oldKeyData);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.Helper.PriceQuote.PriceQuoteCacheHelper.GetCityKeyMapping(modelId = {0}, priceKey = {1})", modelId, priceKey));
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 28 September 2018
        /// Description : returns the topVersion price in all cities
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private ModelTopVersionPrices GetTopVersionPriceInCities(uint modelId)
        {
            ModelTopVersionPrices modelTopVersionPrices = null;
            string key = string.Format("BW_TopVersionPrices_M_{0}", modelId);
            try
            {
                modelTopVersionPrices = _cache.GetFromCache<ModelTopVersionPrices>(key, new TimeSpan(30, 0, 0, 0), () => _objPriceQuote.GetTopVersionPriceInCities(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.Helper.PriceQuote.PriceQuoteCacheHelper.GetTopVersionPriceInCities(modelId = {0})", modelId));
            }
            return modelTopVersionPrices;
        }
    }
}
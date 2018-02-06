using Bikewale.Notifications;
using System;

namespace BikewaleOpr.Cache
{
    /// <summary>
    /// Created by: Sajal Gupta on 09-01-2017
    /// Desc: Static object to hold reference for memcache and delete the data as and when needed
    /// </summary>
    public class BwMemCache
    {
        /// <summary>
        /// Created by : Ashutosh Sharma on 07 Nov 2017
        /// Description : Clear cache for most popular bikes by make with city price
        /// </summary>
        /// <param name="makeId"></param>
        public static void ClearPopularBikesByMakeWithCityPriceCacheKey(uint makeId)
        {
            try
            {
                string key = string.Format("BW_PopularBikesByMakeWithCityPrice_V1_{0}_", makeId);
                string temp = string.Empty;
                for (int i = 0; i < 1500; i++)
                {
                    temp = key + i;
                    MemCachedUtil.Remove(temp);
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikewaleOpr.Cache.BwMemCache.ClearPopularBikesByMakeWithCityPriceCacheKey_{0}", makeId));
            }
        }
        public static bool ClearPopularBikesCacheKey(uint? topCount = null, uint? makeId = null)
        {
            bool cacheKeyClearStatus = false;
            try
            {
                string key;
                key = string.Format("BW_PopularBikes" + (topCount.HasValue ? String.Format("_TC_{0}", topCount.Value) : "") + (makeId.HasValue ? String.Format("_MK_{0}", makeId.Value) : ""));
                MemCachedUtil.Remove(key);
                cacheKeyClearStatus = true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearPopularBikesCacheKey {0}, {1}", topCount, makeId));
            }
            return cacheKeyClearStatus;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Method to clear cache for Ad slots.
        /// </summary>
        public static void ClearAdSlotsCache()
        {
            try
            {
                string key = "BW_AdSlots";
                MemCachedUtil.Remove(key);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.Cache.BwMemCache.ClearAdSlotsCache");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 10 Nov 2017
        /// Description : Method to clear cache for price quote of top cities.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        public static void ClearPriceQuoteOfTopCities(uint modelId, ushort topCount)
        {
            try
            {
                string key = String.Format("BW_TopCitiesPrice_{0}_{1}", modelId, topCount);
                MemCachedUtil.Remove(key);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.Cache.BwMemCache.ClearPriceQuoteOfTopCities");
                throw;
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 10 Nov 2017
        /// Description : Method to clear cache for price in nearest cities.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="topCount"></param>
        public static void ClearModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount)
        {
            try
            {
                string key = string.Format("BW_PriceInNearestCities_m_{0}_c_{1}_t_{2}", modelId, cityId, topCount);
                MemCachedUtil.Remove(key);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.Cache.BwMemCache.ClearModelPriceInNearestCities");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 10 Nov 2017
        /// Description : Method to clear cache for most popular bikes by body style.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="topCount"></param>
        public static void ClearMostPopularBikesByModelBodyStyle(uint modelId, uint cityId, ushort topCount)
        {
            try
            {
                string key = string.Format("BW_PopularBikesListByBodyType_MO_V1_{0}_city_{1}_topcount_{2}", modelId, cityId, topCount);
                MemCachedUtil.Remove(key);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.Cache.BwMemCache.ClearMostPopularBikesByModelBodyStyle");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 10 Nov 2017
        /// Description : Method to clear cache for similar bikes list.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        public static void ClearSimilarBikesList(uint versionId, ushort topCount, uint cityId)
        {
            try
            {
                string key = string.Format("BW_SimilarBikes_V1_{0}_Cnt_{1}_{2}", versionId, topCount, cityId);
                MemCachedUtil.Remove(key);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.Cache.BwMemCache.ClearSimilarBikesList");
            }
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 12 Jan 2016
        /// Description: Clear cache for upcoming bikes with the necessary keys
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public static bool ClearUpcomingBikesCacheKey(int pageSize, int sortBy, uint? makeId, uint? modelId)
        {
            bool cacheKeyClearStatus = false;
            try
            {
                string key = string.Empty;
                key = string.Format("BW_UpcomingBikes_Cnt_{0}_SO_{1}", pageSize, sortBy);
                if (makeId.HasValue && makeId.Value > 0)
                    key += "_MK_" + makeId;
                if (modelId.HasValue && modelId.Value > 0)
                    key += "_MO_" + modelId;
                MemCachedUtil.Remove(key);
                cacheKeyClearStatus = true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearUpcomingBikesCacheKey {0}, {1}" + (makeId.HasValue ? String.Format(", {0}", makeId.Value) : "") + (modelId.HasValue ? String.Format(", {0}", modelId.Value) : ""), pageSize, sortBy));
            }
            return cacheKeyClearStatus;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 22 Nov 2017
        /// Description : Method to clear cache for new, upcoming models, synopsis and other makes from make of a bike series.
        /// Modified by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Changed cache key from 'BW_NewModelsBySeriesId_s_{0}_c_{1}' to 'BW_NewModelsBySeriesId_seriesId_{0}
        /// </summary>
        /// <param name="seriesId">Series Id of which cache needs to be clear</param>
        /// <param name="makeId">Make Id of which cache needs to be clear</param>
        public static void ClearSeriesCache(uint seriesId, uint makeId)
        {
            try
            {
                for (int cityId = 0; cityId < 1500; cityId++)
                {
                    MemCachedUtil.Remove(string.Format("BW_NewModelsBySeriesId_seriesId_{0}_cityId_{1}", seriesId, cityId));
                }
                MemCachedUtil.Remove(string.Format("BW_GetModelIdsBySeries_{0}", seriesId));
                MemCachedUtil.Remove(string.Format("BW_UpcomingModelsBySeriesId_{0}", seriesId));
                MemCachedUtil.Remove(string.Format("BW_SynopsisBySeriesId_{0}", seriesId));
                MemCachedUtil.Remove(string.Format("BW_OtherSeriesByMakeId_{0}", makeId));
                MemCachedUtil.Remove(string.Format("BW_BikeSeriesComparision_{0}", seriesId));
                MemCachedUtil.Remove(string.Format("BW_GetModelIdsBySeries_{0}", seriesId));


            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearSeriesCache_SeriesId_{0}_MakeId_{1}", seriesId, makeId));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   ClearNewLaunchesBikes
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static bool ClearNewLaunchesBikes(string cityId)
        {
            bool cacheKeyClearStatus = false;
            try
            {
                string key = String.Format("BW_NewLaunchedBikes_Cid_{0}", cityId);
                MemCachedUtil.Remove(key);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearNewLaunchesBikes({0}", cityId));
            }
            return cacheKeyClearStatus;
        }

        public static void ClearVersionPrice(string model, string city)
        {
            try
            {
                string key = String.Format("BW_VersionPrices_{0}_C_{1}", model, city);
                MemCachedUtil.Remove(key);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearVersionPrice({0},{1})", city, model));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Mar 2017
        /// Description :   Clear User Reviews Cache
        /// </summary>
        public static void ClearUserReviewsCache()
        {
            try
            {
                MemCachedUtil.Remove("BW_UserReviews");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ClearUserReviewsCache");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summary : Function to clear price quote details for given model id
        /// </summary>
        /// <param name="modelId"></param>
        public static void ClearPriceQuoteCity(uint modelId)
        {
            try
            {
                MemCachedUtil.Remove(string.Format("BW_PQCity_{0}", modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewalwOpr.Cache.BwMemCache.ClearPriceQuoteCity");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summmary : Function to clear version details for given model id
        /// Modified by : Ashutosh Sharma on 04 Oct 2017
        /// Description : Changed cacke key from 'BW_ModelDetail_' to 'BW_ModelDetail_V1'.
        /// Modified by : Rajan Chauhan on 06 Feb 2018.
        /// Description : Changed version of key from 'BW_ModelDetail_V1_' to 'BW_ModelDetail_'.
        /// </summary>
        /// <param name="modelId"></param>
        public static void ClearVersionDetails(uint modelId)
        {
            try
            {
                MemCachedUtil.Remove(string.Format("BW_ModelDetail_{0}", modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewalwOpr.Cache.BwMemCache.ClearVersionDetails");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summary : Function to clear popular bikes by makes
        /// </summary>
        /// <param name="makeId"></param>
        public static void ClearPopularBikesByMakes(uint makeId)
        {
            try
            {
                MemCachedUtil.Remove(string.Format("BW_PopularBikesByMake_{0}", makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewalwOpr.Cache.BwMemCache.ClearPopularBikesByMakes");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summary : Function to clear upcoming bikes
        /// </summary>
        public static void ClearUpcomingBikes()
        {
            try
            {
                MemCachedUtil.Remove("BW_UpcomingModels");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewalwOpr.Cache.BwMemCache.ClearUpcomingBikes");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summary : Function to clear version by type cache
        /// </summary>
        /// <param name="modelId"></param>
        public static void ClearVersionByType(UInt32 modelId)
        {
            try
            {
                MemCachedUtil.Remove(string.Format("BW_VersionsByType_{0}_MO_{1}", 2, modelId)); // for new
                MemCachedUtil.Remove(string.Format("BW_VersionsByType_{0}_MO_{1}", 4, modelId)); // for upcoming
                MemCachedUtil.Remove(string.Format("BW_VersionsByType_{0}_MO_{1}", 7, modelId)); // for all 
                MemCachedUtil.Remove(string.Format("BW_VersionsByType_{0}_MO_{1}", 16, modelId)); // for videos
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewalwOpr.Cache.BwMemCache.ClearVersionByType");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 19-Aug-2017
        /// Description : Method to clear Popular bikes by body style cache.
        /// </summary>
        /// <param name="modelId"></param>
        public static void ClearPopularBikesByBodyStyle(ushort bodyStyleId)
        {
            try
            {
                MemCachedUtil.Remove(string.Format("BW_PopularBikesListByBodyType_Bodystyle_{0}", bodyStyleId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewalwOpr.Cache.BwMemCache.ClearPopularBikesByBodyStyle");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 19-Aug-2017
        /// Description : Method to clear Models by series Id.
        /// </summary>
        /// <param name="modelId"></param>
        public static void ClearModelsBySeriesId(uint seriesId)
        {
            try
            {
                MemCachedUtil.Remove(string.Format("BW_ModelsBySeriesId_{0}", seriesId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewalwOpr.Cache.BwMemCache.ClearGetModelsBySeriesId");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Nov 2017
        /// Description :   Clear Masking Mapping Cache
        /// </summary>
        public static void ClearMaskingMappingCache()
        {
            try
            {
                MemCachedUtil.Remove("BW_ModelMapping");
                MemCachedUtil.Remove("BW_NewModelMaskingNames_v1");
                MemCachedUtil.Remove("BW_OldModelMaskingNames_v1");
                MemCachedUtil.Remove("BW_ModelSeries_MaskingNames");
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BwMemcache.ClearMaskingMappingCache");
            }
        }

        /// <summary>
        /// Clear Bike Version Price memcache
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        public static void ClearBikeVersionPrice(uint dealerId, uint versionId)
        {
            MemCachedUtil.Remove(String.Format("BW_Dealer_{0}_Version_{1}", dealerId, versionId));
        }
    }
}

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
        /// </summary>
        /// <param name="modelId"></param>
        public static void ClearVersionDetails(uint modelId)
        {
            try
            {
                MemCachedUtil.Remove(string.Format("BW_ModelDetail_V1_{0}", modelId));
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

    }
}

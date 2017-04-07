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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearPopularBikesCacheKey {0}, {1}", topCount, makeId));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearUpcomingBikesCacheKey {0}, {1}" + (makeId.HasValue ? String.Format(", {0}", makeId.Value) : "") + (modelId.HasValue ? String.Format(", {0}", modelId.Value) : ""), pageSize, sortBy));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearNewLaunchesBikes({0}", cityId));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.ClearCache.CacheClear.ClearVersionPrice({0},{1})", city, model));
            }
        }
    }
}

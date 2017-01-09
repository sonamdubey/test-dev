using Bikewale.Notifications;
using System;

namespace BikewaleOpr.ClearCache
{
    /// <summary>
    /// Created by : Sajal Gupta on 09-01-2017
    /// Description : Class to hold functions to clear memcache keys.
    /// </summary>
    public class CacheClear
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
    }
}

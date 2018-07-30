using System;
using System.Configuration;

namespace Bikewale.Cache.Core
{
    /// <summary>
    /// CacheRefreshTime class deals with Cache management.
    /// </summary>
    public class CacheRefreshTime
    {
        /// <summary>
        /// Get cache refresh count from sesource file based on passed key. 
        /// If key not found condider default refresh time that is 30 min.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>int refresh time in minutes</returns>
        public static TimeSpan GetInMinutes(string key)
        {
            string refreshTime = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(refreshTime) ? DefaultRefreshTime() : TimeSpan.FromMinutes(Convert.ToDouble(refreshTime));
        }

        /// <summary>
        /// Default cache refresh time
        /// </summary>
        /// <returns></returns>
        public static TimeSpan DefaultRefreshTime()
        {
            return new TimeSpan(0, 30, 0);
        }
    }
}

using Bikewale.Entities.App;
using Bikewale.Interfaces.App;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Cache.App
{
    /// <summary>
    /// Created By  :  Sushil Kumar on 30th June 2016
    /// Description :  Cache Repository for cache of app version
    /// </summary>
    public class AppVersionCacheRepository : IAppVersionCache
    {
        private readonly ICacheManager _cache;
        private readonly IAppVersion _objAppVersion;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objAppVersion"></param>
        public AppVersionCacheRepository(ICacheManager cache, IAppVersion objAppVersion)
        {
            _cache = cache;
            _objAppVersion = objAppVersion;
        }


        /// <summary>
        /// Created by  : Sushil Kumar on 30th June 2016
        /// Summary     : Gets the status for app version
        /// </summary>
        /// <param name="appVersion"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public AppVersion CheckVersionStatus(uint appVersion, uint sourceId)
        {
            AppVersion _appVersion = null;

            string key = String.Format("BW_AppVersion_{1}_Src_{1}", appVersion, sourceId);
            try
            {
                _appVersion = _cache.GetFromCache<AppVersion>(key, new TimeSpan(24, 0, 0), () => _objAppVersion.CheckVersionStatus(appVersion, sourceId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeMakesCacheRepository.CheckVersionStatus");
                objErr.SendMail();
            }
            return _appVersion;
        }
    }
}

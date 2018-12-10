using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData.RecentLaunchedCar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.CarData
{
    public class RecentLaunchedCarCache : IRecentLaunchedCarCacheRepository
    {
        private IRecentLaunchedCarRepository _recentLaunchedRepo;
        private ICacheManager _recentLaunchedMemCache;

        public RecentLaunchedCarCache(IRecentLaunchedCarRepository cmsRepo, ICacheManager memcache)
        {
            _recentLaunchedRepo = cmsRepo;
            _recentLaunchedMemCache = memcache;
        }

        private bool UseMemcache()
        {
            return ConfigurationManager.AppSettings["IsMemcachedUsed"] == "true" ? true : false;
        }

        /// <summary>
        /// caching of list of recentlaunched cars if data is available in cache then list is provided from cache if not then database hit occurs
        /// written by Natesh Kumar on 1/10/2014
        /// </summary>
        /// <returns></returns>
        public List<RecentLaunchedCarEntity> GetRecentLaunchedCars()
        {
            #warning Changing this key will affect refreshing it from opr.So go to OPR web.config and find the key name 'NewLaunchesCacheKey' and update the key Template present there.           

            List<RecentLaunchedCarEntity> result = null;
            if (UseMemcache())
            {
                string cacheKey = "Recent-Launched-Car";
                result = _recentLaunchedMemCache.GetFromCache<List<RecentLaunchedCarEntity>>(cacheKey, CacheRefreshTime.DefaultRefreshTime(),
                                                    () => _recentLaunchedRepo.GetRecentLaunchedCars());
            }
            else 
            {
                result = _recentLaunchedRepo.GetRecentLaunchedCars();
            }

            return result;
        }
    }
}

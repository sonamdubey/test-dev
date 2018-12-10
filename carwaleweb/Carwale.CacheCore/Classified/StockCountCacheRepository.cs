using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Classified;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Carwale.Cache.Classified
{
    public class StockCountCacheRepository : IStockCountCacheRepository
    {
        /// <summary>
        /// Filters count separately from mamcache or database || Class Added by Jugal on 30-08-2014
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>

        private readonly IStockCountRepository _stockCountRepo;
        //ICacheManagerAsync
        private readonly ICacheManager _cacheProvider;

        public StockCountCacheRepository(IStockCountRepository stockCountRepo, ICacheManager cacheProvider)
        {
            _stockCountRepo = stockCountRepo;
            _cacheProvider = cacheProvider;
        }


        /// <summary>
        /// Returns the Used Cars count of CarModels from cache,if present or from Database
        /// Written By : Shalini on 05/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public UsedCarCount GetUsedCarsCount(int rootId,int cityId)
        {
            return _cacheProvider.GetFromCache<UsedCarCount>("UsedCarsCount-" + rootId + "-" + cityId,
                CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                () => _stockCountRepo.GetUsedCarsCount(rootId, cityId));
        }
    }
}

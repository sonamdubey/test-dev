using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Stock;
using Carwale.Interfaces;
using Carwale.Interfaces.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.Stock
{
    public class StockConditionCacheRepository : IStockConditionCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IStockConditionRepository _stockConditionRepository;

        public StockConditionCacheRepository(ICacheManager cacheProvider, IStockConditionRepository stockConditionRepository)
        {
            _cacheProvider = cacheProvider;
            _stockConditionRepository = stockConditionRepository;
        }

        public List<StockConditionItems> GetCarConditionParts()
        {
            return _cacheProvider.GetFromCache<List<StockConditionItems>>("UsedStockConditionParts",
                    CacheRefreshTime.EODExpire(), () => _stockConditionRepository.GetCarConditionParts());
        }
    }
}

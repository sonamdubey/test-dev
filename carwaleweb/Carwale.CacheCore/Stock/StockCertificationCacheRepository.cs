using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock.Certification;
using Carwale.Interfaces;
using Carwale.Interfaces.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.Stock
{
    public class StockCertificationCacheRepository : IStockCertificationCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IStockCertificationBL _stockCertificationBL;
        private const string _certKeyPrefix = "Certification_";

        public StockCertificationCacheRepository(ICacheManager cacheProvider, IStockCertificationBL stockCertificationBL)
        {
            _cacheProvider = cacheProvider;
            _stockCertificationBL = stockCertificationBL;
        }

        public StockCertification GetCarCertification(int inquiryId, bool isDealer)
        {
            return _cacheProvider.GetFromCache<StockCertification>(_certKeyPrefix + inquiryId + "_" + (isDealer ? 1 : 0),
                    CacheRefreshTime.NeverExpire(), () => _stockCertificationBL.GetCarCertification(inquiryId, isDealer));
        }

        public void RefreshCarCertification(int inquiryId, bool isDealer)
        {
            _cacheProvider.ExpireCacheWithoutDelay(_certKeyPrefix + inquiryId + "_" + (isDealer ? 1 : 0));
        }
    }
}

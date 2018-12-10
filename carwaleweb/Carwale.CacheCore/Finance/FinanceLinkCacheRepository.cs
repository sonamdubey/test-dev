using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Finance;
using Carwale.Interfaces;
using Carwale.Interfaces.Finance;

namespace Carwale.Cache.Finance
{
    public class FinanceLinkCacheRepository : IFinanceLinkDataCache
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IFinanceLinkDataRepository _financeLinkRepo;

        public FinanceLinkCacheRepository(ICacheManager cacheProvider, IFinanceLinkDataRepository financeLinkRepo)
        {
            _cacheProvider = cacheProvider;
            _financeLinkRepo = financeLinkRepo;
        }

        public FinanceLinkData GetUrlData(int platformId, int screenId)
        {
            string key = "FinanceLink_" + platformId + "_" + screenId;

            return _cacheProvider.GetFromCache<FinanceLinkData>(key, CacheRefreshTime.GetInDays(7), () => _financeLinkRepo.GetUrlData(platformId, screenId));           
        }
    }
}

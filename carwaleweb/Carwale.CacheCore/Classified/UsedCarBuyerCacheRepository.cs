using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Cache.Core;
using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces.Classified;

namespace Carwale.Cache.Classified
{
    public class UsedCarBuyerCacheRepository : IUsedCarBuyerCacheRepository
    {
        private readonly IUsedCarBuyerRepository _usedCarBuyerRepository;
        private readonly ICacheManager _cacheProvider;

        public UsedCarBuyerCacheRepository(IUsedCarBuyerRepository usedCarBuyerRepository, ICacheManager cacheProvider)
        {
            _usedCarBuyerRepository = usedCarBuyerRepository;
            _cacheProvider = cacheProvider;
        }

        public BuyerInfo GetBuyerInfo(string userId)
        {
            return _cacheProvider.GetFromCache("UsedCarBuyerInfo-" + userId, CacheRefreshTime.OneDayExpire(), () => _usedCarBuyerRepository.GetBuyerInfo(userId));
        }
    }
}
using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using System;

namespace Carwale.Cache.PriceQuote
{
    public class ThirdPartyEmiDetailsCache : IThirdPartyEmiDetailsCache
    {
        private readonly ICacheManager _cacheRepo;
        private readonly IThirdPartyEmiDetailsRepository _thirdPartyEmiDetailsRepository;

        public ThirdPartyEmiDetailsCache(ICacheManager cacheRepo, IThirdPartyEmiDetailsRepository thirdPartyEmiDetailsRepository)
        {
            _cacheRepo = cacheRepo;
            _thirdPartyEmiDetailsRepository = thirdPartyEmiDetailsRepository;
        }

        public ThirdPartyEmiDetails Get(int carVersionId, bool isMetallic)
        {
            return _cacheRepo.GetFromCache<ThirdPartyEmiDetails>(String.Format("thirdPartyEmi-Version-{0}-isMetallic-{1}", carVersionId, isMetallic),
               CacheRefreshTime.NeverExpire(), () => _thirdPartyEmiDetailsRepository.Get(carVersionId, isMetallic));
        }
    }
}

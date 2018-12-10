using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Interfaces;
using Carwale.Interfaces.UserProfiling;

namespace Carwale.Cache.UserProfiling
{
    public class UserProfilingCache : IUserProfilingCache
    {
        private readonly IUserProfilingRepo _userProfilingRepo;
        private readonly ICacheManager _cacheProvider;

        public UserProfilingCache(ICacheManager cacheProvider, IUserProfilingRepo userProfilingRepo)
        {
            _cacheProvider = cacheProvider;
            _userProfilingRepo = userProfilingRepo;
        }
        public bool GetUserProfilingStatus(int platformId)
        {
            return _cacheProvider.GetFromCache<bool>(string.Format("UserProfilingStatus_{0}", platformId),
                          CacheRefreshTime.NeverExpire(), () => _userProfilingRepo.UserProfilingStatus(platformId));
        }
    }
}

using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.Security;
using Carwale.Interfaces;

namespace Carwale.Cache.Security
{
    public class SecurityRepositoryCache<T> : SecurityRepository<T>, ISecurityRepository<T>
    {
        private readonly ICacheManager _cacheProvider;

        public SecurityRepositoryCache(ICacheManager cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public string GetPassword(string username)
        {
            return base.GetPassword(username);
        }

        public bool IsValidSource(string SourceId, string CWK)
        {
            return _cacheProvider.GetFromCache<bool>(string.Format("IsValidSource_{0}_{1}",SourceId,CWK),
                   CacheRefreshTime.NeverExpire(),
                   () => base.IsValidSource(SourceId,CWK));
        }
    }
}

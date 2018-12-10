using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.ES;
using Carwale.Interfaces;
using Carwale.Interfaces.ES;
using System.Collections.Generic;

namespace Carwale.Cache.ES
{
    public class PagesCache : IPagesCache
    {
        private readonly IPagesRepository _pagesRepo;
        protected readonly ICacheManager _cacheProvider;

        public PagesCache(IPagesRepository pagesRepo, ICacheManager cacheProvider)
        {
            _pagesRepo = pagesRepo;
            _cacheProvider = cacheProvider;
        }

        public List<Pages> GetPagesAndPropertiesCache(int applicationId, int platformId)
        {
            return _cacheProvider.GetFromCache(string.Format("PagesAndProperties_{0}_{1}", applicationId, platformId),
                           CacheRefreshTime.NeverExpire(),
                               () => _pagesRepo.GetPagesAndProperties(applicationId, platformId));
        }
    }
}
using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Template;
using Carwale.Interfaces.Template;
using System.Collections.Generic;

namespace Carwale.Cache.Template
{
    public class TemplatesCacheRepository : ITemplatesCacheRepository
    {
        private readonly ITemplatesRepository _templatesRepo;
        private ICacheManager _memCacheArticle;

        public TemplatesCacheRepository(ITemplatesRepository templateRepo, ICacheManager memcache)
        {
            _templatesRepo = templateRepo;
            _memCacheArticle = memcache;
        }

        List<Templates> ITemplatesCacheRepository.GetAll(short platformId, short typeId)
        {
            string cacheKey = "Template-"+typeId+"-"+platformId;

            return _memCacheArticle.GetFromCache<List<Templates>>(cacheKey, CacheRefreshTime.NeverExpire(),
                                                () => _templatesRepo.GetAll(platformId,typeId));
        }

        Templates ITemplatesCacheRepository.GetById(int templateId)
        {
            string cacheKey = "Template-" + templateId;

            return _memCacheArticle.GetFromCache<Templates>(cacheKey, CacheRefreshTime.NeverExpire(),
                                                () => _templatesRepo.GetById(templateId));
        }
    }
}

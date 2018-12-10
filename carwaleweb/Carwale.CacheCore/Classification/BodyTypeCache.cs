using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.Classification;
using Carwale.Entity.Classification;
using Carwale.Interfaces;
using Carwale.Interfaces.Classification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.Classification
{
    /// <summary>
    /// 
    /// </summary>
    public class BodyTypeCache :IBodyTypeCache
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IClassificationRepository _bodytypeRepo ;

        public BodyTypeCache(ICacheManager cacheProvider,IClassificationRepository bodyTypeRepository)
        {
            _cacheProvider = cacheProvider;
            _bodytypeRepo = bodyTypeRepository;
        }

        public List<BodyType> GetBodyType()
        {
            return _cacheProvider.GetFromCache<List<BodyType>>("BodyTypes_v2",
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _bodytypeRepo.GetBodyType());
        }
    }
}

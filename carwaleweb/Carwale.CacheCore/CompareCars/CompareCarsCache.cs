using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using AEPLCore.Cache;
using Carwale.Interfaces;
using Carwale.DAL.CarData;
using Carwale.Entity;
using System.Configuration;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Entity.CompareCars;
using Carwale.DAL.CompareCars;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.CompareCars
{
    public class CompareCarsCache : ICompareCarsCacheRepository
    {
        protected readonly ICacheManager _cacheProvider;
        protected readonly ICompareCarsRepository _ccRepo;

        public CompareCarsCache(ICacheManager cacheProvider, ICompareCarsRepository ccRepo)
        {
            _cacheProvider = cacheProvider;
            _ccRepo = ccRepo;
        }

        public Tuple<Hashtable, Hashtable> GetSubCategories()
        {
             #warning  Changing this key will affect refreshing it from opr.So go to OPR web.config and find the key name 'AllKeysRelatedToModel' and update the key Template present there. 
            return _cacheProvider.GetFromCache("CompareCarData_Categories", CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), () => _ccRepo.GetSubCategories());
        }

        public Hashtable GetItems()
        {
             #warning  Changing this key will affect refreshing it from opr.So go to OPR web.config and find the key name 'AllKeysRelatedToModel' and update the key Template present there. 
            return _cacheProvider.GetFromCache("CompareCarData_Items", CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), () => _ccRepo.GetItems());
        }

        public Tuple<Hashtable, List<Color>, CarWithImageEntity> GetVersionData(int versionId)
        {
             #warning  Changing this key will affect refreshing it from opr.So go to OPR web.config and find the key name 'AllKeysRelatedToModel' and update the key Template present there. 
            return _cacheProvider.GetFromCache("CompareCarData_Version_" + versionId.ToString()+"_v3", CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), () => _ccRepo.GetVersionData(versionId));
        }

        public List<HotCarComparison> GetHotComaprisons(short _topCount)
        {
            return _cacheProvider.GetFromCache(_topCount+"-HotCarComparisons_v1", CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), () => _ccRepo.GetHotComaprisons(_topCount));
        }

        public List<CompareCarOverview> GetCompareCarsDetails(Pagination page) 
        {
            return _cacheProvider.GetFromCache("CompareCarList-" + page.PageNo + "-" + page.PageSize, CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), () => _ccRepo.GetCompareCarsDetails(page));
        }

        public int GetCompareCarCount() 
        {
            return _cacheProvider.GetFromCache("CompareFeatureCars_Count", CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), () => _ccRepo.GetCompareCarCount());
        }
    }
}
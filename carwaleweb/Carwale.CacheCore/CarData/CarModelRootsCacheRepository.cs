using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.CarData;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;

namespace Carwale.Cache.CarData
{
    public class CarModelRootsCacheRepository : ICarModelRootsCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        //convention to be followed 
        //cachekey = CacheKeyPrefix_cacheKeySuffix
        //CacheKeyPrefix = "CW_VERSION_{PASCAL LETTER OF FUNCTION NAME}"  //EXAMPLE:FOR GetVersionSummaryByModel() IT WOULD BE "GVSBM"
        //cacheKeySuffix = _{0}_{1}_{2}_{3}_{4}
        //THINK BEFORE CACHE FUNCTION NAME (DUPLICATE PREFIX ISSUE)
        private static readonly Dictionary<string, string> _cacheKeyPrefix = new Dictionary<string, string>
		{
			{"GetRootsByMake","CW_ROOT_GRBM_D0"}, //DUPLICATE PREFIX CONVENTION
			{"GetRootByModel","CW_ROOT_GRBM_D1"}, //DUPLICATE PREFIX CONVENTION
			{"GetModelsByRootAndYear","CW_ROOT_GMBRAY"},
			{"GetYearsByRootId","CW_ROOT_GYBRI"},
			{"GetRoots","CW_ROOT_GR}"}
		};
        private readonly IModelRootsRepository _modelRootsRepo;
        private static readonly int _suffixArrCount = typeof(ModelRootAttribute).GetProperties().Length;
        public CarModelRootsCacheRepository(ICacheManager cacheProvider, IModelRootsRepository modelRootsRepo)
        {
            _cacheProvider = cacheProvider;
            _modelRootsRepo = modelRootsRepo;
        }

        public List<RootBase> GetRootsByMake(int makeId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelRootParameter.MakeId] = makeId.ToString();
            return _cacheProvider.GetFromCache<List<RootBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetRootsByMake"], cacheKeySuffixArr),
                    CacheRefreshTime.NeverExpire(), () => _modelRootsRepo.GetRootsByMake(makeId), () => _modelRootsRepo.GetRootsByMake(makeId,true));
        }

        public RootBase GetRootByModel(int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelRootParameter.ModelId] = modelId.ToString();
            return _cacheProvider.GetFromCache<RootBase>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetRootByModel"], cacheKeySuffixArr),
                    CacheRefreshTime.NeverExpire(), () => _modelRootsRepo.GetRootByModel(modelId), () => _modelRootsRepo.GetRootByModel(modelId,true));
        }

        public List<ModelsByRootAndYear> GetModelsByRootAndYear(int rootId, int year)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelRootParameter.Id] = rootId.ToString();
            cacheKeySuffixArr[(int)ModelRootParameter.Year] = year.ToString();
            return _cacheProvider.GetFromCache<List<ModelsByRootAndYear>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetModelsByRootAndYear"], cacheKeySuffixArr),
                CacheRefreshTime.DefaultRefreshTime(), () => _modelRootsRepo.GetModelsByRootAndYear(rootId, year), () => _modelRootsRepo.GetModelsByRootAndYear(rootId, year,true));
        }
                    
        public List<CarLaunchDiscontinueYear> GetYearsByRootId(int rootId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelRootParameter.Id] = rootId.ToString();
            return _cacheProvider.GetFromCache<List<CarLaunchDiscontinueYear>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetYearsByRootId"], cacheKeySuffixArr),
                    CacheRefreshTime.NeverExpire(), () => _modelRootsRepo.GetYearsByRootId(rootId), () => _modelRootsRepo.GetYearsByRootId(rootId,true));
        }

        public IEnumerable<RootBase> GetRoots(string rootIds)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelRootParameter.Ids] = rootIds;
            return _cacheProvider.GetFromCache(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetRoots"], cacheKeySuffixArr), CacheRefreshTime.DefaultRefreshTime(),
                () => _modelRootsRepo.GetRoots(rootIds), () => _modelRootsRepo.GetRoots(rootIds,true));
        }
        public bool RefreshCarModelRootCache(List<ModelRootAttribute> modelAttributes)
        {
            try
            {
                bool isRefreshed = true;
                string[] suffixArr = new string[_suffixArrCount];
                foreach (var modelRoot in modelAttributes)
                {
                    suffixArr[(int)ModelRootParameter.Id] = modelRoot.Id;
                    suffixArr[(int)ModelRootParameter.MakeId] = modelRoot.MakeId;
                    suffixArr[(int)ModelRootParameter.ModelId] = modelRoot.ModelId;
                    suffixArr[(int)ModelRootParameter.Year] = modelRoot.Year;
                    suffixArr[(int)ModelRootParameter.Ids] = modelRoot.Ids;
                    foreach (var key in CarModelRootsCacheRepository._cacheKeyPrefix.Values)
                    {
                        string finalKey = _cacheProvider.GenerateCacheKey(key, suffixArr);
                        isRefreshed = _cacheProvider.ExpireCacheWithCriticalRead(finalKey);
                        if (!isRefreshed)
                        {
                            Logger.LogException(null, "MemCache key name = " + finalKey + " is not refreshed.");
                            isRefreshed = true;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }
    }
}

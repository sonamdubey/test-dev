using Carwale.Cache.CarData;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CentralizedCacheRefresh;
using Carwale.Interfaces.NewCarFinder;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Linq;

namespace Carwale.BL.CentralizedCacheRefresh
{
    public class CentralizedCacheRefreshBl : ICentralizedCacheRefreshBl
    {
        private readonly ICarMakesCacheRepository _carMakeCache;
        private readonly ICarModelRootsCacheRepository _carModelRootCache;
        private readonly ICarModelCacheRepository _carModelCache;
        private readonly ICarVersionCacheRepository _carVersionCache;
        private readonly INewCarFinderCacheRepository _newCarFinderCacheRepo;

        public CentralizedCacheRefreshBl(ICarMakesCacheRepository carMakeCache, ICarModelRootsCacheRepository carModelRootCache,
            ICarModelCacheRepository carModelCache, ICarVersionCacheRepository carVersionCache, INewCarFinderCacheRepository newCarFinderCacheRepo)
        {
            _carMakeCache = carMakeCache;
            _carModelRootCache = carModelRootCache;
            _carModelCache = carModelCache;
            _carVersionCache = carVersionCache;
            _newCarFinderCacheRepo = newCarFinderCacheRepo;
        }
        public bool RefreshCentralizedCache(CacheRefreshWrapper refreshWrapper)
        {
            try
            {
                if (refreshWrapper.MakeAttribute.IsNotNullOrEmpty() && !_carMakeCache.RefreshCarMakeCache(refreshWrapper.MakeAttribute))
                {
                    Logger.LogError("Some Exception Occur during Make Cache Key Refresh");
                }

                if (refreshWrapper.ModelRootAttribute.IsNotNullOrEmpty() && !_carModelRootCache.RefreshCarModelRootCache(refreshWrapper.ModelRootAttribute))
                {
                    Logger.LogError("Some Exception Occur during Model Root Cache Key Refresh");
                }

                if (refreshWrapper.ModelAttribute.IsNotNullOrEmpty() && !_carModelCache.RefreshCarModelCache(refreshWrapper.ModelAttribute))
                {
                    Logger.LogError("Some Exception Occur during Model Cache Key Refresh");
                }

                if (refreshWrapper.VersionAttribute.IsNotNullOrEmpty() && !_carVersionCache.RefreshCarVersionCache(refreshWrapper.VersionAttribute))
                {
                    Logger.LogError("Some Exception Occur during Version Cache Key Refresh");
                }

                if (refreshWrapper.NewCarFinderAttribute != null && !_newCarFinderCacheRepo.RefreshCache(refreshWrapper.NewCarFinderAttribute))
                {
                    Logger.LogError("Some Exception Occur during Ncf Cache Key Refresh");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "There is some Exception occured during cache refresh at CentralizedCacheRefreshBl.cs");
                return false;
            }
        }
    }
}

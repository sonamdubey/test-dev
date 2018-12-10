using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified;
using System.Collections.Generic;

namespace Carwale.Cache.Classified
{
    public class CommonOperationsCacheRepository : ICommonOperationsCacheRepository
    {
        private readonly ICommonOperationsRepository _commonOperationsRepository;
        private readonly ICacheManager _cacheProvider;

        public CommonOperationsCacheRepository(ICommonOperationsRepository commonOperationsRepository, ICacheManager cacheProvider)
        {
            _commonOperationsRepository = commonOperationsRepository;
            _cacheProvider = cacheProvider;
        }

        public IList<DealerCityEntity> GetLiveListingCities()
        {
            return _cacheProvider.GetFromCache<IList<DealerCityEntity>>("UsedLiveListingCities", CacheRefreshTime.OneDayExpire(), () => _commonOperationsRepository.GetLiveListingCities());
        }
        public IList<CarMakeEntityBase> GetLiveListingMakes()
        {
            return _cacheProvider.GetFromCache<IList<CarMakeEntityBase>>("UsedLiveListingMakes", CacheRefreshTime.OneDayExpire(), () => _commonOperationsRepository.GetLiveListingMakes());
        }
        public CarModelMaskingResponse GetMakeDetailsByRootName(string rootName)
        {
            return _cacheProvider.GetFromCache<CarModelMaskingResponse>("used_root_"+rootName, 
                CacheRefreshTime.DefaultRefreshTime(), () => _commonOperationsRepository.GetMakeDetailsByRootName(rootName));
        }
    }
}

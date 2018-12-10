using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.PriceQuote;
using System;
using System.Collections.Generic;

namespace Carwale.Cache.PriceQuote
{
    public class PQCacheRepository : IPQCacheRepository
    {
        private readonly IPQRepository _pqRepo;
        private readonly ICacheManager _cacheProvider;
        private readonly string priceCacheKey = "PQ-V4-ModelPrices-ModelId-{0}-CityId-{1}-{2}";

        public PQCacheRepository(IPQRepository pqRepo, ICacheManager cacheProvider)
        {
            _pqRepo = pqRepo;
            _cacheProvider = cacheProvider;
        }

        public PQ GetPQ(int cityId, int versionId)
        {
            var cacheKey = "pq-v1-" + cityId + "-" + versionId;
            return _cacheProvider.GetFromCache<PQ>(cacheKey, CacheRefreshTime.NeverExpire(), () => _pqRepo.GetPQ(cityId, versionId));
        }

        public IEnumerable<ModelPrices> GetModelPrices(int modelId, int cityId)
        {
            var cacheKey = string.Format("pq-model-prices-{0}-{1}", modelId, cityId);
            return _cacheProvider.GetFromCache<List<ModelPrices>>(cacheKey, CacheRefreshTime.NeverExpire(), () => _pqRepo.GetModelPrices(modelId, cityId));
        }

        public void StoreCarVersionDetails(int modelId, int cityId, List<CarVersionEntity> versionDetails)
        {
            var cacheKey = string.Format("pq-version-details-{0}-{1}", modelId, cityId);
            _cacheProvider.StoreToCache<List<CarVersionEntity>>(cacheKey, CacheRefreshTime.DefaultRefreshTime(), versionDetails);
        }

        /// <summary>
        /// Author : Sachin Bharti on 30/06/2016
        /// Purpose : Cache model versions average price
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IDictionary<int, VersionAveragePrice> GetModelsVersionAveragePrices(int modelId, bool isNew)
        {
            var cacheKey = string.Format("pq-model-avg-prices-{0}-" + (isNew ? "new" : "discontinued") , modelId);
            return _cacheProvider.GetFromCache<IDictionary<int, VersionAveragePrice>>(cacheKey, CacheRefreshTime.OneDayExpire(), () => _pqRepo.GetModelsVersionAveragePrices(modelId, isNew, false), 
                () => _pqRepo.GetModelsVersionAveragePrices(modelId, isNew, true));
        }

        public void StoreUserPQTakenModels(string cacheKey, List<int> modelIds)
        {
            _cacheProvider.StoreToCache<List<int>>(cacheKey, CacheRefreshTime.NeverExpire(), modelIds);
        }
        /// <summary>
        /// Get average or Ex showroom price for each version of a perticular model
        /// Written By : Chetan Thambad on <20/07/2016>
        /// </summary>
        public IEnumerable<VersionPrice> GetAllVersionsPriceByModelCity(int modelId, int cityId)
        {
            return _cacheProvider.GetFromCache<IEnumerable<VersionPrice>>(String.Format("model-versions-price-details-{0}-{1}", modelId, cityId), CacheRefreshTime.NeverExpire(),
                   () => _pqRepo.GetAllVersionsPriceByModelCity(modelId, cityId));
        }

        public void ReplaceVersionsPriceByModelCity(int modelId, int cityId)
        {
            _cacheProvider.StoreToCache<IEnumerable<VersionPrice>>(String.Format("model-versions-price-details-{0}-{1}", modelId, cityId), CacheRefreshTime.NeverExpire(),
                        _pqRepo.GetAllVersionsPriceByModelCity(modelId, cityId));
        }

        public void StoreVersionPriceDetails(int versionId, int cityId, PriceOverview carPriceAvailability)
        {
            var cacheKey = string.Format("version-price-details-{0}-{1}", versionId, cityId);
            _cacheProvider.StoreToCache<PriceOverview>(cacheKey, CacheRefreshTime.EODExpire(), carPriceAvailability);
        }

        public CarPriceQuote GetModelPricesCache(int modelId, int cityId, bool isNew)
        {
            return _cacheProvider.GetFromCache<CarPriceQuote>(string.Format(priceCacheKey, modelId, cityId, isNew ? "new" : "discontinued"));
        }

        public void StoreModelPrices(int modelId, int cityId, bool isNew, CarPriceQuote priceQuote)
        {
            var cacheKey = string.Format(priceCacheKey, modelId, cityId, isNew ? "new" : "discontinued");
            _cacheProvider.StoreToCache<CarPriceQuote>(cacheKey, CacheRefreshTime.NeverExpire(), priceQuote);
        }
    }
}

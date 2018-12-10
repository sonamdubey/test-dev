using AEPLCore.Cache;
using Carwale.DAL.Classified.SellCar;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.SellCar;
using System.Configuration;
using System;
using System.Collections.Generic;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.Classified
{
    public class SellCarCacheRepository : ISellCarCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private const string _keyPrefix = "BuyingIndex_";
        private readonly ICTBuyingIndexClient _buyingIndexClient;
        private readonly ISellCarRepository _sellCarRepo;

        public SellCarCacheRepository(ICacheManager cacheProvider, ICTBuyingIndexClient buyingIndexClient, ISellCarRepository sellcarRepo)
        {
            _cacheProvider = cacheProvider;
            _buyingIndexClient = buyingIndexClient;
            _sellCarRepo = sellcarRepo;
        }

        public void StoreBuyingIndex(int inquiryId, int buyingIndex)
        {
            _cacheProvider.StoreToCache<int>(_keyPrefix + inquiryId, CacheRefreshTime.MedianCacheTime(), buyingIndex);
        }

        public int GetBuyingIndex(int inquiryId, ValuationUrlParameters valuationUrlParameters = null)
        {
            return _cacheProvider.GetFromCache<int>(_keyPrefix + inquiryId, CacheRefreshTime.MedianCacheTime(),
                () => valuationUrlParameters != null ? _buyingIndexClient.GetBuyingIndex(valuationUrlParameters) : _buyingIndexClient.GetBuyingIndex(inquiryId));
        }

        public IEnumerable<int> C2BCities()
        {
            return _cacheProvider.GetFromCache<IEnumerable<int>>("c2bcities", CacheRefreshTime.NeverExpire(), () => _sellCarRepo.C2BCities());
        }
    }
}

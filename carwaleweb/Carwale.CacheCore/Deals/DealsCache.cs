using Carwale.Interfaces.Deals.Cache;
using Carwale.Entity.Deals;
using Carwale.Interfaces;
using Carwale.Interfaces.Deals;
using AEPLCore.Cache;
using System.Collections.Generic;
using Carwale.Entity;
using Carwale.Entity.Geolocation;
using System;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.Deals
{
    public class DealsCache : IDealsCache
    {
        protected readonly ICacheManager _cacheProvider;
        protected readonly IDealsRepository _dealsRepo;
        private const string _advantageAdKey = "AdvantageAd-{0}-{1}-{2}-{3}-{4}";

        public DealsCache(ICacheManager cacheProvider, IDealsRepository dealsRepo)
        {
            _cacheProvider = cacheProvider;
            _dealsRepo = dealsRepo;
        }
        public DiscountSummary GetDealsMaxDiscount(int modelId, int cityId)
        {
            return _cacheProvider.GetFromCache<DiscountSummary>(string.Format("Deals-{0}-{1}", modelId, cityId),
                           CacheRefreshTime.NeverExpire(),
                               () => _dealsRepo.GetDealsMaxDiscount(modelId, cityId));
        }

        public DealsStock GetAdvantageAdContent(int modelId, int cityId, byte subSegmentId, int versionId, bool isVersionSpecific)
        {
            // *** Warning : do not add more parameters int this key ***
            return _cacheProvider.GetFromCache<DealsStock>(string.Format(_advantageAdKey, subSegmentId, modelId, versionId, cityId, isVersionSpecific ? "1" : "0"),
                           CacheRefreshTime.OneDayExpire(),
                               () => _dealsRepo.GetAdvantageAdContent(modelId, cityId, subSegmentId, versionId, isVersionSpecific));
        }
        public Dictionary<int, DiscountSummary> BestVersionDealsByModel(int ModelId, int CityId)
        {
            return _cacheProvider.GetFromCache<Dictionary<int, DiscountSummary>>(string.Format("Deals-{0}-{1}-Versions", ModelId, CityId),
                        CacheRefreshTime.NeverExpire(),
                        () => _dealsRepo.BestVersionDealsByModel(ModelId, CityId));
        }
        public List<MakeEntity> GetDealMakesByCity(int cityId)
        {
            return _cacheProvider.GetFromCache<List<MakeEntity>>(string.Format("Deals-MakeList-{0}", cityId),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => _dealsRepo.GetDealMakesByCity(cityId));
        }
        public List<ModelEntity> GetDealModelsByMakeAndCity(int cityId, int makeId)
        {
            return _cacheProvider.GetFromCache<List<ModelEntity>>(string.Format("Deals-ModelList-{0}-{1}", cityId, makeId),
                          CacheRefreshTime.DefaultRefreshTime(),
                              () => _dealsRepo.GetDealModelsByMakeAndCity(cityId, makeId));
        }

        public List<DealsStock> GetDealsByDiscount(int cityId, int carCount)
        {
            return _cacheProvider.GetFromCache<List<DealsStock>>(string.Format("BestDeals_v16.8.1-{0}-{1}", cityId, carCount),
                           CacheRefreshTime.NeverExpire(),
                           () => _dealsRepo.GetDealsByDiscount(cityId, carCount));
        }

        public ProductDetails GetProductDetails(int modelId, int versionId, int cityId)
        {
            return _cacheProvider.GetFromCache<ProductDetails>(string.Format("DealsDetails_v16_9_7-{0}-{1}-{2}", modelId, versionId, cityId),
                CacheRefreshTime.DefaultRefreshTime(),
                () => _dealsRepo.GetProductDetails(modelId, versionId, cityId));
        }

        public List<City> GetAdvantageCities(int modelId, int versionId, int makeId)
        {
            return _cacheProvider.GetFromCache<List<City>>(string.Format("AdvantageCities-{0}-{1}-{2}", modelId, versionId, makeId),
                new TimeSpan(3, 0, 0),
                () => _dealsRepo.GetAdavantageCities(modelId, versionId, makeId));
        }

        public List<DealsStock> GetRecommendationsBySubSegment(int modelId, int cityId)
        {
            return _cacheProvider.GetFromCache<List<DealsStock>>(string.Format("AdvRecommendations_v16.8.1-{0}-{1}", modelId, cityId),
                new TimeSpan(3, 0, 0),
                () => _dealsRepo.GetRecommendationsBySubSegment(modelId, cityId));
        }

        public List<DealsStock> GetAllVersionDeals(int modelId, int cityId)
        {
            return _cacheProvider.GetFromCache<List<DealsStock>>(string.Format("AllVersionDeals_v2-{0}-{1}", modelId, cityId),
                new TimeSpan(3, 0, 0),
                () => _dealsRepo.GetAllVersionsDeals(modelId, cityId));
        }

        public List<int> GetCitiesWithMoreModels(int minimumStockCount)
        {
            return _cacheProvider.GetFromCache<List<int>>(string.Format("AdvCitiesWithMoreModels-{0}", minimumStockCount),
                new TimeSpan(0, 30, 0),
                () => _dealsRepo.GetCitiesWithMoreModels(minimumStockCount));
        }

        public int GetCarCountByCity(int cityId)
        {
            return _cacheProvider.GetFromCache<int>(string.Format("DealsCount-{0}", cityId),
                new TimeSpan(0, 30, 0), () => _dealsRepo.GetCarCountByCity(cityId));
        }

        public DealsPriceBreakupEntity GetDealsPriceBreakUp(int stockId, int cityId)
        {
            return _cacheProvider.GetFromCache<DealsPriceBreakupEntity>(string.Format("PriceBreakUpDetails-{0}-{1}", stockId, cityId),
                new TimeSpan(0, 30, 0), () => _dealsRepo.GetDealsPriceBreakUp(stockId, cityId));
        }

        public DealsStock GetOfferOfWeekDetails(int modelId, int cityId)
        {
            return _cacheProvider.GetFromCache<DealsStock>(string.Format("OfferOfWeek-{0}-{1}", modelId, cityId),
                           CacheRefreshTime.DefaultRefreshTime(),
                               () => _dealsRepo.GetOfferOfWeekDetails(modelId, cityId));
        }

        public List<DealsTestimonialEntity> GetDealsTestimonials(int cityId = 0)
        {
            return _cacheProvider.GetFromCache<List<DealsTestimonialEntity>>(string.Format("DealsTestimonials-{0}", cityId),
                     new TimeSpan(3, 0, 0), () => _dealsRepo.GetDealsTestimonials(cityId));
        }

        public string GetDealsOfferList(string stockIds, int cityId)
        {
            return _cacheProvider.GetFromCache<string>(string.Format("DealsOfferList-{0}-{1}", stockIds, cityId),
                CacheRefreshTime.DefaultRefreshTime(), () => _dealsRepo.GetDealsOfferList(stockIds, cityId));
        }

        public DealsDealers GetDealerDetails(int dealerId)
        {
            return _cacheProvider.GetFromCache<DealsDealers>(string.Format("dealerDetails-{0}", dealerId),
                new TimeSpan(3, 0, 0), () => _dealsRepo.GetDealerDetails(dealerId));
        }

        public List<DealsStock> GetDealsSimilarCarsBySubSegment(int modelId, int cityId, int subsegmentId)
        {
            return _cacheProvider.GetFromCache<List<DealsStock>>(string.Format("DealsSimilarCarsBySubSegment-{0}-{1}-{2}", subsegmentId, modelId, cityId),
                CacheRefreshTime.OneDayExpire(), () => _dealsRepo.GetDealsSimilarCarsBySubSegment(modelId, cityId, subsegmentId));
        }

        public IEnumerable<VersionEntity> GetAdvantageVersions(int modelId, int cityId)
        {
            return _cacheProvider.GetFromCache<IEnumerable<VersionEntity>>(string.Format("AdvantageVersions-{0}-{1}", modelId, cityId),
                CacheRefreshTime.NeverExpire(), () => _dealsRepo.GetAdvantageVersions(modelId, cityId));
        }


        public IEnumerable<ColorEntity> GetAdvantageVersionColors(int versionId, int cityId)
        {
            return _cacheProvider.GetFromCache<IEnumerable<ColorEntity>>(string.Format("advantageVersionColors-{0}-{1}", versionId, cityId),
                CacheRefreshTime.NeverExpire(), () => _dealsRepo.GetAdvantageVersionColors(versionId, cityId));
        }


        public IEnumerable<int> GetAdvantageColorYears(int versionId, int colorId, int cityId)
        {
            return _cacheProvider.GetFromCache<IEnumerable<int>>(string.Format("advantageColorYears-{0}-{1}-{2}", versionId, colorId, cityId),
                CacheRefreshTime.NeverExpire(), () => _dealsRepo.GetAdvantageColorYears(versionId, colorId, cityId));
        }

        public Dictionary<string, DealsStock> GetDealsByVersionList(Dictionary<string, MultiGetCallback<DealsStock>> advantageAdCallback)
        {
            return _cacheProvider.MultiGetFromCache(advantageAdCallback);
        }
        public Dictionary<int, DealsStock> GetAdvantageAdContentV1(List<int> modelIdList, int cityId)
        {
            Dictionary<string, MultiGetCallback<DealsStock>> advantageAdCallback = GetAdvantageAdContentCallBack(modelIdList, cityId);
            var modelDeals = _cacheProvider.MultiGetFromCache(advantageAdCallback);
            Dictionary<int, DealsStock> results = new Dictionary<int, DealsStock>();
            foreach (var modelId in modelIdList)
            {
                string key = string.Format(_advantageAdKey,0, modelId,0, cityId,0);
                DealsStock deal = null;
                modelDeals.TryGetValue(key, out deal);
				if (!results.ContainsKey(modelId))
				{
					results.Add(modelId, deal);
				}
            }
            return results;
        }
        private Dictionary<string, MultiGetCallback<DealsStock>> GetAdvantageAdContentCallBack(List<int> modelIdList, int cityId)
        {
            Dictionary<string, MultiGetCallback<DealsStock>> memcacheList = new Dictionary<string, MultiGetCallback<DealsStock>>();
            string key = null;
            cityId = cityId > 0 ? cityId : 0;
            foreach (var modelId in modelIdList)
            {
                key = string.Format(_advantageAdKey, 0, modelId, 0, cityId, 0);
                MultiGetCallback<DealsStock> memcacheDeal = new MultiGetCallback<DealsStock>();
                memcacheDeal.DbCallback = () => _dealsRepo.GetAdvantageAdContent(modelId, cityId, 0, 0, false);
                memcacheDeal.CacheDuration = CacheRefreshTime.OneDayExpire();
                memcacheList.Add(key, memcacheDeal);
            }
            return memcacheList;
        }
        public int GetDealsDealerId(int stockId, int cityId)
        {
            return _cacheProvider.GetFromCache<int>(string.Format("DealsDealerId-{0}-{1}", stockId, cityId),
                CacheRefreshTime.NeverExpire(), () => _dealsRepo.GetDealsDealerId(stockId, cityId));
        }
    }
}

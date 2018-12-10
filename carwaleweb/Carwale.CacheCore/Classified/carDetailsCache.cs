using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Interfaces.Classified.CarDetail;

namespace Carwale.Cache.Classified
{
    public class CarDetailsCache : ICarDetailsCache
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IListingDetails _carDetailsRepo;
        private const string cachePrefixDealerStock = "v2_CarDetail_D";
        private const string cachePrefixIndivStock = "v2_CarDetail_S";

        public CarDetailsCache(IListingDetails carDetailsRepo , ICacheManager cacheProvider)
        {
            _carDetailsRepo = carDetailsRepo;
            _cacheProvider = cacheProvider;
        }
        public CarDetailsEntity GetDealerListingDetails(uint inquiryId)
        {
            return _cacheProvider.GetFromCache<CarDetailsEntity>(cachePrefixDealerStock + inquiryId,
                    CacheRefreshTime.OneDayExpire(), () => _carDetailsRepo.GetDealerListingDetails(inquiryId));
        }
        public CarDetailsEntity GetIndividualListingDetails(uint inquiryId)
        {
            return _cacheProvider.GetFromCache<CarDetailsEntity>(cachePrefixIndivStock + inquiryId,
                    CacheRefreshTime.OneDayExpire(), () => _carDetailsRepo.GetIndividualListingDetails(inquiryId, false), () => _carDetailsRepo.GetIndividualListingDetails(inquiryId, true));
        }

        /// <summary>
        /// Refresh the individual stock key
        /// </summary>
        /// <param name="inquiryId">InquiryId if individual stock</param>
        /// <param name="isCritical">perform cache refresh for critical read operation</param>
        public void RefreshIndividualStockKey(int inquiryId, bool isCriticalRead)
        {
            string cacheKey = string.Format("{0}{1}", cachePrefixIndivStock, inquiryId);
            if (isCriticalRead)
            {
                _cacheProvider.ExpireCacheWithCriticalRead(cacheKey);
            }
            else
            {
                _cacheProvider.ExpireCache(cacheKey);
            }
        }
    }
}

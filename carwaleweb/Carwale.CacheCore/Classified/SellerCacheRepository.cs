using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.Classified.Leads;
using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.Leads;

namespace Carwale.Cache.Classified
{
    public class SellerCacheRepository : ISellerCacheRepository
    {
        private readonly SellerRepository _sellerRepo;
        private readonly ICacheManager _cacheProvider;

        public SellerCacheRepository(SellerRepository sellerRepo, ICacheManager cacheProvider)
        {
            _sellerRepo = sellerRepo;
            _cacheProvider = cacheProvider;
        }

        public Seller GetIndividualSeller(int inquiryId)
        {
           return _cacheProvider.GetFromCache<Seller>(inquiryId.ToString(), CacheRefreshTime.OneDayExpire(), () => _sellerRepo.GetIndividualSeller(inquiryId));
        }
    }
}

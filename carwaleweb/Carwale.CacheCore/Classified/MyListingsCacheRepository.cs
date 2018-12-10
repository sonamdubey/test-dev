using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.MyListings;

namespace Carwale.Cache.Classified
{
    public class MyListingsCacheRepository : IMyListingsCacheRepository
    {
        private readonly IMyListingsRepository _myListingRepo;
        private readonly ICacheManager _cacheProvider;
        private const string _c2bLeadKey = "c2b_{0}";
        private const string _carTradeLeadKey = "ct_{0}";
        public MyListingsCacheRepository(IMyListingsRepository myListingRepo, ICacheManager cacheProvider)
        {
            _myListingRepo = myListingRepo;
            _cacheProvider = cacheProvider;
        }
        public C2BLeadResponse GetC2BLeads(int inquiryId)
        {
            return _cacheProvider.GetFromCache<C2BLeadResponse>(string.Format(_c2bLeadKey, inquiryId), CacheRefreshTime.GetInMinutes("C2BLeadExpiryTime"), () => _myListingRepo.GetC2BLeads(inquiryId));
        }
        public CarTradeLeadResponse GetCarTradeLeads(int inquiryId)
        {
            return _cacheProvider.GetFromCache<CarTradeLeadResponse>(string.Format(_carTradeLeadKey, inquiryId), CacheRefreshTime.GetInMinutes("C2BLeadExpiryTime"), () => _myListingRepo.GetCarTradeLeads(inquiryId));
        }
    }
}

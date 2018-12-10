using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.Chat;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using System.Collections.Generic;

namespace Carwale.Cache.Dealers
{
    public class DealersCache : IDealerCache
    {
        protected readonly IDealerRepository _dealersRepo;
        protected readonly ICacheManager _cacheProvider;

        public DealersCache(IDealerRepository dealersRepo, ICacheManager cacheProvider)
        {
            _dealersRepo = dealersRepo;
            _cacheProvider = cacheProvider;
        }

        public DealerDetails GetDealerDetailsOnDealerId(int dealerId)
        {
            return _cacheProvider.GetFromCache<DealerDetails>("DealerDetails_v1_" + dealerId,
                           CacheRefreshTime.NeverExpire(),
                               () => _dealersRepo.GetDealerDetailsOnDealerId(dealerId));
        }

        public List<CarModelSummary> GetDealerModelsOnMake(int makeId, int dealerId)
        {
            return _cacheProvider.GetFromCache<List<CarModelSummary>>("DealerModels_makeId" + makeId + "dealerId" + dealerId +"_v1",
                               CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                                   () => _dealersRepo.GetDealerModelsOnMake(makeId, dealerId));
        }

        public Dictionary<int,DealerDetails> MultiGetDealerDetails(List<int> dealerIds)
        {
            string dealerDetailKeyPrefix = "DealerDetails_v1_";
            Dictionary<int, DealerDetails> dealerDetails = new Dictionary<int, DealerDetails>();
            var dealerDetailsCallback = new Dictionary<string, MultiGetCallback<DealerDetails>>();

            if(dealerIds != null)
            {
                foreach (int dealerId in dealerIds)
                {
                    string key = string.Format("{0}{1}", dealerDetailKeyPrefix, dealerId);
                    if (!dealerDetailsCallback.ContainsKey(key))
                    dealerDetailsCallback.Add(key,
                                          new MultiGetCallback<DealerDetails>() { CacheDuration = CacheRefreshTime.NeverExpire(), DbCallback = () => _dealersRepo.GetDealerDetailsOnDealerId(dealerId) });
                }

                Dictionary<string, DealerDetails> dealerDetailsCache = _cacheProvider.MultiGetFromCache(dealerDetailsCallback);

                foreach (int dealerId in dealerIds)
                {
                    string cacheKey = string.Format("{0}{1}", dealerDetailKeyPrefix, dealerId);
                    if (dealerDetailsCache.ContainsKey(cacheKey) && !dealerDetails.ContainsKey(dealerId))
                    {
                        dealerDetails.Add(dealerId, dealerDetailsCache[cacheKey]);
                    }
                    else
                    {
                        dealerDetails.Add(dealerId, null);
                    }
                }
            }            

            return dealerDetails;
        }

        public DealerChatInfo GetDealerMobileFromChatToken(string chatToken)
        {
            return _cacheProvider.GetFromCache("DealerChatInfo-" + chatToken, CacheRefreshTime.OneDayExpire(), () => _dealersRepo.GetDealerMobileFromChatToken(chatToken));
        }
    }
}

using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.CrossSell;
using Carwale.Interfaces;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CrossSell;
using System;
using System.Collections.Generic;

namespace Carwale.Cache.CrossSell
{
    public class CrossSellCacheRepository : ICrossSellCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        
        public CrossSellCacheRepository(ICacheManager cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public void StoreHouseCrossSellVersions(string cacheKey, List<int> featuredVersions)
        {
            _cacheProvider.StoreToCache<List<int>>(cacheKey, CacheRefreshTime.DefaultRefreshTime(), featuredVersions);
        }

        public void StoreCrossSellCampaign(string cacheKey, List<FeaturedVersion> paidCrossSellCampaign)
        {
            _cacheProvider.StoreToCache<List<FeaturedVersion>>(cacheKey, CacheRefreshTime.DefaultRefreshTime(), paidCrossSellCampaign);
        }
    }
}

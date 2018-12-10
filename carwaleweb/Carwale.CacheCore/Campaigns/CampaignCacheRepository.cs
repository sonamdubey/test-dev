using System;
using System.Collections.Generic;
using Carwale.Entity.Campaigns;
using Carwale.Interfaces;
using Carwale.Interfaces.Campaigns;
using AEPLCore.Cache;
using Carwale.Entity.Enum;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.Campaigns
{
    public class CampaignCacheRepository : ICampaignCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private readonly ICampaignRepository _campaignRepo;

        public CampaignCacheRepository(ICacheManager cacheProvider, ICampaignRepository campaignRepo)
        {
            _cacheProvider = cacheProvider;
            _campaignRepo = campaignRepo;
        }

        public Dictionary<int, int> GetCampaignGroupTemplateIdCache(int templateGroupId, int platformId)
        {
            return _cacheProvider.GetFromCache<Dictionary<int, int>>("pq-Group-TemplateId-" + templateGroupId + "-platformId-" + platformId,
                   CacheRefreshTime.DefaultRefreshTime(),
                   () => _campaignRepo.GetCampaignTemplateGroups(templateGroupId, platformId));
        }
    }
}

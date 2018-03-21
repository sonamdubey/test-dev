using Bikewale.Interfaces.Cache.Core;
using Bikewale.ManufacturerCampaign.Interface;
using System;

namespace Bikewale.ManufacturerCampaign.Cache
{
    public class ManufacturerCampaignCache : IManufacturerCampaignCache
    {
        private readonly ICacheManager _cache;
        private readonly Interface.IManufacturerCampaignRepository _repo = null;
        public ManufacturerCampaignCache(ICacheManager cache, Interface.IManufacturerCampaignRepository repo)
        {
            _cache = cache;
            _repo = repo;
        }

        public Entities.ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, Entities.ManufacturerCampaignServingPages pageId)
        {
            string key = String.Format("BW_ES_Campaign_M_{0}_C_{1}_P_{2}", modelId, cityId, (int)pageId);
            try
            {
                Entities.ManufacturerCampaignEntity campaign = _cache.GetFromCache(key, new TimeSpan(0, 5, 0), () => _repo.GetCampaigns(modelId, cityId, pageId));
                return campaign;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "AdSlotCacheRepository.GetAdSlotStatus");
            }
            return null;

        }
    }
}

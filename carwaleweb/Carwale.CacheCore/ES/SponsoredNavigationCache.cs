using Carwale.Interfaces.ES;
using System.Collections.Generic;
using Carwale.Entity.ES;
using Carwale.Interfaces;
using System;
using Carwale.Notifications.Logs;
using AEPLCore.Cache;
using Carwale.Entity.Enum;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.ES
{
    public class SponsoredNavigationCache : ISponsoredNavigationCache
    {
        private readonly ISponsoredNavigationRepository _navigationRepo;
        protected readonly ICacheManager _cacheProvider;

        public SponsoredNavigationCache(ISponsoredNavigationRepository navigationRepo, ICacheManager cacheProvider)
        {
            _navigationRepo = navigationRepo;
            _cacheProvider = cacheProvider;
        }
        public List<SponsoredNavigation> GetSponsoredNavigationData(int sectionId, int platformId)
        {
            string key = string.Format("sponsored_navigation_{0}_{1}", sectionId, platformId);
            var cacheObj = _cacheProvider.GetFromCache<List<SponsoredNavigation>>(key);
            try
            {
                if (cacheObj == null)
                {
                    DateTime nextCampaignStartDate;
                    cacheObj = _navigationRepo.GetSponsoredNavigationData(sectionId, platformId, out nextCampaignStartDate);

                    if (cacheObj != null)//if campaign exists either sponsored or default
                    {
                        TimeSpan cacheDuration = CacheRefreshTime.DefaultRefreshTime();

                        if (cacheObj.Count > 1 && !cacheObj[1].IsDefault)
                        {
                            // for 2 sponsored campaign
                            DateTime earliestEndDate = DateTime.Compare(cacheObj[0].EndDate, cacheObj[1].EndDate) < 0 ? cacheObj[0].EndDate : cacheObj[1].EndDate;
                            cacheDuration = (DateTime.Compare(earliestEndDate, nextCampaignStartDate) < 0 ? earliestEndDate : nextCampaignStartDate) - DateTime.Now;
                        }
                        else if ((cacheObj.Count == 1 || (cacheObj.Count > 1 && cacheObj[1].IsDefault)) && cacheObj[0].EndDate != null)
                        {    
                            // 1 sponsored campaign or 1 sponsored 1 default campaign
                            cacheDuration = (DateTime.Compare(cacheObj[0].EndDate, nextCampaignStartDate) < 0 ? cacheObj[0].EndDate : nextCampaignStartDate) - DateTime.Now;
                        }
                        else if (cacheObj.Count < 1 || (cacheObj.Count == 1 && cacheObj[0].IsDefault) || (cacheObj.Count == 1 && platformId == (int)Platform.CarwaleMobile))
                        {
                            cacheDuration = (nextCampaignStartDate - DateTime.Now);
                        }

                        return _cacheProvider.GetFromCache(key, cacheDuration, () => cacheObj);
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return cacheObj;
       }
    }
}

using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Comparison.Cache
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Jul 2017
    /// Description :   Sponsored Comparison Cache Repository
    /// </summary>
    public class SponsoredComparisonCacheRepository : ISponsoredComparisonCacheRepository
    {
        private readonly ISponsoredComparisonRepository _repository = null;
        private readonly ICacheManager _cache = null;
        public SponsoredComparisonCacheRepository(ISponsoredComparisonRepository repository, ICacheManager cache)
        {
            _repository = repository;
            _cache = cache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Jul 2017
        /// Description :   returns All active sponsored comparisons
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ActiveSponsoredCampaign> GetActiveSponsoredComparisons()
        {
            string key = "BW_SponsoredCampaigns";
            IEnumerable<ActiveSponsoredCampaign> activeComparisons = null;
            try
            {
                activeComparisons = _cache.GetFromCache<IEnumerable<ActiveSponsoredCampaign>>(key, new TimeSpan(0, 30, 0), () => _repository.GetActiveSponsoredComparisons());

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SponsoredComparisonCacheRepository.GetActiveSponsoredComparisons");
            }
            return activeComparisons;
        }
    }
}

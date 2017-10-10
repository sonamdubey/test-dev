﻿using Bikewale.Comparison.Entities;
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
        /// Modified by :   Vivek Singh Tomar on 26th Sep 2017
        /// Description :   Versioning of memcache key
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SponsoredVersionEntityBase> GetActiveSponsoredComparisons()
        {
            string key = "BW_SponsoredComparisons_v1";
            IEnumerable<SponsoredVersionEntityBase> activeComparisons = null;
            try
            {
                activeComparisons = _cache.GetFromCache<IEnumerable<SponsoredVersionEntityBase>>(key, new TimeSpan(24, 0, 0), () => _repository.GetActiveSponsoredComparisons());

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SponsoredComparisonCacheRepository.GetActiveSponsoredComparisons");
            }
            return activeComparisons;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 07 Aug 2017
        /// Description :   Refresh Sponsored Comparison Cache
        /// Modified by :   Vivek Singh Tomar on 26th Sep 2017
        /// Description :   Versioning of memcache key
        /// </summary>
        public void RefreshSpsonsoredComparisonsCache()
        {
            string key = "BW_SponsoredComparisons_v1";
            try
            {
                _cache.RefreshCache(key);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SponsoredComparisonCacheRepository.RefreshSpsonsoredComparisonsCache");
            }
        }
    }
}

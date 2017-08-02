using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Comparison.BAL
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Aug 2017
    /// Description :   Sponsored Comparison Business Layer
    /// </summary>
    public class SponsoredComparison : ISponsoredComparison
    {
        private readonly ISponsoredComparisonCacheRepository _cache = null;
        public SponsoredComparison(ISponsoredComparisonCacheRepository cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Aug 2017
        /// Description :   Returns Sponsored Version and it's details
        /// </summary>
        /// <param name="targetVersionIds">CSV version ids</param>
        /// <returns></returns>
        public SponsoredVersionEntityBase GetSponsoredVersion(string targetVersionIds)
        {
            SponsoredVersionEntityBase sponsoredVersion = null;
            try
            {
                var versions = _cache.GetActiveSponsoredComparisons();
                if (versions != null && !String.IsNullOrEmpty(targetVersionIds))
                {
                    IEnumerable<uint> targets = targetVersionIds.Split(',').Select(v => uint.Parse(v));
                    sponsoredVersion = versions.FirstOrDefault(m => targets.Contains(m.TargetVersionId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("Bikewale.Comparison.BAL.SponsoredComparison.GetSponsoredVersion({0})", targetVersionIds));
            }
            return sponsoredVersion;
        }
    }
}

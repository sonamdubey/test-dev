using Bikewale.Comparison.Entities;
using System.Collections.Generic;

namespace Bikewale.Comparison.Interface
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Jul 2017
    /// Description :   Sponsored Comparison Cache Repository
    /// </summary>
    public interface ISponsoredComparisonCacheRepository
    {
        IEnumerable<SponsoredVersionEntityBase> GetActiveSponsoredComparisons();
        void RefreshSpsonsoredComparisonsCache();
    }
}

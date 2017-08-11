using Bikewale.Comparison.Entities;

namespace Bikewale.Comparison.Interface
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Aug 2017
    /// Description :   Sponsored Comparison Interface for Business Layer
    /// </summary>
    public interface ISponsoredComparison
    {
        SponsoredVersionEntityBase GetSponsoredVersion(string targetVersionIds);
    }
}

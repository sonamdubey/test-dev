using Bikewale.Comparison.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.Comparison
{
    public interface ISponsoredComparison
    {
        SponsoredComparison GetSponsoredComparison();
        IEnumerable<SponsoredComparison> GetSponsoredComparisons(SponsoredComparisonStatus status);
        bool SaveSponsoredComparison(SponsoredComparison campaign);
        bool SaveSponsoredComparisonBikeRules();
        TargetSponsoredMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint sponsoredModelId);
        dynamic GetSponsoredComparisonSponsoredBike(uint camparisonId); //- Returns sponsored model- version and their target bikes
        bool DeleteSponsoredComparisonBikeAllRules(uint camparisonId);
        bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint camparisonId, uint SponsoredmodelId);
        bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint camparisonId, uint SponsoredversionId);
        bool DeleteSponsoredComparisonBikeTargetVersionRules(uint camparisonId, uint targetversionId);

    }
}

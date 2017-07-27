using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Comparison.Entities;

namespace BikewaleOpr.Interface.Comparison
{
    public interface ISponsoredComparison
    {
        SponsoredCampaign GetSponsoredComparison();
        IEnumerable<SponsoredCampaign> GetSponsoredComparisons(SponsoredCampaignStatus status);
        bool SaveSponsoredComparison(SponsoredCampaign campaign);
        bool SaveSponsoredComparisonBikeRules();
        SponsoredVersionMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint sponsoredModelId);
        TargetedModel GetSponsoredComparisonSponsoredBike(uint camparisonId); //- Returns sponsored model- version and their target bikes
        bool DeleteSponsoredComparisonBikeAllRules(uint camparisonId);
        bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint camparisonId, uint SponsoredmodelId);
        bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint camparisonId, uint SponsoredversionId);
        bool DeleteSponsoredComparisonBikeTargetVersionRules(uint camparisonId, uint targetversionId);

    }
}

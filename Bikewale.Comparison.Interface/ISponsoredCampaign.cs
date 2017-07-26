using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Comparison.Entities;

namespace Bikewale.Comparison.Interface
{
    public interface ISponsoredCampaign
    {
        IEnumerable<SponsoredCampaign> GetSponsoredComparison();
        SponsoredCampaign GetSponsoredComparisons(SponsoredCampaignStatus status);
        bool saveSponsoredComparisonBikeRules(SponsoredCampaign campaign);
        bool SaveSponsoredComparisonBikecityRules(uint camparisonId, bool isAllIndia, string cityIds);
        SponsoredVersionMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint sponsoredModelId);
        
        TargetedModel GetSponsoredComparisonSponsoredBike(uint camparisonId); //- Returns sponsored model- version and their target bikes
        IEnumerable<City> GetSponsoredComparisonTargetVersionLocation(uint camparisonId, string targetVersionId);

        bool DeleteSponsoredComparisonBikeAllRules(uint camparisonId);
        bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint camparisonId, uint SponsoredmodelId);
        bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint camparisonId, uint SponsoredversionId);
        bool DeleteSponsoredComparisonBiketargetVersionRules(uint camparisonId, uint targetversionId);
        bool DeleteSponsoredComparisonBikeStateRules(uint camparisonId, uint ruleId, uint stateId);
        bool DeleteSponsoredComparisonBikecityRules(uint camparisonId, uint ruleId, uint cityId);
    }
}

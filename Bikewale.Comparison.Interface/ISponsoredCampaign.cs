using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Comparison.Entities;

namespace Bikewale.Comparison.Interface
{
    /// <summary>
    /// Modified by :- Sangram Nandkhile on 26 july 2017
    /// summary :- Get Sponsored Campaign
    /// </summary>
    /// <returns></returns>
    public interface ISponsoredCampaignRepository
    {
        SponsoredCampaign GetSponsoredComparison();
        IEnumerable<SponsoredCampaign> GetSponsoredComparisons(string statuses);
        bool SaveSponsoredComparison(SponsoredCampaign campaign);
        bool SaveSponsoredComparisonBikeRules(VersionTargetMapping rules);
        TargetSponsoredMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint targetModelId, uint sponsoredModelId);
        dynamic GetSponsoredComparisonSponsoredBike(uint camparisonId); //- Returns sponsored model- version and their target bikes
        bool DeleteSponsoredComparisonBikeAllRules(uint camparisonId);
        bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint camparisonId, uint SponsoredmodelId);
        bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint camparisonId, uint SponsoredversionId);
        bool DeleteSponsoredComparisonBikeTargetVersionRules(uint camparisonId, uint targetversionId);
        //bool DeleteSponsoredComparisonBikeStateRules(uint camparisonId, uint ruleId, uint stateId);
        //bool DeleteSponsoredComparisonBikecityRules(uint camparisonId, uint ruleId, uint cityId);
        //IEnumerable<City> GetSponsoredComparisonTargetVersionLocation(uint camparisonId, string targetVersionId);
        //bool SaveSponsoredComparisonBikecityRules(uint camparisonId, bool isAllIndia, string cityIds);
    }
}

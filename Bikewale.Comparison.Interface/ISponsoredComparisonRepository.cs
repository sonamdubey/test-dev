using Bikewale.Comparison.Entities;
using System.Collections.Generic;

namespace Bikewale.Comparison.Interface
{
    /// <summary>
    /// Modified by :- Sangram Nandkhile on 26 july 2017
    /// summary :- Get Sponsored Campaign
    /// </summary>
    /// <returns></returns>
    public interface ISponsoredComparisonRepository
    {
        SponsoredComparison GetSponsoredComparison();
        IEnumerable<SponsoredComparison> GetSponsoredComparisons(string statuses);
        uint SaveSponsoredComparison(SponsoredComparison campaign);
        bool SaveSponsoredComparisonBikeRules(VersionTargetMapping rules);
        TargetSponsoredMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint targetModelId, uint sponsoredModelId);
        dynamic GetSponsoredComparisonSponsoredBike(uint camparisonId); //- Returns sponsored model- version and their target bikes
        bool DeleteSponsoredComparisonBikeAllRules(uint camparisonId);
        bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint camparisonId, uint SponsoredmodelId);
        bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint camparisonId, uint SponsoredversionId);
        bool DeleteSponsoredComparisonBikeTargetVersionRules(uint camparisonId, uint targetversionId);
        bool ChangeSponsoredComparisonStatus(uint camparisonId, ushort status);
        IEnumerable<SponsoredVersionEntityBase> GetActiveSponsoredComparisons();
        //bool DeleteSponsoredComparisonBikeStateRules(uint camparisonId, uint ruleId, uint stateId);
        //bool DeleteSponsoredComparisonBikecityRules(uint camparisonId, uint ruleId, uint cityId);
        //IEnumerable<City> GetSponsoredComparisonTargetVersionLocation(uint camparisonId, string targetVersionId);
        //bool SaveSponsoredComparisonBikecityRules(uint camparisonId, bool isAllIndia, string cityIds);
    }
}

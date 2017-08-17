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
        TargetSponsoredMapping GetSponsoredComparisonVersionMapping(uint comparisonid, uint targetModelId, uint sponsoredModelId);
        IEnumerable<SponsoredVersion> GetSponsoredComparisonSponsoredBike(uint comparisonid); //- Returns sponsored model- version and their target bikes
        bool DeleteSponsoredComparisonBikeAllRules(uint comparisonid);
        bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint comparisonid, uint SponsoredmodelId);
        bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint comparisonid, uint SponsoredversionId);
        bool DeleteSponsoredComparisonBikeTargetVersionRules(uint comparisonid, uint targetversionId);
        bool ChangeSponsoredComparisonStatus(uint comparisonid, ushort status);
        IEnumerable<SponsoredVersionEntityBase> GetActiveSponsoredComparisons();

        //bool DeleteSponsoredComparisonBikeStateRules(uint comparisonid, uint ruleId, uint stateId);
        //bool DeleteSponsoredComparisonBikecityRules(uint comparisonid, uint ruleId, uint cityId);
        //IEnumerable<City> GetSponsoredComparisonTargetVersionLocation(uint comparisonid, string targetVersionId);
        //bool SaveSponsoredComparisonBikecityRules(uint comparisonid, bool isAllIndia, string cityIds);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Comparison.Entities;
using Bikewale.Comparison.Interface;

namespace Bikewale.Comparison.DAL
{
    public class SponsoredCampaignRepository : ISponsoredCampaign
    {
        public bool DeleteSponsoredComparisonBikeAllRules(uint camparisonId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSponsoredComparisonBikecityRules(uint camparisonId, uint ruleId, uint cityId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSponsoredComparisonBikeSponsoredModelRules(uint camparisonId, uint SponsoredmodelId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSponsoredComparisonBikeSponsoredVersionRules(uint camparisonId, uint SponsoredversionId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSponsoredComparisonBikeStateRules(uint camparisonId, uint ruleId, uint stateId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSponsoredComparisonBiketargetVersionRules(uint camparisonId, uint targetversionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SponsoredCampaign> GetSponsoredComparison()
        {
            throw new NotImplementedException();
        }

        public SponsoredCampaign GetSponsoredComparisons(SponsoredCampaignStatus status)
        {
            throw new NotImplementedException();
        }

        public TargetedModel GetSponsoredComparisonSponsoredBike(uint camparisonId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<City> GetSponsoredComparisonTargetVersionLocation(uint camparisonId, string targetVersionId)
        {
            throw new NotImplementedException();
        }

        public SponsoredVersionMapping GetSponsoredComparisonVersionMapping(uint camparisonId, uint sponsoredModelId)
        {
            throw new NotImplementedException();
        }

        public bool SaveSponsoredComparisonBikecityRules(uint camparisonId, bool isAllIndia, string cityIds)
        {
            throw new NotImplementedException();
        }

        public bool saveSponsoredComparisonBikeRules(SponsoredCampaign campaign)
        {
            throw new NotImplementedException();
        }
    }
}

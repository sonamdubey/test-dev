using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 28 June 2017
    /// Summary    : Wrapper for manufacturer campaign rules
    /// </summary>
    public class ManufacturerCampaignRulesWrapper
    {
        public IEnumerable<ManufacturerRuleEntity> ManufacturerCampaignRules { get; set; }
        public bool ShowOnExShowroom { get; set; }
    }
}

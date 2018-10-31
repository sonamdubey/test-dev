using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.DTO.Campaign
{
    public class ManufacturerCampaignDto
    {
        [JsonProperty("leadCampaign")]
        public ManufacturerLeadCampaignDto LeadCampaign { get; set; }
        [JsonProperty("emiCampaign")]
        public ManufacturerEmiCampaignDto EmiCampaign { get; set; }
    }
}
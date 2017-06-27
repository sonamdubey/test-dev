using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.Entities
{
    public class ManufacturerRuleEntityDTO
    {
        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("stateId")]
        public uint StateId { get; set; }

        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("userId")]
        public uint UserId { get; set; }

        [JsonProperty("isAllIndia")]
        public bool IsAllIndia { get; set; }
    }
}

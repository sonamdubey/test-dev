using Newtonsoft.Json;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 22 Jun 2017
    /// Summary    : DTO for Manufacturer Camapign Rules
    /// </summary>
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

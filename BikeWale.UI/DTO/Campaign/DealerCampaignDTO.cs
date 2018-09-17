using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    /// <summary>
    /// Author  : Kartik Rathod on 12 sept 2018
    /// Desc    : get campaign details for Ds  campaings related to lead popup
    /// </summary>
    public class DealerCampaignDto
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("area")]
        public string Area { get; set; }
        [JsonProperty("areaId")]
        public uint AreaId { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }
    }
}

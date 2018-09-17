using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Created by  : Pratibha Verma on 26 June 2018
    /// Description : changed PQId data type and added LeadId
    /// Modified by : Rajan Chauhan on 27 July 2018
    /// Description : Added AreaId
    /// </summary>
    public class PQCustomerDetailInput
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("pqId")]
        public string PQId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty("clientIP")]
        public string ClientIP { get; set; }
        [JsonProperty("pageUrl")]
        public string PageUrl { get; set; }
        [JsonProperty("versionId")]
        public string VersionId { get; set; }
        [JsonProperty("areaId")]
        public string AreaId { get; set; }
        [JsonProperty("cityId")]
        public string CityId { get; set; }
        [JsonProperty("leadSourceId")]
        public ushort? LeadSourceId { get; set; }
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("leadId")]
        public uint LeadId { get; set; }
        [JsonProperty("platformId")]
        public ushort PlatformId { get; set; }
        [JsonProperty("pageId")]
        public ushort PageId { get; set; }
        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }
    }
}
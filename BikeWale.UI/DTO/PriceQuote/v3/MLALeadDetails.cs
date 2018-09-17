using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Created by  : Pratibha Verma on 24 July 2018
    /// Description : mla lead entity
    /// </summary>
    public class MLALeadDetails
    {
        [JsonProperty("dealerIds")]
        public IEnumerable<uint> DealerIds { get; set; }
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
        [JsonProperty("cityId")]
        public string CityId { get; set; }
        [JsonProperty("leadSourceId")]
        public ushort? LeadSourceId { get; set; }
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("cityName")]
        public string CityName { get; set; }
        [JsonProperty("areaId")]
        public string AreaId { get; set; }
        [JsonProperty("areaName")]
        public string AreaName { get; set; }
        [JsonProperty("platformId")]
        public ushort PlatformId { get; set; }
        [JsonProperty("pageId")]
        public ushort PageId { get; set; }
        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }
    }
}
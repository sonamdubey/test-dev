
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.MaskingNumber
{
    /// <summary>
    /// Created by  : Pratibha Verma on 14 October 2018
    /// Description : masking number lead input entity
    /// </summary>
    public class MaskingNumberLeadInputDto
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }
        [JsonProperty("comments")]
        public string Comments { get; set; }
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("inquirySource")]
        public ushort InquirySource{get; set;}
        [JsonProperty("others")]
        public IDictionary<string, string> Others { get; set; }
    }
}
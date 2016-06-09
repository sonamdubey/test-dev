using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Price Quote Customer Details Input DTO version 2
    /// Author  :   Sumit Kate
    /// Date    :   23 May 2016    
    /// </summary>
    public class PQCustomerDetailInput
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
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
        //Added By  : Sadhana Upadhyay on 29 Dec 2015
        [JsonProperty("leadSourceId")]
        public ushort? LeadSourceId { get; set; }
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
    }
}


using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created by  : Pratibha Verma on 26 June 2018
    /// Description : changes PQId data type and Added LeadId
    /// </summary>
    public class UpdatePQCustomerInput
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
        [JsonProperty("cityId")]
        public string CityId { get; set; }
        [JsonProperty("colorId")]
        public uint ColorId { get; set; }
        //Added By  : Sadhana Upadhyay on 29 Dec 2015
        [JsonProperty("leadSourceId")]
        public ushort? LeadSourceId { get; set; }
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("leadId")]
        public uint LeadId { get; set; }
        [JsonProperty("areaId")]
        public uint AreaId { get; set; }
    }
}


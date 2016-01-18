
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote Customer Details Input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class UpdatePQCustomerInput
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("pqId")]
        public uint PQId { get; set; }
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
    }
}


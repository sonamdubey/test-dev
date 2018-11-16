using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created by  : Pratibha Verma on 16 October 2018
    /// Description : new version of PQMobileVerificationInput
    /// </summary>
    public class PQMobileVerificationInput
    {
        [JsonProperty("pqId")]
        public string PQId { get; set; }
        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty("cwiCode")]
        public string CwiCode { get; set; }
        [JsonProperty("branchId")]
        public UInt32 BranchId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("versionId")]
        public UInt32 VersionId { get; set; }
        [JsonProperty("cityId")]
        public UInt32 CityId { get; set; }
        [JsonProperty("leadId")]
        public uint LeadId { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price Quote Mobile verification input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQMobileVerificationInput
    {
        [JsonProperty("pqId")]
        public UInt32 PQId { get; set; }
        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty("cwiCode")]
        public string CwiCode { get; set; }
        [JsonProperty("branchId")]
        public string BranchId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("versionId")]
        public string VersionId { get; set; }
        [JsonProperty("cityId")]
        public string CityId { get; set; }
    }
}

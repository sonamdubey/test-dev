using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.MobileVerification
{
    /// <summary>
    /// Mobile Verification resend input entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQResendMobileVerificationInput
    {
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
    }
}

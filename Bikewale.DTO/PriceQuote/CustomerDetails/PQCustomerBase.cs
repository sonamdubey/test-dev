using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.CustomerDetails
{
    /// <summary>
    /// Price Quote Customer details
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class PQCustomerBase
    {
        [JsonProperty("customerId")]
        public ulong CustomerId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }
    }
}

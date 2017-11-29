using Bikewale.DTO.Area;
using Bikewale.DTO.City;
using Newtonsoft.Json;

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
        [JsonProperty("city")]
        public CityBase cityDetails { get; set; }
        [JsonProperty("area")]
        public AreaBase AreaEntityBase { get; set; }
    }
}

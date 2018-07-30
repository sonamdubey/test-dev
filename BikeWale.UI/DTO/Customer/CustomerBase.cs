using Newtonsoft.Json;

namespace Bikewale.DTO.Customer
{
    public class CustomerBase
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

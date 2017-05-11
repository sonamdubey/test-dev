
using Newtonsoft.Json;
namespace Bikewale.DTO
{
    public class ManufactureDealerDTO
    {
        [JsonProperty("dealerName")]
        public string DealerName { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}

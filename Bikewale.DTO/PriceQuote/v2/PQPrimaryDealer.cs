using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v2
{
    public class PQPrimaryDealer
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("package")]
        public DealerPackageTypes DealerType { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }

    public enum DealerPackageTypes
    {
        Invalid = 0,
        Standard = 1,
        Deluxe = 2,
        Premium = 3
    }
}

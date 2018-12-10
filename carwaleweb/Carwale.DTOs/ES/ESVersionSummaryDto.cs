using Newtonsoft.Json;

namespace Carwale.DTOs.ES
{
    public class ESVersionSummaryDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("transmission")]
        public string Transmission { get; set; }
    }
}

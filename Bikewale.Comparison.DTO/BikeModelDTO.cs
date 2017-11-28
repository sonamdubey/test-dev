using Newtonsoft.Json;

namespace Bikewale.Comparison.DTO
{
    public class BikeModelDTO
    {
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }

        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        [JsonProperty("price")]
        public uint Price { get; set; }
    }
}

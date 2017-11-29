using Newtonsoft.Json;

namespace Bikewale.Comparison.DTO
{
    public class BikeModelVersionMappingDTO: BikeModelDTO
    {
        [JsonProperty("sponsoredModelId")]
        public uint SponsoredModelId { get; set; }

        [JsonProperty("sponsoredVersionId")]
        public uint SponsoredVersionId { get; set; }

        [JsonProperty("impressionUrl")]
        public string ImpressionUrl { get; set; }
    }
}

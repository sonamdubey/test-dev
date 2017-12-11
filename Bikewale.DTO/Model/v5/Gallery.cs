using Newtonsoft.Json;

namespace Bikewale.DTO.Model.v5
{
    public class Gallery
    {
        [JsonProperty("imageCount")]
        public uint ImageCount { get; set; }
        [JsonProperty("videoCount")]
        public uint VideoCount { get; set; }

        [JsonProperty("colorCount")]
        public uint ColorCount { get; set; }
    }
}

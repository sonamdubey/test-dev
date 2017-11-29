using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    public class ModelDescription
    {
        [JsonProperty("smallDescription")]
        public string SmallDescription { get; set; }

        [JsonProperty("fullDescription")]
        public string FullDescription { get; set; }
    }
}

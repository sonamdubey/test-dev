using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumbItem
    {
        [JsonProperty("@type")]
        public string Type { get { return "Thing"; } }

        [JsonProperty("@id", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

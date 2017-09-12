using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class BreadcrumItem
    {
        [JsonProperty("@type")]
        public string Type { get { return "Thing"; } }

        [JsonProperty("@id")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

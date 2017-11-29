using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class WebSite
    {
        [JsonProperty("@type")]
        public string Type { get { return "WebSite"; } }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("potentialAction", NullValueHandling = NullValueHandling.Ignore)]
        public PotentialAction PotentialAction { get; set; }

    }
}

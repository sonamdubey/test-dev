using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class MainEntityOfPage
    {
        [JsonProperty("@type")]
        public string Type { get { return "WebPage"; } }

        [JsonProperty("@id", NullValueHandling = NullValueHandling.Ignore)]
        public string PageUrlId { get; set; }

    }
}

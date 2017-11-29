using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class Author
    {
        [JsonProperty("@type")]
        public string Type { get { return "Person"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}

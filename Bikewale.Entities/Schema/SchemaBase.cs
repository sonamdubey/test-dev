using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class SchemaBase
    {
        [JsonProperty("@context")]
        public string Context { get { return "http://schema.org"; } }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }

}

using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Aug-2017
    /// </summary>
    public class Brand
    {
        [JsonProperty("@type")]
        public string Type { get { return "Brand"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

    }
}

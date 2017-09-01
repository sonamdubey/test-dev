using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Aug-2017
    /// </summary>
    public class Brand
    {
        [JsonProperty("@type")]
        public string Type { get { return "Thing"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}

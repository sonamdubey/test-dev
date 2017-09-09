using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Aug-2017
    /// </summary>
    public class Manufacturer
    {
        [JsonProperty("@type")]
        public string Type { get { return "Organization"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

}

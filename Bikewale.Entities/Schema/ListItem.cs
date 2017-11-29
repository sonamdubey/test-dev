using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To create a list of items ( if item is used in list then url is null then
    ///                 it would contain a link to other product or thing)
    /// </summary>
    public class ListItem
    {
        [JsonProperty("@type")]
        public string Type { get { return "ListItem"; } }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Position { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

}


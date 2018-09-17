using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.Version
{

    /// <summary>
    /// Created By :  Pratibha Verma on 29 August 2018
    /// Description : Version Price
    /// </summary>
    public class PriceInfo
    {
        [JsonProperty("value")]
        public uint Price { get; set; }
        [JsonProperty("type")]
        public uint Type { get; set; }
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("name")]
        public string Category { get; set; }
    }
}

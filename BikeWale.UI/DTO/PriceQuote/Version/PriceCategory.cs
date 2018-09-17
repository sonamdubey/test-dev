using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.Version
{

    /// <summary>
    /// Created By :  Pratibha Verma on 29 August 2018
    /// Description : Version Price Category
    /// </summary>
    public class PriceCategory
    {
        [JsonProperty("value")]
        public uint Price { get; set; }
        [JsonProperty("name")]
        public string Category { get; set; }
    }
}

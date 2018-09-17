using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.Version
{

    /// <summary>
    /// Created By :  Pratibha Verma on 29 August 2018
    /// Description : Version Price
    /// </summary>
    public class VersionPrice : VersionPriceBase
    {
        [JsonProperty("exshowroom")]
        public uint Exshowroom { get; set; }
        [JsonProperty("rto")]
        public uint RTO { get; set; }
        [JsonProperty("insurance")]
        public uint Insurance { get; set; }
    }
}

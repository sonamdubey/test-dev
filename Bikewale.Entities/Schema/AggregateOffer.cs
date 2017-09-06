using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Aug-2017
    /// Summary: AggregateOffer Schema
    /// </summary>
    public class AggregateOffer
    {
        [JsonProperty("@type")]
        public string Type { get { return "AggregateOffer"; } }

        [JsonProperty("priceCurrency", NullValueHandling = NullValueHandling.Ignore)]
        public string PriceCurrency { get { return "INR"; } }

        [JsonProperty("lowPrice", NullValueHandling = NullValueHandling.Ignore)]
        public uint? LowPrice { get; set; }

        [JsonProperty("highPrice", NullValueHandling = NullValueHandling.Ignore)]
        public uint? HighPrice { get; set; }

    }
}

using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To add offer properties to product or a thing (mainly used for pricing purposes)
    /// </summary>
    public class Offer
    {
        /* Commnted by Sangram on 31 Aug 2017, Use AgreegateOffer instead*/

        [JsonProperty("@type")]
        public string Type { get { return "Offer"; } }

        [JsonProperty("availability", NullValueHandling = NullValueHandling.Ignore)]
        public string Availability { get; set; }

        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public double Price { get; set; }

        [JsonProperty("priceCurrency")]
        public string PriceCurrency { get { return "INR"; } }

    }
}

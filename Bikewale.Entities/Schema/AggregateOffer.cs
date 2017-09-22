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

        [JsonProperty("availability", NullValueHandling = NullValueHandling.Ignore)]
        public string Availability { get; set; }

    }

    public static class OfferAvailability
    {
        public const string _InStock = "http://schema.org/InStock";
        public const string _InStoreOnly = "http://schema.org/InStoreOnly";
        public const string _Discontinued = "http://schema.org/Discontinued";
        public const string _OutOfStock = "http://schema.org/OutOfStock";
        public const string _SoldOut = "http://schema.org/SoldOut";
        public const string _PreSale = "http://schema.org/PreSale";
        public const string _PreOrder = "http://schema.org/PreOrder";
        public const string _LimitedAvailability = "http://schema.org/LimitedAvailability";
        public const string _OnlineOnly = "http://schema.org/OnlineOnly";
    }
}

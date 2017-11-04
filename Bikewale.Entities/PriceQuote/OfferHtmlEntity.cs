using Newtonsoft.Json;

namespace Bikewale.Entities.PriceQuote
{
    public class OfferHtmlEntity
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
}

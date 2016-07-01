
using Newtonsoft.Json;
namespace Bikewale.Entities.AutoBiz
{
    /// <summary>
    /// Entity Class for Get terms for Offer
    /// </summary>
    public class OfferHtmlEntity
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
}

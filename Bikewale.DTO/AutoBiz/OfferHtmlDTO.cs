using Newtonsoft.Json;

namespace Bikewale.DTO.AutoBiz
{
    /// <summary>
    /// Entity Class for Get terms for Offer
    /// </summary>
    public class OfferHtmlEntityDTO
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
}

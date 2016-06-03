using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v2
{
    public class DPQOffer
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("offerCategoryId")]
        public int OfferCategoryId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

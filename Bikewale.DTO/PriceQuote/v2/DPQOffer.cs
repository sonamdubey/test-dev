using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New DPQOffer version for api/dealerversionprices and api/v2/onroadprice
    /// </summary>
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

using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed Dealer Quotation Offer base
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQOfferBase
    {
        [JsonProperty("model")]
        public DDQModelBase objModel { get; set; }
        [JsonProperty("city")]
        public DDQCityBase objCity { get; set; }
        [JsonProperty("dealer")]
        public DDQNewBikeDealer objDealer { get; set; }

        [JsonProperty("offerId")]
        public UInt32 OfferId { get; set; }

        [JsonProperty("offerCategoryId")]
        public UInt32 OfferCategoryId { get; set; }

        [JsonProperty("offerType")]
        public string OfferType { get; set; }

        [JsonProperty("offerText")]
        public string OfferText { get; set; }

        [JsonProperty("offerValue")]
        public UInt32 OfferValue { get; set; }
    }
}

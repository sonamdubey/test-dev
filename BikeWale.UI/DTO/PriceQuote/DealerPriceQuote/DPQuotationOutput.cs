using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer price quote output entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// 
    /// Modified By : Sumit Kate
    /// Date        : 08 Oct 2015
    /// Description : Added PQ_BikeVarient List to send the quotation for other available varients
    /// </summary>
    public class DPQuotationOutput
    {
        [JsonProperty("priceList")]
        public List<DPQ_Price> PriceList { get; set; }
        [JsonProperty("discountedPriceList")]
        public List<DPQ_Price> DiscountedPriceList { get; set; }
        [JsonProperty("disclaimers")]
        public List<string> Disclaimer { get; set; }
        [JsonProperty("offers")]
        public List<DPQOfferBase> objOffers { get; set; }
        [JsonProperty("make")]
        public DPQMakeBase objMake { get; set; }
        [JsonProperty("model")]
        public DPQModelBase objModel { get; set; }
        [JsonProperty("version")]
        public DPQVersionBase objVersion { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("imagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("varients")]
        public IEnumerable<DPQ_BikeVarient> Varients { get; set; }

    }
}

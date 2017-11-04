using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed Dealer quotation base
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQQuotationBase
    {
        [JsonProperty("priceList")]
        public List<DDQPQ_Price> PriceList { get; set; }
        [JsonProperty("disclaimers")]
        public List<string> Disclaimer { get; set; }
        [JsonProperty("offers")]
        public List<DDQOfferBase> objOffers { get; set; }
        [JsonProperty("make")]
        public DDQMakeBase objMake { get; set; }
        [JsonProperty("model")]
        public DDQModelBase objModel { get; set; }
        [JsonProperty("version")]
        public DDQVersionBase objVersion { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("imagePath")]
        public string OriginalImagePath { get; set; }
    }
}

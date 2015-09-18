using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer price quote output entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class DPQuotationOutput
    {
        [JsonProperty("priceList")]
        public List<DPQ_Price> PriceList { get; set; }
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
    }
}

using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Dealer
{
    /// <summary>
    /// Craeted by  :   Sumit Kate on 27 Dec 2017
    /// Description :   Dealer version price dto
    /// </summary>
    public class DealerVersionPricesDTO
    {
        [JsonProperty("priceList")]
        public IEnumerable<DPQ_Price> PriceList { get; set; }
        [JsonProperty("onRoadPrice")]
        public uint OnRoadPrice { get; set; }
    }
}

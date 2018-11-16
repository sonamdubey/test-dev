using Bikewale.DTO.PriceQuote.v2;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v4
{

    /// <summary>
    /// Created By  : Pratibha Verma on 19 October 2018
    /// Description : new version from Bikewale.DTO.PriceQuote.v2.DPQuotationOutput
    /// </summary>
    public class DPQuotationOutput
    {
        [JsonProperty("benefits")]
        public IEnumerable<DPQBenefit> Benefits { get; set; }
        [JsonProperty("emi")]
        public EMI emi { get; set; }
        [JsonProperty("offers")]
        public IEnumerable<DPQOffer> Offers { get; set; }
        [JsonProperty("dealer")]
        public DTO.PriceQuote.v2.PQPrimaryDealer Dealer { get; set; }
        [JsonProperty("version")]
        public IEnumerable<DPQVersionBase> Versions { get; set; }
        [JsonProperty("quoteId")]
        public string PriceQuoteId { get; set; }
    }
}

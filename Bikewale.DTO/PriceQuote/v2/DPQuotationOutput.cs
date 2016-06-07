using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v2
{

    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 3rd June 2016
    /// Description : New DPQuotationOutput version for api/dealerversionprices and api/v2/onroadprice
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
        public ulong PriceQuoteId { get; set; }
    }
}

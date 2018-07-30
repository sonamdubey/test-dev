using Bikewale.DTO.Campaign;
using Bikewale.DTO.PriceQuote.v2;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v3
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13-Oct-2017
    /// Summary: Dealer pricequote entity
    /// 
    /// </summary>
    public class DPQuotationOutput
    {
        [JsonProperty("benefits")]
        public IEnumerable<v2.DPQBenefit> Benefits { get; set; }

        [JsonProperty("emi")]
        public v2.EMI emi { get; set; }
        
        [JsonProperty("version")]
        public IEnumerable<DPQVersionBase> Versions { get; set; }

        [JsonProperty("quoteId")]
        public ulong PriceQuoteId { get; set; }

        [JsonProperty("campaign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CampaignBaseDto Campaign { get; set; }
    }
}

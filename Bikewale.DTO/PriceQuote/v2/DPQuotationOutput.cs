﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.v2
{
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
    }
}

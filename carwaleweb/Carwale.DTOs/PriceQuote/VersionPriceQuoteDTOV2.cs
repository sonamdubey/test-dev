using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class VersionPriceQuoteDTOV2
    {
        [JsonProperty("isMetallic")]
        public bool IsMetallic { get; set; }

        [JsonProperty("pricesList")]
        public List<PQItemListDTOV2> PricesList { get; private set; }
    }
}

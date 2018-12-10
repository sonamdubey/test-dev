using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class VersionPriceQuoteDTO
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }

        [JsonProperty("isMetallic")]
        public bool IsMetallic { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("pricesList")]
        public List<PQItemListDTO> PricesList { get; set; }
    }
}

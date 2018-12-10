using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Carwale.Entity.PriceQuote
{
    [Serializable, JsonObject]
    public class VersionPriceQuote
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

        [JsonProperty("onRoadPrice")]
        public long OnRoadPrice { get; set; }

        [JsonProperty("pricesList")]
        public List<PQItemList> PricesList { get; set; }
    }
}

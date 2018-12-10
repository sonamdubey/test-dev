using Carwale.Entity.Price;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Carwale.Entity.PriceQuote
{
    [Serializable, JsonObject]
    public class PQItemList
    {
        [JsonProperty("pqItemId")]
        public int PQItemId { get; set; }

        [JsonProperty("pqItemName")]
        public string PQItemName { get; set; }

        [JsonProperty("pqItemValue")]
        public long PQItemValue { get; set; }

        [JsonProperty("onRoadPriceInd")]
        public bool OnRoadPriceInd { get; set; }

        [JsonProperty("charge")]
        public ChargePrice ChargePrice { get; set; }

        [JsonProperty("chargeGroup")]
        public ChargeGroupPrice ChargeGroupPrice { get; set; }
    }
}

using Newtonsoft.Json;

namespace Carwale.DTOs.PriceQuote
{
    public class PQItemListDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("onRoadPriceInd")]
        public bool OnRoadPriceInd { get; set; }
    }
}

using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.PriceQuote
{
    public class NearByCityDto : IdNameDto
    {
        [JsonProperty("onRoadPrice")]
        public string OnRoadPrice { get; set; }
    }
}

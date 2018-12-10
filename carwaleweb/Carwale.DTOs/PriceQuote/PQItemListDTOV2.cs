using Newtonsoft.Json;
using Carwale.Entity.Common;

namespace Carwale.DTOs.PriceQuote
{
	public class PQItemListDTOV2 : IdName
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}

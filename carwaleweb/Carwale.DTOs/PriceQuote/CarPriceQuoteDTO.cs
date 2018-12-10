using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class CarPriceQuoteDTO
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("updatedBy")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("versionPricesList")]
        public List<VersionPriceQuoteDTO> VersionPricesList { get; set; }
    }
}

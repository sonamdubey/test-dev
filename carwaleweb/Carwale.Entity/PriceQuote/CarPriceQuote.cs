using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Carwale.Entity.PriceQuote
{
    [Serializable, JsonObject]
    public class CarPriceQuote
    {
        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty("sourceCityId")]
        public int SourceCityId { get; set; }

        [JsonProperty("versionPricesList")]
        public List<VersionPriceQuote> VersionPricesList { get; set; }
    }
}

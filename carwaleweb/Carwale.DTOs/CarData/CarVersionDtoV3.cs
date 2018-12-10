using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
    public class CarVersionDtoV3
    {
        [JsonProperty("versionId")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Version { get; set; }

        [JsonProperty("features")]
        public string SpecsSummary { get; set; }

        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("fuelType")]
        public string CarFuelType { get; set; }


        [JsonProperty("transmissionType")]
        public string TransmissionType { get; set; }

        [JsonProperty("priceOverview")]
        public PriceOverviewDtoV3 PriceOverview { get; set; }

    }
}

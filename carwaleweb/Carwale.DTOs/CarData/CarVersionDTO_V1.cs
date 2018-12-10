using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarVersionDTO_V1
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
        public PriceOverviewDTO PriceOverview { get; set; }

    }
}

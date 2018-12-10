using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    [Serializable]
    public class VersionDetailsDTO
    {
        [JsonProperty("id")]
        public int VersionId { get; set; }
        [JsonProperty("name")]
        public string VersionName { get; set; }
        [JsonProperty("onRoadPrice")]
        public long OnRoadPrice { get; set; }
        [JsonProperty("fuelType")]
        public string FuelType { get; set; }
        [JsonProperty("transmission")]
        public string Transmission { get; set; }
    }
}

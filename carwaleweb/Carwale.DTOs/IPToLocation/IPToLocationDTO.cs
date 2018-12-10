using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.IPToLocation
{
    public class IPToLocationDTO
    {
        [JsonProperty("cityId")]
        public int CityId { get; set; }
        [JsonProperty("cityName")]
        public string CityName { get; set; }
        [JsonProperty("isAreaAvailable")]
        public bool IsAreaAvailable { get; set; }
        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
    }
}

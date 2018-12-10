using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
   
    public class MileageDataDTO_V1
    {
        [JsonProperty("fuelType")]
        public string FuelType { get; set; }

        [JsonProperty("fuelUnit")]
        public string FuelUnit { get; set; }

        [JsonProperty("displacement")]
        public string Displacement { get; set; }

        [JsonProperty("transmission")]
        public string Transmission { get; set; }

        [JsonProperty("average")]
        public string FinalAverage { get; set; }

    }
}

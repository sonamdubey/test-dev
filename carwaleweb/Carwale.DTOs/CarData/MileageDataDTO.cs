using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    
    public class MileageDataDTO
    {


        [JsonProperty("fuelType")]
        public string FuelType { get; set; }

        [JsonProperty("fuelUnit")]
        public string FuelUnit { get; set; }

        [JsonProperty("disp")]
        public string Displacement { get; set; }

        [JsonProperty("trans")]
        public string Transmission { get; set; }

        [JsonProperty("mlFinalAvg")]
        public string FinalAverage { get; set; }

        [JsonProperty("mla")]
        public string Arai { get; set; }

        [JsonProperty("mlc")]
        public string City { get; set; }

        [JsonProperty("mlh")]
        public string Highway { get; set; }

        [JsonProperty("mlo")]
        public string Overall { get; set; }

        [JsonProperty("milAvg")]
        public string Average { get; set; }

        [JsonProperty("milMax")]
        public string Maximum { get; set; }

        [JsonProperty("milMin")]
        public string Minimum { get; set; }

        [JsonProperty("milMed")]
        public string Median { get; set; }
    }
}

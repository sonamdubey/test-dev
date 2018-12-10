using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.CarData;
using Newtonsoft.Json;

namespace Carwale.DTOs.Deals
{
    public class CarColorAndroid_DTO
    {
        [JsonProperty("carColor")]
        public CarColorDTO CarColor { get;set;}
        [JsonProperty("carImage")]
        public CarImageBaseDTO CarImage { get; set;}
        [JsonProperty("currentYear")]
        public int CurrentYear { get; set; }
        [JsonProperty("deals")]
        public List<DealsStockAndroid_DTO> Deals { get; set; }


    }
}

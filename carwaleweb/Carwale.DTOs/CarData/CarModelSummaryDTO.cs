using Carwale.Entity.Deals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarModelSummaryDTO
    {
        [JsonProperty("carMake")]
        public CarMakesDTO CarMake;

        [JsonProperty("carModel")]
        public CarModelsDTO CarModel;

        [JsonProperty("carPrices")]
        public CarPricesDTO CarPrices;

        [JsonProperty("carImageBase")]
        public CarImageBaseDTO CarImageBase;
    }
}

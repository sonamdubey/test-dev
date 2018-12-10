using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
   public class SimilarModelRecommendation
    {
       [JsonProperty("carMake")]
        public CarMakesDTO CarMake;

       [JsonProperty("carModel")]
       public CarModelsDTO CarModel;

       [JsonProperty("carPrices")]
       public CarPricesDTO CarPrices;

       [JsonProperty("custLocation")]
       public CustLocationDTO CustLocation;

       [JsonProperty("carImageBase")]
       public CarImageBaseDTO CarImageBase;
    }
}

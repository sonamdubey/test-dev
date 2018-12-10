using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Carwale.DTOs.CarData;
namespace Carwale.DTOs.Deals
{
   public class DealsSummaryMobile_DTO
    {
        [JsonProperty("make")]
        public CarMakesDTO Make { get; set; }

        [JsonProperty("model")]
        public CarModelsDTO Model { get; set; }

        [JsonProperty("savings")]
        public int Savings { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("OnRoadPrice")]
        public int OnRoadPrice { get; set; }

        [JsonProperty("OfferPrice")]
        public int OfferPrice { get; set; }

        [JsonProperty("stockCount")]
        public int StockCount { get; set; }

        [JsonProperty("carImageDetails")]
        public CarImageBaseDTO CarImageDetails { get; set; }
    }
}

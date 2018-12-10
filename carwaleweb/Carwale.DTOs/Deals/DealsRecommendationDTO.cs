using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DealsRecommendationDTO
    {
        [JsonProperty("carMake")]
        public CarMakesDTO Make { get; set; }

        [JsonProperty("carImgDetails")]
        public CarImageBaseDTO CarImageDetails { get; set; }

        [JsonProperty("carModel")]
        public CarModelsDTO Model { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("stockCount")]
        public int StockCount { get; set; }

        [JsonProperty("onRoadPrice")]
        public int OnRoadPrice { get; set; }

        [JsonProperty("savings")]
        public int Savings { get; set; }

        [JsonProperty("offers")]
        public string Offers { get; set; }

        [JsonProperty("offerPrice")]
        public int OfferPrice { get; set; }

        [JsonProperty("stockId")]
        public int StockId { get; set; }

        [JsonProperty("offerValue")]
        public int OfferValue { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
    }
}

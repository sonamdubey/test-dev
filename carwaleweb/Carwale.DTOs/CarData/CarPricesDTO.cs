using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarPricesDTO
    {
        [JsonProperty("minPrice")]
        public float MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public float MaxPrice { get; set; }

        [JsonProperty("baseVersionOnRoadPrice")]
        public int BaseVersionOnRoadPrice { get; set; }

        [JsonProperty("priceLabel")]
        public string PriceLabel { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }
        
    }
}

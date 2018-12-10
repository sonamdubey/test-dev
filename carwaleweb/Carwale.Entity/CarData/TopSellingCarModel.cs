using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class TopSellingCarModel 
    {
        [JsonProperty("review")]
        public CarReviewBase Review { get; set; }
        [JsonProperty("price")]
        public CarPrice Price { get; set; }
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }

        //[JsonProperty("basicId")]
        //public int BasicId { get; set; }
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
        [JsonProperty("model")]
        public ModelEntity Model { get; set; }
        [JsonProperty("city")]
        public City City { get; set; }
        [JsonProperty("priceOverview")]
        public PriceOverview PriceOverview { get; set; }
    }
}

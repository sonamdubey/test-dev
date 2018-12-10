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
    /// <summary>
    /// Added by : Chetan on 21/07/2015
    /// </summary>
   [Serializable]
   public class LaunchedCarModel
    {
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
        [JsonProperty("model")]
        public ModelEntity Model { get; set; }
        [JsonProperty("launchedDate")]
        public string LaunchedDate { get; set; }
        [JsonProperty("price")]
        public CarPrice Price { get; set; }
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("review")]
        public CarReviewBase Review { get; set; }
        [JsonProperty("city")]
        public City City { get; set; }
        [JsonProperty("basicId")]
        public int BasicId { get; set; }
        [JsonProperty("priceOverview")]
        public PriceOverview PriceOverview { get; set; }
        [JsonProperty("version")]
        public VersionEntity Version { get; set; }
        [JsonProperty("recordCount")]
        public int RecordCount { get; set; }
    }
}

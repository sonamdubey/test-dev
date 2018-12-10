using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.PopularUsedCars
{
    /// <summary>
    /// Crated  By : Sachin Shukla on 28 July 2015 
    /// </summary>
     
    public class PopularUsedCarModel
    {
        [JsonProperty("carName")]
        public string carName { get; set; }
        [JsonProperty("price")]
        public string price { get; set; }
        [JsonProperty("Image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("titleUrl")]
        public string titleUrl { get; set; }
        [JsonProperty("viewSimilarUrl")]
        public string viewSimilarUrl { get; set; }
        [JsonProperty("bodyType")]
        public string bodyType { get; set; }
        [JsonProperty("appTitleUrl")]
        public string appTitleUrl { get; set; }
        [JsonProperty("appSimilarUrl")]
        public string appSimilarUrl { get; set; }
    }
}

using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Newtonsoft.Json;

namespace Carwale.DTOs.Classified.PopularUC
{
    public class PopularUCModelApp
    {
        [JsonProperty("carName")]
        public string carName { get; set; }
        [JsonProperty("price")]
        public string price { get; set; }
        [JsonProperty("Image")]
        public CarImageBase Image { get; set; }        
        [JsonProperty("bodyType")]
        public string bodyType { get; set; }
        [JsonProperty("appTitleUrl")]
        public string appTitleUrl { get; set; }
        [JsonProperty("appSimilarUrl")]
        public string appSimilarUrl { get; set; }
    }

    public class PopularUCModelAppV2
    {
        [JsonProperty("carName")]
        public string carName { get; set; }
        [JsonProperty("price")]
        public string price { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }

        [JsonProperty("bodyType")]
        public string bodyType { get; set; }
        [JsonProperty("appTitleUrl")]
        public string appTitleUrl { get; set; }
        [JsonProperty("appSimilarUrl")]
        public string appSimilarUrl { get; set; }
    }
}

using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class SimilarCarModelsDTO_V1
    {        

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonProperty("versionId")]
        public int PopularVersionId { get; set; }        

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string ModelImageOriginal { get; set; }

        [JsonProperty("make")]
        public string MakeName { get; set; }

        [JsonProperty("model")]
        public string ModelName { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("reviewRate")]
        public string ReviewRateNew;

        [JsonProperty("carModelUrl")]
        public string CarModelUrl { get; set; }

        [JsonProperty("priceOverview")]
        public PriceOverviewDTO PriceOverview { get; set; }
    }
}

using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
    public class SimilarCarModelsDtoV3
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

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("reviewRate")]
        public string ReviewRateNew { get; set; }

        [JsonProperty("carModelUrl")]
        public string CarModelUrl { get; set; }

        [JsonProperty("priceOverview")]
        public PriceOverviewDtoV3 PriceOverview { get; set; }
    }
}

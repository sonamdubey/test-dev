using Newtonsoft.Json;

namespace Carwale.DTOs.Accessories.Tyres
{
    public class TyreSummaryDTO
    {
        [JsonProperty("itemId")]
        public int ItemId { get; set; }
        [JsonProperty("itemName")]
        public string ItemName { get; set; }
        [JsonProperty("brandName")]
        public string BrandName { get; set; }
        [JsonProperty("brandId")]
        public int BrandId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }
        [JsonProperty("warranty")]
        public string Warranty { get; set; }
        [JsonProperty("isTubeless")]
        public bool Tubeless { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("tyreDetailPageUrl")]
        public string TyreDetailPageUrl { get; set; }
        [JsonProperty("isSponsored")]
        public bool IsSponsored { get; set; }
    }
}

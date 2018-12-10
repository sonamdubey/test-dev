using Newtonsoft.Json;

namespace Carwale.DTOs.Classified.CarDetails
{
    public class UsedCar
    {
        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("carName")]
        public string CarName { get; set; }

        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("year")]
        public string MakeYear { get; set; }

        [JsonProperty("kms")]
        public string Kms { get; set; }

        [JsonProperty("city")]
        public string CityName { get; set; }

        [JsonProperty("updated")]
        public string LastUpdatedOn { get; set; }

        [JsonProperty("usedCarDetail")]
        public string UsedCarDetail { get; set; }

        [JsonProperty("isPremium")]
        public string IsPremium { get; set; }

        [JsonProperty("absureRating")]
        public string absureRating { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; }

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("fuel")]
        public string Fuel { get; set; }

        [JsonProperty("gearBox")]
        public string GearBox { get; set; }

        [JsonProperty("certificationScore")]
        public string CertificationScore { get; set; }

        [JsonProperty("dealerRatingText")]
        public string DealerRatingText { get; set; }

        [JsonProperty("isChatAvailable")]
        public bool IsChatAvailable { get; set; }

        [JsonProperty("formattedPrice")]
        public string FormattedPrice { get; set; }
    }
}

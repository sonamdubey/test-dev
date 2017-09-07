using Newtonsoft.Json;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by Snehal Dange on 02-09-2017
    /// Description: DTO created to save input rating 
    /// </summary>
    public class InputRatingSave
    {
        [JsonProperty("overAllrating")]
        public string OverAllrating { get; set; }

        [JsonProperty("ratingQuestionAns")]
        public string RatingQuestionAns { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("makeId")]
        public uint MakeId { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("priceRangeId")]
        public ushort PriceRangeId { get; set; }

        [JsonProperty("reviewId")]
        public uint ReviewId { get; set; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("platformId")]
        public ushort PlatformId { get; set; }

        [JsonProperty("sourceId")]
        public ushort? SourceId { get; set; }

        [JsonProperty("contestSrc")]
        public ushort? ContestSrc { get; set; }

        [JsonProperty("utmzCookieValue")]
        public string UtmzCookieValue { get; set; }

    }
}

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by Snehal Dange on 02-09-2017
    /// Description: DTO created to save input rating 
    /// </summary>
    public class InputRatingSave
    {
        [Required]
        [JsonProperty("overAllrating")]
        public string OverAllrating { get; set; }

        [Required]
        [JsonProperty("ratingQuestionAns")]
        public string RatingQuestionAns { get; set; }

        [Required]
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [Required]
        [JsonProperty("makeId")]
        public uint MakeId { get; set; }

        [Required]
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }


        [JsonProperty("priceRangeId")]
        public ushort PriceRangeId { get; set; }

        [Required]
        [JsonProperty("reviewId")]
        public uint ReviewId { get; set; }

       
        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [Required]
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

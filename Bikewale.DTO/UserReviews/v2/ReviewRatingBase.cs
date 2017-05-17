using Newtonsoft.Json;

namespace Bikewale.DTO.UserReviews.v2
{
    public class ReviewRatingBase
    {
        [JsonProperty("overAllRating")]
        public float OverAllRating { get; set; }
    }
}

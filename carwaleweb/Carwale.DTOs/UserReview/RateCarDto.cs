using Carwale.Entity.UserReview;
using Newtonsoft.Json;

namespace Carwale.DTOs.UserReview
{
    public class RateCarDto
    {
        [JsonProperty("carDetails")]
        public UserReviewCarDetails CarDetails { get; set; }
        [JsonProperty("ratingDetails")]
        public RateCarDetails RatingDetails { get; set; }
    }
}

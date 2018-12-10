using Newtonsoft.Json;

namespace Carwale.DTOs.UserReviews
{
    public class ReviewQuestion
    {
        [JsonProperty("questionId")]
        public int Id { get; set; }
        [JsonProperty("answerId")]
        public int RatingId { get; set; }

    }
}

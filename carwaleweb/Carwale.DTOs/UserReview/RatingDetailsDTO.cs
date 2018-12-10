using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.UserReviews
{
    public class RatingQuestionsDto
    {
        [JsonProperty("questionId")]
        public int QuestionId { get; set; }
        [JsonProperty("answerId")]
        public int Answer { get; set; }
    }
    public class UserDetailsDto
    {
        [JsonProperty("name")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string UserEmail { get; set; }
    }
    public class CarDetailsDto
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
    }
    public class RatingDTO
    {
        [JsonProperty("userRating")]
        public int UserRating { get; set; }
        [JsonProperty("ratingQuestions")]
        public List<RatingQuestionsDto> RatingQuestions { get; set; }
    }
    public class RatingDetailsDTO
    {
        [JsonProperty("userDetails")]
        public UserDetailsDto UserDetails { get; set; }
        [JsonProperty("carDetails")]
        public CarDetailsDto CarDetails { get; set; }
        [JsonProperty("rating")]
        public RatingDTO Rating { get; set; }
        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
        [JsonProperty("platformId")]
        public int PlatformId { get; set; }
        [JsonProperty("reviewId")]
        public int ReviewId { get; set; }
    }
}

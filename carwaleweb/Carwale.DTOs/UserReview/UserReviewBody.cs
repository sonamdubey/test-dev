using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.UserReviews
{
    public class UserReviewBody
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("reviewId")]
        public string Hash { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("questions")]
        public List<ReviewQuestion> ReviewQuestions { get; set; }
    }
}

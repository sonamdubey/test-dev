using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.Entity.UserReview
{
    public class RateCarDetails
    {
        [JsonProperty("heading")]
        public string Heading { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("rating")]
        public List<UserReviewRatingOptions> UserReviewRatingOptions { get; set; }
        [JsonProperty("questions")]
        public List<UserReviewQuestions> UserRatingQuestions { get; set; }
    }
}

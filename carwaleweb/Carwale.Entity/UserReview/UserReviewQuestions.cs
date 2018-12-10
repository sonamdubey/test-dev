using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
    public class UserReviewQuestions
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("heading")]
        public string Heading { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("userResponse", NullValueHandling = NullValueHandling.Ignore)]
        public string UserResponse { get; set; }
        [JsonProperty("option", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserReviewOption> UserReviewOption { get; set; }
    }
}

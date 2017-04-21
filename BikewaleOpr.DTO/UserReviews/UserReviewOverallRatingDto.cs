using Newtonsoft.Json;
using System;

namespace BikewaleOpr.DTO.UserReviews
{

    public class UserReviewOverallRatingDto
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("value")]
        public ushort Value { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("heading")]
        public string Heading { get; set; }
        [JsonProperty("responseHeading")]
        public string ResponseHeading { get; set; }

    }

    public enum UserReviewQuestionType
    {
        Rating = 1,
        Review = 2
    }
}

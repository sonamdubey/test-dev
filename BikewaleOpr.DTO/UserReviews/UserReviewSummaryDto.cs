
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.DTO.Make;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace BikewaleOpr.DTO.UserReviews
{
    public class UserReviewSummaryDto
    {
        [JsonProperty("overallRating")]
        public UserReviewOverallRatingDto OverallRating { get; set; }

        [JsonProperty("make")]
        public MakeBase Make { get; set; }

        [JsonProperty("model")]
        public ModelBase Model { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tips")]
        public string Tips { get; set; }

        [JsonProperty("overallRatingId")]
        public ushort OverallRatingId { get; set; }

        [JsonProperty("questions")]
        public IEnumerable<UserReviewQuestionDto> Questions { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }
    }
}

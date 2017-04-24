
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Bikewale.DTO.UserReviews
{
    public class UserReviewSummaryDto
    {
        [JsonProperty("overallRating"), DataMember]
        public UserReviewOverallRatingDto OverallRating { get; set; }

        [JsonProperty("make"), DataMember]
        public MakeBase Make { get; set; }

        [JsonProperty("model"), DataMember]
        public ModelBase Model { get; set; }

        [JsonProperty("originalImgPath"), DataMember]
        public string OriginalImgPath { get; set; }

        [JsonProperty("hostUrl"), DataMember]
        public string HostUrl { get; set; }

        [JsonProperty("description"), DataMember]
        public string Description { get; set; }

        [JsonProperty("title"), DataMember]
        public string Title { get; set; }

        [JsonProperty("tips"), DataMember]
        public string Tips { get; set; }

        [JsonProperty("overallRatingId"), DataMember]
        public ushort OverallRatingId { get; set; }

        [JsonProperty("questions"), DataMember]
        public IEnumerable<UserReviewQuestionDto> Questions { get; set; }

        [JsonProperty("customerName"), DataMember]
        public string CustomerName { get; set; }

        [JsonProperty("customerEmail"), DataMember]
        public string CustomerEmail { get; set; }
    }
}

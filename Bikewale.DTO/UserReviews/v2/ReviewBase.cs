using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.UserReviews.DTO.v2
{
    public class ReviewBase
    {
        [JsonProperty("reviewId")]
        public int ReviewId { get; set; }
        [JsonProperty("reviewTitle")]
        public string ReviewTitle { get; set; }
        [JsonProperty("writtenBy")]
        public string WrittenBy { get; set; }
        [JsonIgnore]
        public DateTime ReviewDate { get; set; }
        [JsonProperty("reviewDate")]
        public String DisplayReviewDate { get { return ReviewDate.ToString("dd MMMM yyyy"); } }
        [JsonProperty("reviewShortDate")]
        public String DisplayReviewShortDate { get { return ReviewDate.ToString("dd MMM yyyy"); } }
    }
}


using Newtonsoft.Json;
namespace Bikewale.DTO.Model.v5
{
    public class Review
    {
        [JsonProperty("userReviewCount")]
        public uint UserReviewCount { get; set; }

        [JsonProperty("expertReviewCount")]
        public uint ExpertReviewCount { get; set; }

        [JsonProperty("userRatingCount")]
        public uint RatingCount { get; set; }
    }
}

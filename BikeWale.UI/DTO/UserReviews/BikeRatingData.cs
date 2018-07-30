using Newtonsoft.Json;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th Sep 2017
    /// Description : DTO to save bike ratings info
    /// </summary>
    public class BikeRatingData
    {
        [JsonProperty("overallRating")]
        public float OverallRating { get; set; }
        [JsonProperty("totalReviews")]
        public uint TotalReviews { get; set; }
        [JsonProperty("totalRatings")]
        public uint TotalRatings { get; set; }
        [JsonProperty("oneStarRatings")]
        public uint OneStarRatings { get; set; }
        [JsonProperty("twoStarRatings")]
        public uint TwoStarRatings { get; set; }
        [JsonProperty("threeStarRatings")]
        public uint ThreeStarRatings { get; set; }
        [JsonProperty("fourStarRatings")]
        public uint FourStarRatings { get; set; }
        [JsonProperty("fiveStarRatings")]
        public uint FiveStarRatings { get; set; }
        [JsonProperty("maximumRatings")]
        public uint MaximumRatings { get; set; }
    }
}
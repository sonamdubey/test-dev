using Newtonsoft.Json;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th Sep 2017
    /// Description : DTO to save bike reviews info
    /// </summary>
    public class BikeReviewsData
    {
        [JsonProperty("totalReviews")]
        public uint TotalReviews { get; set; }
        [JsonProperty("mostHelpfulReviews")]
        public uint MostHelpfulReviews { get; set; }
        [JsonProperty("mostRecentReviews")]
        public uint MostRecentReviews { get; set; }
        [JsonProperty("postiveReviews")]
        public uint PostiveReviews { get; set; }
        [JsonProperty("negativeReviews")]
        public uint NegativeReviews { get; set; }
        [JsonProperty("neutralReviews")]
        public uint NeutralReviews { get; set; }
    }
}

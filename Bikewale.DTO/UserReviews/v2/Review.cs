using Bikewale.DTO.UserReviews.DTO.v2;
using Newtonsoft.Json;

namespace Bikewale.DTO.UserReviews.v2
{
    /// <summary>
    /// Modified by :   Sumit Kate on 26 Apr 2017
    /// Description :   Add JsonIgnore property for TaggedBike
    /// </summary>
    public class Review : ReviewBase
    {
        [JsonProperty("comments")]
        public string Comments { get; set; }
        [JsonProperty("pros")]
        public string Pros { get; set; }
        [JsonProperty("cons")]
        public string Cons { get; set; }
        [JsonProperty("liked")]
        public ushort Liked { get; set; }
        [JsonProperty("disliked")]
        public ushort Disliked { get; set; }
        [JsonProperty("viewed")]
        public uint Viewed { get; set; }
        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }
        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }
        [JsonProperty("overAllRating")]
        public ReviewRatingBase OverAllRating { get; set; }
        [JsonProperty("reviewAge")]
        public string ReviewAge { get; set; }
    }
}

using Newtonsoft.Json;

namespace Bikewale.Entities.DTO
{
    /// <summary>
    /// Modified by :   Sumit Kate on 26 Apr 2017
    /// Description :   Add JsonIgnore property for TaggedBike
    /// </summary>
    public class Review : ReviewBase
    {
        public string Comments { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public ushort Liked { get; set; }
        public ushort Disliked { get; set; }
        public uint Viewed { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public ReviewRatingBase OverAllRating { get; set; }
        [JsonIgnore]
        public Bikewale.Entities.DTO.ReviewTaggedBike TaggedBike { get; set; }
    }
}

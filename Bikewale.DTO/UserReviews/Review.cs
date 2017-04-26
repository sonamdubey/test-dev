using Newtonsoft.Json;

namespace Bikewale.Entities.DTO
{
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
        public ReviewTaggedBike TaggedBike { get; set; }
    }
}

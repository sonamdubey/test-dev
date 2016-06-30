using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    [Serializable,DataContract]
    public class ReviewEntity : ReviewEntityBase
    {
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string Pros { get; set; }
        [DataMember]
        public string Cons { get; set; }
        [DataMember]
        public ushort Liked { get; set; }
        [DataMember]
        public ushort Disliked { get; set; }
        [DataMember]
        public uint Viewed { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string ModelMaskingName { get; set; }

        private ReviewRatingEntityBase objRating = new ReviewRatingEntityBase();
        [DataMember]
        public ReviewRatingEntityBase OverAllRating { get { return objRating; } set { objRating = value;} }
        [DataMember]
        public ReviewTaggedBikeEntity TaggedBike { get; set; }
    }
}

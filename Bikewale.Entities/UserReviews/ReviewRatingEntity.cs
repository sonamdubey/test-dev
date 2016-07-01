using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class ReviewRatingEntity : ReviewRatingEntityBase
    {
        [DataMember]
        public float StyleRating { get; set; }
        [DataMember]
        public float ComfortRating { get; set; }
        [DataMember]
        public float PerformanceRating { get; set; }
        [DataMember]
        public float ValueRating { get; set; }
        [DataMember]
        public float FuelEconomyRating { get; set; }
    }
}

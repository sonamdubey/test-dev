using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// modified by :- Subodh jain 17 jan 2017
    /// summary :- added model rating looks
    /// </summary>
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
        [DataMember]
        public float ModelRatingLooks { get; set; }
    }
}

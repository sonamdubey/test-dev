using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class ReviewRatingEntityBase
    {
        [DataMember]
        public float OverAllRating { get; set; }
    }
}

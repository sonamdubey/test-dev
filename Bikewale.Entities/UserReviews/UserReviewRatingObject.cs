
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class UserReviewRatingObject
    {
        public ulong CustomerId { get; set; }
        public uint ReviewId { get; set; }
        public bool IsFake { get; set; }
    }
}

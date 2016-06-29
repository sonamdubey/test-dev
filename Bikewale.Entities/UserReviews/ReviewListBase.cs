using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class ReviewListBase
    {
        [DataMember]
        public List<ReviewEntity> ReviewList { get; set; }
        [DataMember]
        public uint TotalReviews { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Modified by :   Sumit Kate on 26 Apr 2017
    /// Description :   Replace List with IEnumerable
    /// </summary>
    [Serializable, DataContract]
    public class ReviewListBase
    {
        [DataMember]
        public IEnumerable<ReviewEntity> ReviewList { get; set; }
        [DataMember]
        public uint TotalReviews { get; set; }

    }
}

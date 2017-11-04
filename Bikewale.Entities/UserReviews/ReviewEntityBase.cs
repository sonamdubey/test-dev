using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    [Serializable,DataContract]
    public class ReviewEntityBase
    {
        [DataMember]
        public uint ReviewId { get; set; }
        [DataMember]
        public string ReviewTitle { get; set; }
        [DataMember]
        public DateTime ReviewDate { get; set; }
        [DataMember]
        public string WrittenBy { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    [Serializable, DataContract]
    public class MostHelpfulReviewObject
    {
        [DataMember]
        public uint RatingValue { get; set; }
        [DataMember]
        public string ReviewTitle { get; set; }
        [DataMember]
        public string ReviewDescription { get; set; }
        [DataMember]
        public string ReviewListUrl { get; set; }
        [DataMember]
        public string ReviewDetailUrl { get; set; }
    }
}

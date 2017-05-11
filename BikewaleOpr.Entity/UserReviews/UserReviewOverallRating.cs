
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
namespace BikewaleOpr.Entities.UserReviews
{
    [Serializable, DataContract]
    public class UserReviewOverallRating
    {
        [DataMember]
        public uint Id { get; set; }
        [DataMember]
        public ushort Value { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Heading { get; set; }
        [DataMember]
        public string ResponseHeading { get; set; }

    }

    public enum UserReviewQuestionType
    {
        Rating = 1,
        Review = 2
    }

}

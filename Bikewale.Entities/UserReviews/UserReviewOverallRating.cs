
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class UserReviewOverallRating
    {
        [JsonProperty("id"), DataMember]
        public uint Id { get; set; }
        [JsonProperty("value"), DataMember]
        public ushort Value { get; set; }
        [JsonProperty("description"), DataMember]
        public string Description { get; set; }
        [JsonProperty("heading"), DataMember]
        public string Heading { get; set; }
        [JsonProperty("responseHeading"), DataMember]
        public string ResponseHeading { get; set; }

    }

    public enum UserReviewQuestionType
    {
        Rating = 1,
        Review = 2
    }

}

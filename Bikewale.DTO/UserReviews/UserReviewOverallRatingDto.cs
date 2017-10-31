using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.DTO.UserReviews
{
    [Serializable, DataContract]
    public class UserReviewOverallRatingDto 
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
        [JsonProperty("originalImagePath"), DataMember]
        public string OriginalImagePath { get; set; }
        [JsonProperty("hostUrl"), DataMember]
        public string HostUrl { get; set; }
    }

    public enum UserReviewQuestionType
    {
        Rating = 1,
        Review = 2
    }
}

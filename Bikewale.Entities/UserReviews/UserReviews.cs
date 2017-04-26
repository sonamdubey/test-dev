using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class UserReviewQuestion
    {
        private bool _isRequired = true;
        private bool _isVisbile = true;

        [JsonProperty("qId")]
        public uint Id { get; set; }

        [JsonProperty("qtype"), DataMember, JsonIgnore]
        public UserReviewQuestionType Type { get; set; }

        [JsonProperty("heading"), DataMember]
        public string Heading { get; set; }

        [JsonProperty("description"), DataMember]
        public string Description { get; set; }

        [JsonProperty("selectedRatingId"), DataMember]
        public uint SelectedRatingId { get; set; }

        [JsonProperty("displayType"), DataMember]
        public UserReviewQuestionDisplayType DisplayType { get; set; }

        [JsonProperty("rating"), DataMember]
        public IEnumerable<UserReviewRating> Rating { get; set; }

        [JsonProperty("order"), DataMember, JsonIgnore]
        public ushort Order { get; set; }

        [JsonProperty("isRequired"), DataMember]
        public bool IsRequired { get { return _isRequired; } set { _isRequired = value; } }

        [JsonProperty("visibility"), DataMember]
        public bool Visibility { get { return _isVisbile; } set { _isVisbile = value; } }

        [JsonProperty("priceRangeIds"), JsonIgnore, DataMember]
        public IEnumerable<uint> PriceRangeIds { get; set; }

        [JsonProperty("subQuestionId"), DataMember]
        public uint SubQuestionId { get; set; }
    }

    [Serializable]
    public enum UserReviewQuestionDisplayType
    {
        Star = 1,
        Text = 2
    }

    [Serializable, DataContract]
    public class UserReviewRating
    {
        [JsonProperty("ratingId"), DataMember]
        public uint Id { get; set; }
        [JsonProperty("ratingValue"), DataMember]
        public string Value { get; set; }
        [JsonProperty("ratingText"), DataMember]
        public string Text { get; set; }
        [JsonProperty("qId"), JsonIgnore, DataMember]
        public uint QuestionId { get; set; }
    }


}

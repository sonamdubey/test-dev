using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.DTO.UserReviews
{

    [Serializable, DataContract]
    public class UserReviewQuestionDto
    {
        [JsonProperty("qId")]
        public uint Id { get; set; }

        [JsonProperty("qtype"), DataMember]
        public UserReviewQuestionType Type { get; set; }

        [JsonProperty("heading"), DataMember]
        public string Heading { get; set; }

        [JsonProperty("minHeading"), DataMember]
        public string MinHeading { get; set; }

        [JsonProperty("description"), DataMember]
        public string Description { get; set; }

        [JsonProperty("selectedRatingId"), DataMember]
        public uint SelectedRatingId { get; set; }

        [JsonProperty("selectedRatingText"), DataMember]
        public string SelectedRatingText { get; set; }

        [JsonProperty("displayType"), DataMember]
        public UserReviewQuestionDisplayType DisplayType { get; set; }

        [JsonProperty("rating"), DataMember]
        public IEnumerable<UserReviewRatingDto> Rating { get; set; }

        [JsonProperty("order"), DataMember, JsonIgnore]
        public ushort Order { get; set; }

        private bool _isRequired = true;

        [JsonProperty("isRequired"), DataMember]
        public bool IsRequired { get { return _isRequired; } set { _isRequired = value; } }


        private bool _isVisbile = true;

        [JsonProperty("visibility"), DataMember]
        public bool Visibility { get { return _isVisbile; } set { _isVisbile = value; } }

        [JsonProperty("priceRangeIds"), JsonIgnore, DataMember]
        public IEnumerable<uint> PriceRangeIds { get; set; }

        [JsonProperty("subQuestionId"), DataMember]
        public uint SubQuestionId { get; set; }
    }

    public enum UserReviewQuestionDisplayType
    {
        Star = 1,
        Text = 2
    }

    [Serializable, DataContract]
    public class UserReviewRatingDto
    {
        [JsonProperty("ratingId"), DataMember]
        public uint Id { get; set; }
        [JsonProperty("ratingValue"), DataMember]
        public string Value { get; set; }
        [JsonProperty("ratingText"), DataMember]
        public string Text { get; set; }
        [JsonProperty("qId"), JsonIgnore, DataMember]
        public uint QuestionId { get; set; }
        [JsonProperty("subQuestionId"), DataMember]
        public uint SubQuestionId { get; set; }
    }
}

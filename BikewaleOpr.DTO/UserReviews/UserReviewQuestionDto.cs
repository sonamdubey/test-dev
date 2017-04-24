using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BikewaleOpr.DTO.UserReviews
{

    public class UserReviewQuestionDto
    {
        [JsonProperty("qId")]
        public uint Id { get; set; }

        [JsonProperty("qtype")]
        public UserReviewQuestionType Type { get; set; }

        [JsonProperty("heading")]
        public string Heading { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("selectedRatingId")]
        public uint SelectedRatingId { get; set; }

        [JsonProperty("displayType")]
        public UserReviewQuestionDisplayType DisplayType { get; set; }

        [JsonProperty("rating")]
        public IEnumerable<UserReviewRatingDto> Rating { get; set; }

        [JsonProperty("order"), JsonIgnore]
        public ushort Order { get; set; }

        private bool _isRequired = true;

        [JsonProperty("isRequired")]
        public bool IsRequired { get { return _isRequired; } set { _isRequired = value; } }


        private bool _isVisbile = true;

        [JsonProperty("visibility")]
        public bool Visibility { get { return _isVisbile; } set { _isVisbile = value; } }

        [JsonProperty("priceRangeIds"), JsonIgnore]
        public IEnumerable<uint> PriceRangeIds { get; set; }

        [JsonProperty("subQuestionId")]
        public uint SubQuestionId { get; set; }
    }

    public enum UserReviewQuestionDisplayType
    {
        Star = 1,
        Text = 2
    }

    public class UserReviewRatingDto
    {
        [JsonProperty("ratingId")]
        public uint Id { get; set; }
        [JsonProperty("ratingValue")]
        public string Value { get; set; }
        [JsonProperty("ratingText")]
        public string Text { get; set; }
        [JsonProperty("qId"), JsonIgnore]
        public uint QuestionId { get; set; }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
namespace Bikewale.Entities.UserReviews
{
    public class UserReviewQuestion
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

        [JsonProperty("displaytype")]
        public UserReviewQuestionDisplayType DisplayType { get; set; }

        [JsonProperty("rating")]
        public IEnumerable<UserReviewRating> Rating { get; set; }

        [JsonProperty("order")]
        public ushort Order { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get { return true; } }

        [JsonProperty("visibility")]
        public bool Visibility { get { return true; } }

        [JsonProperty("subQuestionId")]
        public uint SubQuestionId { get { return 0; } }
    }

    public enum UserReviewQuestionDisplayType
    {
        Star = 1,
        Text = 2
    }

    public class UserReviewRating
    {
        [JsonProperty("ratingId")]
        public uint Id { get; set; }
        [JsonProperty("ratingValue")]
        public string Value { get; set; }
        [JsonProperty("ratingText")]
        public string Text { get; set; }
        [JsonProperty("qId")]
        public uint QuestionId { get; set; }
    }


}

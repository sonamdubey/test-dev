using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.DTO.UserReviews
{
    public class ReviewQuestionsDto
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("heading")]
        public string Heading { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("currentlySelected")]
        public uint SelectedRatingId { get; set; }

        public UserReviewQuestionDisplayTypeDto DisplayType { get; set; }

        [JsonProperty("rating")]
        public IEnumerable<UserReviewratingDto> Rating { get; set; }

        [JsonProperty("type")]
        public string DisplayTypeString { get { return Convert.ToString(DisplayType); } }

    }

    public enum UserReviewQuestionDisplayTypeDto
    {
        star = 1,
        text = 2
    }

    public class UserReviewratingDto
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("ratingText")]
        public string Text { get; set; }
    }
}

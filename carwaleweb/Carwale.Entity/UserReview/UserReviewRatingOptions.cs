using System;
using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
    public class UserReviewRatingOptions
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("heading")]
        public string Heading { get; set; }
        [JsonProperty("originalImgPath", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalImgPath { get; set; }
    }
}

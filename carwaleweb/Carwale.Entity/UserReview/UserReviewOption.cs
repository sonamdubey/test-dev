using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
    public class UserReviewOption
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}

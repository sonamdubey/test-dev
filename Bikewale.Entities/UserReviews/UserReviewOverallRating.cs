
using Newtonsoft.Json;
namespace Bikewale.Entities.UserReviews
{

    public class UserReviewOverallRating
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        public ushort Value { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("heading")]
        public string Heading { get; set; }

    }

    public enum UserReviewQuestionType
    {
        Rating = 1,
        Review = 2
    }

}

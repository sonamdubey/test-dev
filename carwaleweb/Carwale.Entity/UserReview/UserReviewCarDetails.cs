using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
    public class UserReviewCarDetails
    {
        [JsonProperty("make")]
        public NameIdBase Make { get; set; }
        [JsonProperty("model")]
        public NameIdBase Model { get; set; }
        [JsonProperty("version")]
        public NameIdBase Version { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}

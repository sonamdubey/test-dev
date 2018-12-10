using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.Entity.UserReview
{
    public class UserReviewFilter
    {
        [JsonProperty("defaultFilter")]
        public UserReviewFilterBase DefaultFilter { get; set; }
        [JsonProperty("allFilters")]
        public IEnumerable<UserReviewFilterBase> AllFilters { get; set; }
    }

    public class UserReviewFilterBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.Entity.UserReview
{
    public class PageSectionDetails
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public IEnumerable<string> Description { get; set; }
    }
}

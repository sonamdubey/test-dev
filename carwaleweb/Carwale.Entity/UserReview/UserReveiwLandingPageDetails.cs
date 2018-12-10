using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.UserReview
{
    public class UserReveiwLandingPageDetails
    {
        [JsonProperty("primaryBanner")]
        public string PrimaryBanner { get; set; }
        [JsonProperty("secondaryBanner")]
        public string SecondaryBanner { get; set; }
        [JsonProperty("writeReview")]
        public SectionDetails WriteReview { get; set; }
        [JsonProperty("readReview")]
        public SectionDetails ReadReview { get; set; }
        [JsonProperty("filters")]
        public Filters Filters { get; set; }
        [JsonProperty("howToWin")]
        public SectionDetails HowToWin { get; set; }
        [JsonProperty("termAndCondition")]
        public SectionDetails TermsAndCondition { get; set; }
    }

    public class SectionDetails
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Description { get; set; }
    }

    public class Filters
    {
        [JsonProperty("defaultFilter")]
        public FilterDetails DefaultFilter { get; set; }
        [JsonProperty("allFilters")]
        public List<FilterDetails> AllFilters { get; set; }
    }

    public class FilterDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class WebPage
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("keywords", NullValueHandling = NullValueHandling.Ignore)]
        public string Keywords { get; set; }

        [JsonProperty("breadcrumb", NullValueHandling = NullValueHandling.Ignore)]
        public BreadcrumbList Breadcrum { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }
}

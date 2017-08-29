using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class NewsArticle : SchemaBase
    {
        [JsonProperty("@type")]
        public string Type { get { return "NewsArticle"; } }

        [JsonProperty("headline")]
        public string HeadLine { get; set; }

        [JsonProperty("datePublished", NullValueHandling = NullValueHandling.Ignore)]
        public string DatePublished { get; set; }

        [JsonProperty("dateModified", NullValueHandling = NullValueHandling.Ignore)]
        public string DateModified { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("articleBody", NullValueHandling = NullValueHandling.Ignore)]
        public string ArticleBody { get; set; }

        [JsonProperty("mainEntityOfPage", NullValueHandling = NullValueHandling.Ignore)]
        public MainEntityOfPage MainEntityOfPage { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public ImageObject ArticleImage { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("publisher")]
        public Organization Publisher { get { return new Organization(); } }
    }

}

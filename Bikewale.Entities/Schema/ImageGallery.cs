using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.Schema
{
    public class ImageGallery
    {
        [JsonProperty("@type")]
        public string Type { get { return "ImageGallery"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("headline", NullValueHandling = NullValueHandling.Ignore)]
        public string Headline { get; set; }

        [JsonProperty("primaryImageOfPage", NullValueHandling = NullValueHandling.Ignore)]
        public ImageObject PrimaryImageOfPage { get; set; }

        [JsonProperty("associatedMedia", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ImageObject> Images { get; set; }
    }
}

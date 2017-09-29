using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class ImageObject
    {
        [JsonProperty("@type")]
        public string Type { get { return "ImageObject"; } }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageUrl { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public string Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public string Height { get; set; }

        [JsonProperty("contentUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentUrl { get; set; }

        [JsonProperty("thumbnailUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }
    }
}

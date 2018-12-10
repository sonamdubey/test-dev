using Newtonsoft.Json;

namespace Carwale.DTOs.CMS.ThreeSixtyView
{
    public class ThreeSixtyViewImageDto
    {
        [JsonProperty(PropertyName="previewImagePath")]
        public string PreviewImagePath { get; set; }

        [JsonProperty(PropertyName = "originalImagePath")]
        public string OriginalImagePath { get; set; }
    }
}

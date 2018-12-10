using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.CMS.ThreeSixtyView
{
    public class ThreeSixtyExteriorDtoApp
    {
        [JsonProperty(PropertyName = "hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty(PropertyName = "previewImage")]
        public string PreviewImage { get; set; }

        [JsonProperty(PropertyName = "startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty(PropertyName = "images")]
        public List<string> Images { get; set; }
    }
}

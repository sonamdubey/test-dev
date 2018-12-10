using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.CMS.ThreeSixtyView
{
    public class ThreeSixtyInteriorDtoApp
    {
        [JsonProperty(PropertyName = "hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty(PropertyName = "cameraAngle")]
        public ThreeSixtyCameraDto CameraAngle { get; set; }

        [JsonProperty(PropertyName = "images")]
        public Dictionary<string, ThreeSixtyViewImageDto> Images { get; set; }
    }
}

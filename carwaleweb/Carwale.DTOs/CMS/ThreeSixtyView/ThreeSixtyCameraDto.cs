
using Newtonsoft.Json;

namespace Carwale.DTOs.CMS.ThreeSixtyView
{
    public class ThreeSixtyCameraDto
    {
        [JsonProperty(PropertyName="hLookAt")]
        public float HLookAt { get; set; }

        [JsonProperty(PropertyName = "vLookAt")]
        public float VLookAt { get; set; }
    }
}

using Newtonsoft.Json;

namespace Carwale.DTOs.CMS.ThreeSixtyView
{
    public class ThreeSixtyAvailabilityDTO
    {
        [JsonProperty("is360InteriorAvailable")]
        public bool Is360InteriorAvailable { get; set; }
        [JsonProperty("is360OpenAvailable")]
        public bool Is360OpenAvailable { get; set; }
        [JsonProperty("is360ExteriorAvailable")]
        public bool Is360ExteriorAvailable { get; set; }
    }
}
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.ES
{
    public class ESVersionColorsDto
    {
        [JsonProperty("version")]
        public ESVersionSummaryDto Version { get; set; }        

        [JsonProperty("exteriorColor")]
        public List<ExteriorColorDto> ExteriorColor { get; set; }
    }
}

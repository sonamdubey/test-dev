using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.ES
{   
    public class ExteriorColorDto : CarColorDTO
    {
        [JsonProperty("interiorColor")]
        public List<InteriorColorDto> InteriorColor { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}

using Newtonsoft.Json;
using Carwale.DTOs.Common;

namespace Carwale.DTOs.CarData
{
  
    public class BodyTypesDto: IdNameDto
	{
        [JsonProperty("carCount")]
        public int CarCount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("lineIcon")]
        public string LineIcon { get; set; }

        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }

        [JsonProperty("isRecommended")]
        public bool IsRecommended { get; set; }

    }
}


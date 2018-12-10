using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
  
    public class FuelTypesDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("carCount")]
        public int CarCount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }

    }
}


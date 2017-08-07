using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Location
{
    public class CityNameDTO
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("cityName")]
        public string CityName { get; set; }
    }
}

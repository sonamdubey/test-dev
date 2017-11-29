using Newtonsoft.Json;

namespace Bikewale.DTO.City
{
    public class CityBase
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
    }
}

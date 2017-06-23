using Newtonsoft.Json;

namespace Bikewale.ManufacturerCampaign.Entities
{
    public class CityDTO
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }
    }
}

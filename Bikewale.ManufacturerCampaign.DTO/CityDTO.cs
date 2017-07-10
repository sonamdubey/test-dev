using Newtonsoft.Json;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Aditi Srivastava on 22 Jun 2017
    /// Summary    : DTO for cities
    /// </summary>
    public class CityDTO
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }
    }
}

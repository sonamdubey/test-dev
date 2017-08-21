using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Location
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 18 Aug 2017
    /// Description :   Holds city name and city Id
    /// </summary>
    public class CityNameDTO
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("cityName")]
        public string CityName { get; set; }
    }
}

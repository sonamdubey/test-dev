using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Geolocation
{
    public class GroupCityDTO
    {
        [JsonProperty("id", Order = 1)]
        public int GroupId { get; set; }

        [JsonProperty("name", Order = 2)]
        public string GroupName { get; set; }

        [JsonProperty("cities", Order = 3)]
        public List<CityBaseDTO> Cities { get; set; }
    }

}
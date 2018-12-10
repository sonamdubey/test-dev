using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class PQGroupCityDTO
    {
        [JsonProperty("id",Order = 1)]
        public int CityId { get; set; }

        [JsonProperty("name", Order = 2)]
        public string CityName { get; set; }

        [JsonProperty("zones", Order = 4)]
        public List<ZoneDTO> Zones;

        [JsonProperty("group", Order = 5)]
        public List<CityDTO> Group;

        [JsonProperty("groupName", Order = 3)]
        public string GroupName { get; set; }
    }
}

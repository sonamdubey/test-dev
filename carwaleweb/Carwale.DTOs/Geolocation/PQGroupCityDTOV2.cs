using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class PQGroupCityDTOV2
    {
        [JsonProperty("id",Order=1)]
        public int GroupId { get; set; }

        [JsonProperty("name",Order=2)]
        public string GroupName { get; set; }

        [JsonProperty("cities", Order = 3)]
        public List<CityZonesDTOV2> Cities { get; set; }
    }
}

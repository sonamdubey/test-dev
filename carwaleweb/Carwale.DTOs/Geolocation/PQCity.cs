using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class PQCityDTO
    {
        [JsonProperty("groupCities")]
        public List<PQGroupCityDTO> GroupCities;
        [JsonProperty("cities")]
        public List<CityDTO> Cities;
    }
}

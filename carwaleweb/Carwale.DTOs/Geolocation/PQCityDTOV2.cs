using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class PQCityDTOV2
    {
        [JsonProperty("groupCities")]
        public List<PQGroupCityDTOV2> GroupCities;
        [JsonProperty("cities")]
        public List<CityDTO> Cities;
    }
}

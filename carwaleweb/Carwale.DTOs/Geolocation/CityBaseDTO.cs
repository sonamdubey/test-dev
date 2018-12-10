using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class CityBaseDTO
    {
        [JsonProperty("id", Order = 1)]
        public int Id { get; set; }
        [JsonProperty("name", Order = 2)]
        public string Name { get; set; }
        [JsonProperty("stateId", Order = 3)]
        public int StateId { get; set; }
        [JsonProperty("stateName", Order = 4)]
        public string StateName { get; set; }
    }
}

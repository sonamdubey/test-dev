using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class StateAndAllCities
    {
        [JsonProperty("state")]
        public State State { get; set; }
        [JsonProperty("cities")]
        public List<City> Cities { get; set; }
    }

    public class State
    {
        [JsonProperty("stateId")]
        public int StateId { get; set; }
        [JsonProperty("stateName")]
        public string StateName { get; set; }
    }
}

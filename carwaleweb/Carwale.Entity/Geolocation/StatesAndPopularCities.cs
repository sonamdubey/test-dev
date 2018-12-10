using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class StatesAndPopularCities
    {
        [JsonProperty("states")]
        public List<States> States { get; set; }
        [JsonProperty("cities")]
        public List<PopularCity> Cities { get; set; }
    }
}

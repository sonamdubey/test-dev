using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class StateAndAllCities
    {
        public States State { get; set; }
        public List<City> Cities{ get; set; }
    }
}

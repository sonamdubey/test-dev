using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    public class CityZonesV2 : City
    {
        public List<Zone> Zones { get; set; }
    }
}

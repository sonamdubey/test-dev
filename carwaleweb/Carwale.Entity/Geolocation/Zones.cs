using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class Zones
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}

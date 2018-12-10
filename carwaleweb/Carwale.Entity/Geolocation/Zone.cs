using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class Zone : City
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
    }
}

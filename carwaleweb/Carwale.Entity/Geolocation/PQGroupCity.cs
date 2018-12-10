using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class PQGroupCity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public List<Zone> Zones;
        public List<City> Group;
        public string GroupName { get; set; }
    }
}

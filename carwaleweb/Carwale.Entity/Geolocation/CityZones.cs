using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    /// <summary>
    /// TODO:Bangalore Zone Refactoring
    /// </summary>
    [Serializable]
    public class CityZones
    {
        public List<Zone> Mumbai { get; set; }
        public List<Zone> Bangalore { get; set; }
        public List<Zone> Delhi { get; set; }
    }
}

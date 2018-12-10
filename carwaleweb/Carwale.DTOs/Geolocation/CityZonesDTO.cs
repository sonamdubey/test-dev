using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    ///TODO:Bangalore Zone Refactoring
    //I dont want to do mess but m doing bcz m forced to.
    public class CityZonesDTO
    {
        public List<Zone> Mumbai { get; set; }
        public List<Zone> Bangalore { get; set; }
        public List<Zone> Delhi { get; set; }
    }
}

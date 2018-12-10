using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Elastic
{
    public class VersionCityPricesObj
    {
        public int VersionId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public Location Location { get; set; }

    }

    public class Location
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}

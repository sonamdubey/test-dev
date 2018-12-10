using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarValuation
{
    public class ValuationRequest
    {        
        public int VersionId { get; set; }
        public int ManufactureYear { get; set; }
        public int ManufactureMonth { get; set; }
        public int KmsTraveled { get; set; }
        public int CustomerID { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public int ActualCityID { get; set; }
        public DateTime RequestDateTime { get; set; }
        public String RemoteHost { get; set; }
        public Int16 RequestSource { get; set; }
    }
}

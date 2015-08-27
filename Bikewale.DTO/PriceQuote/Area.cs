using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    public class Area
    {
        public UInt32 AreaId { get; set; }
        public string AreaName { get; set; }
        public string PinCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }        
    }
}

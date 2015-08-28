using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Location
{
    public class AreaEntityBase
    {
        public UInt32 AreaId { get; set; }
        public string AreaName { get; set; }
        public string PinCode { get; set; }
        public string AreaMaskingName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public class PQParameterEntity
    {
        public UInt32 VersionId { get; set; }
        public UInt32 CityId { get; set; }
        public UInt32 DealerId { get; set; }
    }
}

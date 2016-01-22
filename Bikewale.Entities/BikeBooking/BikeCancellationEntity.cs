using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public class BikeCancellationEntity
    {
        public string BwId { get; set; }
        public string Mobile { get; set; }
    }

    public class ValidBikeCancellationResponseEntity
    {
        public bool IsVerified { get; set; }
        public string Message { get; set; }
        public int ResponseFlag { get; set; }
    }
}

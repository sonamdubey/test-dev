using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Customer
{
    public class CancelledBikeCustomer : CustomerEntityBase
    {
        public string BikeName { get; set; }
        public string BookingDate { get; set; }
        public UInt16 isCancellable { get; set; }
    }
}

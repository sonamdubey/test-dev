using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    [Serializable]
    public class PQ_Price
    {
        public UInt32 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public UInt32 Price { get; set; }
        public UInt32 DealerId { get; set; }
    }
}

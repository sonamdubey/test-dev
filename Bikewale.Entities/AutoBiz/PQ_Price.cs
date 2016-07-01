using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BikeWale.Entities.AutoBiz
{
    public class PQ_Price
    {
        public UInt32 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public UInt64 Price { get; set; }
        public UInt32 DealerId { get; set; }
    }
}

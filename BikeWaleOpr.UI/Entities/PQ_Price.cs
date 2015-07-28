using System;
using System.Collections.Generic;
using System.Text;

namespace BikeWaleOpr.Entities
{
    [Serializable]
    public class PQ_Price
    {
        public UInt32 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public UInt64 Price { get; set; }
    }
}

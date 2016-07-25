using System;

namespace BikewaleOpr.Entities
{
    public class PQ_Price
    {
        public UInt32 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public UInt32 Price { get; set; }
        public UInt32 DealerId { get; set; }
    }

    public class PQ_VersionPrice : PQ_Price
    {
        public UInt32 VersionId { get; set; }
    }
}

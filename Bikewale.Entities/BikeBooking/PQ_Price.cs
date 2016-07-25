using System;

namespace Bikewale.Entities.BikeBooking
{
    public class PQ_Price
    {
        public UInt32 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public UInt32 Price { get; set; }
        public UInt32 DealerId { get; set; }
    }
}

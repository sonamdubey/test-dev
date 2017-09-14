using System;

namespace Bikewale.Entities.BikeBooking
{

    /// <summary>
    /// Modified by : Ashutosh Sharma on 30 Aug 2017 
    /// Description : Removed IsGstPrice property
    /// </summary>
    public class PQ_Price
    {
        public UInt32 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public UInt32 Price { get; set; }
        public UInt32 DealerId { get; set; }

    }
}

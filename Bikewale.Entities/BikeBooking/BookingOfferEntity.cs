using System;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   Booking Offer Entity
    /// </summary>
    public class BookingOfferEntity
    {
        public int ModelId { get; set; }
        public int DealerId { get; set; }
        public UInt16 OfferCount { get; set; }
    }
}

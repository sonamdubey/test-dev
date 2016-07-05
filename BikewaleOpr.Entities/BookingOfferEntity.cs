using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entities
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

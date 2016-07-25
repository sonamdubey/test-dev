using Bikewale.Entities.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 26 Oct 2015
    /// </summary>
    public class DealerQuotation
    {
        public NewBikeDealers Dealer { get; set; }
        public uint BookingAmount { get; set; }
        public uint Availability { get; set; }
        public uint DealerId { get; set; }
    }
}

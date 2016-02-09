using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Create by   :   Sumit Kate on 05 Feb 2016
    /// Description :   Booking Listing Filter Entity
    /// </summary>
    public class BookingListingFilterEntity
    {
        public string MakeIds { get; set; }
        public string Displacement { get; set; }
        public string Budget { get; set; }
        public string Mileage { get; set; }
        public string RideStyle { get; set; }
        public string AntiBreakingSystem { get; set; }
        public string BrakeType { get; set; }
        public string AlloyWheel { get; set; }
        public string StartType { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string so { get; set; }
        public string sc { get; set; }
    }
}

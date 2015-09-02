using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Created By : Ashwini Todkar 17 Dec 2014
    /// </summary>
    public class BookingAmountEntity
    {
        public BookingAmountEntityBase objBookingAmountEntityBase { get; set; }
        public NewBikeDealers objDealer { get; set; }
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionEntityBase objVersion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeBooking;
namespace Bikewale.Interfaces.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   Bike Booking Listing
    /// </summary>
    public interface IBookingListing
    {
        IEnumerable<BikeBookingListingEntity> FetchBookingList(int cityId, uint areaId, Entities.BikeBooking.BookingListingFilterEntity filter, out int totalCount, out int fetchedCount, out PagingUrl pageUrl);        
    }
}

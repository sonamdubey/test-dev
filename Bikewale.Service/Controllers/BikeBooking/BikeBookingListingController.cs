using Bikewale.DTO.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   BikeBookingListing Controller
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class BikeBookingListingController : CompressionApiController//ApiController
    {
        private IBookingListing _objBookingListing = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objBookingListing"></param>
        public BikeBookingListingController(IBookingListing objBookingListing)
        {
            _objBookingListing = objBookingListing;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Feb 2016
        /// Description :
        /// </summary>
        /// <param name="filter">Booking filters</param>
        /// <param name="cityId">City Id</param>
        /// <param name="areaId">Area Id</param>
        /// <returns></returns>
        [ResponseType(typeof(BikeBookingListingOutput))]
        public IHttpActionResult Get([FromUri]BookingListingFilterDTO filter, int cityId, int areaId)
        {
            IEnumerable<BikeBookingListingEntity> lstEntity = null;
            IEnumerable<BikeBookingListingDTO> lstResult = null;
            BikeBookingListingOutput output = null;
            Bikewale.Entities.BikeBooking.PagingUrl PageUrlEntity = null;
            int fetchedCount = 0;
            int totalCount = 0;

            if (cityId > 0 && areaId > 0)
            {
                try
                {
                    output = new BikeBookingListingOutput();
                    BookingListingFilterEntity filterEntity = BookingListingFilterDTOMapper.Convert(filter);
                    lstEntity = _objBookingListing.FetchBookingList(cityId, Convert.ToUInt32(areaId), filterEntity, out totalCount, out fetchedCount, out PageUrlEntity);
                    lstResult = BikeBookingListingEntityMapper.Convert(lstEntity);
                    output.Bikes = lstResult;
                    output.FetchedCount = fetchedCount;
                    output.TotalCount = totalCount;
                    output.PageUrl = PageUrlEntityMapper.Convert(PageUrlEntity);
                    output.CurPageNo = filter.PageNo == 0 ? 1 : filter.PageNo;
                    if (output.FetchedCount == 0)
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "BikeBookingListingController.Get");
                   
                    return InternalServerError();
                }
            }
            else
            {
                return NotFound();
            }
            return Ok(output);
        }
    }
}

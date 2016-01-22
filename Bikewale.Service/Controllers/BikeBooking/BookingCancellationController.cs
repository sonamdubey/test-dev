using Bikewale.DAL.BikeBooking;
using Bikewale.DTO.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking
{
    /// <summary>
    /// Process Booking Cancellation request 
    /// Author  :   Sangram Nandkhile
    /// Created On  :   21st Jan 2016
    /// </summary>
    public class BookingCancellationController : ApiController
    {
        private readonly IBookingCancellation _objCancellation = null;
        /// <summary>
        /// Constructor 
        /// </summary>
        public BookingCancellationController(IBookingCancellation objCancellation)
        {
            _objCancellation = objCancellation;
        }

        /// <summary>
        /// Verify Booking Cancellation request 
        /// </summary>
        /// <param name="input">entity</param>
        /// <returns></returns>
        //[ResponseType(typeof(bool)), Route("api/bookingcancellation/isvalidcancellation/")]
        //public IHttpActionResult Post([FromBody]BikeCancellationEntity request)
        //{
        //    bool isSuccess = false;
        //    try
        //    {
        //        isSuccess = _objdpq.IsValidCancellation(request.BwId, request.Mobile);
        //        if (isSuccess)
        //        {
        //            return Ok(true);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BookingCancellationController.Post");
        //        objErr.SendMail();
        //        return InternalServerError();
        //    }
        //}

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 22 Jan 2016
        /// Description : verify if Booking Cancellation OTP is correct ror not.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Deatil of the customer who wants to cancel booking.</returns>
 
        [ResponseType(typeof(bool)), Route("api/bookingcancellation/isvalidcancellationotp/")]
        public IHttpActionResult Post([FromBody]BikeCancellationEntity request)
        {
            CancelledBikeCustomer cancelBikeDetail = null;
            try
            {
                cancelBikeDetail = _objCancellation.VerifyCancellationOTP(request.BwId, request.Mobile, request.OTP);
                if (cancelBikeDetail != null)
                {
                    return Ok(cancelBikeDetail);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BookingCancellationController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}

using Bikewale.DTO.BikeBooking;
using Bikewale.Entities.BikeBooking;
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
        private readonly IDealerPriceQuote _objdpq = null;
        /// <summary>
        /// Constructor 
        /// </summary>
        public BookingCancellationController(IDealerPriceQuote objDpq)
        {
            _objdpq = objDpq;
        }

        /// <summary>
        /// Verify Booking Cancellation request 
        /// </summary>
        /// <param name="input">entity</param>
        /// <returns></returns>
        [ResponseType(typeof(bool)), Route("api/bookingcancellation/isvalidcancellation/")]
        public IHttpActionResult Post([FromBody]BikeCancellationEntity request)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objdpq.IsValidCancellation(request.BwId, request.Mobile);
                if (isSuccess)
                {
                    return Ok(true);
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

        /// <summary>
        /// verify if Booking Cancellation OTP is correct ror not 
        /// </summary>
        /// <param name="input">entity</param>
        /// <returns></returns>
        //[ResponseType(typeof(bool)), Route("api/bookingcancellation/isvalidcancellationotp/")]
        //public IHttpActionResult Post([FromBody]BikeCancellationEntity request)
        //{
        //    return NotFound();
        //    try
        //    {
        //        isSuccess = _objdpq.get(request.BwId, request.Mobile);
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
    }
}

using Bikewale.BAL.BikeBooking;
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
       // private BookingCancellation BALBookingCancel = null;
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
        /// <param name="request">Bike Cancellation Entity</param>
        /// <returns></returns>
        [ResponseType(typeof(ValidBikeCancellationResponse)), Route("api/bookingcancellation/isvalidrequest/")]
        public IHttpActionResult Post([FromBody]BikeCancellationEntity request)
        {
            ValidBikeCancellationResponse response = null;
            ValidBikeCancellationResponseEntity responseEntity = null;
            bool isSuccess = false;
            try
            {
                responseEntity = _objCancellation.IsValidCancellation(request.BwId, request.Mobile);

                #region mapping entity into DTO
                response = new ValidBikeCancellationResponse();
                response.ResponseFlag = responseEntity.ResponseFlag;
                response.IsVerified = responseEntity.IsVerified;
                response.Message = responseEntity.Message;
                isSuccess = responseEntity.ResponseFlag == 1 ? true : false;
                #endregion

                if (isSuccess)
                {
                    return Ok(response);
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
        /// Created By : Lucky Rathore
        /// Created On : 22 Jan 2016
        /// Description : verify if Booking Cancellation OTP is correct ror not.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Deatil of the customer who wants to cancel booking.</returns>

        [HttpPost]
        [ResponseType(typeof(CancelledBikeCustomer)), Route("api/bookingcancellation/isvalidotp/")]
        public IHttpActionResult IsValidOtp([FromBody]BikeCancellationEntity request)
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

        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Jan 2016
        /// Summary : To cancel Bike booking 
        /// </summary>
        /// <param name="pqId">Price Quote Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/bookingcancellation/confirm/")]
        public IHttpActionResult CancelBooking(uint pqId)
        {
            bool isCancelled = false;
            try
            {
                isCancelled = _objCancellation.ConfirmCancellation(pqId);

                if (isCancelled)
                    return Ok(isCancelled);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BookingCancellationController.CancelBooking");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/bookingcancellation/SaveCancelOTP/")]
        public IHttpActionResult SaveCancelOTP(BikeCancellationEntity cancellation) 
        {
            uint attemps = 0;   
            try
            {
                attemps = _objCancellation.SaveCancellationOTP(cancellation.BwId, cancellation.Mobile, cancellation.OTP);
                return Ok(attemps);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BookingCancellationController.CancelBooking");
                objErr.SendMail();
                return InternalServerError();
            }
        } 
    }
}

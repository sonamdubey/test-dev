using Bikewale.DTO.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Mobile Verified Controller
    /// Author  :   Sumit Kate
    /// Created On  : 21 Aug 2015
    /// </summary>
    public class DPQMobileVerifiedController : ApiController
    {
        private readonly IDealerPriceQuote _objDealer = null;
        public DPQMobileVerifiedController(IDealerPriceQuote objDealer)
        {
            _objDealer = objDealer;
        }
        /// <summary>
        /// Updates the mobile number verification for a generated Price Quote
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(DPQMobileVerifiedOutput))]
        public IHttpActionResult Post([FromBody]DPQMobileVerifiedInput input)
        {
            bool isSuccess = false;
            DPQMobileVerifiedOutput output = null;
            try
            {
                isSuccess = _objDealer.UpdateIsMobileVerified(input.PQId);
                if (isSuccess)
                {
                    output = new DPQMobileVerifiedOutput();
                    output.IsSuccess = isSuccess;
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DPQMobileVerifiedController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}

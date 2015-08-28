using Bikewale.DTO.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Bikewale.Service.TCAPI;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.MobileVerification
{
    /// <summary>
    /// Mobile Verification Controller
    /// </summary>
    public class PQMobileVerificationController : ApiController
    {
        private readonly IDealerPriceQuote _objDealer = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;

        public PQMobileVerificationController(IDealerPriceQuote objDealer, IMobileVerificationRepository mobileVerRespo)
        {
            _objDealer = objDealer;
            _mobileVerRespo = mobileVerRespo;
        }
        /// <summary>
        /// Mobile Verification method
        /// </summary>
        /// <param name="input">Mobile Verification Input</param>
        /// <returns></returns>
        [ResponseType(typeof(PQMobileVerificationOutput))]
        public IHttpActionResult Post([FromBody]PQMobileVerificationInput input)
        {
            PQMobileVerificationOutput output = null;
            bool isSuccess = false;
            try
            {
                if (!_mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                {
                    if (_mobileVerRespo.VerifyMobileVerificationCode(input.CustomerMobile, input.CwiCode, ""))
                    {
                        isSuccess = _objDealer.UpdateIsMobileVerified(input.PQId);

                        // if mobile no is verified push lead to autobiz
                        if (isSuccess)
                        {
                            AutoBizAdaptor.PushInquiryInAB(input.BranchId, input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail, input.VersionId, input.CityId);
                        }
                    }
                }
                if (isSuccess)
                {
                    output = new PQMobileVerificationOutput();
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.PQMobileVerificationController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}

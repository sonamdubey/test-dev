using Bikewale.DTO.PriceQuote.MobileVerification;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
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
    /// Resend Mobile Verification Code Controller
    /// Author  :   Sumit Kate
    /// Created on : 24 Aug 2015
    /// </summary>
    public class ResendVerificationCodeController : ApiController
    {
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerification _mobileVerification = null;

        public ResendVerificationCodeController(IMobileVerificationRepository mobileVerRespo, IMobileVerification mobileVerification)
        {
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerification;
        }
        /// <summary>
        /// Verified the resend mobile verification code
        /// </summary>
        /// <param name="input">entity</param>
        /// <returns></returns>
        [ResponseType(typeof(PQResendMobileVerificationOutput))]
        public IHttpActionResult Post([FromBody]PQResendMobileVerificationInput input)
        {
            PQResendMobileVerificationOutput output = null;
            bool isSuccess = false;
            MobileVerificationEntity mobileVer = null;
            try
            {
                if (!_mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                {
                    mobileVer = _mobileVerification.ProcessMobileVerification(input.CustomerEmail, input.CustomerMobile);

                    SMSTypes st = new SMSTypes();
                    st.SMSMobileVerification(mobileVer.CustomerMobile, input.CustomerName, mobileVer.CWICode, input.Source);

                    isSuccess = true;
                }
                output = new PQResendMobileVerificationOutput();
                output.IsSuccess = isSuccess;
                if (isSuccess)
                {
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.ResendVerificationCodeController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}

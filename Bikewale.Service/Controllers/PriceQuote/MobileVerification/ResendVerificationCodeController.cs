using Bikewale.DTO.PriceQuote.MobileVerification;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.MobileVerification
{
    /// <summary>
    /// Resend Mobile Verification Code Controller
    /// Author  :   Sumit Kate
    /// Created on : 24 Aug 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class ResendVerificationCodeController : CompressionApiController//ApiController
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
            sbyte noOfAttempts = 0;
            MobileVerificationEntity mobileVer = null;
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerEmail) && !String.IsNullOrEmpty(input.CustomerMobile))
                {
                    noOfAttempts = _mobileVerRespo.OTPAttemptsMade(input.CustomerMobile, input.CustomerEmail);

                    //here -1 implies mobile number is verified and resend OTP attempts is 2
                    if (noOfAttempts > -1)
                    {
                        if (noOfAttempts < 3)
                        {
                            mobileVer = _mobileVerification.ProcessMobileVerification(input.CustomerEmail, input.CustomerMobile);

                            SMSTypes st = new SMSTypes();
                            st.SMSMobileVerification(mobileVer.CustomerMobile, input.CustomerName, mobileVer.CWICode, input.Source);
                        }

                        isSuccess = true;
                    }

                    output = new PQResendMobileVerificationOutput();
                    output.IsSuccess = isSuccess;
                    output.NoOfAttempts = noOfAttempts;

                    if (isSuccess)
                    {
                        return Ok(output);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.ResendVerificationCodeController.Post");
               
                return InternalServerError();
            }
        }
    }
}

using Bikewale.Interfaces.MobileVerification;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Customer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 26 Sep 2016
    /// Description :   Mobile Verification Controller
    /// </summary>
    public class MobileVerificationController : ApiController
    {
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        /// <summary>
        /// To initialize the member variables
        /// </summary>
        /// <param name="mobileVerRespo"></param>
        public MobileVerificationController(IMobileVerificationRepository mobileVerRespo)
        {
            _mobileVerRespo = mobileVerRespo;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Sep 2016
        /// Description :   Performs the mobile verification for a customer via otp
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        [HttpPost, ResponseType(typeof(bool)), Route("api/mobileverification/validateotp/")]
        public IHttpActionResult ValidateOTP(string mobile, string otp)
        {
            if (!String.IsNullOrEmpty(mobile) && !String.IsNullOrEmpty(otp))
            {
                if (_mobileVerRespo.VerifyMobileVerificationCode(mobile, otp, ""))
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

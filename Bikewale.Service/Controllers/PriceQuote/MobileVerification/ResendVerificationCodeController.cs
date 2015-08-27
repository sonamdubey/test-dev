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
        /// <summary>
        /// Verified the resend mobile verification code
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQResendMobileVerificationOutput))]
        public HttpResponseMessage Post([FromBody]PQResendMobileVerificationInput input)
        {
            PQResendMobileVerificationOutput output = null;
            bool isSuccess = false;
            MobileVerificationEntity mobileVer = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IMobileVerificationRepository, Bikewale.BAL.MobileVerification.MobileVerification>();
                    IMobileVerificationRepository mobileVerRespo = container.Resolve<IMobileVerificationRepository>();

                    container.RegisterType<IMobileVerification, Bikewale.BAL.MobileVerification.MobileVerification>();
                    IMobileVerification mobileVerificetion = container.Resolve<IMobileVerification>();

                    if (!mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                    {
                        mobileVer = mobileVerificetion.ProcessMobileVerification(input.CustomerEmail, input.CustomerMobile);

                        SMSTypes st = new SMSTypes();
                        st.SMSMobileVerification(mobileVer.CustomerMobile, input.CustomerName, mobileVer.CWICode, input.Source);

                        isSuccess = true;
                    }

                }
                output = new PQResendMobileVerificationOutput();
                output.IsSuccess = isSuccess;
                if (isSuccess)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, output);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.ResendVerificationCodeController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }            
        }
    }
}

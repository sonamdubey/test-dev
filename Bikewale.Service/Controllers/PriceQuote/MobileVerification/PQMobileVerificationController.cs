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
        /// <summary>
        /// Mobile Verification method
        /// </summary>
        /// <param name="input">Mobile Verification Input</param>
        /// <returns></returns>
        [ResponseType(typeof(PQMobileVerificationOutput))]
        public HttpResponseMessage Post([FromBody]PQMobileVerificationInput input)
        {
            PQMobileVerificationOutput output = null;
            bool isSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    container.RegisterType<IMobileVerificationRepository, Bikewale.BAL.MobileVerification.MobileVerification>();
                    IMobileVerificationRepository mobileVerRespo = container.Resolve<IMobileVerificationRepository>();

                    if (!mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                    {
                        if (mobileVerRespo.VerifyMobileVerificationCode(input.CustomerMobile, input.CwiCode, ""))
                        {
                            isSuccess = objDealer.UpdateIsMobileVerified(input.PQId);

                            // if mobile no is verified push lead to autobiz
                            if (isSuccess)
                            {
                                AutoBizAdaptor.PushInquiryInAB(input.BranchId, input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail, input.VersionId, input.CityId);
                            }
                        }
                    }
                }
                if (isSuccess)
                {
                    output = new PQMobileVerificationOutput();
                    output.IsSuccess = isSuccess;
                    return Request.CreateResponse(HttpStatusCode.OK, output);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Mobile number verification not succeeded.");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.PQMobileVerificationController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }            
        }
    }
}

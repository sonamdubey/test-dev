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
        /// <summary>
        /// Updates the mobile number verification for a generated Price Quote
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(DPQMobileVerifiedOutput))]
        public HttpResponseMessage Post([FromBody]DPQMobileVerifiedInput input)
        {
            bool isSuccess = false;
            DPQMobileVerifiedOutput output = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                    isSuccess = objDealer.UpdateIsMobileVerified(input.PQId);
                    if (isSuccess)
                    {
                        output = new DPQMobileVerifiedOutput();
                        output.IsSuccess = isSuccess;
                        return Request.CreateResponse(HttpStatusCode.OK, output);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotModified, "Mobile Verification failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DPQMobileVerifiedController.Post");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}

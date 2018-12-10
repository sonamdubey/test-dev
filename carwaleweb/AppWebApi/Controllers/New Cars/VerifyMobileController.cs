using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppWebApi.Models;
using AppWebApi.Common;
using Carwale.Utility;
using Carwale.Entity.Enum;
using System.Web;
using System.Web.Http.Cors;

namespace AppWebApi.Controllers
{
    public class VerifyMobileController : ApiController
    {
        [EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public HttpResponseMessage Get(HttpRequestMessage request, string mobileNo, string cwiCode)
        {
            if (CommonOpn.SkipValidation ||
                (request.Headers.Contains("CWK") &&
                request.Headers.Contains("SourceId") &&
                CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First(), request.Headers.GetValues("CWK").First()))
                )
            {
                Platform source = HttpContextUtils.GetHeader<Platform>("sourceID");
                string ampOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
                if (!string.IsNullOrWhiteSpace(ampOrigin))
                {
                    HttpContextUtils.AddAmpHeaders(ampOrigin, true);
                    source = Platform.CarwaleMobile;
                }

                string clientTokenId = !string.IsNullOrEmpty(HttpContextUtils.GetCookie("CWC")) ? HttpContextUtils.GetCookie("CWC") : HttpContextUtils.GetHeader<string>("IMEI");
                VerifyMobile vm = new VerifyMobile(mobileNo, cwiCode, source, clientTokenId);
                if (vm.ServerErrorOccured)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                }
                else
                {
                    return !string.IsNullOrWhiteSpace(ampOrigin) && vm.ResponseCode != "1" ? Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid OTP") : Request.CreateResponse(HttpStatusCode.OK, vm);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}
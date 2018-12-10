using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppWebApi.Models;
using AppWebApi.Common;

namespace AppWebApi.Controllers
{
    public class TipsAndAdvicesDetailController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string basicId, string priority,string subCatId = "-1")
        {
            if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                TipsAndAdvicesDetail tad = new TipsAndAdvicesDetail(basicId, priority, subCatId);

                if (!tad.ServerErrorOccured)
                    return Request.CreateResponse<TipsAndAdvicesDetail>(HttpStatusCode.OK, tad);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}
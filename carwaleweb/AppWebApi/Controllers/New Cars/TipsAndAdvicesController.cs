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
    public class TipsAndAdvicesController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string subCatId, string pageNo, string pageSize)
        {
            if (CommonOpn.CheckValidId(subCatId) && CommonOpn.CheckId(pageNo) && CommonOpn.CheckId(pageSize) && pageNo.Trim() != "0" && pageSize != "0")
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    TipsAndAdvices ta = new TipsAndAdvices(subCatId, pageNo, pageSize);
                    if (!ta.ServerErrorOccured)
                        return Request.CreateResponse<TipsAndAdvices>(HttpStatusCode.OK, ta);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}
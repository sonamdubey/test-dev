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
    public class UpComingCarDetailController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string id)
        {
            if (CommonOpn.CheckId(id))
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    UpComingCarDetail upcd = new UpComingCarDetail(id);
                    if (!upcd.ServerErrorOccured)
                        return Request.CreateResponse<UpComingCarDetail>(HttpStatusCode.OK, upcd);
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

        public HttpResponseMessage Get(HttpRequestMessage request, string id,string makeId)
        {
            if (CommonOpn.CheckId(id) && CommonOpn.CheckId(makeId))
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    UpComingCarDetail upcd = new UpComingCarDetail(id, makeId);

                    if (!upcd.ServerErrorOccured)
                        return Request.CreateResponse<UpComingCarDetail>(HttpStatusCode.OK, upcd);
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
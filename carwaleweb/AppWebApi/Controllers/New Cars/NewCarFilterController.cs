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
    public class NewCarFilterController : ApiController
    {
        //public HttpResponseMessage Get()
        //{
        //    NewCarFilter ncf = new NewCarFilter();
        //    if (!ncf.ServerErrorOccurred)
        //        return Request.CreateResponse<NewCarFilter>(HttpStatusCode.OK, ncf);
        //    else
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
        //}

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                NewCarFilter ncf = new NewCarFilter();

                if (!ncf.ServerErrorOccurred)
                    return Request.CreateResponse<NewCarFilter>(HttpStatusCode.OK, ncf);
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
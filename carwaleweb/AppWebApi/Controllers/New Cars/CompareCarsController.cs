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
    public class CompareCarsController : ApiController
    {
        /*
        Author:Rakesh Yadav
        Date Created: 09 Apr 2014
        Desc: Return top 2 cars for comparision
        */
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                CompareCars cc = new CompareCars("1", "2");

                if (!cc.ServerErrorOccurred)
                    return Request.CreateResponse<CompareCars>(HttpStatusCode.OK, cc);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
    }
}
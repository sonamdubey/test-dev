using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppWebApi.Common;
using AppWebApi.Models;

namespace AppWebApi.Controllers
{
    public class CompareCarsDetailsController : ApiController
    {
        /*
        Author:Rakesh Yadav
        Date Created: 09 Apr 2014
        Desc: Return detailed comaparion of cars
        */
        public HttpResponseMessage Get(HttpRequestMessage request,string version1,string version2)
        {
            if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                CompareCarsDetails cc = new CompareCarsDetails(Convert.ToInt32(version1), Convert.ToInt32(version2));

                if (!cc.ServerErrorOccurred)
                    return Request.CreateResponse<CompareCarsDetails>(HttpStatusCode.OK, cc);
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
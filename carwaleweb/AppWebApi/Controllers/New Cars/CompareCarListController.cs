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
    public class CompareCarListController : ApiController
    {
        /*
        Author:Rakesh Yadav
        Date Created: 09 Apr 2014
        Desc: Return pagewise list of cars for comparision
        */
        public HttpResponseMessage Get(HttpRequestMessage request,string pageNo,string pageSize)
        {
            if (CommonOpn.CheckId(pageNo) && CommonOpn.CheckId(pageSize))
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    CompareCars cc = new CompareCars(pageNo, pageSize);

                    if (!cc.ServerErrorOccurred)
                        return Request.CreateResponse<CompareCars>(HttpStatusCode.OK, cc);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on the server");
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
    }
}
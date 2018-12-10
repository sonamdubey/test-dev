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
    public class UpComingCarsController : ApiController
    {
        /*
         Author:Rakesh Yadav
         Date Created: 30 July 2013
         Desc: Searching upcoming cars of all makes
         */
        public HttpResponseMessage Get(HttpRequestMessage request, string pageNo, string pageSize)
        {
            if (CommonOpn.CheckId(pageNo) && CommonOpn.CheckId(pageSize) && pageNo.Trim() != "0" && pageSize.Trim() != "0")
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    UpComingCars upc = new UpComingCars(pageNo, pageSize);

                    if (!upc.ServerErrorOccured)
                        return Request.CreateResponse<UpComingCars>(HttpStatusCode.OK, upc);
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

        /*
         Author:Rakesh Yadav
         Date Created: 8 Nov 2013
         Desc: Searching upcoming cars of specific makes
         */
        public HttpResponseMessage Get(HttpRequestMessage request, string makeId, string pageNo, string pageSize)
        {
            if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                UpComingCars upc = new UpComingCars(makeId, pageNo, pageSize);

                if (!upc.ServerErrorOccured)
                    return Request.CreateResponse<UpComingCars>(HttpStatusCode.OK, upc);
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
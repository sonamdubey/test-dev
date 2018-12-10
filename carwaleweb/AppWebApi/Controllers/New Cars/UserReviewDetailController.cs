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
    /*
    Author:Rakesh Yadav
    Date Created: 18 july 2013
    Desc: 
    */
    public class UserReviewDetailController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request  ,string reviewId)
        {
            if (CommonOpn.CheckId(reviewId))
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {

                    UserReviewDetail urd = new UserReviewDetail(reviewId);
                    if (!urd.ServerErrorOccured)
                        return Request.CreateResponse<UserReviewDetail>(HttpStatusCode.OK, urd);
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
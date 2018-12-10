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
    public class ReviewsCommentsController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string reviewId,string pageNo,string pageSize)
        {
            if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
            {
                ReviewsComments rc = new ReviewsComments(reviewId, pageNo, pageSize);
                if (!rc.ServerErrorOccured)
                    return Request.CreateResponse<ReviewsComments>(HttpStatusCode.OK, rc);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong on server");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}
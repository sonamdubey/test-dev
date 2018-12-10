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
    public class UserReviewsController : ApiController
    {
        [AcceptVerbs("GET")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, string modelId, string versionId, string pageNo, string pageSize, string sortCriteria)
        {
            if (CommonOpn.CheckId(modelId) && CommonOpn.CheckValidId(versionId) && CommonOpn.CheckId(pageNo) && CommonOpn.CheckId(pageSize) && CommonOpn.CheckValidId(sortCriteria) && pageNo.Trim() != "0" && pageSize.Trim() != "0")
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    UserReviews ur = new UserReviews(modelId, versionId, pageNo, pageSize, sortCriteria);
                    if (!ur.ServerErrorOccured)
                        return Request.CreateResponse<UserReviews>(HttpStatusCode.OK, ur);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppWebApi.Models;
using AppWebApi.Common;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace AppWebApi.Controllers
{
    /*
     Author: Rakesh Yadav   
     Date Created:01 Aug 2013
     */
    public class NewsDetailController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string id)
        {
            if (CommonOpn.CheckId(id))
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    var newsDetails = CMSAppCache.GetContentDetails(id);
                    if (!string.IsNullOrEmpty(newsDetails))
                    {
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(newsDetails, Encoding.UTF8, "application/json");
                        return response;
                    }
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
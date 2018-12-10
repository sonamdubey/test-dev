﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppWebApi.Models;
using AppWebApi.Common;

namespace AppWebApi.Controllers
{
    public class PqModelsController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string makeId)
        {
            if (CommonOpn.CheckId(makeId))
            {
                if (CommonOpn.SkipValidation || (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && CommonOpn.IsValidSource(request.Headers.GetValues("SourceId").First().ToString(), request.Headers.GetValues("CWK").First().ToString())))
                {
                    PqModels pm = new PqModels(makeId);
                    if (!pm.ServerErrorOccured)
                        return Request.CreateResponse<PqModels>(HttpStatusCode.OK, pm);
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
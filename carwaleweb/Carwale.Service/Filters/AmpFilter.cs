using System;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;
using System.Web.Http.Controllers;

namespace Carwale.Service.Filters
{
    class AmpFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string qs = actionContext.Request.RequestUri.Query;

            if (!String.IsNullOrWhiteSpace(qs))
            {
                NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(qs);
                
                bool isAmp;
                Boolean.TryParse(nvc["isamp"], out isAmp);

                if (isAmp)
                {
                    string sourceOrigin = nvc["__amp_source_origin"];
                    if (String.IsNullOrEmpty(sourceOrigin))
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                    else
                    {
                        if (!(sourceOrigin.Contains("carwale.com") || sourceOrigin.Contains("localhost")))
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                    }
                }
            }
            else
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
    }
}

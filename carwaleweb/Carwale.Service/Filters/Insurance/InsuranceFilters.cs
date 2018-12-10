using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters
{
    public class MakeOrModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var querystringPrm = HttpContext.Current.Request.QueryString;

            bool isValidRequest = true;
            string makeId = querystringPrm["makeId"] == null ? "0" : querystringPrm["makeId"];
            string modelId = querystringPrm["modelId"] == null ? "0" : querystringPrm["modelId"];

            if (!(RegExValidations.IsPositiveNumber(makeId) || RegExValidations.IsPositiveNumber(modelId)))
            {
                isValidRequest = false;
            }

            if (!isValidRequest)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
            }
        }
    }

    public class ClientFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var querystringPrm = HttpContext.Current.Request.QueryString;

            bool isValidRequest = true;
            string clientId = querystringPrm["clientId"] == null ? "0" : querystringPrm["clientId"];

            if (!RegExValidations.IsPositiveNumber(clientId))
            {
                isValidRequest = false;
            }

            if (!isValidRequest)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
            }
        }
    }
}

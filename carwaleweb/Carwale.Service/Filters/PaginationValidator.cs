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
    public class PaginationValidator : ActionFilterAttribute
    {
        /// <summary>
        /// for parameters to be passed in querysting for citybylatlong api validator
        /// written by Natesh kumar on 5/11/14
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //var querystringPrm = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value); // mapping value of parameter corresponding to keys

            var querystringPrm = HttpContext.Current.Request.QueryString;

            bool isValidRequest = true;

            string pageno = querystringPrm["pageno"] == null ? "0" : querystringPrm["pageno"];
            string pagesize = querystringPrm["pagesize"] == null ? "0" : querystringPrm["pagesize"];

            if (!RegExValidations.IsNumeric(pageno))
            {
                isValidRequest = false;
            }

            if (!RegExValidations.IsNumeric(pagesize))
            {
                isValidRequest = false;
            }

            if (isValidRequest)
            {
                if (Convert.ToInt32(pageno) < 1 || Convert.ToInt32(pagesize) < 1) { isValidRequest = false; }
            }

            if (!isValidRequest)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Pagination");
            }
        }
    }
}

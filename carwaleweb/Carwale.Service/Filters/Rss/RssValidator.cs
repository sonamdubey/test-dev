using Carwale.Entity.URIs.RSSURI;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters.Rss
{
    public class RssValidator : ActionFilterAttribute
    {

        /// <summary>
        /// rss feed api validator
        /// created by natesh kumar 12/12/14
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            var querystringPrm = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);  // mapping value of parameter corresponding to keys

            bool isValidRequest = true;

            if (!RegExValidations.IsNumeric(querystringPrm["applicationid"]))
            {
                isValidRequest = false;
            }
            else
            {
                ushort appid = Convert.ToUInt16(querystringPrm["applicationid"]);
                if (appid != 1 && appid != 2)
                {
                    isValidRequest = false;
                }
            }

            string categoryIdList = querystringPrm["categoryidlist"];

            if (!RegExValidations.ValidateCommaSeperatedNumbers(categoryIdList))
            {
                isValidRequest = false;
            }

            if (!RegExValidations.IsNumeric(querystringPrm["days"]))
            {
                isValidRequest = false;
            }
            else
            {
                uint days = Convert.ToUInt32(querystringPrm["days"]);
                if (days == 0)
                {
                    isValidRequest = false;
                }
            }


            if (!isValidRequest)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
            }
        }
    }
}

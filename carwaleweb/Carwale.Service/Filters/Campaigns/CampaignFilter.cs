using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Carwale.Service.Filters.Campaigns
{
    public class CampaignFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string qs = actionContext.Request.RequestUri.Query;

            if (!String.IsNullOrWhiteSpace(qs))
            {
                NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(qs);
                if (!RegExValidations.IsNumeric(nvc["sourceId"].ToString()) || !RegExValidations.IsNumeric(nvc["cityId"].ToString()) || !RegExValidations.IsNumeric(nvc["modelId"].ToString()) || !RegExValidations.IsNumeric(nvc["zoneId"].ToString()))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            else
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
    }
}

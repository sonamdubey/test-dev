
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using Carwale.Entity.Enum;

namespace Carwale.Service.Filters
{
    public class ValidateSourceFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> requestSource;
            actionContext.Request.Headers.TryGetValues("sourceid", out requestSource);
            if (requestSource != null && requestSource.Any())
            {
                int sourceId;
                if (!Int32.TryParse(requestSource.ToList()[0], out sourceId) || !Enum.IsDefined(typeof(Platform), sourceId))
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }
    }
}

using Carwale.Notifications.Logs;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            Logger.LogException(actionExecutedContext.Exception);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong. Please try again later!");
        }
    }
}

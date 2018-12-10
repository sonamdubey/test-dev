using Carwale.Notifications.Logs;
using System;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters.ExceptionFilters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiLogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null)
            {
                Logger.LogException(actionExecutedContext.Exception);
            }
        }
    }
}

using Carwale.Entity;
using Carwale.Notifications.Logs;
using System.IO;
using System.Web.Http.Filters;

namespace Carwale.Service.Filters
{
    public class LogApiAttribute : ActionFilterAttribute
    {
        public bool LogSuccess { get; set; } = true;

        public override void OnActionExecuted(HttpActionExecutedContext httpContext)
        {
            if (LogSuccess || (httpContext.Response != null && !httpContext.Response.IsSuccessStatusCode))
            {
                ApiLogData logData = new ApiLogData();
                var reqStream = httpContext.Request.Content.ReadAsStreamAsync().Result;
                if (reqStream.CanSeek)
                {
                    reqStream.Position = 0;
                }
                using (var reader = new StreamReader(reqStream))
                {
                    logData.RequestContent = reader.ReadToEnd();
                }
                logData.RequestHeaders = httpContext.Request.Headers.ToString();
                logData.RequestMethod = httpContext.Request.Method.Method;
                logData.RequestUri = httpContext.Request.RequestUri.OriginalString;

                if (httpContext.Response.Content != null)
                {
                    logData.ResponseContent = httpContext.Response.Content.ReadAsStringAsync().Result;
                }
                logData.ResponseStatusCode = (int)httpContext.Response.StatusCode;
                Logger.LogApiData(logData);
            }
        }
    }
}

using Carwale.Entity;
using Carwale.Interfaces.Logs;
using Carwale.Utility;
using Newtonsoft.Json;
using System.IO;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Carwale.Service.Filters
{
    public class LogRequestAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private ILoggingRepository _logRequest
        {
            get
            {
                return DependencyResolver.Current.GetService<ILoggingRepository>();
            }
        }
  
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            ApiLogData logData = new ApiLogData();
            var reqStream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result;
            if (reqStream.CanSeek)
            {
                reqStream.Position = 0;
            }
            using (var reader = new StreamReader(reqStream))
            {
                logData.RequestContent = reader.ReadToEnd();
            }
            logData.RequestHeaders = actionExecutedContext.Request.Headers.ToString();
            logData.RequestMethod = actionExecutedContext.Request.Method.Method;
            logData.RequestUri = actionExecutedContext.Request.RequestUri.OriginalString;
            if (actionExecutedContext.Response != null)
            {
                logData.ResponseStatusCode = (int)actionExecutedContext.Response.StatusCode;
            }
            logData.ClientIp = UserTracker.GetUserIp();
            string jsonData = JsonConvert.SerializeObject(logData);
            _logRequest.LogRequestBody(jsonData, logData.RequestMethod);
        }
    }
}

using Bikewale.Service.UnityConfiguration;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Bikewale.Service
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityResolver(UnityBootstrapper.Initialize());

            config.Filters.Add(new LogExceptionFilterAttribute());
            //// Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 12 Mar 2018
    /// Description :   Log Exception FilterAttribute
    /// </summary>
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Bikewale.Notifications.ErrorClass.LogError(context.Exception, "LogExceptionFilterAttribute.OnException");
        }
    }
}

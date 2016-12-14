using Bikewale.Service.UnityConfiguration;
using System.Web.Http;
using System.Web.Http.Cors;

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

            // Web API configuration and services
            // Enable CORS
            if (Bikewale.Utility.BWConfiguration.Instance.CORSEnabled)
            {
                var cors = new EnableCorsAttribute(origins: Bikewale.Utility.BWConfiguration.Instance.CORSSite, headers: "*", methods: Bikewale.Utility.BWConfiguration.Instance.CORSMethod);

                config.EnableCors(cors);
            }

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

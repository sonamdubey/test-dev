using BikewaleOpr.Service.UnityConfiguration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BikewaleOpr.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityResolver(UnityBootstrapper.Initialize());
            // Web API configuration and services
            // Enable CORS
            var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*");
            config.EnableCors(cors);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Bikewale.Service.UnityConfiguration;
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

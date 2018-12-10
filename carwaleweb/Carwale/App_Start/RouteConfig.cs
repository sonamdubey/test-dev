using Carwale.Service;
using Carwale.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Carwale
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //register dependency resolver for MVC
            DependencyResolver.SetResolver(new Unity.Mvc3.UnityDependencyResolver(UnityBootstrapper.Initialise()));
            CarDetailsMapper.CreateMaps();

            routes.MapMvcAttributeRoutes();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("ajaxpro/{*pathInfo}");
            routes.IgnoreRoute("ajax/{*pathInfo}");
            routes.IgnoreRoute("default.aspx");

            //routes.RouteExistingFiles = true;
            routes.IgnoreRoute("m/default.aspx");           
            
            routes.MapRoute(
                name: "carwale.ui.m",
                url: "m/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Carwale.MobileWeb.Controllers" }
            );

            routes.MapRoute(
                name: "carwale.ui",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Carwale.Controllers" }
            );
            //
        }
    }
}

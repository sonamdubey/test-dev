﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Bikewale
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("ajaxpro/{*pathInfo}");
            routes.IgnoreRoute("ajax/{*pathInfo}");
            routes.IgnoreRoute("default.aspx");
            routes.IgnoreRoute("m/default.aspx");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*allcss}", new { allcss = @".*\.css(/.*)?" });
            routes.IgnoreRoute("{*alljs}", new { alljs = @".*\.js(/.*)?" });


            routes.MapRoute(
                name: "bikewale.ui.m",
                url: "m/{controller}/{action}/{id}"
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                //namespaces: new[] { "Bikewale.Mobile.Controllers" }
            );

            routes.MapRoute(
                name: "bikewale.ui",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "HomePage", action = "Index", id = UrlParameter.Optional }
                //namespaces: new[] { "Bikewale.Desktop.Controllers" }
            );
        }
    }
}

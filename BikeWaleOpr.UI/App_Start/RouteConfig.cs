using System.Web.Mvc;
using System.Web.Routing;

namespace BikeWaleOpr.MVC.UI
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class to register all the MVC routes
    /// Modified By : Sajal Gupta 
    /// Summary : Api routing for mvc + other controllers
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("ajaxpro/{*pathInfo}");
            routes.IgnoreRoute("ajax/{*pathInfo}");
            routes.IgnoreRoute("m/default.aspx");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                    name: "Default.Opr.UI",
                    url: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}

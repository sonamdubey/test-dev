using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bikewale.Service
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 28 Apr 2014
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "bikewale.service",
                routeTemplate: "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
                name: "bikewale.service.bikedata",
                routeTemplate: "api/{controller}/"
            );
        }
    }
}
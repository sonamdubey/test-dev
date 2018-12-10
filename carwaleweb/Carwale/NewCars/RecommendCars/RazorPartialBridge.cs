using Carwale.Entity.Enum;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Carwale.UI.NewCars.RecommendCars
{
    public class RazorPartialBridge
    {
        public class WebFormShimController : Controller { }
        public static void RenderPartial(string partialName, object model = null)
        {
            //get a wrapper for the legacy WebForm context
            var httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);

            //create a mock route that points to the empty controller
            var rt = new RouteData();
            rt.Values.Add("controller", "WebFormShimController");

            //create a controller context for the route and http context
            var ctx = new ControllerContext(
                new RequestContext(httpCtx, rt), new WebFormShimController());

            //find the partial view using the viewengine
            var view = ViewEngines.Engines.FindPartialView(ctx, partialName).View;

            //create a view context and assign the model
            var vctx = new ViewContext(ctx, view,
                new ViewDataDictionary { Model = model },
                new TempDataDictionary(), httpCtx.Response.Output);

            //render the partial view
            view.Render(vctx, System.Web.HttpContext.Current.Response.Output);
        }

        public static void RenderAction(string actionName, string controllerName, IDictionary<string,object> param = null)
        {
            try
            {
                //get HttpContextBase
                var httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);

                //create a mock route that points to controller
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                routeData.Values.Add("action", actionName);

                if(param != null)
                {
                    foreach(var item in param)
                    {
                        routeData.Values.Add(item.Key, item.Value);
                    }
                }

                //create request context for the controller
                var requestCtx = new RequestContext(httpCtx, routeData);

                // Instantiate the controller and call Execute
                IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                IController controller = factory.CreateController(requestCtx, controllerName);
                controller.Execute(requestCtx);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
﻿using log4net;
using RazorEngine;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bikewale.Utility
{
    public static class MvcHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MvcHelper));
        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jun 2017
        /// Description :   Returns the Render engine raw output
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetRenderedContent<T>(string templateName, string template, T model)
        {
            try
            {
                var renderedContent = Razor.Parse(template, model, templateName);
                return renderedContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Render<T>(string templateName, T model, string template)
        {
            var contextProps = ThreadContext.Properties;
            // loading a template might be expensive, so be careful to cache content
            try
            {
                if (Razor.Resolve(templateName) == null)
                {
                    // we've never seen this template before, so compile it and stick it in cache.                
                    Razor.Compile(template, typeof(T), templateName);
                    _logger.Error(String.Format("Compile {0}", templateName));
                }

                // by now, we know we've got a the template cached and ready to run; this is fast
                return Razor.Run(templateName, model);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Creates an instance of an MVC controller from scratch 
        /// when no existing ControllerContext is present       
        /// </summary>
        /// <typeparam name="T">Type of the controller to create</typeparam>
        /// <returns>Controller Context for T</returns>
        /// <exception cref="InvalidOperationException">thrown if HttpContext not available</exception>
        public static T CreateController<T>(RouteData routeData = null)
                    where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string RenderViewToString<T>(ControllerContext context, string viewName, T model)
        {
            try
            {
                if (context != null)
                {

                    if (string.IsNullOrEmpty(viewName))
                        viewName = context.RouteData.GetRequiredString("action");

                    ViewDataDictionary viewData = new ViewDataDictionary(model);

                    using (StringWriter sw = new StringWriter())
                    {
                        ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                        ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                        viewResult.View.Render(viewContext, sw);

                        return sw.GetStringBuilder().ToString();
                    }
                }
                else return string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

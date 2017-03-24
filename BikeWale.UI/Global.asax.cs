﻿using Bikewale.Utility;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WURFL;
using WURFL.Config;
using System.Web.WebPages;
using Bikewale.Filters;

namespace Bikewale
{
    public class Global : System.Web.HttpApplication
    {
        public const String WurflManagerCacheKey = "__WurflManager";
        public const string WurflDataFilePath = "~/App_Data/wurfl.xml.gz";

        protected void Application_Start(object sender, EventArgs e)
        {
            ViewEngines.Engines.Add(new RazorViewEngine());
            log4net.Config.XmlConfigurator.Configure();
            UnityConfig.RegisterComponents();
            Bikewale.Service.WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.EnsureInitialized();

            //DisplayModeProvider.Instance.Modes.Clear();

            //// Added mobile device detection for the views
            //if (!BWConfiguration.Instance.DebugMobileSite)
            //{
            //    // Remove existing mobile device
            //    DisplayModeProvider.Instance.Modes.RemoveAt(0);

            //    // Custom mobile device detection for pages
            //    DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile")
            //    {
            //        ContextCondition = (ctx => ctx.IsMobileDevice())
            //    });
            //}

            ConfigureWurfl();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            CurrentUser.GenerateUniqueCookie();
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Jan 2017
        /// Summary : Method to register the wurfl manager for device detection
        /// </summary>
        private void ConfigureWurfl()
        {
            var wurflDataFile = HttpContext.Current.Server.MapPath(WurflDataFilePath);

            var configurer = new InMemoryConfigurer()
                     .MainFile(wurflDataFile);

            var manager = WURFLManagerBuilder.Build(configurer);
            HttpContext.Current.Cache[WurflManagerCacheKey] = manager;
        }
    }
}
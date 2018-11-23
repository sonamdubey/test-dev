﻿using ApiGatewayLibrary;
using Bikewale.Utility;
using log4net;
using React;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WURFL;
using WURFL.Config;

namespace Bikewale
{
    public class Global : System.Web.HttpApplication
    {
        public const String WurflManagerCacheKey = "__WurflManager";
        public const string WurflDataFilePath = "~/App_Data/wurfl-latest.zip";

        static ILog _logger = LogManager.GetLogger("Global");

        protected void Application_Start(object sender, EventArgs e)
        {
            ViewEngines.Engines.Add(new CustomViewEngine());
            UnityConfig.RegisterComponents();
            AutoMapperConfig.ConfigureAutoMapper();
            Bikewale.Service.WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.EnsureInitialized();

            ConfigureWurfl();

            if (BWConfiguration.Instance.EnablePWA)
                AssemblyRegistration.Container.Resolve<IReactEnvironment>();
            CustomGRPCLoadBalancer.InitializeGRPCLoadBalancer();
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 28 Feb 2018
        /// Description :   Call new Visitor Cookie generation code.
        /// Added exception handling code if Unique cookie throws an exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.GenerateUniqueCookieV2();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Application_PostAuthenticateRequest.CurrentUser.GenerateUniqueCookieV2");
            }
            if (HttpContext.Current.Request.Cookies.Get("_bwtest") == null)
            {
                CurrentUser.SetBikewaleABTestingUser();
            }
            if (HttpContext.Current.Request.Cookies.Get("_bwutmz") == null)
            {
                BWCookies.SetBWUtmz();
            }
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

        /// <summary>
        /// Creatde by  :   Sumit Kate on 12 Mar 2018
        /// Description :   Log exception in GreyLog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Bikewale.Notifications.ErrorClass.LogError(ex, "Global.Application_Error", _logger);
        }
    }

    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine()
        {
            base.ViewLocationFormats = new string[] 
            {
                "~/UI/Views/{1}/{0}.cshtml",
            "~/UI/Views/{2}/{1}/{0}.cshtml",
            "~/UI/Views/{3}/{2}/{1}/{0}.cshtml"
            };
        }
    }
}

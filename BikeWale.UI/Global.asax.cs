using Bikewale.Utility;
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
        public const string WurflDataFilePath = "~/App_Data/wurfl.xml.gz";

        protected void Application_Start(object sender, EventArgs e)
        {
            ViewEngines.Engines.Add(new RazorViewEngine());
            log4net.Config.XmlConfigurator.Configure();
            UnityConfig.RegisterComponents();
            Bikewale.Service.WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.EnsureInitialized();

            ConfigureWurfl();

            if (BWConfiguration.Instance.EnablePWA)
                AssemblyRegistration.Container.Resolve<IReactEnvironment>();
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

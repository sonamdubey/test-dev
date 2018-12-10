using Carwale.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using log4net;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using WURFL.Config;
using WURFL;
using Carwale.Utility;
using Carwale.UI.PresentationLogic;
using Carwale.UI.ClientBL;
using System.Web.Mvc;
using System.Net;
using Carwale.UI.Mappers;
using AEPLCore.Logging;
using ApiGatewayLibrary;

namespace Carwale
{
    public class Global : HttpApplication
    {
        public const String WurflManagerCacheKey = "__WurflManager";
        public const string WurflDataFilePath = "~/App_Data/wurfl.xml.gz";
        public bool isHttps = (new Uri(System.Configuration.ConfigurationManager.AppSettings["GoogleRedirectURL"]).Scheme == Uri.UriSchemeHttps);
        private static int _abTestKeyMinVal = CustomParser.parseIntObject(System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMinValue"] ?? "1");
        private static int _abTestKeyMaxVal = CustomParser.parseIntObject(System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMaxValue"] ?? "100");

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
			Logger log = LoggerFactory.GetLogger();
			log.LogError("Carwale app started");
            System.Web.Http.GlobalConfiguration.Configure(Carwale.Service.WebApiConfig.Register);
            AppWebApi.Routes.WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
            Carwale.RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var wurflDataFile = HttpContext.Current.Server.MapPath(WurflDataFilePath);
            
            var configurer = new InMemoryConfigurer()
                     .MainFile(wurflDataFile);
                     
            var manager = WURFLManagerBuilder.Build(configurer);
            HttpContext.Current.Cache[WurflManagerCacheKey] = manager;
            MvcHandler.DisableMvcResponseHeader = true;
            CustomGRPCLoadBalancer.InitializeGRPCLoadBalancer();
            MapperConfigurations.CreateMaps();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie abTestCookie = HttpContext.Current.Request.Cookies["_abtest"];
            var appVersionCookie = HttpContext.Current.Request.Headers["appVersion"];
            CookiesCustomers.SetDomain(HttpContext.Current);
            if (abTestCookie == null || string.IsNullOrEmpty(abTestCookie.Value))
            {
                Random randomValue = new Random();
                abTestCookie = new HttpCookie("_abtest");
                abTestCookie.Value = randomValue.Next(_abTestKeyMinVal, _abTestKeyMaxVal + 1).ToString();
                abTestCookie.Expires = DateTime.Now.AddMonths(3);
                abTestCookie.Path = "/";
                abTestCookie.Domain = CookiesCustomers.CookieDomain;
                HttpContext.Current.Response.Cookies.Add(abTestCookie);
            }
            var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
            HttpContext.Current.Items["IsEligibleForORP"] = appVersionCookie==null && (HttpContext.Current.Request.RawUrl.StartsWith("/m/") || (DeviceDetectionManager.IsMobile(httpContextWrapper)));
            GlobalUser.Authenticate();			
			IEnumerable<string> urlSegments = Request.Url.Segments;
            string ampOrigin = HttpContext.Current.Request.QueryString["__amp_source_origin"]?.ToString();
            if (!string.IsNullOrWhiteSpace(ampOrigin) || (!(urlSegments.Contains("api/") || urlSegments.Contains("webapi/")) && Request.Headers["X-AjaxPro-Method"] == null && !(Request.Headers["X-Requested-With"] ?? (Request.Headers["x-requested-with"] ?? string.Empty)).ToLower().Equals("xmlhttprequest")))
			{ 
				GlobalUser.TrackUserBehaviour(); 
				CookiesCustomers.SetCWUtmz();
			}
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            var httpException = ex as HttpException ?? new HttpException(500, "Internal Server Error", ex);

            if (httpException.GetHttpCode() != 404)
            {
                ILog log = LogManager.GetLogger("Global.asax");
                var Cookies = HttpContext.Current.Request.Cookies;

                log4net.ThreadContext.Properties["ClientIP"] = UserTracker.GetUserIp();
                log4net.ThreadContext.Properties["Browser"] = HttpContext.Current.Request.Browser.Type;
                log4net.ThreadContext.Properties["Referrer"] = HttpContext.Current.Request.UrlReferrer;
                log4net.ThreadContext.Properties["UserAgent"] = HttpContext.Current.Request.UserAgent;
                log4net.ThreadContext.Properties["PhysicalPath"] = HttpContext.Current.Request.PhysicalPath;
                log4net.ThreadContext.Properties["Host"] = HttpContext.Current.Request.Url.Host;
                log4net.ThreadContext.Properties["Url"] = HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString();
                log4net.ThreadContext.Properties["QueryString"] = HttpContext.Current.Request.QueryString.ToString();
                log4net.ThreadContext.Properties["CityAndZone"] = string.Format("{0};{1};", (Cookies["_CustCityIdMaster"] != null ? Cookies["_CustCityIdMaster"].Value ?? "NULL" : "NULL"), (Cookies["_CustZoneIdMaster"] != null ? Cookies["_CustZoneIdMaster"].Value ?? "NULL" : "NULL"));
                log4net.ThreadContext.Properties["ABTEST"] = Cookies["_abtest"] != null ? (Cookies["_abtest"].Value ?? "NULL") : "NULL";
                log4net.ThreadContext.Properties["Cookie"] = HttpContext.Current.Request.Headers["Cookie"] ?? "NULL";
                log.Error(ex.GetBaseException());
            }
        }


        //Added by Meet Shah on 4/8/16
        //Moved setting of _abtest cookie from client side to server side
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

    }
}
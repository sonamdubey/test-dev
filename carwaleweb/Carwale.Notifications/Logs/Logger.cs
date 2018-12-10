using Carwale.Entity;
using Carwale.Utility;
using log4net;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Carwale.Notifications.Logs
{
    public static class Logger
    {
        private static ILog log = LogManager.GetLogger(typeof(Logger));

        /// <summary>
        /// Logs an object in graylog as INFO
        /// </summary>
        /// <param name="info"></param>
        public static void LogInfo(object info)
        {
            InitializeLogger();
            log.Info(info);
        }

        /// <summary>
        /// Logs an object in graylog as ERROR
        /// </summary>
        /// <param name="error"></param>
        public static void LogError(object error)
        {
            InitializeLogger();
            log.Error(error);
        }

        /// <summary>
        /// Logs exception in graylog as ERROR
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogException(Exception exception, string message = null)
        {
            InitializeLogger();
            using (log4net.ThreadContext.Stacks["ExceptionType"].Push(exception.GetType().ToString()))
            using (log4net.ThreadContext.Stacks["ExceptionMethod"].Push(Convert.ToString(exception.TargetSite)))
            {
                log.Error(message ?? exception.Message, exception);
            }
        }

        /// <summary>
        /// Logs API request and response data in graylog as INFO
        /// </summary>
        /// <param name="apiLogData"></param>
        public static void LogApiData(ApiLogData apiLogData)
        {
            InitializeLogger();
            log.Info("JSONData: " + JsonConvert.SerializeObject(apiLogData));
        }

        private static void InitializeLogger()
        {
            if (HttpContext.Current != null)
            {
                log4net.ThreadContext.Properties["ClientIP"] = UserTracker.GetUserIp();
                log4net.ThreadContext.Properties["Browser"] = HttpContext.Current.Request.Browser.Type;
                log4net.ThreadContext.Properties["Referrer"] = HttpContext.Current.Request.UrlReferrer;
                log4net.ThreadContext.Properties["UserAgent"] = HttpContext.Current.Request.UserAgent;
                log4net.ThreadContext.Properties["PhysicalPath"] = HttpContext.Current.Request.PhysicalPath;
                log4net.ThreadContext.Properties["Host"] = HttpContext.Current.Request.Url.Host;
                log4net.ThreadContext.Properties["Url"] = HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"]??"NULL";
                log4net.ThreadContext.Properties["QueryString"] = HttpContext.Current.Request.QueryString.ToString();
                var Cookies = HttpContext.Current.Request.Cookies;
                log4net.ThreadContext.Properties["CityAndZone"] = string.Format("{0};{1};",
                    (Cookies["_CustCityIdMaster"] != null ? Cookies["_CustCityIdMaster"].Value ?? "NULL" : "NULL"),
                    (Cookies["_CustZoneIdMaster"] != null ? Cookies["_CustZoneIdMaster"].Value ?? "NULL" : "NULL"));
                log4net.ThreadContext.Properties["ABTEST"] = Cookies["_abtest"] != null ? (Cookies["_abtest"].Value ?? "NULL") : "NULL";
                log4net.ThreadContext.Properties["Cookie"] = HttpContext.Current.Request.Headers["Cookie"] ?? "NULL";
            }
        }
    }
}

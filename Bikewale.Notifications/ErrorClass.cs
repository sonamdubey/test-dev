using log4net;
using System;
using System.Web;

namespace Bikewale.Notifications
{
    public static class ErrorClass
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ErrorClass));

        public static void LogError(Exception ex, string pageUrl)
        {
            LogCurrentHttpParameters();
            log.Error(pageUrl, ex);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Dec 2016
        /// Description :   Log Current Http Parameters to GreyLog
        /// </summary>
        private static void LogCurrentHttpParameters()
        {
            HttpContext objTrace = HttpContext.Current;

            if (objTrace != null && objTrace.Request != null)
            {
                ThreadContext.Properties["ClientIP"] = Convert.ToString(objTrace.Request.ServerVariables["HTTP_CLIENT_IP"]);
                ThreadContext.Properties["Browser"] = objTrace.Request.Browser.Type;
                ThreadContext.Properties["Referrer"] = objTrace.Request.UrlReferrer;
                ThreadContext.Properties["UserAgent"] = objTrace.Request.UserAgent;
                ThreadContext.Properties["PhysicalPath"] = objTrace.Request.PhysicalPath;
                ThreadContext.Properties["Host"] = objTrace.Request.Url.Host;
                ThreadContext.Properties["Url"] = objTrace.Request.Url;
                ThreadContext.Properties["QueryString"] = Convert.ToString(objTrace.Request.QueryString);
                var Cookies = objTrace.Request.Cookies;
                if (Cookies != null)
                {
                    ThreadContext.Properties["BWC"] = (Cookies["BWC"] != null ? Cookies["BWC"].Value : "NULL");
                    ThreadContext.Properties["location"] = (Cookies["location"] != null ? Cookies["location"].Value : "NULL");
                }
            }
        }


    }//class
}   // namespace

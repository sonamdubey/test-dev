using log4net;
using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.Notifications
{
    public class PwaErrorClass
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PwaErrorClass));
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
    
        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public PwaErrorClass(Exception ex, string functionName,string pwaUrl="")
        {            
            LogCurrentHttpParameters(functionName,pwaUrl);
            log.Error(functionName, ex);
        }        


        /// <summary>
        /// Created by  :   Sumit Kate on 21 Dec 2016
        /// Description :   Log Current Http Parameters to GreyLog
        /// </summary>
        private void LogCurrentHttpParameters(string functionName="", string pwaUrl="")
        {
            if (objTrace != null && objTrace.Request != null)
            {
                if(!string.IsNullOrEmpty(functionName))
                    log4net.ThreadContext.Properties["FunctionName"] = functionName;
                if(!string.IsNullOrEmpty(pwaUrl))
                    log4net.ThreadContext.Properties["Pwa-Url"] = pwaUrl;
                log4net.ThreadContext.Properties["ClientIP"] = Convert.ToString(objTrace.Request.ServerVariables["HTTP_CLIENT_IP"]);
                log4net.ThreadContext.Properties["Browser"] = objTrace.Request.Browser.Type;
                log4net.ThreadContext.Properties["Referrer"] = objTrace.Request.UrlReferrer;
                log4net.ThreadContext.Properties["UserAgent"] = objTrace.Request.UserAgent;
                log4net.ThreadContext.Properties["PhysicalPath"] = objTrace.Request.PhysicalPath;
                log4net.ThreadContext.Properties["Host"] = objTrace.Request.Url.Host;
                log4net.ThreadContext.Properties["Url"] = objTrace.Request.Url;
                log4net.ThreadContext.Properties["QueryString"] = Convert.ToString(objTrace.Request.QueryString);
                var Cookies = objTrace.Request.Cookies;
                if (Cookies != null)
                {
                    log4net.ThreadContext.Properties["BWC"] = (Cookies["BWC"] != null ? Cookies["BWC"].Value : "NULL");
                    log4net.ThreadContext.Properties["location"] = (Cookies["location"] != null ? Cookies["location"].Value : "NULL");
                }
            }
        }

    }//class
}   // namespace

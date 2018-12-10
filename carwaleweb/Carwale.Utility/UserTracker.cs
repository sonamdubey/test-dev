using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.Utility
{
    public class UserTracker
    {
        public static string GetUserIp()
        {
            string userIp = string.Empty;

            try
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]))
                {
                    userIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    userIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.UserHostAddress))
                {
                    userIp = HttpContext.Current.Request.UserHostAddress;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userIp;
        }

        public static string GetSessionCookie()
        {
            string cwCookieValue = "";

            if (HttpContext.Current.Request.Cookies["CWC"] != null)
            {
                cwCookieValue = HttpContext.Current.Request.Cookies["CWC"].Value.ToString();
            }
            return cwCookieValue;
        }

        public static string GetAspSessionCookie()
        {
            string aspSessionId = "";

            if (HttpContext.Current.Request.Cookies["_cwv"] != null)
            {
                if (HttpContext.Current.Request.Cookies["_cwv"].Value.ToString().Split('.').Length > 1)
                {
                    aspSessionId = HttpContext.Current.Request.Cookies["_cwv"].Value.ToString().Split('.')[1];
                }
            }
            return aspSessionId;
        }

        public static string GetUserBrowserCapability()
        {
            System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            string browserCapability = "Browser Capabilities\n"
                        + "Type = " + browser.Type + "\n"
                        + "Name = " + browser.Browser + "\n"
                        + "Version = " + browser.Version + "\n";
            return browserCapability;
        }
    }
}

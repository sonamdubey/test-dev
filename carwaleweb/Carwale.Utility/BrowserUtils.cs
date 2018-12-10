using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.Utility
{
    public static class BrowserUtils
    {
        public static bool IsWebView()
        {
            return IsAndroidWebView() || IsIosWebView();
        }

        public static bool IsAndroidWebView()
        {
            string reqWith = HttpContext.Current.Request.ServerVariables["HTTP_X_REQUESTED_WITH"];
            return !string.IsNullOrEmpty(reqWith) && Regex.IsMatch(reqWith, @"^com\.carwale(\..+)?$");
        }

        public static bool IsIosWebView()
        {
            string userAgent = HttpContext.Current.Request.UserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                bool isIphone = Regex.IsMatch(userAgent, @"(?i)iphone|ipod|ipad");
                bool isBrowser = Regex.IsMatch(userAgent, @"(?i)safari");           //'Safari' is passed in user agent from all browsers 
                return isIphone && !isBrowser;
            }
            return false;
        }
    }
}

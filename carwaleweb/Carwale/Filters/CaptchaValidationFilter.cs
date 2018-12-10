using Carwale.Notifications.Logs;
using Carwale.UI.ClientBL;
using Carwale.UI.Common;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WURFL;

namespace Carwale.UI.Filters
{
    public class CaptchaValidationFilterAttribute : ActionFilterAttribute
    {
        private const string _securityUrl = "/m/security/";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var request = filterContext.HttpContext.Request;
                bool showCaptcha = false;
                if (string.IsNullOrWhiteSpace(request.UserAgent))
                {
                    showCaptcha = true;
                }
                //else if (request.UrlReferrer == null  && !CookiesCustomers.IsReCaptchaVerified)
                //{
                //    IDevice device = DeviceDetectionManager.GetDevice();
                    
                //    string browserName = device.GetVirtualCapability("advertised_browser");
                //    string browserVersion = device.GetVirtualCapability("advertised_browser_version");

                //    if (browserName.Contains("Chrome"))
                //    {
                //        showCaptcha = CWConfiguration.ChromeCaptchaEnabledVersions.Contains(browserVersion);
                //        if (showCaptcha) Logger.LogInfo(string.Format("Captcha shown for case : 1 : {0} : {1}",browserName,browserVersion));
                //    }
                //    else if (browserName.Contains("Chromium") || browserName.Contains("Webview"))
                //    {
                //        showCaptcha = CWConfiguration.AndroidWebviewCaptchaEnabledVersions.Contains(browserVersion);
                //        if (showCaptcha) Logger.LogInfo(string.Format("Captcha shown for case : 2 : {0} : {1}", browserName, browserVersion));
                //    }
                //    else if (browserName.Contains("Webkit") || browserName.Contains("Android Browser"))
                //    {
                //        showCaptcha = CWConfiguration.AndroidBrowserCaptchaEnabledVersions.Contains(browserVersion);
                //        if (showCaptcha) Logger.LogInfo(string.Format("Captcha shown for case : 3 : {0} : {1}", browserName, browserVersion));
                //    }
                    
                //}
                if (showCaptcha)
                {
                    Logger.LogInfo(string.Format("Captcha shown "));
                    filterContext.HttpContext.Response.Redirect(_securityUrl);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CaptchaValidationFilterAttribute");
            }
        }
    }
}
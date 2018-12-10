using Carwale.Notifications;
using System;
using System.Web;
using WURFL;

namespace Carwale.UI.ClientBL
{
    public static class DeviceDetectionManager
    {
        private const string wurflManagerCacheKey = "__WurflManager";
        private const string c_strDesktopDetected = "DesktopDetected";
        private const string c_strDesktop = "desktop";
        private const string c_strOne = "1";

        public static bool PerformDetection(HttpContextBase httpContextBase)
        {
            bool _redirectToMobile = false;

            try
            {
                IWURFLManager wurflManager = HttpContext.Current.Cache.Get(wurflManagerCacheKey) as IWURFLManager;
                if (wurflManager == null)
                {
                    return true;
                }

                if (string.IsNullOrWhiteSpace(httpContextBase.Request.UserAgent ?? string.Empty))
                {
                    return false;
                }

                IDevice device = wurflManager.GetDeviceForRequest(httpContextBase.Request.UserAgent);	//gets a device for that user agent

                //gets the capability of device        
                bool boolIsWirelessDevice = device.GetCapability("is_wireless_device").Equals("true");
                bool boolIsAjaxSupportJavascript = device.GetCapability("ajax_support_javascript").Trim().Equals("true");
                bool boolIsTablet = device.GetCapability("is_tablet").Trim().Equals("true");

                if (boolIsWirelessDevice && boolIsAjaxSupportJavascript && !boolIsTablet)
                {
                    _redirectToMobile = true; //Redirect to mobile website
                }
                else if ((!boolIsWirelessDevice && boolIsAjaxSupportJavascript) || (boolIsWirelessDevice && boolIsAjaxSupportJavascript && boolIsTablet))
                {
                    StayOnDesktopSite(httpContextBase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DeviceDetectionFilterAttribute");
                objErr.SendMail();
            }

            return _redirectToMobile;
        }

        public static void StayOnDesktopSite(HttpContextBase httpContextBase)
        {
            httpContextBase.Response.Cookies["DesktopDetected"].Value = c_strOne;
            httpContextBase.Response.Cookies["DesktopDetected"].Expires = DateTime.Now.AddDays(1);
        }

        public static bool IsMobileSiteDetected(HttpContextBase httpContextBase)
        {
            string queryStringsite = httpContextBase.Request.QueryString["site"];
            bool desktopNotDetected = (httpContextBase.Request.Cookies[c_strDesktopDetected] == null);

            return (desktopNotDetected && (string.IsNullOrEmpty(queryStringsite) || !queryStringsite.Equals(c_strDesktop)));
        }

        public static bool IsMobile(HttpContextBase httpContextBase)
        {
            if (string.IsNullOrWhiteSpace(httpContextBase.Request.UserAgent)) return false;
            IWURFLManager wurflManager = HttpContext.Current.Cache.Get(wurflManagerCacheKey) as IWURFLManager;
            if (wurflManager == null)
            {
                return true;
            }
            IDevice device = wurflManager.GetDeviceForRequest(httpContextBase.Request.UserAgent);	//gets a device for that user agent

            bool boolIsWirelessDevice = device.GetCapability("is_wireless_device").Equals("true");
            bool boolIsTablet = device.GetCapability("is_tablet").Trim().Equals("true");

            return boolIsWirelessDevice && !boolIsTablet;
        }

        public static bool UserWantsToViewDesktopSite(HttpContextBase httpContextBase)
        {
            string queryStringsite = httpContextBase.Request.QueryString["site"];
            bool desktopNotDetected = (httpContextBase.Request.Cookies[c_strDesktopDetected] == null);

            return (desktopNotDetected && !string.IsNullOrEmpty(queryStringsite) && queryStringsite.Equals(c_strDesktop));
        }

        public static bool Is(string property, bool isVirtualProperty)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.UserAgent)) return false;
            IDevice device = GetDevice();
            return isVirtualProperty ? device.GetVirtualCapability("is_" + property).Equals("true") : device.GetCapability("is_" + property).Equals("true");
        }

        public static string GetProperty(string property, bool isVirtualProperty)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.UserAgent)) return string.Empty;
            IDevice device = GetDevice();
            return isVirtualProperty ? device.GetVirtualCapability(property) : device.GetCapability(property);
        }

        public static IDevice GetDevice()
        {
            IWURFLManager wurflManager = HttpContext.Current.Cache.Get(wurflManagerCacheKey) as IWURFLManager;
            IDevice device = wurflManager.GetDeviceForRequest(HttpContext.Current.Request.UserAgent);	//gets a device for that user agent
            return device;
        }
    }
}
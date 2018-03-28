using System;
using System.Web;
using WURFL;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 5 jan 2017
    /// Summary : Class have functions related to the device detection
    /// </summary>
    public static class DeviceDetectionManager
    {
        private const string wurflManagerCacheKey = "__WurflManager";
        private const string c_strDesktopDetected = "DesktopDetected";
        private const string c_strDesktop = "desktop";
        private const string c_strOne = "1";

        /// <summary>
        /// Function to perform the device detection. (mobile or desktop)
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns></returns>
        public static bool PerformDetection(HttpContextBase httpContextBase)
        {
            bool _redirectToMobile = false;

            try
            {
                IWURFLManager wurflManager = HttpContext.Current.Cache[wurflManagerCacheKey] as IWURFLManager;

                if (string.IsNullOrWhiteSpace(httpContextBase.Request.UserAgent ?? string.Empty)) return false;

                IDevice device = wurflManager.GetDeviceForRequest(httpContextBase.Request.UserAgent);	//gets a device for that user agent

                //gets the capability of device        
                bool boolIsWirelessDevice = device.GetCapability("is_wireless_device").Equals("true");
                bool boolIsUXFullDesktop = device.GetCapability("ux_full_desktop").Trim().Equals("false");
                bool boolIsTablet = device.GetCapability("is_tablet").Trim().Equals("true");

                if (boolIsWirelessDevice && boolIsUXFullDesktop && !boolIsTablet)
                {
                    _redirectToMobile = true; //Redirect to mobile website
                }
                else if ((!boolIsWirelessDevice && boolIsUXFullDesktop) || (boolIsWirelessDevice && boolIsUXFullDesktop && boolIsTablet))
                {
                    StayOnDesktopSite(httpContextBase);
                }
            }
            catch (Exception)
            {
                StayOnDesktopSite(httpContextBase);
            }

            return _redirectToMobile;
        }

        /// <summary>
        /// Function to add the cookie which will allow user to stay on the desktop website
        /// </summary>
        /// <param name="httpContextBase"></param>
        public static void StayOnDesktopSite(HttpContextBase httpContextBase)
        {
            httpContextBase.Response.Cookies["DesktopDetected"].Value = c_strOne;
            httpContextBase.Response.Cookies["DesktopDetected"].Expires = DateTime.Now.AddDays(1);
        }

        /// <summary>
        /// Function returns whether user is browsing through mobile
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns></returns>
        public static bool IsMobileSiteDetected(HttpContextBase httpContextBase)
        {
            string queryStringsite = httpContextBase.Request.QueryString["site"];
            bool desktopNotDetected = (httpContextBase.Request.Cookies[c_strDesktopDetected] == null);

            return (desktopNotDetected && (string.IsNullOrEmpty(queryStringsite) || !queryStringsite.Equals(c_strDesktop)));
        }

        /// <summary>
        /// Function to actually detect whether user is browsing through mobile device
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns></returns>
        public static bool IsMobile(HttpContextBase httpContextBase)
        {
            if (string.IsNullOrWhiteSpace(httpContextBase.Request.UserAgent)) return false;
            IWURFLManager wurflManager = HttpContext.Current.Cache[wurflManagerCacheKey] as IWURFLManager;
            IDevice device = wurflManager.GetDeviceForRequest(httpContextBase.Request.UserAgent);	//gets a device for that user agent

            bool boolIsWirelessDevice = device.GetCapability("is_wireless_device").Equals("true");
            bool boolIsTablet = device.GetCapability("is_tablet").Trim().Equals("true");

            return boolIsWirelessDevice && !boolIsTablet;
        }

        /// <summary>
        /// Function to check whether user wants to browse website in the desktop mode
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns></returns>
        public static bool UserWantsToViewDesktopSite(HttpContextBase httpContextBase)
        {
            string queryStringsite = httpContextBase.Request.QueryString["site"];
            bool desktopNotDetected = (httpContextBase.Request.Cookies[c_strDesktopDetected] == null);

            return (desktopNotDetected && !string.IsNullOrEmpty(queryStringsite) && queryStringsite.Equals(c_strDesktop));
        }
    }
}

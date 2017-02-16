using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Web.Mvc;

namespace Bikewale.Filters
{
    /// <summary>
    /// This is filter Class for performing the device detection. 
    /// Add this attribute to the desktop website controller.
    /// User will be redirected to the mobile website if browsing through mobile device.
    /// </summary>
    public class DeviceDetection : ActionFilterAttribute
    {
        private static readonly string _hostUrl = "/m";
        private string _mobilePageUrl;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DeviceDetection() { }

        // When redirect url decided externally then user will be redited to the given url
        public DeviceDetection(string mobileUrl)
        {
            _mobilePageUrl = mobileUrl;
        }

        /// <summary>
        /// Function to redirect user to the mobile website
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var httpContextBase = filterContext.HttpContext;

                if (DeviceDetectionManager.IsMobileSiteDetected(httpContextBase))
                {
                    if (DeviceDetectionManager.PerformDetection(filterContext.HttpContext))
                    {
                        if (String.IsNullOrEmpty(_mobilePageUrl))
                        {
                            // This variable is not getting retrived need to check this value. Otherwise deveice detection won't work
                            _mobilePageUrl = filterContext.HttpContext.Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                        }

                        filterContext.Result = new RedirectResult(_hostUrl + _mobilePageUrl);
                    }
                }
                else if (DeviceDetectionManager.UserWantsToViewDesktopSite(filterContext.HttpContext))
                {
                    DeviceDetectionManager.StayOnDesktopSite(filterContext.HttpContext);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DeviceDetectionFilterAttribute.OnActionExecuting");
            }
        }
    }
}
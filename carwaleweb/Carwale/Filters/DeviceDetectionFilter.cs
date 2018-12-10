using Carwale.Notifications;
using Carwale.UI.ClientBL;
using System;
using System.Web.Mvc;

namespace Carwale.UI.Filters
{
    public class DeviceDetectionFilter : ActionFilterAttribute
    {        
        private static readonly string _hostUrl = "/m";
        private string _mobilePageUrl;        

        // When redirect url decided externally
        public DeviceDetectionFilter(string mobileUrl)
        {            
           _mobilePageUrl = mobileUrl;
        }

        // When redirect url decided internally using server variable HTTP_X_REWRITE_URL
        public DeviceDetectionFilter()
        {
           
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var httpContextBase = filterContext.HttpContext;

                if (DeviceDetectionManager.IsMobileSiteDetected(httpContextBase))
                {                    
                    if (DeviceDetectionManager.PerformDetection(filterContext.HttpContext))
                    {                        
                        _mobilePageUrl = filterContext.HttpContext.Request.ServerVariables["HTTP_X_REWRITE_URL"];                       
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
                objErr.SendMail();
            }
        }
    }
}
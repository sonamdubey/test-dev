using System;
using System.Web;
using System.Configuration;
using System.Web.Mail;
using System.Text;
using System.Data;
using WURFL;
using WURFL.Config;
using System.Web.Caching;
using Carwale.Notifications;
using Carwale.UI.ClientBL;

namespace Carwale.UI.Common
{
    public class DeviceDetection
    {        
        private static readonly string _hostUrl;     
        private string _mobilePageUrl = string.Empty;        

        static DeviceDetection()
        {
          _hostUrl = ConfigurationManager.AppSettings["mobileSiteURL"];                 
        }

        public DeviceDetection()
        {
            
        }

        public DeviceDetection(string _mobPageUrl)
        {
            _mobilePageUrl = _mobPageUrl;
        }

        /*
          Procedure : DetectDevice
          Created By : Ashish Ambokar
          Created Date : 25/6/2012
          Note : This function basically decides whether to perform device detection or not. If yes then it performs device detection.
        */
        public void DetectDevice()
        {
            try
            {
                var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);

                if (DeviceDetectionManager.IsMobileSiteDetected(httpContextWrapper))
                {
                    //currentContext.Response.Write("No Desktop Detected.....Perform Device Detection <br/>");
                    //if DesktopDetected cookie does not exist and QueryString parameter site = desktop does not exist 
                    //then  we have to detect device 
                    //to redirect to mobile website or stay with desktop website or show no compatible website page
                    if (DeviceDetectionManager.PerformDetection(httpContextWrapper))
                    {                        
                        if (!string.IsNullOrEmpty(_mobilePageUrl))
                        {
                            HttpContext.Current.Response.Redirect(_hostUrl + _mobilePageUrl, false);
                        }
                    }
                }
                else if (DeviceDetectionManager.UserWantsToViewDesktopSite(httpContextWrapper))
                {
                    //currentContext.Response.Write("Smart phone user wants to see desktop website..hence cookie created and no device detection performed..user can see dektop website only <br/>");
                    //when user clicks "View Desktop Version" link from mobile website in footer
                    //He will be redirected home page with query string parameter site=desktop. This tells user wants to see desktop website
                    //so it creates a cookie and from next time for next 24 hours user visits any page where device detection is done
                    //device detection will not happen and he will get to see desktop version only
                    DeviceDetectionManager.StayOnDesktopSite(httpContextWrapper);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Utility;
using System.Web.Mvc;
using Bikewale.Notifications;

namespace Bikewale.Filters
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 17 Mar 2017
    /// Summary : Class have method to detect the mobile device
    /// </summary>
    public static class DetectDevice
    {
        /// <summary>
        /// Function to detect the mobile device
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns whether mobile device or not accessing the page</returns>
        public static bool IsMobileDevice(this HttpContextBase context)
        {
            bool isMobileSiteDetected = false;

            try
            {
                if (DeviceDetectionManager.IsMobileSiteDetected(context))
                {
                    if (DeviceDetectionManager.PerformDetection(context))
                    {
                        isMobileSiteDetected = true;
                    }
                }
                else if (DeviceDetectionManager.UserWantsToViewDesktopSite(context))
                {
                    DeviceDetectionManager.StayOnDesktopSite(context);                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "IsMobileDevice");
            }

            return isMobileSiteDetected;
        }

    }
}
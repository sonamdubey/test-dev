using log4net;
using System;
using System.Threading;
using System.Web;
using System.Web.Caching;
using WURFL;
using WURFL.Config;


namespace Bikewale.Common
{
    public class DeviceDetection
    {
        private string _hostUrl = "~/m";

        private string _mobilePageUrl = "";
        static readonly ILog _log = LogManager.GetLogger(typeof(DeviceDetection));

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
            if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] == null || HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString().Trim() == "")
            {
                //HttpContext.Current.Response.Redirect("~/nosite.html");
            }
            else if (HttpContext.Current.Request.Cookies["DesktopDetected"] == null && (HttpContext.Current.Request.QueryString["site"] == null || (HttpContext.Current.Request.QueryString["site"] != null && HttpContext.Current.Request.QueryString["site"] != "desktop")))
            {
                //HttpContext.Current.Response.Write("No Desktop Detected.....Perform Device Detection <br/>");
                //if DesktopDetected cookie does not exist and QueryString parameter site = desktop does not exist 
                //then  we have to detect device 
                //to redirect to mobile website or stay with desktop website or show no compatible website page
                try
                {
                    PerformDetection();
                }
                catch (ThreadAbortException)
                {
                    //  no need to log thread abort exception.
                }
                catch (Exception ex)
                {
                    ThreadContext.Properties["UserAgent"] = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
                    _log.Error(ex);
                    ThreadContext.Properties.Remove("UserAgent");
                }
            }
            else if (HttpContext.Current.Request.Cookies["DesktopDetected"] == null && HttpContext.Current.Request.QueryString["site"] != null && HttpContext.Current.Request.QueryString["site"] == "desktop")
            {
                //HttpContext.Current.Response.Write("Smart phone user wants to see desktop website..hence cookie created and no device detection performed..user can see dektop website only <br/>");
                //when user clicks "View Desktop Version" link from mobile website in footer
                //He will be redirected home page with query string parameter site=desktop. This tells user wants to see desktop website
                //so it creates a cookie and from next time for next 24 hours user visits any page where device detection is done
                //device detection will not happen and he will get to see desktop version only
                HttpContext.Current.Response.Cookies["DesktopDetected"].Value = "1";
                HttpContext.Current.Response.Cookies["DesktopDetected"].Expires = DateTime.Now.AddDays(1);
            }
        }

        /*
            Procedure : DetectDevice()
            Code Added By : Ashish Ambokar
            Added Date : 7/2/2012
            Note : The below function checkes whether a request if from mobile device/browser or desktop device/browser or non-compatible device/browser
        */
        private void PerformDetection()
        {
            string WurflManagerCacheKey = "__WurflManager";																				//name of cache

            IWURFLManager wurflManager;
            if (HttpContext.Current.Cache[WurflManagerCacheKey] == null)																//checked whether cahce already exists
            {
                string WurflDataFilePath = HttpContext.Current.Server.MapPath("~/App_Data/wurfl-latest.zip");
                string WurflPatchFilePath = HttpContext.Current.Server.MapPath("~/wurfl/web_browsers_patch.xml");

                CacheDependency dependency = new CacheDependency(WurflDataFilePath);													//dependacy of the cache is on wurfl.xml
                var configurer = new InMemoryConfigurer().MainFile(WurflDataFilePath).PatchFile(WurflPatchFilePath); 					//creates a configurer
                var manager = WURFLManagerBuilder.Build(configurer);																	//creates a manager	
                HttpContext.Current.Cache.Insert(WurflManagerCacheKey, manager, dependency);											//the manager is added to cache for further use
                wurflManager = manager as IWURFLManager;																				//we get manager by newly creating it	
                //HttpContext.Current.Response.Write("new cache created<br/>");
            }
            else
            {
                wurflManager = HttpContext.Current.Cache[WurflManagerCacheKey] as IWURFLManager;										//retrieves the manager from cache
                //HttpContext.Current.Response.Write("cache found<br/>");
            }

            string userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];											//gets the user agent
            //HttpContext.Current.Response.Write("<br/><b>User Agent : </b><br/> " + userAgent);
            IDevice device = wurflManager.GetDeviceForRequest(userAgent);																//gets a device for that user agent

            string is_wireless_device = device.GetCapability("is_wireless_device");														//gets the capability of device
            //HttpContext.Current.Response.Write("<br/>is_wireless_device : " + is_wireless_device);
            string uxFullDesktop = device.GetCapability("ux_full_desktop").ToString().Trim();
            //HttpContext.Current.Response.Write("<br/>ajax_support_javascript : " + ajax_support_javascript);
            string is_tablet = device.GetCapability("is_tablet").ToString().Trim();
            //HttpContext.Current.Response.Write("<br/>is_tablet : " + is_tablet);

            if (is_wireless_device == "true" && uxFullDesktop == "false" && is_tablet == "false")
            {
                //Redirect to mobile website
                //Response.Write("<br/>Redirect to mobile website");
                HttpContext.Current.Response.Redirect(_hostUrl + _mobilePageUrl);
            }
            else if ((is_wireless_device == "false" && uxFullDesktop == "true") || (is_wireless_device == "true" && uxFullDesktop == "true" && is_tablet == "true"))
            {
                //Stay on the desktop website
                //HttpContext.Current.Response.Write("<br/>Stay on the desktop website as the device detected is desktop");
                HttpContext.Current.Response.Cookies["DesktopDetected"].Value = "1";
                HttpContext.Current.Response.Cookies["DesktopDetected"].Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                //Show no compatible message
                //HttpContext.Current.Response.Redirect("~/NoSite.html");
                //HttpContext.Current.Response.Redirect(_hostUrl + _mobilePageUrl);
            }
            //Response.End();
        }
    }
}
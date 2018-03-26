<%@ Page Language="C#" %>

<%@ Import Namespace="WURFL" %>
<%@ Import Namespace="WURFL.Config" %>
<%@ Import Namespace="System.Web.Caching" %>
<%
    Response.StatusCode = 404;
%>
<script runat="server">
    /// <summary>
    /// Modified By :   Sumit Kate on 05 Jan 2016
    /// Description :   Replaced Server.Transfer with Server.TransferRequest
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object Sender, EventArgs e)
    {
        //Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(Request.ServerVariables["HTTP_X_ORIGINAL_URL"].ToString());
        //dd.DetectDevice();
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
        if (!String.IsNullOrEmpty(userAgent))
        {
            IDevice device = wurflManager.GetDeviceForRequest(userAgent);																//gets a device for that user agent

            string is_wireless_device = device.GetCapability("is_wireless_device");														//gets the capability of device
            //HttpContext.Current.Response.Write("<br/>is_wireless_device : " + is_wireless_device);
            string ajax_support_javascript = device.GetCapability("ajax_support_javascript").ToString().Trim();
            //HttpContext.Current.Response.Write("<br/>ajax_support_javascript : " + ajax_support_javascript);
            string is_tablet = device.GetCapability("is_tablet").ToString().Trim();
            //HttpContext.Current.Response.Write("<br/>is_tablet : " + is_tablet);

            if (is_wireless_device == "true" && ajax_support_javascript == "true" && is_tablet == "false")
            {
                //Redirect to mobile website
                //Response.Write("<br/>Redirect to mobile website");
                //HttpContext.Current.Response.Redirect("/m/pagenotfound.aspx",false);            
                Server.TransferRequest("/m/pagenotfound.aspx");
            }
        }

    }
</script>
<!-- #include file="/includes/headhomenoad.aspx" -->
<style>
    h1 {
        color: #003366;
        font-size: 28px;
        font-weight: bold;
        font-family: Verdana, Arial, Helvetica, sans-serif;
        border-bottom: 2px solid orange;
        margin: 10px 10px 10px 0;
    }

    #main {
        padding: 20px;
    }

    p, li {
        font-weight: bold;
        font-size: 12px;
        color: #666666;
    }

    .container-min-height {
        min-height: 500px;
    }
</style>
<div class="container_12 container-min-height">
    <div class="content-block">
        <h1>Page Not Found</h1>
        <h3>Requested page couldn't be found on <a href="/" title="Visit BikeWale home page">BikeWale</a></h3>
        <div class="text-highlight padding-top10">
            Possible causes for this inconvenience are:
			<ul class="std-ul-list content-block">
                <li>The requested page might have been removed from the server.</li>
                <li>The URL might be mis-typed by you.</li>
                <li>Some maintenance process is going on the server.</li>
            </ul>
            Please try visiting the page again within few minutes. 
        </div>
    </div>
    <div class="clear"></div>
</div>
<!-- #include file="/includes/footerinner.aspx" -->

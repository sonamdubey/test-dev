<%@ Import Namespace="Carwale.Utility" %>
<%@ Import Namespace="Carwale.UI.PresentationLogic" %>
<%@ Import Namespace="Carwale.UI.Common"  %>
<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>

<%@ Register TagPrefix="uc" TagName="BlackBerryDownload" Src="/m/Controls/BlackBerryDownload.ascx" %>

<div id="globalPopupBlackOut" class="blackOut-window"></div>
<% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial("~/Views/Shared/m/_NavigationMenu.cshtml", null); %>
<div id="global-search-popup-cars" class="global-search-popup hide">
    <!-- global-search-popup code starts here -->
    <div class="form-control-box">
        <span class="back-arrow-box" id="gs-close">
            <span class="cwmsprite back-long-arrow-left"></span>
        </span>
        <span class="cross-box hide" id="gs-text-clear">
            <span class="cwmsprite cross-md-dark-grey"></span>
        </span>
        <input type="text" id="globalSearchPopup" class="form-control" placeholder="Search">
        <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none; right: 40px;"></span>
    </div>
    <div class="js-voice-search bg-color-grey font14">Would you like to use voice search? <span id="yes-link" class="action-btn text-link margin-left15">Yes</span><span id="no-link" class="action-btn text-link"> No</span></div>
    <!-- #include file="/Views/Shared/m/_GlobalSearch.cshtml" -->
</div>
<!-- global-search-popup code ends here -->
<div id="global-search-popup-pq" class="global-search-popup hide">
    <!-- global-search-popup pq code starts here -->
    <div class="form-control-box">
        <span class="back-arrow-box" id="gs-close-pq">
            <span class="cwmsprite back-long-arrow-left"></span>
        </span>
        <span class="cross-box hide" id="gs-text-clear-pq">
            <span class="cwmsprite cross-md-dark-grey"></span>
        </span>
        <input type="text" id="globalSearchPQ" class="form-control" placeholder="Search">
        <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none; right: 40px;"></span>
    </div>
</div>
<!-- global-search-popup pq code ends here -->
<!-- global-location-popup code starts here -->
  
   <noscript id="location_popup">
		<script type="text/html" src="/static/m/ui/location.html">
		</script>
	</noscript>
    
<!-- global-location-popup code ends here -->
<% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial( "~/views/shared/m/_AndroidAppDownload.cshtml" ); %>

<link rel="stylesheet" href="/static/m/css/white-header-style.css" type="text/css">
<% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial("~/views/shared/m/_WhiteHeader.cshtml", null); %>

<uc:BlackBerryDownload ID="ucBlackBerryDownload" runat="server" />
<div id="divOverlay" style="display: none;"></div>
<!--Add banner starts here-->
<script language="c#" runat="server">
    public bool IsShowAd = true; public bool IsNotHomePage = true;
    bool isMobile = Carwale.UI.ClientBL.DeviceDetectionManager.IsMobile(new HttpContextWrapper(HttpContext.Current));    
</script>
<%if (IsShowAd)
  { %>
<div id="divAdBar">
    <!-- Mobile_CarWale/Mobile_ROS_320x50 -->
    <%if (IsNotHomePage)
      {%>
    <!-- CarWale_Mobile_ROS_320x50 -->
    <div id='div-gpt-ad-1419227721763-0' style='width: 320px; margin: 10px auto 10px auto;'>
        <script type='text/javascript'>
            googletag.cmd.push(function () { googletag.display('div-gpt-ad-1419227721763-0'); });
        </script>
    </div>
    <%} %>
</div>
<%} %>

<% if (!(Request.RawUrl.StartsWith("/m/used") || Request.RawUrl.StartsWith("/used") || Request.RawUrl.Contains("/advantage")))
    { %>
<% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial(ProductExperiments.showCustomNotificationPopup(isMobile) ? "~/Views/Shared/Notification/_CustomNotificationPopup.cshtml" : "~/Views/Shared/Notification/_NotificationPopupScript.cshtml"); %>
<%} %>

<!--Add banner ends here-->
<script type="text/javascript">
    var logoUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["BridgestoneUrl"]%>';
    $('#bridgestoneurl').attr('href', logoUrl);
</script>
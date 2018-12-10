<%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk&sensor=true"></script>--%>
<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<script language="c#" runat="server">
	    private string Title = "CarWale";
	    private string Keywords = "";
	    private string Description = "";
	    private string Canonical = "";
        private string MenuIndex = "";
        private string ModelsTarget="";
        private string IsOldJquery="true";
        public bool ShowBottomAd = true;
        private string PrevPageUrl = "";
        private string NextPageUrl = "";
        private string ModelForDFPAd = "";
        private string SectionName = "";
        private string SectionNameKey = "Section";
        private string MakeTarget = "";
        private string ModelPageTitle = "";
        private bool IsCompareCar = false;
        private string CompareTar = "";
        private string adPath = System.Configuration.ConfigurationManager.AppSettings["adPathMobile"];
        private string ModelName1 = "";
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        private string stagingPath = System.Configuration.ConfigurationManager.AppSettings["stagingPath"];
    </script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
    <meta name="HandheldFriendly" content="true" />
    <%if (Canonical!=""){%><link rel="canonical" href="<%=Canonical%>" /><%}%>
    <%--Link for Favicon Icon of CarWale --%>
    <link href="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/design15/carwale.png?v=1.1" rel="SHORTCUT ICON" />
    <%--Link to add CW touch icon --%>
    <title itemprop="name"><%=Title%><%=ModelPageTitle%></title>
    <meta itemprop="description" name="description" content="<%=Description%>" />
    <link rel="stylesheet" href="/static/m/css/jquery.mobile-1.4.2.min.css" type="text/css" >
    <link rel="stylesheet" href="/static/m/css/style.css" type="text/css" >
    <script  type="text/javascript"  src="/static/m/js/main-js-min.js" ></script>
<link rel="stylesheet" href="/static/m/css/select-city.css" type="text/css" >
<script  type="text/javascript"  src="/static/m/js/citypopup.js" ></script>
    <script type='text/javascript'>
        var googletag = googletag || {};
        googletag.cmd = googletag.cmd || [];
        (function () {
            var gads = document.createElement('script');
            gads.async = true;
            gads.type = 'text/javascript';
            var useSSL = 'https:' == document.location.protocol;
            gads.src = (useSSL ? 'https:' : 'http:') +
            '//www.googletagservices.com/tag/js/gpt.js';
            var node = document.getElementsByTagName('script')[0];
            node.parentNode.insertBefore(gads, node);
        })();
    </script>

   <script language="c#" runat="server">public bool IsNotHomePage = true;</script>
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            <% if(IsNotHomePage == true){%>
            googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_ROS_320x50', [320, 50], 'div-gpt-ad-1419227721763-0').addService(googletag.pubads());
            <%}%>
            <%else {%>
            googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_FeaturedCar_320x130', [320, 130], 'div-gpt-ad-1400919004715-1').addService(googletag.pubads());
            googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_HP_HP_320x50', [320, 50], 'div-gpt-ad-1400919004715-0').addService(googletag.pubads());
            <%}%>
            <% if(IsCompareCar == true){%>

            googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_Comparecar_330x130', [[330, 130], [320, 150]], 'div-gpt-ad-1422362815725-1').addService(googletag.pubads());
            <%}%>
<% if(CompareTar!=""){%>

            googletag.defineSlot('/1017752/CarWale_Mobile_Comparecar_320x70', [320, 70], 'div-gpt-ad-1422613936644-0').addService(googletag.pubads());
 <%}%>


            <%if (ModelsTarget!=""){%>googletag.pubads().setTargeting("Models", "<%=ModelsTarget%>");<%}%>

            <%if (MakeTarget!=""){%>googletag.pubads().setTargeting("Make", "<%=MakeTarget%>");<%}%>
   
         <%if (CompareTar!=""){%>googletag.pubads().setTargeting("CompareTar", "<%=CompareTar%>");<%}%>

            //Ad for quatation page
            //Bottom Ad
            googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_ROS_300x250', [300, 250], 'div-gpt-ad-1435297201507-0').addService(googletag.pubads());

            googletag.pubads().setTargeting("<%=SectionNameKey%>", "<%=SectionName%>");
            googletag.pubads().setTargeting("City", "<%= MobileWeb.Common.CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting("Section", "<%=SectionName%>");
        });
    </script> 
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>

 <!-- Global Site Tag (gtag.js) - Google AdWords: 999894061 -->
    <% RazorPartialBridge.RenderPartial("~/Views/Shared/_GlobalSiteTagScript.cshtml"); %>

    <% RazorPartialBridge.RenderAction("GetAdTagetingData", "UserProfile", new Dictionary<string,object>{ {"cwcCookieId" , Carwale.UI.Common.CurrentUser.CWC}, {"platform" , Carwale.Entity.Enum.Platform.CarwaleMobile }}); %>  

<%-- BlackBerry App Download Ticker --%>
<%--<%@ Register TagPrefix="uc" TagName="mobilepopup" src="/m/Controls/AndroidAppDownload.ascx" %>--%>

<%--<uc:mobilepopup  runat="server" /> --%>
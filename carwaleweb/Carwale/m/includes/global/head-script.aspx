<%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk&sensor=true"></script>--%>
<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<script language="c#" runat="server">
    private string Title = "CarWale";
    private string Keywords = "";
    private string Description = "";
    private string Canonical = "";
    private string MenuIndex = "";
    private string ModelsTarget = "";
    private string IsOldJquery = "true";
    private string PrevPageUrl = "";
    private string NextPageUrl = "";
    private string ModelForDFPAd = "";
    private string SectionName = "UsedCarValuation";
    private string SectionNameKey = "Section";
    private string MakeTarget = "";
    private string ModelPageTitle = "";
    private bool IsCompareCar = false;
    private string CompareTar = "";
    private string adPath = System.Configuration.ConfigurationManager.AppSettings["adPathMobile"];
    private string ModelName1 = "";
    private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string stagingPath = System.Configuration.ConfigurationManager.AppSettings["stagingPath"];
    private string DeeplinkAlternatives = string.Empty, DeeplinkAlternativesAndroid = string.Empty;
    private bool showAdTargeting = false;
</script>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<meta name="keywords" content="" />
<meta name="google-site-verification" content="y33JBqCALrAtfZBeI-VVHz7jyLJrZqpOsggckAZlBj8" />
<%if (Canonical != "")
  {%><link rel="canonical" href="<%=Canonical%>" /><%}%>
<title itemprop="name"><%=Title%><%=ModelPageTitle%></title>
<meta itemprop="description" name="description" content="<%=Description%>" />
<link rel="SHORTCUT ICON" href="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/design15/carwale.png?v=1.1" />
<%if (!string.IsNullOrWhiteSpace(DeeplinkAlternativesAndroid)) { %>
<link rel="alternate" href="android-app://com.carwale/carwaleandroid1/www.carwale.com<%=DeeplinkAlternativesAndroid %>" />
<%}
if(!string.IsNullOrWhiteSpace(DeeplinkAlternatives)) {%>
<%--<link rel="alternate" href="ios-app://910137745/ioscarwale/www.carwale.com<%=DeeplinkAlternatives %>" />--%>
<%}%>
<% if( PrevPageUrl != "" ) { %><link rel="prev" href="<%= "" + PrevPageUrl %>" /><% } %>
<% if( NextPageUrl != "" ) { %><link rel="next" href="<%= "" + NextPageUrl %>" /><% } %>
<link rel="stylesheet" href="/static/m/css/cwm-common-style.css" type="text/css">
<link rel="stylesheet" href="/static/cwfontawesome/cw-font-awesome.css" type="text/css">
<link rel="stylesheet" href="/static/m/css/easy-autocomplete.css" type="text/css">
<link rel="stylesheet" href="/static/sass/partials/android-app-download.css" type="text/css">
<script type="text/javascript" src="/static/m/js/frameworks.js"></script>
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
<!--CrazyEgg heatmaps  -->
<script type="text/javascript">
    setTimeout(function () {
        var a = document.createElement("script");
        var b = document.getElementsByTagName("script")[0];
        a.src = document.location.protocol + "//script.crazyegg.com/pages/scripts/0012/9477.js?" + Math.floor(new Date().getTime() / 3600000);
        a.async = true; a.type = "text/javascript"; b.parentNode.insertBefore(a, b)
    }, 1);
</script>
<script type='text/javascript'>
    googletag.cmd.push(function () {
            <% if ((HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"]??"") != "/m/")
               {%>
        googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_ROS_320x50', [[320, 50], [320, 100], [320, 150]], 'div-gpt-ad-1419227721763-0').addService(googletag.pubads());
        <%}else{%>
        googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_FeaturedCar_320x130', [320, 130], 'div-gpt-ad-1400919004715-1').addService(googletag.pubads());
        googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_HP_HP_320x50', [320, 50], 'div-gpt-ad-1400919004715-0').addService(googletag.pubads());
        <%}%>
            <% if (IsCompareCar == true)
               {%>

        googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_Comparecar_330x130', [[330, 130], [320, 150]], 'div-gpt-ad-1422362815725-1').addService(googletag.pubads());
        <%}%>
<% if (CompareTar != "")
   {%>

        googletag.defineSlot('/1017752/CarWale_Mobile_Comparecar_320x70', [320, 70], 'div-gpt-ad-1422613936644-0').addService(googletag.pubads());
        <%}%>


            <%if (ModelsTarget != "")
              {%>googletag.pubads().setTargeting("Models", "<%=ModelsTarget%>");<%}%>

            <%if (MakeTarget != "")
              {%>googletag.pubads().setTargeting("Make", "<%=MakeTarget%>");<%}%>

            <%if (CompareTar != "")
              {%>googletag.pubads().setTargeting("CompareTar", "<%=CompareTar%>");<%}%>


        //Ad for quatation page
        //Bottom Ad
        googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_ROS_300x250', [300, 250], 'div-gpt-ad-1435297201507-0').addService(googletag.pubads());
        <%if (SectionName == "Insurance") {%>
        // Insurance cholaMS custom tags
        googletag.defineSlot('/1017752/<%=adPath%>CarWale_Mobile_Insurance_298x70', [[298, 70], [298, 150], [298, 100]], 'div-gpt-ad-1444731593680-0').addService(googletag.pubads());
        <%}%>

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


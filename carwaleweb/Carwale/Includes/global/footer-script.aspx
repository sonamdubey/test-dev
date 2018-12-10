<%@ Import Namespace="Carwale.Cache.ChatManagement" %>
<%@ Import Namespace="Carwale.Entity.ChatManagement" %>
<script language="c#" runat="server">
    public string google_dynx_itemid = "", google_dynx_itemid2 = "", google_dynx_pagetype = "", google_dynx_totalvalue = "";
    bool IsLessThanIe9 = (HttpContext.Current.Request.Browser.Type == "IE8" || HttpContext.Current.Request.Browser.Type == "IE7") ? true : false;
</script>
<% if (IsLessThanIe9) { %>
<!-- #include file="/Views/Shared/_browserDownload.cshtml" -->
<% } %>
<script type="text/javascript">
    var cwTrackingPath = '<%= ConfigurationManager.AppSettings["HostUrl"] %>';
    var insuranceKeys = {
        cholaStates: [<%=System.Configuration.ConfigurationManager.AppSettings["InsuranceStates"]??""%>]
    }
    var defaultCookieDomain = '<%= Carwale.UI.Common.CookiesCustomers.CookieDomain%>';
    var isEligibleForORP = '<%=Carwale.UI.Common.CookiesCustomers.IsEligibleForORP%>' === 'True';
    var abTestKey = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestVersion"] %>";
    var abTestKeyMaxValue = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMaxValue"] %>";
    var abTestKeyMinValue = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMinValue"] %>";
    var askingAreaCityId = [<%=System.Configuration.ConfigurationManager.AppSettings["AskingAreaCityIds"]%>];
</script>
<script type="text/javascript" src="/static/js/plugins.js"></script>
<script  type="text/javascript" src="/static/src/graybox.js"></script>
<script  type="text/javascript" src="/static/js/adsense.js"></script>
<script  type="text/javascript" src="/static/js/v2/app/clientCache.js"></script>
<script  type="text/javascript" src="/static/js/globalSearch.js"></script>
<script  type="text/javascript" src="/static/js/sponsoredNavigation.js"></script>
<script type="text/javascript" src="/static/js/error-logger.js"></script>
<script  type="text/javascript" src="/static/js/common.js"></script>
<script  type="text/javascript" src="/static/js/lead-conversion-tracking.js"></script>
<script  type="text/javascript" src="/static/js/cookie-migration.js"></script>
<script  type="text/javascript" src="/static/js/newcar-common.js"></script>
<script  type="text/javascript" src="/static/js/v2/app/cwTracking.js"></script>
<script  type="text/javascript" src="/static/js/location-plugin.js"></script>
<script  type="text/javascript" src="/static/js/pricebreakup.js"></script>
<script type="text/javascript">
    cwTracking.hostPath = '<%= ConfigurationManager.AppSettings["HostUrl"] %>';
</script>

    <script>
        //CW Track Code
        Common.googleApiKey = '<%= (System.Configuration.ConfigurationManager.AppSettings["APIKey"] ?? "") %>';
    var url = unescape(window.location);
    var landingURL = url;
    var imgCreation = new Image();
    var hashIndex = url.indexOf("#");

    url = url.substr(url.indexOf("?") + 1, hashIndex == -1 ? url.length : url.indexOf("#") - (url.indexOf("?") + 1));
    landingURL = landingURL.substr(0, landingURL.indexOf("?"));

    var searchAttributes = url.split('&');

    for (var no = 0; no < searchAttributes.length; no++) {
        var cutSrc = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc"), searchAttributes[no].indexOf("="))
        if (cutSrc == 'ltsrc') {
            var qryString = searchAttributes[no].substr(searchAttributes[no].indexOf("ltsrc") + 6, searchAttributes[no].length)
            imgCreation.src = "/lts/ts.aspx?c=" + qryString + "&refUrl=" + landingURL;
        }
    }
    //End of CW Track code
    </script>
<!-- Google Tag Manager -->
<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-W2Z3ZM"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
<script>(function (w, d, s, l, i) {
    w[l] = w[l] || []; w[l].push({
        'gtm.start':
        new Date().getTime(), event: 'gtm.js'
    }); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
    '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
})(window, document, 'script', 'dataLayer', 'GTM-W2Z3ZM');</script>
<!-- End Google Tag Manager -->

<% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial("~/Views/Shared/_OutbrainScriptView.cshtml", null); %>

<script language="c#" runat="server">	
    private ChatResponse chatFlags = null;
    private int PageId;
</script>

<%
    chatFlags = ChatManagementCache.GetChatFlags(PageId); 
%>

<% if (chatFlags != null && chatFlags.IsActive) { %>
       <script type="text/javascript">
           var isChatOn = '<%= chatFlags.IsChatOn.ToString().ToLower() %>';
            </script>
            <script type="text/javascript" src="/static/js/zopimChat.js"></script>
<% } %>
<meta charset="utf-8">
<script language="c#" runat="server">
    private string title = "", relPrevPageUrl = string.Empty, relNextPageUrl = string.Empty, description = "", keywords = "", AdId = "", AdPath = "", canonical = "", TargetedModel = "", TargetedMakes = "", TargetedModels = "", TargetedCity = ""
        , OGImage = "";
    private ushort feedbackTypeId = 0;
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private bool Ad_320x50 = false, Ad_Bot_320x50 = false, Ad_300x250 = false, Ad320x150_I = false, Ad320x150_II = false,
        EnableOG = true, Ad_Mid_320x50 = false, ShowSellBikeLink = false;
</script>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title><%=title %></title>
<meta name="description" content="<%=description%>" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<meta name="google-site-verification" content="fG4Dxtv_jDDSh1jFelfDaqJcyDHn7_TCJH3mbvq6xW8" />
<meta name="theme-color" content="#2a2a2a" />
<% if(!String.IsNullOrEmpty(keywords)) { %>
<meta name="keywords" content="<%= keywords %>" /><% } %>
<%if (!String.IsNullOrEmpty(canonical)) { %>
<link rel="canonical" href="<%=canonical %>" /><% } %>
<%if(!String.IsNullOrEmpty(relPrevPageUrl)) { %>
<link rel="prev" href="<%= relPrevPageUrl %>" /><% } %>
 <%if(!String.IsNullOrEmpty(relNextPageUrl)){ %>
<link rel="next" href="<%= relNextPageUrl %>" /><% }%>
<link rel="SHORTCUT ICON" href="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>
<style>
    @@font-face {
        font-family: 'Open Sans';
        font-style: normal;
        font-weight: 400;
        src: local('Open Sans Regular'), local('OpenSans-Regular'), url('/css/fonts/OpenSans/open-sans-v15-latin-regular.woff') format('woff');
    }
    @@font-face {
	    font-family: 'Open Sans';
	    font-style: normal;
	    font-weight: 700;
	    src: local('Open Sans Bold'), local('OpenSans-Bold'), url('/css/fonts/OpenSans/open-sans-v15-latin-700.woff') format('woff');
	}
</style>


<link href="/m/css/bwm-common-style.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

<%if (EnableOG)
  { %>
<meta property="og:title" content="<%=title %>" />
<meta property="og:type" content="website" />
<meta property="og:description" content="<%=description%>" />
    <%if(!String.IsNullOrEmpty(canonical)) { %><meta property="og:url" content="<%=canonical %>" /> <% } %>
<meta property="og:image" content="<%= string.IsNullOrEmpty(OGImage) ? Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo : OGImage %>" />
<% } %>


<script type="text/javascript" src="<%= staticUrl  %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
<script type="text/javascript">
   var ga_pg_id = '0';    
</script>
<!-- #include file="\includes\gacode.aspx" -->
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

<script type='text/javascript'>
    googletag.cmd.push(function () {
        <% if(Ad_320x50) { %>googletag.defineSlot('<%= AdPath%>Top_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());<% } %>
        <% if(Ad_Bot_320x50) { %>googletag.defineSlot('<%= AdPath%>Bottom_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());<% } %>
        <% if (Ad_300x250) { %>googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());<% } %>
        <% if (Ad320x150_I) { %>
        googletag.defineSlot('<%= AdPath%>FirstSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads());
        <% } %>
        <% if (Ad320x150_II) { %>
        googletag.defineSlot('<%= AdPath%>SecondSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-4').addService(googletag.pubads());
        <% } %>
        <% if (!String.IsNullOrEmpty(TargetedModel)) { %>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedMakes)){ %>googletag.pubads().setTargeting("Make", "<%= TargetedMakes %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedModels)){ %>googletag.pubads().setTargeting("CompareBike-M", "<%= TargetedModels %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedCity)){%>googletag.pubads().setTargeting("City", "<%= TargetedCity %>");<%}%>
        googletag.pubads().enableSingleRequest();
        googletag.pubads().collapseEmptyDivs();
        googletag.enableServices();
    });
</script>

<script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>

<% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/m/src/html5.js"></script>
<![endif]-->
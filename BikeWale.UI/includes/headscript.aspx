<%@ Import namespace ="Bikewale.Utility" %>
<script language="c#" runat="server">	
    private string staticUrl = Bikewale.Utility.BWConfiguration.Instance.StaticUrl;
    private string staticFileVersion = Bikewale.Utility.BWConfiguration.Instance.StaticFileVersion;
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate = "", ShowTargeting = "", TargetedModel = "", TargetedSeries = "", TargetedMake = "", TargetedModels = "", canonical = "", TargetedCity = ""
        , fbTitle = "", fbImage,
        ogImage = "";
    private ushort feedbackTypeId = 0;
    private bool isHeaderFix = true,
        isAd970x90Shown = true,
        isAd970x90BTFShown = false,
        isAd970x90BottomShown = true,
        isAd976x400FirstShown = false,
        isAd976x400SecondShown = false,
        isAd976x204 = false,
        isAd300x250BTFShown=true,
        isAd300x250Shown=true,
        isTransparentHeader = false,
        enableOG = true;
</script>

<title><%= title %></title>
<meta name="description" content="<%= description %>" />
<meta charset="utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
<meta name="google-site-verification" content="fG4Dxtv_jDDSh1jFelfDaqJcyDHn7_TCJH3mbvq6xW8" />
<% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
<%if(!String.IsNullOrEmpty(alternate)) { %><link rel="alternate" type="text/html" media="handheld" href="<%= alternate %>" title="Mobile/PDA" /><% } %>
<%if(!String.IsNullOrEmpty(canonical)) { %>
<link rel="canonical" href="<%=canonical %>" /> 
<% } %>
<%if(enableOG) { %>
<meta property="og:title" content="<%= title %>" />
<meta property="og:type" content="website" />
<meta property="og:description" content="<%= description %>" />
<%if(!String.IsNullOrEmpty(canonical)) { %><meta property="og:url" content="<%=canonical %>" /> <% } %>
<meta property="og:image" content = "<%= string.IsNullOrEmpty(ogImage) ? Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo : ogImage %>" />
<% } %>
<script type="text/javascript">
    bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrlForJs"] %>';
    var ga_pg_id = '0';
</script>
<!-- #include file="\includes\gacode.aspx" -->

<script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>

<link rel="SHORTCUT ICON" href="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>


        <%
            string fontFile = "/css/fonts/OpenSans/open-sans-v15-latin-regular.woff",
            fontUrl = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion);
        %>
        <style>
            @font-face { 
                font-family: 'Open Sans';
                font-style: normal;
                font-weight: 400;
                src: local('Open Sans Regular'), local('OpenSans-Regular'), url('<%= fontUrl%>') format('woff');
             }
        </style>
        
        <%  
            fontFile = "/css/fonts/OpenSans/open-sans-v15-latin-700.woff";
            fontUrl  = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion); 
        %>
        <style>
             @font-face { 
	            font-family: 'Open Sans';
	            font-style: normal;
	            font-weight: 700;
	            src: local('Open Sans Bold'), local('OpenSans-Bold'), url('<% =fontUrl %>') format('woff');
             }
        </style>

<link href="<%= staticUrl  %>/css/bw-common-style.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="<%= staticUrl %>/src/frameworks.js?<%=staticFileVersion %>"></script>
<script type='text/javascript'>
    var googletag = googletag || {};
    googletag.cmd = googletag.cmd || [];
    (function () {
        var gads = document.createElement('script');
        gads.async = true;
        gads.type = 'text/javascript';
        var useSSL = 'https:' == document.location.protocol;
        gads.src = (useSSL ? 'https:' : 'http:') +
        '//www.googletagservices.com/tag/js/gpt.js?v=1.0';
        var node = document.getElementsByTagName('script')[0];
        node.parentNode.insertBefore(gads, node);
    })();
</script>
<script type='text/javascript'>
    googletag.cmd.push(function () {
        <% if(isAd300x250Shown){ %>
        googletag.defineSlot('<%= AdPath%>300x250', [[300, 250]], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());                    
        <% } %>
        <% if(isAd300x250BTFShown){ %>
        googletag.defineSlot('<%= AdPath%>300x250_BTF', [[300, 250]], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());        
        <% } %>
        <% if(isAd970x90Shown){ %>
        googletag.defineSlot('<%= AdPath%>970x90', [[970, 66], [970, 60], [960, 90], [950, 90], [960, 66], [728, 90], [960, 60], [970, 90]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads()); 
        <% } %>
        <% if(isAd970x90BTFShown){ %>
        googletag.defineSlot('<%= AdPath%>970x90_BTF', [[970, 200],[970, 150],[960, 60], [970, 66], [960, 90], [970, 60], [728, 90], [970, 90], [960, 66]], 'div-gpt-ad-<%= AdId%>-4').addService(googletag.pubads());
        <% } %>
        <% if(isAd970x90BottomShown){ %>
        googletag.defineSlot('<%= AdPath%>Bottom_970x90', [[970, 60], [960, 90], [970, 66], [960, 66], [728, 90], [970, 90], [950, 90], [960, 60]], 'div-gpt-ad-<%= AdId%>-5').addService(googletag.pubads());
        <% } %>
        <% if (isAd976x400FirstShown){ %>
        googletag.defineSlot('<%= AdPath%>FirstSlot_976x400',[[976, 150], [976, 100], [976, 250], [976, 300], [976, 350], [976, 400], [970, 90], [976, 200]],'div-gpt-ad-<%= AdId%>-6').addService(googletag.pubads());
        <% } %>
        <% if (isAd976x400SecondShown) { %>
        googletag.defineSlot('<%= AdPath%>SecondSlot_976x400',[[976, 150], [976, 100], [976, 250], [976, 300], [976, 350], [976, 400], [970, 90], [976, 200]],'div-gpt-ad-<%= AdId%>-7').addService(googletag.pubads());
        <% } %>
        <% if (isAd976x204) { %>
        googletag.defineSlot('/1017752/BikeWale_HomePageNews_FirstSlot_976x204', [[976, 200], [976, 250], [976, 204]], 'div-gpt-ad-1395985604192-8').addService(googletag.pubads());
        <% } %>
        
        googletag.pubads().enableSingleRequest();
        <% if(!String.IsNullOrEmpty(TargetedModel)){%>googletag.pubads().setTargeting("Model", "<%= TargetedModel.RemoveSpecialCharacters() %>");<%}%>             
        <% if(!String.IsNullOrEmpty(TargetedMake)){%>googletag.pubads().setTargeting("Make", "<%= TargetedMake.RemoveSpecialCharacters() %>");<%}%>
        <% if(!String.IsNullOrEmpty(TargetedModels)){%>googletag.pubads().setTargeting("CompareBike-D", "<%= TargetedModels.RemoveSpecialCharacters() %>");<%}%>
        <% if (!String.IsNullOrEmpty(TargetedCity)){%>googletag.pubads().setTargeting("City", "<%= TargetedCity.RemoveSpecialCharacters() %>");<%}%>
        
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>

<script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>


<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/src/html5.js"></script>
<![endif]-->

<%@ Register Src="/m/controls/BookBikeSlug.ascx" TagPrefix="BikeBooking" TagName="BookBikeSlug" %>
<html xmlns="http://www.w3.org/1999/xhtml"  xmlns:fb="http://www.facebook.com/2008/fbml" lang="en">
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", AdId = "", AdPath = "",ShowTargeting="",TargetedModel="", TargetedSeries="", TargetedMakes="",TargetedModels="", AdModel_300x250=""
        ,AdSeries_300x250="", canonical = "",relPrevPageUrl = "",relNextPageUrl = "",fbTitle = "",fbImage = "", menu = "",OGImage = "";
        private bool Ad_320x50 = false, Ad_Bot_320x50 = false, Ad_300x250 = false, EnableOG = true, ShowSellBikeLink=false;
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    </script> 
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
    <% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
    <meta name="description" content="<%= description %>" />
    <% if(!string.IsNullOrEmpty(canonical)){ %><link rel="canonical" href="<%= canonical %>" /> <% } %>
    <%if (EnableOG){ %>
    <meta property="og:title" content="<%=title %>" />
    <meta property="og:type" content="website" />
    <meta property="og:description" content="<%=description%>" />
    <%if(!String.IsNullOrEmpty(canonical)) { %><meta property="og:url" content="<%=canonical %>" /> <% } %>
    <meta property="og:image" content="<%= string.IsNullOrEmpty(OGImage) ? Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo : OGImage %>" />
    <% } %>
    <title><%= title %></title>
    <%if(!String.IsNullOrEmpty(relPrevPageUrl)) { %><link rel="prev" href="<%= relPrevPageUrl %>" /><% } %>
    <%if(!String.IsNullOrEmpty(relNextPageUrl)){ %><link rel="next" href="<%= relNextPageUrl %>" /><% }%>
    <meta name="theme-color" content="#2a2a2a" />
    <!-- #include file="/includes/gacode.aspx" --> 
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
        <% if(Ad_320x50) { %> googletag.defineSlot('<%= AdPath%>Top_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());<% } %>
        <% if(Ad_Bot_320x50) { %> googletag.defineSlot('<%= AdPath%>Bottom_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());<% } %>
        <% if (Ad_300x250) { %> googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());<% } %>  
        
        <% if(!String.IsNullOrEmpty(ShowTargeting)) { %>
        googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");        
        googletag.pubads().setTargeting("Make", "<%= TargetedMakes %>");
        googletag.pubads().setTargeting("CompareBike-M", [<%= TargetedModels %>]);
        <% } %>        
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>
    <div id="fb-root"></div>
    <script>(function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.0";
        fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));

        //Twitter Tweet Button script
        !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");

        //Google +1 Button script
        (function () {
            var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
            po.src = 'https://apis.google.com/js/plusone.js?v=1.0';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
        })();
    </script>

    <script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>

    <% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
    <!-- #include file="\includes\globalStaticFiles_mobile.aspx" -->
</head>
<body>
    <form runat="server">    
    <div id="divParentPageContainer" data-role="page" style="position:relative;">
        <div role="main">
        	<!-- Header code starts here-->
            <!-- #include file="/includes/headBW_Mobile.aspx" --> 
            <!-- Header code ends here-->
            <!-- inner-section code starts here-->
            <div class="inner-section">
               
              
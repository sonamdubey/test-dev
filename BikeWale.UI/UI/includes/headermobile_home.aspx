<%@ Register Src="/UI/m/controls/BookBikeSlug.ascx" TagPrefix="BikeBooking" TagName="BookBikeSlug" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml"  xmlns:fb="http://www.facebook.com/2008/fbml" lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
	<meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <meta name="canonical" content="<%= canonical %>" />
    <% if(!String.IsNullOrEmpty(fbTitle) && !String.IsNullOrEmpty(fbImage)) { %>
        <meta property="og:type" content="website" />
        <meta property="og:title" content="<%=fbTitle%>"/> 
        <meta property="og:image" content="<%=fbImage%>"/> 
        <meta property="og:url" content="<%= canonical %>" />
        <meta property="og:description" content="<%= description %>" />
    <% } %>
    <title><%= title %></title>
    <%if(!String.IsNullOrEmpty(relPrevPageUrl)) { %><link rel="prev" href="<%= relPrevPageUrl %>" /><% } %>
    <%if(!String.IsNullOrEmpty(relNextPageUrl)){ %><link rel="next" href="<%= relNextPageUrl %>" /><% }%>
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", 
        canonical = "",relPrevPageUrl = "",relNextPageUrl = "",fbTitle = "",fbImage = "", menu = "", Ad_HP_Banner_400x310 = "";
        private bool isHeaderFix = true, isAd320x50Shown = false,isAd300x250Shown = false, ShowSellBikeLink=false;
        private bool Ad_320x50 = false, Ad_Bot_320x50 = false, Ad_300x250 = false;
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    </script> 
    <!-- #include file="/UI/includes/gacode.aspx" --> 
    <script type='text/javascript'>
        (function () {
            var useSSL = 'https:' == document.location.protocol;
            var src = (useSSL ? 'https:' : 'http:') +
            '//www.googletagservices.com/tag/js/gpt.js';
            document.write('<scr' + 'ipt src="' + src + '"></scr' + 'ipt>');
        })();
    </script>
    <script type='text/javascript'>
        googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath%>Top_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath%>Bottom_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());      
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSyncRendering();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
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

    <!-- #include file="\UI\includes\globalStaticFiles_mobile.aspx" -->
</head>
<body>
    <form runat="server">    
    <div id="divParentPageContainer" data-role="page" style="position:relative;">
        <div role="main">
        	<!-- Header code starts here-->
            <!-- #include file="/UI/includes/headBW_Mobile.aspx" --> 
            <!-- Header code ends here-->
            <!-- inner-section code starts here-->
            <%--<div class="inner-section">
                <!-- Ad unit code starts here-->
                <div class="ad-unit">
                    <!-- #include file="/UI/ads/Ad320x50_mobile_Sync.aspx" -->
                </div>--%>

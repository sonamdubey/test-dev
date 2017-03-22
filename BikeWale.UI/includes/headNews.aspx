﻿<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
    <meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <% if(!String.IsNullOrEmpty(fbTitle) && !String.IsNullOrEmpty(fbImage)) { %>
        <meta property="og:type" content="website" />
        <meta property="og:title" content="<%=fbTitle%>"/> 
        <meta property="og:image" content="<%=fbImage%>"/> 
        <meta property="og:url" content="<%= canonical %>" />
    <% } %>
    <title><%= title %></title>
    <% if(!string.IsNullOrEmpty(canonical)){ %><link rel="canonical" href="<%= canonical %>" /> <% } %>
    <% if(!string.IsNullOrEmpty(alternate)){ %><link rel="alternate" href="<%= alternate %>" /> <% } %>
    <% if( prevPageUrl != "" ) { %><link rel="prev" href="<%= prevPageUrl %>" /><% } %>
    <% if( nextPageUrl != "" ) { %><link rel="next" href="<%= nextPageUrl %>" /><% } %>
    <!-- #include file="globalStaticFiles.aspx"-->
    <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
    <script src="https://st2.aeplcdn.com/src/jquery.jcarousel.min.js" type="text/javascript"></script>
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", canonical = "",prevPageUrl = "",nextPageUrl = "", fbTitle = "", fbImage = "", AdId = "", AdPath = "",alternate = "";	    
      
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
        private bool isHeaderFix = true,isAd970x90Shown = true,isAd970x90BTFShown = false,isAd970x90BottomShown = true,isAd300x250Shown=true,isAd300x250_BTFShown=true;        
    </script>
    <!-- #include file="/includes/gacode.aspx" --> 
    <script type="text/javascript">
        setTimeout(function () {
            var a = document.createElement("script");
            var b = document.getElementsByTagName("script")[0];
            a.src = document.location.protocol + "//script.crazyegg.com/pages/scripts/0012/9477.js?" + Math.floor(new Date().getTime() / 3600000);
            a.async = true; a.type = "text/javascript"; b.parentNode.insertBefore(a, b)
        }, 1);
    </script>
    <script type='text/javascript'>
        var ga_pg_id = '0';
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
            googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());
              <% } %>
             <% if(isAd300x250_BTFShown){ %>
            googletag.defineSlot('<%= AdPath%>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());
              <% } %>
            <%--googletag.defineSlot('<%= AdPath%>300x600_BTF', [[120, 240], [120, 600], [160, 600], [250, 250], [300, 250], [300, 600]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads());
            --%>
            <% if(isAd970x90Shown){ %>
            googletag.defineSlot('/1017752/Bikewale_NewBike_970x90', [[970, 66], [970, 60], [960, 90], [950, 90], [960, 66], [728, 90], [960, 60], [970, 90]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads()); 
            <% } %>
            <% if(isAd970x90BottomShown){ %>
            googletag.defineSlot('/1017752/Bikewale_NewBike_Bottom_970x90', [[970, 60], [960, 90], [970, 66], [960, 66], [728, 90], [970, 90], [950, 90], [960, 60]], 'div-gpt-ad-<%= AdId%>-5').addService(googletag.pubads());
            <% } %>
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>

    <script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>

    <% Bikewale.Utility.BWCookies.SetBWUtmz(); %>

    <div id="fb-root"></div>
    <script type="text/javascript">
        //facebook like button script
        ( function ( d, s, id ) {
            var js, fjs = d.getElementsByTagName( s )[0];
            if ( d.getElementById( id ) ) return;
            js = d.createElement( s ); js.id = id;
            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
            fjs.parentNode.insertBefore( js, fjs );
        }( document, 'script', 'facebook-jssdk' ) );

        //Twitter Tweet Button script
        !function ( d, s, id ) { var js, fjs = d.getElementsByTagName( s )[0]; if ( !d.getElementById( id ) ) { js = d.createElement( s ); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore( js, fjs ); } }( document, "script", "twitter-wjs" );
        
        //Google +1 Button script
        ( function () {
            var po = document.createElement( 'script' ); po.type = 'text/javascript'; po.async = true;
            po.src = 'https://apis.google.com/js/plusone.js?v=1.0';
            var s = document.getElementsByTagName( 'script' )[0]; s.parentNode.insertBefore( po, s );
        } )();
    </script>
</head>
<body class="bg-white header-fixed-inner">
    <form runat="server">    
	<div class="main-container">
    	<!-- #include file="/includes/headBW.aspx" -->
        <%--<section class="bg-white">
            <div class="container_12">
                <div class="grid_12">
                    <div class="padding-bottom5 text-center">
                        <!-- #include file="/ads/Ad970x90.aspx" -->
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>--%>
        <div class="clear"></div> 
    	
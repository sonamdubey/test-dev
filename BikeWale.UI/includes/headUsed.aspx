﻿<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <title><%= title %></title>
    <% if( canonical != "" ) { %><link rel="canonical" href="<%=canonical%>" /><% } %>
    <% if( prevPageUrl != "" ) { %><link rel="prev" href="<%= prevPageUrl %>" /><% } %>
    <% if( nextPageUrl != "" ) { %><link rel="next" href="<%= nextPageUrl %>" /><% } %>
    <!-- #include file="globalStaticFiles.aspx"-->    
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", canonical = "", prevPageUrl = "", nextPageUrl = "", AdId = "", AdPath = "";
        private bool isHeaderFix = true, isAd970x90Shown = false;
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
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
            googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath%>300x600_BTF', [[120, 240], [120, 600], [160, 600], [250, 250], [300, 250], [300, 600]], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath%>728x90', [728, 90], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
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
<body class="header-fixed-inner">
    <form runat="server">
    <!-- #include file="/includes/gacode.aspx" --> 
	<div class="main-container">
    	<!-- #include file="/includes/headBW.aspx" -->        
        
        
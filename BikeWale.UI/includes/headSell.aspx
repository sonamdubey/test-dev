﻿<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <title><%= title %></title>
    <!-- #include file="globalStaticFiles.aspx"-->
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", AdId = "", AdPath = "";	    
        private bool isHeaderFix = true, isAd970x90Shown = true, isAd970x90BottomShown = true;   
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
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
            googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath%>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());
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
    <% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
</head>
<body class="header-fixed-inner">
    <form runat="server">    
	<div class="main-container">
    	<!-- #include file="/includes/headBW.aspx" -->
        
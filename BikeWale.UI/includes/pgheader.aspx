<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
    <meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <%if(!string.IsNullOrEmpty(alternate)) {%><meta name="alternate" content="<%= alternate %>" /><%} %>
 
    <title><%= title %></title>
    <% if(!string.IsNullOrEmpty(canonical)){ %><link rel="canonical" href="<%= canonical %>" /> <% } %>

    <!-- #include file="globalStaticFiles.aspx"-->
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "",ShowTargeting="",TargetedModel="", TargetedSeries="", TargetedMake="", canonical = "",prevPageUrl = "",nextPageUrl = "", fbTitle = "", fbImage = "", AdId = "", AdPath = "", alternate="";	    
    </script>
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
            googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath%>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath%>728x90', [728, 90], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
            <% if(!String.IsNullOrEmpty(ShowTargeting)) { %>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");
            googletag.pubads().setTargeting("Series", "<%= TargetedSeries %>");
            googletag.pubads().setTargeting("Make", "<%= TargetedMake %>");
            <% } %>
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
</head>
<body>
    <!-- #include file="/includes/gacode.aspx" --> 
	<div class="main-container">
        <!--Header code start here -->
      <div class="bw-header">
            <div class="left-float">
        	    <a href="http://www.bikewale.com/"><img src="http://img1.carwale.com/bikewaleimg/images/bikebooking/images/bikewale_logo_s.png" alt="bikewale"></a>
            </div>
    	    <div class="clear"></div>
      </div>
        <!--Header code end here -->
        
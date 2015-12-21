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
        private bool isHeaderFix = true;
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
<body class="header-fixed-inner">
    <form runat="server">    
	<div class="main-container">
        <!--Header code start here -->
        <div id="header" class="header-fixed">
            <div class="left-float margin-left50">
                <a href="/" class="bwsprite bw-logo"></a>
            </div>
            <div class="clear"></div>
        </div>
        <!--Header code end here -->
        
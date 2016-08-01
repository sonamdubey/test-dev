<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml"> 
    <%--html xmlns:fb="http://www.facebook.com/2008/fbml" xmlns:og="http://opengraphprotocol.org/schema/" --%>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
    <%if(keywords != "") {%>
<meta name="keywords" content="<%= keywords %>" />
    <%} %>
<meta name="description" content="<%= description %>" />
<%if(!string.IsNullOrEmpty(alternate)) {%><meta name="alternate" content="<%= alternate %>" /><%} %>
<% if(!String.IsNullOrEmpty(fbTitle) && !String.IsNullOrEmpty(fbImage)) { %>
<meta property="og:type" content="website" />
<meta property="og:title" content="<%=fbTitle%>"/> 
<meta property="og:image" content="<%=fbImage%>"/> 
<meta property="og:url" content="<%= canonical %>" />
<meta property="og:description" content="<%= description %>" />
 <% } %>
<title><%= title %></title>
<% if(!string.IsNullOrEmpty(canonical)){ %><link rel="canonical" href="<%= canonical %>" /> <% } %>
<% if( prevPageUrl != "" ) { %><link rel="prev" href="<%= prevPageUrl %>" /><% } %>
<% if( nextPageUrl != "" ) { %><link rel="next" href="<%= nextPageUrl %>" /><% } %>
<!-- #include file="globalStaticFiles.aspx"-->
    <script src="http://st2.aeplcdn.com/src/jquery.jcarousel.min.js" type="text/javascript"></script>
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "",ShowTargeting="",TargetedModel="", TargetedSeries="", TargetedMake="",TargetedModels ="", canonical = "",prevPageUrl = "",nextPageUrl = "", fbTitle = "", fbImage = "", AdId = "", AdPath = "", alternate="";	    
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
            <% if(isAd970x90Shown){ %>
                googletag.defineSlot('/1017752/Bikewale_NewBike_970x90', [[970, 66], [970, 60], [960, 90], [950, 90], [960, 66], [728, 90], [960, 60], [970, 90]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads());
            <% } %>
            <% if(isAd970x90BottomShown){ %>
                googletag.defineSlot('/1017752/Bikewale_NewBike_Bottom_970x90', [[970, 60], [960, 90], [970, 66], [960, 66], [728, 90], [970, 90], [950, 90], [960, 60]], 'div-gpt-ad-<%= AdId%>-5').addService(googletag.pubads());
            <% } %>
            <% if(!String.IsNullOrEmpty(ShowTargeting)) { %>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");
            googletag.pubads().setTargeting("Series", "<%= TargetedSeries %>");
            googletag.pubads().setTargeting("Make", "<%= TargetedMake %>");
            googletag.pubads().setTargeting("CompareBike-D", "<%= TargetedModels %>");
            <% } %>
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
    <% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
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
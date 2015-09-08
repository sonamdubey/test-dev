<%@ Register Src="/m/controls/BookBikeSlug.ascx" TagPrefix="BikeBooking" TagName="BookBikeSlug" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://ogp.me/ns/fb#" xmlns:fb="http://www.facebook.com/2008/fbml">
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
        <meta property="og:image:width" content="200" />
        <meta property="og:image:height" content="200" />
        <meta property="og:url" content="<%= canonical %>" />
    <% } %>
    <title><%= title %></title>
    <%if(!String.IsNullOrEmpty(relPrevPageUrl)) { %><link rel="prev" href="<%= relPrevPageUrl %>" /><% } %>
    <%if(!String.IsNullOrEmpty(relNextPageUrl)){ %><link rel="next" href="<%= relNextPageUrl %>" /><% }%>
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", 
        canonical = "",relPrevPageUrl = "",relNextPageUrl = "",fbTitle = "",fbImage = "", menu = "", Ad_HP_Banner_400x310 = "";
    </script> 
    <script type='text/javascript'>
        (function () {
            var useSSL = 'https:' == document.location.protocol;
            var src = (useSSL ? 'https:' : 'http:') +
            '//www.googletagservices.com/tag/js/gpt.js';
            document.write('<scr' + 'ipt src="' + src + '"></scr' + 'ipt>');
        })();
    </script>
    <script type='text/javascript'>
        googletag.defineSlot('<%= AdPath%>_Top_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath%>_Bottom_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());      
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
    <!-- #include file="\includes\globalStaticFiles_mobile.aspx" -->
</head>
<body>
    <!-- #include file="/includes/gacode.aspx" --> 
    <div id="divParentPageContainer" data-role="page" style="position:relative;">
        <div role="main">
        	<!-- Header code starts here-->
            <!-- #include file="/includes/headBW_Mobile.aspx" --> 
            <!-- Header code ends here-->
            <!-- inner-section code starts here-->
            <div class="inner-section">
                <!-- Ad unit code starts here-->
                <div class="ad-unit">
                    <!-- #include file="/ads/Ad320x50_mobile_Sync.aspx" -->
                </div>

﻿<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://ogp.me/ns/fb#">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
    <meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <meta name="alternate" content="<%= alternate %>" />
    <title><%= title %></title>
    <!-- #include file="globalStaticFiles.aspx"-->
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate="";
        private ushort feedbackTypeId = 0; 	    
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
            googletag.defineSlot('<%= AdPath%>728x90', [728, 90], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
            googletag.defineSlot('/1017752/BikeWale_FeaturedBike_600x270', [600, 270], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());
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
<body>
    <!-- #include file="/includes/gacode.aspx" -->     
	<div class="main-container">
    	<!--Top nav code start here -->
    	<div class="top-nav">
        	<div class="container_12">
            	<div class="grid_9">
                    <ul class="left-float">
                    	<li><a href="http://www.carwale.com">CarWale</a></li>
                        <li>|</li>
                        <li><a href="/aboutus.aspx">About Us</a></li>
                        <li>|</li>                        
                        <%--<li><a href="/advertisewithus.aspx">Advertise with Us</a></li>                       
                        <li>|</li>
                        <li><a href="/contactus.aspx">Contact Us</a></li>
                        <li>|</li>--%>                
                    </ul>
                    <div class="left-float margin-left10">                        	
                        <fb:like href="https://www.facebook.com/pages/BikeWale/471265026228033" layout="button_count" show-faces="false" action="like" colorscheme="light" />
                    </div>
                    <div class="left-float margin-left10">                            
                        <a href="https://twitter.com/bikewale" class="twitter-follow-button" data-show-count="true" data-lang="en">Follow @twitter</a>    
                    </div>
                    <div class="left-float margin-left10 hide">
                        <div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com"></div>
                    </div>
                </div>
                <BikeWale:LoginStatus Id="ctrl_LoginStatus" runat="server" />                
            </div>
        </div>
        <!--Top nav code end here -->
        
        <!--Header code start here -->
        <div class="header-container">            
            <div class="container_12">
                <div class="grid_12 margin-top5">
                    <!-- logo code start here -->
                    <div class="bw-logo  left-float"><a href="/"></a></div>
                    <!-- logo code end here -->
                    <!-- Ad Slot code start here -->
                    <div class="ad-slot left-float">
                        <!-- #include file="/ads/Ad728x90.aspx" -->
                    </div>
                    <!-- Ad Slot code end here -->
                </div>
            </div>
            <div class="container_12">
            	<!-- Primary Navigation start here -->
                <div class=" grid_12 primary-nav-container">
                	<ul>
                    	<li class="active"><a href="/">Home</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/new/">New Bikes</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/used/">Used Bikes</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/used/sell/">Sell Bike</a></li>
                        <li class="pri-nav-sept"></li>
                        <%--<li><a href="/forums/">Forum</a></li>--%>
                        <li><a href="/finance/emicalculator.aspx">Tools</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/news/">News</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/mybikewale/">My BikeWale</a></li>
                        <li class="pri-nav-sept"></li>
                      <%--  <li><a href="/autoexpo/2014/" class="ae-link">AutoExpo 2014</a></li>--%>
                    </ul>
                </div>
                <!-- Primary Navigation end here -->
            </div>
        </div>
        <!--Header code end here -->
        
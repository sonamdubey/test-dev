<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
	<meta name="keywords" content="<%= keywords %>" />
	<meta name="description" content="<%= description %>" />
    <link rel="alternate" type="text/html" media="handheld" href="<%= alternate %>" title="Mobile/PDA" />
	<title><%= title %></title>
	<link rel="SHORTCUT ICON" href="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>
	<!-- #include file="globalStaticFiles.aspx"-->
	<script language="c#" runat="server">	    
		private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate="";
		private ushort feedbackTypeId = 0;
        private bool isHeaderFix = true, isAd970x90Shown = true, isAd970x90BottomShown = true, isAd300x250Shown = true, isAd600x270Shown=true; 	    
		private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
		private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
	</script>
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
            <%  if (isAd300x250Shown) { %>
			googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());
            <% } %>
            <%  if (isAd600x270Shown) { %>
			googletag.defineSlot('/1017752/BikeWale_FeaturedBike_600x270', [600, 270], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());
            <% } %>
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

    <script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>

	<% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
</head>
<body class="header-fixed-inner">
	<form runat="server">    
	<div class="main-container">
		<!-- #include file="/includes/headBW.aspx" -->    
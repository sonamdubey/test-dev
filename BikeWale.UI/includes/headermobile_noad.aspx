<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
	<meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <meta name="canonical" content="<%= canonical %>" />
    <title><%= title %></title>
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", canonical = "",menu="", TargetedModel = "", TargetedMakes = "", TargetedModels = "", TargetedCity = "";
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];        
        private bool Ad_320x50 = false, Ad_Bot_320x50 = false, Ad_300x250 = false, Ad320x150_I = false, Ad320x150_II = false, ShowSellBikeLink=false;
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    </script> 
    <!-- #include file="/includes/gacode.aspx" --> 

    <script type='text/javascript'>
        googletag.cmd.push(function () {
            <% if(Ad_320x50) { %>googletag.defineSlot('<%= AdPath%>Top_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());<% } %>
        <% if(Ad_Bot_320x50) { %>googletag.defineSlot('<%= AdPath%>Bottom_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());<% } %>
        <% if (Ad_300x250) { %>googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());<% } %>
        <% if (Ad320x150_I) { %>
        googletag.defineSlot('<%= AdPath%>FirstSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads());
        <% } %>
        <% if (Ad320x150_II) { %>
        googletag.defineSlot('<%= AdPath%>SecondSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-4').addService(googletag.pubads());
        <% } %>
        <% if (!String.IsNullOrEmpty(TargetedModel)) { %>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedMakes)){ %>googletag.pubads().setTargeting("Make", "<%= TargetedMakes %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedModels)){ %>googletag.pubads().setTargeting("CompareBike-M", "<%= TargetedModels %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedCity)){%>googletag.pubads().setTargeting("City", "<%= TargetedCity %>");<%}%>
        googletag.pubads().enableSingleRequest();
        googletag.pubads().collapseEmptyDivs();
        googletag.enableServices();
    });
</script>
    
    <script type="text/javascript">!function (a, b) { "use strict"; function f() { if (!d) { d = !0; for (var a = 0; a < c.length; a++) c[a].fn.call(window, c[a].ctx); c = [] } } function g() { "complete" === document.readyState && f() } a = a || "docReady", b = b || window; var c = [], d = !1, e = !1; b[a] = function (a, b) { if ("function" != typeof a) throw new TypeError("callback for docReady(fn) must be a function"); return d ? void setTimeout(function () { a(b) }, 1) : (c.push({ fn: a, ctx: b }), void ("complete" === document.readyState || !document.attachEvent && "interactive" === document.readyState ? setTimeout(f, 1) : e || (document.addEventListener ? (document.addEventListener("DOMContentLoaded", f, !1), window.addEventListener("load", f, !1)) : (document.attachEvent("onreadystatechange", g), window.attachEvent("onload", f)), e = !0))) } }("docReady", window);</script>
       
    <% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
   <!-- #include file="\includes\globalStaticFiles_mobile.aspx" -->    
</head>
<body>
    <form runat="server">    
    <div data-role="page" style="position:relative;">
        <div role="main">
        	<!-- Header code starts here-->
            <!-- #include file="/includes/headBW_Mobile.aspx" --> 
            <!-- Header code ends here-->
            <!-- inner-section code starts here-->
            <div class="inner-section">

<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false"  %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="AutoExpo" TagName="RepeaterPager" src="/autoexpo/controls/RepeaterPagerNews.ascx" %>
<%@ Register TagPrefix="AutoExpo" TagName="spContent" src="/autoexpo/controls/sponsoredContent.ascx" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId          = 6;
	Title 			= "CarWale Auto Expo 2014 Mobile App";
	Description 	= "CarWale's coverage on Auto Expo 2014, the largest auto show in India.";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://autoexpo.carwale.com/2014/";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
<script src="http://cdn.topsy.com/topsy.js?init=topsyWidgetCreator" type="text/javascript"></script>

<div id="content" class="left-grid">
    <div style="background-color:#fff;padding:2px 0 2px 15px;width:100%;">
	    <div class="Mobileapp">
            <h1 class="hd1">Mobile App</h1>
            <div class="mobileapphead"><span >Description</span></div>                        
            <div class="Mobhr"></div>
            <p style="padding:20px 10px 20px 10px;">
                The official app of Auto Expo 2014 powered by CarWale, India’s number one car portal. This application helps you navigate to key stalls in Auto Expo 2014 at Pragati Maidan, New Delhi.
            <p>
            <p style="padding:0px 10px 0px 10px;">
                Features Include
                <ul style="padding:0px 10px 20px 10px; list-style:none;">
                    <li style="padding:10px"><b>Navigate Me:</b>
                        Use our detailed property maps to navigate around the exhibition floor. Find key Exhibitors locations on the map, get turn-by-turn directions for reaching the stalls.
                    </li>
                    <li style="padding:10px"><b>Event Schedule:</b>
                        Get the list of all events with essential details. See the events happening today and events that are upcoming. This will help you plan your trip to Auto Expo 2014.
                    </li>
                    <li style="padding:10px"><b>Fun and Contest:</b>
                        Answer interesting questions related to Auto Expo 2014 and win exciting prizes daily. 
                    </li>
                </ul>
            </p>
            <a href="https://market.android.com/details?id=com.n5.autoexpo&feature=search_result#?t=W251bGwsMSwxLDEsImNvbS5uNS5hdXRvZXhwbyJd" target="_blank"><img src="/autoexpo/images/appbutton.jpg" alt="Download the app now" /></a>
            <p>&nbsp;</p>
            <div class="mobileapphead" style="width:27%;"><span >App Screenshots</span></div>            
            <div class="Mobhr"></div> 
            <img src="/autoexpo/images/apps.jpg" alt="" />
        </div>	  
    </div>  
</div>
<!-- #include file="/autoexpo/includes/sidebar_other.aspx" -->
<div style="clear:both;"></div>
<!-- #include file="/autoexpo/includes/footer.aspx" -->	  
<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false"  %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId          = 10;
	Title 			= "BikeWale Auto Expo About Us";
	Description 	= "BikeWale's coverage on Auto Expo 2014, the largest auto show in India.";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://bikewale.com/autoexpo/2014/aboutautoexpo.aspx";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
<script src="http://cdn.topsy.com/topsy.js?init=topsyWidgetCreator" type="text/javascript"></script>

<div class="left-grid">
	<div style="background-color:#fff;padding:2px 0 2px 10px;width:100%;height:370px;">
        <h1 class="hd1">About Auto Expo</h1>
        <div style="float:right"><p><img src="/autoexpo/images/Logo-Auto-Expo-2014-2.png" alt="Auto Expo 2014" title="Auto Expo 2014" /></p></div>
        <p>
            Asia's largest and complete automotive show, the Auto Expo conceived in the year 1985, had its debut showcasing in 1986. From providing a platform to the Indian automotive industry, to see and learn new technologies being used by the developed world, today a quarter century later, it provides a platform to the Indian automotive industry to showcase its expertise as a sourcing hub and the global industry to launch itself in the Indian market. The Auto Expo 1986 was organized from 3 – 11 January 1986 at Pragati Maidan, New Delhi.
        </p>
        <p>&nbsp;</p>
        <p>
            The Auto Expo 2014 is being jointly organised by Automotive Component Manufacturers Association of India (ACMA), Confederation of Indian Industry (CII) and Society of Indian Automobile Manufacturers (SIAM) is scheduled from 7-11 February 2014 at India Expo Mart, Greater Noida, New Delhi, India. The Auto Expo 2014 is the 12th edition of the premiere motoring show and is expected to play host to a whole group of manufacturers of both Indian and foreign origin.
        </p>        
    </div>	    
</div>

<div style="clear:both;"></div>
<!-- #include file="/autoexpo/includes/footer.aspx" -->	  
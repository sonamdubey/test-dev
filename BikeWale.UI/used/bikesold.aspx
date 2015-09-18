﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.BikeSold" Trace="false" Debug="false" %>
<%@ Register TagPrefix="BikeWale" TagName="BikesInBudget" src="/Controls/BikesInBudget.ascx" %>
<% 
    AdId = "1395992162974";
    AdPath = "/1017752/BikeWale_UsedBikes_HomePage_";
%>
<!-- #include file="/includes/headUsed.aspx" -->
        <div class="container_12">
            <div class="grid_12">
                <ul class="breadcrumb">
                    <li>You are here: </li>
                    <li><a href="/">Home</a></li>
                    <li class="fwd-arrow">&rsaquo;</li>
                    <li><a href="/used/">Used Bikes</a></li>
                    <li class="fwd-arrow">&rsaquo;</li>                    
                    <li class="current"><strong>Bike Sold Out</strong></li>
                </ul><div class="clear"></div>
            </div>
            <div class="grid_8 margin-top15"><!--    Left Container starts here -->
                <h2>Oops... Bike Sold Out!</h2> 		        
			    <div class="border-light grey-bg margin-top5 padding5 red-text">
				        The bike you are looking for has been removed.
			    </div>	
			    <!-- Showing more bikes in this budget-->
				<BikeWale:BikesInBudget id="cbBikesInBudget" Records="15" runat="server" />   
            </div><!--    Left Container ends here -->
            <div class="grid_4 margin-top20"><!--    Right Container starts here -->
                <!-- BikeWale_UsedBike/BikeWale_UsedBike_300x250 -->
                <!-- #include file="/ads/Ad300x250.aspx" -->
            </div><!--    Right Container ends here -->
        </div>
<!-- #include file="/includes/footerInner.aspx" -->
<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/UI/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
    <meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <title><%= title %></title>
    <!-- #include file="globalStaticFiles.aspx"-->
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "";	    
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    </script>

</head>
<body>
    <form runat="server">
    <!-- #include file="/UI/includes/gacode.aspx" --> 
	<div class="main-container">
    	<!--Top nav code start here -->
    	<div class="top-nav">
        	<div class="container_12">
            	<div class="grid_8">
                    <ul>
                    	<li><a href="https://www.carwale.com">CarWale</a></li>
                        <li>|</li>
                        <li><a href="/aboutus.aspx">About Us</a></li>
                        <li>|</li>                       
                        <li><a href="/advertisewithus.aspx">Advertise with Us</a></li>
                        <li>|</li>                        
                        <li><a href="/contactus.aspx">Contact Us</a></li>
                    </ul>
                </div>
                <BikeWale:LoginStatus Id="ctrl_LoginStatus" runat="server" />
            </div>
        </div>
        <!--Top nav code end here -->
        
        <!--Header code start here -->
        <div class="header-container">
            <div class="container_12">
                <div class="grid_3 bw-logo"><a href="/"></a></div>
                <div class="grid_9 omega padding-top35">                    
                    <div class="right-float hide">
                    	<!-- search code start here -->
                        <div class="left-float">
                            <span class="icon-sprite search-lt"></span>
                            <span class="search-middle-bg">
                            	<input type="text" class="search" onclick="this.value='';" onfocus="this.select()" onblur="this.value=!this.value?'Search':this.value;" value="Search" />
                            </span>
                            <a href="#"><span class="icon-sprite search-rt"></span></a>
                        </div>
                        <!-- search code end here -->
                        <!-- Social media icon code start here -->
                        <div class="left-float margin-left10">
                        	<a href="https://www.carwale.com" target="_blank" rel="noopener"  class="icon-sprite fb-icon"></a>
                        </div>
                        <div class="left-float margin-left10">
                        	<a href="https://www.carwale.com" target="_blank" rel="noopener"  class="icon-sprite tw-icon"></a>
                        </div>
                        <!-- Social media icon code start here -->
                    </div>
                </div>
            </div>
            <div class="container_12">
            	<!-- Primary Navigation start here -->
                <div class=" grid_12 primary-nav-container">
                	<ul>
                    	<li><a href="/">Home</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/new-bikes-in-india/">New Bikes</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/used/">Used Bikes</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/used/sell/">Sell Bike</a></li>
                        <li class="pri-nav-sept"></li>
                        <%--<li class="active"><a href="/forums/">Forum</a></li>--%>
                        <li class="active"><a href="/finance/emicalculator.aspx">EMI Calculator</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/news/">News</a></li>
                        <li class="pri-nav-sept"></li>
                        <li><a href="/mybikewale/">My BikeWale</a></li>
                    </ul>
                </div>
                <!-- Primary Navigation end here -->
            </div>
        </div>
        <!--Header code end here -->

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bikewale.BikeBooking.Default" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/cancellation.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
    <%
        isAd970x90Shown = false;
         %>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="booking-cancellation-banner">    	
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom10">Bike Booking</h1>
                    <p class="font20">Book your Bike in quick steps</p>
                </div>
            </div>
        </header>

        <section>
            <div class="container margin-bottom30 margin-top30">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <p class="font14 padding-left5"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
       
    </form>
</body>
</html>


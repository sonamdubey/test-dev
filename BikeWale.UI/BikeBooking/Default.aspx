<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bikewale.BikeBooking.Default" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bookinglanding.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
    <%
        isAd970x90Shown = false;
         %>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="booking-landing-banner">    	
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Book your dream bike</h1>
                    <p class="font20">Online booking is now available in</p>
                    <p class="font22 text-bold margin-bottom45">Mumbai, Pune and Bengaluru</p>
                    <div class="booking-landing-search-container">

                    </div>
                </div>
            </div>
        </header>

        <section>
            <div id="onlineBenefitsWrapper" class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom30 font28">Benefits of booking online</h2>
                    <ul>
                        <li>
                            <div class="icon-outer-container rounded-corner50 margin-bottom20">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite question-mark-icon"></span>
                                </div>
                            </div>
                            <div>Exclusive<br />offers</div>
                        </li>
                        <li>
                            <span></span>
                            <span>Save on<br />dealer visits</span>
                        </li>
                        <li>
                            <span></span>
                            <span>Complete<br />buying assistance</span>
                        </li>
                        <li>
                            <span></span>
                            <span>Easy<br />cancellation</span>
                        </li>
                    </ul>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/bookinglanding.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>


﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.ListNewBikeDealersByCity" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-dealersbylocation.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey">
    <form runat="server">
        <header>
            <div class="header-fixed fixed">
                <div class="leftfloat header-back-btn">
                    <a href="#"><span class="bwmsprite fa-arrow-back"></span></a>
                </div>
                <div class="leftfloat header-title text-bold text-white font18">Select city</div>
                <div class="clear"></div>
            </div>
        </header>

        <section class="container margin-top10 margin-bottom30">
            <div class="grid-12">
                <div id="listingWrapper" class="box-shadow padding-top15">
                    <div id="listingHeader" class="padding-right20 padding-left20">
                        <h1 class="font16 text-pure-black margin-bottom10">Bajaj dealers in Maharashtra</h1>
                        <h2 class="font14 text-unbold text-xt-light-grey text-truncate padding-bottom15 margin-bottom20 border-solid-bottom">25 dealers across 40 cities in Maharashtra</h2>
                        <div class="form-control-box">
                            <span class="bwmsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="getCityInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                        </div>
                        <div class="font16 text-black selected-state-label">Maharashtra</div>
                    </div>
                    <ul id="listingUL" class="city-listing">
                        <li>
                            <a href="#">Ahmednagar (1)</a>
                        </li>
                        <li>
                            <a href="#">Amravati (1)</a>
                        </li>
                        <li>
                            <a href="#">Jalgaon (1)</a>
                        </li>
                        <li>
                            <a href="#">Kalyan (1)</a>
                        </li>
                        <li>
                            <a href="#">Kolhapur (1)</a>
                        </li>
                        <li>
                            <a href="#">Mumbai (1)</a>
                        </li>
                        <li>
                            <a href="#">Nagpur (1)</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/dealersbylocation.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>

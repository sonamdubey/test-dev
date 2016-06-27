<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.ListNewBikeDealersByState" %>
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
                <div class="leftfloat header-title text-bold text-white font18">Select state</div>
                <div class="clear"></div>
            </div>
        </header>

        <section class="container margin-top10 margin-bottom30">
            <div class="grid-12">
                <div id="listingWrapper" class="box-shadow padding-top15">
                    <div id="listingHeader" class="padding-right20 padding-left20">
                        <h1 class="font16 text-pure-black margin-bottom10">Bajaj dealers in India</h1>
                        <h2 class="font14 text-unbold text-xt-light-grey text-truncate padding-bottom15 margin-bottom20 border-solid-bottom">25 dealers across 40 states</h2>
                        <div class="form-control-box">
                            <span class="bwmsprite search-icon-grey"></span>
                            <input type="text" class="form-control padding-right40" placeholder="Type to select state" id="getStateInput" />
                            <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                        </div>
                    </div>
                    <ul id="listingUL" class="state-listing">
                        <li>
                            <a href="#">Andhra Pradesh</a>
                        </li>
                        <li>
                            <a href="#">Bihar</a>
                        </li>
                        <li>
                            <a href="#">Chandigarh</a>
                        </li>
                        <li>
                            <a href="#">Delhi</a>
                        </li>
                        <li>
                            <a href="#">Goa</a>
                        </li>
                        <li>
                            <a href="#">Gujarat</a>
                        </li>
                        <li>
                            <a href="#">Haryana</a>
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

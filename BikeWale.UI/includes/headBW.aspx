<%@ Register Src="~/controls/LoginControlNew.ascx" TagPrefix="BW" TagName="Login" %>
<%@ Register Src="~/controls/LoginStatusNew.ascx" TagPrefix="BW" TagName="LoginStatus" %>
<html>
<head>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="keywords" content="new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price" />
<meta name="description" content="BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values." />
<meta name="alternate" content="http://www.bikewale.com/m/" />
<title>New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India</title>
<link rel="canonical" href="http://www.bikewale.com" />
<link rel="alternate" type="text/html" media="handheld" href="http://bikewale.com/m/" title="Mobile/PDA" />
<link rel="SHORTCUT ICON" href="#" />
<link href="/css/bw-common-style.css" rel="stylesheet" type="text/css">
<link href="/css/home.css" rel="stylesheet" type="text/css">
<script type="text/javascript" src="/src/frameworks.js"></script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/src/html5.js"></script>
<![endif]-->
</head>
<body class="bg-white">
	<div class="blackOut-window"></div>
    <!-- #include file="/includes/Navigation.aspx" -->
    <BW:Login ID="ctrlLogin" runat="server" />
    <div class="globalcity-popup rounded-corner2 hide" id="globalcity-popup"><!-- global city pop up code starts here -->
    	<div class="globalcity-popup-data text-center">
        	<div class="globalcity-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="cityPopup-box rounded-corner50 margin-bottom20">
            	<span class="bwsprite cityPopup-icon margin-top10"></span>
            </div>
            <p class="font20 margin-bottom15 text-capitalize">Please tell us your city</p>
            <p class="text-light-grey margin-bottom15">This allows us to provide relevant content for you.</p>
            <div class="form-control-box globalcity-input-box">
                <div class="margin-bottom20">
                	<span class="position-abt pos-right15 pos-top15 cwmsprite cross-sm-dark-grey cur-pointer"></span>
                    <input type="text" class="form-control padding-right30" name="globalCityPopUp" placeholder="Type to select city" id="globalCityPopUp">
                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none"></span>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">No city found. Try a different search.</div>
                </div>
            </div>
            <div>
            	<button class="btn btn-orange font18 text-uppercase">Confirm city</button>
            </div>
        </div>
        <div class="clear"></div>
    </div>
    <div id="header" class="header-fixed"> <!-- Fixed Header code starts here -->
        <div class="leftfloat">
            <span class="navbarBtn bwsprite nav-icon margin-right25"></span>
            <a href="/" class="bwsprite bw-logo"></a>
        </div>
        <div class="rightfloat">
            <div class="global-location">
                <div class="gl-default-stage">
                	<div id="globalCity-text">
                        <span class="cityName" id="cityName">Select City</span>
                        <span class="fa fa-map-marker margin-left10"></span>
                    </div>
                </div>            
            </div>
            <BW:LoginStatus ID="ctrlLoginStatus" runat="server" />
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div> <!-- ends here -->
    <div class="clear"></div>


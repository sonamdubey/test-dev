<%@ Page Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.New.Default" %>
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW"  %>
<%@ Register Src="~/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW"  %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="MMostPopularBikes" TagPrefix="BW"  %>

<!doctype html>
<html>
<head>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title>BikeWale - New Bikes</title>
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<meta name="keywords" content="" />
<meta name="description" content="BikeWale is India's most authoritative source of new bike pricing. Focused around bike buyers, BikeWale promises to help you buy the right bike at the right price." />
<link rel="canonical" href="http://www.bikewale.com" />
<link rel="alternate" type="text/html" media="handheld" href="http://bikewale.com/m/" title="Mobile/PDA" />
<link rel="SHORTCUT ICON" href="http://img2.aeplcdn.com/v2/icons/bikewale.png?v=1.1" />
<link href="../css/bwm-common-style.css" rel="stylesheet" type="text/css">
<link href="../css/bwm-newbikes.css" rel="stylesheet" type="text/css">
<!--<link href="../css/home.css" rel="stylesheet" type="text/css">-->
<script type="text/javascript" src="js/frameworks.js"></script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="../js/html5.js"></script>
<![endif]-->
</head>
<body class="bg-light-grey">
	<div class="blackOut-window"></div>
    <nav id="nav"> <!-- nav code starts here -->
        <ul class="navUL">
            <li>
                <a href="#">
                    <span class="bwmsprite home-icon"></span>
                    <span class="navbarTitle">Home</span>
                </a>
            </li>
            <li>
                <a href="javascript:void(0)">
                    <span class="bwmsprite newBikes-icon"></span>
                    <span class="navbarTitle">New Bikes</span>
                    <span class="nav-drop fa fa-angle-down"></span>
                </a>
                <ul class="nestedUL">
                    <li><a href="#">Find New Bikes</a></li>
                    <li><a href="#">Compare Bikes</a></li>
                    <li><a href="#">Check On-Road Price</a></li>
                    <li><a href="#">Locate Dealer</a></li>
                    <li><a href="#">Upcoming Bikes</a></li>
                    <li><a href="#">New Launches</a></li>
                    <li><a href="#">Videos</a></li>
                </ul>
            </li>
            <li>
                <a href="javascript:void(0)">
                    <span class="bwmsprite usedBikes-icon"></span>
                    <span class="navbarTitle">Used Bikes</span>
                    <span class="nav-drop fa fa-angle-down"></span>
                </a>
                <ul class="nestedUL">
                    <li><a href="#">Find Used Bikes</a></li>
                    <li><a href="#">All Used Bikes</a></li>
                </ul>
            </li>
            <li>
                <a href="#">
                    <span class="bwmsprite sellBikes-icon"></span>
                    <span class="navbarTitle">Sell Your Bike</span>
                </a>
            </li>
            <li>
                <a href="#">
                    <span class="bwmsprite sellBikes-icon"></span>
                    <span class="navbarTitle">Claim Your Offer</span>
                </a>
            </li>
            <li>
                <a href="javascript:void(0)">
                    <span class="bwmsprite reviews-icon"></span>
                    <span class="navbarTitle">Reviews and News</span>
                    <span class="nav-drop fa fa-angle-down"></span>
                </a>
                <ul class="nestedUL">
                    <li><a href="#">News</a></li>
                    <li><a href="#">Expert Reviews</a></li>
                    <li><a href="#">User Reviews</a></li>
                    <li><a href="#">Features</a></li>
                </ul>
            </li>
            <li>
                <a href="#">
                    <span class="bwmsprite forum-icon"></span>
                    <span class="navbarTitle">Tools</span>
                </a>
            </li>
            <li>
                <a href="#">
                    <span class="bwmsprite myBikeWale-icon"></span>
                    <span class="navbarTitle">My BikeWale</span>
                </a>
            </li>
        </ul>
	</nav>
    <!-- nav code ends here -->
    <div class="loginPopUpWrapper" id="loginPopUpWrapper"><!-- login code starts here -->
        <div class="loginBoxContent" id="Testlogin-box">
            <div class="loginCloseBtn position-abt pos-top10 pos-left10 infoBtn bwmsprite cross-md-dark-grey cur-pointer"></div>
            <div class="loginStage">
                <div class="loginWithCW margin-bottom40">
                    <h2 class="margin-bottom30">Log in to BikeWale</h2>
                    <div class="form-control-box margin-bottom20">
                        <input type="text" class="form-control border-red" name="email id" id="txtloginemail" placeholder="Email">
                        <span class="bwmsprite error-icon"></span>
                        <div class="bw-blackbg-tooltip">Please enter your name</div>
                    </div>
                    <div class="form-control-box margin-bottom20">
                        <input type="text" class="form-control" id="txtpasswordlogin" placeholder="Password">
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter your password</div>
                    </div>
                    <button id="btnLogin" class="btn btn-orange text-uppercase margin-bottom15 margin-right10">log in</button>
                    <button class="loginBtnSignUp btn btn-white btn-md text-uppercase margin-bottom15">sign up</button>
                    <div><a class="cur-pointer font12" id="forgotpass">Forgot password?</a></div>
                    <!-- Forget Password -->
                    <div id="forgotpassdiv">
                        <div class="hide margin-top20 margin-bottom30" id="forgotpassbox">
                            <div class="form-control-box margin-bottom20">
                                <input type="text" class="form-control" name="emailId" id="txtForgotPass" placeholder="Enter your registered email">
                                <span class="bwmsprite error-icon hide"></span>
                                <div class="bw-blackbg-tooltip hide">Please enter your registered email</div>
                            </div>
                            <button id="frgPwd" class="btn btn-orange text-uppercase">send</button>
                        </div>
                    </div>
                </div>
                <div class="loginWithFbGp">
                    <p class="font14 margin-bottom30">Or Login with</p>
                    <a href="" class="btn fbLoginBtn margin-bottom30">
                        <span class="fbIcon">
                            <span class="bwmsprite fb-login-icon"></span>
                        </span>
                        <span class="textWithFbGp">Sign in with facebook</span>
                    </a>
                    <a class="btn gplusLoginBtn">
                        <span class="gplusIcon">
                            <span class="bwmsprite gplus-login-icon"></span>
                        </span>
                        <span class="textWithFbGp">Sign in with Google</span>
                    </a>
                </div>
            </div>
            <div class="signUpStage">
                <div class="signUpWithCW">
                    <h2 class="margin-bottom30">Sign up to BikeWale</h2>
                    <div class="form-control-box margin-bottom20">
                        <input type="text" class="form-control" name="name" placeholder="Name" id="txtnamelogin">
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter your name</div>
                    </div>
                    <div class="form-control-box margin-bottom20">
                        <input type="text" class="form-control" name="emailId" id="txtemailsignup" placeholder="Email Id">
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter your email id</div>
                    </div>
                    <div class="form-control-box margin-bottom20">
                        <span class="form-mobile-prefix">+91</span>
                        <input type="text" class="form-control padding-left40" name="mobile" placeholder="Mobile no." id="txtmobilelogin">
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter your mobile number</div>
                    </div>
                    <div class="form-control-box margin-bottom20">
                        <input type="password" class="form-control" name="password" placeholder="Password" id="txtRegPasswd">
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter your password</div>
                    </div>
                    <div class="form-control-box margin-bottom20">
                        <input type="password" class="form-control" name="confirmPassword" placeholder="Confirm Password" id="txtConfPasswdlogin">
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter your password</div>
                    </div>                    
                    <div class="margin-bottom30 font12">
                        <input id="agreecheck" type="checkbox">
                        <label for="agreecheck">I have read and agree with the
                        <a href="javascript:void(0)">User Agreement</a> & <a href="javascript:void(0)">Privacy Policy</a></label>
                    </div>
                    <button class="signupBtnLogin btn btn-orange text-uppercase margin-bottom15 margin-right10">log in</button>
                    <button id="btnSignUp" class="btn btn-white btn-md text-uppercase margin-bottom15">sign up</button>
                </div>
            </div>            
        </div>
    </div> <!-- login ends here -->
    <div class="globalcity-popup rounded-corner2 hide" id="globalcity-popup">
    	<div class="globalcity-popup-data text-center">
        	<div class="globalcity-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
        	<div class="cityPopup-box rounded-corner50percent margin-bottom20">
            	<span class="bwmsprite cityPopup-icon margin-top10"></span>
            </div>
            <p class="font20 margin-bottom15 text-capitalize">Please tell us your city</p>
            <p class="text-light-grey margin-bottom15">This allows us to provide relevant content for you.</p>
            <div class="form-control-box">
                <div class="margin-bottom20 position-rel">
                	<span class="position-abt pos-right15 pos-top15 bwmsprite cross-sm-dark-grey cur-pointer"></span>
                    <input type="text" class="form-control padding-right30" name="globalCityPopUp" placeholder="Type to select city" id="globalCityPopUp">
                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:block"></span>
                </div>
            </div>
            <div>
            	<button class="btn btn-orange btn-full-width font18 text-uppercase">Confirm city</button>
            </div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="global-search-popup hide"> <!-- global-search-popup code starts here -->
    	<div class="form-control-box">
        	<span class="back-arrow-box" id="gs-close">
            	<span class="bwmsprite back-long-arrow-left"></span>
            </span>
            <span class="cross-box" id="gs-text-clear">
            	<span class="bwmsprite cross-md-dark-grey"></span>
            </span>            
        	<input type="text" id="globalSearchPopup" class="form-control" placeholder="Search">
        </div>
    </div> <!-- global-search-popup code ends here -->
    <header>
    	<div class="header-fixed"> <!-- Fixed Header code starts here -->
        	<a href="javascript:void(0)" class="bwmsprite bw-logo bw-lg-fixed-position"></a>
            <div class="leftfloat">
                <span class="navbarBtn bwmsprite nav-icon margin-right10"></span>                
            </div>
            <div class="rightfloat">
                <div class="global-location">
                    <span class="fa fa-map-marker"></span>
                </div>
            </div>
            <div class="clear"></div>
        </div> <!-- ends here -->
    	<div class="clear"></div>
        
        <section>
    	<div class="container">
        	<div class="newbikes-banner-div"><!-- Top banner code starts here -->
            	<h1 class="text-uppercase text-white text-center padding-top25 font24">NEW Bikes</h1>
                <p class=" font16 text-white text-center">View every bike under one roof</p>
            </div><!-- Top banner code ends here -->
        </div>
    </section>
        
    </header>
    
    <section class="container"><!-- Brand section code starts here -->
    	<div class="grid-12">
        	<div class="bg-white brand-wrapper content-box-shadow margin-minus30">
            	<h2 class="content-inner-block-10 text-uppercase text-center margin-top20 margin-bottom20">Brand</h2>
                <div class="brand-type-container">
                    <ul class="text-center">
                        <li>
                            <a href="javascript:void(0)">
                                <span class="one brandlogosprite brand-aprilia"></span>
                                <span class="brand-type-title">aprilia</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-honda"></span>
                                <span class="brand-type-title">honda</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-royal"></span>
                                <span class="brand-type-title">Royal Enfield</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-bajaj"></span>
                                <span class="brand-type-title">bajaj</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-hyosung"></span>
                                <span class="brand-type-title">hyosung</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-suzuki"></span>
                                <span class="brand-type-title">suzuki</span>
                            </a>
                        </li>
                    </ul>
                    <ul id="more-brand-nav" class="text-center hide">
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-benelli"></span>
                                <span class="brand-type-title">benelli</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-indian"></span>
                                <span class="brand-type-title">indian</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-triumph"></span>
                                <span class="brand-type-title">triumph</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-bmw"></span>
                                <span class="brand-type-title">bmw</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-kawasaki"></span>
                                <span class="brand-type-title">kawasaki</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-tvs"></span>
                                <span class="brand-type-title">tvs</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-ducati"></span>
                                <span class="brand-type-title">ducati</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-ktm"></span>
                                <span class="brand-type-title">ktm</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-vespa"></span>
                                <span class="brand-type-title">vespa</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-harley"></span>
                                <span class="brand-type-title">harley</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-lml"></span>
                                <span class="brand-type-title">lml</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-yamaha"></span>
                                <span class="brand-type-title">yamaha</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-hero"></span>
                                <span class="brand-type-title">hero</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-mahindra"></span>
                                <span class="brand-type-title">mahindra</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-yo"></span>
                                <span class="brand-type-title">yo</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-hero-elec"></span>
                                <span class="brand-type-title">hero-elec</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="brandlogosprite brand-guzzi"></span>
                                <span class="brand-type-title">guzzi</span>
                            </a>
                        </li>
                        
                	</ul>
                </div>
                <div class="text-center padding-bottom20">
                    <a href="javascript:void(0)" id="more-brand-tab" class="view-more-btn font16">View <span>more</span> Brands</a>
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </section>
    
    <section><!--  Upcoming, New Launches and Top Selling code starts here -->
        <div class="container">
            <div class="grid-12">
                <h2 class="text-center margin-top40 margin-bottom30">Discover your bike</h2>
                <div class="bw-tabs-panel">
                    <div class="bw-tabs margin-bottom15">
                        <div class="form-control-box">
                            <select class="form-control">
                                <option value="mctrlMostPopularBikes">Most Popular</option>
                                <option value="mctrlNewLaunchedBikes">New Launches</option>
                                <option value="mctrlUpcomingBikes">Upcoming</option>
                            </select>
                        </div>
                    </div>
                    
                    <BW:MUpcomingBikes runat="server" ID="mctrlUpcomingBikes"/> <!-- Upcoming Bikes Control-->

                    <BW:MNewLaunchedBikes runat="server" ID="mctrlNewLaunchedBikes"/> 

                    <BW:MMostPopularBikes runat="server" ID="mctrlMostPopularBikes"/> 

                </div>        
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
    <section><!--  Compare section code starts here -->
        <div class="container">
        	<div class="grid-12">
                <h2 class="text-center margin-top40 margin-bottom30">Compare now</h2>
                <div class="compare-bikes-container content-box-shadow">
                    <div class="bike-preview margin-bottom10">
                        <img src="http://imgd1.aeplcdn.com//310x174//bw/bikecomparison/kawasaki_ninja300_vs_yamaha_yzf-r3.jpg?20151708125625" title="IMG title" alt="IMG title">
                    </div>
                    <h3 class="font16 text-center padding-top10">
                        <a href="javascript:void(0)" class="text-grey">Suzuki Gixxer SF Vs Pulsar AS 200</a>
                    </h3>
                    <div class="font16 text-center padding-top15 padding-bottom15">
                        <a href="javascript:void(0)">View more Comparisons</a>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
	<section class="container"><!-- Tools you may need code starts here -->
    	<div class="grid-12">
        	<h2 class="text-center margin-top40 margin-bottom30">Tool you may need</h2>
            <div class="tools-need-container margin-bottom30 text-center">
                <ul>
                	<li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20">
                    	<a href="javascript:void(0)">
                        	<span class="tools-need-logo">
                            	<span class="bwm-circle-icon getfinalprice-icon"></span>
                            </span>
                            <span class="tools-need-desc text-left">
                            	<span class="font18 text-bold">Get final price</span>
                                <br>
                                <span class="font14 tools-need-text">Get final price without filling any form</span>
                            </span>
                        </a>
                    </li>
                    <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20">
                    	<a href="javascript:void(0)">
                        	<span class="tools-need-logo">
                            	<span class="bwm-circle-icon locatedealer-icon"></span>
                            </span>
                            <span class="tools-need-desc text-left">
                            	<span class="font18 text-bold">Locate dealer</span>
                                <br>
                                <span class="font14 tools-need-text">Find a dealer near your current location</span>
                            </span>
                        </a>
                    </li>
                    <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20">
                    	<a href="javascript:void(0)">
                        	<span class="tools-need-logo">
                            	<span class="bwm-circle-icon checkcarvalue-icon"></span>
                            </span>
                            <span class="tools-need-desc text-left">
                            	<span class="font18 text-bold">Calculate EMI's</span>
                                <br>
                                <span class="font14 tools-need-text">Instant calculate loan EMI</span>
                            </span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="clear"></div>
    </section>
    
    <section><!--  News, reviews and videos code starts here -->
        <div class="container">
        	<div class="grid-12">
                <h2 class="text-center margin-top20 margin-bottom30">Latest Updates</h2>
                <div class="bw-tabs-panel">
                    <div class="bw-tabs margin-bottom15">
                    	<div class="form-control-box">
                        	
                            <select class="form-control">
                                <option class="active" data-tabs="News">News</option>
                                <option data-tabs="Reviews">Reviews</option>
                                <option data-tabs="Videos">Videos</option>
                            </select>
                        </div>
                    </div>
                    <div class="bw-tabs-data" id="News">
                        <div class="jcarousel-wrapper">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="imageWrapper">
                                                    <a href="javascript:void(0)">
                                                    	<img alt="Valentino Rossi" title="Valentino Rossi" src="http://imgd1.aeplcdn.com//566x318//bw/ec/19895/Harley-Davidson-India-56380.jpg?wm=2">
                                                    </a>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood Festival of Speed for the first time</h3>
                                                    </div>
                                                    <div class="margin-bottom10 text-light-grey">2 hours ago</div>                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="imageWrapper">
                                                    <a href="javascript:void(0)">
                                                    	<img alt="Valentino Rossi" title="Valentino Rossi" src="http://imgd1.aeplcdn.com//566x318//bw/ec/19895/Harley-Davidson-India-56380.jpg?wm=2">
                                                    </a>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood Festival of Speed for the first time</h3>
                                                    </div>
                                                    <div class="margin-bottom10 text-light-grey">2 hours ago</div>                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="imageWrapper">
                                                    <a href="javascript:void(0)">
                                                    	<img alt="Valentino Rossi" title="Valentino Rossi" src="http://imgd1.aeplcdn.com//566x318//bw/ec/19895/Harley-Davidson-India-56380.jpg?wm=2">
                                                    </a>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood Festival of Speed for the first time</h3>
                                                    </div>
                                                    <div class="margin-bottom10 text-light-grey">2 hours ago</div>                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination margin-bottom30"></p>
                        </div>
                        <div class="text-center margin-bottom40">
                            <a class="font16" href="javascript:void(0)">View More News</a>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="Reviews">
                        <div class="jcarousel-wrapper">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="imageWrapper">
                                                    <a href="javascript:void(0)">
                                                    	<img alt="Acura NSX" title="Acura NSX" src="http://imgd1.aeplcdn.com//566x318//bw/ec/19874/Harley-Davidson-Road-King-First-Look-Review-56330.jpg?wm=0">
                                                    </a>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood Festival of Speed for the first time</h3>
                                                    </div>
                                                    <div class="margin-bottom10 text-light-grey">2 hours ago</div>                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="imageWrapper">
                                                    <a href="javascript:void(0)">
                                                    	<img alt="Acura NSX" title="Acura NSX" src="http://imgd1.aeplcdn.com//566x318//bw/ec/19874/Harley-Davidson-Road-King-First-Look-Review-56330.jpg?wm=0">
                                                    </a>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood Festival of Speed for the first time</h3>
                                                    </div>
                                                    <div class="margin-bottom10 text-light-grey">2 hours ago</div>                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="imageWrapper">
                                                    <a href="javascript:void(0)">
                                                    	<img alt="Acura NSX" title="Acura NSX" src="http://imgd1.aeplcdn.com//566x318//bw/ec/19874/Harley-Davidson-Road-King-First-Look-Review-56330.jpg?wm=0">
                                                    </a>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood Festival of Speed for the first time</h3>
                                                    </div>
                                                    <div class="margin-bottom10 text-light-grey">2 hours ago</div>                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination margin-bottom30"></p>
                        </div>
                        <div class="text-center margin-bottom40">
                            <a class="font16" href="javascript:void(0)">View more reviews</a>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="Videos">
                        <div class="jcarousel-wrapper">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="yt-iframe-preview">
                                                    <iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood </h3>
                                                    </div>
                                                    <div class="margin-bottom15 text-light-grey">
                                                    	<span class="bwmsprite review-sm-lgt-grey"></span> Views <span>398</span>
                                                    </div>
                                                    <div class="text-light-grey">
                                                    	<span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span>120</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="yt-iframe-preview">
                                                    <iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood</h3>
                                                    </div>
                                                    <div class="margin-bottom15 text-light-grey">
                                                    	<span class="bwmsprite review-sm-lgt-grey"></span> Views <span>398</span>
                                                    </div>
                                                    <div class="text-light-grey">
                                                    	<span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span>120</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="front">
                                            <div class="contentWrapper">
                                            	<div class="yt-iframe-preview">
                                                    <iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom20">
                                                        <h3>Valentino Rossi participates at Goodwood</h3>
                                                    </div>
                                                    <div class="margin-bottom15 text-light-grey">
                                                    	<span class="bwmsprite review-sm-lgt-grey"></span> Views <span>398</span>
                                                    </div>
                                                    <div class="text-light-grey">
                                                    	<span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span>120</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination margin-bottom30"></p>
                        </div>
                        <div class="text-center margin-bottom40 clear">
                            <a class="font16" href="javascript:void(0)">View more videos</a>
                        </div>
                    </div>
                </div>        
        	</div>
            <div class="clear"></div>
        </div>
    </section>
    
    <footer class="bg-footer padding-top30 padding-bottom15"><!-- Footer section code starts here -->
        <div class="container">
        	<div class="grid-12 text-center">
                <div>
                    <a href="javascript:void(0)" class="bwmsprite bw-logo"></a>              
                </div>
                <div class="margin-top15">
                    <ul>
                        <li><a href="/aboutus.aspx">About Us</a></li>
                        <li><a href="/forums/">Forums</a></li>
                        <li><a href="/sitemap.aspx">Sitemap</a></li>
                    </ul>
                </div>
                <div class="margin-top15 margin-bottom15">
                	<a href="javascript:void(0)" target="_blank" class="text-white">View Desktop Version</a>
                </div>
                
            </div>
            <div class="clear"></div>
        </div>
        <div class="border-solid-top text-white font11 margin-top5 padding-top10 grid-12">
                    <div class="grid-4 alpha text-left opacity50">&copy; BikeWale India</div>
                    <div class="grid-8 omega text-right">
                        <a href="http://www.bikewale.com/visitoragreement.aspx" class="text-white">Visitor Agreement </a>&
                        <a href="http://www.bikewale.com/privacypolicy.aspx" class="text-white">Privacy Policy</a>
                    </div>
                </div>
            	<div class="clear"></div>
    </footer> <!-- Ends here -->
   	<!-- all other js plugins -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script type="text/javascript" src="../src/Plugins.js"></script>
    <script type="text/javascript" src="../src/common.js"></script>
	<script type="text/javascript" src="../src/bwm-newbikes.js"></script>
</body>
</html>

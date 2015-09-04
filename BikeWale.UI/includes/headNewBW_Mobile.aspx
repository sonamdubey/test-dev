
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
<link href="/m/css/bwm-common-style.css" rel="stylesheet" type="text/css">
<link href="/m/css/bwm-newbikes.css" rel="stylesheet" type="text/css">
<!--<link href="/m/css/home.css" rel="stylesheet" type="text/css">-->
<script type="text/javascript" src="/m/src/frameworks.js"></script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/m/src/js/html5.js"></script>
<![endif]-->
</head>
<body class="bg-light-grey">
	<div class="blackOut-window"></div>
    <!-- #include file="/includes/Navigation_Mobile.aspx" -->
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
        
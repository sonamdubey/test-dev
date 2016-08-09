<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Login_New.aspx.cs" Inherits="Bikewale.Login_New" %>

<!DOCTYPE html>
<html>
<head>
<script language="c#" runat="server">
    private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private string title = "", description = "", keywords = "";
</script>

<meta charset="utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<%
    title = "User Login - BikeWale";
    description = "bikewale.com user login";
    keywords = "users, login, register, forgot password";
%>
<title><%= title %></title>

<link rel="SHORTCUT ICON" href="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>
<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
<link href="/css/bw-common-style.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
<link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/login.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/src/html5.js"></script>
<![endif]-->

</head>
<body>
    <form id="form1" runat="server">

        <div class="blackOut-window"></div>
        <!-- #include file="/includes/Navigation.aspx" -->
        
        <div id="header" class="header-fixed"> <!-- Header code starts here -->
            <div class="leftfloat">
                <span class="navbarBtn bwsprite nav-icon margin-right25"></span>
                <a href="/" id="bwheader-logo" class="bwsprite bw-logo" title="Bikewale"></a>
           
            </div>
            <div class="clear"></div>
        </div> <!-- ends here -->
        
        <section id="loginSignupWrapper" class="rounded-corner2 text-center">
	        <div class="login-signup-wrapper position-rel">
    	        <div class="user-login-box">
                    <h2 class="text-default text-bold margin-bottom45 font18 text-uppercase">Login to BikeWale</h2>
                    <div class="margin-bottom45">
                        <div class="form-control-box margin-bottom20">
                            <input name="ctrlLogin$txtLoginEmail" type="email" id="ctrlLogin_txtLoginEmail" class="form-control" placeholder="Email">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom17">
                            <input name="ctrlLogin$txtLoginPassword" type="password" id="ctrlLogin_txtLoginPassword" class="form-control" placeholder="Password">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="text-right margin-bottom17">
                            <a href="javascript:void(0)" class="font14 forgot-password-target">Forgot password?</a>
                        </div>
                        <input type="submit" name="ctrlLogin$btnLogin" value="Log in" id="ctrlLogin_btnLogin" class="btn btn-orange btn-full-width">
                    </div>
                    <div class="horizontal-divider position-rel">
                        <span class="text-uppercase text-light-grey position-abt font14">Or</span>
                    </div>
                </div>
        
                <div class="forgot-password-content">
                    <h2 class="text-default text-bold margin-bottom45 font18 text-uppercase">Forgot PassWord</h2>
                    <p class="text-light-grey margin-bottom30">We will send a recovery link on your registered email</p>
                    <div class="form-control-box margin-bottom20">
	                    <input type="email" class="form-control" name="emailId" id="txtForgotPassEmail" placeholder="Enter your registered email">
                        <span class="bwsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide"></div>
                    </div>
                    <input type="button" id="btnForgetPass" class="btn btn-orange btn-full-width margin-bottom15" value="Send">
                    <a href="javascript:void(0)" class="font14 forgot-password-back-btn">back to login</a>
                </div>
        
                <div class="user-signup-box">
                    <h2 class="text-default text-bold margin-bottom20 font18 text-uppercase login-box-signup-target">Sign up for BikeWale</h2>
                    <div>
                        <div class="form-control-box margin-bottom20">
                            <input name="ctrlLogin$txtNameSignup" type="text" id="ctrlLogin_txtNameSignup" class="form-control" placeholder="Name">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom20">
                        <input name="ctrlLogin$txtEmailSignup" type="email" id="ctrlLogin_txtEmailSignup" class="form-control" placeholder="Email Id">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom20">
	                        <span class="form-mobile-prefix pos-left0">+91</span>
                            <input name="ctrlLogin$txtMobileSignup" type="text" id="ctrlLogin_txtMobileSignup" class="form-control padding-left40" placeholder="Mobile no.">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom17">
                        <input name="ctrlLogin$txtRegPasswdSignup" type="password" id="ctrlLogin_txtRegPasswdSignup" class="form-control" placeholder="Password">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="margin-bottom17 font13 text-left">
                                <input type="checkbox" checked="checked" name="agreecheck">
                                <label for="agreecheck">I agree with 
                                <a href="/visitoragreement.aspx" target="_blank">User Agreement</a> &amp; <a href="/privacypolicy.aspx" target="_blank">Privacy Policy</a></label>
                            </div>
                        <input type="submit" name="ctrlLogin$btnSignup" value="Sign up" id="ctrlLogin_btnSignup" class="btn btn-orange btn-full-width margin-bottom10">
                        <a href="javascript:void(0)" class="font14 signup-box-back-btn">back to login</a>
                    </div>
                </div>

                <div class="login-box-footer"></div>
            </div>
        </section>

        <footer id="loginFooter" class="footer-fixed">
	        <div class="container">
                <div class="grid-6">
        	        &copy; BikeWale India
                </div>
                <div class="grid-6 text-right">
	                <a href="/visitoragreement.aspx" class="text-white">Visitor Agreement</a>
                    &amp;
                    <a href="/privacypolicy.aspx" class="text-white">Privacy Policy</a>
                </div>
                <div class="clear"></div>
            </div>
        </footer>
        
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/Plugins.js?<%= staticFileVersion %>"></script>
        <%--<script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/login/login.js?<%= staticFileVersion %>"></script>--%>

        <script type="text/javascript">
	
	        var timer;
	        $('.login-box-signup-target').on('click', function() {
		        clearTimeout(timer);
		        $('.login-signup-wrapper').addClass('activate-signup');
		        $('.user-signup-box form').show();
		        timer = setTimeout(function () {
				        $('.login-box-footer').hide();
			        }, 1000);
	        });
	
	        $('.signup-box-back-btn').on('click', function() {
		        clearTimeout(timer);
		        $('.login-signup-wrapper').removeClass('activate-signup');
		        $('.login-box-footer').show();
		        timer = setTimeout(function () {
				        $('.user-signup-box form').hide();
			        }, 1000);
	        });
	
	        $('.forgot-password-target').on('click', function() {
		        $('.user-login-box, .user-signup-box').hide();
		        $('.forgot-password-content').show();
	        });
	
	        $('.forgot-password-back-btn').on('click', function() {
		        $('.forgot-password-content').hide();
		        $('.user-login-box, .user-signup-box').show();
	        });
            	        
	        $(document).ready(function () {
	            if ($(window).innerHeight() < 550) { // for devices with height around 540px
	                $('#loginSignupWrapper').css({ 'height': '420px', 'padding': '20px 70px', 'top': '12%' });
	            }
	            setBackgroundImage();
	        });

	        $(window).resize(function () {
	            setBackgroundImage();
	        });

	        var setBackgroundImage = function () {
	            if ($(window).innerWidth() > 768)
	                $('body').css({ 'background': '#6e6d71 url(http://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/login-background-banner.jpg) no-repeat center center' });
	            else
	                $('body').css({'background': 'none'});
	        }

            // nav bar code starts here
	        $(".navbarBtn").on('click', function () {
	            navbarShow();
	        });

	        $(".blackOut-window").mouseup(function (e) {
	            navbarHide();
	        });

	        $(".navUL > li > a").on('click', function () {
	            if (!$(this).hasClass("open")) {
	                var a = $(".navUL li a");
	                a.removeClass("open").next("ul").slideUp(350);
	                $(this).addClass("open").next("ul").slideDown(350);

	                if ($(this).siblings().size() === 0) {
	                    navbarHide();
	                }

	                $(".nestedUL > li > a").click(function () {
	                    $(".nestedUL li a").removeClass("open");
	                    $(this).addClass("open");
	                    navbarHide();
	                });

	            }
	            else if ($(this).hasClass("open")) {
	                $(this).removeClass("open").next("ul").slideUp(350);
	            }
	        });

	        $(document).keydown(function (e) {
	            if (e.keyCode == 27) {
	                navbarHide();
	            }
	        });

	        function navbarShow() {
	            $("#nav").addClass('open').animate({ 'left': '0px' });
	            $(".blackOut-window").show();
	        }

	        function navbarHide() {
	            $("#nav").removeClass('open').animate({ 'left': '-350px' });
	            $(".blackOut-window").hide();
	        }
            // nav bar code ends here

        </script>

    </form>
</body>
</html>
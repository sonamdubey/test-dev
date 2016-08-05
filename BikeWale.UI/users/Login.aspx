<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikWale.Users.Login" Trace="false" %>
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
                           
                            <asp:TextBox ID="txtLoginid" runat="server" class="form-control" placeholder="Email" />
                            <span class="bwsprite error-icon hide"></span>
                            
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom17">
                           
                            <asp:TextBox ID="txtPasswd" TextMode="password" runat="server" class="form-control" placeholder="Password" />
                            <span class="bwsprite error-icon hide"></span>
                         
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="text-right margin-bottom17">
                            <a href="javascript:void(0)" class="font14 forgot-password-target">Forgot password?</a>
                        </div>
                        <asp:Button name="butLogin" id="btnLogin" Text="Log in" class="btn btn-orange btn-full-width" OnClientClick="return pressLoginButton(e)" runat="server" />
                    </div>
                    <div class="horizontal-divider position-rel">
                        <span class="text-uppercase text-light-grey position-abt font14">Or</span>
                    </div>
                </div>
        
                <div class="forgot-password-content">
                    <h2 class="text-default text-bold margin-bottom45 font18 text-uppercase">Forgot PassWord</h2>
                    <p class="text-light-grey margin-bottom30">We will send a recovery link on your registered email</p>
                    <div class="form-control-box margin-bottom20">
	                    <asp:TextBox runat="server" class="form-control" name="emailId" id="txtForgotPassEmail" placeholder="Enter your registered email" />
                        <span class="bwsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide"></div>
                    </div>
                    <asp:Button runat="server" id="btnForgetPass" Text="Send" class="btn btn-orange btn-full-width margin-bottom15" />
                    <a href="javascript:void(0)" class="font14 forgot-password-back-btn">back to login</a>
                </div>
        
                <div class="user-signup-box">
                    <h2 class="text-default text-bold margin-bottom20 font18 text-uppercase login-box-signup-target">Sign up for BikeWale</h2>
                    <div>
                        <div class="form-control-box margin-bottom20">
                            <asp:TextBox runat="server" name="txtNameSignup" id="txtNameSignup" class="form-control" placeholder="Name" />
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom20">
                        <asp:Textbox runat="server" name="txtEmailSignup" id="txtEmailSignup" class="form-control" placeholder="Email Id" />
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom20">
	                        <span class="form-mobile-prefix pos-left0">+91</span>
                            <asp:TextBox runat="server" name="txtMobileSignup" id="txtMobileSignup" class="form-control padding-left40" placeholder="Mobile no." />
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="form-control-box margin-bottom17">
                        <asp:TextBox runat="server" name="txtRegPasswdSignup" TextMode="password" id="txtRegPasswdSignup" class="form-control" placeholder="Password" />
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide"></div>
                        </div>
                        <div class="margin-bottom17 font13 text-left">
                                <input type="checkbox" id="chkAgreeSignup" onClick="checkStatus(this)" name="chkPrivacy" checked value="checkbox">
                                <label for="agreecheck">I agree with 
                                <a href="/visitoragreement.aspx" target="_blank">User Agreement</a> &amp; <a href="/privacypolicy.aspx" target="_blank">Privacy Policy</a></label>
                            </div>
                        <asp:Button name="btnSignup" OnClientClick="pressSignupbutton(e)" Text="Sign up"  id="btnSignup" class="btn btn-orange btn-full-width margin-bottom10" runat="server"/>
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
        <!--<script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/login/login.js?<%= staticFileVersion %>"></script>-->

        <script type="text/javascript">

            var timer;
            $('.login-box-signup-target').on('click', function () {
                clearTimeout(timer);
                $('.login-signup-wrapper').addClass('activate-signup');
                $('.user-signup-box form').show();
                timer = setTimeout(function () {
                    $('.login-box-footer').hide();
                }, 1000);
            });

            $('.signup-box-back-btn').on('click', function () {
                clearTimeout(timer);
                $('.login-signup-wrapper').removeClass('activate-signup');
                $('.login-box-footer').show();
                timer = setTimeout(function () {
                    $('.user-signup-box form').hide();
                }, 1000);
            });

            $('.forgot-password-target').on('click', function () {
                $('.user-login-box, .user-signup-box').hide();
                $('.forgot-password-content').show();
            });

            $('.forgot-password-back-btn').on('click', function () {
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
                    $('body').css({ 'background': 'none' });
            }

            
            //client side validation for login
            var email = $("#<%=txtLoginid.ClientID.ToString() %>");
            var pass = $("#<%=txtPasswd.ClientID.ToString() %>");
            var loginBtn = $("#btnLogin");



            $("#<%=txtLoginid.ClientID.ToString() %>,#<%=txtPasswd.ClientID.ToString() %>").keypress(function (e) {
                try {	//for firefox           
                    if (e.which || e.keyCode) {
                        if ((e.which == 13) || (e.keyCode == 13)) {
                            return true;
                        }
                    }
                    else { return false };
                }
                catch (exception) {
                    //for ie
                    if (event.keyCode) {
                        if (event.keyCode == 13) {
                            return false;
                        }
                    }
                    else { return true };
                }
            });


            function pressLoginButton(e) {
                var isValid = false;
                emailVal = email.val().trim();
                passVal = pass.val().trim();
                if (emailVal.length > 0 && validateEmail(emailVal)) {
                    if (passVal.length > 0) {
                        isValid = true;
                    }
                    else {
                        setError(pass, 'Password should not be empty');
                        isValid = false;
                    }
                }
                else {
                    setError(email, 'Enter valid email id');
                    isValid = false;
                }
                return isValid;
            }

            function validateEmail(emailVal) {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return re.test(emailVal);
            }


            email.on('focus', function () {

                hideError($(this));

            });

            pass.on('focus', function () {

                hideError($(this));

            });

            function setError(ele, msg) {
                ele.addClass("border-red");
                ele.siblings("span, div").show();
                ele.siblings("div").text(msg);
            }

            function hideError(ele) {
                if (ele != null) {
                    ele.removeClass("border-red");
                    ele.siblings("span, div").hide();
                }
            }
            //validation ends here
            
            //client side validation for register
            var nameVal = $("#<%=txtNameSignup.ClientID.ToString() %>");
            var emailVal = $("#<%=txtEmailSignup.ClientID.ToString() %>");
            var mobileVal = $("#<%=txtMobileSignup.ClientID.ToString() %>");
            var passVal = $("#<%=txtRegPasswdSignup.ClientID.ToString() %>");


            function pressSignupbutton(e) {
                if (validateControl()) {
                    return false;
                }
                else {
                    return true;
                }
            };

           

            function validateControl() {

                console.log("here");
                var nameSignup = nameVal.val();
                var emailSignup = emailVal.val();
                var mobileSignup = mobileVal.val();
                var passSignup = passVal.val();
                var isError = false;
                var reName = /^[a-zA-Z0-9'\- ]+$/;
                var re = /^[0-9]*$/
                var regPass = /^[a-zA-Z]+$/;

                var emailValid = false, nameValid = false, mobileValid = false, pwdValid = false;

                var isValid = false;

                if ($.trim(nameSignup) == "") {
                    setError(nameVal, 'Required');
                    nameValid = false;
                } else if (!reName.test($.trim(nameSignup))) {
                    setError(nameVal, 'Name should be Alphanumeric.');
                    nameValid = false;
                }
                else {
                    nameValid = true;
                }



                if (emailSignup.length > 0 && validateEmail(emailSignup)) {
                    emailValid = true;
                }
                else {
                    setError(emailVal, 'Enter valid email id');
                    emailValid = false;
                }


                if ($.trim(mobileSignup) != "") {
                    if (!re.test($.trim(mobileSignup).toLowerCase())) {
                        setError(mobileVal, 'Mobile No. should be numeric only');
                        mobileValid = false;
                    } else if (mobileSignup.length < 10) {
                        if (!re.test($.trim(mobileSignup).toLowerCase())) {
                            setError(mobileVal, 'Mobile no should be greater than 10 digits');
                            mobileValid = false;
                        } else {
                            mobileValid = true;
                        }
                    }
                    else {
                        setError(mobileVal, 'Please enter a mobile no.');
                        mobileValid = false;
                    }


                    if (passSignup.length > 5) {
                        pwdValid = true;
                    }
                    else {
                        setError(passVal, 'Password should contain atleast 6 characters');
                        pwdValid = false;
                    }

                    isValid = nameValid & emailValid & mobileValid & pwdValid;
                    isError = !isvalid;

                    return isError;
                }
            }

            function checkStatus(chk) {
                getCtrlId('btnSignup').disabled = chk.checked ? false : true;
            }
            
            emailVal.on('focus', function () {

                hideError($(this));

            });

            passVal.on('focus', function () {

                hideError($(this));

            });

            nameVal.on('focus', function () {

                hideError($(this));

            });

            mobileVal.on('focus', function () {

                hideError($(this));

            });
            
            function getCtrlId(controlId) {
                return document.getElementById('<%=this.ID%>_' + controlId);
            }

            //


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

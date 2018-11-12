<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikWale.Mobile.Users.Login" Trace="false" %>
<!DOCTYPE html>
<html>
<head>

<%
    title = "User Login - BikeWale";
    description = "bikewale.com user login";
    keywords = "users, login, register, forgot password";
%>
<!-- #include file="/UI/includes/headscript_mobile.aspx" -->
<link href="<%= staticUrl  %>/UI/css/login.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="blackOut-window bwm-blackOut-window"></div>       
        <!-- #include file="/UI/includes/Navigation_Mobile.aspx" -->
        
        <div id="header" class="header-fixed"> <!-- Header code starts here -->
            <div class="leftfloat">
                <span id="navbarBtn" class="navbarBtn bwm-navbarBtn nav-icon margin-right25"></span>
                <a href="/" id="bwheader-logo" class="bw-logo" title="BikeWale">BikeWale</a>
           
            </div>
            <div class="clear"></div>
        </div> <!-- ends here -->
        
        <section id="loginSignupWrapper" class="rounded-corner2 text-center">
	        <div class="login-signup-wrapper position-rel">
    	        <div class="user-login-box">
                    <span id="errEmail" class="text-red margin-bottom15" runat="server"></span>
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
                        <asp:HiddenField ID="hdnAuthData" runat="server" />
                        <asp:Button name="butLogin" id="btnLogin" Text="Log in" class="btn btn-orange btn-full-width" runat="server" />
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
                        <p id="processing_pwd_fp" class="hide"> Please wait...</p>
                    </div>
                    <input type="button" id="btnForgetPass" Value="Send" class="btn btn-orange btn-full-width margin-bottom15"  />                    
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
	                        <p class="form-mobile-prefix position-abt pos-left10">+91</p>
                            <asp:TextBox runat="server" name="txtMobileSignup" id="txtMobileSignup" class="form-control padding-left40" MaxLength="10" placeholder="Mobile no." />
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
                                <a href="/visitoragreement.aspx" target="_blank" rel="noopener">User Agreement</a> &amp; <a href="/privacypolicy.aspx" target="_blank" rel="noopener">Privacy Policy</a></label>
                            </div>
                        <asp:Button name="btnSignup" Text="Sign up"  id="btnSignup" class="btn btn-orange btn-full-width margin-bottom10" OnClientClick="return pressSignupbutton(e)"  runat="server"/>                       
                        <a href="javascript:void(0)" class="font14 signup-box-back-btn">back to login</a>
                    </div>
                </div>

                <div class="login-box-footer"></div>
            </div>
        </section>
        <!-- #include file="/UI/includes/footerloginBW.aspx" -->       
        <script type="text/javascript">
            //client side validation for login
            var email = $("#<%=txtLoginid.ClientID.ToString() %>");
            var pass = $("#<%=txtPasswd.ClientID.ToString() %>");
            var ctrlHdnAuthDataId = '<%= hdnAuthData.ClientID.ToString() %>';
            var loginBtn = $("#btnLogin");

            //client side validation for register
            var nameVal = $("#<%=txtNameSignup.ClientID.ToString() %>");
            var emailVal = $("#<%=txtEmailSignup.ClientID.ToString() %>");
            var mobileVal = $("#<%=txtMobileSignup.ClientID.ToString() %>");
            var passVal = $("#<%=txtRegPasswdSignup.ClientID.ToString() %>");
            var forgotPass = $("#txtForgotPassEmail");

            $(window).resize(function () {
                setBackgroundImage();
            });
        </script>
        <script type="text/javascript" src="<%= staticUrl  %>/UI/src/login/login.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>

<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LoginControlNew" %>
<% if(Bikewale.Common.CurrentUser.Id == "-1") { %>
<div class="loginPopUpWrapper" id="loginPopUpWrapper" runat="server"><!-- login code starts here -->
    <div class="loginBoxContent" id="Testlogin-box">
        <div class="loginCloseBtn position-abt pos-top10 pos-left10 infoBtn bwsprite cross-md-dark-grey cur-pointer"></div>
        <div class="loginStage" id="divLogin" runat="server">
            <div class="loginWithCW margin-bottom40">
                <p id="errorRegister" class="text-red margin-bottom10" runat="server">Already Registered. Please Login</p>
                <h2 class="margin-bottom30">Log in to BikeWale</h2>
                <div class="form-control-box margin-bottom20">
                    <asp:TextBox id="txtLoginEmail" runat="server" class="form-control" name="email id" placeholder="Email" />
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Invalid Email</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <asp:TextBox id="txtLoginPassword" runat="server" class="form-control" placeholder="Password" TextMode="Password" />
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please Enter correct Password</div>
                </div>
                <div class="margin-bottom20 font12">
                    <asp:CheckBox id="chkRemMe" runat="server"/>
                    <label for="<%= chkRemMe.ClientID %>">Remember me</label>
                </div>
                <asp:HiddenField ID="hdnAuthData" runat="server" />                
                <asp:Button Text="log in" id="btnLogin" runat="server" class="btn btn-orange text-uppercase margin-bottom15 margin-right10" />
                <button type="button" class="loginBtnSignUp btn btn-white btn-md text-uppercase margin-bottom15">sign up</button>
                <div><a class="cur-pointer font12" id="forgotpass">Forgot password?</a></div>
                <!-- Forget Password -->
                <div id="forgotpassdiv" class="hide">
                    <div class="margin-top20 margin-bottom30" id="forgotpassbox">
                        <div class="form-control-box margin-bottom20">
                            <input type="text" class="form-control" name="emailId" id="txtForgotPassEmail" placeholder="Enter your registered email">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide">Invalid Email</div>
                        </div>
                        <input type="button" id="btnForgetPass" class="btn btn-orange text-uppercase" value="send">
                    </div>
                    <b><span id="processing_pwd" class="hide"> Please wait...</span></b>
                </div>
            </div>
            <%--<div class="loginWithFbGp">
                <p class="font14 margin-bottom30">Or Login with</p>
                <a href="" class="btn fbLoginBtn margin-bottom30">
                    <span class="fbIcon">
                        <span class="bwsprite fb-login-icon"></span>
                    </span>
                    <span class="textWithFbGp">Sign in with facebook</span>
                </a>
                <a class="btn gplusLoginBtn">
                    <span class="gplusIcon">
                        <span class="bwsprite gplus-login-icon"></span>
                    </span>
                    <span class="textWithFbGp">Sign in with Google</span>
                </a>
            </div>--%>
        </div>
        <div class="signUpStage" id="divSignUp" runat="server">
            <div class="signUpWithBW">
                <h2 class="margin-bottom30">Sign up to BikeWale</h2>
                <div class="form-control-box margin-bottom20">
                    <asp:TextBox class="form-control" name="name" placeholder="Name" id="txtNameSignup" runat="server" />
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your name</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <asp:TextBox class="form-control" name="emailId" id="txtEmailSignup" placeholder="Email Id" runat="server" />
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your email id</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <span class="form-mobile-prefix">+91</span>
                    <asp:TextBox class="form-control padding-left40" name="mobile" placeholder="Mobile no." id="txtMobileSignup" runat="server" />
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your mobile number</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <asp:TextBox type="password" class="form-control" name="password" TextMode="Password" placeholder="Password" id="txtRegPasswdSignup" runat="server" />
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your password</div>
                </div>                
                <div class="margin-bottom30 font12">
                    <asp:CheckBox id="chkAgreeSignup" type="checkbox" runat="server" />
                    <label for="agreecheck">I have read and agree with the
                    <a href="/visitoragreement.aspx" target="_blank">User Agreement</a> & <a href="/privacypolicy.aspx" target="_blank">Privacy Policy</a></label>
                </div>
                <asp:Button class="signupBtnLogin btn btn-orange text-uppercase margin-bottom15 margin-right10" Text="sign up" ID="btnSignup" runat="server" />
                <button type="button" id="btnSignUpBack" class="btn btn-white btn-md text-uppercase margin-bottom15">back</button>
            </div>
        </div>            
    </div>
</div><!-- ends here -->
<% } else { %>
<div class="loggedinProfileWrapper" id="loggedinProfileWrapper"><!-- Logged in user profile code starts here -->
    <div class="profileBoxContent">
        <div class="loginCloseBtn afterLoginCloseBtn position-abt pos-top10 pos-left10 infoBtn bwsprite cross-md-dark-grey cur-pointer"></div>
        <div class="user-profile-banner">
            <div class="user-profile-details padding-left10 padding-top70">
                <div class="user-profile-image rounded-corner50">
	            	<span class=""></span>
                </div>
                <div class="user-profile-name">
	            	<p class="font16 text-white padding-left10"><%= System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Bikewale.Common.CurrentUser.Name.ToLower()) %></p>
                </div>
            </div>
        </div>
            
        <div class="user-profile-option-list padding-top20">
            <ul class="profileUL">
                <li>
                    <a href="/users/MyContactDetails.aspx">
                        <span class="margin-left10 bwsprite myBikeWale-icon"></span>
                        <span class="padding-left10 profile-option-title">My Profile</span>
                    </a>
                </li>
                <li>
                    <a href="/mybikewale/myinquiries/">
                        <span class="margin-left15 bwsprite inquiry-icon"></span>
                        <span class="padding-left20 profile-option-title">My Inquiries</span>
                    </a>
                </li>                
                <li>
                    <a href="/users/newssubscription.aspx">
                        <span class="margin-left15 bwsprite newsletter-icon"></span>
                        <span class="padding-left20 profile-option-title">Subscribe Newsletters</span>
                    </a>
                </li>
                <li>
                    <a href="/mybikewale/changepassword/">
                        <span class="margin-left15 bwsprite login-password-icon"></span>
                        <span class="padding-left20 profile-option-title">Change password</span>
                    </a>
                </li>
                <li>
                    <a href="<%= Bikewale.Common .CommonOpn.AppPath + "users/login.aspx?logout=logout" %>">
                        <span class="margin-left15 bwsprite login-logout-icon"></span>
                        <span class="padding-left20 profile-option-title">Log out</span>
                    </a>
                </li>
            </ul>
        </div>
            
    </div>
</div>
<% } %>
<script type="text/javascript">
    ctrlBtnLoginId = '<%= btnLogin.ClientID%>';
    ctrlTxtLoginEmailId = '<%= txtLoginEmail.ClientID %>';
    ctrlTxtLoginPasswordId = '<%= txtLoginPassword.ClientID %>';
    ctrlHdnAuthDataId = '<%= hdnAuthData.ClientID %>';
    ctrlBtnSignup = '<%= btnSignup.ClientID %>';
    ctrlTxtNameSignup = '<%= txtNameSignup.ClientID %>';
    ctrlTxtEmailSignup = '<%= txtEmailSignup.ClientID %>';
    ctrlTxtMobileSignup = '<%= txtMobileSignup.ClientID %>';
    ctrlTxtRegPasswdSignup = '<%= txtRegPasswdSignup.ClientID %>';
    ctrlChkAgreeSignup = '<%= chkAgreeSignup.ClientID %>';
</script>

<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LoginControlNew" %>
<% if(Bikewale.Common.CurrentUser.Id == "-1") { %>
<div class="loginPopUpWrapper" id="loginPopUpWrapper"><!-- login code starts here -->
    <div class="loginBoxContent" id="Testlogin-box">
        <div class="loginCloseBtn position-abt pos-top10 pos-left10 infoBtn bwsprite cross-md-dark-grey cur-pointer"></div>
        <div class="loginStage">
            <div class="loginWithCW margin-bottom40">
                <h2 class="margin-bottom30">Log in to BikeWale</h2>
                <div class="form-control-box margin-bottom20">
                    <input type="text" class="form-control border-red" name="email id" id="txtloginemail" placeholder="Email">
                </div>
                <div class="form-control-box margin-bottom20">
                    <input type="text" class="form-control border-red" id="txtpasswordlogin" placeholder="Password">
                </div>
                <div class="margin-bottom20 font12">
                    <input id="chkRemMe" type="checkbox">
                    <label for="chkRemMe">Remember me</label>
                </div>
                <button type="button" id="btnLogin" class="btn btn-orange text-uppercase margin-bottom15 margin-right10">log in</button>
                <button type="button" class="loginBtnSignUp btn btn-white btn-md text-uppercase margin-bottom15">sign up</button>
                <div><a class="cur-pointer font12" id="forgotpass">Forgot password?</a></div>
                <!-- Forget Password -->
                <div id="forgotpassdiv">
                    <div class="hide margin-top20 margin-bottom30" id="forgotpassbox">
                        <div class="form-control-box margin-bottom20">
                            <input type="text" class="form-control" name="emailId" id="txtForgotPass" placeholder="Enter your registered email">
                            <span class="bwsprite error-icon hide"></span>
                            <div class="bw-blackbg-tooltip hide">Invalid Email</div>
                        </div>
                        <button id="frgPwd" class="btn btn-orange text-uppercase">send</button>
                    </div>
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
        <div class="signUpStage">
            <div class="signUpWithBW">
                <h2 class="margin-bottom30">Sign up to BikeWale</h2>
                <div class="form-control-box margin-bottom20">
                    <input type="text" class="form-control" name="name" placeholder="Name" id="txtnamelogin">
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your name</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <input type="text" class="form-control" name="emailId" id="txtemailsignup" placeholder="Email Id">
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your email id</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <span class="form-mobile-prefix">+91</span>
                    <input type="text" class="form-control padding-left40" name="mobile" placeholder="Mobile no." id="txtmobilelogin">
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your mobile number</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <input type="password" class="form-control" name="password" placeholder="Password" id="txtRegPasswd">
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your password</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <input type="password" class="form-control" name="confirmPassword" placeholder="Confirm Password" id="txtConfPasswdlogin">
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your password</div>
                </div>                    
                <div class="margin-bottom30 font12">
                    <input id="agreecheck" type="checkbox">
                    <label for="agreecheck">I have read and agree with the
                    <a href="#">User Agreement</a> & <a href="#">Privacy Policy</a></label>
                </div>
                <button type="button" class="signupBtnLogin btn btn-orange text-uppercase margin-bottom15 margin-right10">sign up</button>
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
                    <a herf="/mybikewale/myinquiries/">
                        <span class="margin-left15 bwsprite login-edit-icon"></span>
                        <span class="margin-left20 profile-option-title">Manage my bike listing</span>
                    </a>
                </li>                
                <li>
                    <a herf="/mybikewale/changepassword/">
                        <span class="margin-left15 bwsprite login-password-icon"></span>
                        <span class="margin-left20 profile-option-title">Change password</span>
                    </a>
                </li>
                <li>
                    <a herf="<%= Bikewale.Common .CommonOpn.AppPath + "users/login.aspx?logout=logout" %>">
                        <span class="margin-left15 bwsprite login-logout-icon"></span>
                        <span class="margin-left20 profile-option-title">Log out</span>
                    </a>
                </li>
            </ul>
        </div>
            
    </div>
</div>
<% } %>
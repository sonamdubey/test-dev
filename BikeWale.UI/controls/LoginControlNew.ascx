<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LoginControlNew" %>
<% if(Bikewale.Common.CurrentUser.Id == "-1") { %>

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
                    <a href="<%= Bikewale.Common .CommonOpn.AppPath + "users/login.aspx?logout=logout&ReturnUrl=" + HttpContext.Current.Request.RawUrl %>">
                        <span class="margin-left15 bwsprite login-logout-icon"></span>
                        <span class="padding-left20 profile-option-title">Log out</span>
                    </a>
                </li>
            </ul>
        </div>
            
    </div>
</div>
<% } %>
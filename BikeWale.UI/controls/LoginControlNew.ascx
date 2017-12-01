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
            
        <div class="user-profile-option-list padding-top20 padding-left10">
            <ul class="profileUL">
                <li>
                    <a href="/users/MyContactDetails.aspx">
                        <span class="bwsprite myBikeWale-icon"></span>
                        <span class="profile-option-title">My Profile</span>
                    </a>
                </li>
                <li>
                    <a href="/mybikewale/myinquiries/">
                        <span class="bwsprite inquiry-icon"></span>
                        <span class="profile-option-title">My Inquiries</span>
                    </a>
                </li>                
                <li>
                    <a href="/users/newssubscription.aspx">
                        <span class="bwsprite newsletter-icon"></span>
                        <span class="profile-option-title">Subscribe Newsletters</span>
                    </a>
                </li>
                <li>
                    <a href="/mybikewale/changepassword/">
                        <span class="bwsprite login-password-icon"></span>
                        <span class="profile-option-title">Change password</span>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0)" data-url="<%= String.Format("{0}{1}", "/api/customer/logout/?ReturnUrl=" , HttpUtility.UrlEncode( HttpContext.Current.Request.RawUrl)) %>" data-sitetoken="<%= Bikewale.Utility.BikewaleSecurity.Encrypt(Bikewale.Common.CurrentUser.Id) %>" id="btnLogout">
                        <span class="bwsprite login-logout-icon"></span>
                        <span class="profile-option-title">Log out</span>
                    </a>
                    <script type="text/javascript">
                        docReady(function () {
                            try {
                                $("#btnLogout").click(function () {
                                    var btn = $(this);                                    
                                    $.ajax(
                                        {
                                            type: "POST",
                                            url: btn.data("url"),
                                            headers: {
                                                "customerId": <%= Bikewale.Common.CurrentUser.Id%>,
                                                "token" : btn.data("sitetoken")
                                            }
                                        })
                                        .done(function(data) { if(data=="/") { window.location.reload(); }else{ window.location.href = data; }})
                                        .fail(function() {console.log( "Logout failed" );});
                                });
                            }
                                catch (e) {
                                console.warn(e.message);
                            }
                        });
                    </script>
                </li>
            </ul>
        </div>
            
    </div>
</div>
<% } %>
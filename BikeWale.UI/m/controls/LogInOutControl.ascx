﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.LogInOutControl" %>
<% if (loggedInUserId > 0)
   { %>
        <li class="padding-top10 padding-bottom15">
            <div class="border-solid-top"></div>
        </li>
        <li>
            <p class="font12 text-light-grey padding-left15">Profile</p>
        </li>
        <li>
            <a href="/users/MyContactDetails.aspx">
                <span class="bwmsprite myBikeWale-icon"></span>
                <span class="navbarTitle">My Profile</span>
            </a>
        </li>
        <li>
            <a href="/mybikewale/myinquiries/">
                <span class="bwmsprite inquiry-icon"></span>
                <span class="navbarTitle">My Inquiries</span>
            </a>
        </li>
        <li>
            <a href="/users/newssubscription.aspx">
                <span class="bwmsprite newsletter-icon"></span>
                <span class="navbarTitle">Subscribe Newsletters</span>
            </a>
        </li>
        <li>
            <a href="/mybikewale/changepassword/">
                <span class="bwmsprite password-icon"></span>
                <span class="navbarTitle">Change password</span>
            </a>
        </li>
        <li>
            <a href="javascript:void(0)" data-url="<%=String.Format("{0}{1}", "/api/customer/logout/?ReturnUrl=" , HttpUtility.UrlEncode( HttpContext.Current.Request.RawUrl))%>" rel="nofollow" data-sitetoken="<%= Bikewale.Utility.BikewaleSecurity.Encrypt(Bikewale.Common.CurrentUser.Id) %>"  id="btnLogout">
                <span class="bwmsprite logout-icon"></span>
                <span class="navbarTitle">Logout</span>
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
        <% } else { %>
        <li>
            <a href="/m/users/login.aspx" rel="nofollow">
                <span class="bwmsprite myBikeWale-icon"></span>
                <span class="navbarTitle">Login</span>
            </a>
        </li>
        <% } %>
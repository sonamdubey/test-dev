<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LogInoutControl" %>
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
                <span class="bwmsprite home-icon"></span>
                <span class="navbarTitle">My Profile</span>
            </a>
        </li>
        <li>
            <a href="/mybikewale/myinquiries/">
                <span class="bwmsprite home-icon"></span>
                <span class="navbarTitle">My Inquiries</span>
            </a>
        </li>
        <li>
            <a href="/users/newssubscription.aspx">
                <span class="bwmsprite home-icon"></span>
                <span class="navbarTitle">Subscribe Newsletters</span>
            </a>
        </li>
        <li>
            <a href="/mybikewale/changepassword/">
                <span class="bwmsprite home-icon"></span>
                <span class="navbarTitle">Change password</span>
            </a>
        </li>
        <li>
            <a href="<%= Bikewale.Common.CommonOpn.AppPath + "m/users/login.aspx?logout=logout&ReturnUrl=" + HttpContext.Current.Request.RawUrl%>" rel="nofollow">
                <span class="bwmsprite myBikeWale-icon"></span>
                <span class="navbarTitle">Logout</span>
            </a>
        </li>
        <% } else { %>
        <li>
            <a href="/m/users/login.aspx" rel="nofollow">
                <span class="bwmsprite myBikeWale-icon"></span>
                <span class="navbarTitle">Login</span>
            </a>
        </li>
        <% } %>
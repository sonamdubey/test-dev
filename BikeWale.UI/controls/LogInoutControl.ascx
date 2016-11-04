<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LogInoutControl" %>
<% if(loggedInUser != "-1") { %>
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
﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LoginStatusNew" %>
<% if(Bikewale.Common.CurrentUser.Id == "-1") { %>
<div class="login-box" id="firstLogin">Log In</div>
<% } else { %>
<div id="userLoggedin" class="loggedin-box rounded-corner50">
    <span class="bwsprite login-no-photo-icon"></span>
    <span class="login-with-photo hide">
        <img src="">
    </span>
</div>
<% } %>
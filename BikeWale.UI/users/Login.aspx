<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikWale.Users.Login" Trace="false" %>
<%@ Register TagPrefix="BikeWale" TagName="Login" src="/Controls/LoginControl.ascx" %>
<%
    title = "User Login - BikeWale";
    description = "bikewale.com user login";
    keywords = "users, login, register, forgot password";
%>
<!-- #include file="/includes/headMyBikeWale.aspx" -->
<div class="container_12 container-min-height margin-top10 content-inner-block-10">
    <div class="grid_8">    
        <BikeWale:Login id="ctlLogin" runat="server" />
    </div>
    <div class="clear"></div>
</div>
<!-- #include file="/includes/footerInner.aspx" -->

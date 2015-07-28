<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikWale.Users.Registration" Trace="false"%>
<%@ Register TagPrefix="BikeWale" TagName="Register" src="/Controls/RegisterControl.ascx" %>
<%
    title = "User Registration - BikeWale";
    description = "bikewale.com User Registration";
    keywords = "users, registration, register, forgot password";
%>
<!-- #include file="/includes/headMyBikeWale.aspx" -->
<form id="Form1" runat="server"> 
    <div class="container_12 margin-top15">
        <div class="grid_8">            
            <BikeWale:Register id="ctlRegister" runat="server" />
        </div>    
    </div>
</form>
<!-- #include file="/includes/footerInner.aspx" -->

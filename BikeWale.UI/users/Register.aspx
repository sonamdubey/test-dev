<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikWale.Users.Registration" Trace="false"%>
<%@ Register TagPrefix="BikeWale" TagName="Register" src="/Controls/RegisterControl.ascx" %>
<%
    title = "User Registration - BikeWale";
    description = "bikewale.com User Registration";
    keywords = "users, registration, register, forgot password";
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd300x250BtfShown = false;
%>
<!-- #include file="/includes/headMyBikeWale.aspx" -->
    <div class="container_12 margin-top15">
        <div class="grid_8">            
            <BikeWale:Register id="ctlRegister" runat="server" />
        </div>    
    </div>
<!-- #include file="/includes/footerInner.aspx" -->

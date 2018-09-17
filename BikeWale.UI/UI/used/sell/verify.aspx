<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.VerifySellBikeUser" Trace="false" Debug="false" %>
<%
    title = "One-time Mobile Verification";
    description = "One-time Mobile Verification";

    isAd300x250Shown = false;
    isAd300x250BTFShown = false;
%>

<!-- #include file="/UI/includes/headSell.aspx" -->
<script type="text/javascript" src="<%= staticUrl  %>/UI/src/classified/sellbike.js?1.1"></script>
<div class="container_12 margin-top20">    
    <div class="grid_8 min-height"><!--    Left Container starts here -->
        <h1>One-time Mobile Verification</h1>
        <p class="desc-para">We have just sent you an SMS with a 5-digit verification code on your mobile number. Please enter the verification code below to proceed.</p>
        <div class="margin-top5">
            <img align="absmiddle" src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/mobi-verif.gif" border="0" />
            <asp:TextBox id="txtVerificationCode" runat="server" Text="Enter your code here"></asp:TextBox>
            <asp:Button id="btnVerifyCustomer" runat="server" Text="Verify" class="buttons text_white" />       
            <asp:Label id="lblError" runat="server" class="error"></asp:Label>            
         </div>
    </div>
</div>
<script type="text/javascript">
    $("#txtVerificationCode").click(function () {
        $(this).val("");
    }).blur(function () {       
        if ($(this).val() == "") {
            $(this).val("Enter your code here");
        }
    });
</script>
<style type="text/css">
    #txtVerificationCode { height:40px; }
</style>
<!--    #include file="/UI/includes/footerInner.aspx" -->

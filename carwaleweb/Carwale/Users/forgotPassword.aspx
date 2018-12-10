<%@ Page trace="false" Inherits="Carwale.UI.Users.ForgotPassword" AutoEventWireUp="false" Language="C#" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 2;
	Title 			= "Password Recovery";
	Description 	= "CarWale.com Password Recovery";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/headMyCarwale.aspx" -->

<form runat="server">
<div class="left_container_top" style="width:643px;">
	<div id="youHere">
		<img src="<%=Carwale.UI.Common.ImagingFunctions.GetRootImagePath()%>/images/bullet/arrow.gif" align="absmiddle" /><span>You are here</span> : 
		<a href="/Mycarwale/default.aspx">My CarWale</a> &raquo; Forgot Password
	</div>
	<h2 class="red">Password Recovery</h2>
	<div class="infoTop">
		<span id="spnError" class="error" runat="server"></span>
		<table width="100%"  border="0" cellspacing="0" cellpadding="6" style="border:1px solid #000000; ">
			<tr>
				<th height="20" colspan="2" bgcolor="#8E8E8E"><span style="color: #FEFEFE">Recover your password </span></th>
				<th bgcolor="#6597ca">
					<span style="color: #FEFEFE; font-weight: bold">Not a Member!</span></th>
			</tr>
			<tr>
				<td align="right"><span style="font-weight: bold">Your Email</span></td>
				<td style="border-right:1px solid #000000;">
					<asp:TextBox ID="txtLoginid" runat="server"></asp:TextBox>
				</td>
				<td rowspan="2">
					<p>Welcome to the CarWale.com<br>
					Registration Center</p>
					<p>Signing up is quick, easy and FREE.<br>
					Just click on the button below</p>						</td>
			</tr>
			
			<tr>
				<td colspan="2" style="border-right:1px solid #000000; ">&nbsp;</td>
			</tr>
			<tr align="center">
				<td colspan="2" style="border-right:1px solid #000000;">
					<div class="buttons">
						<asp:Button ID="butLogin" CssClass="buttons" Text="Email me my password" runat="server" />						
					</div>	
				</td>
				<td>
					<div class="buttons">
						<asp:Button ID="butSignup" CssClass="buttons" CausesValidation="false" Text="Register" runat="server" />						
					</div>
				</td>
			</tr>
		</table>
	</div>
</div>
<!-- Start of Right Container -->
<div class="right_container" style="float:right;width:300px;">
	<div style="clear:both;float:left;margin-top:20px;"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %></div>
</div>

</form>
        
<!-- #include file="/includes/footer-old.aspx" -->

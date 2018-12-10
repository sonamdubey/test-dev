<%@ Page trace="false" AutoEventWireUp="false" Inherits="Carwale.UI.Users.VerifyEmail" Language="C#" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 1;
	Title 			= "Member Accout Verification";
	Description 	= "CarWale.com Member Registration Confirmation";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/headMyCarwale.aspx" -->
<!-- Header ends here -->
<style>
	p, li { font-size:12px; color:#003366; }
</style>

<!-- For the ad only starts here -->
<table width="100%" border="0">
	<tr>
		<td valign="top">
<!-- For the ad only ends here-->
<table width="100%" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td class="ct" align="left"><span class="rh">account</span> <span class="bh">verification pending!</span></td>
        <td class="rtc"><img src="<%=CommonOpn.ImagePath%>spacer.gif" width="1" height="1"></td>
      </tr>
      <tr>
        <td class="cmc">
			<blockquote>
			
			
<p>
<span id="spnError" class="error" runat="server" />
</p>
<p>
	Note: To generate verification code, follow these steps:<br><br>
	1. Login (skip, if you are already logged in). <br>
	2. Click 'verification pending' link next to your name (in the header).<br>
	3. Click 'Email me my account verification code'.<br> 
	4. Check yoor mail.<br> 
	5. Follow the link in the mail and its done. <br>
</p>
</blockquote></td>
        <td class="rmb"><img src="<%=CommonOpn.ImagePath%>spacer.gif" width="1" height="1"></td>
      </tr>
      <tr>
        <td class="cb"><img src="<%=CommonOpn.ImagePath%>spacer.gif" width="1" height="1"></td>
        <td class="rbc"><img src="<%=CommonOpn.ImagePath%>spacer.gif" width="1" height="1"></td>
      </tr>
    </table>				
<!-- ad starts here -->		  
</td>
<td valign="top" align="right" width="160">&nbsp;
  
</td>
</tr>
</table>
<!-- ad ends here -->	       

<!-- Footer starts here -->
<!-- #include file="/includes/footer-old.aspx" -->
<!-- Footer ends here -->  
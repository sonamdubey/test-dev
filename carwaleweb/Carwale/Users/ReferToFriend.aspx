<%@ Page trace="false" Language="C#" Inherits="Carwale.UI.Users.ReferToFriend" AutoEventWireup="false" %>
<html>
	<head>
		<title>Refer to friend : CarWale.com</title>
		<style>
			h2 { color:#003366;border-bottom:1px solid #DFDFDF; font-size:17px; padding:7px; }
			html, body { margin:0px;font-family:Arial, Helvetica, sans-serif; }
			td { font-size:12px; }
			.butons{background-color:#6597CA; color:#ffffff; border:0; padding:0; font-weight:bold; font-family:Verdana, Arial, Helvetica, sans-serif;}
		</style>	
		<script language="javascript">
			<% if ( dataSaved ) { %>	
				alert("Thanks for referring CarWale.com\nThe mail has been sent successfully.");
				window.close();
			<% } %>
		</script>	
	</head>
<body>
	<h2>Refer CarWale.com to friends</h2>
	<form runat="server">
	<table width="100%">
		<tr><td colspan="2"><asp:Label ID="lblMessage" EnableViewState="false" Font-Bold="true" ForeColor="#FF0000" runat="server"></asp:Label></td></tr>
		<tr>
		  <td width="30%">Your Name<span class="error">*</span></td>
		  <td><asp:TextBox ID="txtName" Columns="20" MaxLength="50" runat="server" /></td>
	  </tr>
		<tr>
		  <td>Your Email<span class="error">*</span></td>
		  <td><asp:TextBox ID="txtEmail" Columns="20" MaxLength="50" runat="server" /> </td>
	  </tr>
		<tr>
		  <td>
		  	Friend's Emails<span class="error">*</span><br>
		  	<span style="color:#777;">Seperate multiple emails by commas</span>
		  </td>
		  <td><asp:TextBox ID="txtFriendEmail" Columns="20" TextMode="MultiLine" Rows="3" runat="server" /> </td>
	  </tr>
		<tr>
			<td align="center" colspan="2"><asp:Button CssClass="butons" ID="btnSend" Text="Send Mail" runat="server" /></td>
		</tr>
	</table>
	</form>
	<script language="javascript">
		
		if ( document.getElementById('btnSend') )
			document.getElementById('btnSend').onclick = validateme;
			
		function validateme()
		{
			
			if(trim(document.getElementById('txtName').value) == "")
			{
				alert("Please Provide Your Name.");
				document.getElementById('txtName').focus();
				return false;
			}
			
			if(trim(document.getElementById('txtEmail').value) == "")
			{
				alert("Please Provide Your Email.");
				document.getElementById('txtEmail').focus();
				return false;
			}
			
			str=document.getElementById('txtEmail').value;
			strlen=str.length
			strlen=(strlen-1)
			strlen1=(str.indexOf('.')+7)
			strsub1=str.substring(str.indexOf('.'),strlen1)
	
			if((document.getElementById('txtEmail').value) !="")
			{
				flag=true;
				if((str.lastIndexOf('@') >= strlen || str.indexOf('@')== 0 || str.indexOf('@')== -1 || str.indexOf(' ')>0 )) 
					{
						alert("Please enter a valid email address. Only one email address will be accepted.");
						document.getElementById('txtEmail').focus();
						return false ;
					}
			}
			
			if(trim(document.getElementById('txtFriendEmail').value) == "")
			{
				alert("Please Provide Your Friend's Email.");
				document.getElementById('txtFriendEmail').focus();
				return false;
			}
			
			email=document.getElementById('txtFriendEmail').value;
			var expression=/^(([a-zA-Z0-9\-\._]+)@(([a-zA-Z0-9\-_]+\.)+)([a-zA-Z]{2,3})(,(?!$))?)+$/;
			if(!(expression.test(email)) )
			{
			   	alert("Please enter a valid email address");
				document.getElementById('txtFriendEmail').focus();
			   return false;
			 }
			
		return true;
		}
		
		
		function trim(strTrim)
		{
			return strTrim.replace(/^\s*|\s*$/g,"");
		}
		
		
	</script>	
	</body>
</html>

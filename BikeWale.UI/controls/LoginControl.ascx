<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.LoginControl" %>
<%@ Import NameSpace="Bikewale.Common" %>
<span id="spnError" class="error" runat="server"></span>
<h1>Login &nbsp; &nbsp;<span> Don't have an account? <a href="/users/register.aspx">Register</a></span></h1>
<table class="tbl-default margin-top15" border="0" cellspacing="0" cellpadding="3">
	<tr>
		<td width="130"><strong>Enter your email id</strong></td>
		<td><asp:TextBox ID="txtLoginid" Columns="25" runat="server" CssClass="text"></asp:TextBox></td>
	</tr>
	<tr>
		<td><strong>Password</strong></td>
		<td><asp:TextBox ID="txtPasswd" TextMode="Password" runat="server" CssClass="text"></asp:TextBox>&nbsp;&nbsp;<a href="/users/forgotpassword.aspx">Forgot password?</a></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td><asp:CheckBox ID="chkRemember" Text=" Remember me on this computer" runat="server" /></td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td><asp:Button ID="butLogin" CssClass="action-btn" Text="Login" runat="server" /></td>
	</tr>	
</table>
<script type="text/javascript">

    $("#<%=txtLoginid.ClientID.ToString() %>,#<%=txtPasswd.ClientID.ToString() %>").keypress(function (e) {        
        try {	//for firefox           
            if (e.which || e.keyCode) {
                if ((e.which == 13) || (e.keyCode == 13)) {                    
                    return true;
                }
            }
            else { return false };
        }
        catch (exception) {
            //for ie
            if (event.keyCode) {
                if (event.keyCode == 13) {
                    return false;
                }
            }
            else { return true };
        }
    });

    function pressLoginButton(e) {

    }
</script>
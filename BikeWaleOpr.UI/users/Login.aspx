<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.users.Login" Trace="false" Debug="false" EnableEventValidation="false" %>
<!-- #Include file="/includes/headerUser.aspx" -->
<style>
	.loginTable{ border-radius:10px;background-color:#EEEEEE;color:#000000; font-weight:bold; font-size:14px;font-family:trebuchet ms; /* For FF / -webkit-border-radius:10px; / For Safari and Chrome / -o-border-radius:10px; / For Opera / -ms-border-radius:10x; / For IE */}
	.loginTable th{padding-left :10px; background-color:#FF9148;text-align:left; border-collapse:collapse; color:#FFFFFF; font-weight:bold; font-size:20px; height:25px; border-bottom:1px solid #9B9B9B;border-radius:10px 10px 0 0; -moz-border-radius:10px 0 0 10px;}
	.loginTable td{ border-collapse:collapse; color:#000000; font-size:13px; height:20px; padding:10px 2px 3px 10px;}
	.txtBox{ width:140px; height:20px; text-align:left; padding-top:4px;font-weight:bold; font-size:14px;font-family:trebuchet ms}
	.button{ height:30px; text-align:left; padding:5px;font-weight:bold; font-size:14px;font-family:trebuchet ms; cursor:pointer;color:#FFFFFF;}
    .bold { font-weight: bold; }
</style>
   <div style="height:350px;">
             <div style="margin:130px auto 0 auto; width:300px;">                 
                    <table  class="loginTable" width="270" border="0" cellspacing="0" cellpadding="5">
                        <tr >
                            <th colspan="3">Login</th>
                        </tr>
						<tr>
							<td><span class="bold">Login Name</span>&nbsp;</td>
							<td><asp:TextBox CssClass="txtBox" ID="txtLoginid" runat="server" Columns="12"></asp:TextBox></td>
						</tr>
						<tr>
							<td><span class="bold">Password </span></td>
							<td><asp:TextBox CssClass="txtBox" ID="txtPasswd" TextMode="Password" runat="server" Columns="12"></asp:TextBox></td>
						</tr>
						<tr align="center" >
							<td colspan="2" align="right" style="padding-right:20px;"><asp:Button ID="btnLogin" CssClass="button" Text="Login" runat="server" style="background-color: #FF9148" /></td>
						</tr>
                        <tr align="right">
                            <td colspan="3">
                                <span id="spnErrorPwd" class="errorMessage"  runat="server"></span>  
                            </td>
                        </tr>                   
                    </table>
           </div>  
	</div>
<!-- #Include file="/includes/footerNew.aspx" -->
<script type="text/javascript" >
    $(document).ready(function () {
        $('#btnLogin').click(function () {
            var txtName =$('#txtLoginid').val();
            var txtPwd = $('#txtPasswd').val();
            $('#spnErrorPwd').text("");
            if ((txtName == "" || txtPwd == "")) {
                if (txtName == "" && txtPwd == "") {
                    $('#spnErrorPwd').text("Please Enter username and password");
                    return false;
                }
                else if (txtName == "") {
                    $('#spnErrorPwd').text("Please Enter username");
                    return false;
                }
                else if (txtPwd == "")
                {
                    $('#spnErrorPwd').text("Please Enter password");
                    return false;
                }
            }
            else 
                return true;

        });
    })
</script>


<%@ Control Inherits="Carwale.UI.Controls.LoginControl" Language="C#" %>
<%@ Import NameSpace="Carwale.UI.Common" %>
<h1 class="leftfloat content-inner-block" style="font-size:19px;"><%= Header%></h1>
<div class="margin-top15">Don't have an account? <a href="/users/register.aspx">Register</a></div>
<div class="clear"></div>
<div class="white-shadow content-inner-block">
    <table border="0" cellpadding="0" cellspacing="0">
	    <tr>
		    <td><span id="spnError" class="error" runat="server"></span></td>
	    </tr>
	    <tr>
		    <td>
			    <table border="0" cellspacing="0" cellpadding="3" class="margin-top15">
                   <col width="67">
				    <tr>
					    <td valign="top" align="left"><span>Email Id</span></td>
					    <td>
						    <asp:TextBox ID="txtLoginid" Columns="25" runat="server" CssClass="text"></asp:TextBox><span id="spnErrEmail" class="error margin-left5"></span>
					    </td>
				    </tr>
				    <tr class="margin-top10">
					    <td align="left"><span>Password</span></td>
					    <td><asp:TextBox ID="txtPasswd" TextMode="Password" runat="server" CssClass="text" Width="145" style="padding:5px;"></asp:TextBox>
                            <span id="spnErrPass" class="error margin-left5"></span>
					    </td>
				    </tr>
				    <tr>
					    <td>&nbsp;</td>
					    <td><asp:CheckBox ID="chkRemember" Text=" Remember me on this computer" runat="server" /></td>
				    </tr>
				    <tr>
					    <td>&nbsp;</td>
					    <td>
						    <div class="buttons margin-top10">
							    <asp:Button ID="butLogin" CssClass="buttons-gray" Text="Login" runat="server" />
						    </div>
					    </td>
				    </tr>
				    <tr style='display:<%= ShowFooter == true ? "" : "none" %>'>
					    <td colspan="2">
						    Forgot password? <a onclick="forgotPwd()">Click Here</a>
						    <div id="divForgot" style="display:none; margin-top:20px; padding:10px; background-color:#FFFFCC; border:1px solid #CCCCCC;white-space: nowrap">
                                <div id="response">

                                </div>
							    <div id="pwdWait" style="display:none;">Processing your request.Please wait....</div>
							    <table id="tblEmail">
								    <tr>
									    <td nowrap>Email <input type="text" id="txtForgotPwd" /><span id="valEmail" class="error" ></span></td>
									    <td><input type="button" id="btnSendPwd" value="Send" class="buttons-gray" onclick="sendPwd()" /></td>
								    </tr>
							    </table>
						    </div>
					    </td>
				    </tr>
			    </table>
		    </td>
	    </tr>
</table>
</div>
<script type="text/javascript">
    objSpnError = '<%= spnError.ClientID %>';
    objTxtLoginid = '<%= txtLoginid.ClientID %>';
    objTxtPasswd = '<%= txtPasswd.ClientID %>';    

    function forgotPwd() {        
	    document.getElementById("divForgot").style.display = 'block';	    
	    if ($("#" + objTxtLoginid).val() != "")
	    {	        
	        $("#txtForgotPwd").val($("#" + objTxtLoginid).val());
	    }

	    document.getElementById("response").style.display = 'none';
	    document.getElementById("pwdWait").style.display = 'none';
	    document.getElementById("tblEmail").style.display = 'block';
	}
	
    function sendPwd() {
		var email = document.getElementById("txtForgotPwd").value;
		var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;
		
		if( reEmail.test(email) ) {
			document.getElementById("pwdWait").style.display = 'block';
			document.getElementById("tblEmail").style.display = 'none'; 
			setTimeout('resetPwd()', 1000); // prepare loading	
		} else {
			document.getElementById("valEmail").innerHTML = "Invalid email address";
			return false;
		}
	}
	
	function requestPwd(){		
	    var response = AjaxCommon.CustomerPasswordChangeRequest(document.getElementById("txtForgotPwd").value);
		if(response.value == true){
		    document.getElementById("response").innerHTML = "Congratulations! An email with a link to reset the password has been sent to your email address.";
		}
	    else
		    document.getElementById("response").innerHTML = "Invalid email address.";

		document.getElementById("response").style.display = 'block';
		document.getElementById("pwdWait").style.display = 'none';
		document.getElementById("tblEmail").style.display = 'none';
	}

	function resetPwd() {
	    $.ajax({
	        url: "/ajaxpro/CarwaleAjax.AjaxCommon,Carwale.ashx",
	        data: JSON.stringify(new Object({ email: document.getElementById("txtForgotPwd").value })),
	        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CustomerPasswordChangeRequest"); },
	        type: "POST",
	        success: function (response) {
	            //do your stuff here
	            var jsonString = eval('(' + response + ')');
	                if (jsonString.value == true) {
	                document.getElementById("response").innerHTML = "Congratulations! An email with a link to reset the password has been sent to your email address.";
	            }
	            else
	                document.getElementById("response").innerHTML = "Invalid email address.";

	            document.getElementById("response").style.display = 'block';
	            document.getElementById("pwdWait").style.display = 'none';
	            document.getElementById("tblEmail").style.display = 'none';
	        }
	    });
	}
	getLoginCtrlId("butLogin").onclick = pressLoginButton;

	function pressLoginButton(e) {	    
	    if (IsValidLoginDetails()) {
	        return true;
	    }
	    else { return false; }
	}
	
	function getLoginCtrlId( controlId ) {
		return document.getElementById( '<%=this.ID%>_' + controlId );
	}

    function IsValidLoginDetails() {
        var isValid = true;
        
        $("#spnErrEmail").text("");
        $("#spnErrPass").text("");

        if ($("#" + objTxtLoginid).val() == "")
        {
            $("#spnErrEmail").text("Required");
            isValid = false;
        }


        if ($("#" + objTxtPasswd).val() == "")
        {
            $("#spnErrPass").text("Required");
            isValid = false;
        }

        return isValid;
    }
</script>
<%@ Page Language="C#" %>

<div class="hide">
    <%--<table border="0" cellspacing="0" cellpadding="0" width="100%">	  
  <tr>
	<td width="50%" valign="top" style="border-right:1px dashed #C9C9C9; padding-right:10px;">
		<div id="loginError" class="error" style="display:none;"></div>
		<table width="100%" border="0" cellspacing="0" cellpadding="5">
		<tr><td colspan="2" class="login_hdr"><strong>Please Login</strong></td></tr>
		<tr>
			<td>Email</td>
			<td><input type="text" id="txtLogin" title="Write your email" class="text"/></td>
		  </tr>
		  <tr>
			<td>Password</td>
			<td><input type="password" id="txtPassword" class="text" onkeydown="javascript:if(event.keyCode==13) login_click();"/></td>
		  </tr>	
		  <tr>
			<td>&nbsp;</td>
			<td><input type="checkbox" id="chkRemMe" name="rem" /> <label for="rem">Remember Me</label></td>
		  </tr>		  
		  <tr>
			<td>&nbsp;</td>
			<td><a id="btnLogin" class="buttons-gray" onclick="login_click()">Submit</a></td>
		  </tr>
		  <tr>				
			<td colspan="2">Forgot password? <a onclick="forgotPwd()">Click Here</a></td>				
		  </tr>
		  <tr>
			<td colspan="2">
				<div id="divForgot" style="display:none; padding:5px; background-color:#FFFFCC; border:1px solid #CCCCCC;">
					<div id="pwdWait" style="display:none;">Processing your request.Plesae wait....</div>
					<table id="tblEmail">
						<tr><td colspan="2">Please provide your e-mail address</td></tr>
						<tr>
							<td><input type="text" id="txtForgotPwd" class="text" /></td>
							<td><div class="buttons"><input type="button" id="btnSendPwd" value="Send" class="buttons" onclick="sendPwd()" /></div></td>
						</tr>
						<tr>
							<td colspan="2"><span id="valEmail" class="error"></span></td>
						</tr>
					</table>
				</div>
			</td>
		  </tr>
		</table>
	</td>
	<td valign="top" style="padding-left:10px;">
		<div id="regUser">			
			<table id="tblReg" width="100%" border="0" cellspacing="0" cellpadding="5">
			  <tr>
				<td colspan="2" class="login_hdr"><strong>New Member! Please Register</strong><div id="userRegAlert" style="margin:7px 0 5px 0;">All form fields are required.</div></td>
			  </tr>
			  <tr>
				<td width="130">Name<span class="error">*</span></td>
				<td><input type="text" id="txtName" class="text" /> <span id="spnName" class="error"></span></td>
			  </tr>
			  <tr>
				<td>Email<span class="error">*</span></td>
				<td></span></td>
			  </tr>
			  <tr>
				<td>Password<span class="error">*</span></td>
				<td></td>
			  </tr>
			   <tr>
				<td valign="top">Confirm Password<span class="error">*</span></td>
				<td>/td>
			  </tr>
			  <tr>
				<td>Mobile No.<span class="error">*</span> +91-</td>
				<td><input type="text" id="txtMobile" class="text" onkeydown="javascript:if(event.keyCode==13) registerCust();" /> <span id="spnMobile" class="error"></span></td>
			  </tr>
			  <tr>
				<td>&nbsp;</td>
				<td><a id="btnLogin" class="buttons-gray" onclick="initRegisterCust()">Submit</a></td>
			  </tr>
		</table>
	</div>
</td>
</tr>
</table>--%>
</div>

<div class="us-login-popup rounded-corner5" id="loginBox">
    <span class="us-sprite close-icon-md close-box" id="closeLogin"></span>
    <div class="clear"></div>
    <div class="login-popup-tabs">
        <ul>
            <li class="active">Login</li>
            <li class="last">Sign up</li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="us-login-data">
        <table border="0" cellpadding="0" cellpadding="0" width="100%" class="us-popup">
            <tr>
                <td colspan="2">
                    <div id="loginError" class="error" style="display: none;"></div>
                </td>
            </tr>
            <tr>
                <td>Email Address:</td>
                <td>
                    <input type="text" id="txtLogin" title="Write your email" class="text" /></td>
            </tr>
            <tr>
                <td>Password:</td>
                <td>
                    <input type="password" id="txtPassword" class="text" onkeydown="javascript:if(event.keyCode==13) login_click();" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <a id="btnLogin" class="buttons-gray" onclick="login_click()">Submit</a>
                    <a class="margin-left10" onclick="forgotPwd()">Forgot password</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divForgot" style="display: none; padding: 5px; background-color: #FFFFCC; border: 1px solid #CCCCCC;">
                        <div id="pwdWait" style="display: none;">Processing your request.Plesae wait....</div>
                        <table id="tblEmail">
                            <tr>
                                <td colspan="2">Please provide your e-mail address</td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" id="txtForgotPwd" class="text" /></td>
                                <td>
                                    <div class="buttons">
                                        <a id="btnSendPwd" class="buttons-gray" onclick="sendPwd()">Send</a></div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span id="valEmail" class="error"></span></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="us-login-data hide">
        <table border="0" cellpadding="0" cellpadding="0" width="100%" class="us-popup">
            <tr>
                <td colspan="2">
                    <div id="userRegAlert">All form fields are required.</div>
                </td>
            </tr>

            <tr>
                <td>Email Address:</td>
                <td>
                    <input type="text" id="txtEmail" class="text" />
                    <span id="spnEmail" class="error"></span></td>
            </tr>
            <tr>
                <td>Password:</td>
                <td>
                    <input type="password" id="txtRegPasswd" class="text" />
                    <span id="spnRegPassword" class="error"></span></td>
            </tr>
           <%-- <tr>
                <td>Re-enter:</td>
                <td>
                    <input type="password" id="txtConfPasswd" class="text" />
                    <span id="spnConfPasswd" class="error"></span></td>
            </tr>--%>
            <tr>
                <td>&nbsp;</td>
                <td><a id="btnLogin" class="buttons-gray" onclick="initRegisterCust()">Sign Up Now</a></td>
            </tr>
        </table>
    </div>
</div>
<script language="javascript">
    var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;

    function login_click() {
        doLoginCustomer();
    }

    /*********************************************************************************
		Code to login customer through ajax
	***********************************************************************************/
    function doLoginCustomer() {
        var loginId = $("#txtLogin").val();
        var passwd = $("#txtPassword").val();
        var postLogin = '<%=Request.QueryString["postlogin"]%>';
		var rememberMe = true;
		if (loginId != "" && passwd != "") {
		    $.ajax({
		        type: "POST",
		        url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
		        data: '{"loginId":"' + loginId + '","pwd":"' + passwd + '","rememberMe":' + rememberMe + ',"isDealer":' + false + '}',
		        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UserLogin"); },
		        success: function (response) {
		            var loginStatus = eval('(' + response + ')');
		            if (loginStatus.value == true) {
		                $.ajax({
		                    type: "POST",
		                    url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
		                    data: {},
		                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCurrentUserName"); },
		                    success: function (userName) {
		                        var userName = eval('(' + userName + ')');
		                        $("#login1_lblUser").html("Welcome " + userName.value); // update user name
		                        $("#login1_hrefLogin").attr("href", "/users/login.aspx?logout=logout").html("   Logout"); // update logout href
		                        $("#login1_spnReg").hide();
		                        loginCallBack();
		                    }
		                });
		            } else {
		                alert("Invalid login details. Please try again.");
		            }
		        }
		    });
		} else {
		    $("#loginError").show().html("Please provide your login details.");
		}
    }
    /***************************************************************************
		Code to retrive customer password on the basis of Email 
	****************************************************************************/
    function forgotPwd() {
        $("#divForgot").show();
    }

    function initRegisterCust() {
        var email = $("#txtEmail");
        var passwd = $("#txtRegPasswd");
        //var confPasswd = $("#txtConfPasswd");

        registerCust(email, passwd);
    }

    /***************************************************************************
		Code to register customer with CarWale through ajax
	****************************************************************************/
    function registerCust(objEmail, objPasswd) {
        if (validateCustRegDetails(objEmail, objPasswd)) {
            var phone = "";
            var cityId = "";
            var name = $("#txtEmail").val().substr(0, $("#txtEmail").val().indexOf('@'));
            $.ajax({
                type: "POST",
                url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
                data: '{"custName":"' + name + '", "password":"' + $(objPasswd).val() + '", "email":"' + $(objEmail).val() + '", "phone":"' + phone + '", "mobile":"' + "" + '", "cityId":"' + cityId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UserRegistration"); },
                success: function (response) {
                    var status = eval('(' + response + ')');

                    if (Number(status.value) > 0) {
                        $("#login1_lblUser").html("Welcome " + name); // update user name
                        $("#login1_hrefLogin").attr("href", "/users/login.aspx?logout=logout").html("&nbsp Logout"); // update logout href
                        $("#login1_spnReg").hide();
                        loginCallBack();
                    } else {
                        alert("This email id is already registered with CarWale.");
                    }
                }
            });
        }
    }

    /***************************************************************************
		Code to validate customer registration details. 
		Alert user if try to register with invalid details
	****************************************************************************/
    function validateCustRegDetails(objEmail, objPasswd) {
        var email = $(objEmail).val();
        var passwd = $(objPasswd).val();
       // var confPasswd = $(objConfPasswd).val();

        if (email == "") {
            $("#userRegAlert").html("Email is required").addClass("error");
            $(objEmail).addClass("textAlert");
            return false;
        } else if (!reEmail.test(email)) {
            $("#userRegAlert").html("Invalid email address").addClass("error");
            $(objEmail).addClass("textAlert");
            return false;
        } else {
            $("#userRegAlert").html("All form fields are required.").removeClass("error");
            $(objEmail).removeClass("textAlert");
        }

        if (passwd == "") {
            $("#userRegAlert").html("Password is required").addClass("error");
            $(objPasswd).addClass("textAlert");
            return false;
        } else {
            $("#userRegAlert").html("All form fields are required.").removeClass("error");
            $(objPasswd).removeClass("textAlert");
        }

        //if (confPasswd == "") {
        //    $("#userRegAlert").html("Confirm password is required").addClass("error");
        //    $(objConfPasswd).addClass("textAlert");
        //    return false;
        //} else if (passwd != confPasswd) {
        //    $("#userRegAlert").html("Password didn't match. Please retype carefully");
        //    return false;
        //} else {
        //    $("#userRegAlert").html("All form fields are required.").removeClass("error");
        //    $(objConfPasswd).removeClass("textAlert");
        //}

        return true;
    }
</script>

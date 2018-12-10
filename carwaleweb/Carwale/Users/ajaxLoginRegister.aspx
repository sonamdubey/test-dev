<%@ Page Language="C#" %>
<table border="0" cellspacing="0" cellpadding="0" width="100%">	  
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
			<td><a id="btnLoginUsed" class="buttons-gray" onclick="login_click()">Submit</a></td>
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
				<td><input type="text" id="txtEmail" class="text" /> <span id="spnEmail" class="error"></span></td>
			  </tr>
			  <tr>
				<td>Password<span class="error">*</span></td>
				<td><input type="password" id="txtRegPasswd" class="text" /> <span id="spnRegPassword" class="error"></span></td>
			  </tr>
			   <tr>
				<td valign="top">Confirm Password<span class="error">*</span></td>
				<td><input type="password" id="txtConfPasswd" class="text" /> <span id="spnConfPasswd" class="error"></span></td>
			  </tr>
			  <tr>
				<td>Mobile No.<span class="error">*</span> +91-</td>
				<td><input type="text" id="txtMobile" class="text" onkeydown="javascript:if(event.keyCode==13) registerCust();" /> <span id="spnMobile" class="error"></span></td>
			  </tr>
			  <tr>
				<td>&nbsp;</td>
				<td><a id="btnLoginUsed" class="buttons-gray" onclick="initRegisterCust()">Submit</a></td>
			  </tr>
		</table>
	</div>
</td>
</tr>
</table>	
<script language="javascript">
    var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;

    function login_click() {
        doLoginCustomerUsed();
    }

    /*********************************************************************************
		Code to login customer through ajax
	***********************************************************************************/
    function doLoginCustomerUsed() {
        var loginId = $("#txtLogin").val();
        var passwd = $("#txtPassword").val();
        var postLogin = '<%=Request.QueryString["postlogin"]%>';
		var rememberMe = document.getElementById("chkRemMe").checked == 'checked' ? true : false;
		if (loginId != "" && passwd != "") {
		    $.ajax({
		        type: "POST",
		        url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
		        data: '{"loginId":"' + loginId + '","pwd":"' + passwd + '","rememberMe":"' + rememberMe + '","isDealer":' + false + '}',
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
        var custName = $("#txtName");
        var email = $("#txtEmail");
        var passwd = $("#txtRegPasswd");
        var confPasswd = $("#txtConfPasswd");
        var mobie = $("#txtMobile");

        registerCust(custName, email, passwd, confPasswd, mobie);
    }

    /***************************************************************************
		Code to register customer with CarWale through ajax
	****************************************************************************/
    function registerCust(objCust, objEmail, objPasswd, objConfPasswd, objMobile) {
        if (validateCustRegDetails(objCust, objEmail, objPasswd, objConfPasswd, objMobile)) {
            var phone = "";
            var cityId = "";

            $.ajax({
                type: "POST",
                url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
                data: '{"custName":"' + $(objCust).val() + '", "password":"' + $(objPasswd).val() + '", "email":"' + $(objEmail).val() + '", "phone":"' + phone + '", "mobile":"' + $(objMobile).val() + '", "cityId":"' + cityId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UserRegistration"); },
                success: function (response) {
                    var status = eval('(' + response + ')');

                    if (Number(status.value) > 0) {
                        $("#login1_lblUser").html("Welcome " + $(objCust).val()); // update user name
                        $("#login1_hrefLogin").attr("href", "/users/login.aspx?logout=logout").html("&nbsp Logout"); // update logout href
                        $("#login1_spnReg").hide();
                        loginCallBack();
                    } else {
                        alert("This email id is already registred with CarWale.");
                    }
                }
            });
        }
    }

    /***************************************************************************
		Code to validate customer registration details. 
		Alert user if try to register with invalid details
	****************************************************************************/
    function validateCustRegDetails(objCust, objEmail, objPasswd, objConfPasswd, objMobie) {
        var custName = $(objCust).val();
        var email = $(objEmail).val();
        var mobie = $(objMobie).val();
        var passwd = $(objPasswd).val();
        var confPasswd = $(objConfPasswd).val();

        if (custName == "") {
            $("#userRegAlert").html("Customer name is required").addClass("bg-hilight");
            $(objCust).addClass("textAlert");
            return false;
        } else {
            $("#userRegAlert").html("All form fields are required.").removeClass("bg-hilight");
            $(objCust).removeClass("textAlert");
        }

        if (email == "") {
            $("#userRegAlert").html("Email is required").addClass("bg-hilight");
            $(objEmail).addClass("textAlert");
            return false;
        } else if (!reEmail.test(email)) {
            $("#userRegAlert").html("Invalid email address").addClass("bg-hilight");
            $(objEmail).addClass("textAlert");
            return false;
        } else {
            $("#userRegAlert").html("All form fields are required.").removeClass("bg-hilight");
            $(objEmail).removeClass("textAlert");
        }

        if (passwd == "") {
            $("#userRegAlert").html("Password is required").addClass("bg-hilight");
            $(objPasswd).addClass("textAlert");
            return false;
        } else {
            $("#userRegAlert").html("All form fields are required.").removeClass("bg-hilight");
            $(objPasswd).removeClass("textAlert");
        }

        if (confPasswd == "") {
            $("#userRegAlert").html("Confirm password is required").addClass("bg-hilight");
            $(objConfPasswd).addClass("textAlert");
            return false;
        } else if (passwd != confPasswd) {
            $("#userRegAlert").html("Password didn't match. Please retype carefully");
            return false;
        } else {
            $("#userRegAlert").html("All form fields are required.").removeClass("bg-hilight");
            $(objConfPasswd).removeClass("textAlert");
        }

        if (mobie == "") {
            $("#userRegAlert").html("Mobile number is required").addClass("bg-hilight");
            $(objMobie).addClass("textAlert");
            return false;
        } else if (!re.test(mobie) || mobie.length > 10 || mobie.length < 10) {
            $("#userRegAlert").html("Invalid mobile number").addClass("bg-hilight");
            $(objMobie).addClass("textAlert");
            return false;
        } else {
            $("#userRegAlert").html("All form fields are required.").removeClass("bg-hilight");
            $(objMobie).removeClass("textAlert");
        }

        return true;
    }
</script>
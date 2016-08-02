<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.LoginControl" %>
<%@ Import NameSpace="Bikewale.Common" %>
<style type="text/css">
.tbl-default td { padding: 0 10px 25px; }
.container_12 input[type="text"], .container_12 input[type="password"] { width: 225px; }
.login-form-box { position:relative; width: 225px; }
.login-form-box .error-icon { display:none; top: 4px; }
.login-form-box .bw-blackbg-tooltip { display:none; top: 28px; }
</style>
<span id="spnError" class="error" runat="server"></span>
<h1>Login &nbsp; &nbsp;<span> Don't have an account? <a href="/users/register.aspx">Register</a></span></h1>
<table class="tbl-default margin-top15" border="0" cellspacing="0" cellpadding="3">
    <tr>
        <td width="150"><strong>Enter your email id</strong></td>
        <td>
            <div class="login-form-box">
                <asp:TextBox ID="txtLoginid" Columns="25" runat="server" CssClass="text form-control" TabIndex="1"></asp:TextBox>
                <span class="bwsprite error-icon"></span>
                <div class="bw-blackbg-tooltip"></div>
            </div>
        </td>
    </tr>
    <tr>
        <td><strong>Password</strong></td>
        <td>
            <div class="login-form-box">
                <asp:TextBox ID="txtPasswd" TextMode="Password" runat="server" CssClass="text" TabIndex="2"></asp:TextBox>
                &nbsp;&nbsp;<a href="/users/forgotpassword.aspx">Forgot password?</a>
                <span class="bwsprite error-icon"></span>
                <div class="bw-blackbg-tooltip"></div>
            </div>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td><asp:CheckBox ID="chkRemember" Text=" Remember me on this computer" runat="server" TabIndex="3"/></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td><asp:Button ID="butLogin" CssClass="action-btn text_white" Text="Login" runat="server" TabIndex="4" OnClientClick="return pressLoginButton(e)" /></td>
    </tr>	
</table>
<script type="text/javascript">

    var email = $("#<%=txtLoginid.ClientID.ToString() %>");
    var pass = $("#<%=txtPasswd.ClientID.ToString() %>");
    var loginBtn = $("#ctlLogin_butLogin");

    

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
        var isValid = false;
        emailVal = email.val().trim();
        passVal = pass.val().trim();
        if (emailVal.length > 0 && validateEmail(emailVal))
        {
            if (passVal.length > 0) {
                isValid = true;
            }
            else {
                setError(pass, 'Password should not be empty');
                isValid = false;
            }
        }   
        else {
            setError(email, 'Enter valid email id');
            isValid = false;
        }
        return isValid;
    }

    function validateEmail(emailVal) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(emailVal);
    }


    email.on('focus', function () {
        
            hideError($(this));
        
    });

    pass.on('focus', function () {
        
            hideError($(this));
        
    });

    function setError(ele, msg) {
        ele.addClass("border-red");
        ele.siblings("span, div").show();
        ele.siblings("div").text(msg);
    }
 
    function hideError(ele) {
        if (ele != null) {
            ele.removeClass("border-red");
            ele.siblings("span, div").hide();
        }
    }
</script>
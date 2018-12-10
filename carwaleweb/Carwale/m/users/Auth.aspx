<%@ Page Language="C#" ContentType="text/html" AutoEventWireup="false" trace="false" debug="true" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%
   bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<style>
   .box .plus {
    width: 18px;
    height: 18px;
    position: absolute;
    top: 10px;
    right: 5px;
    background-image: url("/m/images/icons-sheet.png?v=cwkghjfyfbnstyuitgktiutggbkyoi");
    background-position: -2px -248px;
}
   .box .minus {
    width: 18px;
    height: 18px;
    position: absolute;
    top: 10px;
    right: 5px;
    background-image: url("/m/images/icons-sheet.png?v=cwkghjfyfbnstyuitgktiutggbkyoi");
    background-position: -2px -272px;
}
   .arr-small {
    color: red;
    font-size: 15px;
}
</style>
</head>

<body class="<%= (showExperimentalColor  ? "btn-abtest" : "")%> m-special-skin-body m-no-bg-color">
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	<section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
            <div class="grid-12">
			
            <div id="divLoginRegister" class="f-13 darkgray margin-bottom10">
                <div id="expandoLogin" class="box box content-inner-block-10 content-box-shadow rounded-corner2 margin-top10 position-rel" onclick="BoxClicked(this);" type="expando">
                    <div class="heading">Already a member login here</div>
                    <div class="plus"></div>
                </div>
                <div id="extLogin" class="box-bot content-inner-block-10 content-box-shadow rounded-corner2" style="display:none;">
                    <div>Email &nbsp;&nbsp; <span id="spnEmail" class="error" style="display:none;">(Required)</span></div>
                    <div class="margin-top5"><input class="form-control" id="txtEmail" type="text" /></div>
                    <div class="margin-top15">Password &nbsp;&nbsp; <span id="spnPass" class="error" style="display:none;">(Required)</span></div>
                    <div class="margin-top5"><input class="form-control" id="txtPass" type="password" /></div>
                    <div class="margin-top15"><span class="linkButtonBig btn btn-xs btn-orange" onclick="LoginClicked();">&nbsp;&nbsp;&nbsp;&nbsp;<span id="spnLoginSubmit">Submit</span>&nbsp;&nbsp;&nbsp;&nbsp;</span> &nbsp;&nbsp; <span id="spnSubmit" class="error" style="display:none;">(Invalid Login)</span></div>
                    <div class="margin-top5"><a class="normal" onclick="ForgotPasswordClicked();">Forgot Password</a></div>
                </div>
                <div id="expandoRegister" class="box box content-inner-block-10 content-box-shadow rounded-corner2 margin-top10 position-rel" onclick="BoxClicked(this);" type="expando">
                    <div class="heading">New member register here</div>
                    <div class="plus"></div>
                </div>
                <div id="extRegister" class="box-bot content-inner-block-10 content-box-shadow rounded-corner2" style="display:none;">
                    <div>Name &nbsp;&nbsp; <span id="spnNameReg" class="error"></span></div>
                    <div class="margin-top5"><input class="form-control" id="txtNameReg" type="text" /></div>
                    <div class="margin-top15">Email &nbsp;&nbsp; <span id="spnEmailReg" class="error"></span></div>
                    <div class="margin-top5"><input class="form-control" id="txtEmailReg" type="text" /></div>
                    <div class="margin-top15">Password &nbsp;&nbsp; <span id="spnPassReg" class="error"></span></div>
                    <div class="margin-top5"><input class="form-control" id="txtPassReg" type="password" /></div>
                    <div class="margin-top15">Confirm Password &nbsp;&nbsp; <span id="spnConfirmPassReg" class="error"></span></div>
                    <div class="margin-top5"><input class="form-control" id="txtConfirmPassReg" type="password" /></div>
                    <div class="margin-top15">Mobile (+91) &nbsp;&nbsp; <span id="spnMobileReg" class="error"></span></div>
                    <div class="margin-top5"><input class="form-control" id="txtMobileReg" type="number" /></div>
                    <div class="margin-top15"><span class="linkButtonBig btn btn-xs btn-orange" onclick="RegisterClicked()">&nbsp;&nbsp;&nbsp;&nbsp;<span id="spnRegSubmit">Register</span>&nbsp;&nbsp;&nbsp;&nbsp;</span></div>
                    <div id="divRegMsg" class="new-line5 error">&nbsp;</div>
                </div>
            </div>	
            <div id="divForgotPassword" class="f-13 darkgray">
                <div class="box bot-rad-0 new-line5"><h2 class="heading margin-bottom10">Password Recovery</h2></div>
                <div id="divRecoveryForm" class="box-bot content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">
                    <div>Email</div>
                    <div class="margin-top5"><input class="form-control" id="txtEmailRecovery" type="text" /></div>
                    <div class="margin-top15">
                        <span class="linkButtonBig btn btn-xs btn-orange" onclick="PassRecoveryClicked();"><span id="spnSubmitRecovery">Email me password</span></span>
                        <span class="linkButtonBig btn btn-xs btn-orange" onclick="RecoveryCancelled();">Cancel</span>
                    </div>
                    <div id="spnEmailRecoveryErr" class="margin-top10 error">&nbsp;</div>
                </div>
                <div id="divRecoverySuccess" class="box-bot content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10" style="display:none;">
                    Your password has been sent to your email id "<span id="sentEmailId"></span>"<br /><br />
                    Redirecting to login screen in few seconds..Please wait.
                </div>
            </div>
            
            </div>
            <div class="clear"></div>
        </div>
        <!--Main container ends here-->
    </section>
    <div class="clear"></div>
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
	<!-- #include file="/m/includes/global/footer-script.aspx" -->

    <script language="javascript" type="text/javascript">
        function PassRecoveryClicked() {
            var _recoveryEmail = $("#txtEmailRecovery").val().trim().toLowerCase();
            if (RecoveryEmailValid(_recoveryEmail)) {
                $("#spnSubmitRecovery").html("Please Wait..");
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/MobileWeb.Ajax.Forums,Carwale.ashx",
                    data: '{"emailId":"' + _recoveryEmail + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SendPasswordByEmail"); },
                    success: function (response) {
                        $("#spnSubmitRecovery").html("Email me password");
                        var response = eval('(' + response + ')');
                        if (response.value == "0")
                            $("#spnEmailRecoveryErr").html("This email id is not registered with CarWale.");
                        else {
                            $("#divRecoveryForm").hide();
                            $("#divRecoverySuccess").show();
                            $("#sentEmailId").html(_recoveryEmail);
                            setTimeout(RedirectToLogin, 2500);
                        }
                    }
                });
            }
        }

        function RedirectToLogin() {
            $("#divRecoveryForm").show();
            $("#divRecoverySuccess").hide();
            RecoveryCancelled();
        }

        function RecoveryEmailValid(_recoveryEmail) {
            var retVal = true;
            $("#spnEmailRecoveryErr").html("&nbsp;");
            var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;

            if (_recoveryEmail == "") {
                retVal = false;
                $("#spnEmailRecoveryErr").html("Email required");
            }
            else if (!reEmail.test(_recoveryEmail)) {
                retVal = false;
                $("#spnEmailRecoveryErr").html("Invalid email");
            }

            return retVal;
        }

        function ForgotPasswordClicked() {
            $("#divLoginRegister").hide();
            $("#divForgotPassword").show();
            $("#divRecoveryForm").show();

            //SetControlWidth();
        }

        function RecoveryCancelled() {
            $("#divLoginRegister").show();
            $("#divForgotPassword").hide();
        }

        $(document).ready(function () {

            $("#spnLoginHead").hide();
            $("#divForgotPassword").hide();

                    <%if (Request.QueryString["act"] != null && Request.QueryString["act"].ToString().ToLower() == "log"){%>
                    BoxClicked(document.getElementById("expandoLogin"));
                    <%} else if (Request.QueryString["act"] != null && Request.QueryString["act"].ToString().ToLower() == "reg") {%>
                    BoxClicked(document.getElementById("expandoRegister"));
                    <%}%>
                });

        function BoxClicked(box) {
            var nextHidden = $(box).next().is(":hidden").toString();

            $("div[type='expando']").each(function () {
                $(this).find(":nth-child(2)").attr("class", "plus");
            });

            $(".box-bot").hide();
            $("div[type='expando']").removeClass("bot-rad-0");

            if (nextHidden == "true") {
                $(box).next().show();
                $(box).find(":nth-child(2)").attr("class", "minus");
                $(box).addClass("bot-rad-0");
            }
            else {
                $(box).next().hide();
                $(box).find(":nth-child(2)").attr("class", "plus");
                $(box).removeClass("bot-rad-0");
            }

            //SetControlWidth();
        }

        function LoginClicked() {
            var loginId = $("#txtEmail").val();
            var passwd = $("#txtPass").val();

            if (LoginValid(loginId, passwd)) {
                $("#spnLoginSubmit").html("Please Wait..");
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/MobileWeb.Ajax.Forums,Carwale.ashx",
                    data: '{"loginId":"' + loginId + '","passwdEnter":"' + passwd + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DoLogin"); },
                    success: function (response) {
                        $("#spnLoginSubmit").html("Submit");
                        var loginResponse = eval('(' + response + ')');
                        var lgRes = loginResponse.value.split("::::");
                        if (lgRes[0] == "1") {
                            //hostUrl = "http://m.carwale.com";
                            hostUrl = "http://<%=System.Configuration.ConfigurationSettings.AppSettings["HostUrl"].ToString()%>";
                                    location.href = hostUrl + "<%=Request.QueryString["returnUrl"].ToString()%>";
                                }
                                else
                                    $("#spnSubmit").show();
                            }
                        });
                    }
                }

                function LoginValid(loginId, passwd) {
                    $("#spnEmail").hide();
                    $("#spnPass").hide();
                    $("#spnSubmit").hide();

                    var retVal = true;

                    if (loginId == "") {
                        retVal = false;
                        $("#spnEmail").show();
                    }

                    if (passwd == "") {
                        retVal = false;
                        $("#spnPass").show();
                    }

                    return retVal;
                }

                function RegisterClicked() {
                    var custName = $("#txtNameReg").val().trim();
                    var custEmail = $("#txtEmailReg").val().trim().toLowerCase();
                    var custPass = $("#txtPassReg").val().trim();
                    var custConfirmPass = $("#txtConfirmPassReg").val().trim();
                    var custMobile = $("#txtMobileReg").val().trim();

                    $("#divRegMsg").html("&nbsp;");

                    if (RegisterValid(custName, custEmail, custPass, custConfirmPass, custMobile)) {
                        var phone = "";
                        var cityId = "";

                        $("#spnRegSubmit").html("Please wait..");

                        $.ajax({
                            type: "POST",
                            url: "/ajaxpro/MobileWeb.Ajax.Forums,Carwale.ashx",
                            data: '{"customerName":"' + custName + '", "password":"' + custPass + '", "email":"' + custEmail + '", "phone":"' + phone + '", "mobile":"' + custMobile + '", "cityId":"' + cityId + '"}',
                            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UserRegistration"); },
                            success: function (response) {
                                var status = eval('(' + response + ')');
                                $("#spnRegSubmit").html("Register");
                                if (status.value == "")
                                    $("#divRegMsg").html("Error occured..Try again.");
                                else if (status.value == "-1")
                                    $("#divRegMsg").html("Email Id already registered..Try with another email id.");
                                else if (parseInt(status.value) > 0) {
                                    hostUrl = "http://<%=System.Configuration.ConfigurationSettings.AppSettings["HostUrl"].ToString()%>";
                                    //hostUrl = "http://webserver:8085";
                                    location.href = hostUrl + "<%=Request.QueryString["returnUrl"].ToString()%>";
                                }
                            }
                        });
            }
        }

        function RegisterValid(custName, custEmail, custPass, custConfirmPass, custMobile) {
            $("#spnNameReg").html("");
            $("#spnEmailReg").html("");
            $("#spnPassReg").html("");
            $("#spnConfirmPassReg").html("");
            $("#spnMobileReg").html("");

            var retVal = true;

            var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;
            var reMobile = /^[0-9]*$/;

            if (custName == "") {
                retVal = false;
                $("#spnNameReg").html("(Required)");
            }

            if (custEmail == "") {
                retVal = false;
                $("#spnEmailReg").html("(Required)");
            }
            else if (!reEmail.test(custEmail)) {
                retVal = false;
                $("#spnEmailReg").html("(Invalid Email)");
            }

            if (custPass == "") {
                retVal = false;
                $("#spnPassReg").html("(Required)");
            }

            if (custConfirmPass == "") {
                retVal = false;
                $("#spnConfirmPassReg").html("(Required)");
            }

            if (custPass != "" && custConfirmPass != "" && custPass != custConfirmPass) {
                retVal = false;
                $("#spnConfirmPassReg").html("(Password and Confirm Password should match)");
            }

            if (custMobile == "") {
                retVal = false;
                $("#spnMobileReg").html("(Required)");
            }
            else if (!reMobile.test(custMobile)) {
                retVal = false;
                $("#spnMobileReg").html("(Digits allowed)");
            }
            else if (custMobile.length != 10) {
                retVal = false;
                $("#spnMobileReg").html("(Required 10 digits)");
            }

            return retVal;
        }
            </script>
</body>
</html>
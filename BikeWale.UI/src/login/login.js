$("#" + ctrlBtnLoginId).click(function () {
    var isSuccess = false;

    if (isValidLoginDetails()) {
        $.ajax({
            type: "GET",
            url: bwHostUrl + "/api/customer/authenticate?email=" + $("#" + ctrlTxtLoginEmailId).val().trim() + "&password=" + $("#" + ctrlTxtLoginPasswordId).val().trim() + "&createAuthTicket=true",
            dataType: 'json',
            async: false,
            success: function (response) {
                if (response != null && response.isAuthorized == true) {
                    $("#" + ctrlHdnAuthDataId).val(JSON.stringify(response));
                    isSuccess = true;
                }
                else {
                    toggleErrorMsg($("#" + ctrlTxtLoginPasswordId), true, "Invalid Email or Password");
                }
            }
        });
    }

    return isSuccess;
});

function isValidLoginDetails()
{
    var isValid = true;
    var objEmail = $("#" + ctrlTxtLoginEmailId);
    var objPass = $("#" + ctrlTxtLoginPasswordId);
    
    if (objEmail.val().trim() != "") {
        toggleErrorMsg(objEmail, false);
    }
    else {
        toggleErrorMsg(objEmail, true, "Please enter valid email");
        isValid = false;
    }
    if (objPass.val().trim() != "") {
        toggleErrorMsg(objPass, false);
    } else {
        toggleErrorMsg(objPass, true, "Please enter valid password");
        isValid = false;
    }

    return isValid;
}

$("#btnForgetPass").click(function () {
    sendPwd();
});

function sendPwd() {
    var objEmail = $("#txtForgotPassEmail");
    var email = objEmail.val();
    var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;

    if (reEmail.test(email)) {        
        setTimeout('requestPwd()', 1000);
    } else {
        toggleErrorMsg(objEmail, true, "Invalid Email");
        return false;
    }
}

function requestPwd() {    
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
        data: '{"email":"' + $("#txtForgotPassEmail").val() + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SendCustomerPwd"); },
        success: function (response) {
            var responseJSON = eval('(' + response + ')');
            if (responseJSON.value == true) {
                $("#processing_pwd").html("Your password has been sent to your email address.").show();
            }
            else if (responseJSON.value == false) {
                //$("#processing_pwd").html("<span class=readmore>This email id is not registered with us. <a href='register.aspx' style='font-weight:normal;'>Register Now</a><span>");
                toggleErrorMsg($("#txtForgotPassEmail"), true, "This email id is not registered with us.");
            }
        }
    });
    return false;
}
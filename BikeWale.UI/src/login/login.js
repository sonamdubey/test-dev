var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;
var objREmail = $("#" + ctrlTxtEmailSignup);
var objRPass = $("#" + ctrlTxtRegPasswdSignup);
var objRName = $("#" + ctrlTxtNameSignup);
var objRMobile = $("#" + ctrlTxtMobileSignup);
var objRbtnSignup = $("#" + ctrlBtnSignup);
var objRCheckbox = $("#" + ctrlChkAgreeSignup);
var objRegister = {};

$(objRCheckbox).attr('checked', true);

$("#" + ctrlBtnLoginId).click(function () {
    var isSuccess = false;

    if (isValidLoginDetails()) {
        var source = {
            'Email': $("#" + ctrlTxtLoginEmailId).val().trim(),
            'Password': $("#" + ctrlTxtLoginPasswordId).val().trim(),
            'CreateAuthTicket' : true
        };

        $.ajax({
            type: "POST",
            url: bwHostUrl + "/api/customer/authenticate/",
            data: JSON.stringify(source),
            contentType: 'application/json',
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
    sendForgetPwd();
});

function sendForgetPwd() {
    var objEmail = $("#txtForgotPassEmail");
    var email = objEmail.val();    

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


$(objRbtnSignup).click(function () {       

    return isValidRegDetails();

});

function isValidRegDetails() {

    reName = /^[a-zA-Z0-9'\- ]+$/;
    re = /^[0-9]*$/
    reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    isValid = true;
    regPass = /^[a-zA-Z]+$/;


    name = $(objRName).val().trim();
    email = $(objREmail).val().trim();
    pass = $(objRPass).val().trim();
    mobile = $(objRMobile).val().trim();
        
    if ($.trim(name) == "") {          
        toggleErrorMsg(objRName, true, "Please enter your Name");
        isValid = false;
    } else if (!reName.test($.trim(name))) {
        toggleErrorMsg(objRName, true,"Name should be Alphanumeric.");
        isValid = false;
    }
    else {
        toggleErrorMsg(objRName, false);
        objRegister.Name = name;
    }

    if ($.trim(email) == "") {
        toggleErrorMsg(objREmail, true, "Please enter your Email");
        isValid = false;
    } else if (!reEmail.test($.trim(email).toLowerCase())) {
        toggleErrorMsg(objREmail, true, "Invalid EmailId");
        isValid = false;
    } else {
        toggleErrorMsg(objREmail, false);
        objRegister.Email = email;
    }        


    if ($.trim(pass) == "") {
        toggleErrorMsg(objRPass, true, "Please enter password");
        isValid = false;
    } else if ($.trim(pass).length < 6) {
        toggleErrorMsg(objRPass, true, "Password should be atleast 6 characters long");
        isValid = false;
    } else if (regPass.test($.trim(pass))) {
        toggleErrorMsg(objRPass, true, "Password should contain atleast one number or special character.");
        isValid = false;
    }
    else {
        toggleErrorMsg(objRPass, false)
        objRegister.Password = pass;
    }

   
    if ($.trim(mobile) != "") {
        if (!re.test($.trim(mobile).toLowerCase())) {
            toggleErrorMsg(objRMobile, true, "Mobile No. should be numeric only");
            isValid = false;
        } else if (mobile.length < 10) {
            toggleErrorMsg(objRMobile, true, "Mobile no should be greater than 10 digits");
            isValid = false;
        } else {
            toggleErrorMsg(objRMobile, false);
            objRegister.Mobile = mobile;
        }
    }
    else {
        objRegister.Mobile = "";
    }

    if (!$(objRCheckbox).prop('checked'))
    {
        toggleErrorMsg(objRCheckbox, true, "You should agree to Bikewale terms and conditions");
        isValid = false;
    }
    else {
        toggleErrorMsg(objRCheckbox, false);
        objRegister.ClientIP = "";
    }

    return isValid;
}


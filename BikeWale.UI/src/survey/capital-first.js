var validate,
    isDesktop;

docReady(function () {
    validate = {
        setError: function (element, message) {
            var elementLength = element.val().length,
                errorTag = element.siblings('.error-text');

            errorTag.text(message).show();
            if (!elementLength) {
                element.closest('.input-box').removeClass('not-empty').addClass('invalid');
            }
            else {
                element.closest('.input-box').addClass('not-empty invalid');
            }
        },

        hideError: function (element) {
            element.closest('.input-box').removeClass('invalid').addClass('not-empty');
            element.siblings('.error-text').text('');
        },

        onFocus: function (inputField) {
            if (inputField.closest('.input-box').hasClass('invalid')) {
                validate.hideError(inputField);
            }
        },

        onBlur: function (inputField) {
            var inputLength = inputField.val().length;
            if (!inputLength) {
                inputField.closest('.input-box').removeClass('not-empty');
            }
            else {
                inputField.closest('.input-box').addClass('not-empty');
            }
        }
    };

    isDesktop = $(".capital-first-desktop");
    
    $("#cfDOB").Zebra_DatePicker({
        container : $("#cfDOB").closest(".input-box")
    });

    $(".page-tabs-data input, #otpNumber").on('blur', function () {
        validate.onBlur($(this));
    });
    $(".page-tabs-data input[type!=button], #otpNumber").on('focus', function () {
        validate.onFocus($(this));
        if (!isDesktop.length) {
            var offsetTop = $(this).offset();
            scrollTop(offsetTop);
        }
    });

    $("#personal-detail-submit").on('click', function () {
        validatePersonalInfo();
    });

    $("#employment-detail-submit").on('click', function () {
        validateEmploymentInfo();
    });

    $(".otp-container__edit-icon").on('click', function () {
        $(".otp-container__verification").hide();
        $(".otp-container__edit").show();
    });

    $("#saveNewNumber").on('click', function () {
        var otpNewNum = $("#otpNewNumber");
        if (validatePhoneNumber(otpNewNum)) {
            $(".otp-container__verification").show();
            $(".otp-container__edit").hide();
            var newNum = $(otpNewNum).val();
            $(".otp-container__phone-number").text(newNum);
            $(otpNewNum).val('');
        }
    });

    $(".otp-container__close").on('click', function () {
        $(".otp-container").hide();
    });
   
});

function scrollTopError() {
    var elem = $(".invalid").offset();
    scrollTop(elem);
}

function validatePersonalInfo() {
    var isValid = false;

    isValid = validateUserName($("#cfFName"));
    isValid &= validateUserName($("#cfLName"));
    isValid &= validatePhoneNumber($("#cfNum"));
    isValid &= validateEmailId($("#cfEmail"));
    isValid &= validateAddress($("#cfAddress1"));
    isValid &= validateAddress($("#cfAddress2"));
    isValid &= validatePinCode($("#cfPincode"));
    isValid &= validatePanNumber($("#cfPan"));
    isValid &= validateRadioButtons("gender");
    isValid &= validateRadioButtons("marital");

    if (isValid) {
        $("#personal-detail-tab").addClass("hide");
        $("#employment-detail-tab").removeClass("hide");

        savePersonalDetails();
        $(".personal-image-unit").removeClass('personal-icon').addClass('white-tick-icon');
        $(".employment-image-unit").removeClass('gray-bag-icon').addClass('white-bag-icon');
        if (isDesktop) {
            $(".employment__title ").removeClass("inactive");
            $(".employment-details-container").addClass("visible");
            scrollTop($("#employment-detail-tab").offset());
        }
    } else {
        scrollTopError();
    }
}
function savePersonalDetails()
{
    var personDetails = {
        "firstName":$('#cfFName').val(),
        "lastName":$('#cfLName').val(),
        "mobileNumber":$('#cfNum').val(),
        "emailId":$('#cfEmail').val(),
        "dateOfBirth": $('#cfDOB').val(),
        "gender": $('#cfGenderM').is(':checked') ? 1 : 2,
        "maritalStatus": $('#cfMaritalS').is(':checked') ? 1 : 2,
        "addressLine1": $("#cfAddress1").val(),
        "addressLine2": $('#cfAddress2').val(),
        "pincode": $("#cfPincode").val(),
        "pancard": $("#cfPan").val()

    }

    $.ajax({
        type: "POST",
        url: "/api/finance/savepersonaldetails/",
        contentType: "application/json",
        data: ko.toJSON(personDetails),
        success: function (response) {
            Materialize.toast('Desktop banner configured', 4000);
            $('.stepper').nextStep();
        }
    });


}

function validateEmploymentInfo() {
    var isValid = false;

    isValid = validateUserName($("#cfCompName"));
    isValid &= validateIncome($("#cfCompIncome"));
    isValid &= validateAddress($("#cfCompAddress1"));
    isValid &= validateAddress($("#cfCompAddress2"));
    isValid &= validatePinCode($("#cfCompPincode"));
    isValid &= validateRadioButtons("status");

    if (isValid) {
        saveEmployeDetails();
    }
}
function saveEmployeDetails() {

    var employeDetails = {
        "status": $('#cfStatusS').is(':checked') ? 1 : 2,
        "companyName": $('#cfCompName').val(),
        "officalAddressLine1": $('#cfCompAddress1').val(),
        "officalAddressLine2": $('#cfCompAddress2').val(),
        "pincode": $('#cfCompPincode').val(),
        "annualIncome": $('#cfCompIncome').val()

    }

    $.ajax({
        type: "POST",
        url: "/api/finance/saveemployedetails/",
        contentType: "application/json",
        data: ko.toJSON(employeDetails),
        success: function (response) {
            Materialize.toast('Desktop banner configured', 4000);
            $('.stepper').nextStep();
        }
    });


}
function validateUserName(elem) {

    var nameRegex = /^[a-zA-Z ]{2,255}$/,
        value = $(elem)[0].value.trim();

    if (nameRegex.test(value) && value.length > 1) {
        validate.hideError(elem);
        isValid = true;
    }
    else {
        validate.setError(elem, "Error");
        isValid = false;
    }
    return isValid;
}

function validatePhoneNumber(inputMobile) {
    var regMob = new RegExp('^((7)|(8)|(9))[0-9]{9}$', 'i'),
        value = $(inputMobile).val();

    if(value.length < 10){
        validate.setError($(inputMobile), "Enter 10 digits");
        isValid = false;
    }
    else if (!regMob.test(value)) {
        validate.setError($(inputMobile), "Mobile Number should start with only 7, 8 or 9");
        isValid = false;
    } else {
        validate.hideError($(inputMobile));
        isValid = true;
    }
    return isValid;
}

function validateEmailId(inputEmail) {
    var isValid = true,
        emailVal = $(inputEmail).val(),
        reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailVal == "") {
        validate.setError($(inputEmail), 'Please enter email id');
        isValid = false;
    }
    else if (!reEmail.test(emailVal)) {
        validate.setError($(inputEmail), 'Invalid Email');
        isValid = false;
    }
    return isValid;
}

function validateAddress(inputAddress) {
    var value = $(inputAddress)[0].value.trim();

    if (value.length > 1) {
        validate.hideError(inputAddress);
        isValid = true;
    }
    else {
        validate.setError(inputAddress, "Error");
        isValid = false;
    }
    return isValid;
}

function validatePinCode(inputPincode) {
    var isValid = true,
        pc = inputPincode.val().trim();

    if (!(/^[1-9][0-9]{5}$/.test(pc))) {
        validate.setError(inputPincode, 'Invalid pincode');
        isValid = false;
    }
    return isValid;
}

function validatePanNumber(inputPanNum) {
    var isValid = true,
        panNum = inputPanNum.val().trim();

    if (!(/([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}/.test(panNum)) && panNum.length < 6) {
        validate.setError(inputPanNum, 'Invalid Pan Number');
        isValid = false;
    }
    return isValid;
}

function validateRadioButtons(groupName) {
    var isValid = true;
    if ($('input[name=' + groupName + ']:checked').length <= 0) {
        validate.setError($('input[name=' + groupName + ']').closest('ul'), 'Please select');
        isValid = false;
    } else {
        validate.hideError($('input[name=' + groupName + ']').closest('ul'));
        isValid = true;
    }
    return isValid;
}

function validateIncome(inputIncome) {
    var isValid = true;

    if ($(inputIncome).val().length <= 0) {
        validate.setError(inputIncome, 'Invalid Income');
        isValid = false;
    }
    return isValid;
}

function scrollTop(offsetElem) {
    var offsetTop = 40;
    if (isDesktop.length) {
        offsetTop = 170;

    }
    $("html, body").animate({
        scrollTop: offsetElem.top - offsetTop
    });
}
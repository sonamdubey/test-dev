
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
    
    $("#cfDOB").Zebra_DatePicker();

    $(".page-tabs-data input").on('blur', function () {
        validate.onBlur($(this));
    });
    $(".page-tabs-data input").on('focus', function () {
        validate.onFocus($(this));
    });

    $("#personal-detail-submit").on('click', function () {
        validatePersonalInfo();
    });

    $("#employment-detail-submit").on('click', function () {
        validateEmploymentInfo();
    });
   
});

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
    }
}

function validateEmploymentInfo() {
    var isValid = false;

    isValid = validateUserName($("#cfCompName"));
    isValid &= validatePhoneNumber($("#cfCompIncome"));
    isValid &= validateAddress($("#cfCompAddress1"));
    isValid &= validateAddress($("#cfCompAddress2"));
    isValid &= validatePinCode($("#cfCompPincode"));
    isValid &= validateRadioButtons("status");

    if (isValid) {
        //submit
    }
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
    var regMob = new RegExp('^((7)|(8))[0-9]{9}$', 'i'),
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
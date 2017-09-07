
docReady(function () {

    validate = {
        setError: function (element, message) {
            var elementLength = element.val().length,
                errorTag = element.siblings('span.error-text');

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
            element.siblings('span.error-text').text('');
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


    var pageTabElem = $(".page-tabs__li");

    $(pageTabElem).on('click', function () {
        if (!$(this).hasClass('active')) {
            $(pageTabElem).removeClass('active');
            $(this).addClass('active');
        }
    });
    //var newScript = $(document.createElement('script'));
    //newScript.src = "/src/zebra-datepicker.js";

    
    $("#cfDOB").Zebra_DatePicker();

    $("#cfFName, #cfLName, #cfNum, #cfEmail, #cfAddress1, #cfAddress2, #cfPincode, #cfPan").on('blur', function () {
        validate.onBlur($(this));
    });
    $("#cfFName, #cfLName, #cfNum, #cfEmail, #cfAddress1, #cfAddress2, #cfPincode, #cfPan").on('focus', function () {
        validate.onFocus($(this));
    });

    $("#personal-detail-submit").on('click', function () {
        validatePersonalInfo();
    });
   
});

function validatePersonalInfo() {
    var isValid = false;

    isValid = validateUserName($("#cfFName"));
    isValid = validateUserName($("#cfLName"));
    isvalid = validatePhoneNumber($("#cfNum"));
    isvalid = validateEmailId($("#cfEmail"));
}

function validateUserName(elem) {
    var value = $(elem)[0].value.trim();

    if (value.length > 1) {
        validate.hideError(elem);
        isValid = true;
    } else {
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
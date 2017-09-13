var validate,
    otpScreen,
    docBody,
    isDesktop,
    blackWindowElem,
    otpPhoneInput,
    otpEditElem,
    otpVerificationElem,
    otpContainerElem,
    otpNewNum;

docReady(function () {

    otpPhoneInput = $(".otp-container__phone-number");
    otpNewNum = $("#otpNewNumber");
    otpEditElem = $(".otp-container__edit");
    otpVerificationElem = $(".otp-container__verification");
    otpContainerElem = $(".otp-container")
    blackWindowElem = $(".otp-black-window");

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

    docBody = {
        lockScroll: function () {
            var html_el = $('html'), body_el = $('body'), doc = $(document);
            showBlackWindow();
            if (doc.height() > $(window).height()) {
                var scrollTop = (html_el.scrollTop()) ? html_el.scrollTop() : body_el.scrollTop(); // Works for Chrome, Firefox, IE...
                if (scrollTop < 0) { scrollTop = 0; }
                html_el.addClass('lock-browser-scroll').css('top', -scrollTop);
            }
        },
        unlockScroll: function () {
            var scrollTop = parseInt($('html').css('top'));
            hideBlackWindow();
            $('html').removeClass('lock-browser-scroll');
            $('html,body').scrollTop(-scrollTop);
        }
    };

    otpScreen = {
        openOtp: function() {
            $(otpContainerElem).show();
            docBody.lockScroll();
        },
        closeOtp: function() {
            $(otpContainerElem).hide();
            docBody.unlockScroll();
        }
    };

    isDesktop = $(".capital-first-desktop");
    
    $("#cfDOB").Zebra_DatePicker({
        container : $("#cfDOB").closest(".input-box")
    });

    $(".page-tabs-data input, .otp-container input[type!=button]").on('blur', function () {
        validate.onBlur($(this));
    });

    $(".page-tabs-data input[type!=button], .otp-container input[type!=button]").on('focus', function () {
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

    FillDummyDetails();
   
    $(".otp-container__edit-icon").on('click', function () {
        var editPhone = $(otpPhoneInput).text();
        $(otpVerificationElem).hide();
        $(otpEditElem).show();
        $(otpNewNum).val(editPhone).trigger('focus');
    });

    $("#saveNewNumber").on('click', function () {
        if (validatePhoneNumber(otpNewNum)) {
            $(otpVerificationElem).show();
            $(otpEditElem).hide();
            var newNum = $(otpNewNum).val();
            $(otpPhoneInput).text(newNum);
            $(otpNewNum).val('');
        }
    });

    $(".otp-container__close").on('click', function () {
        otpScreen.closeOtp();
    });

    $(blackWindowElem).on('click', function () {
        otpScreen.closeOtp();
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
    isValid &= validateDOB($("#cfDOB"));

    if (isValid) {
       

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
        "objLeadJson": $("#objLead").val(),
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
            $("#personal-detail-tab").addClass("hide");
            $("#employment-detail-tab").removeClass("hide");
          
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
        "OfficialAddressLine1": $('#cfCompAddress1').val(),
        "OfficialAddressLine2": $('#cfCompAddress2').val(),
        "pincode": $('#cfCompPincode').val(),
        "annualIncome": $('#cfCompIncome').val(),
        "mobileNumber": $('#cfNum').val(),
        "emailId": $('#cfEmail').val(),

    }

    $.ajax({
        type: "POST",
        url: "/api/finance/saveemployedetails/",
        contentType: "application/json",
        data: ko.toJSON(employeDetails),
        success: function (response) {
            
            if (response == "Not Registered Mobile Number")
            {
                otpScreen.openOtp();
                var objData={
                    "userName": $('#cfFName').val() + +$('#cfLName').val(),
                    "mobileNumber": $('#cfNum').val()
                }
                otpvm.setParameters(objData);
            }
            else if (response == "Registered Mobile Number")
            {
                otpScreen.closeOtp();

            }
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

function validateDOB(inputAge) {
    var isValid = true,
        setDate = $(inputAge).val(),
        date1 = new Date(setDate),
        date2 = new Date(),
        timeDiff = Math.abs(date2.getTime() - date1.getTime()),
        diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)),
        diffYears = diffDays / 365;

    if (diffYears < 21) {
        validate.setError(inputAge, 'Age should be greater than 21');
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

function showBlackWindow() {
    $(blackWindowElem).show()
}

function hideBlackWindow() {
    $(blackWindowElem).hide()
    }
    
function FillDummyDetails() {
    $('#cfFName').val('John');
    $('#cfLName').val('doe');
    $('#cfNum').val('9892112233');
    $('#cfEmail').val('ss@ss.com');
    $('#cfDOB').val('1993-02-02');
    $('#cfGenderM').prop("checked", true);
    $('#cfMaritalS').prop("checked", true);
    $("#cfAddress1").val('Mannat, Shop No 3, Juhu Tara Road');
    $('#cfAddress2').val('Santacruz West, Mumbai - 400054');
    $("#cfPincode").val('400605');
    $("#cfPan").val('AIJPN5455N');
}
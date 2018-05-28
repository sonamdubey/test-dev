var validate,
    otpScreen,
    docBody,
    isDesktop,
    blackWindowElem,
    otpPhoneInput,
    otpEditElem,
    otpVerificationElem,
    otpContainerElem,
    otpNewNum,
    otpContainerContent,
    otpContainerContentHeight,
    employmentDeatilTab,
    personalDetailTab,
        bikeName,
        formTabsContainer;
var objPinCodes = new Object();
docReady(function () {


    otpPhoneInput = $(".otp-container__phone-number");
    otpNewNum = $("#otpNewNumber");
    otpEditElem = $(".otp-container__edit");
    otpVerificationElem = $(".otp-container__verification");
    otpContainerElem = $(".otp-container")
    blackWindowElem = $(".otp-black-window"),
    otpContainerContent = $(".otp-container__content"),
    employmentDeatilTab = $("#employment-detail-tab");
    personalDetailTab = $("#personal-detail-tab");
    bikeName = $('#hdnBikeName').val();
    formTabsContainer = $('#form-tabs-content');
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
        openOtp: function () {
            $(otpContainerElem).show();
            otpContainerContentHeight = $(otpContainerContent).innerHeight();
            docBody.lockScroll();
        },
        closeOtp: function () {
            $(otpContainerElem).hide();
            docBody.unlockScroll();
        }
    };

    $('input:radio[name="gender"]').change(
    function () {
            validateRadioButtons("gender");
    });

    $('input:radio[name="marital"]').change(
    function () {
            validateRadioButtons("marital");
	});
	
	// register modal popup events
	modalPopup.registerDomEvents();

	isDesktop = $(".capital-first-desktop");
	
	var startDate = new Date();
	startDate.setFullYear(startDate.getFullYear() - 21);
	
	var startMonth = (startDate.getMonth() + 1) < 10 ? '0' + (startDate.getMonth() + 1) : (startDate.getMonth() + 1);

	var pickerEndDate = startDate.getFullYear() + '-' + startMonth + '-' + startDate.getDate();
	
    $("#cfDOB").Zebra_DatePicker({
        container: $("#cfDOB").closest(".input-box"),
		view: 'years',
		start_date: pickerEndDate,
		direction: ['1900-01-01', pickerEndDate]
	});
	
    var dateOfBirthPicker = $('#cfDOB').data('Zebra_DatePicker');
    if (dateOfBirthPicker != null)
    {
        dateOfBirthPicker.datepicker.find('.dp_heading').text('Date of Birth');
    }
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
    $("#contact-detail-submit").on('click', function () {
        validateContactInfo();
    });

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

    
    $("#otpNumber, #otpNewNumber").on('focus', function () {
        $(otpContainerElem).animate({
            scrollTop: otpContainerContentHeight + 30
        });
    });

    $("#cfPincode").on('focus', function () {
        $.fn.hint = bwHint;
        $.fn.bw_autocomplete = bwAutoComplete;
        $("#cfPincode").bw_autocomplete({
            source: 6,
            recordCount: 3,
            minLength: 2,
            onClear: function () {
                objPinCodes = new Object();
            },
            click: function (event, ui, orgTxt) {
                if (ui && ui.item) {
                    $('#cfPincode').closest('.input-box').addClass('not-empty');
                    $('#cfPincode').val(ui.item.payload.pinCode);
                }
                else {
                    $('#cfPincode').val();
                }

            },
            open: function (result) {
                objPinCodes.result = result;
            },
            focusout: function () {
                if ($('#cfPincode').find('li.ui-state-focus a:visible').text() != "") {
                    $('#errPinCodeSearch,#errPinCodeSearch_office').hide();
                    focusedMakeModel = new Object();
                    focusedMakeModel = objPinCodes.result ? objPinCodes.result[$('li.ui-state-focus').index()] : null;
                }
                else {
                    $('#errPinCodeSearch,#errPinCodeSearch_office').hide();
                }
            },
            afterfetch: function (result, searchtext) {
                if (result != undefined && result.length > 0 && searchtext.trim() != "") {
                    $('#errPinCodeSearch,#errPinCodeSearch_office').hide();
                }
                else {
                    focusedMakeModel = null;
                    if (searchtext.trim() != "") {
                        $('#errPinCodeSearch,#errPinCodeSearch_office').show();

                    }
                }
            },
            keyup: function () {
                if ($(event.target).val().trim() != '' && $('li.ui-state-focus a:visible').text() != "") {
                    focusedMakeModel = new Object();
                    focusedMakeModel = objPinCodes.result ? objPinCodes.result[$('li.ui-state-focus').index()] : null;
                    $('#errPinCodeSearch,#errPinCodeSearch_office').hide();
                } else {
                    if ($(event.target).val().trim() == '') {
                        $('#errPinCodeSearch,#errPinCodeSearch_office').hide();
                    }
                }

                if ($(event.target).val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
                    if (focusedMakeModel == null || focusedMakeModel == undefined) {
                        if ($(event.target).val().trim() != '') {
                            $('#errPinCodeSearch,#errPinCodeSearch_office').show();
                            $('#cfPincode').val();
                        }
                    }
                    else {
                        $('#errPinCodeSearch,#errPinCodeSearch_office').hide();
                    }

                }
            }
        }).autocomplete({ appendTo: $(event.target).closest(".input-box") }).autocomplete("widget").addClass("pincode-autocomplete");
    });


});

var self = this;

function scrollTopError() {
    var elem = $($(".invalid")[0]).offset();
    scrollTop(elem);
}

function validateContactInfo() {
    var panStatus = $("#panStatus").val();
    $('#screenLoader').show();
    var isValid = validateUserName($("#cfFName"));
    isValid &= validateUserName($("#cfLName"));
    isValid &= validatePhoneNumber($("#cfNum"));
    isValid &= validateEmailId($("#cfEmail"));
    isValid &= validatePinCode($("#cfPincode"));
    if (panStatus)
    {
        isValid &= validatePanNumber($("#cfPan"));
    }

    if (isValid) {

        saveContactDetails();

        
    }
    else {
        scrollTopError();
    }

    $('#screenLoader').hide();

}

function saveContactDetails()
{
    var contactDetails = {
        "objLeadJson": $("#objLead").val(),
        "firstName": $('#cfFName').val(),
        "lastName": $('#cfLName').val(),
        "mobileNumber": $('#cfNum').val(),
        "emailId": $('#cfEmail').val(),
        "pincode": $("#cfPincode").val().substring(0, 6),
        "pancard": $("#cfPan").val(),
        "id": $("#cpId").val(),
        "ctLeadId": $("#ctLeadId").val(),
        "leadId": $("#leadId").val()
    };
    $.ajax({
        type: "POST",
        url: "/api/finance/savepersonaldetails/?source=" + $("#hdnPlatform").val(),
        contentType: "application/json",
        data: ko.toJSON(contactDetails),
        beforeSend: function () {
            $('#otpLoader').show();
        },
        success: function (response) {
            if (response) {
                if (response != null) {
                    triggerGA('Loan_Application', 'Step_1_Filled', bikeName + '_' + $('#cfNum').val());
                    switch (response.status) {
                        case 1:
                            $("#cpId").val(response.cpId);
                            $("#ctLeadId").val(response.ctLeadId);
                            $("#leadId").val(response.leadId);
                            otpScreen.openOtp();
                            var objData = {
                                "userName": $('#cfFName').val() + " " + $('#cfLName').val(),
                                "mobileNumber": $('#cfNum').val()
                            }
                            otpvm.setParameters(objData);
                            break;
                        case 6:                            
                            triggerGA('Loan_Application', 'OTP_Success', bikeName + '_' + $('#cfNum').val());
                            $('.otp-container__info').hide();
                            $('#thankyouScreen').removeClass("hide");
                            otpScreen.openOtp();
                            break;
                        default:
                            var obj = {
                                message: response.message,
                                isYesButtonActive: true,
                                yesButtonText: "Okay",
                                yesButtonLink: "javascript:void(0)"
                            };

                            $('.otp-container__info').hide();
                            $('#thankyouScreen').removeClass("hide");

                            modalPopup.showModal(templates.modalPopupTemplate(obj));
                    }
                }
            }
        },
        complete: function () {
            $('#otpLoader').hide();
        },
        error: function () {
            var obj = {
                message: navigator.onLine ? "Some error has occured." : "You're offline. Please check your internet connection.",
                isYesButtonActive: true,
                yesButtonText: "Okay",
                yesButtonLink: "javascript:void(0)"
            };
            modalPopup.showModal(templates.modalPopupTemplate(obj));
        }
    });


}


function validateUserName(elem) {
    var isValid;
    var nameRegex = /^[a-zA-Z]{2,255}$/,
        value = $(elem)[0].value.trim();
    if (value.length == 0) {
        validate.setError(elem, "Please enter required field");
        isValid = false;
    }
    else if (nameRegex.test(value) && value.length > 1) {
        validate.hideError(elem);
        isValid = true;
    }
    else {
        validate.setError(elem, "Please enter valid name");
        isValid = false;
    }
    return isValid;
}

function validatePhoneNumber(inputMobile) {
    var isValid;
    var regMob = new RegExp('^((7)|(8)|(9))[0-9]{9}$', 'i'),
        value = $(inputMobile).val();
    if (value.length == 0) {
        validate.setError($(inputMobile), "Please enter Mobile number");
        isValid = false;
    }
    if (value.length < 10) {
        validate.setError($(inputMobile), "Please enter 10 digits");
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
        validate.setError($(inputEmail), 'Please enter Email Id');
        isValid = false;
    }
    else if (!reEmail.test(emailVal)) {
        validate.setError($(inputEmail), 'Please enter valid Email Id');
        isValid = false;
    }
    return isValid;
}

function validateAddress(inputAddress) {
    var isValid = true,  addressVal =  $(inputAddress)[0].value.trim(),
      regex = /^[A-z0-9 #\/()-.,:\[\]]*$/;
 
    if (addressVal == "")
    {
        validate.setError(inputAddress, "Please enter Address");
        isValid = false;
    }
    else if (!regex.test(addressVal)) {
        validate.setError(inputAddress, "Please enter valid Address");
        isValid = false;
    }
  
    return isValid;
}


function checkPinCode(pinCode, inputPincode) {
    var isValid = false;
    $.ajax({
        async: false,
        type: "GET",
        url: "/api/autosuggest/?source=6&inputText=" + pinCode + "&noofrecords=5",
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (data && data.suggestionList.length > 0) {
                $(inputPincode).val(data.suggestionList[0].text);
                isValid = true;
            }
            else {
                validate.setError($(inputPincode), 'We do not serve in this area');
                isValid = false;
            }
        }
    });
    return isValid;
};


function validatePinCode(inputPincode) {
    var  isValid = true,
               pinCodeValue = inputPincode.val().trim(),
               rePinCode = /^[1-9][0-9]{5}$/;

    if (pinCodeValue.indexOf(',') > 0)
        pinCodeValue = pinCodeValue.substring(0, 6);

    if (pinCodeValue == "") {
        validate.setError(inputPincode, 'Please enter pincode');
        isValid = false;
    }
    else if (!rePinCode.test(pinCodeValue)) {
        validate.setError(inputPincode, 'Invalid pincode');
        isValid = false;
    }

    if (isValid && inputPincode == '#cfPincode') isValid &= checkPinCode(pinCodeValue, inputPincode);

    return isValid;
}

function validatePanNumber(inputPanNum) {
    var isValid = true,
        panNum = inputPanNum.val().trim();
    if (panNum.length == 0) {
        validate.setError(inputPanNum, 'Please enter Pan Number');
        isValid = false;
    }
    else if (!(/[a-zA-Z]{3}[P|p][a-zA-Z]{1}[0-9]{4}[a-zA-Z]{1}$/.test(panNum))) {
        validate.setError(inputPanNum, 'Please enter valid Pan Number');
        isValid = false;
    }
    return isValid;
}

function validateRadioButtons(groupName) {
    var isValid = true;
    if ($('input[name=' + groupName + ']:checked').length <= 0) {
        validate.setError($('input[name=' + groupName + ']').closest('ul'), 'Please select required field');
        isValid = false;
    } else {
        validate.hideError($('input[name=' + groupName + ']').closest('ul'));
        isValid = true;
    }
    return isValid;
}

function validateIncome(inputIncome) {
    var isValid = true;
    var numRegex = /^[0-9]{0,9}$/;
    var value = $(inputIncome).val().trim();
    if ($(inputIncome).val().length <= 0) {
        validate.setError(inputIncome, 'Please enter Income');
        isValid = false;
    }
    else if (!numRegex.test(value) || value.length > 9) {
        validate.setError(inputIncome, 'Please enter valid Income');
        isValid = false;
    }
    return isValid;
}

function validateDOB(inputAge) {
    var dob = $(inputAge).val().trim();

        var isValid = true,
            setDate = $(inputAge).val(),
            date1 = new Date(setDate),
            date2 = new Date(),
            timeDiff = Math.abs(date2.getTime() - date1.getTime()),
            diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)),
            diffYears = diffDays / 365;
      if (dob.length == 0) {
          validate.setError(inputAge, 'Please enter Age');
          isValid = false;
        }
    else if (diffYears < 21) {
        validate.setError(inputAge, 'Age should be greater than 21');
        isValid = false;
    }
    return isValid;
}

function scrollTop(offsetElem) {
    var offsetTop = 25;
    if (isDesktop.length) {
        offsetTop = 130;

    }
    $("html, body").animate({
        scrollTop: offsetElem.top - offsetTop
    });
}

function showBlackWindow() {
    $(blackWindowElem).show();
}

function hideBlackWindow() {
    $(blackWindowElem).hide();
}

var templates = {
	modalPopupTemplate: function(obj) {
		var template = '';

		template += '<span class="modal__close bwsprite bwmsprite cross-default-15x16"></span>';
		if (obj.message!=null && obj.message.length > 0) {
			template += '<p class="modal__description">' + obj.message + '</p>';
		}
		if (obj.isYesButtonActive) {
			template += '<a href="' + obj.yesButtonLink + '" class="btn btn-orange btn-124-36 modal__close">' + obj.yesButtonText + '</a>';
		}

		return template;
	}
};

var modalPopup = {
	registerDomEvents: function() {
		$(document).on('click', '.modal-box .modal__close', function () {
			modalPopup.closeActiveModalPopup();
		});
	},

	showModal: function(htmlString, modalBox) {
		modalBox = modalBox || $('#modalPopUp');
		$('#modalBg').show();
		modalBox.html(htmlString).show();
		modalPopup.lockScroll();
	},

	hideModal: function(modalBox) {
		modalBox = modalBox || $('#modalPopUp');
		$('#modalBg').hide();
		modalBox.html('').hide();
		modalPopup.unlockScroll();
	},

	lockScroll: function() {
		var html_el = $('html'),
			body_el = $('body');

		if ($(document).height() > $(window).height()) {
			var scrollTop = (html_el.scrollTop()) ? html_el.scrollTop() : body_el.scrollTop();

			if (scrollTop < 0) {
				scrollTop = 0;
			}

			html_el.addClass('lock-browser-scroll').css('top', -scrollTop);
		}
	},

	unlockScroll: function() {
		var scrollTop = parseInt($('html').css('top'));

		$('html').removeClass('lock-browser-scroll');
		$('html, body').scrollTop(-scrollTop);
	},

	isVisible: function(modalBox) {
		modalBox = modalBox || $('#modalPopUp');
		return modalBox.is(':visible');
	},

	closeActiveModalPopup: function() {
		if (modalPopup.isVisible()) {
			modalPopup.hideModal();
			return true;
		}
		return false;
	}
};
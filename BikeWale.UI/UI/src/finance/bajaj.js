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
    formTabsContainer,
    companyId = 0,
	pincodeId = 0,
	versionId = 0,
	bajajAutoId,
	leadId = 0,
	leadObject,
	redirectPageUrl,
    isStep1Done = false,
    isStep2Done = false,
    isFormSubmitted = false;

var empTypeEnum = {
    Salaried: 1,
    Self_Employed_Business:2,
    Self_Employed_Professional: 2,
    Retired: 4,
    Student: 4,
    Homemaker:4
};
var objPinCodes = new Object(), objCompanies = new Object();



docReady(function () {

	redirectPageUrl =  $('#pageUrl').val();
	otpPhoneInput = $(".otp-container__phone-number");
	otpNewNum = $("#otpNewNumber");
	otpEditElem = $(".otp-container__edit");
	otpVerificationElem = $(".otp-container__verification");
	otpContainerElem = $(".otp-container")
	blackWindowElem = $(".otp-black-window"),
    otpContainerContent = $(".otp-container__content"),
    employmentDeatilTab = $("#other-detail-tab");
	personalDetailTab = $("#employee-detail-tab");
	bikeName = $('#hdnBikeName').val();
	formTabsContainer = $('#form-tabs-content');
	leadObject = JSON.parse($("#objLead").val());
	leadObject.deviceId = getCookie('BWC');
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
			var elementLength = element.val().length;

			element.closest('.input-box').removeClass('invalid');
			if (elementLength) {
				element.closest('.input-box').addClass('not-empty')
			}
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
		},
		hideAllError: function (multipleElement) {
		    var elementArray = multipleElement.split(",");
		    $.each(elementArray, function (i) {
		        if(elementArray[i].trim() != "")
		            validate.hideError($(elementArray[i]));
		    });
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
    	basicInfo.validateRadioButtons("gender");
    });

	$('input:radio[name="marital"]').change(
    function () {
    	basicInfo.validateRadioButtons("marital");
    });

	// register modal popup events
	modalPopup.registerDomEvents();

	isDesktop = $(".finance-desktop");

	// date of birth
	var startDate = new Date();
	startDate.setFullYear(startDate.getFullYear() - 18);
	var startMonth = (startDate.getMonth() + 1) < 10 ? '0' + (startDate.getMonth() + 1) : (startDate.getMonth() + 1);
	var startDay = startDate.getDate() < 10 ? '0' + startDate.getDate() : startDate.getDate();

	var pickerEndDate = startDate.getFullYear() + '-' + startMonth + '-' + startDay;

	$("#financeDOB").Zebra_DatePicker({
		container: $("#financeDOB").closest(".input-box"),
		view: 'years',
		start_date: pickerEndDate,
		direction: ['1900-01-01', pickerEndDate],
		onClose: function() {
			$("#financeDOB").blur()
		}
	});

	var dateOfBirthPicker = $('#financeDOB').data('Zebra_DatePicker');
	dateOfBirthPicker.datepicker.find('.dp_heading').text('Date of Birth');

	// purchase date
	$("#financeDOP").Zebra_DatePicker({
		container: $("#financeDOP").closest(".input-box"),
		direction: 1
	});

	var dateOfPurchasePicker = $('#financeDOP').data('Zebra_DatePicker');
	dateOfPurchasePicker.datepicker.find('.dp_heading').text('Likely purchase date of bike');

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
	$("#basic-detail-submit").on('click', function () {
		validateBasicInfo();
	});

	$("#employment-detail-submit").on('click', function () {
		validateEmployeeInfo();
	});

	$("#other-detail-submit").on('click', function () {
		validateOtherInfo();
	});



    $('.step1').on('click', function () {
        if(isStep1Done){
			showStep1();
			isStep2Done = false;
        }
    });

    $('.step2').on('click', function () {
        if (isStep2Done) {
            showStep2();
        }
    });


	$(".otp-container__edit-icon").on('click', function () {
		var editPhone = $(otpPhoneInput).text();
		$(otpVerificationElem).hide();
		$(otpEditElem).show();
		$(otpNewNum).val(editPhone).trigger('focus');
	});

	$("#saveNewNumber").on('click', function () {
		if (basicInfo.validatePhoneNumber(otpNewNum)) {
			$(otpVerificationElem).show();
			$(otpEditElem).hide();
		}
	});

	$(".otp-container__close").on('click', function () {
		otpScreen.closeOtp();
	});

	$(blackWindowElem).on('click', function () {
		if(!$('#thankyouScreen').hasClass('hide'))
			window.location.href = redirectPageUrl;
		otpScreen.closeOtp();
	});


	$("#otpNumber, #otpNewNumber").on('focus', function () {
		$(otpContainerElem).animate({
			scrollTop: otpContainerContentHeight + 30
		});
	});


	$("#financePincode").on('focus', function () {
		$.fn.hint = bwHint;
		$.fn.bw_autocomplete = bwAutoComplete;
		$("#financePincode").bw_autocomplete({
			source: 6,
			recordCount: 3,
			minLength: 2,
			onClear: function () {
			    objPinCodes = new Object();
			    pincodeId = 0;

			},
			click: function (event, ui, orgTxt) {
				if (ui && ui.item) {
					$('#financePincode').closest('.input-box').addClass('not-empty');
					$('#financePincode').val(ui.item.payload.pinCode);
					pincodeId = ui.item.payload.pinCodeId;
				}
				else {
					$('#financePincode').val();
				}

			},
			open: function (result) {
				objPinCodes.result = result;
			},
			focusout: function () {
				if ($('#financePincode').find('li.ui-state-focus a:visible').text() != "") {
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
							$('#financePincode').val();
						}
					}
					else {
						$('#errPinCodeSearch,#errPinCodeSearch_office').hide();
					}

				}
			}
		}).autocomplete({ appendTo: $(event.target).closest(".input-box") }).autocomplete("widget").addClass("pincode-autocomplete");
	});



	$("#financeCompanyName").on('focus', function () {
	    $.fn.hint = bwHint;
	    $.fn.bw_autocomplete = bwAutoComplete;
	    $("#financeCompanyName").bw_autocomplete({
	        source: 9,
	        recordCount: 5,
	        minLength: 3,
	        onClear: function () {
                objCompanies = new Object();
                companyId = 0;
	        },
	        click: function (event, ui, orgTxt) {
	            if (ui && ui.item) {
	                $('#financeCompanyName').closest('.input-box').addClass('not-empty');
                    $('#financeCompanyName').val(ui.item.payload.companyname);
                    companyId = ui.item.payload.value
	            }
	            else {
	                $('#financeCompanyName').val();
	            }

	        },
	        open: function (result) {
	            objCompanies.result = result;
	        },
	        focusout: function () {
	            if ($('#financeCompanyName').find('li.ui-state-focus a:visible').text() != "") {
	                $('#errCompanyNameSearch').hide();
	                focusedMakeModel = new Object();
	                focusedMakeModel = objCompanies.result ? objCompanies.result[$('li.ui-state-focus').index()] : null;
	            }
	            else {
	                $('#errCompanyNameSearch').hide();
	            }
	        },
	        afterfetch: function (result, searchtext) {
	            if (result != undefined && result.length > 0 && searchtext.trim() != "") {
	                $('#errCompanyNameSearch').hide();
	            }
	            else {
	                focusedMakeModel = null;
	                if (searchtext.trim() != "") {
	                    $('#errCompanyNameSearch').show();

	                }
	            }
	        },
	        keyup: function () {
	            if ($(event.target).val().trim() != '' && $('li.ui-state-focus a:visible').text() != "") {
	                focusedMakeModel = new Object();
	                focusedMakeModel = objPinCodes.result ? objPinCodes.result[$('li.ui-state-focus').index()] : null;
	                $('#errCompanyNameSearch').hide();
	            } else {
	                if ($(event.target).val().trim() == '') {
	                    $('#errCompanyNameSearch').hide();
	                }
	            }

	            if ($(event.target).val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
	                if (focusedMakeModel == null || focusedMakeModel == undefined) {
	                    if ($(event.target).val().trim() != '') {
	                        $('#errCompanyNameSearch').show();
	                        $('#financeCompanyName').val();
	                    }
	                }
	                else {
	                    $('#errCompanyNameSearch').hide();
	                }

	            }
	        }
	    }).autocomplete({ appendTo: $(event.target).closest(".input-box") }).autocomplete("widget").addClass("company-autocomplete");
	});


});

var self = this;

function scrollTopError() {
	var elem = $($(".invalid")[0]).offset();
	scrollTop(elem);
}

function validateBasicInfo() {
    
        $('#screenLoader').show();
        var isValid = basicInfo.validateTitleBox($("#financeTitle"));
        isValid &= basicInfo.validateUserName($('#financeFname'));
        isValid &= basicInfo.validateUserName($("#financeLName"));
        isValid &= basicInfo.validateRadioButtons("gender");
        isValid &= basicInfo.validatePhoneNumber($("#financeNum"));
        isValid &= basicInfo.validateDOB($("#financeDOB"));
        isValid &= basicInfo.validateEmail($("#financeEmail"));
        isValid &= basicInfo.validateAddress($("#financeAddress1"));
        isValid &= basicInfo.validateAddress($("#financeAddress2"));
        isValid &= basicInfo.validateLandmark($("#financeLandmark"));
        isValid &= basicInfo.validatePinCode($("#financePincode"));
        isValid &= selectBox.validateSelectBox($("#financePresentResidenceStatus"));
        isValid &= basicInfo.validateResidingSince($("#financeResidingSince"));


        if (isValid) {
            saveBasicDetails();


        }
        else {
            scrollTopError();
        }

        $('#screenLoader').hide();
    
    

}

function saveBasicDetails() {
	var contactDetails = {
	    "bajajAutoId": bajajAutoId ? bajajAutoId : null,
	    "salutation": $('#financeTitle').val(),
	    "firstName": $('#financeFname').val(),
	    "lastName": $('#financeLName').val(),
	    "gender": $('input[name=gender]:checked').val(),
	    "mobileNumber": $('#financeNum').val(),
	    "emailId": $('#financeEmail').val(),
	    "dateOfBirth": $('#financeDOB').val(),
	    "addressLine1": $("#financeAddress1").val(),
	    "addressLine2": $('#financeAddress2').val(),
	    "landmark": $('#financeLandmark').val(),
	    "pincode": $("#financePincode").val().substring(0, 6),
		"residenceStatus": $("#financePresentResidenceStatus option:selected").val(),
		"residingSince": $('#financeResidingSince').val(),
		"versionId": leadObject.versionId,
		"cityId": leadObject.cityId

	};

	$.ajax({
		type: "POST",
		url: "/api/finance/bajaj/basicdetails/",
		contentType: "application/json",
		data: JSON.stringify(contactDetails),
		beforeSend: function () {
			$('#otpLoader').show();
		},
		success: function (response) {
		    if (response != null) {
				if ( response > 0) {
					triggerGA('Loan_Application_BajajFinance', 'Step_1_Filled', bikeName + '_' + $('#financeNum').val());

					bajajAutoId = response;
					isStep1Done = true;
					showStep2();
				}
				else {
				    var obj = {
				        message: "Some error has occured.",
				        isYesButtonActive: true,
				        yesButtonText: "Okay",
				        yesButtonLink: "javascript:void(0)"
				    };
				    modalPopup.showModal(templates.modalPopupTemplate(obj));
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


function validateEmployeeInfo() {
    
        $('#screenLoader').show();


        var isValid = selectBox.validateSelectBox($("#financePrimaryEmploymentType"));

        var empTypeId = $("#financePrimaryEmploymentType").val();
        if (!$('#otherCompanyCheckbox').is(':checked')) {
            isValid &= employmentDetails.validateCompany($("#financeCompanyName"));
        }
        else {
            isValid &= employmentDetails.validateOtherCompany($("#financeOtherCompany"));
        }
        isValid &= employmentDetails.validateWorkingSince($("#financeWorkingSince"));
        isValid &= employmentDetails.validateNetPrimaryIncome("#financeNetPrimaryIncome");
        isValid &= employmentDetails.validateNoOfDependants("#financeNoofDependents");
        
        

        if (isValid) {
            saveEmployeeDetails();
        }
        else {
            scrollTopError();
        }

        $('#screenLoader').hide();
    
   
}


function saveEmployeeDetails() {

	var empDetails = {
	    "employmentType": parseInt($('#financePrimaryEmploymentType').val()) || 0,
	    "companyId": companyId > 0 ? companyId : null,
	    "otherCompany": companyId > 0 ? null : $("#financeOtherCompany").val(),
	    "workingsince": $("#financeWorkingSince").val(),
		"primaryIncome": $("#financeNetPrimaryIncome").val(),
		"dependents": $("#financeNoofDependents").val(),
		"bajajAutoId": bajajAutoId ? bajajAutoId : null,
		"versionId": leadObject.versionId,
		"pinCodeId": pincodeId
	};
	$.ajax({
		type: "POST",
		url: "/api/finance/bajaj/employeedetails/",
		contentType: "application/json",
		data: JSON.stringify(empDetails),
		beforeSend: function () {
			$('#otpLoader').show();
		},
		success: function (response) {
				if (response != null) {
				    triggerGA('Loan_Application_BajajFinance', 'Step_2_Filled', bikeName + '_' + $('#financeNum').val());
					
				    if (response.supplierDetails != null && response.supplierDetails.length > 0) {
				        var selectBoxDealer = $("#financeNearestDealer");
				        for (var i = 0; i < response.supplierDetails.length ; i++) {
				            selectBoxDealer.
                                append('<option value="' + response.supplierDetails[i].icas_supplierid + '">' + response.supplierDetails[i].sup_name + '</option>');
				        }
				        selectBoxDealer.trigger("chosen:updated");
					} 
					else {
				        $("#financeNearestDealer").attr("disabled", true).trigger("chosen:updated");
				    }
				    isStep2Done = true;
				    showStep3();
				}
				else {
				    var obj = {
				        message: "Some error has occured.",
				        isYesButtonActive: true,
				        yesButtonText: "Okay",
				        yesButtonLink: "javascript:void(0)"
				    };
				    modalPopup.showModal(templates.modalPopupTemplate(obj));
				}
		},
		complete: function () {
			$('#otpLoader').hide();
		}
        ,
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


function validateOtherInfo() {
    
        var isValid = false;

        isValid = selectBox.validateSelectBox($("#financeNearestDealer"));
        isValid = otherDetails.validateDOP($("#financeDOP"));
        isValid &= selectBox.validateSelectBox($("#financeRepayMode"));
        isValid &= selectBox.validateSelectBox($("#financeIdentificationdocument"));
        isValid &= validateCorrespondingIDNo();
        isValid &= selectBox.validateSelectBox($("#financeAccountTerm"));
        isValid &= otherDetails.validateBankAccount("#financeBankAccountNumber");

        if (isValid) {
            saveOtherDetails();
        }
        else {
            scrollTopError();
        }
    
    
}

function saveOtherDetails() {

    var allDetails = {
        "bajajAutoId": bajajAutoId ? bajajAutoId : null,
        "leadId": leadId,
        "salutation": $('#financeTitle').val(),
        "firstName": $('#financeFname').val(),
        "lastName": $('#financeLName').val(),
        "gender": $('input[name=gender]:checked').val(),
        "mobileNumber": $('#financeNum').val(),
        "emailId": $('#financeEmail').val(),
        "dateOfBirth": $('#financeDOB').val(),
        "addressLine1": $("#financeAddress1").val(),
        "addressLine2": $('#financeAddress2').val(),
        "landmark": $('#financeLandmark').val(),
        "pincode": $("#financePincode").val().substring(0, 6),
        "residenceStatus": $("#financePresentResidenceStatus option:selected").val(),
        "residingSince": $('#financeResidingSince').val(),
        "versionId": leadObject.versionId,
        "cityId": leadObject.cityId,

        "employmentType": $('#financePrimaryEmploymentType').val(),
        "workingsince": $("#financeWorkingSince").val(),
        "companyId": companyId > 0 ? companyId : null,
        "otherCompany": companyId > 0 ? null : $("#financeOtherCompany").val(),
        "primaryIncome": $("#financeNetPrimaryIncome").val(),
        "dependents": $("#financeNoofDependents").val(),
        "pinCodeId": pincodeId,

        "bajajSupplierId": $('#financeNearestDealer option:selected').val(),
        "likelyPurchaseDate": $('#financeDOP').val(),
        "repaymentMode": $('#financeRepayMode option:selected').val(),
        "idProof": $('#financeIdentificationdocument option:selected').val(),
        "idProofNo": $('#financeCorrespondingIDNo').val(),
        "bankAccountNo": $("#financeBankAccountNumber").val(),
        "accountVintage": $("#financeAccountTerm option:selected").val(),
        "pageUrl": leadObject.pageUrl,
        "leadJson": JSON.stringify(leadObject)
    };

	$.ajax({
		type: "POST",
		url: "/api/finance/bajaj/otherdetails/?source=" + $("#hdnPlatform").val(),
		contentType: "application/json",
		data: ko.toJSON(allDetails),
		beforeSend: function () {
			$('#otpLoader').show();
		},
		success: function (response) {
		    triggerGA('Loan_Application_BajajFinance', 'Step_3_Filled', bikeName + '_' + $('#financeNum').val());
            if (response) {
                leadId = response.leadId;
				otpScreen.openOtp();
				var objData = {
					"userName": allDetails.firstName + " " + allDetails.lastName,
					"mobileNumber": allDetails.mobileNumber,
					"email": allDetails.emailId,
					"redirectPageUrl": $('#pageUrl').val(),
					"changeMobileApiUrl": "/api/finance/bajaj/otherdetails/?source=" + $("#hdnPlatform").val(),
					"changeMobileApiUrlData": allDetails
				};
				otpvm.setParameters(objData);
				if (response.status == 1) {
					$('.otp-container__info').hide();
					$('#thankyouScreen').removeClass("hide");
					isFormSubmitted = true;
				}
				else if(response.status == 0){
					$('.otp-container__info').hide();
					$('#failureScreen').removeClass("hide");
					isFormSubmitted = true;
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


};
$('.chosen-select').chosen({ width: "100%" });


$(document).ready(function () {

    var selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

    window.onbeforeunload = function () {
        if (!isFormSubmitted) {
            return true;
        }
    };
    
});


$('.select-lbl').change(function () {
    $(this).closest('.select-box').addClass('done');
    
     validate.hideError($(this));
    
});

$("#financePrimaryEmploymentType").change(function () {
    if ($(this).val() == empTypeEnum.Retired) {
        $('#otherCompanyCheckbox').prop('checked', false).change();
        $("#financeWorkingSince,#financeCompanyName,#financeOtherCompany,#otherCompanyCheckbox,#financeNetPrimaryIncome,#financeNoofDependents").attr({ "disabled": true, "value": "" });
        validate.hideAllError("#financeWorkingSince,#financeCompanyName,#otherCompanyCheckbox,#financeOtherCompany,#financeNetPrimaryIncome,#financeNoofDependents");
        companyId = 0;
    }
    else {
        $("#financeWorkingSince,#financeCompanyName,#otherCompanyCheckbox,#financeNetPrimaryIncome,#financeNoofDependents").attr("disabled", false);
    }
});

    $('#otherCompanyCheckbox').change(function () {
    	if ($(this).is(':checked')) {
    		$("#financeOtherCompany").removeAttr("disabled");
    	}
    	else {
    		$("#financeOtherCompany").attr("disabled", true);
    	}

    	validate.hideError($('#financeOtherCompany'));
    	validate.hideError($('#financeCompanyName'));
    });

    $('#financeIdentificationdocument').change(function () {
        var idname = $('#financeIdentificationdocument option:selected').data("idproof");
        $("#identificationLabel").empty();
        $("#identificationLabel").append(idname);
    });
   
    $('#financePrimaryEmploymentType').change(function () {
        
        var employmentType = parseInt($('#financePrimaryEmploymentType option:selected').val());
        $("#incomeType").empty();
        $('#netprimaryincome').closest('.input-box').removeClass('invalid');
        $('#netprimaryincome .error-text').text('');
        switch(employmentType) {
            case 1:
                $("#incomeType").append("Monthly Income<sup>*</sup>");
                $("#financeNetPrimaryIncome").removeAttr("disabled");
                break;
        
            case 2:
                $("#incomeType").append("Last Profit After Tax<sup>*</sup>");
                $("#financeNetPrimaryIncome").removeAttr("disabled");
                break;
            case 4:
                $("#incomeType").append("Net Primary Income<sup>*</sup>");
                $("#financeNetPrimaryIncome").attr("disabled", true);
                break;

        }
    });



  

    function validateCorrespondingIDNo() {
        var inputIdValue = parseInt($('#financeIdentificationdocument').val());
        var correspondingId = $('#financeCorrespondingIDNo');
        switch (inputIdValue) {
            case 159:
                return otherDetails.validateAadharCard(correspondingId);
                break;

            case 167:
                return otherDetails.validateDrivingLicense(correspondingId);
                break;
            case 155:
                return otherDetails.validatePanCard(correspondingId);
                break;
            case 157:
                return otherDetails.validatePassport(correspondingId);
                break;
            case 156:
                return otherDetails.validateVoterId(correspondingId);
                break;
            case 158:
                return otherDetails.validateGovId(correspondingId);
                break;
            default:
                return otherDetails.validateOtherIDProof(correspondingId);
        }  
    }

    function scrollTop(offsetElem) {
        var offsetTop = 25;
        if (isDesktop.length) {
            offsetTop = 130;

        }
        if (offsetElem !== null) {
        	$("html, body").animate({
        		scrollTop: offsetElem.top - offsetTop
        	});
       }
    }

    function showBlackWindow() {
        $(blackWindowElem).show();
    }

    function hideBlackWindow() {
        $(blackWindowElem).hide();
    }

    var templates = {
        modalPopupTemplate: function (obj) {
            var template = '';

            template += '<span class="modal__close bwsprite bwmsprite cross-default-15x16"></span>';
            if (obj.message != null && obj.message.length > 0) {
                template += '<p class="modal__description">' + obj.message + '</p>';
            }
            if (obj.isYesButtonActive) {
                template += '<a href="' + obj.yesButtonLink + '" class="btn btn-orange btn-124-36 modal__close">' + obj.yesButtonText + '</a>';
            }

            return template;
        }
    };

    var modalPopup = {
        registerDomEvents: function () {
            $(document).on('click', '.modal-box .modal__close', function () {
                modalPopup.closeActiveModalPopup();
            });
        },

        showModal: function (htmlString, modalBox) {
            modalBox = modalBox || $('#modalPopUp');
            $('#modalBg').show();
            modalBox.html(htmlString).show();
            modalPopup.lockScroll();
        },

        hideModal: function (modalBox) {
            modalBox = modalBox || $('#modalPopUp');
            $('#modalBg').hide();
            modalBox.html('').hide();
            modalPopup.unlockScroll();
        },

        lockScroll: function () {
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

        unlockScroll: function () {
            var scrollTop = parseInt($('html').css('top'));

            $('html').removeClass('lock-browser-scroll');
            $('html, body').scrollTop(-scrollTop);
        },

        isVisible: function (modalBox) {
            modalBox = modalBox || $('#modalPopUp');
            return modalBox.is(':visible');
        },

        closeActiveModalPopup: function () {
            if (modalPopup.isVisible()) {
                modalPopup.hideModal();
                return true;
            }
            return false;
        }
    };


    function showStep1() {
        $("#basic-detail-tab").removeClass("hide");
        $(personalDetailTab).addClass("hide");
        $("#other-detail-tab").addClass("hide");
        $(".contact-image-unit").addClass('contact-icon').removeClass('white-tick-icon');
        $(".personal-image-unit").addClass('gray-bag-icon bg-gray').removeClass('white-personal-icon white-bag-icon white-tick-icon bg-red gray-personal-icon');
        $(".employment-image-unit").addClass('gray-personal-icon').removeClass('white-personal-icon bg-red');

        if (isDesktop.length) {
            scrollTop($(personalDetailTab).offset());
            $(".personal__title").addClass("inactive");
            $(".personal-details-container").removeClass("visible");
        }
        else {
            scrollTop(formTabsContainer.offset());
            formTabsContainer.find('.page-tabs__li.active').addClass('active');
            formTabsContainer.find('.page-tabs__li[data-id=employee-detail-tab]').addClass("inactive").removeClass('active');
            formTabsContainer.find('.page-tabs__li[data-id=other-detail-tab]').removeClass('active').addClass('inactive');
        }
    }

    function showStep2() {
        $("#basic-detail-tab,#other-detail-tab").addClass("hide");
        $(personalDetailTab).removeClass("hide");

        $(".contact-image-unit").removeClass('contact-icon').addClass('white-tick-icon');
        $(".personal-image-unit").removeClass('gray-bag-icon').addClass('white-bag-icon');
        $(".personal-image-unit").addClass('personal-icon bg-red').removeClass('white-tick-icon bg-gray');
        $(".employment-image-unit").addClass('gray-personal-icon').removeClass('white-personal-icon bg-red');

        if (isDesktop.length) {
            scrollTop($(personalDetailTab).offset());
            $(".personal__title").removeClass("inactive");
            $(".personal-details-container").addClass("visible");

        }
        else {
            scrollTop(formTabsContainer.offset());
            formTabsContainer.find('.page-tabs__li.active').removeClass('active');
            formTabsContainer.find('.page-tabs__li[data-id=employee-detail-tab]').removeClass("inactive").addClass('active');
            formTabsContainer.find('.page-tabs__li[data-id=other-detail-tab]').removeClass('active').addClass('inactive');
        }
    }

    function showStep3() {
        $("#employee-detail-tab,#basic-detail-tab").addClass("hide");
        $(employmentDeatilTab).removeClass("hide");

        $(".contact-image-unit").removeClass('contact-icon').addClass('white-tick-icon');
        $(".personal-image-unit").removeClass('white-bag-icon bg-gray').addClass('white-tick-icon bg-red');
        $(".employment-image-unit").addClass('white-personal-icon bg-red').removeClass('gray-personal-icon bg-gray');

        if (isDesktop.length) {
            scrollTop($(employmentDeatilTab).offset());
            $(".employment__title").removeClass("inactive");
            $(".employment-details-container").addClass("visible");
        }
        else {
            scrollTop(formTabsContainer.offset());
            formTabsContainer.find('.page-tabs__li.active').removeClass('active');
            formTabsContainer.find('.page-tabs__li[data-id=other-detail-tab]').removeClass('inactive').addClass('active');
        }
    }
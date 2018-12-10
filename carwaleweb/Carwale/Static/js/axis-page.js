var loanAxis = {
    loanCity: {},
    personCityInputField: $("#personCity"),
    blackoutWindow: '#blackOut-window',
    popupContainer: '.popup-container',
    inputTxt: $('.input-txt'),
    palceholder: '.palceholder',
    result: "",
    personLeadCity: {},
    apiInput: {},
    visiblePage: $('div.page:visible'),
    prevPageId: $('div.page:visible').prev('.page').attr('id'),
    currentPageId: $('div.page:visible').attr('id'),
    nextPageId: $('div.page:visible').next('.page').attr('id'),
    modelId: 0,
    pageLoad: function () {
        loanAxis.commonEvents.registerEvents();
        loanAxis.personalDetail.registerEvents();
        loanAxis.cityAutoSuggest();
    },
    commonEvents: {
        registerEvents: function () {
            loanAxis.commonEvents.disableNext();
            //to disable arrow image in dropdowns IE
            var isIE11 = !!navigator.userAgent.match(/Trident.*rv\:11\./);
            if (isIE11 || $.browser.msie)
                $('.loan-section .select-box').css('background', "transparent");

            loanAxis.commonEvents.radioFillCheck();
            loanAxis.commonEvents.backBtnClick(); //back button click function
            loanAxis.commonEvents.nextBtnClick(); //next arrow click function 
            loanAxis.commonEvents.btnClick();
            loanAxis.commonEvents.radioChange();
            loanAxis.personalDetail.inputFocus();

            if ($.cookie('_CustCityIdMaster') > 0) {
                loanAxis.personLeadCity.name = $.cookie("_CustCityMaster");
                loanAxis.personLeadCity.id = $.cookie("_CustCityIdMaster");
                $('#personCity').parent().find('.palceholder').addClass('hide');
                $('#personCity').parent().closest('label').addClass('hide');
                $("#personCity").val(loanAxis.personLeadCity.name);
            }
            $('#drpMake').on('change', function () {
                loanAxis.apiCalls.bindModels();
                if ($('#drpMake').val() != -1) {
                    $(this).css('color', '#1a1a1a');
                } else {
                    $(this).css('color', '#999');
                    $('#drpModel').css('color', '#999');
                }
            });
            $('#drpModel').on('change', function () {
                loanAxis.personalDetail.checkDropdownInValid($('#drpModel'), $('#spnDrpModel'));
                if ($('#drpModel').val() != -1) {
                    $(this).css('color', '#1a1a1a');
                } else {
                    $(this).css('color', '#999');
                }
            });
            $("#form-submit a").click(function () {
                loanAxis.personalDetail.validateUserDetails();
            });
            $('#drpModel').prop("disabled", true);
            loanAxis.commonEvents.prefillCustomerDetails();
            loanAxis.commonEvents.prefillCarData();
        },
        nextBtnClick: function () {
            $(document).on('click', '#next:not(".btn-disable"), .next-button:not(".btn-disable")', function () {
                loanAxis.commonEvents.nextClick();
            });
        },
        nextClick: function () {
            loanAxis.visiblePage = $('div.page:visible');
            loanAxis.currentPageId = $('div.page:visible').attr('id');
            loanAxis.nextPageId = $('div.page:visible').next('.page').attr('id');
            if (loanAxis.currentPageId == "employmentType") {
                if ($("#salaried").is(":not(:checked)") && $("#self-employed").is(":not(:checked)")) {
                    $('#employmentType div.error').removeClass('hide').text("Please select Employment Type");
                }
                else {
                    loanAxis.commonEvents.pageShow(loanAxis.currentPageId, loanAxis.nextPageId);
                    loanAxis.commonEvents.enableBack();
                    $('#employmentType div.error').hide();
                }
            }
            if (loanAxis.currentPageId == "salaryRangeType") {
                if ($("#below").is(":checked")) {
                    var bankbazaarUrl = "https://www.bankbazaar.com/car-loan.html?variant=slide&headline=HEADLINE_CL_MelaSale&WT.mc_id=bb01%7CCL%7Cdesk_finance&utm_source=bb01&utm_medium=display&utm_campaign=bb01%7CCL%7Cdesk_finance&variantOptions=mobileRequired&ck=Y%2BziX71XnZjIM9ZwEflsyJdl6U4IGH%2FGwk8%2FK0ps%2F2%2BZC0TqFEkhF6ljN6nbC1ZXXBwImSfaF7%2BS%0AmJOG7qYyyg%3D%3D&rc=1";
                    $('#salaryRangeType, #back, #next,.page-title').addClass('hide');
                    $('#bankbazaar').show();
                    var myTimer = setInterval(function () {
                            clearInterval(myTimer);
                            window.location.href = bankbazaarUrl;
                    }, 5000);
                }
                else if ($("#average").is(":not(:checked)") && $("#above").is(":not(:checked)")) {
                    $('#salaryRangeType div.error').show().text("Please select Salary Range Type");
                }
                else {
                    $('.form-page-title, .how-it-works-section').show();
                    $('.loan-section div:first, #salaryRangeType div.error').hide();
                    loanAxis.commonEvents.pageShow(loanAxis.currentPageId, loanAxis.nextPageId);
                }
                loanAxis.commonEvents.enableBack();
            }
        },
        disableNext: function () {
            $('#next').addClass('btn-disable');
        },
        enableNext: function () {
            $('#next').removeClass('btn-disable');
        },
        disableBack: function () {
            $('#back').addClass('btn-disable');
        },
        enableBack: function () {
            $('#back').removeClass('btn-disable');
        },
        backBtnClick: function () {
            $(document).on('click', '#back:not(".btn-disable")', function () {
                $('.form-page-title, .how-it-works-section').hide();
                $('.loan-section div:first').show();
                loanAxis.commonEvents.pageHide($('div.page:visible').attr('id'), $('div.page:visible').prev().attr('id'));
                loanAxis.commonEvents.enableNext();
            });
        },
        pageShow: function (currentId, nextId) {
            $('#' + currentId).addClass('hide');
            $('#' + nextId).removeClass('hide');
            loanAxis.commonEvents.radioFillCheck();
        },
        pageHide: function (currentId, prevId) {
            $('#' + currentId).addClass('hide');
            $('#' + prevId).removeClass('hide');
            loanAxis.commonEvents.radioFillCheck();
        },
        radioFillCheck: function () {
            if ($('div.page:visible').find(".optional-unit input[type='radio']").is(":checked")) {
                loanAxis.currentPageId == "employmentType" ? $('#employmentType div.error').hide() : $('#salaryRangeType div.error').hide();
                loanAxis.commonEvents.enableNext();
            }
            else {
                loanAxis.commonEvents.disableNext();
            }

            if ($('div.page:visible').index() == 0) {
                loanAxis.commonEvents.disableBack();
            }

            if ($('div.page:visible').index() == $('div.page').length) {
                loanAxis.commonEvents.disableNext();
            }

        },
        btnClick: function () {
            $('#popup-btn, #blackOut-window,.cross-lg-dark-grey').click(function () {
                $(loanAxis.popupContainer).addClass('hide');
                $(loanAxis.blackoutWindow).removeClass('show');
                $('body').removeClass('lock-browser-scroll');
            });
        },
        radioChange: function () {
            $('.optional-unit input').on('change', function () {
                
                loanAxis.commonEvents.radioFillCheck();
            });
        },
        prefillCustomerDetails: function () {
            if ($.cookie('_CustomerName') != null) {
                $('#txtFirstName').parent().find('.palceholder').addClass('hide');
                $("#txtFirstName").val($.cookie('_CustomerName'));
            }
            if ($.cookie('_CustEmail') != null && $.cookie('_CustEmail').length > 0) {
                $('#txtEmail').parent().find('.palceholder').addClass('hide');
                $("#txtEmail").val($.cookie('_CustEmail'));
            }
            if ($.cookie('_CustMobile') != null) {
                $('#txtMobile').parent().find('.palceholder').addClass('hide');
                $("#txtMobile").val($.cookie('_CustMobile'));
            }
        },

        prefillCarData: function () {
            var url = window.location.href.toLowerCase();
            loanAxis.modelId = url.indexOf("modelid") < 0 ? "" : Common.utils.getValueFromQS('modelid');
            if (url.indexOf("modelid") != -1 && loanAxis.modelId != "" && loanAxis.modelId > 0) {
                    $.when(loanAxis.apiCalls.getCarModelData(loanAxis.modelId)).done(function (data) {
                        if (data != null && data.MakeId > 0) {
                            loanAxis.carData = data;
                            loanAxis.apiCalls.bindMakes(loanAxis.carData.MakeId);
                        }
                    });
            }
            else
                loanAxis.apiCalls.bindMakes();

        }
    },

    personalDetail: {
        registerEvents: function () {
            $('#txtFirstName').on('change', function () {
                loanAxis.personalDetail.checkNameInvalid($('#txtFirstName'), $('#spntxtFirstName'));
            });
            $('#txtEmail').on('change', function () {
                loanAxis.personalDetail.checkEmailInvalid();
            });
            $('#txtMobile').on('change', function () {
                loanAxis.personalDetail.checkMobInvalid();
            });

            if ($('#personalDetail').is(':visible'))
                $('#next, #form-submit').click(function () {
                    loanAxis.personalDetail.validatePersonalDetails();
                    loanAxis.validateCity(loanAxis.personCityInputField);
                });
        },
        validateUserDetails: function () {
            var cityEle = $("#personCity");
            var validatePersonal = loanAxis.personalDetail.validatePersonalDetails();
            var validateCity = loanAxis.validateCity(cityEle);
            var isMakeInValid = loanAxis.personalDetail.checkDropdownInValid($('#drpMake'), $('#spnDrpMake'));
            var isModelInValid = loanAxis.personalDetail.checkDropdownInValid($('#drpModel'), $('#spnDrpModel'));
            if (validatePersonal && validateCity && !isMakeInValid && !isModelInValid) {
                loanAxis.apiCalls.leadPush(loanAxis.apiCalls.processResponse);
            }
        },

        validateInputField: function (field, regex) {
            try {
                if (!regex.test(field.val().toLowerCase())) {
                    return false;
                }
                return true;
            }
            catch (e) { console.log(e) }
            return false;
        },
        checkMobInvalid: function () {
            var isError = false;
            var reMobile = /^[6789]\d{9}$/;
            var reNumeric = /^[0-9]*$/;

            if ($('#txtMobile').val() == "") {
                $('#spntxtMobile').show();
                return true;
            }
            else if ($('#txtMobile').val() != "") {

                if (!reNumeric.test($("#txtMobile").val())) {
                    $('#spntxtMobile').show().text("Please enter numeric data only.");
                    return true;
                }
                else if (!reMobile.test($("#txtMobile").val())) {
                    $('#spntxtMobile').show().text("Please enter valid mobile number.");
                    return true;
                }
                else if (!reNumeric.test($("#txtMobile").val()) && $("#txtMobile").val().length < 10) {
                    $('#spntxtMobile').show().text("Please enter 10 digit.");
                    return true;
                }
                else {
                    $('#spntxtMobile').hide();
                    return false;
                }
            }
            else {
                $('#spntxtMobile').hide();
                return false;
            }
        },

        checkEmailInvalid: function () {
            var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
            if ($('#txtEmail').val() == "") {
                $('#spntxtEmail').show();
                return true;
            }
            else if (!loanAxis.personalDetail.validateInputField($("#txtEmail"), reEmail)) {
                $('#spntxtEmail').show().text("Please enter valid email.");
                return true;
            }
            else {
                $('#spntxtEmail').hide();
                return false;
            }
        },

        checkNameInvalid: function (id, errorId) {
            var reName = /^([-a-zA-Z ']*)$/;
            if ($.trim(id.val()) == "") {
                errorId.show();
                return true;
            }
            else if (!loanAxis.personalDetail.validateInputField(id, reName)) {
                errorId.show().text("Please enter valid name.");
                return true;
            }
            else {
                errorId.hide();
                return false;
            }
        },

        validatePersonalDetails: function () {
            var isFirstNameInvalid = loanAxis.personalDetail.checkNameInvalid($('#txtFirstName'), $('#spntxtFirstName'));
            var isMobInvalid = loanAxis.personalDetail.checkMobInvalid();
            var isEmailInvalid = loanAxis.personalDetail.checkEmailInvalid();

            if (isFirstNameInvalid || isMobInvalid || isEmailInvalid) {
                return false;
            }
            else {
                return true;
            }
        },

        checkDropdownInValid: function (id, errorId) {
            if (id.val() < 0) {
                errorId.show();
                return true;
            }
            else {
                errorId.hide();
                return false;
            }
        },

        inputFocus: function () {
            $(loanAxis.palceholder).click(function () {
                $(this).siblings('input').focus();
            });
            (loanAxis.inputTxt).focus(function () {
                $(this).siblings(loanAxis.palceholder).hide();
            });
            (loanAxis.inputTxt).blur(function () {
                var $this = $(this);
                if ($this.val().length == 0)
                    $(this).siblings(loanAxis.palceholder).show();
            });
            (loanAxis.inputTxt).blur();
        },
    },

    validateCity: function (targetId) {
        var cityVal = Common.utils.getSplitCityName(targetId.val());
        if (cityVal == $.cookie("_CustCityMaster") && typeof (loanAxis.personCityInputField) != "undefined") {
            if (typeof isMobileDevice == "undefined") {
                loanAxis.showHideMatchError(false, targetId);
            }
            else {
                loanAxis.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
            }
            return true;
        }
        else if (cityVal == "" || targetId.hasClass('border-red') ||
    (
    ($('li.ui-state-focus a:visible').text() != cityVal && cityVal != "") &&
    (typeof (loanAxis.personLeadCity) == "undefined" || typeof (loanAxis.personLeadCity.name) == "undefined" || loanAxis.personLeadCity.name.toLowerCase() != cityVal.toLowerCase())
    )
      ) {
            if (typeof isMobileDevice == "undefined") {
                loanAxis.showHideMatchError(true, targetId, "Please Enter City");
            }
            else {
                loanAxis.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
            }
            return false;
        }
        return true;
    },

    showHideMatchError: function (error, TargetId, errText) {
        if (error) {
            TargetId.siblings('#spntxtCity').removeClass('hide').text(errText);
        }
        else {
            TargetId.siblings().addClass('hide');
        }
    },

    cityAutoSuggest: function () {
        if (typeof isMobileDevice == "undefined") {
            $("#personCity").cw_autocomplete({
                resultCount: 5,
                source: ac_Source.allCarCities,
                click: function (event, ui, orgTxt) {
                    loanAxis.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                    loanAxis.personLeadCity.id = ui.item.id;
                    ui.item.value = loanAxis.personLeadCity.name;
                    loanAxis.commonEvents.enableBack();
                },
                open: function (result) {
                    loanAxis.personLeadCity.result = result;
                },
                afterfetch: function (result, searchtext) {
                    this.result = result;
                    if (typeof result == "undefined" || result.length <= 0)
                        loanAxis.showHideMatchError(true, $('#personCity'), "No city Match");
                    else
                        loanAxis.showHideMatchError(false, $('#personCity'));
                },
                focusout: function () {
                    if ($('li.ui-autocomplete a:visible').text() != "") {
                        console.log(5)
                        if (loanAxis.loanCity == undefined) loanAxis.loanCity = new Object();
                        var focused = loanAxis.loanCity.result[$('li.ui-autocomplete').index()];
                        if (focused != undefined && focused.label == $('#personCity').val()) {
                            loanAxis.loanCity.Name = Common.utils.getSplitCityName(focused.label);
                            loanAxis.loanCity.Id = focused.id;
                            loanAxis.apiInput.CityId = loanAxis.loanCity.Id;
                            loanAxis.eligibilityApiInput.CityId = loanAxis.loanCity.Id;
                            loanAxis.apiInput.Res_City = loanAxis.loanCity.Name;
                        }
                        else {
                            loanAxis.loanCity = {};
                        }
                    }
                    else {
                        if ($('#otherCityError').is(':visible'))
                            $('#otherCityError').hide();
                        if ($('#selectCity:visible .pill-box.active-box').length < 1)
                            loanAxis.commonEvents.disableNext();
                    }
                }
            });
        }
        else {
            var personCityInputField = $("#personCity");

            $("#personCity").cw_easyAutocomplete({
                inputField: personCityInputField,
                resultCount: 5,
                source: ac_Source.allCarCities,
                click: function (event) {
                    var selectionValue = personCityInputField.getSelectedItemData().value,
                    selectionLabel = personCityInputField.getSelectedItemData().label;

                    loanAxis.personLeadCity.name = Common.utils.getSplitCityName(selectionLabel);
                    loanAxis.personLeadCity.id = selectionValue;
                    $(personCityInputField).val(loanAxis.personLeadCity.name);
                },

                afterFetch: function (result, searchText) {
                    loanAxis.personLeadCity.result = result;

                    if (typeof result == "undefined" || result.length <= 0) {
                        loanAxis.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                    }
                    else {
                        loanAxis.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                    }
                },
                focusout: function () {
                    if ($('#otherCityError').is(':visible')) {
                        $('#otherCityError').hide();
                    }
                    if ($('#selectCity:visible .pill-box.active-box').length < 1) {
                        loanAxis.commonEvents.disableNext();
                    }
                }
            });
        }
    },

    apiCalls: {
        bindMakes: function (makeId) {
            var opt = '<option value="-1">Make</option>';
            $.ajax({
                url: '/webapi/CarMakesData/GetCarMakes/?type=new',
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        for (var i = 0; i < response.length; i++) {
                            opt += '<option value=' + response[i].makeId + '>' + response[i].makeName + '</option>';
                        }
                        $("#drpMake").append(opt);
                        if (typeof makeId !== "undefined" && makeId > 0 && !loanAxis.isPrefilled) {
                            $("#drpMake option[value='" + makeId + "']").attr("selected", "selected");
                            loanAxis.apiCalls.bindModels();
                        }
                    }
                }
            });
        },
        bindModels: function () {
            $('#spnDrpMake').hide(); 
            $("#drpModel").empty();
            var opt = '<option value="-1">Model</option>';
            $.ajax({
                url: '/webapi/carmodeldata/GetCarModelsByType/?type=new&makeId=' + $('#drpMake').val(),
                type: "GET",
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null && $('#drpMake').val() != -1) {
                        for (var i = 0; i < response.length; i++) {
                            opt += '<option value=' + response[i].ModelId + '>' + response[i].ModelName + '</option>';
                        }
                    }
                    $("#drpModel").append(opt);
                    $('#drpModel').prop("disabled", false);
                    if (loanAxis.modelId > 0 && !loanAxis.isPrefilled) {
                        isPrefilled = true;
                        $("#drpModel option[value='" + loanAxis.modelId + "']").attr("selected", "selected");
                    }
                }
            });
        },
        createApiInput: function (event) {
            loanAxis.apiInput.FinanceLeadId = -1;//replace with campaign id
            loanAxis.apiInput.First_Name = $("#txtFirstName").val();
            loanAxis.apiInput.Car_Model = $('#drpModel option:selected').text();
            loanAxis.apiInput.Email = $("#txtEmail").val();
            loanAxis.apiInput.Mobile = $("#txtMobile").val();
            loanAxis.apiInput.ClientId = 12;
            loanAxis.apiInput.CityId = loanAxis.personLeadCity.id;
            loanAxis.apiInput.Car_Make = $('#drpMake option:selected').text();
            loanAxis.apiInput.Emp_Type = $("#salaried").is(":checked") ? "Salaried" : "Self Employed";
            loanAxis.apiInput.IncomeTypeId = $("#salaried").is(":checked") ? 1 : 2;
            loanAxis.apiInput.Monthly_Income = $("#average").is(":checked") ? 70000 : 110000;
        },
        leadPush: function (callback) {
            loanAxis.apiCalls.createApiInput();
            $.ajax({
                type: 'POST',
                url: '/api/finance/quote/',
                data: loanAxis.apiInput,
                contentType: "application/x-www-form-urlencoded",
                dataType: 'Json',
                headers: { "clientId": "12", "sourceId": platFormId },
                success: function (response) {
                    callback(response);
                },
                error: function () {
                    callback(null);
                }
            });
        },

        processResponse: function (data) {
            $(loanAxis.popupContainer).removeClass('hide');
            $(loanAxis.blackoutWindow).addClass('show');
            $('body').addClass('lock-browser-scroll');
        },

        getCarModelData: function (modelId) {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarModelData/GetCarDetailsByModelId/?modelid=' + modelId,
                dataType: 'Json'
            });
        },
        
    }
}
$(document).ready(function () {
    loanAxis.pageLoad();
    Common.utils.trackAction('CWNonInteractive', AxisCarLoanPage, 'Axis Car Loan_shown', 'Axis Car Loan');
    cwTracking.trackCustomData("ESProperties", "Impression", "page=" + AxisCarLoanPage, !0)
});
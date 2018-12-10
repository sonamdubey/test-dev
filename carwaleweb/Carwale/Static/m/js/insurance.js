var selectedStateId = "";
var selectedStateName = "";
var selectedCityId = "";
var selectedCityName = "";
var selectedModelName = "";
var selectedMakeName = "";
var selectedModelId = "";
var selectedMakeId = "";
var selectedVersionId = "";
var selectedVersionName = "";
var policyType = "New";
var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
$(document).ready(function () {

    function pickerOpen() {
        var $this = this;
        setTimeout(function () {
            $this.blur();
        }, 100);
        $('html').css({ 'overflow': 'hidden' });
    };

    function pickerClose() {
        var $this = this;
        setTimeout(function () {
            $this.blur();
        }, 100);
        $('html').css({ 'overflow': 'auto' });
    };

    var $regDate = $("#txtRegDate").Zebra_DatePicker({
        format: 'M, Y',
        onOpen: pickerOpen,
        onClose: pickerClose
    });

    if ($("#hdn_radio").value == "") {
        $("#hdn_radio").value = "2";
    }

    //Accordion script
    var $this, plusminus, winHt = $(window).height();

    $(document).on('click', '.accordion > .stepStrip:not(".disabled")', function () {
        $this = $(this);
        plusminus = $this.find('.icon.plus-minus .fa');
        if ($this.next().is(":visible")) {
            $this
                .next().slideToggle(0)
                .parent().toggleClass('open');
        } else {
            $this.parent().siblings('.accordion').find('.stepStrip + div').slideUp(0);
            $('.accordion').removeClass('open');
            $this
                .next().slideDown(0)
                .parent().addClass('open');

            if ($this.find('.icon').hasClass('plus-minus')) {
                $('.icon.plus-minus .fa')
                    .attr('class', 'fa fa-plus');
            }

            //scroll to an element
            var offset = $this.parent().offset().top;
            var windHt = $(window).scrollTop() + $(window).height() - 300;

            if (offset > windHt) {
                $('html, body').animate({
                    scrollTop: offset - (($(window).height() * 0.5))
                }, 700);
            }

        }

        if ($this.find('.icon').hasClass('plus-minus')) {
            if (plusminus.hasClass('fa-plus'))
                plusminus.removeClass('fa-plus').addClass('fa-minus');
            else
                plusminus.removeClass('fa-minus').addClass('fa-plus');
        }

    });

    preFillCustomerDetails();

    Insurance.pageLoad();
});

//policy for new car
function rdNew_Click(e) {
    $('#step1 .icon.edit-icon').removeClass('hide');
    $('#step2').removeClass('disabled').trigger('click');
    $('#divRegDate').addClass('hide');
    $("#hdn_radio").val($("#rdNew").val());
    $("#errRegDate").text("").hide();
    policyType = "New";
    //$.clearControls();
    dataLayer.push({ event: 'Insurance_mobile', cat: 'Insurance_mobile', act: 'Insurance_Formpage', lab: 'Step1_New' });
}

//renew car insurance
function rdRenew_Click(e) {
    $('#step1 .icon.edit-icon').removeClass('hide');
    $('#step2').removeClass('disabled').trigger('click');
    $(".ncb-tabs li:nth-child(2)").click();
    if (!$('#errMsgInsType').hasClass('hide')) {
        $('#errMsgInsType').addClass('hide')
    }
    $("#hdn_radio").val($("#rdRenew").val());
    $('#divRegDate').removeClass('hide');
    policyType = "Used";
    if ($('#hdn_RegDate').val() != "") {
        $("#txtRegDate").val($('#hdn_RegDate').val());
    }
    dataLayer.push({ event: 'Insurance_mobile', cat: 'Insurance_mobile', act: 'Insurance_Formpage', lab: 'Step1_Renew' });
}

//clear all the controls
$.clearControls = function () {
    $('#insSelectCar').text();
    $('#hdn_MakeId').val("");
    $('#hdn_VersionId').val("");
    $('#errCar').html("").hide();

    $("#errRegDate").val("").text("").hide();
    $("#errRegDate").text("").hide();
}

function openMakePopup() {
    $.pushState(null, "select make", "make");
    var currentPopup = $('.MakeDiv');
    showLoadingForDiv(currentPopup, currentPopup.prev());
    $('#txtMake').val('');
    $.ajax({
        type: 'GET',
        url: '/webapi/CarMakesData/GetCarMakes/?type=' + policyType + '',
        dataType: 'Json',
        success: function (json) {
            selectedModelId = json[0].ModelId;
            var viewModel = {
                CarMakes: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("drpMake"));
            ko.applyBindings(viewModel, document.getElementById("drpMake"));
            hideLoadingForDiv(currentPopup);
            $('#txtMake').cw_fastFilter('#drpMake');
        }
    });
}

$.pushState = function (obj, title, hashStr) {
    window.location.hash = hashStr;
};


function makeChanged(makeLi) {
    selectedMakeId = $(makeLi).val();
    selectedMakeName = $(makeLi).text();
    $("#hdn_Make").val(selectedMakeName);
    var currentPopup = $(".modelDiv");
    showLoadingForDiv(currentPopup, currentPopup.prev());
    hdn_MakeId.value = selectedMakeId;
    $('#txtModel').val('');
    $.ajax({
        type: 'GET',
        url: '/webapi/CarModelData/GetCarModelsByType/?type=' + policyType + '&makeId=' + selectedMakeId,
        dataType: 'Json',
        success: function (json) {
            var viewModel = {
                CarModels: ko.observableArray(json)
            };

            ko.cleanNode(document.getElementById("model"));
            ko.applyBindings(viewModel, document.getElementById("model"));
            hideLoadingForDiv(currentPopup);
            $.pushState(null, "select model", "model");
            $('#txtModel').cw_fastFilter('#model');
        }
    });

}

function modelChanged(modelLI) {

    selectedModelId = $(modelLI).val();
    selectedModelName = $(modelLI).text();
    $("#hdn_Model").val(selectedModelName);
    var currentPopup = $(".versionDiv");
    showLoadingForDiv(currentPopup, currentPopup.prev());
    hdn_ModelId.value = selectedModelId;
    $('#txtVersion').val('');
    $.ajax({
        type: 'GET',
        url: '/webapi/CarVersionsData/GetCarVersions/?type=' + policyType + '&modelid=' + selectedModelId,
        dataType: 'Json',
        success: function (json) {

            var viewModel = {
                CarVersions: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("version"));
            ko.applyBindings(viewModel, document.getElementById("version"));
            hideLoadingForDiv(currentPopup);
            $.pushState(null, "select version", "version");
            $('#txtVersion').cw_fastFilter('#version');
        }
    });
}

window.onhashchange = function (event) {
    if ((event.oldURL.indexOf('#make') > 0 && event.newURL.indexOf('#') < 0) || (event.oldURL.indexOf('#make') > 0 && event.newURL.indexOf('#') > 0))
        hidepopup($('.MakeDiv div.filterBackArrow')[0], 'insSelectCar');
    else if (event.oldURL.indexOf('#model') > 0 && event.newURL.indexOf('#make') > 0)
        hidepopup($('.modelDiv div.filterBackArrow')[0], '');
    else if (event.oldURL.indexOf('#version') > 0 && event.newURL.indexOf('#model') > 0)
        hidepopup($('.versionDiv div.filterBackArrow')[0], '');
    else if (event.oldURL.indexOf('#state') > 0)
        hidepopup($('.StateDiv div.filterBackArrow')[0], 'insSelectState');
    else if ((event.oldURL.indexOf('#city') > 0 && event.newURL.indexOf('#') < 0) || (event.oldURL.indexOf('#city') > 0 && event.newURL.indexOf('#') > 0))
        hidepopup($('.CityDiv div.filterBackArrow')[0], 'insSelectCity');
    else if (event.oldURL.indexOf('#thankyou') > 0 && ((event.newURL.indexOf('') == 0 || event.newURL.indexOf('#') > 0) && ($('#thankYou1').is(":visible") || $('#thankYou2').is(":visible"))))
        window.location.reload();
}

function versionChanged(selectedVersion) {
    selectedVersionId = $(selectedVersion).val();
    selectedVersionName = $(selectedVersion).text();
    $("#hdn_VersionId").val(selectedVersionId);
    $("#hdn_Version").val(selectedVersionName);
    $('.versionDiv').hide();
    unlockPopup();
    $("#main-container").show();
    $('#insSelectCar').text(selectedMakeName + ' ' + selectedModelName + ' ' + selectedVersionName);
    $('#errCar').html("").show();
    $('html,body').animate({ 'scrollTop': $('#insSelectCar').offset().top }, 300);
    $.pushState(null, "", "");
}

function hidepopup(back, inputId) {
    var chkFlag = inputId;
    inputId = '#' + inputId;
    var divToShow = $(back).parent().parent().prev();
    var divToHide = $(back).parent().parent();
    divToHide.hide();
    unlockPopup();
    divToShow.show();
    if ($('div.popup_layer').is(":visible"))
        lockPopup();
    if (chkFlag != '')
        $('html,body').animate({ 'scrollTop': $(inputId).offset().top }, 300);
    if ($('div.noFound').length > 0) $('div.noFound').remove();
    divToHide.find('.cross-box-wrap .cross-box').hide();
}

//Close button
function CloseWindow() {
    $("#insCarSelectDiv").hide();
    $(".MakeDiv").hide();
    $(".modelDiv").hide();
    $(".versionDiv").hide();
    $(".StateDiv").hide();
    $(".CityDiv").hide();
    unlockPopup();
}

function OpenPopupState() {
    $('#txtState').val('');
    var currentPopup = $('.StateDiv');
    $('#errState').hide();
    showLoadingForDiv(currentPopup, currentPopup.prev());

    $.ajax({
        type: 'GET',
        url: '/webapi/geocity/GetStates/',
        dataType: 'Json',
        success: function (json) {

            var viewModel = {
                CarState: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("state"));
            ko.applyBindings(viewModel, document.getElementById("state"));
            hideLoadingForDiv(currentPopup);
            $.pushState(null, "select state", "state");
            $('#txtState').cw_fastFilter('#state');
        }
    });

}

function stateChanged(stateLi) {
    selectedStateId = $(stateLi).val();
    selectedStateName = $(stateLi).text();
    $("#hdn_StateId").val(selectedStateId);
    $("#hdn_StateName").val(selectedStateName);
    $('.StateDiv').hide();
    unlockPopup();
    $('#insSelectState').text(selectedStateName);
    $("#insSelectCity").text('Select City');
    $('#hdn_CityId').val("");
    $('html,body').animate({ 'scrollTop': $('#insSelectState').offset().top }, 300);
    $.pushState(null, "", "");
}

function OpenPopupCity() {
    $('#txtCity').val('');
    if ($('#hdn_StateId').val() == "") {
        $('#errCity').html("Select State First*.");
        $("#errCity").show();
    }
    else {
        var currentPopup = $('.CityDiv');
        showLoadingForDiv(currentPopup, currentPopup.prev());
        $.ajax({
            type: 'GET',
            url: '/webapi/geocity/GetCitiesByState/?stateId=' + $("#hdn_StateId").val(),
            dataType: 'Json',
            success: function (json) {

                var viewModel = {
                    CarCity: ko.observableArray(json)
                };
                ko.cleanNode(document.getElementById("city"));
                ko.applyBindings(viewModel, document.getElementById("city"));
                hideLoadingForDiv(currentPopup);
                $.pushState(null, "select city", "city");
                $('#txtCity').cw_fastFilter('#city');
            }
        });
    }
}

function cityChanged(cityLi) {
    selectedCityId = $(cityLi).val();
    selectedCityName = $(cityLi).text();
    $("#hdn_CityName").val(selectedCityName);
    $("#hdn_CityId").val(selectedCityId);
    $('.CityDiv').hide();
    unlockPopup();
    $("#insSelectCity").show();
    $('#insSelectCity').text(selectedCityName);
    $("#costCar").show();
    $('html,body').animate({ 'scrollTop': $('#insSelectCity').offset().top }, 300);
}

function hideLoadingForDiv(currentPopup) {
    currentPopup.find("div.popup_content").show();
    currentPopup.find("div.m-loading-popup").hide();
}

function showLoadingForDiv(currentPopup, prevPopup) {
    prevPopup.hide();
    currentPopup.find("div.popup_content").hide();
    currentPopup.find("div.m-loading-popup").show();
    currentPopup.addClass("popup_layer").show().scrollTop(0);
    window.scrollTo(0, 0);
    lockPopup();
}

function preFillCustomerDetails() {

    $('#txtName').val($.cookie('_CustomerName')).attr('Value', $.cookie('_CustomerName'));
    $('#txtEmail').val($.cookie('_CustEmail')).attr('Value', $.cookie('_CustEmail'));
    $('#txtMobile').val($.cookie('_CustMobile')).attr('Value', $.cookie('_CustMobile'));
}

function setCustomerCookies(custName, custEmail, custMobile) {
    document.cookie = '_CustomerName=' + custName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustEmail=' + custEmail + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustMobile=' + custMobile + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
}

//showDetails
function showDetails() {

    var isError = false;
    if ($('#hdn_VersionId').val() == "") {
        $('#errCar').html("Select Car*.");
        $("#errCar").show();
        isError = true;
    }
    else {
        $('#errCar').html("");
    }
    if ($('#hdn_StateId').val() == "") {
        $('#errState').html("Select State*.");
        $("#errState").show();
        isError = true;
    }
    else {
        $('#errState').html("");
    }
    if ($('#hdn_CityId').val() == "") {
        $('#errCity').html("Select City*.");
        $("#errCity").show();
        isError = true;
    }
    else {
        $('#errCity').html("");
    }

    if ($('#hdn_radio').val() == 1 && $("#txtRegDate").val() == "") {
        $("#errRegDate").text("Please select registration date*.").show();
        isError = true;
    }
    else {
        $("#errRegDate").text("").hide();
    }


    if (isError == true)
        return false;
    else {
        $("#errRegDate").text("").hide();
        $('#step2 .icon.edit-icon').removeClass('hide');        
        $('#step3').removeClass('disabled').trigger('click');
        $('html,body').animate({ 'scrollTop': $('#step3').offset().top }, 300);
        if (policyType == "New")
            dataLayer.push({ event: 'Insurance_mobile', cat: 'Insurance_mobile', act: 'Insurance_Formpage', lab: 'Step2_NEXT_New' });
        else if (policyType == "Used")
            dataLayer.push({ event: 'Insurance_mobile', cat: 'Insurance_mobile', act: 'Insurance_Formpage', lab: 'Step2_NEXT_Renew' });

        return true;       
    }
}
function getInsuranceInputs() {
    apiInput = {};
    apiInput.LeadSource = 43;
    apiInput.MakeId = Number($('#hdn_MakeId').val());
    apiInput.ModelId = Number($('#hdn_ModelId').val());
    apiInput.VersionId = Number($('#hdn_VersionId').val());
    apiInput.CityId = Number($('#hdn_CityId').val());
    apiInput.CityName = $.trim($('#hdn_CityName').val());
    apiInput.StateId = Number($("#hdn_StateId").val());
    apiInput.StateName = $.trim($("#hdn_StateName").val());    
    apiInput.InsuranceNew = Number($('#hdn_radio').val()) == Number($("#rdRenew").val()) ? false : true;
    apiInput.Name = $('#txtName').val();
    apiInput.Email = $('#txtEmail').val();
    apiInput.Mobile = $('#txtMobile').val();
    apiInput.CarPurchaseDate = $("#txtRegDate").val();      
    if (!apiInput.InsuranceNew) {        
        var arraytemp = apiInput.CarPurchaseDate.split(',');
        apiInput.CarManufactureYear = $.trim(arraytemp[1]);        
    }
    var url = window.location.href.toLowerCase();
    var utm = url.indexOf("utm") < 0 ? "" : Common.utils.getValueFromQS('utm');
    apiInput.UtmCode = utm == "" ? "othersmsite" : utm.indexOf("#") < 0 ? utm : utm.split("#")[0] == "" ? "othersmsite" : utm.split("#")[0];
    return apiInput;
}

function sendLeadToOther() {

    apiInput = getInsuranceInputs();

    $("#hdn_VersionId").val("");
    $("input[name=btnCalculate]").val("Please wait...").attr('disabled', 'true');
    $.ajax({
        type: 'POST',
        url: '/api/v2/insurance/quote/',
        data: apiInput,
        contentType: "application/x-www-form-urlencoded",
        headers: { "clientId": clientId },
        dataType: 'Json'
    }).done(function (data) {        
            Insurance.utils.showThanksSuccess(data);        
    });
}

function form_Submit() {

    var isError = false;
    var custName = $('#txtName').val();
    var custEmail = $('#txtEmail').val();
    var custMobile = $('#txtMobile').val();
    var errorMsgs = ValidateContactDetails(custName, custEmail, custMobile);

    //error message for custName
    if (errorMsgs[0] != "") {
        $('#errName').html(errorMsgs[0] + '*');
        $("#errName").show();
        isError = true;
    } else {
        $('#errName').html('');
    }

    //error message for CustEmail
    if (errorMsgs[1] != "") {
        $('#errEmail').html(errorMsgs[1] + '*');
        $("#errEmail").show();
        isError = true;
    } else {
        $('#errEmail').html('');
    }

    //error message for CustMobileNo.
    if (errorMsgs[2] != "") {
        $('#errMobile').html(errorMsgs[2] + '*');
        $("#errMobile").show();
        isError = true;
    } else {
        $('#errMobile').html("");
    }

    if (isError)
        return false;
    else {
        setCustomerCookies(custName, custEmail, custMobile);
        if (policyType == "New")
            dataLayer.push({ event: 'Insurance_mobile', cat: 'Insurance_mobile', act: 'Insurance_Formpage', lab: 'Step3_SUBMIT_New' });
        else if (policyType == "Used")
            dataLayer.push({ event: 'Insurance_mobile', cat: 'Insurance_mobile', act: 'Insurance_Formpage', lab: 'Step3_SUBMIT_Renew' });
        
        checkMobileVerification()

        return false;
    }
}
function checkMobileVerification() {
    var mobileNumber = $.trim($("#txtMobile").val());
    var tollFreeNumber;
    $.ajax({
        type: 'GET',
        headers: { "sourceid": "43" },
        url: '/api/mobile/verify/' + mobileNumber + '/',
        dataType: 'Json',
        success: function (json) {
            if (json.tollFreeNumber) {
                $('#btnMissCallAnchorTag').text(json.tollFreeNumber);
                var telnumber='tel: ' + json.tollFreeNumber.replace(/\s/g, '');
                $('#btnMissCallAnchorTag').attr('href', telnumber);
                if (!json.isMobileVerified) {
                    showOtpPopup();
                }
                else {
                    sendLeadToOther();
                }
            }
            else {
                if (!json.isMobileVerified) {
                    showOtpPopup();
                    $('.otp-call').addClass('hide');
                    $('#buyerForm').addClass('otp-call-hide');
                }
                else {
                    sendLeadToOther();
                }
            }
        },
        fail: function () {
            sendLeadToOther();
        }
    });
}
function verifyOtp() {
    var enteredOTP = $.trim($("#txtOTP").val());
    var mobileNumber = $.trim($("#txtMobile").val());
    if (enteredOTP !== '') {
        $('#imgLoadingBtnVerify').removeClass('hide');

        $.ajax({
            type: 'GET',
            url: '/api/verifymobile/?mobileNo=' + mobileNumber + "&cwiCode=" + enteredOTP,
            headers: { 'CWK': 'KYpLANI09l53DuSN7UVQ304Xnks=', 'SourceId': '43' },
            dataType: 'Json',
            success: function (json) {
                $('#imgLoadingBtnVerify').addClass('hide');
                if (json.responseCode == 1) {
                    $('#otpError').addClass('hide');
                    hideOtpPopup();
                    $("#txtOTP").val("");
                    sendLeadToOther();
                }
                else {
                    $('#otpError').removeClass('hide').text('Please enter valid OTP');
                    console.log("verification error: " + json.responseMessage);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("error: " + errorThrown);
            }
        });
    }
    else $('#otpError').removeClass('hide').text('Please enter 5 digit OTP');
}

function showOtpPopup() {
    $('#buyerForm,#m-blackOut-window').show();
    $('#buyerForm .screen2').slideDown(700);
    lockPopup();
}

function hideOtpPopup() {
    $('#buyerForm').slideUp();
    $('#buyerForm .screen2,#m-blackOut-window').hide();
    unlockPopup();
}

function ContactFixedScroll() {
    var footerTop, contactFixed = $('#contact-fixed'), adDiv = $(document).find('.ad-div');
    var extraDiv = $(document).find('.extraDivHt'), extraDivHt = contactFixed.outerHeight();
    footerTop = $('footer').offset().top - $(window).height() + 60;
    extraDiv.height(extraDivHt);
    if ($(this).scrollTop() > footerTop) {
        contactFixed.removeClass('contact-fixed');
        $('.extraDivHt').hide();
    }
    else {
        contactFixed.addClass('contact-fixed');
        $('.extraDivHt').show();
    }
}

$(window).load(function () {
    if ($('#contact-fixed').is(":visible")) {
        ContactFixedScroll(); 
        $(window).scroll(function () {
            ContactFixedScroll();
        });
    }
});


var Insurance = {
    doc: $(document),
    registerEvents: function () {
        Insurance.doc.on('click', '#rdRenew', function () {
            Common.utils.trackAction('', '', '', '');
        });

        $('#btnVerify').on('click', function () {
            verifyOtp();
        });

        $('#otp-close').on('click', function () {
            hideOtpPopup();
        });
    },
    utils: {
        showThanksSuccess: function (data) {
            var mainForm = $("#mainForm").addClass("hide");
            selectedCityName = $("#hdn_CityName").val();
            if (data != null && data.total != null && data.total > 0) {
                var thankYouString = Common.utils.formatNumeric(Math.floor(data.total)) + " for " + selectedMakeName + " " + selectedModelName + " " + selectedVersionName + " in " + selectedCityName;
                $("#premiumAmt").html(thankYouString);
                $("#thankYou1").removeClass("hide");
                $(window).scrollTop($('#thankYou1').offset().top);
            }
            else {
                if (!data.success)
                    $('#clientMsg').text("Sorry we couldn't fetch a quote basis your requirements.");
                $("#thankYou2").removeClass("hide"); $(window).scrollTop($('#thankYou2').offset().top);
            }
            $.pushState(null, "", "thankyou");
        }
    },
    pageLoad: function () {
        Insurance.registerEvents();
    }
};

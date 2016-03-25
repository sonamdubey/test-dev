var makeCityViewModel = {
    makeName : ko.observable(makeName != "" ? makeName : 'Select brand'),
    selectedMakeId: ko.observable(makeId),
    selectedMakeMaskingName: ko.observable(makeMaskingName),
    cityName : ko.observable(cityName != "" ? cityName : 'Select City'),
    selectedCityId: ko.observable(cityId),
    selectedCityMaskingName: ko.observable(cityMaskingName)
};

ko.applyBindings(makeCityViewModel, $('#divMakeCity')[0]);

var key = "dealerCities_";
lscache.setBucket('DLPage');

$('.listing-filter-btn').on('click', function () {
    $('#dealersFilterWrapper').animate({ 'left': '0' }, 500);
});
$('.filterBackArrow').on('click', function () {
    $('#dealersFilterWrapper').animate({ 'left': '100%' }, 500);
});

var selectBrand = $('#selectBrand'),
    selectCity = $('#selectCity'),
    dealerFilterContent = $('#dealerFilterContent');

selectBrand.on("click", function () {
    $("#dealerFilterContent .dealers-brand-popup-box").show().siblings("div.dealers-city-popup-box").hide();
    animateFilterList();
});

selectCity.on("click", function () {
    $("#dealerFilterContent .dealers-brand-popup-box").hide().siblings("div.dealers-city-popup-box").show();
    animateFilterList();
});

var animateFilterList = function () {
    dealerFilterContent.addClass("open").stop().animate({ 'left': '0' }, 500);
    $(".user-input-box").stop().animate({ 'left': '0' }, 500);
}

$(".dealers-brand-city-wrapper .dealers-back-arrow-box").on("click", function () {
    dealerFilterContent.removeClass("open").stop().animate({ 'left': '100%' }, 500);
    $(".user-input-box").stop().animate({ 'left': '100%' }, 500);
});

$("#dealersBrandInput, #dealersCityInput").on("keyup", function () {
    locationFilter($(this));
});

$(".filter-brand-city-ul").on("click", "li", function () {

    $ulCities = $("#filterCityList");
    $ullMakes = $("#filterBrandList");

    var selectedElement = $(this),
        selectedElementValue = selectedElement.text(),
        selectedElementParent = selectedElement.parent(),
        selectedElementInputField = selectedElementParent.siblings("div.user-input-box"),
        selectedElementParentAttr = selectedElementParent.attr("data-filter-type"),
        selectedElementId = selectedElement.attr("value"),
        selectElementMaskingName = selectedElement.attr("maskingName");

    selectedElementInputField.find("input").val(selectedElementValue);

    if (selectedElementParentAttr == "brand-filter") {       
        makeCityViewModel.makeName(selectedElementValue);
        makeCityViewModel.selectedMakeId(selectedElementId);
        makeCityViewModel.selectedMakeMaskingName(selectElementMaskingName);
        toggleErrorMsg($("#selectBrand"), false);
        makeCityViewModel.selectedCityId(0);
        $ulCities.empty();
        var selMakeId = makeCityViewModel.selectedMakeId();
        startLoading($("#selectCity"));
        if (!isNaN(selMakeId) && selMakeId != "0") {
            if (!checkCacheCityAreas(selMakeId)) {
                $.ajax({
                    type: "GET",
                    url: "/api/v2/DealerCity/?makeId=" + selMakeId,
                    contentType: "application/json",                    
                    success: function (data) {
                        lscache.set(key + selMakeId, data.City, 30);
                        setOptions(data.City);
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            lscache.set(key + selMakeId, null, 30);
                            setOptions(null);
                        }                       
                    }
                });
            }
            else {
                data = lscache.get(key + selMakeId.toString());
                setOptions(data);
            }
        }
        else {
            setOptions(null);
        }

    }
    else {
        makeCityViewModel.cityName(selectedElementValue);
        makeCityViewModel.selectedCityId(selectedElementId);
        makeCityViewModel.selectedCityMaskingName(selectElementMaskingName);
        toggleErrorMsg($("#selectCity"), false);
    }

    setUserSelection();
});

function checkCacheCityAreas(cityId) {
    bKey = key + cityId;
    if (lscache.get(bKey)) return true;
    else return false;
}

function setOptions(optList) {  
    if (optList != null) {
        
        makeCityViewModel.cityName("Select city");

        $.each(optList, function (i, value) {
            $ulCities.append($('<li>').text(value.cityName).attr({ 'value': value.cityId, 'maskingName': value.cityMaskingName }));
        });
    }

    stopLoading($("#selectCity"));
}

$("#applyDealerFilter").click(function () {

    //$ulCities = $("#filterCityList");
    //$ullMakes = $("#filterBrandList");
    var ulmakemasking = makeCityViewModel.selectedMakeMaskingName();
    var ulMakeId = makeCityViewModel.selectedCityId();
    var ulcityId = makeCityViewModel.selectedCityId();   

    if (ulMakeId == "0") {
        toggleErrorMsg($("#selectBrand"), true, "Choose a brand");
    }

    else {
        toggleErrorMsg($("#selectBrand"), false);
    }

    if (ulcityId != "0" && ulMakeId != "0") {
        var ulcityMasking = makeCityViewModel.selectedCityMaskingName();
        toggleErrorMsg($("#selectCity"), false);
        $(".filterBackArrow").trigger("click");
        //window.location.href = "/m/dealerlocator/" + ddlmakemasking + "/" + ddlcityId + "-" + ddlcityMasking + ".html";
        alert(1);
    }
    else {
        toggleErrorMsg($("#selectCity"), true, "Choose a city");
    }
});

var setUserSelection = function () {
    $(".dealers-brand-city-wrapper .dealers-back-arrow-box").trigger("click");
};

$("#dealerFilterReset").on("click", function () {
    makeCityViewModel.makeName("Select brand");
    makeCityViewModel.cityName("Select city");
    makeCityViewModel.selectedMakeId(0);
    makeCityViewModel.selectedCityId(0);
    makeCityViewModel.selectedMakeMaskingName("");
    makeCityViewModel.selectedCityMaskingName("");
    $("#dealerFilterContent").find("input").val("");
});

//$("#applyDealerFilter").on("click", function () {
//    $(".filterBackArrow").trigger("click");
//});

//assistance form
$(".get-assistance-btn").on('click', function () {
    $("#leadCapturePopup").show();
    appendHash("assistancePopup");
    $("div#contactDetailsPopup").show();
    $("#otpPopup").hide();
});$(".leadCapture-close-btn, #notifyOkayBtn").on("click", function () {
    assistancePopupClose();
    window.history.back();
});var assistancePopupClose = function () {
    $("#leadCapturePopup").hide();
    $("#notify-response").hide();
};$("#user-details-submit-btn").on("click", function () {
    if (validateUserDetail()) {
        $("#contactDetailsPopup").hide();        $("#otpPopup").show();        $(".lead-mobile").text($("#getMobile").val());        //$(".notify-leadUser").text($("#getFullName").val());        //$("#notify-response").show();    }
});var validateUserDetail = function () {
    var isValid = true;
    isValid = validateName();
    isValid &= validateEmail();
    isValid &= validateMobile();
    isValid &= validateModel();
    return isValid;
};

var validateName = function () {
    var isValid = true,
        name = $("#getFullName"),
        nameLength = name.val().length;
    if (name.val().indexOf('&') != -1) {
        setError(name, 'Invalid name');
        isValid = false;
    }
    else if (nameLength == 0) {
        setError(name, 'Please enter your name');
        isValid = false;
    }
    else if (nameLength >= 1) {
        hideError(name);
        isValid = true;
    }
    return isValid;
};

var validateEmail = function () {
    var isValid = true,
        emailId = $("#getEmailID"),
        emailVal = emailId.val(),
        reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (emailVal == "") {
        setError(emailId, 'Please enter email address');
        isValid = false;
    }
    else if (!reEmail.test(emailVal)) {
        setError(emailId, 'Invalid Email');
        isValid = false;
    }
    return isValid;
};

var validateMobile = function () {
    var isValid = true,
        mobileNo = $("#getMobile"),
        mobileVal = mobileNo.val(),
        reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(mobileNo, "Please enter your Mobile Number");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(mobileNo, "Mobile number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo)
    return isValid;
};

var validateModel = function () {
    var isValid = true,
        model = $("#getModelName"),
        modelLength = model.val().length;
    if (model.val().indexOf('&') != -1) {
        setError(model, 'Invalid model name');
        isValid = false;
    }
    else if (modelLength == 0) {
        setError(model, 'Please enter model name');
        isValid = false;
    }
    else if (modelLength >= 1) {
        hideError(model);
        isValid = true;
    }
    return isValid;
};

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};

$("#getMobile, #getFullName, #getEmailID, #getModelName, #getUpdatedMobile, #getOTP").on("focus", function () {
    hideError($(this));
});

//otp form
$("#otpPopup .edit-mobile-btn").on("click", function () {
    var prevMobile = $(this).prev("span.lead-mobile").text();
    $(".lead-otp-box-container").hide();
    $(".update-mobile-box").show();
    $("#getUpdatedMobile").val(prevMobile).focus();
});

$("#generateNewOTP").on("click", function () {
    if (validateUpdatedMobile()) {
        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        $(".update-mobile-box").hide();
        $(".lead-otp-box-container").show();
        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
    }
});

var validateUpdatedMobile = function () {
    var isValid = true,
        mobileNo = $("#getUpdatedMobile"),
        mobileVal = mobileNo.val(),
        reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(mobileNo, "Please enter your Mobile number");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(mobileNo, "Mobile number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo)
    return isValid;
};

var otpText = $("#getOTP"),
    otpBtn = $("#otp-submit-btn");

otpBtn.on("click", function () {
    if (validateOTP()) {

    }
});

var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").show();
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = otpText.val();
    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
    }
    else {
        if (isNaN(cwiCode)) {
            retVal = false;
            otpVal("Verification code should be numeric");
        }
        else if (cwiCode.length != 5) {
            retVal = false;
            otpVal("Verification code should be of 5 digits");
        }
    }
    return retVal;
}

function startLoading(ele) {
    try {
        var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
        _self.animate({ width: '100%' }, 7000);
    }
    catch (e) { return };
}

function stopLoading(ele) {
    try {
        var _self = $(ele).find(".progress-bar");
        _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
    }
    catch (e) { return };
}

function toggleErrorMsg(element, error, msg) {
    if (error) {
        element.find('.error-icon').removeClass('hide');
        element.find('.bw-blackbg-tooltip').text(msg).removeClass('hide');
        element.addClass('border-red')
    }
    else {
        element.find('.error-icon').addClass('hide');
        element.find('.bw-blackbg-tooltip').text("").addClass('hide');
        element.removeClass('border-red');
    }
}
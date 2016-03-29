﻿var userLat, userLong;
$(".get-assistance-btn").on('click', function () {
    $("#leadCapturePopup").show();
    appendHash("assistancePopup");
    $("div#contactDetailsPopup").show();
    $("#otpPopup").hide();

    getDealerBikes($(this).attr("data-item-id"));

});

var customerViewModel;
var leadBtnBookNow = $("a.get-assistance-btn"), leadCapturePopup = $("#leadCapturePopup"), fullName = $("#getFullName"), emailid = $("#getEmailID"), mobile = $("#getMobile"), otpContainer = $(".mobile-verification-container"), getModelName = $("#getModelName");
var getCityArea = GetGlobalCityArea();

customerViewModel = new CustomerModel();
ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

function CustomerModel() {  
    var arr = setuserDetails();
    var self = this;
    if (arr != null && arr.length > 0) {
        self.fullName = ko.observable(arr[0]);
        self.emailId = ko.observable(arr[1]);
        self.mobileNo = ko.observable(arr[2]);
    }
    else {
        self.fullName = ko.observable();
        self.emailId = ko.observable();
        self.mobileNo = ko.observable();
    }
    self.cityId = ko.observable(ctyId);
    self.dealerId = ko.observable(dealrId);
    self.versionId = ko.observable(0);
    self.IsVerified = ko.observable(false);
    self.NoOfAttempts = ko.observable(0); 
    self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
    self.otpCode = ko.observable();
    self.pqId = ko.observable();
    self.modelId = ko.observable(0);


    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": self.dealerId(),
                "pqId": self.pqId(),
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "clientIP": clientIP,
                "pageUrl": "/m/dealerlocator/dealerdetails.aspx",
                "versionId": self.versionId(),
                "cityId": self.cityId(),
                "leadSourceId": 2,
                "deviceId": getCookie('BWC')
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                data: ko.toJSON(objCust),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('__utmz'));
                },
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    if (!self.IsVerified()) {
                        self.NoOfAttempts(obj.noOfAttempts);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };


    self.generateOTP = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "pqId": self.pqId(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "cwiCode": self.otpCode(),
                "branchId": self.dealerId(),
                "customerName": self.fullName(),
                "versionId": self.versionId(),
                "cityId": self.cityId()
            }
            $.ajax({
                type: "POST",
                url: "/api/PQMobileVerification/",
                data: ko.toJSON(objCust),
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };

    self.regenerateOTP = function () {
        if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
            var url = '/api/ResendVerificationCode/';
            var objCustomer = {
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "source": 2
            }
            $.ajax({
                type: "POST",
                url: url,
                async: false,
                data: ko.toJSON(objCustomer),
                contentType: "application/json",
                success: function (response) {
                    self.IsVerified(false);
                    self.NoOfAttempts(response.noOfAttempts);
                    alert("You will receive the new OTP via SMS shortly.");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };

    self.generatePQ = function (data, event) {
        self.IsVerified(false);
        isSuccess = false;
        isValidDetails = false;
        isValidDetails = validateUserDetail();

        if (isValidDetails && self.modelId() && self.versionId()) {
            var url = '/api/RegisterPQ/';
            var objData = {
                "dealerId": self.dealerId(),
                "modelId": self.modelId(),
                "clientIP": clientIP,
                "pageUrl": "/m/dealerlocator/dealerdetails.aspx",
                "versionId": self.versionId(),
                "cityId": self.cityId(),
                "areaId": 0,
                "sourceType": 2,
                "pQLeadId": pqSource,
                "deviceId": getCookie('BWC')
            }
            $.ajax({
                type: "POST",
                url: url,
                async: false,
                data: ko.toJSON(objData),
                contentType: "application/json",
                success: function (response) {
                    self.pqId(response.quoteId);
                    isSuccess = true;
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        self.IsVerified(false);
                        isSuccess = false;
                    }

                }
            });
        }

        return isSuccess;

    }

    self.submitLead = function (data, event) {
        var isValidDetails = self.generatePQ(data, event);
        $("#dealer-lead-msg").hide();
        if (isValidDetails) {
            self.verifyCustomer();
            if (self.IsValid()) {
                $("#contactDetailsPopup").hide();
                $("#personalInfo").hide()
                $("#otpPopup").hide();
                $("#dealer-lead-msg").fadeIn();
                $(".lead-mobile").text(self.mobileNo());
                $(".notify-leadUser").text(self.fullName());
                $("#notify-response").show();
            }
            else {
                $("#leadCapturePopup").show();
                $('body').addClass('lock-browser-scroll');
                $(".blackOut-window").show();
                $("#contactDetailsPopup").hide();
                $("#otpPopup").show();
                var leadMobileVal = mobile.val();
                $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();
        }
    };

    function setPQUserCookie() {
        var val = customerViewModel.fullName() + '&' + customerViewModel.emailId() + '&' + customerViewModel.mobileNo();
        SetCookie("_PQUser", val);
    }

    $("body").on('click', '#otp-submit-btn', function () {
        $('#processing').show();
        isValidDetails = false;
        if (!validateOTP())
            $('#processing').hide();
        isValidDetails = validateUserDetail();
        if (validateOTP() && isValidDetails) {
            customerViewModel.generateOTP();
            if (customerViewModel.IsVerified()) {
                $("#personalInfo").hide();
                $(".booking-dealer-details").removeClass("hide").addClass("show");
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                $("#personalInfo").hide();
                $("#otpPopup").hide();
                $("#dealer-lead-msg").fadeIn();
                $(".notify-leadUser").text(self.fullName());
                $("#notify-response").show();

                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
            }
            else {
                $('#processing').hide();
                otpVal("Please enter a valid OTP.");
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
            }
        }
    });
}
$(".leadCapture-close-btn, #notifyOkayBtn").on("click", function () {
    assistancePopupClose();
    window.history.back();
});
var assistancePopupClose = function () {
    $("#leadCapturePopup").hide();
    $("#notify-response").hide();
};
function validateUserDetail() {
    var isValid = true;
    isValid = validateName();
    isValid &= validateEmail();
    isValid &= validateMobile();
    isValid &= validateModel();
    return isValid;
}
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
        model = $('.dealer-search-brand-form');

    if (!model.hasClass('selection-done')) {
        setError(model, 'Please select a model');
        isValid = false;
    }
    else if (model.hasClass('selection-done')) {
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

var brandSearchBar = $("#brandSearchBar"),
                dealerSearchBrand = $(".dealer-search-brand"),
                dealerSearchBrandForm = $(".dealer-search-brand-form");
dealerSearchBrand.on('click', function () {
    $('.dealer-brand-wrapper').show();
    brandSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
    brandSearchBar.find(".user-input-box").animate({ 'left': '0px' }, 500);
    $("#assistanceBrandInput").focus();
});
$("#sliderBrandList").on("click", "li", function () {
    var _self = $(this),
        selectedElement = _self.text();
    setSelectedElement(_self, selectedElement);
    _self.addClass('activeBrand').siblings().removeClass('activeBrand');
    dealerSearchBrandForm.addClass('selection-done').find("span").text(selectedElement);
    brandSearchBar.find(".user-input-box").animate({ 'left': '100%' }, 500);
    hideError(dealerSearchBrandForm);
    customerViewModel.versionId($(this).attr("versionId"));
    customerViewModel.modelId($(this).attr("modelId"));
});
function setSelectedElement(_self, selectedElement) {
    _self.parent().prev("input[type='text']").val(selectedElement);
    brandSearchBar.addClass('open').animate({ 'left': '100%' }, 500);
};
$(".dealer-brand-wrapper .back-arrow-box").on("click", function () {
    brandSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
    brandSearchBar.find(".user-input-box").animate({ 'left': '100%' }, 500);
});
getLocation();
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            setUserLocation
        );
    }
}
function setUserLocation(position) {
    userLat = position.coords.latitude;
    userLong = position.coords.longitude;
    $("#anchorGetDir").attr("href", "https://maps.google.com/?saddr=" + userLat + "," + userLong + "&daddr=" + dealerLat + "," + dealerLong + '');
}
$("#assistanceBrandInput").on("keyup", function () {
    locationFilter($(this));
});
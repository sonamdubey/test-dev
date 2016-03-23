var getCityArea = GetGlobalCityArea();

var leadBtnBookNow = $("#leadBtnBookNow,#leadLink,#leadBtn,#btnEmiQuote"), leadCapturePopup = $("#leadCapturePopup");
var fullName = $("#getFullName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");



var detailsSubmitBtn = $("#user-details-submit-btn, #buyingAssistBtn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");

var prevEmail = "";
var prevMobile = "";

var getCityArea = GetGlobalCityArea();
var customerViewModel = new CustomerModel();

$(function () {
    leadBtnBookNow.on('click', function () {
        leadCapturePopup.show();
        $("#dealer-lead-msg").hide();
        $("div#contactDetailsPopup").show();
        $("#otpPopup").hide();
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window").show();

    });
    $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
        leadCapturePopup.hide();
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
    });

    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) {
            $("#leadCapturePopup .leadCapture-close-btn").click();
            $("div.termsPopUpCloseBtn").click();
        }
    });
});

if ($('#dealerAssistance').length > 0)
    ko.applyBindings(customerViewModel, $('#dealerAssistance')[0]);

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
    self.IsVerified = ko.observable(false);
    self.NoOfAttempts = ko.observable(0);
    self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
    self.otpCode = ko.observable();
    self.isAssist = ko.observable(false);

    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": dealerId,
                "pqId": pqId,
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": versionId,
                "cityId": cityId,
                "leadSourceId": 1,
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
                "pqId": pqId,
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "cwiCode": self.otpCode(),
                "branchId": dealerId,
                "customerName": self.fullName(),
                "versionId": versionId,
                "cityId": cityId
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
                "source": 1
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

    self.submitLead = function () {
        $("#dealer-lead-msg").hide();
        self.IsVerified(false);
        isValidDetails = false;
        if (event.target.id == 'buyingAssistBtn') {
            self.isAssist(true);
            isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
        }
        else {
            self.isAssist(false);
            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
        }
        if (isValidDetails) {
            self.verifyCustomer();
            if (self.IsValid()) {
                if (self.isAssist()) {
                    $("#buying-assistance-form").hide();
                    $("#dealer-assist-msg").fadeIn();

                } else {
                    $("#contactDetailsPopup").hide();
                    $("#personalInfo").hide()
                    $("#otpPopup").hide();
                    $("#dealer-lead-msg").fadeIn();
                }
            }
            else {
                $("#leadCapturePopup").show();
                $('body').addClass('lock-browser-scroll');
                $(".blackOut-window").show();
                $("#contactDetailsPopup").hide();
                $("#otpPopup").show();
                var leadMobileVal = mobile.val();
                $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                otpContainer.removeClass("hide").addClass("show");
                //nameValTrue();
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Lead_Submitted', 'lab': bikeName + '_' + getCityArea });
        }
    };

    otpBtn.click(function () {
        $('#processing').show();
        isValidDetails = false;
        if (!validateOTP())
            $('#processing').hide();

        if (event.currentTarget.id == 'buyingAssistBtn') {
            self.isAssist(true);
            isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
        }
        else {
            self.isAssist(false);
            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
        }

        if (validateOTP() && isValidDetails) {
            customerViewModel.generateOTP();
            if (customerViewModel.IsVerified()) {
                $("#personalInfo").hide();
                $(".booking-dealer-details").removeClass("hide").addClass("show");
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                $("#personalInfo").hide()
                $("#otpPopup").hide();

                if (self.isAssist()) {
                    $("#leadCapturePopup .leadCapture-close-btn").click();
                    $("#buying-assistance-form").hide();
                    $("#dealer-assist-msg").fadeIn();
                }
                else {
                    $("#dealer-lead-msg").fadeIn();
                }
                
                // OTP Success
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
            }
            else {
                $('#processing').hide();
                otpVal("Please enter a valid OTP.");
                // push OTP invalid
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
            }
        }
    });
}

function ValidateUserDetail(fullName, emailid, mobile) {
    return validateUserInfo(fullName, emailid, mobile);
};


var prevEmail = "",
	prevMobile = "";

$("#assistanceGetName,#getFullName").on("focus", function () {
    hideError($(this));
});

$("#assistanceGetEmail,#getEmailID").on("focus", function () {
    hideError($(this));
    prevEmail = $(this).val().trim();
});

$("#assistanceGetMobile,#getMobile,#getUpdatedMobile").on("focus", function () {
    hideError($(this));
    prevMobile = $(this).val().trim();
});



$("#assistanceGetEmail,#getEmailID").on("blur", function () {
    if (prevEmail != $(this).val().trim()) {
        if (validateEmailId($(this))) {
            customerViewModel.IsVerified(false);
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError($(this));
        }
    }
});

$("#assistanceGetMobile,#getMobile,#getUpdatedMobile").on("blur", function () {
    if (prevMobile != $(this).val().trim()) {
        if (validateMobileNo($(this))) {
            customerViewModel.IsVerified(false);
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError($(this));
        }
    }
});

function mobileValTrue() {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};


otpText.on("focus", function () {
    otpText.val('');
    otpText.siblings("span, div").hide();
});

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};
var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").show();
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = otpText.val();
    customerViewModel.IsVerified(false);
    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
        bindInsuranceText();
    }
    else {
        if (isNaN(cwiCode)) {
            retVal = false;
            otpVal("Verification Code should be numeric");
        }
        else if (cwiCode.length != 5) {
            retVal = false;
            otpVal("Verification Code should be of 5 digits");
        }
    }
    return retVal;
}

function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

function setPQUserCookie() {
    var val = customerViewModel.fullName() + '&' + customerViewModel.emailId() + '&' + customerViewModel.mobileNo();
    SetCookie("_PQUser", val);
}

$(".edit-mobile-btn").on("click", function () {
    var prevMobile = $(this).prev("span.lead-mobile").text();
    $(".lead-otp-box-container").hide();
    $(".update-mobile-box").show();
    $("#getUpdatedMobile").val(prevMobile).focus();
});

$("#generateNewOTP").on("click", function () {
    if (validateMobileNo($("#getUpdatedMobile"))) {
        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        $(".update-mobile-box").hide();
        $(".lead-otp-box-container").show();
        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
    }
});


var variantsDropdown = $(".variants-dropdown"),
variantSelectionTab = $(".variant-selection-tab"),
variantUL = $(".variants-dropdown-list"),
variantListLI = $(".variants-dropdown-list li");

variantsDropdown.click(function (e) {
    if (!variantsDropdown.hasClass("open"))
        $.variantChangeDown(variantsDropdown);
    else
        $.variantChangeUp(variantsDropdown);
});

$.variantChangeDown = function (variantsDropdown) {
    variantsDropdown.addClass("open");
    variantUL.show();
};

$.variantChangeUp = function (variantsDropdown) {
    variantsDropdown.removeClass("open");
    variantUL.slideUp();
};


$(document).mouseup(function (e) {
    if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
        $.variantChangeUp($(".variants-dropdown"));
    }
});

var assistanceGetName = $('#assistanceGetName'),
    assistanceGetEmail = $('#assistanceGetEmail'),
    assistanceGetMobile = $('#assistanceGetMobile');


var validateUserInfo = function (leadUsername, leadEmailId, leadMobileNo) {
    var isValid = true;
    isValid = validateUserName(leadUsername);
    isValid &= validateEmailId(leadEmailId);
    isValid &= validateMobileNo(leadMobileNo);
    return isValid;
};

var validateUserName = function (leadUsername) {
    var isValid = true,
		nameLength = leadUsername.val().length;
    if (leadUsername.val().indexOf('&') != -1) {
        setError(leadUsername, 'Invalid name');
        isValid = false;
    }
    else if (nameLength == 0) {
        setError(leadUsername, 'Please enter your name');
        isValid = false;
    }
    else if (nameLength >= 1) {
        hideError(leadUsername);
        isValid = true;
    }
    return isValid;
};

var validateEmailId = function (leadEmailId) {
    var isValid = true,
		emailVal = leadEmailId.val(),
		reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (emailVal == "") {
        setError(leadEmailId, 'Please enter email id');
        isValid = false;
    }
    else if (!reEmail.test(emailVal)) {
        setError(leadEmailId, 'Invalid Email');
        isValid = false;
    }
    return isValid;
};

var validateMobileNo = function (leadMobileNo) {
    var isValid = true,
		mobileVal = leadMobileNo.val(),
		reMobile = /^[1-9][0-9]{9}$/;
    if (mobileVal == "") {
        setError(leadMobileNo, "Please enter your mobile no.");
        isValid = false;
    }
    else if (mobileVal[0] == "0") {
        setError(leadMobileNo, "Mobile no. should not start with zero");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(leadMobileNo, "Mobile no. should be 10 digits only");
        isValid = false;
    }
    else
        hideError(leadMobileNo)
    return isValid;
};

$(document).ready(function () {
    if ($('#pqBikeDetails').height() < 400) {
        $('#PQDealerSidebarContainer').css({'padding-bottom': '20px'});
        $('#PQDealerSidebarContainer .pqdealer-and-listing-container').css({ 'height': '360px'});
    }
    var $window = $(window),
        disclaimerText = $('#disclaimerText'),
        PQDealerSidebarContainer = $('#PQDealerSidebarContainer'),
        dealerPriceQuoteContainer = $('#dealerPriceQuoteContainer'),
        PQDealerSidebarHeight;
    $(window).scroll(function () {
        PQDealerSidebarHeight = PQDealerSidebarContainer.height();
        var windowScrollTop = $window.scrollTop(),
            disclaimerTextOffset = disclaimerText.offset(),
            dealerPriceQuoteContainerOffset = dealerPriceQuoteContainer.offset();
        if ($('#dealerPriceQuoteContainer').height() > 500) {
            if (windowScrollTop < dealerPriceQuoteContainerOffset.top - 50) {
                PQDealerSidebarContainer.css({ 'position': 'relative', 'top': '0', 'right' : '0' })
            }
            else if (windowScrollTop > (disclaimerTextOffset.top - PQDealerSidebarHeight - 80)) {
                PQDealerSidebarContainer.css({ 'position': 'relative', 'top': disclaimerTextOffset.top - PQDealerSidebarHeight - 150, 'right': '0' })
            }
            else {
                PQDealerSidebarContainer.css({ 'position': 'fixed', 'top': '50px', 'right': $(window).innerWidth() - (996 + $('#dealerPriceQuoteContainer').offset().left - 11) })
            }
        }
    });
});



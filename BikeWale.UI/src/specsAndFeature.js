var leadBtnBookNow = $("#leadBtnBookNow"), leadCapturePopup = $("#leadCapturePopup");
var fullName = $("#getFullName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");
var detailsSubmitBtn = $("#user-details-submit-btn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");
var prevEmail = "";
var prevMobile = "";

var customerViewModel = new CustomerModel();

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
                "leadSourceId": leadSourceId,
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
                dataType: 'json',
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
                dataType: 'json',
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
                dataType: 'json',
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

    self.submitLead = function (data, event) {
        if (event.target.id == "assistFormSubmit") {
            leadSourceId = $(event.target).attr("leadSourceId");
        }
        self.IsVerified(false)
        var isValidCustomer = ValidateUserDetail(fullName, emailid, mobile);
        if (isValidCustomer && isDealerPriceAvailable == "True" && campaignId == 0) {
            self.verifyCustomer();
            if (self.IsValid()) {
                if ($("#leadCapturePopup").css('display') === 'none') {
                    $("#leadCapturePopup").show();
                    $(".blackOut-window-model").show();
                }
                $("#contactDetailsPopup,#otpPopup").hide();
                $('#notify-response .notify-leadUser').text(self.fullName());
                $('#notify-response').show();

                if (getOffersClick) {
                    //dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": "Main_Form_" + bikeVersionLocation });
                    getOffersClick = false;
                }
                else if (event.target.id == "assistFormSubmit") {
                    //dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": "Open_Form_" + bikeVersionLocation });
                }
                else {
                    //dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": bikeVersionLocation });
                }
            }
            else {
                $("#leadCapturePopup").show();
                $("#contactDetailsPopup").hide();
                $("#otpPopup").show();
                $(".blackOut-window-model").show();
                var leadMobileVal = mobile.val();
                $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                otpContainer.removeClass("hide").addClass("show");
                nameValTrue();
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();
        }

        else if (isValidCustomer && isDealerPriceAvailable == "False" && campaignId > 0) {
            self.submitCampaignLead();

            setPQUserCookie();

            if (getOffersClick) {
                //dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": "Main_Form_" + bikeVersionLocation });
                getOffersClick = false;
            }
            else if (event.target.id == "assistFormSubmit") {
                //dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": "Open_Form_" + bikeVersionLocation });
            }
            else if (leadSourceId == "24") {
                //dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": "Floating_Card_" + bikeVersionLocation });
            }
            else {
                //dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": bikeVersionLocation });
            }
        }
    };

    self.submitCampaignLead = function () {

        $('#processing').show();
        var objCust = {
            "dealerId": manufacturerId,
            "pqId": pqId,
            "name": self.fullName(),
            "mobile": self.mobileNo(),
            "email": self.emailId(),
            //"clientIP": clientIP,
            //"pageUrl": pageUrl,
            "versionId": versionId,
            "cityId": cityId,
            "leadSourceId": leadSourceId,
            "deviceId": getCookie('BWC')
        }
        $.ajax({
            type: "POST",
            url: "/api/ManufacturerLead/",
            data: ko.toJSON(objCust),
            beforeSend: function (xhr) {
                xhr.setRequestHeader('utma', getCookie('__utma'));
                xhr.setRequestHeader('utmz', getCookie('__utmz'));
            },
            async: false,
            contentType: "application/json",
            dataType: 'json',
            success: function (response) {
                $("#personalInfo,#otpPopup").hide();
                $('#processing').hide();
                $("#contactDetailsPopup").hide();
                $('#notify-response .notify-leadUser').text(self.fullName());
                $('#notify-response').show();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#processing').hide();
                $("#contactDetailsPopup,#otpPopup").hide();
                var leadMobileVal = mobile.val();
                nameValTrue();
                hideError(self.mobileNo());
            }
        });
    };

    otpBtn.click(function () {
        $('#processing').show();
        if (!validateOTP())
            $('#processing').hide();
        if (validateOTP() && ValidateUserDetail(fullName, emailid, mobile)) {
            customerViewModel.generateOTP();
            if (customerViewModel.IsVerified()) {
                $("#personalInfo").hide();
                $(".booking-dealer-details").removeClass("hide").addClass("show");
                $('#processing').hide();
                detailsSubmitBtn.show();
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                //$("#leadCapturePopup .leadCapture-close-btn").click();
                $("#contactDetailsPopup,#otpPopup").hide();
                $('#notify-response .notify-leadUser').text(self.fullName());
                $('#notify-response').show();
            }
            else {
                $('#processing').hide();
                otpVal("Please enter a valid OTP.");
            }
        }
    });
}

function ValidateUserDetail(parameterName, parameterEmail, parameterMobile) {
    var isValid = true;
    isValid = validateEmail(parameterEmail);
    isValid &= validateMobile(parameterMobile);
    isValid &= validateName(parameterName);
    return isValid;
};

function validateName(parameterName) {
    var isValid = true;
    var a = parameterName.val().length;
    if ((/&/).test(parameterName.val())) {
        isValid = false;
        setError(parameterName, 'Invalid name');
    }
    else
        if (a == 0) {
            isValid = false;
            setError(parameterName, 'Please enter your name');
        }
        else if (a >= 1) {
            isValid = true;
            nameValTrue(parameterName)
        }
    return isValid;
}

function nameValTrue(parameterName) {
    if (parameterName != null) {
        hideError(parameterName)
        parameterName.siblings("div").text('');
    }
};

$("#getFullName, #assistGetName").on("focus", function () {
    hideError($(this));
});

$("#getEmailID, #assistGetEmail").on("focus", function () {
    hideError($(this));
    prevEmail = $(this).val().trim();
});

$("#getMobile, #assistGetMobile").on("focus", function () {
    hideError($(this));
    prevMobile = $(this).val().trim();

});

emailid.on("blur", function () {
    if (prevEmail != emailid.val().trim()) {
        if (validateEmail(emailid)) {
            customerViewModel.IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(emailid);
        }
    }
});

mobile.on("blur", function () {
    if (mobile.val().length < 10) {
        $("#user-details-submit-btn").show();
        $(".mobile-verification-container").removeClass("show").addClass("hide");
    }
    if (prevMobile != mobile.val().trim()) {
        if (validateMobile(mobile)) {
            customerViewModel.IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(mobile);
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

function setError(ele, msg) {
    ele.addClass("border-red");
    ele.siblings("span, div").show();
    ele.siblings("div").text(msg);
}

function hideError(ele) {
    if (ele != null) {
        ele.removeClass("border-red");
        ele.siblings("span, div").hide();
    }
}
/* Email validation */
function validateEmail(parameterEmail) {
    var isValid = true;
    var emailID = parameterEmail.val();
    var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailID == "") {
        setError(parameterEmail, 'Please enter email address');
        isValid = false;
    }
    else if (!reEmail.test(emailID)) {
        setError(parameterEmail, 'Invalid Email');
        isValid = false;
    }
    return isValid;
}

function validateMobile(parameterMobile) {
    var isValid = true;
    var reMobile = /^[0-9]{10}$/;
    var mobileNo = parameterMobile.val();
    if (mobileNo == "") {
        isValid = false;
        setError(parameterMobile, "Please enter your mobile no.");
    }
    else if (!reMobile.test(mobileNo) && isValid) {
        isValid = false;
        setError(parameterMobile, "Number should be 10 digits");
    }
    else {
        hideError(parameterMobile)
    }
    return isValid;
}

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

function setPQUserCookie() {
    var val = fullName.val() + '&' + emailid.val() + '&' + mobile.val();
    SetCookie("_PQUser", val);
}

$(".edit-mobile-btn").on("click", function () {
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
        setError(mobileNo, "Please enter your Mobile Number");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(mobileNo, "Mobile Number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo)
    return isValid;
};

var getOnRoadPriceBtn = $("#getOnRoadPriceBtn");

$("#getOnRoadPriceBtn").on("click", function () {
    $("#onRoadPricePopup").show();
    $(".blackOut-window").show();
});

$(".leadCapture-close-btn, #notifyOkayBtn").on("click", function () {
    leadCapturePopup.hide();
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window-model").hide();
    $("#contactDetailsPopup").show();
    $("#otpPopup,#notify-response").hide();
});

var assistFormSubmit = $('#assistFormSubmit'),
    assistGetName = $('#assistGetName'),
    assistGetEmail = $('#assistGetEmail'),
    assistGetMobile = $('#assistGetMobile');

assistFormSubmit.on('click', function () {
    leadSourceId = $(this).attr("leadSourceId");
    ValidateUserDetail(assistGetName, assistGetEmail, assistGetMobile);
});
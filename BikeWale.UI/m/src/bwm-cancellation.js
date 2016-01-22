// JavaScript Document
var BookingCancellationViewModel = function () {
    var self = this;
    self.CurrentStep = ko.observable(1);
    self.ActualSteps = ko.observable(1);

    self.userBikeDetails = function (data, event) {
        if (validateUserBikeDetails()) {
            otpPopupOpen();
            $("#otpPopup .lead-mobile").text($("#getUserRegisteredNum").val());
        }
    };

    self.processOTP = function (data, event) {
        otpPopupClose();
        if ($("#getBikeBookingId").val() === "bw123") { // if booking period has exceeded 15 days for id:bw123
            $("#cancellationStepsWrapper").hide();
            $("#processResponse").show();
        }
        else {
            self.CurrentStep(2);
            self.ActualSteps(2);
        }
    };

    self.cancelBooking = function (data, event) {
        self.CurrentStep(3);
        self.ActualSteps(3);
        $('html, body').animate({ scrollTop: $(".cancellation-tabs").offset().top });
    };

    self.sendFeedback = function (data, event) {
        self.CurrentStep(-1);
        self.ActualSteps(-1);
    };

};

var viewModel = new BookingCancellationViewModel();

ko.applyBindings(viewModel, $('#cancellationPage')[0]);

var validateUserBikeDetails = function () {
    var getUserRegisteredNum = $("#getUserRegisteredNum");
    var isValid = true;
    isValid = validateBookingID();
    isValid &= validateMobile(getUserRegisteredNum);

    return isValid;
};

var validateBookingID = function () {
    var isValid = true,
		bookingID = $("#getBikeBookingId"),
		bookingIDValue = bookingID.val(),
		pattern = /^bw/i,
		result = bookingIDValue.match(pattern);
    if (bookingIDValue == "") {
        setError(bookingID, 'Please enter a booking ID');
        isValid = false;
    }
    else if (!result) {
        setError(bookingID, 'Please enter a valid booking ID');
        isValid = false;
    }
    else if (result)
        hideError(bookingID);
    return isValid;
};

var validateMobile = function (getMobileNum) {
    var isValid = true,
		mobileNo = getMobileNum,
		mobileVal = mobileNo.val(),
		reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(mobileNo, "Please enter your mobile number");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(mobileNo, "Mobile number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo);
    return isValid;
};

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};

$("#getBikeBookingId, #getUserRegisteredNum").on("focus", function () {
    hideError($(this));
});

$("#getBikeBookingId, #getUserRegisteredNum").on("change", function () {
    viewModel.CurrentStep(1);
    viewModel.ActualSteps(1);
});

$(".otpPopup-close-btn").mouseup(function (e) {
    otpPopupClose();
});

function otpPopupOpen() {
    $("#otpPopup").show();
};

function otpPopupClose() {
    $("#otpPopup").hide();
    $(".blackOut-window").hide();
};

$("#otpPopup .edit-mobile-btn").on("click", function () {
    var prevMobile = $(this).prev("span.lead-mobile").text();
    $(".lead-otp-box-container").hide();
    $(".update-mobile-box").show();
    $("#getUpdatedMobile").val(prevMobile).focus();
});

$("#generateNewOTP").on("click", function () {
    var getUpdatedMobile = $("#getUpdatedMobile");
    if (validateMobile(getUpdatedMobile)) {
        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        $(".update-mobile-box").hide();
        $(".lead-otp-box-container").show();
        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
    }
});

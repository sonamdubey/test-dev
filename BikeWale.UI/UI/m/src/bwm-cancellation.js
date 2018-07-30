var prevData = "";
var otpText = $("#getOTP");

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

$(".otpPopup-close-btn").on("click", function (e) {
    otpPopupClose();
    window.history.back();
});

function otpPopupOpen() {
    $("#otpPopup").show();
};

function otpPopupClose() {
    $("#otpPopup").hide();
    $(".blackOut-window").hide();
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = $("#getOTP").val().trim();
    viewModel.User().IsValidBooking(false);
    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
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

var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").css("display", "block");
    otpText.siblings("div").text(msg);
};


var UserDetails = function () {
    var self = this;
    self.BookingId = ko.observable();
    self.Mobile = ko.observable();
    self.IsValidBooking = ko.observable(false);
    self.OTPCode = ko.observable();
    self.ErrMessage = ko.observable("");
    self.IsValidOTP = ko.observable();
    self.OtpAttempts = ko.observable(0);
    self.Name = ko.observable();
    self.MobileNo = ko.observable();
    self.Email = ko.observable();
    self.BookingId = ko.observable();
    self.BikeName = ko.observable();
    self.BookingDate = ko.observable();
    self.IsCancelled = ko.observable(false);
    self.PQId = ko.observable(0);

    self.verifyBooking = function (data, event) {
        isSuccess = false;
        if (validateUserBikeDetails()) {
            objData = { "bwId": self.BookingId(), "mobile": self.Mobile() };
            $.ajax({
                type: "POST",
                url: "/api/bookingcancellation/isvalidrequest/",
                data: ko.toJSON(objData),
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    if (obj.isVerified && obj.responseFlag == 1) {
                        self.IsValidBooking(obj.isVerified);
                        otpPopupOpen();
                        isSuccess = true;
                        appendHash("otp");
                    }
                    else isSuccess = false;
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        self.IsValidBooking(false);
                        self.ErrMessage("Either your booking id or mobile number is incorrect or we have already processed your cancellation request.");
                        isSuccess = false;
                    }
                }
            });
        }
        return isSuccess;
    }

    //self.regenerateOTP = function () {
    //    if (self.OtpAttempts() <= 2 && !self.IsVerified()) {
    //        var url = '/api/ResendVerificationCode/';
    //        var objData = {
    //            "BookingId": self.BookingId(),
    //            "Mobile": self.Mobile()
    //        };
    //        $.ajax({
    //            type: "POST",
    //            url: url,
    //            async: false,
    //            data: ko.toJSON(objData),
    //            contentType: "application/json",
    //            success: function (response) {
    //                self.IsVerified(false);
    //                self.OtpAttempts(response.noOfAttempts);
    //                alert("You will receive the new OTP via SMS shortly.");
    //            },
    //            error: function (xhr, ajaxOptions, thrownError) {
    //                self.IsVerified(false);
    //            }
    //        });
    //    }
    //};

    self.verifyOTP = function (data, event) {
        var isSuccess = false;
        if (validateOTP() && validateUserBikeDetails()) {
            var url = "/api/bookingcancellation/isvalidotp/";
            var objData = {
                "bwId": self.BookingId(),
                "mobile": self.Mobile(),
                "otp": self.OTPCode()
            };
            $.ajax({
                type: "POST",
                url: url,
                async: false,
                data: ko.toJSON(objData),
                contentType: "application/json",
                success: function (response) {
                    objCust = ko.toJS(response);
                    if (objCust.isCancellable > 0) {
                        if (objCust.isCancellable == 1) {
                            self.Name(objCust.CustomerName);
                            self.MobileNo(objCust.CustomerMobile);
                            self.Email(objCust.CustomerEmail);
                            self.BookingId(viewModel.User().BookingId());
                            self.BikeName(objCust.BikeName);
                            self.BookingDate(objCust.BookingDate);
                            self.IsCancelled(false);
                            self.PQId(objCust.PQId);
                        }
                        else {
                            self.IsCancelled(true);
                            $("#cancellationStepsWrapper").hide();
                            $("#processResponse").show();
                        }

                        self.OTPCode("");
                        $("#otpPopup").hide();
                        $(".blackOut-window").hide();
                        isSuccess = true;
                        self.IsValidOTP(true);
                        viewModel.CurrentStep(2);
                        viewModel.ActualSteps(2);
                    }
                    else {
                        self.IsCancelled(false);
                        self.Name();
                        self.MobileNo();
                        self.Email();
                        self.BookingId();
                        self.BikeName();
                        self.BookingDate();
                        self.PQId(0);
                        otpText.addClass("border-red");
                        otpText.siblings("span, div").css("display", "block");
                        otpText.siblings("div").text("Please enter a valid OTP.");
                        isSuccess = false;
                        self.IsValidOTP(false);
                    }
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        self.IsCancelled(false);
                        $('#processing').hide();
                        otpText.addClass("border-red");
                        otpText.siblings("span, div").css("display", "block");
                        otpText.siblings("div").text("Please enter a valid OTP.");
                        isSuccess = false;
                        self.IsValidOTP(false);
                    }
                }
            });
        }
        return isSuccess;
    };

    self.cancelBooking = function (data, event) {
        isSuccess = false;
        if (confirm("Do you really want to cancel your bike booking ?")) {
            if (validateUserBikeDetails() && self.PQId() > 0) {
                $('#hdnBwid').val(self.BookingId());
                var url = "/api/bookingcancellation/confirm/?pqId=" + self.PQId();
                $.ajax({
                    type: "POST",
                    url: url,
                    async: false,
                    data: ko.toJSON(objData),
                    contentType: "application/json",
                    success: function (response) {
                        if (response) {
                            isSuccess = true;
                            viewModel.CurrentStep(3);
                            viewModel.ActualSteps(3);
                        }
                        else {
                            isSuccess = false;
                        }
                    },
                    complete: function (xhr, ajaxOptions, thrownError) {
                        if (xhr.status == 404 || xhr.status == 204) {
                        }
                    }
                });
            }
        }

        return isSuccess;

    };


};


var BookingCancellationViewModel = function () {
    var self = this;
    self.CurrentStep = ko.observable(1);
    self.ActualSteps = ko.observable(1);
    self.User = ko.observable(new UserDetails());

};

var viewModel = new BookingCancellationViewModel();
ko.applyBindings(viewModel, $('#cancellationPage')[0]);

// JavaScript Document
var validateTabB = false,
	validateTabC = false,
	bikeSummary = $("#bikeSummary"),
	bikeSummaryTab = $("#bikeSummaryTab"),
	deliveryDetails = $("#deliveryDetails"),
	deliveryDetailsTab = $("#deliveryDetailsTab"),
	bikePayment = $("#bikePayment"),
	bikePaymentTab = $("#bikePaymentTab");
otpText = $("#getOTP");

$(".select-dropdown").on("click", function () {
    if (!$(this).hasClass("open")) {
        selectStateDown($(this));
    }
    else
        selectStateUp($(this));
});

var selectStateDown = function (div) {
    $(".select-dropdown").removeClass("open");
    $(".select-dropdown").next("div.select-dropdown-list").slideUp();
    div.addClass("open");
    div.next("div.select-dropdown-list").slideDown();
};

var selectStateUp = function (div) {
    div.removeClass("open");
    div.next("div.select-dropdown-list").slideUp();
};

$(document).mouseup(function (e) {
    if (!$(".select-dropdown, .select-dropdown span.select-btn, .select-dropdown .upDownArrow").is(e.target)) {
        selectStateUp($(".select-dropdown"));
    }
});


var validateUserDetail = function () {
    var isValid = true;
    isValid = validateName();
    isValid &= validateEmail();
    isValid &= validateMobile();

    return isValid;
};

var validateName = function () {
    var isValid = true,
		name = $("#getLeadName"),
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
        setError(mobileNo, "Mobile Number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo)
    return isValid;
};

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};

var prevEmail = "",
	prevMobile = "";

$("#getMobile,#getLeadName,#getEmailID,#getOTP,#getUpdatedMobile").on("focus", function () {
    hideError($(this));
    prevMobile = $(this).val().trim();
});




$(".otpPopup-close-btn, .blackOut-window").mouseup(function (e) {
    otpPopupClose();
});

$(document).keydown(function (e) {
    if (e.keyCode == 27)
        otpPopupClose();
});

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

var versionul = $("#customizeBike ul.select-versionUL");
var colorsul = $("#customizeBike ul.select-colorUL");

var BookingPageViewModel = function () {
    var self = this;
    self.Bike = ko.observable(new BikeDetails);
    self.Dealer = ko.observable(new BikeDealerDetails);
    self.Customer = ko.observable(new BikeCustomer);
    self.CurrentStep = ko.observable(1);
    self.SelectedVersionId = ko.observable();
    self.SelectedColorId = ko.observable(0);
    self.UserOptions = ko.observable();
    self.ActualSteps = ko.observable(1);
    self.CustomerInfo = ko.observable();
    self.changedSteps = function () {
        if (self.Bike().selectedVersionId() > 0) {
            self.SelectedVersionId(self.Bike().selectedVersionId());
            if (self.Bike().selectedColorId() > 0) {
                self.SelectedColorId(self.Bike().selectedColorId());
                if (self.CurrentStep() != 3) {
                    self.CurrentStep(self.CurrentStep() + 1);
                    self.ActualSteps(self.ActualSteps() + 1)
                }
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }

    };

    self.verifyCustomer = function (data, event) {
        var isSuccess = false, validate = validateUserDetail();
        var curCustInfo = viewModel.Customer().EmailId().trim() + viewModel.Customer().MobileNo().trim();
        if (self.CustomerInfo() != curCustInfo) {
            if (validate && self.Customer().IsVerified(false)) {
                var objCust = {
                    "dealerId": self.Dealer().DealerId,
                    "pqId": self.Dealer().PQId,
                    "customerName": self.Customer().Name,
                    "customerMobile": self.Customer().MobileNo(),
                    "customerEmail": self.Customer().EmailId(),
                    "clientIP": clientIP,
                    "pageUrl": pageUrl,
                    "versionId": self.SelectedVersionId(),
                    "cityId": self.Dealer().CityId,
                    "colorId": self.SelectedColorId
                }

                $.ajax({
                    type: "POST",
                    url: "/api/UpdatePQCustomerDetails/",
                    data: ko.toJSON(objCust),
                    async: false,
                    contentType: "application/json",
                    success: function (response) {
                        var obj = ko.toJS(response);
                        self.Customer().IsVerified(obj.isSuccess);
                        self.Customer().OtpAttempts(obj.noOfAttempts)
                        if (!self.Customer().IsVerified() && self.Customer().OtpAttempts() != -1) {
                            //getotp code here
                            $("#otpPopup").show();
                            $('.update-mobile-box').hide().siblings().show();
                            $(".blackOut-window").show();
                            isSuccess = false;

                        }
                        else {
                            self.Customer().IsVerified();
                            self.changedSteps();
                            self.CustomerInfo(curCustInfo);
                            isSuccess = true;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        self.Customer().IsVerified(false);
                        $("#otpPopup").show();
                        $('.update-mobile-box').hide().siblings().show();
                        $(".blackOut-window").show();
                        isSuccess = false;
                    }
                });
            }
            else {
                if (validate)
                {
                    $("#otpPopup").show();
                    $(".blackOut-window").show();
                }                
                isSuccess = false;
            }
        }           
        else {
            isSuccess = true;
            self.changedSteps();
        }

        if (!isSuccess) {
            event.preventDefault();
            event.stopPropagation();
            return false;
        }
        else {
            return true;
        }
    };

    self.bookNow = function (data, event) {
        var isSuccess = false;
        if (self.Customer().IsVerified() && (self.CurrentStep() >= 2) && (self.Bike().bookingAmount() > 0)) {
            var curUserOptions = self.Bike().selectedVersionId().toString() + self.Bike().selectedColorId().toString();
            if (self.UserOptions() != curUserOptions) {
                self.UserOptions(curUserOptions);

                url = "/api/UpdatePQ/";
                var objData = {
                    "pqId": self.Dealer().PQId(),
                    "versionId": self.Bike().selectedVersionId(),
                }
                $.ajax({
                    type: "POST",
                    url: (self.Bike().selectedColorId() > 0) ? url + "?colorId=" + self.Bike().selectedColorId() : url,
                    async: false,
                    data: ko.toJSON(objData),
                    contentType: "application/json",
                    success: function (response) {
                        var obj = ko.toJS(response);
                        if (obj.isUpdated) {
                            isSuccess = true;
                            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + self.Dealer().PQId() + "&VersionId=" + self.Bike().selectedVersionId() + "&DealerId=" + self.Dealer().DealerId();
                            SetCookie("_MPQ", cookieValue);
                            isSuccess = true;
                        }
                        else isSuccess = false;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        isSuccess = false;
                    }
                });
            }
            else {
                isSuccess = true;
            }
        }

        return isSuccess;

    };
}

var BikeCustomer = function () {
    var self = this;
    self.Name = ko.observable();
    self.MobileNo = ko.observable();
    self.EmailId = ko.observable();
    self.IsVerified = ko.observable(false);
    self.OtpAttempts = ko.observable(0);
    self.OtpCode = ko.observable();

    self.verifyOtp = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "pqId": viewModel.Dealer().PQId(),
                "customerName": self.Name(),
                "customerMobile": self.MobileNo(),
                "customerEmail": self.EmailId(),
                "cwiCode": self.OtpCode(),
                "branchId": viewModel.Dealer().DealerId(),
                "versionId": viewModel.Bike().selectedVersionId(),
                "cityId": viewModel.Dealer().CityId()
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
                    if (obj.isSuccess && obj.dealer)
                        viewModel.changedSteps();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };

    self.regenerateOTP = function () {
        if (self.OtpAttempts() <= 2 && !self.IsVerified()) {
            var url = '/api/ResendVerificationCode/';
            var objCustomer = {
                "customerName": self.Name(),
                "customerMobile": self.MobileNo(),
                "customerEmail": self.EmailId(),
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
                    self.OtpAttempts(response.noOfAttempts);
                    alert("You will receive the new OTP via SMS shortly.");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };

    self.validateOTP = function (data, event) {
        var isSuccess = false;
        if (validateOTP() && validateUserDetail()) {
            self.verifyOtp();
            if (self.IsVerified()) {
                self.OtpCode("");
                $("#otpPopup").hide();
                $(".blackOut-window").hide();
                isSuccess = true;
            }
            else {
                $('#processing').hide();
                otpText.addClass("border-red");
                otpText.siblings("span, div").css("display", "block");
                otpText.siblings("div").text("Please enter a valid OTP.");
                isSuccess = false;
            }
        }
        return isSuccess;
    };

}

var BikeDetails = function () {
    var self = this;
    self.bikeVersions = ko.observableArray(versionList);
    self.selectedVersionId = ko.observable(bikeVersionId);
    self.selectedVersion = ko.observable();
    self.versionPriceBreakUp = ko.observableArray([]);
    self.versionColors = ko.observableArray([]);
    self.priceListBreakup = ko.observableArray([]);
    self.selectedColor = ko.observable();
    self.bookingAmount = ko.observable();
    self.versionSpecs = ko.observable();
    self.waitingPeriod = ko.observable();
    self.selectedColorId = ko.observable(0);
    self.isInsuranceFree = ko.observable(insFree);
    self.insuranceAmount = ko.observable(insAmt);
    self.bikeImageUrl = ko.computed(function () {
        if (self.selectedVersion() != undefined) {
            return (self.selectedVersion().HostUrl + "/310x174/" + self.selectedVersion().ImagePath);
        }
        return "";
    }, this);

    self.versionPrice = ko.computed(function () {
        var priceTxt = '';

        var total = 0, vlen = self.versionPriceBreakUp().length;
        for (i = 0; i < vlen ; i++) {
            total += self.versionPriceBreakUp()[i].Price;
        }
        if (self.isInsuranceFree()) {
            total - self.insuranceAmount();
        }
        return total;
    }, this);

    self.bikeName = ko.computed(function () {
        var _bikeName = '';
        if (self.selectedVersion() != undefined && self.selectedVersionId != undefined) {
            _bikeName = self.selectedVersion().Make.MakeName + ' ' + self.selectedVersion().Model.ModelName + ' ' + self.selectedVersion().MinSpec.VersionName;
        }
        return _bikeName;
    }, this);

    self.remainingAmount = ko.computed(function () {
        if (self.selectedVersion() != undefined && self.selectedVersion().OnRoadPrice > 0) {

            var _remainingAmount = self.selectedVersion().OnRoadPrice - self.selectedVersion().BookingAmount;
            if (self.isInsuranceFree())
                _remainingAmount = _remainingAmount - self.insuranceAmount();
            return _remainingAmount;
        }
        return "Not available";

    }, this);

    self.displayMinSpec = ko.computed(function () {
        var spec = '';
        if (self.versionSpecs() != undefined) {
            spec += (self.versionSpecs().AlloyWheels ? "Alloy" : "Spoke") + ' Wheels, ';
            if (self.versionSpecs().BrakeType) {
                spec += self.versionSpecs().BrakeType + ' Brake' + ', ';
            }
            spec += ' ' + (self.versionSpecs().ElectricStart ? ' Electric' : 'Kick') + ' Start, ';
            if (self.versionSpecs().AntilockBrakingSystem) {
                spec += ' ' + 'ABS' + ', ';
            }
        }

        if (spec) {
            return spec.substring(0, spec.length - 2);
        }
        return spec;
    }, this);

    self.getVersion = function (data, event) {
        self.selectedVersionId(data);
        $.each(self.bikeVersions(), function (key, value) {
            if (self.selectedVersionId() != undefined && self.selectedVersionId() > 0 && self.selectedVersionId() == value.MinSpec.VersionId) {
                self.selectedVersion(value);
                self.versionColors(value.BikeModelColors);
                self.selectedColor(value.BikeModelColors[0]);
                self.versionSpecs(value.MinSpec);
                self.versionPriceBreakUp(value.PriceList);
                self.waitingPeriod(value.NoOfWaitingDays);
                self.bookingAmount(value.BookingAmount);
                $("#selectedVersionId").val(self.selectedVersionId());
            }
        });
    };

    self.getColor = function (data, event) {
        self.selectedColorId(data.Id);
        self.selectedColor(data);
    };

    self.getVersion(self.selectedVersionId());

}

ko.bindingHandlers.googlemap = {
    init: function (element, valueAccessor) {
        var
          value = valueAccessor(),
          latLng = new google.maps.LatLng(value.latitude, value.longitude),
          mapOptions = {
              zoom: 10,
              center: latLng,
              mapTypeId: google.maps.MapTypeId.ROADMAP
          },
          map = new google.maps.Map(element, mapOptions),
          marker = new google.maps.Marker({
              position: latLng,
              map: map
          });
    }
};

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

function formatPrice(price) {
    price = price.toString();
    var lastThree = price.substring(price.length - 3);
    var otherNumbers = price.substring(0, price.length - 3);
    if (otherNumbers != '')
        lastThree = ',' + lastThree;
    var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    return price;
}


function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = $("#getOTP").val().trim();
    viewModel.Customer().IsVerified(false);
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

function setColor() {
    var vc = viewModel.Bike().versionColors();
    if (preSelectedColor > 0) {
        if (vc != null && vc.length > 0) {
            $.each(vc, function (key, value) {
                if (value.Id == preSelectedColor) {
                    viewModel.Bike().selectedColor(value);
                    viewModel.Bike().selectedColorId(value.Id);
                }
            });
        }
    }
    else {
        if (vc != null && vc.length > 0) {
            viewModel.Bike().selectedColor(vc[0]);
            viewModel.Bike().selectedColorId(vc[0].Id);
        }
    }
}

$("#bikeSummary").on('click', 'span.viewBreakupText', function () {
    $("div#breakupPopUpContainer").show();
    $(".blackOut-window").show();
});

$(".breakupCloseBtn,.blackOut-window").on('mouseup click', function (e) {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
});

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        $("div.breakupCloseBtn").click();
    }
});

function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

var viewModel = new BookingPageViewModel;
ko.applyBindings(viewModel, $("#bookingFlow")[0]);
setColor();
viewModel.UserOptions(bikeVersionId.toString() + preSelectedColor.toString());

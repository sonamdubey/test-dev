// JavaScript Document
var validateTabB = false,
	validateTabC = false,
	bikeSummary = $("#bikeSummary"),
	bikeSummaryTab = $("#bikeSummaryTab"),
	deliveryDetails = $("#deliveryDetails"),
	deliveryDetailsTab = $("#deliveryDetailsTab"),
	bikePayment = $("#bikePayment"),
	bikePaymentTab = $("#bikePaymentTab");
$(".select-dropdown").on("click", function () {
    if (!$(this).hasClass("open")) {
        selectStateDown($(this));
    }
    else
        selectStateUp($(this));
});
var msg = "";
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
    var mobileNo = $("#getMobile"),
    mobileVal = mobileNo.val();
    if (!validateMobileNo(mobileVal, this)) {
        setError(mobileNo, this.msg);
        return false;
    }
    else {
        hideError(mobileNo);
        return true;
    }
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


var versionul = $("#customizeBike ul.select-versionUL");
var colorsul = $("#customizeBike ul.select-colorUL");

var BookingPageViewModel = function () {
    var self = this;
    self.IsMapLoaded = false;
    self.Bike = ko.observable(new BikeDetails);
    self.Dealer = ko.observable(new BikeDealerDetails);
    self.Customer = ko.observable(new BikeCustomer);
    self.CurrentStep = ko.observable(1);
    self.SelectedVersionId = ko.observable();
    self.SelectedColorId = ko.observable(0);
    self.UserOptions = ko.observable();
    self.ActualSteps = ko.observable(1);
    self.CustomerInfo = ko.observable();
    self.LeadId = ko.observable(0);
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
        if (event.target.id = "bikeSummaryNextBtn")
        {
            leadSourceId = $(event.target).attr("leadSourceId");
        }
        var isSuccess = false, validate = validateUserDetail();
        var curCustInfo = '';
        if (viewModel.Customer().EmailId() != undefined && viewModel.Customer().MobileNo() != undefined) {
            curCustInfo = viewModel.Customer().EmailId().trim() + viewModel.Customer().MobileNo().trim();
        }
        if (self.CustomerInfo() != curCustInfo) {
            if (validate && self.Customer().IsVerified(false)) {
                var objCust = {
                    "dealerId": self.Dealer().DealerId(),
                    "pqId": self.Dealer().PQId(),
                    "customerName": self.Customer().Name(),
                    "customerMobile": self.Customer().MobileNo(),
                    "customerEmail": self.Customer().EmailId(),
                    "clientIP": clientIP,
                    "pageUrl": pageUrl,
                    "versionId": self.Bike().selectedVersionId(),
                    "cityId": self.Dealer().CityId(),
                    "areaId": self.Dealer().AreaId(),
                    "colorId": self.Bike().selectedColorId(),
                    "leadSourceId": leadSourceId,
                    "deviceId": getCookie('BWC')
                }

                $.ajax({
                    type: "POST",
                    url: "/api/v1/UpdatePQCustomerDetails/",
                    data: ko.toJSON(objCust),
                    async: false,
                    contentType: "application/json",
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('utma', getCookie('__utma'));
                        xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                    },
                    success: function (response) {
                        var obj = ko.toJS(response);
                        self.Customer().IsVerified(obj.isSuccess);
                        self.LeadId(obj.leadId);
                        // Getting MPQ from querystring
                        var match = new RegExp('[\\?&]MPQ=([^&#]*)').exec(window.location.href);
                        var queryParams = atob(match[1]);
                        if (queryParams.indexOf("&leadId") == -1) {
                          queryParams += "&leadId=" + self.LeadId();
                          window.history.replaceState( null, null, "?MPQ=" + Base64.encode(queryParams));
                        }
                        if (!self.Customer().IsVerified()) {
                          isSuccess = false;
                        }
                        else {
                            self.Customer().IsVerified();
                            self.changedSteps();
                            self.CustomerInfo(curCustInfo);
                            isSuccess = true;
                        }
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Lead_Submitted', 'lab': thisBikename + "_" + getCityArea });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                  
                        isSuccess = false;
                    }
                });
                self.setUserCookie();
            }
            else {
                              
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

    self.setUserCookie = function setPQUserCookie() {
        var val = self.Customer().Name() + '&' + self.Customer().EmailId() + '&' + self.Customer().MobileNo();
        SetCookie("_PQUser", val);
    }

    self.bookNow = function (data, event) {
        var isSuccess = false;
        if (self.Customer().IsVerified() && (self.CurrentStep() >= 2) && (self.Bike().bookingAmount() > 0)) {
            var curUserOptions = self.Bike().selectedVersionId().toString() + self.Bike().selectedColorId().toString();
            if (self.UserOptions() != curUserOptions) {
                self.UserOptions(curUserOptions);
                var objData = {
                    "pqguid": self.Dealer().PQId(),
                    "versionId": self.Bike().selectedVersionId(),
                    "colorId": self.Bike().selectedColorId(),
                    "leadId": self.LeadId()
                }
                $.ajax({
                    type: "POST",
                    url: "/api/v1/UpdatePQ/",
                    async: false,
                    data: ko.toJSON(objData),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        var obj = ko.toJS(response);
                        if (obj.isUpdated) {
                            isSuccess = true;
                            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + self.Dealer().PQId() + "&VersionId=" + self.Bike().selectedVersionId() + "&DealerId=" + self.Dealer().DealerId() + "&leadId=" + self.LeadId();
                            history.replaceState(null, null, "?MPQ="+ Base64.encode(cookieValue));
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
    self.discountList = ko.observableArray(discountDetail);

    self.totalDiscount = ko.computed(function () {
        var discount = 0;
        if (self.discountList() != undefined && self.discountList().length > 0) {
            var vlen = self.discountList().length;
            for (i = 0; i < vlen ; i++) {
                discount += self.discountList()[i].Price;
            }
        }
        return discount;
    }, this);

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
            _bikeName = self.selectedVersion().Make.makeName + ' ' + self.selectedVersion().Model.ModelName + ' ' + self.selectedVersion().MinSpec.VersionName;
        }
        return _bikeName;
    }, this);

    self.remainingAmount = ko.computed(function () {
        if (self.selectedVersion() != undefined && self.selectedVersion().OnRoadPrice > 0) {

            var _remainingAmount = 0;
            _remainingAmount = self.selectedVersion().OnRoadPrice - self.selectedVersion().BookingAmount - self.totalDiscount();
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
                self.bookingAmount(value.BookingAmount);
                $("#selectedVersionId").val(self.selectedVersionId());
                if (self.selectedColor().NoOfDays != -1)
                    self.waitingPeriod(self.selectedColor().NoOfDays);
                else self.waitingPeriod(self.selectedVersion().NoOfWaitingDays);
            }
        });
    };

    self.getColor = function (data, event) {
        self.selectedColorId(data.ColorId);
        self.selectedColor(data);
        if (data.NoOfDays != -1)
            self.waitingPeriod(data.NoOfDays);
        else self.waitingPeriod(self.selectedVersion().NoOfWaitingDays);
    };

    self.getVersion(self.selectedVersionId());

}



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






function setColor() {
    var vc = viewModel.Bike().versionColors();
    if (preSelectedColor > 0) {
        if (vc != null && vc.length > 0) {
            $.each(vc, function (key, value) {
                if (value.ColorId == preSelectedColor) {
                    viewModel.Bike().selectedColor(value);
                    viewModel.Bike().selectedColorId(value.ColorId);
                }
            });
        }
    }
    else {
        if (vc != null && vc.length > 0) {
            viewModel.Bike().selectedColor(vc[0]);
            viewModel.Bike().selectedColorId(vc[0].ColorId);
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
        $("div.termsPopUpCloseBtn").click();
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
// GA Tags
$('#bikeSummaryNextBtn').on('click', function (e) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_1_Submit', 'lab': thisBikename + "_" + getCityArea });
});

$('#deliveryDetailsNextBtn').on('click', function (e) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_2_Make_Payment_Click', 'lab': thisBikename + "_" + getCityArea });
});

$('.tnc').on('click', function (e) {
    LoadTerms($(this).attr("id"));
});

function LoadTerms(offerId) {
   
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    $('#terms').empty();
    if (offerId != 0 && offerId != null) {
        $(".termsPopUpContainer").css('height', '150')
        $('#termspinner').show();
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $('#termspinner').hide();
                if (response != null)
                    $('#terms').html(response);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });
    }
    else {
        $("#terms").load("/statichtml/tnc.html");
      
    }
    $(".termsPopUpContainer").css('height', '500');
}
$(".termsPopUpCloseBtn,.blackOut-window").on('mouseup click', function (e) {
    $("div#termsPopUpContainer").hide();
    $(".blackOut-window").hide();
    $('.cancellation-popup').hide();
});

$('#cancellation-box').click(function () {
    $(".blackOut-window").show();
    $('.cancellation-popup').show();
});
$('.close-btn').click(function () {
    $(".blackOut-window").hide();
    $('.cancellation-popup').hide();
});
$('.white-close-btn').click(function () {
    $("#blackOut-window").hide();
    $('.rsa-popup').hide();
});

$('.jcarousel').jcarousel({ wrap: 'circular' }).jcarouselAutoscroll({ interval: 7000, target: '+=1', autostart: true });
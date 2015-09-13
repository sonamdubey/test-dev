function BookingPageVMModel() {
    var self = this;
    self.Dealer = ko.observable();
    self.SelectedVarient = ko.observable();
    self.ModelColors = ko.observableArray([]);
    self.Varients = ko.observableArray([]);
    self.CustomerVM = ko.observable(new CustomerModel());
    self.SelectedModelColor = ko.observable();
    self.selectModelColor = function (model, event) {
        var curElement = $(event.currentTarget);
        self.SelectedModelColor(model);
        if (!curElement.find('span.ticked').hasClass("selected")) {
            $('.color-box').find('span.ticked').hide();
            $('.color-box').find('span.ticked').removeClass("selected");
            curElement.find('span.ticked').show();
            curElement.find('span.ticked').addClass("selected");
        }
        else {
            curElement.find('span.ticked').hide();
            curElement.find('span.ticked').removeClass("selected");
        }
    }
    self.selectVarient = function (varient, event) {
        self.SelectedVarient(varient);
        $(".varient-item").removeClass("border-dark selected");
        $(event.currentTarget).addClass("border-dark selected");
        $(".varient-heading-text").removeClass("text-orange");
    }
    self.getBookingPage = function () {
        var bookPage = null;
        $.getJSON('/api/BikeBookingPage/?versionId=' + verId + '&cityId=' + cityId + '&dealerId=' + dealerId)
        .done(function (data) {
            if (data) {
                bookPage = ko.toJS(data);

                self.Dealer(new DealerModel(bookPage.dealer.address1,
                    bookPage.dealer.address2,
                    bookPage.dealer.area,
                    bookPage.dealer.city,
                    bookPage.dealer.contactHours,
                    bookPage.dealer.emailId,
                    bookPage.dealer.faxNo,
                    bookPage.dealer.firstName,
                    bookPage.dealer.id,
                    bookPage.dealer.lastName,
                    bookPage.dealer.lattitude,
                    bookPage.dealer.longitude,
                    bookPage.dealer.mobileNo,
                    bookPage.dealer.organization,
                    bookPage.dealer.phoneNo,
                    bookPage.dealer.pincode,
                    bookPage.dealer.state));
                $.each(bookPage.modelColors, function (key, value) {
                    self.ModelColors.push(new ModelColorsModel(value.id, value.colorName, value.hexCode, value.modelId));
                });
                $.each(bookPage.varients, function (key, value) {
                    var priceList = [];
                    $.each(value.priceList, function (key, value) {
                        priceList.push(new PriceListModel(value.DealerId, value.ItemId, value.ItemName, value.Price));
                    })
                    if (verId == value.minSpec.versionId) {
                        self.SelectedVarient(
                            new VarientModel(
                            value.bookingAmount,
                            value.hostUrl,
                            new MakeMdl(value.make.makeId, value.make.makeName, value.make.maskingName),
                            new VersionMinSpecModel(
                                value.minSpec.alloyWheels,
                                value.minSpec.antilockBrakingSystem,
                                value.minSpec.brakeType,
                                value.minSpec.electricStart,
                                value.minSpec.modelName,
                                value.minSpec.versionName,
                                value.minSpec.price,
                                value.minSpec.versionId),
                            new ModelMdl(value.model.modelId, value.model.modelName, value.model.maskingName),
                            value.imagePath,
                            value.noOfWaitingDays,
                            value.onRoadPrice,
                            priceList
                        ));
                    }
                    self.Varients.push(
                        new VarientModel(
                            value.bookingAmount,
                            value.hostUrl,
                            new MakeMdl(value.make.makeId, value.make.makeName, value.make.maskingName),
                            new VersionMinSpecModel(
                                value.minSpec.alloyWheels,
                                value.minSpec.antilockBrakingSystem,
                                value.minSpec.brakeType,
                                value.minSpec.electricStart,
                                value.minSpec.modelName,
                                value.minSpec.versionName,
                                value.minSpec.price,
                                value.minSpec.versionId),
                            new ModelMdl(value.model.modelId, value.model.modelName, value.model.maskingName),
                            value.imagePath,
                            value.noOfWaitingDays,
                            value.onRoadPrice,
                            priceList
                        ));
                });
            }
        });
    };
    self.generatePQ = function () {
        var objPQ =
        {
            "cityId": cityId,
            "areaId": areaId,
            "modelId": self.SelectedVarient().model().modelId(),
            "clientIP": clientIP,
            "sourceType": 2,
            "versionId": self.SelectedVarient().minSpec().versionId()
        }

        $.ajax({
            type: "POST",
            url: "/api/PriceQuote/",
            data: ko.toJSON(objPQ),
            contentType: "application/json",
            success: function (response) {
                var obj = ko.toJS(response);
                if (obj) {
                    pqId = obj.quoteId;
                    var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + obj.quoteId + "&VersionId=" + self.SelectedVarient().minSpec().versionId() + "&DealerId=" + dealerId;
                    SetCookie("_MPQ", cookieValue);                    
                    var objCust = {
                        "dealerId": dealerId,
                        "pqId": pqId,
                        "customerName": self.CustomerVM().firstName() + ' ' + self.CustomerVM().lastName(),
                        "customerMobile": self.CustomerVM().mobileNo(),
                        "customerEmail": self.CustomerVM().emailId(),
                        "clientIP": clientIP,
                        "pageUrl": pageUrl,
                        "versionId": verId,
                        "cityId": cityId
                    }
                    $.ajax({
                        type: "POST",
                        url: "/api/PQCustomerDetail/",
                        data: ko.toJSON(objCust),
                        contentType: "application/json",
                        success: function (response) {
                            var obj = ko.toJS(response);
                            if (self.SelectedModelColor()) {
                                var objPQColor = {
                                    "pqId": pqId,
                                    "colorId": self.SelectedModelColor().id()
                                }
                                $.ajax({
                                    type: "POST",
                                    url: "/api/PQBikeColor/",
                                    data: ko.toJSON(objPQColor),
                                    contentType: "application/json",
                                    success: function (response) {
                                        var obj = ko.toJS(response);
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                        alert("Some error has occured while updating the Bike color.");
                                        return false;
                                    }
                                });
                            }                            
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert("Error while registering the price quote");
                        }
                    });
                    return;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("Some error has occured while registering new price quote.");
                return false;
            }
        });
    }
}

function CustomerModel() {
    var self = this;
    self.firstName = ko.observable();
    self.lastName = ko.observable();
    self.emailId = ko.observable();
    self.mobileNo = ko.observable();
    self.IsVerified = ko.observable();
    self.otpCode = ko.observable();
    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": dealerId,
                "pqId": pqId,
                "customerName": self.firstName() + ' ' + self.lastName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": verId,
                "cityId": cityId
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                data: ko.toJSON(objCust),
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    if (self.IsVerified())
                        $("#otp-submit-btn").trigger("click");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                    //var message;
                    //var statusErrorMap = {
                    //    '400': "Server understood the request, but request content was invalid.",
                    //    '401': "Unauthorized access.",
                    //    '404': "Bike not found.",
                    //    '403': "Forbidden resource can't be accessed.",
                    //    '500': "Internal server error.",
                    //    '503': "Service unavailable."
                    //};
                    //if (xhr.status) {
                    //    message = statusErrorMap[xhr.status];
                    //    if (!message) {
                    //        message = "Unknown Error \n.";
                    //    }
                    //} else if (ajaxOptions == 'parsererror') {
                    //    message = "Error.\nParsing JSON Request failed.";
                    //} else if (ajaxOptions == 'timeout') {
                    //    message = "Request Time out.";
                    //} else if (ajaxOptions == 'abort') {
                    //    message = "Request was aborted by the server";
                    //} else {
                    //    message = "Unknown Error \n.";
                    //}
                    //vm.bikeSearch.Bikes([]);
                    //alert(message);
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
                "customerName": self.firstName() + ' ' + self.lastName(),
                "versionId": verId,
                "cityId": cityId
            }
            $.ajax({
                type: "POST",
                url: "/api/PQMobileVerification/",
                data: ko.toJSON(objCust),
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    $("#user-details-submit-btn").trigger("click");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    }
    self.fullName = ko.computed(function () {
        return self.firstName() + ' ' + self.lastName();
    }, this);
}

function DealerModel(address1, address2, area, city, contactHours, emailId, faxNo, firstName, id, lastName, lattitude, longitude, mobileNo, organization, phoneNo, pincode, state, websiteUrl) {
    var self = this;
    self.address1 = ko.observable(address1);
    self.address2 = ko.observable(address2);
    self.area = ko.observable(area);
    self.city = ko.observable(city);
    self.contactHours = ko.observable(contactHours);
    self.emailId = ko.observable(emailId);
    self.faxNo = ko.observable(faxNo);
    self.firstName = ko.observable(firstName);
    self.id = ko.observable(id);
    self.lastName = ko.observable(lastName);
    self.lattitude = ko.observable(lattitude);
    self.longitude = ko.observable(longitude);
    self.mobileNo = ko.observable(mobileNo);
    self.organization = ko.observable(organization);
    self.phoneNo = ko.observable(phoneNo);
    self.pincode = ko.observable(pincode);
    self.state = ko.observable(state);
    self.websiteUrl = ko.observable(websiteUrl);    
}

function DisclaimerModel(disclaimers) {
    var self = this;
    self.disclaimers = ko.observableArray(disclaimers);
}

function ModelColorsModel(id, colorName, hexCode, modelId) {
    var self = this;
    self.id = ko.observable(id);
    self.colorName = ko.observable(colorName);
    self.hexCode = ko.observable(hexCode);
    self.modelId = ko.observable(modelId);
}

function OfferModel(id, text, type, value) {
    var self = this;
    self.id = ko.observable(id);
    self.text = ko.observable(text);
    self.type = ko.observable(type);
    self.value = ko.observable(value);
}

function VarientModel(bookingAmount, hostUrl, make, minSpec, model, imagePath, noOfWaitingDays, onRoadPrice, priceList) {
    var self = this;
    self.bookingAmount = ko.observable(bookingAmount);
    self.hostUrl = ko.observable(hostUrl);
    self.make = ko.observable(make);
    self.minSpec = ko.observable(minSpec);
    self.model = ko.observable(model);
    self.imagePath = ko.observable(imagePath);
    self.noOfWaitingDays = ko.observable(noOfWaitingDays);
    self.onRoadPrice = ko.observable(onRoadPrice);
    self.priceList = ko.observableArray(priceList);

    self.availText = ko.computed(function () {
        var _days = self.noOfWaitingDays();
        var _availText = _days ? '<p class="font12 text-light-grey">Waiting of ' + _days + ' days</p>' : '<p class="font12 text-green text-bold">Now available</p>';
        return _availText;
    }, this);

    self.bikeName = ko.computed(function () {
        var _bikeName = '';
        _bikeName = self.make().makeName() + ' ' + self.model().modelName() + ' ' + self.minSpec().versionName();
        return _bikeName;
    }, this);
    self.imageUrl = ko.computed(function () {
        var _imageUrl = '';
        _imageUrl = self.hostUrl() + '/310x174/' + self.imagePath();
        return _imageUrl;
    }, this);
    self.remainingAmount = ko.computed(function () {
        var _remainingAmount = self.onRoadPrice() - self.bookingAmount();
        return _remainingAmount;
    }, this);
}

function VersionMinSpecModel(alloyWheels, antilockBrakingSystem, brakeType, electricStart, modelName, versionName, price, versionId) {
    var self = this;
    self.alloyWheels = ko.observable(alloyWheels);
    self.antilockBrakingSystem = ko.observable(antilockBrakingSystem);
    self.brakeType = ko.observable(brakeType);
    self.electricStart = ko.observable(electricStart);
    self.modelName = ko.observable(modelName);
    self.price = ko.observable(price);
    self.versionId = ko.observable(versionId);
    self.versionName = ko.observable(versionName);
    self.displayMinSpec = ko.computed(function () {
        var spec = '';
        spec += (self.alloyWheels() ? "Alloy" : "Spoke") + ' Wheels, ';
        if (self.brakeType()) {
            spec += self.brakeType() + ' brake' + ', ';
        }
        spec += ' ' + (self.electricStart() ? ' Electric' : 'Kick') + ' start, ';
        if (self.antilockBrakingSystem()) {
            spec += ' ' + 'ABS' + ', ';
        }
        if (spec) {
            return spec.substring(0, spec.length - 2);
        }
        return '';
    }, this);
}

function MakeMdl(makeId, makeName, maskingName) {
    var self = this;
    self.makeId = ko.observable(makeId);
    self.makeName = ko.observable(makeName);
    self.maskingName = ko.observable(maskingName);
}

function ModelMdl(maskingName, modelId, modelName) {
    var self = this;
    self.maskingName = ko.observable(maskingName);
    self.modelId = ko.observable(modelId);
    self.modelName = ko.observable(modelName);
}

function PriceListModel(DealerId, ItemId, ItemName, Price) {
    var self = this;
    self.DealerId = ko.observable(DealerId);
    self.ItemId = ko.observable(ItemId);
    self.ItemName = ko.observable(ItemName);
    self.Price = ko.observable(Price);
}

$(document).ready(function () {
    viewModel = new BookingPageVMModel();
    viewModel.getBookingPage();
    ko.applyBindings(viewModel);
});


var firstname = $("#getFirstName");
var lastname = $("#getLastName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");

var detailsSubmitBtn = $("#user-details-submit-btn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");
var normalHeader = $('header .navbarBtn, header .global-location, header .bw-logo');
var mobileValue = '';

detailsSubmitBtn.click(function () {
    var a = validateEmail();
    var b = validateMobile();
    var c = validateName();
    if (c == false) {
        fnameVal();
    }
    else {
        if (a == false) {
            emailVal();
        }
        else {
            if (b == false) {
                mobileVal();
            }
        }
        if (a == true && b == true && c == true) {
            if (!viewModel.CustomerVM().IsVerified()) {
                viewModel.CustomerVM().verifyCustomer();
                otpContainer.removeClass("hide").addClass("show");
                if (viewModel.CustomerVM().IsVerified()) {

                    return;
                }
                $(this).hide();
                nameValTrue();
                mobileValTrue();
            }
            else {
                if (viewModel.CustomerVM().IsVerified()) {
                    otpBtn.trigger("click");
                }
            }
        }
    }
    mobileValue = mobile.val();
});

var validateName = function () {
    var a = firstname.val().length;
    if (a == 0)
        return false;
    else if (a >= 1)
        return true;
}

var nameValTrue = function () {
    firstname.removeClass("border-red");
    firstname.siblings("span, div").hide();
};

firstname.on("focus", function () {
    firstname.removeClass("border-red");
    firstname.siblings("span, div").hide();
});

emailid.on("focus", function () {
    emailid.removeClass("border-red");
    emailid.siblings("span, div").hide();
});

var mobileValTrue = function () {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};

var prevMob;

mobile.change(function () {
    console.log(mobileValue);
    var b = validateMobile();
    if (b == false) {
        mobileVal();
        otpContainer.removeClass("show").addClass("hide");
        detailsSubmitBtn.show();
    }
    else if (b == true || mobileValue == mobile.val()) {
        mobileValTrue();
        otpContainer.removeClass("hide").addClass("show");
        detailsSubmitBtn.hide();
    }
    if (mobileValue != mobile.val()) {
        otpContainer.removeClass("show").addClass("hide");
        detailsSubmitBtn.show();
    }
});

var fnameVal = function () {
    firstname.addClass("border-red");
    firstname.siblings("span, div").css("display", "block");
};

var emailVal = function (msg) {
    emailid.addClass("border-red");
    emailid.siblings("span, div").css("display", "block")
    emailid.siblings("div").text(msg);
};

var mobileVal = function (msg) {
    mobile.addClass("border-red");
    mobile.siblings("span, div").css("display", "block");
    mobile.siblings("div").text(msg);
};

/* Email validation */
function validateEmail() {
    var emailID = emailid.val();
    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;

    if (emailID == "") {
        emailVal('Please enter email address');
        return false;
    }
    else if (!reEmail.test(emailID)) {
        emailVal('Invalid Email');
        return false;
    }
    return true;
}

function validateMobile() {
    var reMobile = /^[0-9]*$/;
    var mobileNo = mobile.val();
    if (mobileNo == "") {
        return false;
        mobileVal("Please enter your Mobile Number");
    }
    else if (!reMobile.test(mobileNo)) {
        return false;
        mobileVal("Mobile Number should be numeric");
    }
    else if (mobileNo.length != 10) {
        return false;
        mobileVal("Mobile Number should be of 10 digits");
    }    return true;
}

var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").css("display", "block");
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]*$/;
    var cwiCode = otpText.text();

    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
    }
    else if (!isNumber.test(cwiCode)) {
        retVal = false;
        otpVal("Verification Code should be numeric");
    }
    else if (cwiCode.length != 5) {
        retVal = false;
        otpVal("Verification Code should be of 5 digits");
    }
    return retVal;
}

otpBtn.click(function () {
    if (!validateOTP()) {
        if (viewModel.CustomerVM().IsVerified()) {
            $.customizeState();
            $("#personalInfo").hide();
            $("#personal-info-tab").removeClass('text-bold');
            $("#customize").show();
            $('.colours-wrap .jcarousel').jcarousel('reload', {
                'animation': 'slow'
            });
            $('#customize-tab').addClass('text-bold');
            $('#customize-tab').addClass('active-tab').removeClass('disabled-tab');
            $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
            $(".booking-dealer-details").removeClass("hide").addClass("show");
            $(".call-for-queries").hide();
            $.scrollToSteps();
        }
        else {
            viewModel.CustomerVM().generateOTP();
        }
    }
});

$(".customize-submit-btn").click(function (e) {
    var a = varientSelection();
    if (a == true) {
        $.confirmationState();
        $("#customize").hide();
        $("#customize-tab").removeClass('text-bold');
        $("#confirmation").show();
        $('#confirmation-tab').addClass('active-tab text-bold').removeClass('disabled-tab');
        $.scrollToSteps();
    }
    else {
        $(".varient-heading-text").addClass("text-orange");
        $.scrollToSteps();
    }
});

$("#personal-info-tab, .customizeBackBtn").on('click', function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.personalInfoState();
        $.showCurrentTab('personalInfo');
        $('#personal-info-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#customize-tab').addClass('active-tab').removeClass('text-bold');
    }
});

$('#customize-tab, .confirmationBackBtn').on('click', function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.customizeState();
        $.showCurrentTab('customize');
        $('#customize-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#personal-info-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    }
});

$("#confirmation-tab").click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.confirmationState();
        $.showCurrentTab('confirmation');
        $('#confirmation-tab').addClass('active-tab text-bold');
        $('#customize-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#personal-info-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    }
});

$.showCurrentTab = function (tabType) {
    $('#personalInfo,#customize,#confirmation').hide();
    $('#' + tabType).show();
};

$.personalInfoState = function () {
    var container = $('.bike-to-buy-tabs ul');
    container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon personalInfo-icon-selected');
    container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon customize-icon-grey');
    container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon confirmation-icon-grey');
    container.find('li').removeClass('ticked');
    $('#book-back').removeClass('customizeBackBtn').addClass('hide');
    normalHeader.removeClass('hide');
    $('header').removeClass('fixed');

};

$.customizeState = function () {
    var container = $('.bike-to-buy-tabs ul');
    container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon booking-tick-blue');
    container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon customize-icon-selected');
    container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon confirmation-icon-grey');
    container.find('li').each(function () {
        if ($(this).find('div.bike-buy-part').attr('data-type-tab') == 'preference')
            $(this).find('div.bike-buy-part').removeClass('active-tab').addClass('disabled-tab');
        else
            $(this).find('div.car-buy-part').addClass('active-tab').removeClass('disabled-tab');
    });
    $('.booking-tabs ul li:first-child, .booking-tabs ul li:eq(1)').addClass('ticked');
    $('.booking-tabs ul li:last-child').removeClass('ticked');
    $('.booking-tabs ul li:eq(1)').removeClass('middle').addClass('middle');
    normalHeader.addClass('hide');
    $('header').addClass('fixed');
    $('#book-back').removeClass('hide');
    $('#book-back').removeClass('confirmationBackBtn').addClass('customizeBackBtn');
};

$.confirmationState = function () {
    var container = $('.bike-to-buy-tabs ul');
    container.find('li:eq(0)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon booking-tick-blue');
    container.find('li:eq(1)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon booking-tick-blue');
    container.find('li:eq(2)').find('span.buy-icon').attr('class', '').attr('class', 'booking-sprite buy-icon confirmation-icon-selected');
    container.find('li').each(function () {
        $(this).find('div.bike-buy-part').addClass('active-tab').removeClass('disabled-tab');
    });
    $('.booking-tabs ul li').addClass('ticked');
    $('.booking-tabs ul li:eq(1)').removeClass('middle');
    normalHeader.addClass('hide');
    $('header').addClass('fixed');
    $('#book-back').removeClass('hide');
    $('#book-back').removeClass('customizeBackBtn').addClass('confirmationBackBtn');
};

$.scrollToSteps = function () {
    var topScroll = $('#offerSection').offset().top - 50;
    $('body').animate({ scrollTop: topScroll }, 300);
};

$(window).scroll(function () {
    if ($('#book-back').is(':visible')) {
        if ($(window).scrollTop() > 50) {
            $('header').addClass('fixed');
        }
        else {
            $('header').removeClass('fixed');
        }
    }
});

var varientSelection = function () {
    var total = viewModel.SelectedVarient();
    if (total) {
        return true;
    }
    else{
        return false;
    }
}
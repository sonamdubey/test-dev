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
        if (!curElement.find('span.ticked').hasClass("selected")) {
            $('.color-box').find('span.ticked').hide();
            $('.color-box').find('span.ticked').removeClass("selected");
            curElement.find('span.ticked').show();
            curElement.find('span.ticked').addClass("selected");
            self.SelectedModelColor(model);
        }
        else {
            curElement.find('span.ticked').hide();
            curElement.find('span.ticked').removeClass("selected");
            self.SelectedModelColor(undefined);
        }
    }
    self.selectVarient = function (varient, event) {
        self.SelectedVarient(varient);
        $(".varient-item").removeClass("border-dark selected").addClass("border-solid");
        $(event.currentTarget).removeClass("border-solid").addClass("border-dark selected");
        $(".varient-heading-text").removeClass("text-orange");
    }
    self.getBookingPage = function () {
        var bookPage = null;
        $.getJSON('/api/BikeBookingPage/?versionId=' + verId + '&cityId=' + cityId + '&dealerId=' + dealerId)
        .done(function (data) {
            if (data) {
                bookPage = ko.toJS(data);
                //$.each(bookPage.modelColors, function (key, value) {
                //    self.ModelColors.push(new ModelColorsModel(value.id, value.colorName, value.hexCode, value.modelId));
                //});
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
                            priceList,
                            value.bikeModelColors
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
                            priceList,
                            value.bikeModelColors
                        ));
                });
            }
        });
    };
    self.generatePQ = function () {
        // Push GA Analytics
        var cityArea = GetGlobalCityArea();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_2', 'lab': thisBikename + '_' + cityArea });
        var objPQ =
        {
            "pqId": pqId,
            "versionId": self.SelectedVarient().minSpec().versionId()
        }
        //var isSameVersion = getCookie("_MPQ").indexOf("&VersionId=" + self.SelectedVarient().minSpec().versionId() + "&") > 0 ? true : false;
        var reVersionId = new RegExp("&VersionId=" + self.SelectedVarient().minSpec().versionId() + "&", "g");
        var isSameVersion = reVersionId.test(getCookie("_MPQ"));
        if (!isSameVersion) {
            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + self.SelectedVarient().minSpec().versionId() + "&DealerId=" + dealerId;
            //SetCookie("_MPQ", cookieValue);
            
            $.ajax({
                type: "POST",
                url: "/api/UpdatePQ/",
                data: ko.toJSON(objPQ),
                async: false,
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Some error has occured while registering new price quote.");
                    return false;
                }
            });
            window.location.href = "/m/pricequote/bookingsummary_new.aspx?MPQ=" + Base64.encode(cookieValue);
        }
        if (self.SelectedModelColor() && self.SelectedModelColor() != undefined) {
            self.updateColor(pqId, self.SelectedModelColor().id);
        }
        else {
            self.updateColor(pqId, 0);
        }
    }

    self.updateColor = function (pqId, colorId) {
        var objPQColor = {
            "pqId": pqId,
            "colorId": colorId
        }
        $.ajax({
            type: "POST",
            url: "/api/PQBikeColor/",
            async: false,
            data: ko.toJSON(objPQColor),
            contentType: "application/json",
            dataType: 'json',
            success: function (response) {
                var obj = ko.toJS(response);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("Some error has occured while updating the Bike color.");
                return false;
            }
        });
    }
}

var prevEmail = "";
var prevMobile = "";

function CustomerModel() {
    var arr = setuserDetails();
    var self = this;
    if (arr != null && arr.length > 0) {
        self.firstName = ko.observable(arr[0]);
        self.lastName = ko.observable(arr[1]);
        if (arr[2]!="undefined") {
            self.emailId = ko.observable(arr[2]);
        } else {
            self.emailId = ko.observable();
        }
        self.mobileNo = ko.observable(arr[3]);
    }
    else {
        self.firstName = ko.observable();
        self.lastName = ko.observable();
        self.emailId = ko.observable();
        self.mobileNo = ko.observable();
    }
    self.IsVerified = ko.observable();
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
                "versionId": verId,
                "cityId": cityId,
                "leadSourceId": 5,
                "deviceId": getCookie('BWC')
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                async: false,
                data: ko.toJSON(objCust),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                },
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    if (self.IsVerified()) {
                        if (obj.isSuccess && obj.dealer) {
                            viewModel.Dealer(new DealerModel(
                                obj.dealer.address1,
                                obj.dealer.address2,
                                obj.dealer.area,
                                obj.dealer.city,
                                obj.dealer.contactHours,
                                obj.dealer.emailid,
                                obj.dealer.faxNo,
                                obj.dealer.firstName,
                                obj.dealer.id,
                                obj.dealer.lastName,
                                obj.dealer.lattitude,
                                obj.dealer.longitude,
                                obj.dealer.mobileNo,
                                obj.dealer.organization,
                                obj.dealer.phoneNo,
                                obj.dealer.pincode,
                                obj.dealer.state,
                                obj.dealer.websiteUrl));
                        }
                    }
                    else {
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
                "versionId": verId,
                "cityId": cityId
            }
            $.ajax({
                type: "POST",
                url: "/api/v1/PQMobileVerification/",
                async: false,
                data: ko.toJSON(objCust),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    if (obj.isSuccess && obj.dealer) {
                        viewModel.Dealer(new DealerModel(
                            obj.dealer.address1,
                            obj.dealer.address2,
                            obj.dealer.area,
                            obj.dealer.city,
                            obj.dealer.contactHours,
                            obj.dealer.emailid,
                            obj.dealer.faxNo,
                            obj.dealer.firstName,
                            obj.dealer.id,
                            obj.dealer.lastName,
                            obj.dealer.lattitude,
                            obj.dealer.longitude,
                            obj.dealer.mobileNo,
                            obj.dealer.organization,
                            obj.dealer.phoneNo,
                            obj.dealer.pincode,
                            obj.dealer.state,
                            obj.dealer.websiteUrl));
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    }
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
    }
    self.fullName = ko.computed(function () {
        var _firstName = self.firstName() != undefined ? self.firstName() : "";
        var _lastName = self.lastName() != undefined ? self.lastName() : "";
        return _firstName + ' ' + _lastName;
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

function VarientModel(bookingAmount, hostUrl, make, minSpec, model, imagePath, noOfWaitingDays, onRoadPrice, priceList, bikeModelColors) {
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
    self.bikeModelColors = ko.observableArray(bikeModelColors);
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
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount()) : 0;
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
var normalHeader = $('header .navbarBtn, header .global-location');
var mobileValue = '';

detailsSubmitBtn.click(function () {
    var a = validateEmail();
    var b = validateMobile();
    var c = validateName();
    var d = validateLastName();
    var cityArea = GetGlobalCityArea();
    if (c == false) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_Submit_Error_Name', 'lab': cityArea });
    }
    else {
        if (a == false) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_Submit_Error_Email', 'lab': cityArea });
        }
        else {
            if (b == false) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': cityArea });
            }
        }
        if (a == true && b == true && c == true && d == true) {
            viewModel.CustomerVM().verifyCustomer();
            if (viewModel.CustomerVM().IsValid()) {
                $.customizeState();
                $("#personalInfo").hide();
                $("#personal-info-tab").removeClass('text-bold');
                $("#customize").show();
                //$('.colours-wrap .jcarousel').jcarousel('reload', {
                  //  'animation': 'slow'
                //});
                $('#customize-tab').addClass('text-bold');
                $('#customize-tab').addClass('active-tab').removeClass('disabled-tab');
                $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                $(".booking-dealer-details").removeClass("hide").addClass("show");
                $(".call-for-queries").hide();
                $.scrollToSteps();
            }
            else {
                otpContainer.removeClass("hide").addClass("show");
                $(this).hide();
                nameValTrue();
                mobileValTrue();
                otpText.val('').removeClass("border-red");
                otpText.siblings("span, div").css("display", "none");
            }
            setPQUserCookie();
            var getCityArea = GetGlobalCityArea();
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_1_Successful_Submit', 'lab': getCityArea });
        }
    }
    mobileValue = mobile.val();
});

var validateName = function () {
    var isValid;
    var a = firstname.val().length;
    //if (firstname.val().indexOf('&') != -1) {
    if ((/&/).test(firstname.val())) {
        isValid = false;
        setError(firstname, 'Invalid name');
    }
    else if (a == 0) {
        isValid = false;
        setError(firstname, 'Please enter your first name');
    }
    else if (a >= 1) {
        isValid = true;
        hideError(firstname)
    }
    return isValid;
}
var validateLastName = function () {
    var isError = true;
    //if (lastname.val().indexOf('&') != -1) {
    if ((/&/).test(lastname.val())) {
        setError(lastname, 'Invalid name');
        isError = false;
    }
    else {
        hideError(lastname)
}
    return isError;
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
    prevEmail = emailid.val().trim();
});

mobile.on("focus", function () {
    hideError(mobile);
    prevMobile = mobile.val().trim();
});

emailid.on("blur", function () {
    if (prevEmail != emailid.val().trim())
    {
        var getCityArea = GetGlobalCityArea();
        if (validateEmail(getCityArea))
        {
            viewModel.CustomerVM().IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(emailid);
        }
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    else
        viewModel.CustomerVM().IsVerified(true);
});

mobile.on("blur", function () {
    if (prevMobile != mobile.val().trim())
    {
        var getCityArea = GetGlobalCityArea();
        if (validateMobile(getCityArea))
        {
            viewModel.CustomerVM().IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(mobile);
        }
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
    else
        viewModel.CustomerVM().IsVerified(true);

});

var mobileValTrue = function () {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};

var prevMob;

mobile.change(function () {
    viewModel.CustomerVM().IsVerified(false);    
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


//var fnameVal = function () {
//    firstname.addClass("border-red");
//    firstname.siblings("span, div").css("display", "block");
//};

var setError = function (ele, msg) {
    ele.addClass("border-red");
    ele.siblings("span, div").show();
    ele.siblings("div").text(msg);
}

function hideError(ele) {
    ele.removeClass("border-red");
    ele.siblings("span, div").hide();
}


var mobileVal = function (msg) {
    mobile.addClass("border-red");
    mobile.siblings("span, div").css("display", "block");
    mobile.siblings("div").text(msg);
};

/* Email validation */
function validateEmail() {
    var emailID = emailid.val();
    var reEmail = /^[A-z0-9._%+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailID == "") {
        setError(emailid, 'Please enter email address');
        return false;
    }
    else if (!reEmail.test(emailID)) {
        setError(emailid, 'Invalid Email');
        return false;
    }
    hideError(emailid);
    return true;
}

function validateMobile() {
    var reMobile = /^[0-9]*$/;
    var mobileNo = mobile.val();
    if (mobileNo == "") {
        setError(mobile, "Please enter your Mobile Number");
        return false;
    }
    else if (!reMobile.test(mobileNo)) {
        setError(mobile, "Mobile Number should be numeric");
        return false;
    }
    else if (mobileNo.length != 10) {
        setError(mobile, "Mobile Number should be of 10 digits");
        return false;
    }
    hideError(mobile);
    return true;
}

var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").css("display", "block");
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]*$/;
    var cwiCode = otpText.val();
    viewModel.CustomerVM().IsVerified(false);
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

mobile.change(function () {
    viewModel.CustomerVM().IsVerified(false);
});

otpText.on("focus", function () {
    otpText.siblings("span, div").css("display", "none");
});

    
emailid.change(function () {
    viewModel.CustomerVM().IsVerified(false);
});

otpBtn.click(function () {
    var isValid = true;
    isValid = validateEmail();
    var getCityArea = GetGlobalCityArea();
    if (isValid == false) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_OTP_Submit_Error_Name', 'lab': getCityArea  });
    }
    isValid &= validateMobile();
    if (isValid == false) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_OTP_Submit_Error_Mobile', 'lab': getCityArea });
    }
    isValid &= validateName();
    if (isValid == false) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_OTP_Submit_Error_Name', 'lab': getCityArea });
    }
    isValid &= validateLastName();
    
    $('#processing').show();
    if (!validateOTP())
        $('#processing').hide();

    if (validateOTP() && isValid) {
        viewModel.CustomerVM().generateOTP();
        if (viewModel.CustomerVM().IsVerified()) {
            $.customizeState();
            $("#personalInfo").hide();
            $("#personal-info-tab").removeClass('text-bold');
            $("#customize").show();
            //$('.colours-wrap .jcarousel').jcarousel('reload', {
            //    'animation': 'slow'
            //});
            $('#customize-tab').addClass('text-bold');
            $('#customize-tab').addClass('active-tab').removeClass('disabled-tab');
            $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
            $(".booking-dealer-details").removeClass("hide").addClass("show");
            $(".call-for-queries").hide();
            $.scrollToSteps();
            $('#processing').hide();

            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");

            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
        }
        else {
            $('#processing').hide();
            otpVal("Please enter a valid OTP.");
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
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

$("#personal-info-tab").click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.personalInfoState();
        $.showCurrentTab('personalInfo');
        $('#personal-info-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
});

$(".header-fixed").on("click", ".customizeBackBtn", function () {
    $.personalInfoState();
    $.showCurrentTab('personalInfo');
    $('#personal-info-tab').addClass('active-tab text-bold');
    $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    $('#customize-tab').addClass('active-tab').removeClass('text-bold');
    $("#book-back").addClass("tab2");
});

$('#customize-tab').click(function () {
    if (!$(this).hasClass('disabled-tab')) {
        $.customizeState();
        $.showCurrentTab('customize');
        $('#customize-tab').addClass('active-tab text-bold');
        $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
        $('#personal-info-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    }
});

$(".header-fixed").on("click", ".confirmationBackBtn", function () {
    $.customizeState();
    $.showCurrentTab('customize');
    $('#customize-tab').addClass('active-tab text-bold');
    $('#confirmation-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
    $('#personal-info-tab').addClass('active-tab').removeClass('disabled-tab text-bold');
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
    $('#book-back').removeClass('customizeBackBtn').addClass('hide').hide();
    normalHeader.removeClass('hide');
    //$('header').removeClass('fixed');

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
    //$('header').addClass('fixed');
    $('#book-back').removeClass('hide').show();
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
    //$('header').addClass('fixed');
    $('#book-back').removeClass('hide').show();
    $('#book-back').removeClass('customizeBackBtn').addClass('confirmationBackBtn');
};

$.scrollToSteps = function () {
    var topScroll = $('#offerSection').offset().top - 50;
    $('body').animate({ scrollTop: topScroll }, 300);
};
/*
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
*/
var varientSelection = function () {
    var total = viewModel.SelectedVarient();
    if (total) {
        return true;
    }
    else {
        return false;
    }
}

$('#btnMakePayment').on('click', function (e) {
    var cityArea = GetGlobalCityArea();
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Booking Page', 'act': 'Step 3_Pay_Click', 'lab': cityArea });
});


function setPQUserCookie() {

    var val = firstname.val() + '&' + lastname.val() + '&' + emailid.val()+ '&' + mobile.val() ;
    SetCookie("_PQUser", val);
}

function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

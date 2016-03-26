
var customerViewModel, userLocation = { "latitude": "", "longitude": "" }, assistanceGetName = $('#assistanceGetName'), assistanceGetEmail = $('#assistanceGetEmail'), assistanceGetMobile = $('#assistanceGetMobile'), getModelName = $("#getModelName"), assistGetModel = $("#assistGetModel");
var dealerArr = [];
var markerArr = [];
var map, infowindow;
var blackMarkerImage = 'http://imgd2.aeplcdn.com/0x0/bw/static/design15/map-marker-black.png';
var redMarkerImage = 'http://imgd3.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';

var flag = false;

var getCityArea = GetGlobalCityArea();

var leadBtnBookNow = $("a.get-assistance-btn"), leadCapturePopup = $("#leadCapturePopup"), fullName = $("#getFullName"), emailid = $("#getEmailID"), mobile = $("#getMobile"), otpContainer = $(".mobile-verification-container");


var variantsDropdown = $(".variants-dropdown"),
variantSelectionTab = $(".variant-selection-tab"),
variantUL = $(".variants-dropdown-list"),
variantListLI = $(".variants-dropdown-list li");



var detailsSubmitBtn = $("#user-details-submit-btn, #submitAssistanceFormBtn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");

var prevEmail = "";
var prevMobile = "";

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

ko.bindingHandlers.chosen = {
    init: function (element, valueAccessor, allBindings, customerViewModel, bindingContext) {
        var $element = $(element);
        var options = ko.unwrap(valueAccessor()) + { width: "100%" };
        if (typeof options === 'object')
            $element.chosen(options);

        ['options', 'selectedOptions', 'value'].forEach(function (propName) {
            if (allBindings.has(propName)) {
                var prop = allBindings.get(propName);
                if (ko.isObservable(prop)) {
                    prop.subscribe(function () {
                        $element.trigger('chosen:updated');
                    });
                }
            }
        });
        return;
    }
}

$(document).ready(function () {
    var windowWidth = window.innerWidth,
        windowHeight = window.innerHeight;
    $('#dealerMapWrapper, #dealersMap').css({ 'width': windowWidth - 355, 'height': windowHeight - 50 });
    $('.dealer-map-wrapper').css({ 'height': $('#dealerListingSidebar').height() + 1 });
});

$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).innerHeight() > $(document).height() - $('#bg-footer').innerHeight()) {
        $('#dealerMapWrapper').css({ 'position': 'relative', 'top': $('#bg-footer').offset().top - $('#dealerMapWrapper').height() - 52 });
    }
    else {
        $('#dealerMapWrapper').css({ 'position': 'fixed', 'top': '50px' });
    }
});

$(document).on('click', '#dealerDetailsSliderCard div.dealer-slider-close-btn', function () {
    $('#sidebarHeader').addClass('border-solid-bottom');
    $('body').removeClass('hide-scroll');
    $('#dealerDetailsSliderCard').animate({ 'right': '-338px' }, { complete: function () { $('#dealerDetailsSliderCard').hide().css({ 'height': '0' }); } });
    $('#dealersList li').removeClass('active');
    showPrevDealer(dealerId);
});

var showPrevDealer = function (dealerId) {
    for (var i = 0; i < markerArr.length; i++) {
        if (markerArr[i].dealerId == dealerId) {
            infowindow.setContent(markerArr[i].dealerName);
            infowindow.open(map, markerArr[i]);
            break;
        }
    }
}

$(document).keydown(function (e) {
    if (e.keyCode == 27) {
        $('.dealer-slider-close-btn').trigger('click');
    }
});

function getLocation() {
    if (navigator.geolocation) {
        var timeoutVal = 10 * 1000 * 1000;
        navigator.geolocation.getCurrentPosition(
            savePosition,
            showError,
            { enableHighAccuracy: true, maximumAge: 600000 }
        );
    }
}

function savePosition(position) {
    userLocation = {
        "latitude": position.coords.latitude,
        "longitude": position.coords.longitude
    }
}

function showError(error) {
    switch (error.code) {
        case error.PERMISSION_DENIED:
            console.log("User denied the request for Geolocation.");
            break;
        case error.POSITION_UNAVAILABLE:
            console.log("Location information is unavailable.");
            break;
        case error.TIMEOUT:
            console.log("The request to get user location timed out.");
            break;
        case error.UNKNOWN_ERROR:
            console.log("An unknown error occurred.");
            break;
    }
}



$("ul#dealersList li").each(function () {
    _self = $(this);
    _dealer = new Object();
    _dealer.id = _self.attr("data-item-id");
    _dealer.isFeatured = (_self.attr("data-item-type") == "3" || _self.attr("data-item-type") == "2");
    _dealer.latitude = _self.attr("data-lat");
    _dealer.longitude = _self.attr("data-log");
    _dealer.address = _self.attr("data-address");
    _dealer.name = _self.find("a.dealer-sidebar-link").text();
    _dealer.maskingNumber = _self.attr("data-item-number");
    dealerArr.push(_dealer);
});


function initializeMap(dealerArr) {
    getLocation();
    var zoomLevel, centerPoint;
    var i, marker, dealer, markerPosition, content, zIndex;
    if (dealerArr.length && (dealerArr[0].latitude != "0" || dealerArr[0].longitude != "0")) {
        centerPoint = new google.maps.LatLng(dealerArr[0].latitude, dealerArr[0].longitude);
        zoomLevel = 13;
    }
    else {
        centerPoint = new google.maps.LatLng(21.7679, 78.8718);
        zoomLevel = 5;
    }

    var mapProp = {
        center: centerPoint,
        zoom: zoomLevel,
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        zoomControl: true,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_TOP
        },
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("dealersMap"), mapProp);
    infowindow = new google.maps.InfoWindow();

    for (i = 0; i < dealerArr.length; i++) {
        dealer = dealerArr[i];
        if (dealer && (dealer.latitude != "0" || dealer.longitude != "0")) {
            markerPosition = new google.maps.LatLng(dealer.latitude, dealer.longitude);
            zIndex = 100;

            if (dealer.isFeatured) {
                markerIcon = redMarkerImage;
            } else {
                markerIcon = blackMarkerImage;
            }

            marker = new google.maps.Marker({
                dealerId: dealer.id,
                dealerName: dealer.name,
                position: markerPosition,
                icon: markerIcon,
                zIndex: zIndex
            });

            markerArr.push(marker);
            marker.setMap(map);

            content = '<div class="dealer-info-tooltip"><h3 class="font16 margin-bottom5"><a href="javascript:void(0)" data-tooltip-id="' + dealer.id + '" class="text-black tooltip-target-link">' + dealer.name + '</a></h3><div class="font14 text-light-grey"><div class="margin-bottom5">' + dealer.address + '</div><div><span class="bwsprite phone-grey-icon"></span><span>' + dealer.maskingNumber + '</span></div></div></div>';

            google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {
                return function () {
                    infowindow.setContent(content);
                    infowindow.open(map, marker);
                };
            })(marker, content, infowindow));



            google.maps.event.addListener(marker, 'click', (function (marker, infowindow) {
                return function () {
                    infowindow.close();
                    getDealerFromSidebar(marker.dealerId);
                };
            })(marker, infowindow));
        }

    }

    $(window).resize(function () {
        google.maps.event.trigger(map, "resize");
    });


}

$('body').on('click', ' #dealersMap a.tooltip-target-link', function () {
    getDealerFromSidebar($(this).attr('data-tooltip-id'));
});

$('body').on('mouseover', '#dealersList li', function () {
    if (!$("body").hasClass("hide-scroll")) {
        var currentLI = $(this),
         currentDealerId = currentLI.attr('data-item-id');
        for (var i = 0; i < markerArr.length; i++) {
            if (markerArr[i].dealerId == currentDealerId) {
                infowindow.setContent(markerArr[i].dealerName);
                infowindow.open(map, markerArr[i]);
                break;
            }
        }
    }

});

$('#dealersList li').mouseout(function () {
    infowindow.close();
});

$("body").on('click', 'a.dealer-sidebar-link', function () {
    var parentLI = $(this).parents('li');
    selectedDealer(parentLI);
    isInquired = (parentLI.attr("data-item-inquired") == "true") ? true : false;
    if (isInquired) {
        $("#buying-assistance-form").hide().siblings("#dealer-assist-msg").show();
    }
    else {
        $("#buying-assistance-form").show().siblings("#dealer-assist-msg").hide();
    }
});



$(function () {
    $("body").on('click', '#dealersList a.get-assistance-btn', function () {

        id = $(this).attr("data-item-id");
        type = $(this).attr("data-item-type");
        isInquired = ($(this).parents("li").attr("data-item-inquired") == "true") ? true : false;

        if (type != "0" || type != "1") {
            leadCapturePopup.show();
            $("#dealer-lead-msg").hide();
            $("div#contactDetailsPopup").show();
            $("#otpPopup").hide();
            $('body').addClass('lock-browser-scroll');
            $(".blackOut-window").show();
            getDealerDetails(id)
        }
        else {
            $('body').removeClass('lock-browser-scroll');
        }

        if (isInquired) {
            $("#contactDetailsPopup").hide().siblings("#dealer-lead-msg").show();
        }
        else {
            $("#contactDetailsPopup").show().siblings("#dealer-lead-msg").hide();           
        }

    });

    $("body").on("click mouseup", ".leadCapture-close-btn, .blackOut-window", function () {
        leadCapturePopup.hide();
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
    });

    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) {
            leadCapturePopup.hide();
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window").hide();
        }
    });
});

var getDealerFromSidebar = function (tooltipId) {
    var dealer;
    $('#dealersList li').each(function () {
        dealer = $(this);
        if (dealer.attr('data-item-id') == tooltipId) {
            flag = true;
            if (!dealer.hasClass('active')) {
                selectedDealer(dealer);
            }
            if (flag)
                return false;
        }
    });
}

var dealerId;

var selectedDealer = function (dealer) {

    infowindow.close();
    $('#sidebarHeader').removeClass('border-solid-bottom');
    $('html, body').animate({ scrollTop: dealer.offset().top - 103 });
    if (dealer.hasClass('active')) {
        $('.dealer-slider-close-btn').trigger('click');
    }
    else {
        dealer.addClass('active');
        dealer.siblings().removeClass('active');
        dealerId = dealer.attr("data-item-id");
        type = dealer.attr("data-item-type");
        if (type == "2" || type == "3") {
            $('#dealerDetailsSliderCard').show().animate({ 'right': '338px' });
            $('#dealerDetailsSliderCard').css({ 'height': $(window).innerHeight() - 52 });
            $("#assistGetName").focus();
            $('body').addClass('hide-scroll')
            getDealerDetails(dealerId)
        }
        else {
            $('#dealerDetailsSliderCard').hide().animate({ 'right': '-338px' }, { complete: function () { $('#dealerDetailsSliderCard').hide().css({ 'height': '0' }); } });
            $('body').removeClass('hide-scroll');
        }
    }

};

$("body").on('click', '#dealer-assist-msg .cur-pointer', function () {
    $("#dealer-assist-msg").parent().slideUp();
});


var dealerDetails = function (data) {
    var self = this;
    self.userLocation = userLocation.latitude + "," + userLocation.longitude;
    self.name = ko.observable(data.name);
    self.mobile = ko.observable(data.maskingNumber);
    self.address = ko.observable(data.address);
    self.city = ko.observable(data.cityName);
    self.workingHours = ko.observable(data.workingHours);
    self.email = ko.observable(data.email);
    self.dealerType = ko.observable(data.dealerPackageType);
    self.showRoomOpeningHours = ko.observable(data.showRoomOpeningHours);
    self.showRoomClosingHours = ko.observable(data.showRoomClosingHours);

    if (data.Area) {
        self.area = ko.observable(data.Area.areaName);
        self.lat = ko.observable(data.Area.latitude);
        self.lng = ko.observable(data.Area.longitude);
    }
    else {
        self.area = ko.observable();
        self.lat = ko.observable();
        self.lng = ko.observable();
    }

}

var dealerBikes = function (data) {

    var self = this;
    self.bikeName = ko.observable(data.bike);
    self.bikePrice = ko.observable(data.versionPrice);
    self.bikeUrl = ko.observable("");
    self.minSpecs = ko.observable(data.specs);
    self.imagePath = ko.observable(data.hostUrl + "/310x174/" + data.imagePath);

    if (data.make)
        self.makeName = ko.observable(data.make.makeName);

    if (data.model) {
        self.modelName = ko.observable(data.model.modelName);
        self.modelId = ko.observable(data.model.modelId);
    }

    if (data.version)
        self.versionId = ko.observable(data.version.versionId);

    self.displayMinSpec = ko.computed(function () {
        var spec = '';
        if (self.minSpecs().displacement && self.minSpecs().displacement != "0")
            spec += "<span><span>" + self.minSpecs().displacement + "</span><span class='text-light-grey'> CC</span></span>";

        if (self.minSpecs().fuelEffeciency && self.minSpecs().fuelEffeciency != "0")
            spec += "<span>, <span>" + self.minSpecs().fuelEffeciency + "</span><span class='text-light-grey'> Kmpl</span></span>";

        if (self.minSpecs().maxPower && self.minSpecs().maxPower != "0")
            spec += "<span>, <span>" + self.minSpecs().maxPower + "</span><span class='text-light-grey'> bhp</span></span>";

        if (self.minSpecs().maxTorque && self.minSpecs().displacement != "0")
            spec += "<span><span>" + self.minSpecs().displacement + "</span><span class='text-light-grey'> CC</span></span>";


        if (spec != "")
            return spec;
        else
            return "Specs Unavailable";
    }, this);

    if (data.make && data.model) {
        self.bikeUrl("/" + data.make.maskingName + "-bikes/" + data.model.maskingName + "/");
    }

}

var DealerModel = function (data) {
    var self = this;
    self.DealerDetails = ko.observable(new dealerDetails(data.dealerDetails));
    self.DealerBikes = ko.utils.arrayMap(data.dealerBikes, function (item) {
        return new dealerBikes(item);
    });
    self.CustomerDetails = ko.observable(customerViewModel);
}


function getDealerDetails(id) {
    var obj = new Object();

    if (!isNaN(id) && id != "0") {
        var dealerKey = "dealerDetails_" + id;
        var dealerInfo = lscache.get(dealerKey);
        if (!dealerInfo) {
            $.ajax({
                type: "GET",
                url: "/api/DealerBikes/?dealerId=" + id,
                contentType: "application/json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('__utmz'));
                },
                success: function (response) {
                    lscache.set(dealerKey, response, 30);
                    bindDealerDetails(response);
                },
                complete: function (xhr) {
                    if (xhr.status == 204 || xhr.status == 404) {
                        lscache.set(dealerKey, null, 30);
                    }
                }
            });
        }
        else {
            bindDealerDetails(dealerInfo);
        }
    }

    return obj;
}

function bindDealerDetails(response) {
    obj = ko.toJS(response);
    customerViewModel = new CustomerModel(obj);
    ko.cleanNode($('#dealerInfo')[0]);
    ko.applyBindings(new DealerModel(obj), $('#dealerInfo')[0]);
    //$ddlModels.chosen('destroy');
    //$ddlModels.trigger("chosen : updated");
}


initializeMap(dealerArr);



var bikesList = function (data) {
    var self = this;
    self.makeName = ko.observable();
    self.modelName = ko.observable();
    self.modelId = ko.observable();
    self.versionId = ko.observable();

    if (data.make)
        self.makeName(data.make.makeName);
    if (data.model) {
        self.modelName(data.model.modelName);
        self.modelId(data.model.modelId);
    }
    if (data.version)
        self.versionId(data.version.versionId);
}

function CustomerModel(obj) {
    data = obj.dealerBikes;
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
    self.selectedBike = ko.observable();
    self.dealerId = ko.observable(obj.dealerDetails.id);
    self.versionId = ko.observable(0);
    self.dealerName = ko.observable(obj.dealerDetails.name);
    self.IsVerified = ko.observable(false);
    self.NoOfAttempts = ko.observable(0);
    self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
    self.otpCode = ko.observable();
    self.isAssist = ko.observable(false);
    self.pqId = ko.observable();
    self.modelId = ko.observable(0);
    self.bikes = ko.observableArray([]);
    self.chosenUpdate = function () { $ddlModels.trigger("chosen:updated"); };

    if (obj.dealerBikes && obj.dealerBikes.length > 0) {
        //(obj.dealerBikes).push({"bike" : "Select a bike model"})
        self.bikes = ko.observableArray(obj.dealerBikes);
    }


    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": self.dealerId(),
                "pqId": self.pqId(),
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": self.versionId(),
                "cityId": bikeCityId,
                "leadSourceId": leadSrcId,
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
                "cityId": bikeCityId
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

    self.generatePQ = function (data, event) {
        self.IsVerified(false);
        isSuccess = false;
        isValidDetails = false;
        if (event.target.id == 'submitAssistanceFormBtn') {
            self.isAssist(true);
            isValidDetails &= validateBike(assistGetModel);
            isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);

        }
        else {
            isValidDetails &= validateBike(getModelName);
            self.isAssist(false);
            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);

        }
        var bike = self.selectedBike();
        if (bike && bike.version && bike.model) {
            self.versionId(bike.version.versionId);
            self.modelId(bike.model.modelId);
        }
        else {
            self.versionId(0);
            self.modelId(0);
        }

        if (isValidDetails && self.modelId() && self.versionId()) {
            var url = '/api/RegisterPQ/';
            var objData = {
                "dealerId": self.dealerId(),
                "modelId": self.modelId(),
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": self.versionId(),
                "cityId": bikeCityId,
                "areaId": 0,
                "sourceType": pageSrcId,
                "pQLeadId": leadSrcId,
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
                if (self.isAssist()) {
                    $("#buying-assistance-form").hide();
                    $("#dealer-assist-msg").fadeIn();

                } else {
                    $("#contactDetailsPopup").hide();
                    $("#personalInfo").hide()
                    $("#otpPopup").hide();
                    $("#dealer-lead-msg").fadeIn();
                }
                $("ul#dealersList li[data-item-id=" + self.dealerId() + "]").attr("data-item-inquired", true);
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
        }
    };

    $("body").on('click', '#otp-submit-btn', function () {
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

            }
            else {
                $('#processing').hide();
                otpVal("Please enter a valid OTP.");                 
            }
        }
    });
}

function ValidateUserDetail(fullName, emailid, mobile) {
    return validateUserInfo(fullName, emailid, mobile);
};


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


$("body").on('click', '#dealer-lead-msg .okay-thanks-msg', function () {
    $(".leadCapture-close-btn").click();
});


var validateUserInfo = function (leadUsername, leadEmailId, leadMobileNo) {
    var isValid = true;
    isValid = validateUserName(leadUsername);
    isValid &= validateEmailId(leadEmailId);
    isValid &= validateMobileNo(leadMobileNo);
    return isValid;
};

var validateBike = function (bike) {
    var selectedVersion = bike.val();
    if (selectedVersion && selectedVersion.length && selectedVersion != "0") {
        hideError(bike);
        isValid = true;
        return true;
    }
    else {
        setError(bike, 'Select a bike');
        isValid = false;
    }
    return false;
}

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


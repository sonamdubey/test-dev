var customerViewModel, dealerDetailsViewModel, iterator = 0, originPlace, map, infowindow, dealerId, userLocation = { "latitude": "", "longitude": "" }, assistanceGetName = $('#assistanceGetName'), assistanceGetEmail = $('#assistanceGetEmail'), assistanceGetMobile = $('#assistanceGetMobile'), getModelName = $("#getModelName"), assistGetModel = $("#assistGetModel");
var dealerArr = [], markerArr = [];
var blackMarkerImage = 'https://imgd.aeplcdn.com/0x0/bw/static/design15/map-marker-black.png';
var redMarkerImage = 'https://imgd.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';
var flag = false;
var userAddress = "";
var getCityArea = GetGlobalCityArea();
var leadBtnBookNow = $("a.get-assistance-btn"), leadCapturePopup = $("#leadCapturePopup"), fullName = $("#getFullName"), emailid = $("#getEmailID"), mobile = $("#getMobile"), otpContainer = $(".mobile-verification-container");
var variantsDropdown = $(".variants-dropdown"),
variantSelectionTab = $(".variant-selection-tab"),
variantUL = $(".variants-dropdown-list"),
variantListLI = $(".variants-dropdown-list li");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");
var prevEmail = "";
var prevMobile = "";
var currentUrl = ""

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

$(document).ready(function () {
    var windowWidth = window.innerWidth,
        windowHeight = window.innerHeight;
    if ($('#sidebarHeader').height() > 30) {
        $('#dealersList').css({ 'padding-top': '72px' });
        $('#dealerMapWrapper, #dealersMap').css({ 'width': windowWidth - 355, 'height': windowHeight - 30 });
    }
    else
        $('#dealerMapWrapper, #dealersMap').css({ 'width': windowWidth - 355, 'height': windowHeight - 50 });
    $('#dealerListingSidebar').css({ 'min-height': windowHeight - 50 });
    $('.dealer-map-wrapper').css({ 'height': $('#dealerListingSidebar').height() + 1 });

    currentUrl = window.location.href;
    if (currentUrl.indexOf('#') > -1)
    {
        urlDealerId = currentUrl.split('#')[1];
        showDealerDetailsHash(urlDealerId);
    }

});

$(window).scroll(function () {
    if (!$('#dealerDetailsSliderCard').is(':visible'))
        $('body').removeClass('lock-browser-scroll');

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
    if (userAddress != "") {
        $("#locationSearch").val("").val(userAddress);
        google.maps.event.trigger(originPlace, 'place_changed');
    }
    else {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                savePosition,
                showError,
                { enableHighAccuracy: true, maximumAge: 600000 }
            );
        }
    }
}

$(document).on("click", "#getUserLocation", function () { getLocation(); })

function savePosition(position) {
    userLocation = {
        "latitude": position.coords.latitude,
        "longitude": position.coords.longitude
    }
    if (dealerDetailsViewModel && dealerDetailsViewModel.CustomerDetails())
        dealerDetailsViewModel.CustomerDetails().userSrcLocation(userLocation.latitude + "," + userLocation.longitude);
    if (userAddress == "") {
        $.getJSON("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + userLocation.latitude + "," + userLocation.longitude + "&key=" + googleMapAPIKey, function (data) {
            if (data.status == "OK" && data.results.length > 0) {
                userAddress = data.results[0].formatted_address;
            }
            else {
                userAddress = "Your location";
            }
            $("#locationSearch").val("").val(userAddress);
            google.maps.event.trigger(originPlace, 'place_changed');
        });
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
    var i, marker, dealer, markerPosition, content, zIndex;
    var mapProp = {
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
    var directionsService = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer;
    directionsDisplay.setMap(map);

    originPlace = new google.maps.places.Autocomplete(
      (document.getElementById('locationSearch')),
      {
          types: ['geocode'],
          componentRestrictions: { country: "in" }
      });

    google.maps.event.addListener(originPlace, 'place_changed', function () {
        var place = originPlace.getPlace();
        if (!(place && place.geometry)) {
            origin_place_id = new google.maps.LatLng(userLocation.latitude, userLocation.longitude);
        }
        else {

            origin_place_id = place.geometry.location
            userLocation.latitude = place.geometry.location.lat();
            userLocation.longitude = place.geometry.location.lng();
            userAddress = place.formatted_address;
        };

        travel_mode = google.maps.TravelMode.DRIVING;

        route(origin_place_id, travel_mode, directionsService, directionsDisplay);
        $('.location-details').show();
    });

    infowindow = new google.maps.InfoWindow();

    for (i = 0; i < dealerArr.length; i++) {
        setTimeout(function () {
            setMarkersNInfo();
        }, i * 200);

    }

    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': currentCityName }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            map.fitBounds(results[0].geometry.viewport);
        }
    });


}

function setMarkersNInfo() {
    dealer = dealerArr[iterator++];
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
            animation: google.maps.Animation.DROP,
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
                //toggleBounce(marker);
                getDealerFromSidebar(marker.dealerId);
            };
        })(marker, infowindow));
    }
}

function setMapCenter(newLat, newLng) {
    if (!isNaN(newLat) && !isNaN(newLng)) {
        map.setCenter({
            lat: parseFloat(newLat),
            lng: parseFloat(newLng)
        });
    }

}

function route(origin_place_id, travel_mode, directionsService, directionsDisplay) {

    activeItem = $("ul#dealersList li.active");
    _lat = activeItem.attr("data-lat");
    _lng = activeItem.attr("data-log");
    destination_place_id = new google.maps.LatLng(_lat, _lng);

    if (!origin_place_id || !destination_place_id) {
        return;
    }

    directionsService.route({
        origin: origin_place_id,
        destination: destination_place_id,
        travelMode: travel_mode
    }, function (response, status) {
        if (status === google.maps.DirectionsStatus.OK) {
            getCommuteInfo(response);
            directionsDisplay.setDirections(response);
        }
    });

}

function getCommuteInfo(result) {
    var totalDistance = 0;
    var totalDuration = 0;
    var legs = result.routes[0].legs;
    for (var i = 0; i < legs.length; ++i) {
        totalDistance += legs[i].distance.value;
        totalDuration += legs[i].duration.value;
    }
    $('#commuteDistance').text((totalDistance / 1000).toFixed(2) + " kms");
    $('#commuteDuration').text(totalDuration.toString().toHHMMSS());

}

function toggleBounce(_marker) {
    if (_marker.getAnimation() != null) {
        _marker.setAnimation(null);
    } else {
        _marker.setAnimation(google.maps.Animation.BOUNCE);
    }
    setTimeout(function () { _marker.setAnimation(null); }, 1500);
}

$(document).on('click', ' #dealersMap a.tooltip-target-link', function () {
    getDealerFromSidebar($(this).attr('data-tooltip-id'));
});

$(document).on('mouseover', '#dealersList li', function () {
    if (!$("body").hasClass("hide-scroll")) {
        var currentLI = $(this),
         currentDealerId = currentLI.attr('data-item-id');
        for (var i = 0; i < markerArr.length; i++) {
            if (markerArr[i].dealerId == currentDealerId) {
                infowindow.setContent(markerArr[i].dealerName);
                infowindow.open(map, markerArr[i]);
                //toggleBounce(markerArr[i]);
                break;
            }
        }
    }

});

$(document).on('mouseout', '#dealersList li', function () {
    infowindow.close();
});

$(document).on('click', '.dealer-card', function () {
    var parentLI = $(this).parents('li');
    selectedDealer(parentLI);
    $("#buyingAssistanceForm").show();
    $("#buying-assistance-form").show().siblings("#dealer-assist-msg").hide();
    stopLoading('#buyingAssistanceForm');

});

$(document).on('click', '#dealersList a.get-assistance-btn', function (e) {
    leadSourceId = $(this).attr("leadSourceId");
    pqSourceId = $(this).attr("pqSourceId");
    id = $(this).attr("data-item-id");
    type = $(this).attr("data-item-type");
    parentLi = $(this).parents("li");
    isInquired = (parentLi.attr("data-item-inquired") == "true") ? true : false;

    if (type != "0" || type != "1") {
        leadCapturePopup.show();
        $("#dealer-lead-msg").hide();
        $("div#contactDetailsPopup").show();
        $("#otpPopup").hide();
        if (!$('#dealerDetailsSliderCard').is(':visible'))
            $('body').addClass('lock-browser-scroll');
        else
            $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").show();
        campId = parentLi.attr("data-campaignid");
        dname = parentLi.find("a.dealer-sidebar-link").text();
        if (!parentLi.hasClass("active"))
            getDealerDetails(id, campId, dname);
        else stopLoading(getModelName.parent());
    }
    else {
        $('body').removeClass('lock-browser-scroll');
    }
    $("#contactDetailsPopup").show().siblings("#dealer-lead-msg").hide();

    setMapCenter(parentLi.attr("data-lat"), parentLi.attr("data-log"));

    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_Locator", "act": "Get_Offers_Clicked", "lab": makeName + "_" + bikeCityName });

});

$(document).on("click mouseup", ".leadCapture-close-btn, .blackOut-window", function () {
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

var getDealerFromSidebar = function (tooltipId) {
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
            campId = $("ul#dealersList li.active").attr("data-campaignid");
            dname = dealer.find("a.dealer-sidebar-link").text();
            getDealerDetails(dealerId, campId, dname);
            getLocation();

        }
        else {
            $('#dealerDetailsSliderCard').hide().animate({ 'right': '-338px' }, { complete: function () { $('#dealerDetailsSliderCard').hide().css({ 'height': '0' }); } });
            $('body').removeClass('hide-scroll');
        }
    }

};

$(document).on('click', '#dealer-assist-msg .cur-pointer', function () {
    $("#dealer-assist-msg").parent().slideUp();
});

var dealerDetails = function (data) {
    var self = this;
    self.name = ko.observable(data.name);
    self.mobile = ko.observable(data.maskingNumber);
    self.address = ko.observable(data.address);
    self.city = ko.observable(data.cityName);
    self.workingHours = ko.observable(data.workingHours);
    self.email = ko.observable(data.email);
    self.dealerType = ko.observable(data.dealerPackageType);



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

var dealerBikes = function (data)
{

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

function getDealerDetails(id, campId, name) {
    showDealerDetailsLoader();
    var obj = new Object();
    if (!isNaN(id) && id != "0" && campId != "0") {
        var dealerKey = "dealerDetails_" + id + "_camp_" + campId;
        var dealerInfo = lscache.get(dealerKey);
        if (!dealerInfo) {
            $.ajax({
                type: "GET",
                url: "/api/DealerBikes/?dealerId=" + id + "&campaignId=" + campId,
                contentType: "application/json",
                dataType: 'json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                },
                success: function (response) {
                    lscache.set(dealerKey, response, 30);
                    bindDealerDetails(response);
                },
                complete: function (xhr) {
                    if (xhr.status == 204 || xhr.status == 404) {
                        lscache.set(dealerKey, null, 30);
                    }
                    hideDealerDetailsLoader(ele);
                }
            });
        }
        else {
            bindDealerDetails(dealerInfo);
            hideDealerDetailsLoader(ele);
        }
    }
    return obj;
}

function showDealerDetailsLoader() {
    ele = $("#buyingAssistanceForm");
    popupEle = $("#getModelName");
    startLoading(popupEle.parent());
    popupEle.prev().show();
    $("#dealerPersonalInfo").css("visibility", "hidden");
    $("#buying-assistance-form").css("visibility", "hidden");
    $("#dealerModelwiseBikes").css("visibility", "hidden");
    $("#commute-distance-form").css("visibility", "hidden");
    $("#BWloader").empty().html("<span class='fa fa-spinner fa-spin' ></span> Loading " + dname + " dealer details...").show();
    startLoading(ele);
}

function hideDealerDetailsLoader(ele, dname) {
    ele = $("#buyingAssistanceForm");
    popupEle = $("#getModelName");
    stopLoading(popupEle.parent());
    popupEle.prev().hide();
    $("#BWloader").hide();
    $("#dealerPersonalInfo").css("visibility", "visible");
    $("#buying-assistance-form").css("visibility", "visible");
    $("#commute-distance-form").css("visibility", "visible");
    $("#dealerModelwiseBikes").css("visibility", "visible");
    stopLoading(ele);

}

function startLoading(ele) {
    try {
        var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
        _self.animate({ width: '100%' }, 500);
    }
    catch (e) { return };
}

function stopLoading(ele) {
    try {
        var _self = $(ele).find(".progress-bar");
        _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
    }
    catch (e) { return };
}

function bindDealerDetails(response) {
    obj = ko.toJS(response);
    ko.cleanNode($('#dealerInfo')[0]);
    customerViewModel = new CustomerModel(obj);
    dealerDetailsViewModel = new DealerModel(obj)
    ko.applyBindings(dealerDetailsViewModel, $('#dealerInfo')[0]);
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
    hideFormErrors();
    var self = this;
    self.fullName = ko.observable();
    self.emailId = ko.observable();
    self.mobileNo = ko.observable();
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
    self.selectedBikeName = ko.observable();

    if (arr != null && arr.length > 0) {
        self.fullName = ko.observable(arr[0]);
        if (arr[1]!="undefined") {
            self.emailId = ko.observable(arr[1]);
        } else {
            self.emailId = ko.observable();
        }

        self.mobileNo = ko.observable(arr[2]);
    }

    self.userSrcLocation = ko.observable("");
    if (userLocation.latitude != "" && userLocation.longitude != "")
        self.userSrcLocation = ko.observable(userLocation.latitude + "," + userLocation.longitude);

    if (obj.dealerBikes && obj.dealerBikes.length > 0) {
        self.bikes = ko.observableArray(obj.dealerBikes);
    }

    self.generatePQ = function (data, event) {
        self.IsVerified(false);
        isSuccess = false;
        isValidDetails = false;
        if (event.target.id == 'submitAssistanceFormBtn') {
            leadSourceId = $(event.target).attr("leadSourceId");
            pqSourceId = $(event.target).attr("pqSourceId");
            isValidDetails &= validateBike(assistGetModel);
            isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
            startLoading($("#buyingAssistanceForm"));
            self.isAssist(true);
        }
        else {
            isValidDetails &= validateBike(getModelName);
            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
            startLoading($("#user-details-submit-btn").parent());
            self.isAssist(false);
        }

        var bike = self.selectedBike();
        if (bike && bike.version && bike.model) {
            self.versionId(bike.version.versionId);
            self.modelId(bike.model.modelId);
            self.selectedBikeName(bike.make.makeName + " " + bike.model.modelName + "_" + bike.version.versionName);
        }
        else {
            self.versionId(0);
            self.modelId(0);
            self.selectedBikeName("");
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
                "pQLeadId": pqSourceId,
                "deviceId": getCookie('BWC')
            }
            $.ajax({
                type: "POST",
                url: url,
                async: false,
                data: ko.toJSON(objData),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    self.pqId(response.quoteId);
                    isSuccess = true;
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        self.IsVerified(false);
                        isSuccess = false;
                        if (self.isAssist()) {
                            stopLoading($("#buyingAssistanceForm"));
                        }
                        else {
                            stopLoading($("#user-details-submit-btn").parent());
                        }
                    }

                }
            });
        }

        return isSuccess;

    }

    self.verifyCustomer = function () {
        if (self.isAssist()) {
            stopLoading($("#buyingAssistanceForm"));
            startLoading($("#buyingAssistanceForm"));
        }
        else {
            stopLoading($("#user-details-submit-btn").parent());
            startLoading($("#user-details-submit-btn").parent());
        }

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
                "leadSourceId": leadSourceId,
                "deviceId": getCookie('BWC')
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                data: ko.toJSON(objCust),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
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
                complete: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status != 200)
                        self.IsVerified(false);
                    if (self.isAssist()) {
                        stopLoading($("#buyingAssistanceForm"));
                    }
                    else {
                        stopLoading($("#user-details-submit-btn").parent());
                    }
                }
            });
        }


    };

    self.generateOTP = function () {
        if (self.isAssist()) {
            stopLoading($("#buyingAssistanceForm"));
            startLoading($("#buyingAssistanceForm"));
        }
        else {
            stopLoading($("#user-details-submit-btn").parent());
            startLoading($("#user-details-submit-btn").parent());
        }
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
                url: "/api/v1/PQMobileVerification/",
                data: ko.toJSON(objCust),
                async: false,
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                },
                complete: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.status != 200)
                        self.IsVerified(false);
                    if (self.isAssist()) {
                        stopLoading($("#buyingAssistanceForm"));
                    }
                    else {
                        stopLoading($("#user-details-submit-btn").parent());
                    }
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

        var isValidDetails = self.generatePQ(data, event);
        var btnId = event.target.id;
        if (isValidDetails) {
            if (self.isAssist()) {
                startLoading($("#buyingAssistanceForm"));
            }
            else {
                startLoading($("#user-details-submit-btn").parent());
            }
            self.verifyCustomer();
            if (self.IsValid()) {
                $("#contactDetailsPopup").hide();
                $("#personalInfo").hide()
                $("#otpPopup").hide();
                if (self.isAssist()) {
                    $("#leadCapturePopup .leadCapture-close-btn").click();
                    $("#buying-assistance-form").hide();
                    $("#dealer-assist-msg").fadeIn();
                    startLoading($("#buyingAssistanceForm"));

                } else {
                    $("#dealer-lead-msg").fadeIn();
                }

                if (btnId == "submitAssistanceFormBtn") {
                    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_Locator", "act": "Lead_Submitted", "lab": "Open_Form_" + self.selectedBikeName() + "_" + bikeCityName });
                }

                else if (btnId == "user-details-submit-btn") {
                    dataLayer.push({ "event": "Bikewale_all", "cat": "Dealer_Locator", "act": "Lead_Submitted", "lab": "Main_Form_" + self.selectedBikeName() + "_" + bikeCityName });
                }
            }
            else {
                $("#leadCapturePopup").show();
                if (!$('#dealerDetailsSliderCard').is(':visible'))
                    $('body').addClass('lock-browser-scroll');
                else
                    $('body').removeClass('lock-browser-scroll');
                $(".blackOut-window").show();
                $("#contactDetailsPopup").hide();
                $("#otpPopup").show();
                var leadMobileVal = mobile.val();
                $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                otpContainer.removeClass("hide").addClass("show");
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();
        }

        if (self.isAssist()) {
            stopLoading($("#buyingAssistanceForm"));
        }
        else {
            stopLoading($("#user-details-submit-btn").parent());
        }        
    };

    $("body").on('click', '#otp-submit-btn', function () {
        startLoading($("#otp-submit-btn").parent());
        $('#processing').show();
        isValidDetails = false;
        if (!validateOTP())
            $('#processing').hide();

        if (self.isAssist() == true) {
            isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
        }
        else {
            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
        }

        if (validateOTP() && isValidDetails) {
            customerViewModel.generateOTP();
            if (customerViewModel.IsVerified()) {
                $("#personalInfo").hide();
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

        stopLoading($("#otp-submit-btn").parent());
    });
}

function ValidateUserDetail(fullName, emailid, mobile) {
    return validateUserInfo(fullName, emailid, mobile);
};

function hideFormErrors() {
    hideError(getModelName);
    hideError(fullName);
    hideError(emailid);
    hideError(mobile);
    hideError(assistanceGetEmail);
    hideError(assistanceGetMobile);
    hideError(assistanceGetName);
    hideError(assistGetModel);
}

$(document).on("focus", "#assistanceGetName,#getFullName", function () {
    hideError($(this));
});

$(document).on("focus", "#assistanceGetEmail,#getEmailID", function () {
    hideError($(this));
    prevEmail = $(this).val().trim();
});

$(document).on("focus", "#assistanceGetMobile,#getMobile,#getUpdatedMobile", function () {
    hideError($(this));
    prevMobile = $(this).val().trim();
});

$(document).on("blur", "#assistanceGetEmail,#getEmailID", function () {
    if (prevEmail != $(this).val().trim()) {
        if (validateEmailId($(this))) {
            customerViewModel.IsVerified(false);
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError($(this));
        }
    }
});

$(document).on("blur", "#assistanceGetMobile,#getMobile,#getUpdatedMobile", function () {
    if (prevMobile != $(this).val().trim()) {
        if (validateMobileNo($(this))) {
            customerViewModel.IsVerified(false);
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError($(this));
        }
    }
});

$(document).on("click", ".edit-mobile-btn", function () {
    var prevMobile = $(this).prev("span.lead-mobile").text();
    $(".lead-otp-box-container").hide();
    $(".update-mobile-box").show();
    $("#getUpdatedMobile").val(prevMobile).focus();
});

$(document).on("click", "#generateNewOTP", function () {
    if (validateMobileNo($("#getUpdatedMobile"))) {
        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        $(".update-mobile-box").hide();
        $(".lead-otp-box-container").show();
        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
    }
});

$(document).on('click', '#dealer-lead-msg .okay-thanks-msg', function () {
    $(".leadCapture-close-btn").click();
});

$(document).on("focus", "#getOTP", function () {
    otpText.val('');
    otpText.siblings("span, div").hide();
});

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = otpText.val();
    customerViewModel.IsVerified(false);
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

var validateUserInfo = function (leadUsername, leadEmailId, leadMobileNo) {
    var isValid = true;
    isValid = validateUserName(leadUsername);
    isValid &= validateEmailId(leadEmailId);
    isValid &= validateMobileNo(leadMobileNo);
    return isValid;
};

var validateBike = function (bike) {
    if (customerViewModel.selectedBike() && customerViewModel.selectedBike().model && customerViewModel.selectedBike().model.modelId != "0") {
        hideError(bike);
        return true;
    }
    else {
        setError(bike, 'Select a bike');
        return false;
    }

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
        
        isValid = true;
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

String.prototype.toHHMMSS = function () {
    var sec_num = parseInt(this, 10);
    var hrs = Math.floor(sec_num / 3600);
    var mins = Math.floor((sec_num - (hrs * 3600)) / 60);
    var secs = sec_num - (hrs * 3600) - (mins * 60);

    if (hrs < 10) { hrs = "0" + hrs; }
    if (mins < 10) { mins = "0" + mins; }
    if (secs < 10) { secs = "0" + secs; }
    var time = hrs + ' hrs ' + mins + ' mins ';// + secs +" s";
    return time;
}


function showDealerDetailsHash(dealerId)
{
    var parentLI = $("#dealersList li[data-item-id=" + dealerId + "]");
    selectedDealer(parentLI);
    $("#buyingAssistanceForm").show();
    $("#buying-assistance-form").show().siblings("#dealer-assist-msg").hide();
    stopLoading('#buyingAssistanceForm');
}

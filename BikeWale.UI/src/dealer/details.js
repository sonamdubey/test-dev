﻿var redMarkerImage = 'http://imgd3.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';
var originPlace, userLocation = { "latitude": "", "longitude": "" }, userAddress = "";
var customerViewModel, dealerDetailsViewModel;
$(document).ready(function () {
    dropdown.setDropdown();
   // bindDealerDetails();
});

function initializeMap() {
    var mapCanvas = document.getElementById("dealer-map");
    var mapProp = {
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        center: new google.maps.LatLng(19.0720625, 72.8550991),
        zoom: 15,
        disableDefaultUI: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
    };

    var map = new google.maps.Map(mapCanvas, mapProp);

    var marker = new google.maps.Marker({
        position: mapProp.center,
        icon: redMarkerImage
    });

    marker.setMap(map);

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

    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': currentCityName }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            map.fitBounds(results[0].geometry.viewport);
        }
    });
}

initializeMap();


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




var dealerDetails = function (dealerDetailsBind) {
    var self = this;
    var dealerObj=ko.toJSON(dealerDetailsBind)
    self.name = ko.observable(dealerObj.Name);
    self.mobile = ko.observable(dealerObj.MaskingNumber);
    self.address = ko.observable(dealerObj.Address);
    self.city = ko.observable(dealerObj.City);
    self.workingHours = ko.observable(dealerObj.WorkingHours);
    self.email = ko.observable(dealerObj.Email);
    self.dealerType = ko.observable(dealerObj.DealerPackageType);
    self.dealerId = ko.observable(dealerObj.DealerId);


    if (dealerObj.Area) {
        self.area = ko.observable(dealerObj.Area.AreaName);
        self.lat = ko.observable(dealerObj.Area.Latitude);
        self.lng = ko.observable(dealerObj.Area.Longitude);
    }
    else {
        self.area = ko.observable();
        self.lat = ko.observable();
        self.lng = ko.observable();
    }

}


$('.dropdown-select-wrapper').on('click', '.dropdown-label', function () {
    dropdown.active($(this));
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list.dropdown-with-select li', function () {
    var element = $(this);
    if (!element.hasClass('active')) {
        dropdown.selectItem($(this));
        dropdown.selectOption($(this));
    }
    else {
        element.closest('.dropdown-menu').removeClass('dropdown-active');
    }
});

var dropdown = {
    setDropdown: function () {
        var selectDropdown = $('.dropdown-select');

        selectDropdown.each(function () {
            dropdown.setMenu($(this));
        });
    },

    setMenu: function (element) {
        $('<div class="dropdown-menu"></div>').insertAfter(element);
        dropdown.setStructure(element);
    },

    setStructure: function (element) {
        var elementValue = element.find('option:selected').text(),
			menu = element.next('.dropdown-menu');
        menu.append('<p class="dropdown-label">' + elementValue + '</p><div class="dropdown-list-wrapper"><p class="dropdown-selected-item">' + elementValue + '</p><ul class="dropdown-menu-list dropdown-with-select"></ul></div>');
        dropdown.setOption(element);
    },

    setOption: function (element) {
        var selectedIndex = element.find('option:selected').index(),
			menu = element.next('.dropdown-menu'),
			menuList = menu.find('ul');

        element.find('option').each(function (index) {
            if ($(this).val() != 0 && $(this).val().length != 0) { // check for dropdown label value
                menuList.append('<li data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
            }
        });
    },

    active: function (label) {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
        label.closest('.dropdown-menu').addClass('dropdown-active');
        if (label.closest('.dropdown-select-wrapper').hasClass('invalid')) {
            validate.dropdown.hideError(label);
        }
    },

    inactive: function () {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
    },

    selectItem: function (element) {
        var elementValue = element.attr('data-option-value');
        if (elementValue != 0 || elementValue.length != 0) { // check for dropdown label value
            var elementText = element.text(),
			    menu = element.closest('.dropdown-menu'),
			    dropdownLabel = menu.find('.dropdown-label'),
			    selectedItem = menu.find('.dropdown-selected-item');

            element.siblings('li').removeClass('active');
            element.addClass('active');
            selectedItem.text(elementText);
            dropdownLabel.text(elementText);
            menu.removeClass('dropdown-active');
        }
    },

    selectOption: function (element) {
        var elementValue = element.attr('data-option-value'),
			wrapper = element.closest('.dropdown-select-wrapper'),
			selectDropdown = wrapper.find('.dropdown-select');

        selectDropdown.val(elementValue).trigger('change');
    }
};

$(document).on('click', function (event) {
    event.stopPropagation();
    var dropdown = $('.dropdown-menu');

    if (dropdown.hasClass('dropdown-active')) {
        if (!dropdown.is(event.target) && dropdown.has(event.target).length == 0) {
            dropdown.removeClass('dropdown-active');
        }
    }
});

var assistanceGetName = $('#assistGetName'),
    assistanceGetEmail = $('#assistGetEmail'),
    assistanceGetMobile = $('#assistGetMobile'),
    assistGetModel = $('#getLeadBike');

/* input focus */
assistanceGetName.on("focus", function () {
    validate.onFocus(assistanceGetName);
});

assistanceGetEmail.on("focus", function () {
    validate.onFocus(assistanceGetEmail);
});

assistanceGetMobile.on("focus", function () {
    validate.onFocus(assistanceGetMobile);
});

/* input blur */
assistanceGetName.on("blur", function () {
    validate.onBlur(assistanceGetName);
});

assistanceGetEmail.on("blur", function () {
    validate.onBlur(assistanceGetEmail);
});

assistanceGetMobile.on("blur", function () {
    validate.onBlur(assistanceGetMobile);
});


$('#submitAssistanceFormBtn').on('click', function () {
    var isValidDetails = false;
    isValidDetails &= validateBike(assistGetModel);
    isValidDetails = ValidateUserDetail(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
    if (isValidDetails) {
        return true;
    }
});

function validateBikeData() {
    if ($('#getLeadBike').val().length == 0) {
        validate.dropdown.setError($('#getLeadBike'), 'Select a bike');
        return true;
    }
    else {
        validate.dropdown.hideError($('#getLeadBike'));
        return false;
    }
}

$(document).on('click', '#assistance-response-close-btn', function () {
    $("#dealer-assist-msg").slideUp();
});


function hideFormErrors() {

    hideError(fullName);
    hideError(emailid);
    hideError(mobile);
    hideError(assistanceGetEmail);
    hideError(assistanceGetMobile);
    hideError(assistanceGetName);
    hideError(assistGetModel);
};

/* form validation */
var validate = {
    setError: function (element, message) {
        var elementLength = element.val().length;
        errorTag = element.siblings('span.error-text');

        errorTag.show().text(message);
        if (!elementLength) {
            element.closest('.input-box').removeClass('not-empty').addClass('invalid');
        }
        else {
            element.closest('.input-box').addClass('not-empty invalid');
        }
    },

    hideError: function (element) {
        element.closest('.input-box').removeClass('invalid').addClass('not-empty');
        element.siblings('span.error-text').text('');
    },

    onFocus: function (inputField) {
        if (inputField.closest('.input-box').hasClass('invalid')) {
            validate.hideError(inputField);
        }
    },

    onBlur: function (inputField) {
        var inputLength = inputField.val().length;
        if (!inputLength) {
            inputField.closest('.input-box').removeClass('not-empty');
        }
        else {
            inputField.closest('.input-box').addClass('not-empty');
        }
    },

    dropdown: {
        setError: function (element, message) {
            var dropdownWrapper = element.closest('.dropdown-select-wrapper'),
                errorTag = dropdownWrapper.find('.error-text');

            dropdownWrapper.addClass('invalid');
            errorTag.show().text(message);
        },

        hideError: function (element) {
            var dropdownWrapper = element.closest('.dropdown-select-wrapper'),
                errorTag = dropdownWrapper.find('.error-text');

            dropdownWrapper.removeClass('invalid');
            errorTag.text('');
        }
    }
}

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

function route(origin_place_id, travel_mode, directionsService, directionsDisplay) {


    _lat = dealerLat;
    _lng = dealerLng;
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

String.prototype.toHHMMSS = function () {
    var e = parseInt(this, 10)
      , a = Math.floor(e / 3600)
      , t = Math.floor((e - 3600 * a) / 60)
      , i = e - 3600 * a - 60 * t;
    a < 10 && (a = "0" + a),
    t < 10 && (t = "0" + t),
    i < 10 && (i = "0" + i);
    var o = a + " hrs " + t + " mins ";
    return o
}
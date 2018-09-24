var redMarkerImage = 'https://imgd.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';
var originPlace, userLocation = { "latitude": "", "longitude": "" }, userAddress = "";
var customerViewModel, dealerDetailsViewModel;
var dealerLat;
var googleMapAPIKey;
var dealerLong, dropdown, assistGetModel, currentAddress, pqId, pageUrl, clientIP, cityArea,dealerbikesListEle;

docReady(function () {
    pageUrl = window.location.href;
    assistGetModel = $('#getLeadBike'), dealerbikesListEle =$("#dealer-assist-btn") ;
    dealerLong = document.getElementById("locationSearch").getAttribute("data-long");
    dealerLat = document.getElementById("locationSearch").getAttribute("data-lat");
    googleMapAPIKey = document.getElementById("locationSearch").getAttribute("data-Map");
    clientIP = document.getElementById("locationSearch").getAttribute("data-clietIp");
    initializeMap();
    dropdown.setDropdown();

    assistGetModel.change(function () {
        var ele = assistGetModel.find("option:selected");
        if (ele && ele.val() && ele.val()!="0") {
            versionId = ele.val();
            if (dealerbikesListEle)
            {
                dealerbikesListEle.attr("data-versionid", ele.val()).attr("data-modelid", ele.attr("modelid"));
            }
        }
       
    });
    $(".dealerDetails").click(function (e) {
        var btnDpq = $(this);
        var pqSourceId = btnDpq.data("pqsourceid");
        var modelId = btnDpq.data("modelid");
        var versionId = btnDpq.data("versionid");
        var areaId = btnDpq.data("areaid");
        var cityId = btnDpq.data("cityid");
        var cityName = btnDpq.data("cityname");
        var areaName = btnDpq.data("areaname");
        var dealerid = btnDpq.data("dealerid");
        vmquotation.CheckCookies();
        vmquotation.IsLoading(true);
        $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
        var options = {
            "modelId": modelId,
            "versionid": versionId,
            "cityId": cityId,
            "areaId": areaId,
            "city": cityName,
            "area": areaName,
            "pagesrcid": pqSourceId,
            "dealerid": dealerid
        };
        vmquotation.IsOnRoadPriceClicked(false);
        vmquotation.setOptions(options, e);
    });
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
    
    $(document).on('click', function (event) {
        event.stopPropagation();
        var dropdown = $('.dropdown-menu');

        if (dropdown.hasClass('dropdown-active')) {
            if (!dropdown.is(event.target) && dropdown.has(event.target).length == 0) {
                dropdown.removeClass('dropdown-active');
            }
        }
    });
    $('#submitAssistanceFormBtn').on('click', function () {
        var isValidDetails = false;
        isValidDetails &= validateBike(assistGetModel);
        isValidDetails = ValidateUserDetail(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
        if (isValidDetails) {
            return true;
        }
    });
    $(document).on('click', '#assistance-response-close-btn', function () {
        $("#dealer-assist-msg").slideUp();
    });
    $(document).on("click", "#getUserLocation", function () { getLocation(); })
    

});
dropdown = {
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
            validateDropdown.dropdown.hideError(label);
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
function initializeMap() {
    var mapCanvas = document.getElementById("dealer-map");
    currentAddress = document.getElementById("locationSearch").getAttribute("data-currentaddress");
    var mapProp = {
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        center: new google.maps.LatLng(dealerLat, dealerLong),
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
            userAddress = place.formatted_address.trim();
        };

        travel_mode = google.maps.TravelMode.DRIVING;

        route(origin_place_id, travel_mode, directionsService, directionsDisplay);
        if (userAddress != "")
            $('.location-details').show();
    });
}

function savePosition(position) {
    userLocation = {
        "latitude": position.coords.latitude,
        "longitude": position.coords.longitude
    }
    if (dealerDetailsViewModel && dealerDetailsViewModel.CustomerDetails())
        dealerDetailsViewModel.CustomerDetails().userSrcLocation(userLocation.latitude + "," + userLocation.longitude);
    if (userAddress.trim() == "") {
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
    self.msg = "";
}

 

function validateBikeData() {
    if ($('#getLeadBike').val().length == 0) {
        validateDropdown.dropdown.setError($('#getLeadBike'), 'Select a bike');
        return true;
    }
    else {
        gaLabel = $('#getLeadBike option:selected').text() + '_' + cityArea;
        
        validateDropdown.dropdown.hideError($('#getLeadBike'));
        return false;
    }
}

function validateUserLeadDetails() {
    var isValidUser = false;
    isValidUser = validateName();
    isValidUser &= validatePhone();
    isValidUser &= validateEMail();
    return isValidUser;
}
function validateName() {
    var assistGetName = $('#assistGetName');
    leadFullname = assistGetName.val();
    var isValid = false;
    if (leadFullname != null && leadFullname.trim() != "") {
        nameLength = leadFullname.length;

        if (leadFullname.indexOf('&') != -1) {
            validate.setError(assistGetName, 'Invalid name');
            isValid = false;
        }
        else if (nameLength == 0) {
            validate.setError(assistGetName, 'Please enter your name');
            isValid = false;
        }
        else if (nameLength >= 1) {
            validate.hideError(assistGetName);
            isValid = true;
        }
    }
    else {
        validate.setError(assistGetName, 'Please enter your name');
        isValid = false;
    }
    return isValid;
}

function validateEMail() {
    var assistGetEmail = $('#assistGetEmail');
    var isValid = true,
        emailVal = assistGetEmail.val(),
        reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (emailVal == "") {
        isValid = true;
    }
    else if (!reEmail.test(emailVal)) {
        validate.setError(assistGetEmail, 'Invalid Email');
        isValid = false;
    }
 return isValid;
}

function validatePhone()
{
    var assistGetMobile = $('#assistGetMobile');
     leadMobileNo = assistGetMobile.val();
     if (!validateMobileNo(leadMobileNo, self)) {
         validate.setError(assistGetMobile, self.msg);
         return false;
     }
     else {
         validate.hideError(assistGetMobile);
         return true;
     }
}



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
var validateDropdown = {
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
};

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



function route(origin_place_id, travel_mode, directionsService, directionsDisplay) {


    _lat = dealerLat;
    _lng = dealerLong;
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
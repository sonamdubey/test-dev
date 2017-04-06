var originPlace, userLocation = { "latitude": "", "longitude": "" }, userAddress = "", bikeschedule, serviceLat, serviceLong, currentCityName, googleMapAPIKey;

function getLocation() {
    if (navigator.geolocation) {
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
    if (userAddress.trim() == "") {
        $.getJSON("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + userLocation.latitude + "," + userLocation.longitude + "&key=" + googleMapAPIKey, function (data) {
            if (data.status == "OK" && data.results.length > 0) {
                userAddress = data.results[0].formatted_address;
            }
            else {
                userAddress = "";
            }
            $("#locationSearch").val("").val(userAddress);
            google.maps.event.trigger(originPlace, 'place_changed');
        });
    }
    setUserLocation(position);
}

function setUserLocation(position) {
    if (position != null) {
        $("#linkMap").attr("href", "https://maps.google.com/?saddr=" + position.coords.latitude + "," + position.coords.longitude + "&daddr=" + serviceLat + "," + serviceLong + '');
    }
}

function initializeMap() {
    getLocation();
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

        var directionsService = new google.maps.DirectionsService;

        if (originPlace != null && originPlace.getPlace() != null && originPlace.getPlace().name != null && originPlace.getPlace().name.trim() != "") {
            route(origin_place_id, travel_mode, directionsService);
            $('.location-details').show();
        }
    });
}


function route(origin_place_id, travel_mode, directionsService) {
    _lat = serviceLat;
    _lng = serviceLong;
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
            setUserLocation(origin_place_id);
        }
        else {
            $(".location-details").addClass("hide");
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
    $(".location-details").removeClass("hide");
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

/* service center number */
var contactPopup = $('#contact-service-popup');

contactPopup.on('click', '.popup-list li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        contactServicePopup.set.selection(item);
    }
});

$('.contact-service-btn').on('click', function () {
    modalPopup.open(contactPopup);
    appendState('contactService');
    contactServicePopup.set.contactList($(this));
});

$('.cancel-popup-btn').on('click', function () {
    var container = $(this).closest('.modal-popup-container');
    history.back();
    modalPopup.close(container);
});

var modalPopup = {
    open: function (container) {
        container.show();
        $('html, body').addClass('lock-browser-scroll');
        $('.modal-background').show();
    },

    close: function (container) {
        container.hide();
        $('html, body').removeClass('lock-browser-scroll');
        $('.modal-background').hide();
    }
};

var contactServicePopup = {
    initial: {
        selection: function () {
            var element = contactPopup.find('.popup-list li').first(),
            elementValue = element.find('.list-label').text();

            element.addClass('active');
            contactServicePopup.set.contact(elementValue);
        }
    },

    set: {
        contactList: function (element) {
            var serviceName = element.attr('data-service-name'),
                serviceNumbers = element.attr('data-service-number'),
                contacts = serviceNumbers.split(','),
                contactsLength = contacts.length,
                popupList = contactPopup.find('.popup-list');

            popupList.empty();

            contactPopup.find('.popup-header').text(serviceName);
            for (var i = 0; i < contactsLength; i++) {
                popupList.append('<li><span class="bwmsprite radio-uncheck"></span><span class="list-label">' + contacts[i] + '</span></li>');
            };

            contactServicePopup.initial.selection();
        },

        selection: function (element) {
            var elementValue = element.find('.list-label').text();

            contactPopup.find('li.active').removeClass('active');
            element.addClass('active');

            contactServicePopup.set.contact(elementValue);
        },

        contact: function (elementValue) {
            contactPopup.find('#call-service-btn').attr('href', 'tel:' + elementValue);
        }
    }
}

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#contact-service-popup').is(':visible')) {
        modalPopup.close('#contact-service-popup');
    }
});

function SchedulesViewModel() {
    var self = this;
    self.bikes = ko.observable(bikeschedule);
    self.currentBikeName = ko.observable();
    self.bikesList = ko.observable(self.bikes());
    self.isDataExist = ko.observable();
    self.isDays = ko.observable();
    self.isKms = ko.observable();
    self.selectedModelId = ko.observable();
    self.GetModelId = function (data, event) {
        self.selectedModelId($(event.target).val());
        var bikename = ko.utils.arrayFirst(self.bikes(), function (bike) {
            return bike.ModelId == self.selectedModelId();
        });
        if (bikename != null)
            self.currentBikeName(bikename.ModelName);
    };
    function isDaysDataExists(sch) {
        var isFound = false;
        ko.utils.arrayForEach(sch, function (s) {
            if (s.Days && s.Days > 0) {
                isFound = true;
            }
        });
        return isFound;
    }
    function isKmsDataExists(sch) {
        var isFound = false;
        ko.utils.arrayForEach(sch, function (s) {
            if (s.Kms && s.Kms.length > 0) {
                isFound = true;
            }
        });
        return isFound;
    }
    self.selectedModelId.subscribe(function () {
        var arr = ko.utils.arrayFirst(self.bikes(), function (b) {
            return b.ModelId == self.selectedModelId();
        });
        self.bikesList(null);
        self.bikesList(arr);
        if (arr.Schedules.length > 0)
            self.isDataExist(true);
        else
            self.isDataExist(false);
        self.isDays(isDaysDataExists(arr.Schedules));
        self.isKms(isKmsDataExists(arr.Schedules));
    });
}

docReady(function () {
    $('.chosen-select').chosen();
    $(document).on("click", "#getUserLocation", function () { getLocation(); });
    $("#assistanceBrandInput").on("keyup", function () {
        locationFilter($(this));
    });

    serviceLat = $('#service-schedule-data').data("servicelat");
    serviceLong = $('#service-schedule-data').data("servicelong");
    currentCityName = $('#service-schedule-data').data("cityname");
    googleMapAPIKey = $('#service-schedule-data').data("googlekey");

    if ($("#service-schedule-data").html()) {
        bikeschedule = JSON.parse($("#service-schedule-data").html().replace(/\s/g, ' '));
        var vmService = new SchedulesViewModel();
        ko.applyBindings(vmService, $("#service-schedular")[0]);
        vmService.selectedModelId(vmService.bikes()[0].ModelId);
        vmService.currentBikeName($("#selBikes option:selected").text());

    }

    initializeMap();
});

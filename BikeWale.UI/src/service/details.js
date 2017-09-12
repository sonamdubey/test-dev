var redMarkerImage = 'https://imgd.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';
var originPlace, userLocation = { "latitude": "", "longitude": "" }, userAddress = "";
var customerViewModel, dealerDetailsViewModel, chosenSelectBox, vmService, bikeschedule, currentCityName, currentAddress;

function initializeMap() {
    var mapCanvas = document.getElementById("dealer-map");
    var mapProp = {
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        center: new google.maps.LatLng(serviceLat, serviceLong),
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

function SchedulesViewModel() {
    var self = this;
    self.bikes = ko.observable(bikeschedule);
    self.currentBikeName = ko.observable();
    self.bikesList = ko.observable(self.bikes());
    self.isDataExist = ko.observable();
    self.imagePath = ko.observable();
    self.isDays = ko.observable();
    self.isKms = ko.observable();
    self.selectedModelId = ko.observable();
    self.GetModelId = function (data, event) {
        self.selectedModelId($(event.target).val());
        var bikename = ko.utils.arrayFirst(self.bikes(), function (bike) {
            return bike.ModelId == self.selectedModelId();
        });
        if (bikename != null) {
            self.currentBikeName(bikename.ModelName);
        }
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
        var selbike = ko.utils.arrayFirst(self.bikes(), function (b) {
            return b.ModelId == self.selectedModelId();
        });
        self.bikesList(null);
        self.bikesList(selbike);
        if (selbike != null && selbike.Schedules.length > 0)
            self.isDataExist(true);
        else
            self.isDataExist(false);
        self.isDays(isDaysDataExists(selbike.Schedules));
        self.isKms(isKmsDataExists(selbike.Schedules));
        if (selbike.HostUrl != '' && selbike.OriginalImagePath != '') {
            self.imagePath(selbike.HostUrl + "227x128" + selbike.OriginalImagePath);
        }
        else {
            self.imagePath('https://imgd.aeplcdn.com/0x0/bikewaleimg/images/noimage.png');
        }        
    });

   
}

function setUserLocation(position) {
    $("#linkMap").attr("href", "https://maps.google.com/?saddr=" + position.lat() + "," + position.lng() + "&daddr=" + serviceLat + "," + serviceLong + '');
}


docReady(function () {

    chosenSelectBox = {    
        setPlaceholder: function() {
            $('.chosen-select').each(function () {
                var text = $(this).attr('data-placeholder');
                $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
            });
        }
    };

    $('.chosen-select').chosen();

    if ($("#service-schedule-data").html()) {
        bikeschedule = JSON.parse($("#service-schedule-data").html().replace(/\s/g, ' '));
        vmService = new SchedulesViewModel();
        ko.applyBindings(vmService, $("#service-scheduler")[0]);
        vmService.selectedModelId(vmService.bikes()[0].ModelId);
    }




    chosenSelectBox.setPlaceholder();

    serviceLat = $('#dealer-map').data("servicelat");
    serviceLong = $('#dealer-map').data("servicelong");
    currentCityName = $('#dealer-map').data("cityname");
    currentAddress = document.getElementById("locationSearch").getAttribute("data-currentaddress");
    googleMapAPIKey = document.getElementById("locationSearch").getAttribute("data-Map");
    clientIP = document.getElementById("locationSearch").getAttribute("data-clietIp");

    initializeMap();

    $(document).on("click", "#getUserLocation", function () { getLocation(); })

});
﻿var redMarkerImage = 'https://imgd3.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';
var originPlace, userLocation = { "latitude": "", "longitude": "" }, userAddress = "";
var customerViewModel, dealerDetailsViewModel;

$(document).ready(function () {
    chosenSelectBox.setPlaceholder();
});

$('.chosen-select').chosen();

var chosenSelectBox = {    
    setPlaceholder: function() {
        $('.chosen-select').each(function () {
            var text = $(this).attr('data-placeholder');
            $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
        });
    }
};

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

    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': currentCityName }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            map.fitBounds(results[0].geometry.viewport);
        }
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

$(document).on("click", "#getUserLocation", function () { getLocation(); })

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
function setUserLocation(position) {
    $("#linkMap").attr("href", "https://maps.google.com/?saddr=" + position.lat() + "," + position.lng() + "&daddr=" + serviceLat + "," + serviceLong + '');
}
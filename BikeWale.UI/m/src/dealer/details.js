var originPlace, userLocation = { "latitude": "", "longitude": "" }, userAddress = "";
initializeMap();

function getLocation() {
    if (navigator.geolocation) {        
        navigator.geolocation.getCurrentPosition(
            savePosition,
            showError,
            { enableHighAccuracy: true, maximumAge: 600000 }
        );
    }    
}

$(document).on("click", "#getUserLocation", function () { getLocation(); });

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
    $("#anchorGetDir").attr("href", "https://maps.google.com/?saddr=" + position.coords.latitude + "," + position.coords.longitude + "&daddr=" + dealerLat + "," + dealerLong + '');
}
$("#assistanceBrandInput").on("keyup", function () {
    locationFilter($(this));
});

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


var originPlace, userLocation = { "latitude": "", "longitude": "" }, userAddress = "";
var dealerLat;
var dealerLong;
var locationKey;
var googleMapAPIKey, pageUrl;
var cityName, areaName;
docReady(function () {
    dealerLong = document.getElementById("locationSearch").getAttribute("data-long");
    pageUrl = window.location.href;
    dealerLat = document.getElementById("locationSearch").getAttribute("data-lat");
    clientIP = document.getElementById("locationSearch").getAttribute("data-clietIp");
    locationKey = document.getElementById("locationSearch").getAttribute("data-cityid") + "_" + document.getElementById("locationSearch").getAttribute("data-cityname");
    googleMapAPIKey = document.getElementById("locationSearch").getAttribute("data-Map");
    SetCookieInDays("location", locationKey, 365);
    initializeMap();
    $(document).on("click", "#getUserLocation", function () {
        getLocation();
    });
    $("#assistanceBrandInput").on("keyup", function () {
        locationFilter($(this));
    });
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
    $(window).scroll(function () {
        bodHt = $('body').height();
        footerHt = $('footer').height();
        scrollPosition = $(this).scrollTop();
        if ($('.float-button').hasClass('float-fixed')) {
            if (scrollPosition + $(window).height() > (bodHt - footerHt))
                $('.float-button').removeClass('float-fixed').hide();
        }
        if (scrollPosition + $(window).height() < (bodHt - footerHt))
            $('.float-button').addClass('float-fixed').show();
    });
    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "pageurl": window.location.href,
            "clientip": clientIP,
            "isregisterpq": true,
            "isdealerbikes": true,
            "campaignId": ele.attr('data-campaignid'),
            "dealerareaname": ele.attr('data-item-area'),
            "dealercityname": ele.attr('data-cityname'),
            "eventcategory": ele.attr('data-eventcategory'),
            "gaobject": {
                cat: ele.attr("data-cat"),
                act: ele.attr("data-act"),
                lab: ele.attr("data-lab")
            }
        };
        dleadvm.setOptions(leadOptions);
    });

    $(".dealerDetails").click(function (e) {
        btnDpq = $(this);
        var pqSourceId = btnDpq.data("pqsourceid");
        var modelId = btnDpq.data("modelid");
        var versionId = btnDpq.data("versionid");
        var areaId = btnDpq.data("areaid");
        var cityId = btnDpq.data("cityid");
        var cityName = btnDpq.data("cityname");
        var areaName = btnDpq.data("areaname");
        var dealerid = btnDpq.data("dealerid");
        vmquotation.CheckCookies();
        $('#priceQuoteWidget,#popupContent,.blackOut-window').show();
        $('#popupWrapper').addClass('loader-active');
        $('#popupWrapper,#popupContent').show();
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
        vmquotation.setOptions(options, e);
    });
});

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
    try {
        $("#anchorGetDir").attr("href", "https://maps.google.com/?saddr=" + position.lat() + "," + position.lng() + "&daddr=" + dealerLat + "," + dealerLong + '');
    } catch (e) {

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




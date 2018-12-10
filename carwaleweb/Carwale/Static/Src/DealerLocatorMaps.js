var MARKERARRAY = [];
var INFOWINDOW;
var GOOGLEMAP;
var GOOGLEMAPSWORKING = true;
var ISGOOGLEAPICALLED = false;
function getGoogleMapsJs(showroomsData, mapDivId, boundAreaByMarkers) {

    if (ISGOOGLEAPICALLED == false) {

        Common.utils.loadGoogleApi(loadGoogleApiWithInputs, {
            showroomsData: showroomsData,
            mapDivId: mapDivId,
            boundAreaByMarkers: boundAreaByMarkers
        });
    }
    else {
        initializeGoogleMaps(showroomsData, mapDivId, boundAreaByMarkers);
    }
}

function loadGoogleApiWithInputs(inputs)
{
    ISGOOGLEAPICALLED = true;
    initializeGoogleMaps(inputs.showroomsData, inputs.mapDivId, inputs.boundAreaByMarkers);
}

function initializeGoogleMaps(showroomsData, mapDivId, boundAreaByMarkers) {
    //Check if google api js was imported
    if (typeof google === "undefined" || google == null || google == "") {
        GOOGLEMAPSWORKING = false;
        return;
    }
    MARKERARRAY = [];
    var mapProp = {
        center: new google.maps.LatLng(21, 83),
        zoom: 4,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    GOOGLEMAP = new google.maps.Map(document.getElementById(mapDivId), mapProp);
    INFOWINDOW = new google.maps.InfoWindow();
    var bounds = new google.maps.LatLngBounds();

    for (var i = 0; i < showroomsData.length; i++) {
        var dealerObject = showroomsData[i];
        var blackMarkerImagePath = 'https://img.carwale.com/dealer/map-marker-black.png';
        var redMarkerImagePath = 'https://img.carwale.com/dealer/map-marker-red.png';

        if (isLatLongValid(dealerObject.latitude, dealerObject.longitude)) {
            var markerPosition = new google.maps.LatLng(dealerObject.latitude, dealerObject.longitude);
            var markerIcon = blackMarkerImagePath;
            var zIndex = 1000;
            if (dealerObject.isPremium == true) {
                markerIcon = redMarkerImagePath;
                zIndex = 1001;
            }

            var marker = new google.maps.Marker({
                dealerId: dealerObject.dealerId,
                dealerName: dealerObject.name,
                position: markerPosition,
                icon: markerIcon,
                zIndex: zIndex
            });

            MARKERARRAY.push(marker);
            if (boundAreaByMarkers == true) {
                bounds.extend(marker.getPosition());
            }
            marker.setMap(GOOGLEMAP);

            var content = "<div class='dvWindow'>" +
                            "<h3 class='head'>" + dealerObject.name + "</h3>" +
                            "<div class='dvAddr'>" + dealerObject.address + ", " +
                            dealerObject.cityName + ", " + dealerObject.state + " " + dealerObject.pincode + "<br/>";
            if (dealerObject.mobileNo != null && dealerObject.mobileNo != "") {
                content = content + "<img class='map-call-ic' src='https://img.carwale.com/dealer/phone.png' /> " + dealerObject.mobileNo;
            }
            content = content + "</div>";

            //setting zoom through adding listner to 15 if ever bounds are changed and zoom level become more that 15
            google.maps.event.addListenerOnce(GOOGLEMAP, 'bounds_changed', function (event) {
                if (this.getZoom() > 15) {
                    this.setZoom(15);
                }
            });

            // adding info window on click of marker
            google.maps.event.addListener(marker, 'click', (function (marker, content, infowindow) {
                return function () {
                    infowindow.setContent(content);
                    infowindow.open(GOOGLEMAP, marker);
                };
            })(marker, content, INFOWINDOW));
        }
    }

    if (boundAreaByMarkers == true && MARKERARRAY.length > 0) {
        GOOGLEMAP.fitBounds(bounds);
    }
}

/* Functions available for users */
//Plots the Google map
function plotGoogleMap(showroomsData, mapDivId, boundAreaByMarkers) {
    if (!GOOGLEMAPSWORKING) {
        return;
    }
    getGoogleMapsJs(showroomsData, mapDivId, boundAreaByMarkers);
}

//Show a InfoWindow(tooltip) on the marker with basic details(Dealer name)
function showBasicDealerDetails(dealerId) {
    if (!GOOGLEMAPSWORKING) {
        return;
    }
    for (var i = 0; i < MARKERARRAY.length; i++) {
        if (MARKERARRAY[i].dealerId == dealerId) {
            INFOWINDOW.setContent(MARKERARRAY[i].dealerName);
            INFOWINDOW.open(GOOGLEMAP, MARKERARRAY[i]);
            break;
        }
    }
}

//Hide the current InfoWindow
function hideInfoWindow() {
    if (!GOOGLEMAPSWORKING) {
        return;
    }
    INFOWINDOW.close();
}

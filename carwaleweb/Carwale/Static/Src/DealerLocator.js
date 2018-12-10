$(document).ready(function () {
    var cityCoord = "";
    var infoWindow = null;    
});

function PanZoom(lat, lng, dealerName, addr, mobile) {
    var marker = new google.maps.Marker({
        position: new google.maps.LatLng(lat, lng),
        map: map,
        title: '',
        icon: "https://oem.carwale.com/skoda/dealernw/img/bullet-big.png"
    });
    var latlng = new google.maps.LatLng(lat, lng);
    map.autoPan = false;
    map.panTo(latlng);
    map.setZoom(15);
    var content = "<div class='dvWindow'><h3 class='head'>" + dealerName + "</h3><div class='dvAddr'>" + addr + "<br/><img src='https://img.carwale.com/skodaimage/images/call-icon.png' /> " + mobile + "</div>"
                + "<form onsubmit='calcRoute(" + lat + "," + lng + ");return false;'>"
                + "<input type='text' id='start' value=''>"
                + "<input type='submit' value='Get directions'></form>";
    google.maps.event.addListener(marker, 'click', (function (marker, content) {
        return function () {
            infowindow.close();
            infowindow = new google.maps.InfoWindow();
            infowindow.setContent(content);
            infowindow.open(map, marker);
        }
    })(marker, content));
    infowindow.setContent(content);
    infowindow.open(map, marker);
}


function initializeLoad(latlongArray, cityCoord, imgPath) {
    if (cityCoord != "") {
        var citylatlng = cityCoord.split('|');
        var iconImage = imgPath;
        var myOptions = {
            zoom: 11,
            center: new google.maps.LatLng(citylatlng[0], citylatlng[1]),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
        for (var i = 0; i < latlongArray.length; ++i) {
            var val = new String(latlongArray[i]).split('|');
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(val[0], val[1]),
                map: map,
                title: val[2],
                icon: iconImage
            });
            map.autoPan = false;
            infowindow = new google.maps.InfoWindow();

            directionDisplay = new google.maps.DirectionsRenderer();
            directionDisplay.setMap(map);
            google.maps.event.addListener(marker, 'click', (function (marker, val) {
                return function () {
                    if (val[3] == null || val[4] == null)
                        var content = "<div class='dvWindow'><h3 class='head'>" + val[2] + "</h3>" + val[3] + "</div>";
                    else
                        var content = "<div class='dvWindow'><h3 class='head'>" + val[2] + "</h3><div class='dvAddr'>" + val[3] + "<br/><img src=\"https://img.carwale.com/skodaimage/images/call-icon.png\" /> " + val[4] + "</div>"
                                         + "<form onsubmit='calcRoute(" + val[0] + "," + val[1] + ");return false;'>"
                                         + "<input type='text' id='start' value=''>"
                                         + "<input type='submit' value='Get directions'></form>";
                    infowindow.setContent(content);
                    infowindow.open(map, marker);
                }
            })(marker, val));
        }
    }
}

function preGoogleApiUsedCars(input) {
    initializeMap(input.latlongArray, input.cityCoord, input.imgPath);
}

function initializeMap(latlongArray, cityCoord, imgPath) {
    if (cityCoord != "") {
        var citylatlng = cityCoord.split('|');
        var iconImage = imgPath;
        var myOptions = {
            zoom: 11,
            center: new google.maps.LatLng(citylatlng[0], citylatlng[1]),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
        for (var i = 0; i < latlongArray.length; ++i) {
            var val = new String(latlongArray[i]).split('|');
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(val[0], val[1]),
                map: map,
                title: val[2],
            });
            map.autoPan = false;
            infowindow = new google.maps.InfoWindow();

            directionDisplay = new google.maps.DirectionsRenderer();
            directionDisplay.setMap(map);
            google.maps.event.addListener(marker, 'click', (function (marker, val) {
                return function () {
                    if (val[3] == null || val[4] == null || val[4].length==0)
                        var content = "<div class='dvWindow'><h3 class='head'>" + val[2] + "</h3>" + val[3] + "</div>";
                    else
                        var content = "<div class='dvWindow'><h3 class='head'>" + val[2] + "</h3><div class='dvAddr'>" + val[3] + "<br/><span class='fa fa-phone'></span> " + val[4] + "</div>";
                    infowindow.setContent(content);
                    infowindow.open(map, marker);
                }
            })(marker, val));
        }
    }
}

function calcRoute(lat, lng) {
    //alert(lat + " - " + lng);
    var start = document.getElementById("start").value;
    var end = lat + ", " + lng;
    var request = {
        origin: start,
        destination: end,
        travelMode: google.maps.DirectionsTravelMode.DRIVING
    };
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionDisplay.setDirections(response);
        }
        else {
            alert("Unable to locate search");
        }
    });
}
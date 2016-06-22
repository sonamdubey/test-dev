$(document).ready(function () {
    var windowWidth = window.innerWidth,
        windowHeight = window.innerHeight;
    $('#dealerCityMapWrapper, #dealersCityMap').css({ 'width': windowWidth - 355, 'height': windowHeight - 51 });
    $('#listingSidebar').css({ 'height': windowHeight });
    $('.dealer-city-map-wrapper').css({ 'height': $('#listingSidebar').height() });
});

$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).innerHeight() > $(document).height() - $('#bg-footer').innerHeight()) {
        $('#dealerCityMapWrapper').css({ 'position': 'relative', 'top': $('#bg-footer').offset().top - $('#dealerCityMapWrapper').height() - 52 });
    }
    else {
        $('#dealerCityMapWrapper').css({ 'position': 'fixed', 'top': '50px' });
    }
});

var stateArr = [
	{ id: 1, name: 'Andhra Pradesh', latitude: 16.5000, longitude: 80.6400, dealerCount: 4 },
    { id: 2, name: 'Arunachal Pradesh', latitude: 27.0600, longitude: 93.3700, dealerCount: 1 },
    { id: 3, name: 'Gujarat', latitude: 23.2167, longitude: 72.6833, dealerCount: 4 },
    { id: 4, name: 'Goa', latitude: 15.4989, longitude: 73.8278, dealerCount: 2 },
    { id: 5, name: 'Maharashtra', latitude: 18.9600, longitude: 72.8200, dealerCount: 4 }
];

var markerArr = [];
var map, infowindow;
var markerIcon = 'http://imgd2.aeplcdn.com/0x0/bw/static/design15/marker-icon.png';

function initializeMap(arrList, latPos, longPos, zoomLevel) {
    var mapProp = {
        center: new google.maps.LatLng(latPos, longPos), //india
        zoom: zoomLevel,
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        zoomControl: true,
        zoomControlOptions: {
            position: google.maps.ControlPosition.LEFT_TOP
        },
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("dealersCityMap"), mapProp);
    infowindow = new google.maps.InfoWindow();

    var i, marker, element, markerPosition, content;

    for (i = 0; i < arrList.length; i++) {
        element = arrList[i];
        markerPosition = new google.maps.LatLng(element.latitude, element.longitude);
        
        marker = new MarkerWithLabel({
            id: element.id,
            name: element.name,
            position: markerPosition,
            icon: markerIcon,
            labelText: element.dealerCount,
            labelClass: 'labels label-' + element.id
        });

        markerArr.push(marker);
        marker.setMap(map);

        content = '<div>' + element.name + '</div>';

        google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {
            return function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
            };
        })(marker, content, infowindow));
    }

}

var initialLat = 21,
    initialLong = 78,
    initialZoom = 5;
initializeMap(stateArr, initialLat, initialLong, initialZoom);

$('#listingSidebarList h3').mouseover(function () {
    var currentLI = $(this),
        currentElementId = currentLI.attr('data-state-id');
    for (var i = 0; i < markerArr.length; i++) {
        if (markerArr[i].id == currentElementId) {
            infowindow.setContent(markerArr[i].name);
            infowindow.open(map, markerArr[i]);
            break;
        }
    }
});

$("#getStateInput").on("keyup", function () {
    filterLocation($(this));
});

var filterLocation = function (filterContent) {
    var inputText = $(filterContent).val();
    inputText = inputText.toLowerCase();
    var inputTextLength = inputText.length;
    if (inputText != "") {
        $(filterContent).parents("div.listingSidebarHeading").siblings("ul").find("li").each(function () {
            var locationName = $(this).text().toLowerCase().trim();
            if (/\s/.test(locationName))
                var splitlocationName = locationName.split(" ")[1];
            else
                splitlocationName = "";

            if ((inputText == locationName.substring(0, inputTextLength)) || inputText == splitlocationName.substring(0, inputTextLength))
                $(this).show();
            else
                $(this).hide();
        });
    }
    else {
        $(filterContent).parents("div.listingSidebarHeading").siblings("ul").find("li").each(function () {
            $(this).show();
        });
    }
};

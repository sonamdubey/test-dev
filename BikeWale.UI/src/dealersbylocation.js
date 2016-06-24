﻿$(document).ready(function () {
    var windowWidth = window.innerWidth,
        windowHeight = window.innerHeight;
    if ($('#listingSidebarHeading').height() > 145) {
        $('#listingSidebarList').css({ 'padding-top': $('#listingSidebarHeading').height() + 10 });
    }
    $('#dealersMapWrapper, #dealersMap').css({ 'width': windowWidth - 355, 'height': windowHeight - 50 });
    $('#listingSidebar').css({ 'min-height': windowHeight - 50 });
    $('.dealer-map-wrapper').css({ 'height': $('#listingSidebar').height() });

});

$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).innerHeight() > $(document).height() - $('#bg-footer').innerHeight()) {
        $('#dealersMapWrapper').css({ 'position': 'relative', 'top': $('#bg-footer').offset().top - $('#dealersMapWrapper').height() - 52 });
    }
    else {
        $('#dealersMapWrapper').css({ 'position': 'fixed', 'top': '50px' });
    }
});

var stateArr = [
	{ id: 1, name: 'Andhra Pradesh', link: '#', latitude: 16.5000, longitude: 80.6400, dealerCount: 4 },
    { id: 2, name: 'Arunachal Pradesh', link: '#', latitude: 27.0600, longitude: 93.3700, dealerCount: 1 },
    { id: 3, name: 'Gujarat', link: '#', latitude: 23.2167, longitude: 72.6833, dealerCount: 4 },
    { id: 4, name: 'Goa', link: '#', latitude: 15.4989, longitude: 73.8278, dealerCount: 2 },
    { id: 5, name: 'Maharashtra', link: '#', latitude: 18.9600, longitude: 72.8200, dealerCount: 4 }
];

var cityArr = [
    { id: 90, name: 'Mumbai', link: '#', latitude: 19.085442, longitude: 72.872951, dealerCount: 21 },
    { id: 91, name: 'Panvel', link: '#', latitude: 18.998724, longitude: 73.118817, dealerCount: 15 },
    { id: 92, name: 'Chiplun', link: '#', latitude: 17.536045, longitude: 73.521305, dealerCount: 9 },
    { id: 93, name: 'Solapur', link: '#', latitude: 17.668327, longitude: 75.914494, dealerCount: 4 }
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

    map = new google.maps.Map(document.getElementById("dealersMap"), mapProp);
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

        content = '<div class="dealer-location-tooltip"><a href=' + element.link + ' class="font16 text-default">' + element.name + '</a></div>';

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

if (typeof (dealersByCity) != 'undefined') {
    initialLat = 18.9600; //lat-long for maharashtra
    initialLong = 72.8200;
    initialZoom = 7;
}

//initializeMap(stateArr, initialLat, initialLong, initialZoom);
initializeMap(cityArr, initialLat, initialLong, initialZoom);


$('.state-sidebar-list a, .city-sidebar-list a').mouseover(function () {
    var currentLI = $(this),
        currentElementId = currentLI.attr('data-item-id');
    mapsInfoWindow.open(currentElementId);
});

$('.state-sidebar-list a, .city-sidebar-list a').mouseout(function () {
    var currentLI = $(this),
        currentElementId = currentLI.attr('data-item-id');
    mapsInfoWindow.close(currentElementId);
});

var mapsInfoWindow = {
    open: function (elementId) {
        for (var i = 0; i < markerArr.length; i++) {
            if (markerArr[i].id == elementId) {
                infowindow.setContent(markerArr[i].name);
                infowindow.open(map, markerArr[i]);
                break;
            }
        }
    },

    close: function (elementId) {
        for (var i = 0; i < markerArr.length; i++) {
            if (markerArr[i].id == elementId) {
                infowindow.close();
                break;
            }
        }
    }
};

$("#getStateInput, #getCityInput").on("focus", function () {
    $("html, body").animate({ scrollTop: 0 });
});

$("#getStateInput, #getCityInput").on("keyup", function (event) {
    if (event.keyCode != 40 && event.keyCode != 38 && event.keyCode != 13) {
        filter.location($(this));
    }
    else {
        if ($(this).val().length > 0) {
            switch (event.keyCode) {
                case 40:
                    filter.topDownSelection();
                    break;
                case 38:
                    filter.bottomUpSelection();
                    break;
                case 13:
                    filter.targetSelection();
                    break;
                default:
                    break;
            }
        }
    }
});

var filter = {

    list: $("#listingSidebarList"),

    location: function (filterContent) {
        var inputText = $(filterContent).val();
        inputText = inputText.toLowerCase();
        var inputTextLength = inputText.length;
        if (inputText != "") {
            $(filterContent).parents("#listingSidebarHeading").siblings("ul").find("li").each(function () {
                var locationName = $(this).text().toLowerCase().trim();
                if (/\s/.test(locationName))
                    var splitlocationName = locationName.split(" ")[1];
                else
                    splitlocationName = "";

                if ((inputText == locationName.substring(0, inputTextLength)) || inputText == splitlocationName.substring(0, inputTextLength))
                    $(this).show().addClass("filtered");
                else
                    $(this).hide().removeClass("filtered highlight");
            });
            var filteredList = filter.list.find("li.filtered");
            filteredList.removeClass("highlight");
            filteredList.first().addClass("highlight");
            filter.highlightOnMap();
        }
        else {
            $(filterContent).parents("#listingSidebarHeading").siblings("ul").find("li").each(function () {
                $(this).show().removeClass("filtered highlight");
                filter.list.find("a").trigger("mouseout");
            });
        }
    },

    topDownSelection: function () {
        var currentSelection = filter.list.find("li.filtered.highlight"),
            nextSelection = currentSelection.next("li.filtered");
        if (nextSelection.length != 0) {
            var nextSelectionValue = nextSelection.find("a").text();
            currentSelection.removeClass("highlight");
            $("#listingSidebarHeading input").val(nextSelectionValue);
            nextSelection.addClass("highlight");
            filter.highlightOnMap();
        }
    },

    bottomUpSelection: function() {
        var currentSelection = filter.list.find("li.filtered.highlight"),
            prevSelection = currentSelection.prev("li.filtered");
        if (prevSelection.length == 0) {
            filter.list.prev("div#listingSidebarHeading").find("input[type='text']").val('');
            filter.list.find("li").show().removeClass("filtered highlight");
        }
        else {
            var prevSelectionValue = prevSelection.find("a").text();
            currentSelection.removeClass("highlight");
            $("#listingSidebarHeading input").val(prevSelectionValue);
            prevSelection.addClass("highlight");
            filter.highlightOnMap();
        }
    },

    targetSelection: function () {
        var currentSelection = filter.list.find("li.filtered.highlight"),
            targetLink = currentSelection.find("a").attr("href");
        if (typeof (targetLink) != 'undefined') {
            window.location.href = targetLink;
        }
    },

    highlightOnMap: function () {
        filter.list.find("li.highlight a").trigger("mouseover");
    }

}
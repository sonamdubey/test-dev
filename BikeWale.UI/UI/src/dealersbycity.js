$(document).ready(function () {
    var windowWidth = window.innerWidth,
        windowHeight = window.innerHeight;
    $('#dealerCityMapWrapper, #dealersCityMap').css({ 'width': windowWidth - 355, 'height': windowHeight - 51 });
    $('#cityListingSidebar').css({ 'height': windowHeight + 300 }); //300 extra for last element
    $('.dealer-city-map-wrapper').css({ 'height': $('#cityListingSidebar').height()});
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

var cityArr = [
	{ id: 50, stateId: 1, name: 'Adilabad', latitude: 15.935918, longitude: 79.281676, dealerCount: 8 },
    { id: 51, stateId: 1, name: 'Chirala', latitude: 15.565835, longitude: 79.644225, dealerCount: 9 },
    { id: 52, stateId: 1, name: 'Ponnur', latitude: 14.267815, longitude: 78.654027, dealerCount: 20 },
    { id: 53, stateId: 1, name: 'Vizianagaram', latitude: 15.008116, longitude: 78.148978, dealerCount: 14 },
    { id: 60, stateId: 2, name: 'Naharlagun', latitude: 28.387992, longitude: 93.818192, dealerCount: 24 },
    { id: 71, stateId: 3, name: 'Ahmedabad', latitude: 23.013332, longitude: 72.570804, dealerCount: 4 },
    { id: 72, stateId: 3, name: 'Patan', latitude: 23.847238, longitude: 72.124900, dealerCount: 12 },
    { id: 73, stateId: 3, name: 'Vadodara', latitude: 22.292821, longitude: 73.188180, dealerCount: 18 },
    { id: 74, stateId: 3, name: 'Kutch', latitude: 23.386078, longitude: 69.774750, dealerCount: 22 },
    { id: 80, stateId: 4, name: 'Ponda', latitude: 15.355691, longitude: 74.051793, dealerCount: 9 },
    { id: 81, stateId: 4, name: 'Mapusa', latitude: 15.261647, longitude: 74.005101, dealerCount: 4 },
    { id: 90, stateId: 5, name: 'Mumbai', latitude: 19.085442, longitude: 72.872951, dealerCount: 21 },
    { id: 91, stateId: 5, name: 'Panvel', latitude: 18.998724, longitude: 73.118817, dealerCount: 15 },
    { id: 92, stateId: 5, name: 'Chiplun', latitude: 17.536045, longitude: 73.521305, dealerCount: 9 },
    { id: 93, stateId: 5, name: 'Solapur', latitude: 17.668327, longitude: 75.914494, dealerCount: 4 }
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

        //content = '<div class="dealer-info-tooltip"><h3 class="font16 margin-bottom5"><a href="javascript:void(0)" data-tooltip-id="' + dealer.id + '" class="text-black tooltip-target-link">' + dealer.name + '</a></h3><div class="font14 text-light-grey"><div class="margin-bottom5">' + dealer.address + '</div><div><span class="bwsprite phone-grey-icon"></span><span>' + 9876543210 + '</span></div></div></div>';
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

$('#citySidebarList h3').mouseover(function () {
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

$('#citySidebarList .citySubList li').mouseover(function () {
    var currentLI = $(this),
        currentElementId = currentLI.attr('data-city-id');
    for (var i = 0; i < markerArr.length; i++) {
        if (markerArr[i].id == currentElementId) {
            infowindow.setContent(markerArr[i].name);
            infowindow.open(map, markerArr[i]);
            break;
        }
    }
});

$('#dealersList li').mouseout(function () {
    infowindow.close();
});

$('#citySidebarList').on('click', 'h3', function () {
    var clickedState = $(this),
        clickedStateParent = clickedState.parent('li'),
        clickedStateId = clickedState.attr('data-state-id'),
        clickedStateLat = clickedState.attr('data-state-lat'),
        clickedStateLong = clickedState.attr('data-state-long');
    if (!clickedState.hasClass('active-state')) {
        $('#citySidebarList h3').removeClass('active-state');
        $('#citySidebarList ul.citySubList').hide();
        clickedState.addClass('active-state');
        $('html, body').animate({ scrollTop: clickedStateParent.offset().top - 155 });
        clickedState.next('ul.citySubList').slideDown();
    }
    else if (clickedState.hasClass('active-state')) {
        $('#citySidebarList ul.citySubList').hide();
        clickedState.removeClass('active-state');
    }
    focusInState(clickedStateId);
    initializeMap(cityArr, clickedStateLat, clickedStateLong, 7);
});

function focusInState(elementId) {
    for (var i = 0; i < markerArr.length; i++) {
        if (markerArr[i].id == elementId) {
            map.setCenter(markerArr[i].getPosition());
            break;
        }
    }
};

var userCityInput, userCityInputLength, element, elementName;
var filteredCityList = $('#filteredCityList');

$('#getCityInput').on('keyup', function () {
    userCityInput = $(this).val(),
    userCityInput = userCityInput.toLowerCase(),
    userCityInputLength = userCityInput.length;
    if (userCityInput != "") {
        $('#citySidebarList').hide();
        filteredCityList.empty();
        $('#filteredCityList').show();
        $('#citySidebarList .citySubList li').find('span.city-name').each(function () {
            element = $(this);
            elementName = element.text().toLowerCase().trim();
            elementDealerCount = element.siblings('span').find('span.city-dealer-count').text();
            elementTargetLink = $(this).parent('a').attr('href');
            elementId = element.parent().parent().attr('data-city-id');
            if (/\s/.test(elementName))
                var splitlocationName = elementName.split(" ")[1];
            else
                splitlocationName = "";

            if ((userCityInput == elementName.substring(0, userCityInputLength)) || userCityInput == splitlocationName.substring(0, userCityInputLength)) {
                filteredCityList.append('<li data-city-id=' + elementId + '><a href=' + elementTargetLink + '><span class="city-name">' + element.text() + '</span> <span class="city-dealer-count">(' + elementDealerCount + ')</span></a></li>');
            }
            else {
                
            }
        });
    }
    else {
        $('#filteredCityList').hide();
        $('#citySidebarList').show();
        
    }
});
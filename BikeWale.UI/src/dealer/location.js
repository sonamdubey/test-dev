var markerArr = [], map, infowindow;
var minZoomLevel = 5;
var markerIcon = 'http://imgd2.aeplcdn.com/0x0/bw/static/design15/marker-icon.png';

$(document).ready(function () {

    var windowHeight = window.innerHeight,
        mapWrapper = $('#listing-right-column'),
        listingFooter = $('#listing-footer');

    locationMap.dimension();

    $(window).on('scroll', function () {
        var windowTop = $(window).scrollTop(),
            mapWrapperOffset = mapWrapper.offset(),
            listingFooterOffset = listingFooter.offset();


        if (windowTop > mapWrapperOffset.top) {
            locationMap.fixedPosition();

            if (windowTop > listingFooterOffset.top - windowHeight - 20) {
                locationMap.absolutePosition();
            }
        }
        else {
            locationMap.relativePosition();
        }
    });

    // filter no-result message
    $('#no-result').text('No result found!');

});

$(window).resize(function () {
    locationMap.dimension();
});

var locationMap = {
    rightColumn: $('#listing-right-column'),

    wrapper: $('#dealerMapWrapper'),

    dimension: function () {
        var windowHeight = window.innerHeight;

        $('#listing-left-column').css({
            'min-height': windowHeight
        });

        $('.dealer-map-wrapper').css({
            'height': $('#listing-left-column').height()
        });

        $('#dealerMapWrapper, #dealersMap').css({
            'width': $('#listing-right-column').width() + 1,
            'height': windowHeight + 1
        });
    },

    relativePosition: function () {
        locationMap.wrapper.css({
            'position': 'relative',
            'top': 0,
            'left': 0
        });
    },

    absolutePosition: function () {
        locationMap.wrapper.css({
            'position': 'absolute',
            'top': 'auto',
            'left': 0,
            'bottom': 0
        });
    },

    fixedPosition: function () {
        locationMap.wrapper.css({
            'position': 'fixed',
            'top': 0,
            'left': locationMap.rightColumn.offset().left
        });
    }
};

function initializeMap(arrList, latPos, longPos, zoomLevel) {

    var mapCanvas = document.getElementById("dealersMap");
    var mapProp = {
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        center: new google.maps.LatLng(latPos, longPos),
        zoom: zoomLevel,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(mapCanvas, mapProp);
    infowindow = new google.maps.InfoWindow();

    var i, marker, element, markerPosition, content;
    markerArr = []; // empty the marker array
    for (i = 0; i < arrList.length; i++) {
        element = arrList[i];
        markerPosition = new google.maps.LatLng(element.latitude, element.longitude);
        marker = new MarkerWithLabel({
            id: element.id,
            name: element.name,
            position: markerPosition,
            icon: markerIcon,
            labelText: '<a href="' + element.link + '" class="tooltip-marker" data-id="'+ element.id +'">' + element.dealerCount + '</a>',
            labelClass: 'labels label-' + element.id
        });

        markerArr.push(marker);
        marker.setMap(map);

        content = '<div class="dealer-location-tooltip"><a href=' + element.link + ' class="tooltip-marker font16 text-default" data-id="' + element.id + '">' + element.name + '</a></div>';

        google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {
            return function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
            };
        })(marker, content, infowindow));

        google.maps.event.addListener(map, 'zoom_changed', function () {
            if (map.getZoom() < minZoomLevel) map.setZoom(minZoomLevel);
        });
    }
}

var initialLat = 21,
    initialLong = 78,
    initialZoom = 5,
    locationArr = [
        { id: 1, name: 'Andhra Pradesh', latitude: 15.8651212, longitude: 78.5260235, dealerCount: 10},
        { id: 2, name: 'Assam', latitude: 26.0360114, longitude: 90.6049718, dealerCount: 44 },
        { id: 3, name: 'Delhi', latitude: 28.6466771, longitude: 76.8130664, dealerCount: 23 },
        { id: 4, name: 'Goa', latitude: 15.3484145, longitude: 73.4544214, dealerCount: 5 },
        { id: 5, name: 'Gujarat', latitude: 22.4013323, longitude: 69.089417, dealerCount: 15 }
    ]

initializeMap(locationArr, initialLat, initialLong, initialZoom); //india

$('#location-list').on('mouseover', '.item-state a', function () {
    var currentLI = $(this),
        currentElementId = currentLI.attr('data-item-id');
    mapsInfoWindow.open(currentElementId);
});

$('#location-list').on('mouseout', '.item-state a', function () {
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

/* state links */
$('#location-list').on('click', '.type-state', function (event) {
    var item = $(this),
        bgFooter = $('#bg-footer');

    event.preventDefault();

    if (!item.hasClass('active')) {
        $('.location-list-city').hide();
        item.addClass('active').siblings('.location-list-city').show();
        var cityArr = [
            { id: 51, name: 'Amalapuram', latitude: 16.5738383, longitude: 81.9775335, dealerCount: 1 },
            { id: 52, name: 'Anantapur', latitude: 14.6649496, longitude: 77.5241654, dealerCount: 1 },
            { id: 53, name: 'Bhimavaram', latitude: 16.5432696, longitude: 81.4811214, dealerCount: 1 },
            { id: 54, name: 'Chirala', latitude: 15.8290249, longitude: 80.293576, dealerCount: 1 },
            { id: 55, name: 'Chittoor', latitude: 13.3080257, longitude: 78.4986888, dealerCount: 1 }
        ];
        initializeMap(cityArr, 15.8651212, 78.5260235, 7); // set map with city lat-long
    }
    else {
        item.removeClass('active').siblings('.location-list-city').hide();
        initializeMap(locationArr, initialLat, initialLong, initialZoom); // reset map with india lat-long
    }

    $('.dealer-map-wrapper').css({
        'height': $('#listing-left-column').height()
    });

    if ($(window).scrollTop > $('#listing-left-column').offset().top) {
        locationMap.fixedPosition(); // set map to default fixed position
    }
    
    if ($(window).scrollTop() > (bgFooter.offset().top - window.innerHeight)) { // reset map position
        locationMap.absolutePosition();
    }    
    
});

$('#listing-right-column').on('click', '.tooltip-marker', function (event) {
    var item = $(this),
        itemId = item.attr('data-id');

    event.preventDefault();

    $('#location-list .type-state[data-item-id="'+ itemId +'"]').trigger('click');

});

/* filter */
$("#getCityInput").on("keyup", function (event) {
    $('.location-list-city').show();
    filter.location($(this));
});

var filter = {

    location: function (filterContent) {
        var inputText = $(filterContent).val(),
            inputTextLength = inputText.length,
            elementList = $('.location-list-city li'),
            len = elementList.length,
            element, i;

        inputText = inputText.toLowerCase();
        
        if (inputText != "") {
            for (i = 0; i < len; i++) {
                element = elementList[i];

                var locationName = $(element).text().toLowerCase().trim();
                if (/\s/.test(locationName))
                    var splitlocationName = locationName.split(" ")[1];
                else
                    splitlocationName = "";

                if ((inputText == locationName.substring(0, inputTextLength)) || inputText == splitlocationName.substring(0, inputTextLength)) {
                    element.style.display = "block";
                }
                else { 
                    element.style.display = "none";
                }
            }

            var cityList = $('.location-list-city'),
                visibleState = 0;

            cityList.each(function () {
                var visibleElements = $(this).find('li[style*="display: block;"]').length;

                if (visibleElements == 0) {
                    $(this).closest('li').hide();
                }
                else {
                    $(this).closest('li').show();
                    visibleState++;
                }
            });

            if (visibleState == 0) {
                $('#no-result').show();
            }
            else {
                $('#no-result').hide();
            }
        }
        else {
            for (i = 0; i < len; i++) {
                element = elementList[i];
                element.style.display = "block";
            }
            $('.item-state').show();
            $('#no-result, .location-list-city').hide();
        }
    }
}

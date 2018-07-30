var markerArr = [], map, infowindow, locationMap, filter, mapsInfoWindow;
var minZoomLevel = 5;
var markerIcon = 'https://imgd.aeplcdn.com/0x0/bw/static/design15/marker-icon.png';
var initialLat = 23.2134079, initialLong = 81.3530178, initialZoom = 5, locationArr = [];


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
            labelText: element.isstate ? '<a href="javascript:void(0);" rel="nofollow" class="tooltip-marker" data-id="' + element.id + '">' + element.dealerCount + '</a>' : '<a href="' + element.link + '"  data-id="' + element.id + '">' + element.dealerCount + '</a>',
            labelClass: 'labels label-' + element.id
        });

        markerArr.push(marker);
        marker.setMap(map);
        content = element.isstate ? '<a href="javascript:void(0);" rel="nofollow" class="tooltip-marker" data-id="' + element.id + '">' + element.name + '</a>' : '<div class="dealer-location-tooltip"><a href="' + element.link + '" class=" font16 text-default" data-id="' + element.id + '">' + element.name + '</a></div>';

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

function mapDealersArray() {
    $("ul#location-list>li>p").each(function () {
        _self = $(this);
        _objStateCity = new Object();
        _objStateCity.id = _self.attr("data-item-id");
        _objStateCity.name = _self.attr("data-item-name")
        _objStateCity.latitude = _self.attr("data-lat");
        _objStateCity.longitude = _self.attr("data-long");
        _objStateCity.dealerCount = _self.attr("data-dealercount");
        _objStateCity.isstate = true;
        locationArr.push(_objStateCity);
    });
}

function mapCityArray(listitem) {
    var citynewArr = [];
    $(listitem).next('ul').children('li').children().each(function () {
        _self = $(this);
        _objCity = new Object();
        _objCity.id = _self.attr("data-item-id");
        _objCity.name = _self.attr("data-item-name")
        _objCity.latitude = _self.attr("data-lat");
        _objCity.longitude = _self.attr("data-long");
        _objCity.dealerCount = _self.attr("data-dealercount");
        _objCity.link = _self.attr("data-link");
        _objCity.isstate = false;
        citynewArr.push(_objCity);
    });
    return citynewArr;
}

function initializeDealerMap() {
    mapDealersArray();
    initializeMap(locationArr, initialLat, initialLong, initialZoom); //india
};

docReady(function () {

    locationMap = {
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

    mapsInfoWindow = {
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

    filter = {

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
    };

    var windowHeight = window.innerHeight,
		mapWrapper = $('#listing-right-column'),
		listingFooter = $('#listing-footer');

    locationMap.dimension();

    initializeDealerMap();

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
   
    $(window).resize(function () {
        locationMap.dimension();
    });

    $('#location-list').on('mouseover', '.item-state p, .location-list-city a', function () {
        var currentLI = $(this),
            currentElementId = currentLI.attr('data-item-id');
        mapsInfoWindow.open(currentElementId);
    });

    $('#location-list').on('mouseout', '.item-state p, .location-list-city a', function () {
        var currentLI = $(this),
            currentElementId = currentLI.attr('data-item-id');
        mapsInfoWindow.close(currentElementId);
    });


    /* state links */
    $('#location-list').on('click', '.type-state', function (event) {
        var item = $(this),
            bgFooter = $('#bg-footer');

        event.preventDefault();

        if (!item.hasClass('active')) {
            $('.location-list-city').hide();
            $('.type-state.active').removeClass('active');
            item.addClass('active').siblings('.location-list-city').show();
            var cityArr = mapCityArray(item);
            initializeMap(cityArr, item.attr('data-lat'), item.attr('data-long'), 7); // set map with city lat-long
            $('html, body').animate({ scrollTop: item.offset().top });
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

        $('#location-list .type-state[data-item-id="' + itemId + '"]').trigger('click');

    });

    /* filter */
    $("#getCityInput").on("keyup", function (event) {
        $('.location-list-city').show();
        filter.location($(this));
    });

});
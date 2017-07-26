var markerArr = [], dealerArr = [], map, infowindow, readMoreTarget, dealerMoreContent, mapDimension, currentCityName;
var blackMarkerImage = 'https://imgd.aeplcdn.com/0x0/bw/static/design15/map-marker-black.png';
var redMarkerImage = 'https://imgd.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';
var clientip;
function initializeMap(dealerArr) {
    var i, marker, dealer, markerPosition, content, zIndex;
    currentCityName = document.getElementById("dealerMapWrapper").getAttribute("data-currentcityname");
    var mapProp = {
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
    for (i = 0; i < dealerArr.length; i++) {
        dealer = dealerArr[i];
        markerPosition = new google.maps.LatLng(dealer.latitude, dealer.longitude);
        if (!dealer.isFeatured) {
            markerIcon = blackMarkerImage;
            zIndex = 100;
        }
        else {
            markerIcon = redMarkerImage;
            zIndex = 101;
        }
        marker = new google.maps.Marker({
            dealerId: dealer.id,
            dealerName: dealer.name,
            dealerNumber: dealer.maskingNumber,
            position: markerPosition,
            icon: markerIcon,
            zIndex: zIndex
        });

        markerArr.push(marker);
        marker.setMap(map);
        if (dealer.maskingNumber == '')
            content = '<div class="dealer-info-tooltip"><a href="' + dealer.id + '-' + dealer.dealermaskingname + '/" class="text-black block"><p class="font16 text-bold margin-bottom5">' + dealer.name + '</p><div class="font14 text-light-grey"><div class="margin-bottom5">' + dealer.address + '</div></div></a></div>';
        else
            content = '<div class="dealer-info-tooltip"><a href="' + dealer.id + '-' + dealer.dealermaskingname + '/" class="text-black block"><p class="font16 text-bold margin-bottom5">' + dealer.name + '</p><div class="font14 text-light-grey"><div class="margin-bottom5">' + dealer.address + '</div><div><span class="bwsprite phone-black-icon vertical-top margin-right5"></span><span class="vertical-top dealership-card-details">' + dealer.maskingNumber + '</span></div></div></a></div>';
        google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {

            return function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
            };
        })(marker, content, infowindow));

        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': currentCityName + ", India" }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                map.fitBounds(results[0].geometry.viewport);
            }
        });
    }
}

function mapDealersArray() {
    $("ul#dealersList li").each(function () {
        _self = $(this);
        _dealer = new Object();
        _dealer.id = _self.attr("data-item-id");
        _dealer.isFeatured = (_self.attr("data-item-type") == "3" || _self.attr("data-item-type") == "2");
        _dealer.latitude = _self.attr("data-lat");
        _dealer.longitude = _self.attr("data-log");
        _dealer.address = _self.attr("data-address");
        _dealer.name = _self.find(".dealer-name").text();
        _dealer.maskingNumber = _self.attr("data-item-number");
        _dealer.dealermaskingname = _self.attr("data-item-url");
        dealerArr.push(_dealer);
    });
}

function initializeDealerMap() {
    mapDealersArray();
    initializeMap(dealerArr);
};

docReady(function () {
    clientip = document.getElementById("dealerLead").getAttribute("data-clientip");
    var windowHeight = window.innerHeight,
        mapWrapper = $('#listing-right-column'),
        mapColumn = $('#dealerMapWrapper'),
        listingFooter = $('#listing-footer');


    $('#listing-left-column').css({
        'min-height': windowHeight
    });

    // read more-collapse
    readMoreTarget = $('#read-more-target'), dealerMoreContent = $('#dealer-more-content');

    mapDimension = function () {
        var windowHeight = window.innerHeight;

        $('.dealer-map-wrapper').css({
            'height': $('#listing-left-column').height()
        });

        $('#dealerMapWrapper, #dealersMap').css({
            'width': $('#listing-right-column').width() + 1,
            'height': windowHeight + 1
        });
    };
    
    mapDimension();
    initializeDealerMap();
    $(window).on('scroll', function () {
        var windowTop = $(window).scrollTop(),
            mapWrapperOffset = mapWrapper.offset(),
            listingFooterOffset = listingFooter.offset();

        if (windowTop > mapWrapperOffset.top) {
            mapColumn.css({
                'position': 'fixed',
                'top': 0,
                'left': mapWrapperOffset.left
            });

            if (windowTop > listingFooterOffset.top - windowHeight - 20) {
                mapColumn.css({
                    'position': 'absolute',
                    'top': 'auto',
                    'left': 0,
                    'bottom': 0
                });
            }
        }
        else {
            mapColumn.css({
                'position': 'relative',
                'top': 0,
                'left': 0
            });
        }  
    });

    // dealer card mouseover show tooltip
    $(document).on('mouseover', '#dealersList li', function () {

        var currentLI = $(this),
            currentDealerId = currentLI.attr('data-item-id');
        var latitude = currentLI.attr('data-lat');
        var longitude = currentLI.attr('data-log');
        if (latitude != 0 || longitude != 0) {
            for (var i = 0; i < markerArr.length; i++) {
                if (markerArr[i].dealerId == currentDealerId) {
                    infowindow.setContent(markerArr[i].dealerName);
                    infowindow.open(map, markerArr[i]);
                    break;
                }
            }
        }
    });

    $(window).resize(function () {
        mapDimension();
    });

    readMoreTarget.on('click', function () {
        if (!dealerMoreContent.hasClass('active')) {
            dealerMoreContent.addClass('active');
            readMoreTarget.text('Collapse');
        }
        else {
            dealerMoreContent.removeClass('active');
            readMoreTarget.text('Read more');
        }
    });
    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "dealerarea": ele.attr('data-item-area'),
            "campid": ele.attr('data-campid'),
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "isdealerbikes": true,
            "pageurl": window.location.href,
            "isregisterpq": true,
            "clientip": clientip,
            "dealercityname": ele.attr('data-cityname'),
            "dealerareaname": ele.attr('data-areaname'),
            "eventcategory": ele.attr('data-eventcategory')
        };
        dleadvm.setOptions(leadOptions);
    });

});
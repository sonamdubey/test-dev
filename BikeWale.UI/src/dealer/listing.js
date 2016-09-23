var dealerArr = [
	{ id: 13151, isPremium: true, name: 'BW_Bajaj Dealer(pre)', address: 'Paragon Plaza, Phoenix MarketCity, Unit Nos 24/25, Next to Maruti Showroom, LBS Road, Ghatkopar (W)', latitude: 19.066206, longitude: 72.999041 },
	{ id: 21067, isPremium: true, name: 'Mysql test dealer', address: '', latitude: 19.068118, longitude: 72.998643 },
	{ id: 21072, isPremium: false, name: 'bw_test honda', address: 'automotive exchange pvt ltd,12th floor, vishwaroop it park sector 30a, vashi, navi mumbaii - 400705', latitude: 19.072285, longitude: 72.998139 },
	{ id: 21193, isPremium: false, name: 'test campaign', address: 'vashi aepl', latitude: 19.073218, longitude: 72.995725 },
    { id: 21071, isPremium: false, name: 'campaign test', address: 'automotive exchange pvt ltd,12th floor, vishwaroop it park sector 30a, vashi, navi mumbaii - 400705', latitude: 19.074218, longitude: 72.996725 }
];

var markerArr = [], map, infowindow;
var blackMarkerImage = 'http://imgd2.aeplcdn.com/0x0/bw/static/design15/map-marker-black.png';
var redMarkerImage = 'http://imgd3.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';

$(document).ready(function () {
    var windowHeight = window.innerHeight,
        mapWrapper = $('#listing-right-column'),
        mapColumn = $('#dealerMapWrapper'),
        listingFooter = $('#listing-footer');

    mapDimension();
    
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

});

$(window).resize(function () {
    mapDimension();
});

var mapDimension = function () {
    var windowHeight = window.innerHeight;

    $('.dealer-map-wrapper').css({
        'height': $('#listing-left-column').height()
    });

    $('#dealerMapWrapper, #dealersMap').css({
        'width': $('#listing-right-column').width() + 1,
        'height': windowHeight + 1
    });
};

function initializeMap(dealerArr) {
    var i, marker, dealer, markerPosition, content, zIndex;

    var mapProp = {
        center: new google.maps.LatLng(19.07016, 73), //vashi
        zoom: 14,
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
        markerIcon = blackMarkerImage;
        zIndex = 100;

        if (dealer.isPremium) {
            markerIcon = redMarkerImage;
            zIndex = 101;
        }

        marker = new google.maps.Marker({
            dealerId: dealer.id,
            dealerName: dealer.name,
            position: markerPosition,
            icon: markerIcon,
            zIndex: zIndex
        });

        markerArr.push(marker);
        marker.setMap(map);

        content = '<div class="dealer-info-tooltip"><a href="" class="text-black block"><p class="font16 text-bold margin-bottom5">' + dealer.name + '</p><div class="font14 text-light-grey"><div class="margin-bottom5">' + dealer.address + '</div><div><span class="bwsprite phone-black-icon vertical-top margin-right5"></span><span class="vertical-top dealership-card-details">' + 9876543210 + '</span></div></div></a></div>';

        google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {
            return function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
            };
        })(marker, content, infowindow));

    }

}

// dealer card mouseover show tooltip
$(document).on('mouseover', '#dealersList li', function () {
    var currentLI = $(this),
        currentDealerId = currentLI.attr('data-item-id');
    for (var i = 0; i < markerArr.length; i++) {
        if (markerArr[i].dealerId == currentDealerId) {
            infowindow.setContent(markerArr[i].dealerName);
            infowindow.open(map, markerArr[i]);
            break;
        }
    }
});

initializeMap(dealerArr);
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

function initializeMap() {

    var mapCanvas = document.getElementById("dealersMap");
    var mapProp = {
        scrollwheel: false,
        streetViewControl: false,
        mapTypeControl: false,
        center: new google.maps.LatLng(19.0720625, 72.8550991),
        zoom: 11,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    var map = new google.maps.Map(mapCanvas, mapProp);
}

initializeMap();
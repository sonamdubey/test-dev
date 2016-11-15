var markerArr = [],dealerArr = [], map, infowindow;
var blackMarkerImage = 'http://imgd2.aeplcdn.com/0x0/bw/static/design15/map-marker-black.png';
var redMarkerImage = 'http://imgd3.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';

$(document).ready(function () {
    var windowHeight = window.innerHeight,
        mapWrapper = $('#listing-right-column'),
        mapColumn = $('#dealerMapWrapper'),
        listingFooter = $('#listing-footer');

    $('#listing-left-column').css({
        'min-height': windowHeight
    });

    
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
        else{
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

// dealer card mouseover show tooltip
$(document).on('mouseover', '#center-list li', function () {
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

mapDealersArray();
initializeMap(dealerArr);

function mapDealersArray() {
    $("ul#center-list li").each(function () {
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

// read more-collapse
var readMoreTarget = $('#read-more-target'),
    serviceMoreContent = $('#service-more-content');

readMoreTarget.on('click', function () {
    if (!serviceMoreContent.hasClass('active')) {
        serviceMoreContent.addClass('active');
        readMoreTarget.text('Collapse');
    }
    else {
        serviceMoreContent.removeClass('active');
        readMoreTarget.text('Read more');
    }
});

$('#center-list').on('click', '.get-details-btn', function () {
    var element = $(this),
        leadCaptureContainer = element.siblings('.lead-mobile-content');

    element.hide();
    leadCaptureContainer.show();
    leadCaptureContainer.find('input').focus();
});

$('.service-center-lead-mobile').on('focus', function () {
    var inputField = $(this);

    validate.onFocus(inputField);
});

$('.service-center-lead-mobile').on('blur', function () {
    var inputField = $(this);

    validate.onBlur(inputField);
});

$('.submit-service-center-lead-btn').on('click', function () {
    var element = $(this),
        inputbox = element.siblings('.input-box').find('input'),
        valid = validatePhone(inputbox);
    if (!valid) {
        // invalid
    }
    else {
        var inputParent = element.closest('.service-center-lead-content');
        inputParent.hide();
        inputParent.siblings('.service-center-lead-response').show();

    }
});

function validatePhone(inputElement) {
    var leadMobileNo = inputElement.val();
    var isValid = true,
      reMobile = /^[1-9][0-9]{9}$/;
    if (leadMobileNo == "") {
        validate.setError(inputElement, "Enter your mobile number");
        isValid = false;
    }
    else if (leadMobileNo[0] == "0") {
        validate.setError(inputElement, "Mobile no. should not start with 0");
        isValid = false;
    }
    else if (!reMobile.test(leadMobileNo) && isValid) {
        validate.setError(inputElement, "10 digit mobile number only!");
        isValid = false;
    }
    else
        validate.hideError(inputElement)
    return isValid;
}

/* form validation */
var validate = {
    setError: function (element, message) {
        var elementLength = element.val().length;
        errorTag = element.siblings('span.error-text');

        errorTag.show().text(message);
        if (!elementLength) {
            element.closest('.input-box').removeClass('not-empty').addClass('invalid');
        }
        else {
            element.closest('.input-box').addClass('not-empty invalid');
        }
    },

    hideError: function (element) {
        element.closest('.input-box').removeClass('invalid').addClass('not-empty');
        element.siblings('span.error-text').text('');
    },

    onFocus: function (inputField) {
        if (inputField.closest('.input-box').hasClass('invalid')) {
            validate.hideError(inputField);
        }
    },

    onBlur: function (inputField) {
        var inputLength = inputField.val().length;
        if (!inputLength) {
            inputField.closest('.input-box').removeClass('not-empty');
        }
        else {
            inputField.closest('.input-box').addClass('not-empty');
        }
    }
}
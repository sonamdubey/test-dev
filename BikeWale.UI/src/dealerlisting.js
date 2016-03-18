$(document).ready(function () {
    var windowWidth = window.innerWidth,
        windowHeight = window.innerHeight;
    $('#dealerMapWrapper, #dealersMap').css({ 'width': windowWidth - 355, 'height': windowHeight - 50 });
    $('.dealer-map-wrapper').css({ 'height': $('#dealerListingSidebar').height() + 1});
});

$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).innerHeight() > $(document).height() - $('#bg-footer').innerHeight()) {
        $('#dealerMapWrapper').css({ 'position': 'relative', 'top': $('#bg-footer').offset().top - $('#dealerMapWrapper').height() - 52 });        
    }
    else {
        $('#dealerMapWrapper').css({ 'position': 'fixed', 'top': '50px' });
    }
});

var dealerArr = [
	{ id: 10, isPremium: true, name: 'Dealer 1', address: 'Bhagwan Mahaveer Rd, Sector 30A, Vashi', latitude: 19.066206, longitude: 72.999041 },
	{ id: 21, isPremium: false, name: 'Dealer 2', address: 'Panvel Hwy, Sector 17, Vashi Navi Mumbai, Maharashtra', latitude: 19.068118, longitude: 72.998643 },
	{ id: 32, isPremium: false, name: 'Dealer 3', address: 'Sector 17, Vashi, Navi Mumbai', latitude: 19.072285, longitude: 72.998139 },
	{ id: 43, isPremium: true, name: 'Dealer 4', address: 'Sector 2, Vashi, Maharashtra', latitude: 19.073218, longitude: 72.995725 },
    { id: 54, isPremium: false, name: 'Dealer 5', address: 'Sector 2, Maharashtra', latitude: 19.074218, longitude: 72.996725 }
];

var markerArr = [];
var map, infowindow;
var blackMarkerImage = 'http://imgd2.aeplcdn.com/0x0/bw/static/design15/map-marker-black.png';
var redMarkerImage = 'http://imgd3.aeplcdn.com/0x0/bw/static/design15/map-marker-red.png';

function initializeMap(dealerArr) {
    var mapProp = {
        center: new google.maps.LatLng(19.07016, 73), //vashi
        zoom: 15,
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
    
    var i, marker, dealer, markerPosition, content, zIndex;

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

        //content = '<div class="dealer-info-tooltip"><h3 class="font16 margin-bottom5"><a href="javascript:void(0)" data-tooltip-id="' + dealer.id + '" class="text-black tooltip-target-link">' + dealer.name + '</a></h3><div class="font14 text-light-grey"><div class="margin-bottom5">' + dealer.address + '</div><div class="margin-bottom5"><span class="bwsprite tel-sm-grey-icon"></span><span>' + 9876543210 + '</span></div><div class="margin-bottom10"><a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span>bikewale@motors.com</a></div><div class="text-link"><a href="">Get direction</a></div></div></div>';
        content = '<div class="dealer-info-tooltip"><h3 class="font16 margin-bottom5"><a href="javascript:void(0)" data-tooltip-id="' + dealer.id + '" class="text-black tooltip-target-link">' + dealer.name + '</a></h3><div class="font14 text-light-grey"><div class="margin-bottom5">' + dealer.address + '</div><div><span class="bwsprite phone-grey-icon"></span><span>' + 9876543210 + '</span></div></div></div>';

        google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {
            return function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
            };
        })(marker, content, infowindow));

        google.maps.event.addListener(marker, 'click', (function (marker, infowindow) {
            return function () {
                infowindow.close();
                getDealerFromSidebar(marker.dealerId);
            };
        })(marker, infowindow));
    }

}

$('#dealersMap').on('click', 'a.tooltip-target-link', function () {
    getDealerFromSidebar($(this).attr('data-tooltip-id'));
});

$('#dealersList li').mouseover(function () {
    var currentLI = $(this),
        currentDealerId = currentLI.attr('data-dealer-id');
    for (var i = 0; i < markerArr.length; i++) {
        if (markerArr[i].dealerId == currentDealerId) {
            infowindow.setContent(markerArr[i].dealerName);
            infowindow.open(map, markerArr[i]);
            break;
        }
    }
});

$('#dealersList li').mouseout(function () {
    infowindow.close();
});

$('#dealersList li').on('click', 'a.dealer-sidebar-link', function () {
    var parentLI = $(this).parents('li');
    selectedDealer(parentLI);
});

var flag = false;
var getDealerFromSidebar = function (tooltipId) {
    var dealer;
    $('#dealersList li').each(function () {
        dealer = $(this);
        if (dealer.attr('data-dealer-id') == tooltipId) {
            flag = true;
            if (!dealer.hasClass('active')) {
                selectedDealer(dealer);
            }
            if (flag)
                return false;
        }
    });
}

var dealerId;

var selectedDealer = function (dealer) {
    infowindow.close();
    $('#sidebarHeader').removeClass('border-solid-bottom');
    $('html, body').animate({ scrollTop: dealer.offset().top - 103 }, { complete: function () { $('body').addClass('hide-scroll') } });
    $('#dealerDetailsSliderCard').show().animate({ 'right': '338px' });
    $('#dealerDetailsSliderCard').css({ 'height': $(window).innerHeight() - 52 });
    dealer.addClass('active');
    dealer.siblings().removeClass('active');
    dealerId = dealer.attr('data-dealer-id');
};

initializeMap(dealerArr);

$('.dealer-slider-close-btn').on('click', function () {
    $('#sidebarHeader').addClass('border-solid-bottom');
    $('body').removeClass('hide-scroll');
    $('#dealerDetailsSliderCard').animate({ 'right': '-338px' }, { complete: function () { $('#dealerDetailsSliderCard').hide().css({ 'height': '0' }); } });
    $('#dealersList li').removeClass('active');
    console.log(dealerId);
    showPrevDealer(dealerId);
});

var showPrevDealer = function (dealerId) {
    for (var i = 0; i < markerArr.length; i++) {
        if (markerArr[i].dealerId == dealerId) {
            infowindow.setContent(markerArr[i].dealerName);
            infowindow.open(map, markerArr[i]);
            break;
        }
    }
}

$(document).keydown(function (e) {
    if (e.keyCode == 27) {
        $('.dealer-slider-close-btn').trigger('click');
    }
});

var assistGetName = $('#assistGetName'),
    assistGetEmail = $('#assistGetEmail'),
    assistGetMobile = $('#assistGetMobile'),
    assistGetModel = $('#assistGetModel');

$('#submitAssistanceFormBtn').on('click', function () {
    if (validateUserInfo(assistGetName, assistGetEmail, assistGetMobile, assistGetModel)) {
    }
});

var validateUserInfo = function (leadUsername, leadEmailId, leadMobileNo, leadModelName) {
    var isValid = true;
    isValid = validateUserName(leadUsername);
    isValid &= validateEmailId(leadEmailId);
    isValid &= validateMobileNo(leadMobileNo);
    isValid &= validateModelName(leadModelName);
    return isValid;
};

var validateUserName = function (leadUsername) {
    var isValid = true,
		nameLength = leadUsername.val().length;
    if (leadUsername.val().indexOf('&') != -1) {
        setError(leadUsername, 'Invalid name');
        isValid = false;
    }
    else if (nameLength == 0) {
        setError(leadUsername, 'Please enter your name');
        isValid = false;
    }
    else if (nameLength >= 1) {
        hideError(leadUsername);
        isValid = true;
    }
    return isValid;
};

var validateEmailId = function (leadEmailId) {
    var isValid = true,
		emailVal = leadEmailId.val(),
		reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (emailVal == "") {
        setError(leadEmailId, 'Please enter email id');
        isValid = false;
    }
    else if (!reEmail.test(emailVal)) {
        setError(leadEmailId, 'Invalid Email');
        isValid = false;
    }
    return isValid;
};

var validateMobileNo = function (leadMobileNo) {
    var isValid = true,
		mobileVal = leadMobileNo.val(),
		reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(leadMobileNo, "Please enter your mobile no.");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(leadMobileNo, "Mobile no. should be 10 digits");
        isValid = false;
    }
    else
        hideError(leadMobileNo)
    return isValid;
};

var validateModelName = function (leadModelName) {
    var isValid = true,
		modelNameLength = leadModelName.val().length;
    if (leadModelName.val().indexOf('&') != -1) {
        setError(leadModelName, 'Invalid model name');
        isValid = false;
    }
    else if (modelNameLength == 0) {
        setError(leadModelName, 'Please enter model name');
        isValid = false;
    }
    else if (modelNameLength >= 1) {
        hideError(leadModelName);
        isValid = true;
    }
    return isValid;
};

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};

$("#assistGetName, #assistGetModel").on("focus", function () {
    hideError($(this));
});

$("#assistGetEmail").on("focus", function () {
    hideError($(this));
});

$("#assistGetMobile").on("focus", function () {
    hideError($(this));
});

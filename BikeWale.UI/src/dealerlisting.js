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

$('#dealersList li').on('click', 'a.dealer-sidebar-link,a.get-assistance-btn', function () {
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
    $("#assistGetName").focus();
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


///////////////////////////////////////////////////////////////////////////////////////////////////////////

var getCityArea = GetGlobalCityArea();

var leadBtnBookNow = $("#get-assistance-btn"), leadCapturePopup = $("#leadCapturePopup");
var fullName = $("#getFullName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");



var detailsSubmitBtn = $("#user-details-submit-btn, #buyingAssistBtn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");

var prevEmail = "";
var prevMobile = "";

var getCityArea = GetGlobalCityArea();
var customerViewModel = new CustomerModel();

$(function () {
    leadBtnBookNow.on('click', function () {
        leadCapturePopup.show();
        $("#dealer-lead-msg").hide();
        $("div#contactDetailsPopup").show();
        $("#otpPopup").hide();
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window").show();

    });
    $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
        leadCapturePopup.hide();
        $('body').removeClass('lock-browser-scroll');
        $(".blackOut-window").hide();
    });

    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) {
            leadCapturePopup.hide();
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window").hide();
        }
    });
});


function CustomerModel() {
    var arr = setuserDetails();
    var self = this;
    if (arr != null && arr.length > 0) {
        self.fullName = ko.observable(arr[0]);
        self.emailId = ko.observable(arr[1]);
        self.mobileNo = ko.observable(arr[2]);
    }
    else {
        self.fullName = ko.observable();
        self.emailId = ko.observable();
        self.mobileNo = ko.observable();
    }
    self.IsVerified = ko.observable(false);
    self.NoOfAttempts = ko.observable(0);
    self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
    self.otpCode = ko.observable();
    self.isAssist = ko.observable(false);

    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": dealerId,
                "pqId": "21643185",  //to be removed
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "clientIP": "",
                "pageUrl": "",
                "versionId": 165,
                "cityId": 1,
                "leadSourceId": 1,
                "deviceId": getCookie('BWC')
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                data: ko.toJSON(objCust),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('__utmz'));
                },
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                    if (!self.IsVerified()) {
                        self.NoOfAttempts(obj.noOfAttempts);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };


    self.generateOTP = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "pqId": pqId,
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "cwiCode": self.otpCode(),
                "branchId": dealerId,
                "customerName": self.fullName(),
                "versionId": versionId,
                "cityId": cityId
            }
            $.ajax({
                type: "POST",
                url: "/api/PQMobileVerification/",
                data: ko.toJSON(objCust),
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var obj = ko.toJS(response);
                    self.IsVerified(obj.isSuccess);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };

    self.regenerateOTP = function () {
        if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
            var url = '/api/ResendVerificationCode/';
            var objCustomer = {
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "source": 1
            }
            $.ajax({
                type: "POST",
                url: url,
                async: false,
                data: ko.toJSON(objCustomer),
                contentType: "application/json",
                success: function (response) {
                    self.IsVerified(false);
                    self.NoOfAttempts(response.noOfAttempts);
                    alert("You will receive the new OTP via SMS shortly.");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    self.IsVerified(false);
                }
            });
        }
    };

    self.submitLead = function () {
        $("#dealer-lead-msg").hide();
        self.IsVerified(false);
        isValidDetails = false;
        if (event.currentTarget.id == 'buyingAssistBtn') {
            self.isAssist(true);
            isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
        }
        else {
            self.isAssist(false);
            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
        }
        if (isValidDetails) {
            self.verifyCustomer();
            if (self.IsValid()) {
                if (self.isAssist()) {
                    $("#buying-assistance-form").hide();
                    $("#dealer-assist-msg").fadeIn();

                } else {
                    $("#contactDetailsPopup").hide();
                    $("#personalInfo").hide()
                    $("#otpPopup").hide();
                    $("#dealer-lead-msg").fadeIn();
                }
            }
            else {
                $("#leadCapturePopup").show();
                $('body').addClass('lock-browser-scroll');
                $(".blackOut-window").show();
                $("#contactDetailsPopup").hide();
                $("#otpPopup").show();
                var leadMobileVal = mobile.val();
                $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                otpContainer.removeClass("hide").addClass("show");
                //nameValTrue();
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Lead_Submitted', 'lab': bikeName + '_' + getCityArea });
        }
    };

    otpBtn.click(function () {
        $('#processing').show();
        isValidDetails = false;
        if (!validateOTP())
            $('#processing').hide();

        if (event.currentTarget.id == 'buyingAssistBtn') {
            self.isAssist(true);
            isValidDetails = validateUserInfo(assistanceGetName, assistanceGetEmail, assistanceGetMobile);
        }
        else {
            self.isAssist(false);
            isValidDetails = ValidateUserDetail(fullName, emailid, mobile);
        }

        if (validateOTP() && isValidDetails) {
            customerViewModel.generateOTP();
            if (customerViewModel.IsVerified()) {
                $("#personalInfo").hide();
                $(".booking-dealer-details").removeClass("hide").addClass("show");
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                $("#personalInfo").hide()
                $("#otpPopup").hide();

                if (self.isAssist()) {
                    $("#leadCapturePopup .leadCapture-close-btn").click();
                    $("#buying-assistance-form").hide();
                    $("#dealer-assist-msg").fadeIn();
                }
                else {
                    $("#dealer-lead-msg").fadeIn();
                }

                // OTP Success
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
            }
            else {
                $('#processing').hide();
                otpVal("Please enter a valid OTP.");
                // push OTP invalid
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
            }
        }
    });
}

function ValidateUserDetail(fullName, emailid, mobile) {
    return validateUserInfo(fullName, emailid, mobile);
};


var prevEmail = "",
	prevMobile = "";

$("#assistanceGetName,#getFullName").on("focus", function () {
    hideError($(this));
});

$("#assistanceGetEmail,#getEmailID").on("focus", function () {
    hideError($(this));
    prevEmail = $(this).val().trim();
});

$("#assistanceGetMobile,#getMobile,#getUpdatedMobile").on("focus", function () {
    hideError($(this));
    prevMobile = $(this).val().trim();
});



$("#assistanceGetEmail,#getEmailID").on("blur", function () {
    if (prevEmail != $(this).val().trim()) {
        if (validateEmailId($(this))) {
            customerViewModel.IsVerified(false);
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError($(this));
        }
    }
});

$("#assistanceGetMobile,#getMobile,#getUpdatedMobile").on("blur", function () {
    if (prevMobile != $(this).val().trim()) {
        if (validateMobileNo($(this))) {
            customerViewModel.IsVerified(false);
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError($(this));
        }
    }
});

function mobileValTrue() {
    mobile.removeClass("border-red");
    mobile.siblings("span, div").hide();
};


otpText.on("focus", function () {
    otpText.val('');
    otpText.siblings("span, div").hide();
});

var setError = function (element, msg) {
    element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
    element.siblings("div.errorText").text(msg);
};

var hideError = function (element) {
    element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
};
var otpVal = function (msg) {
    otpText.addClass("border-red");
    otpText.siblings("span, div").show();
    otpText.siblings("div").text(msg);
};

function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = otpText.val();
    customerViewModel.IsVerified(false);
    if (cwiCode == "") {
        retVal = false;
        otpVal("Please enter your Verification Code");
        bindInsuranceText();
    }
    else {
        if (isNaN(cwiCode)) {
            retVal = false;
            otpVal("Verification Code should be numeric");
        }
        else if (cwiCode.length != 5) {
            retVal = false;
            otpVal("Verification Code should be of 5 digits");
        }
    }
    return retVal;
}

function setuserDetails() {
    var cookieName = "_PQUser";
    if (isCookieExists(cookieName)) {
        var arr = getCookie(cookieName).split("&");
        return arr;
    }
}

function setPQUserCookie() {
    var val = customerViewModel.fullName() + '&' + customerViewModel.emailId() + '&' + customerViewModel.mobileNo();
    SetCookie("_PQUser", val);
}

$(".edit-mobile-btn").on("click", function () {
    var prevMobile = $(this).prev("span.lead-mobile").text();
    $(".lead-otp-box-container").hide();
    $(".update-mobile-box").show();
    $("#getUpdatedMobile").val(prevMobile).focus();
});

$("#generateNewOTP").on("click", function () {
    if (validateMobileNo($("#getUpdatedMobile"))) {
        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        $(".update-mobile-box").hide();
        $(".lead-otp-box-container").show();
        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
    }
});


var variantsDropdown = $(".variants-dropdown"),
variantSelectionTab = $(".variant-selection-tab"),
variantUL = $(".variants-dropdown-list"),
variantListLI = $(".variants-dropdown-list li");

variantsDropdown.click(function (e) {
    if (!variantsDropdown.hasClass("open"))
        $.variantChangeDown(variantsDropdown);
    else
        $.variantChangeUp(variantsDropdown);
});

$.variantChangeDown = function (variantsDropdown) {
    variantsDropdown.addClass("open");
    variantUL.show();
};

$.variantChangeUp = function (variantsDropdown) {
    variantsDropdown.removeClass("open");
    variantUL.slideUp();
};


$(document).mouseup(function (e) {
    if (!$(".variants-dropdown, .variant-selection-tab, .variant-selection-tab #upDownArrow").is(e.target)) {
        $.variantChangeUp($(".variants-dropdown"));
    }
});

var assistanceGetName = $('#assistanceGetName'),
    assistanceGetEmail = $('#assistanceGetEmail'),
    assistanceGetMobile = $('#assistanceGetMobile');


var validateUserInfo = function (leadUsername, leadEmailId, leadMobileNo) {
    var isValid = true;
    isValid = validateUserName(leadUsername);
    isValid &= validateEmailId(leadEmailId);
    isValid &= validateMobileNo(leadMobileNo);
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
		reMobile = /^[1-9][0-9]{9}$/;
    if (mobileVal == "") {
        setError(leadMobileNo, "Please enter your mobile no.");
        isValid = false;
    }
    else if (mobileVal[0] == "0") {
        setError(leadMobileNo, "Mobile no. should not start with zero");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(leadMobileNo, "Mobile no. should be 10 digits only");
        isValid = false;
    }
    else
        hideError(leadMobileNo)
    return isValid;
};

var DealerDetails = function()
{
    var self = this;
    self.name = ko.observable();
    self.mobile = ko.observable();
    self.address = ko.observable();
    self.city = ko.observable();
    self.area = ko.observable();
    self.workingHours = ko.observable();
    self.email = ko.observable();
    self.dealerType = ko.observable();
}

var DealerBikesList  = function()
{
    var self = this;
    self.bikeName = ko.observable();
    self.bikePrice = ko.observable();
    self.minSpecs = ko.observable();
    self.displacement = ko.observable();
    self.fuelEffecient = ko.observable();
    self.maxpower = ko.observable();
    self.displayMinSpec = ko.computed(function () {
        var spec = '';
        if (self.displacement() && self.displacement() != "0")
            str += "<span><span>"+ self.displacement() +"</span><span class='text-light-grey'> CC</span></span>";

        if (self.fuelEffecient() && self.fuelEffecient() != "0")
            str += "<span>, <span>" + self.fuelEffecient() + "</span><span class='text-light-grey'> Kmpl</span></span>";

        if (self.maxpower() && self.maxpower() != "0")
            str += "<span>, <span>" + self.maxpower() + "</span><span class='text-light-grey'> bhp</span></span>";

        if (spec != "")
            return spec;
        else
            return "Specs Unavailable";
    }, this);

    


}

var DealerModel = function() {
    var self = this;
    self.address1 = ko.observable(address1);
    self.address2 = ko.observable(address2);
    self.area = ko.observable(area);
    self.city = ko.observable(city);
    self.contactHours = ko.observable(contactHours);
    self.emailId = ko.observable(emailId);
    self.faxNo = ko.observable(faxNo);
    self.firstName = ko.observable(firstName);
    self.id = ko.observable(id);
    self.lastName = ko.observable(lastName);
    self.lattitude = ko.observable(lattitude);
    self.longitude = ko.observable(longitude);
    self.mobileNo = ko.observable(mobileNo);
    self.organization = ko.observable(organization);
    self.phoneNo = ko.observable(phoneNo);
    self.pincode = ko.observable(pincode);
    self.state = ko.observable(state);
    self.websiteUrl = ko.observable(websiteUrl);                
}


ko.applyBindings(customerViewModel, $('#dealerDetailsSliderCard')[0]);

ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);


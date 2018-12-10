var GAEvent = "locate_dealer_section";
var category = "price_quote_page";

function CheckCheckboxes(category, buttonDiv) {

    var comment = "";
    var comments = "";
    if (category == "price_quote_page") {

        var checkboxes = $(buttonDiv).find(".checkboxes li");
        checkboxes.each(function () {
            if ($(this).hasClass("checked")) {
                comment = $.trim($(this).text());
                comments += comment + ",";
            }
        });
    }

    if (category == "dealer_microsite_desktop") {
        $('#checkboxes li').each(function () {
            if ($(this).hasClass("checked")) {
                comment = $.trim($(this).text());
                comments += comment + ",";
            }
        });
    }

    return comments.slice(0, -1);
}

function SendNewCarRequestDealer(dealerInquiryDetails, targetFormDiv, emailSubmitted) {
    $('#btnSubmitDealer').val("Processing...");
    var comments = CheckCheckboxes(category, targetFormDiv);
    var requestType = 1;
    dealerInquiryDetails.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
    dealerInquiryDetails.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
    dealerInquiryDetails.Comments = comments.replace("Assistance", "");
    dealerInquiryDetails.PlatformSourceId = "1";
    dealerInquiryDetails.BuyTimeText = "1 week";
    dealerInquiryDetails.BuyTimeValue = 7;
    dealerInquiryDetails.RequestType = 1;
    dealerInquiryDetails.EncryptedPQDealerAdLeadId = $(targetFormDiv).find('#encryptedResponse').val();
    dealerInquiryDetails.ModelsHistory = userHistory.getUserModelHistory();
    dealerInquiryDetails.SponsoredBannerCookie = isCookieExists('_sb' + dealerInquiryDetails.modelId) ? $.cookie('_sb' + dealerInquiryDetails.modelId) : '';

    showThankYou(targetFormDiv);

    $.ajax({
        type: "POST",
        url: "/webapi/DealerSponsoredAd/PostDealerInquiry/",
        data: dealerInquiryDetails,
        success: function (response) {
            if (!emailSubmitted) {
                $(targetFormDiv).find('#encryptedResponse').val(response);
                if (dealerInquiryDetails.cwtccat && dealerInquiryDetails.cwtcact && dealerInquiryDetails.cwtclbl)
                    cwTracking.trackCustomData(dealerInquiryDetails.cwtccat, dealerInquiryDetails.cwtcact, dealerInquiryDetails.cwtclbl, false);
                if (typeof dealerInquiryDetails.score != 'undefined' && dealerInquiryDetails.score >= -100 && typeof dealerInquiryDetails.label != 'undefined' && dealerInquiryDetails.label != '') {
                    cwTracking.trackCustomData('PredictionFeedback', '2', dealerInquiryDetails.label, false);
                }
                
            } else {
                $(targetFormDiv).find('#encryptedResponse').val("");
                dealerShowroom.thankYouScreen.hideScreen(targetFormDiv);
            }
            $('#btnSubmitDealer').val("Submit");
            if (typeof dealerInquiryDetails.EncryptedPQDealerAdLeadId == "undefined" || dealerInquiryDetails.EncryptedPQDealerAdLeadId == "") {
                leadConversionTracking.track(dealerInquiryDetails.LeadClickSource, dealerInquiryDetails.DealerId);
            }
        }
    });
    dataLayer.push({ event: 'locate_dealer_section', cat: category, act: 'submit_success_checkboxes' });
}

$(document).ready(function () {
    dealershowroomTabs();
    prefillFormfields();
    $('.select-services li').click(function () {
        $(this).toggleClass('checked');
    });
});

function initialize(latitude, longitude, dealerName, address, dealerMobileNo) {
    try {
        var myLatlng = new google.maps.LatLng(latitude, longitude);

        var mapOptions = {
            zoom: 16,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

        var contentString = '<div id="content" style="width:200px;">' +
          '<strong>' + dealerName + '</strong>' + '<br/>' + address + '<br/>' + '<strong>' + dealerMobileNo + '</strong>' + '<br/>'
          + '<a onClick="javascript:onViewMapTrack()" id="googleMap" style="text-decorations:none; color:inherit;" target="_blank">View on Google Map' + '</a>'
        '</div>';

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,

        });
        marker.setMap(map);

        google.maps.event.addListener(marker, 'click', function () {
            infowindow.open(map, marker);
        });

    }
    catch (e) {
        console.debug(e);
    }
}

//To track number of clicks on View on Google Map Link
function onViewMapTrack() {
    dataLayer.push({ event: 'locate_dealer_section', cat: category, act: "View_On_Google_Map_Click", lab: dealerName });
    $('#googleMap').attr('href', "https://maps.google.com/maps?z=12&t=m&q=loc:" + latitude + '+' + longitude);
}

function popUpClose() {
    $('.popup-close-btn, .back-btn').click(function () {
        $('#blackOut-window, .thank-you-popup').hide();
        $('ul li').each(function () {
            if ($(this).hasClass("checked")) {
                $(this).removeClass("checked");
            }
        });
    });
}

function showLoadingImage() {
    $('.blackOut-window').show();
    $('#loadingCarImg').show();
}

function hideLoadingImage() {
    $('.blackOut-window').hide();
    $('#loadingCarImg').hide();
}

function shakeForm(domElement) {
    domElement.parent('.form-control-box').addClass("shake-form").delay(1000).queue(function (next) {
        $(this).removeClass("shake-form");
        next();
    });
}
function validateCustName(targetFormDiv) {
    var custName = $.trim($(targetFormDiv).find(' #custName').val());
    var reName = /^([-a-zA-Z ']*)$/;


    if (custName == "" || custName == "Enter your name" || custName == "Name") {
        ShakeFormView($(".baNameField"));
        $(targetFormDiv).find(' .err-name-icon').show();
        $(targetFormDiv).find(' .err-name-msg').show();
        $("#nameIcon").hide();
        $(targetFormDiv).find(' .err-name-msg').html('Please enter your name');
        dataLayer.push({ event: GAEvent, cat: category, act: 'errNameNotFilled' });
        return false;

    } else if (reName.test(custName) == false) {
        ShakeFormView($(".baNameField"));
        $(targetFormDiv).find(' .err-name-icon').show();
        $(targetFormDiv).find(' .err-name-msg').show();
        $("#nameIcon").hide();
        $(targetFormDiv).find(' .err-name-msg').html('Please enter only alphabets');
        dataLayer.push({ event: GAEvent, cat: category, act: 'errNonAlphabetName' });
        return false;
    } else if (name.length == 1) {
        ShakeFormView($(".baNameField"));
        $(targetFormDiv).find(' .err-name-icon').show();
        $(targetFormDiv).find(' .err-name-msg').show();
        $("#nameIcon").hide();
        $(targetFormDiv).find(' .err-name-msg').html('Please enter your complete name');
        dataLayer.push({ event: GAEvent, cat: category, act: 'errOneCharName' });
        return false;
    } else {
        $(targetFormDiv).find(' .err-name-msg').hide();
        $(targetFormDiv).find(' .err-name-icon').hide();
        $("#nameIcon").show();
    }
    return true;
}

function validateCustMobile(targetFormDiv) {

    var custMobile = $.trim($(targetFormDiv).find(' #custMobile').val());
    var reMobile = /^[6789]\d{9}$/;
    var _custMobile = custMobile;

    function shakeMobileForm() {
        $("#custMobile").parent('.mob-no-box').addClass("shake-form").delay(1000).queue(function (next) {
            $(this).removeClass("shake-form");
            next();
        });
    }
    if (_custMobile == "" || _custMobile == "Enter your mobile number") {
        ShakeFormView($(".baMobileField"));
        $(targetFormDiv).find(' .err-mobile-icon').show();
        $(targetFormDiv).find(' .err-mobile-msg').show();
        $("#mobileIcon").hide();
        $(targetFormDiv).find(' .err-mobile-msg').html('Please enter your mobile number');
        return false;

    } else if (_custMobile.length != 10) {
        ShakeFormView($(".baMobileField"));
        $(targetFormDiv).find(' .err-mobile-icon').show();
        $(targetFormDiv).find(' .err-mobile-msg').show();
        $("#mobileIcon").hide();
        $(targetFormDiv).find(' .err-mobile-msg').html('Mobile number should be of 10 digits');
        dataLayer.push({ event: GAEvent, cat: category, act: 'errMobileNot10Digits' });
        return false;

    } else if (!reMobile.test(_custMobile)) {
        ShakeFormView($(".baMobileField"));
        $(targetFormDiv).find(' .err-mobile-icon').show();
        $(targetFormDiv).find(' .err-mobile-msg').show();
        $("#mobileIcon").hide();
        $(targetFormDiv).find(' .err-mobile-msg').html('Please provide a valid 10 digit Mobile number');
        dataLayer.push({ event: GAEvent, cat: category, act: 'errMobileNotValid' });
        return false;

    } else {
        $(targetFormDiv).find(' .err-mobile-msg').hide();
        $(targetFormDiv).find(' .err-mobile-icon').hide();
        $("#mobileIcon").show();
    }
    return true;
}

function validateCustEmail(targetFormDiv) {
    var custEmail = getCustEmailValue(targetFormDiv);

    var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
    var _email = custEmail.toString().toLowerCase();

    if ($(targetFormDiv).find(' #custEmail').is(":visible") || $(targetFormDiv).find(' #custEmailOptional').is(":visible")) {
        if (_email == "" || _email == "Enter your e-mail id") {
            $(targetFormDiv).find(' .err-email-icon').show();
            $(targetFormDiv).find(' .err-email-msg').show();
            $("#emailIcon").hide();
            $(targetFormDiv).find(' .err-email-msg').html('Please enter your email');
            dataLayer.push({ event: GAEvent, cat: category, act: 'errEmailNotFilled' });
            return false;
        } else if (!reEmail.test(_email)) {
            $(targetFormDiv).find(' .err-email-icon').show();
            $(targetFormDiv).find(' .err-email-msg').show();
            $("#emailIcon").hide();
            $(targetFormDiv).find(' .err-email-msg').html('Invalid Email');
            dataLayer.push({ event: GAEvent, cat: category, act: 'errEmailNotValid' });
            return false;
        } else {
            $(targetFormDiv).find(' .err-email-msg').hide();
            $(targetFormDiv).find(' .err-email-icon').hide();
            $("#emailIcon").show();
        }
    }
    return true;
}

function isValid(targetFormDiv) {

    var retVal = true;
    if (!validateCustName(targetFormDiv))
        retVal = false;

    if (!validateCustMobile(targetFormDiv))
        retVal = false;

    if (!validateCustEmail(targetFormDiv))
        retVal = false;

    return retVal;
}

function getCustEmailValue(targetFormDiv) {
    if ($(targetFormDiv).find(' #custEmailOptional').is(":visible"))
        return $.trim($(targetFormDiv).find(' #custEmailOptional').val());
    else
        return $.trim($(targetFormDiv).find(' #custEmail').val());

}

function onBlurValidation(targetFormDiv) {
    $(targetFormDiv).find(' .custName').on('blur', function () {
        validateCustName(targetFormDiv);
    });
    $(targetFormDiv).find(' .custMobile').on('blur', function () {
        validateCustMobile(targetFormDiv);
    });
    $(targetFormDiv).find(' .custEmail').on('blur', function () {
        validateCustEmail(targetFormDiv);
    });
    $(targetFormDiv).find(' #custEmailOptional').on('blur', function () {
        if (($(targetFormDiv).find(' #custEmailOptional').val()).length > 0) {
            validateCustEmail(targetFormDiv);
        }
    });
}

function showThankYou(targetFormDiv) {
    $(targetFormDiv).find('.thank-you-msg').show();
    $(targetFormDiv).find('.tyHeading').append('Thank You ' + $(targetFormDiv).find(' #custName').val()).show();
    $(targetFormDiv).find('.contactdetails').hide();
    $(targetFormDiv).find('.selectservices').hide();
}

function trackingCode() {
    // tracking code
    dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_desktop', act: 'page_load', lab: dealerName });
    dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_desktop', act: 'buying_assist_tab', lab: dealerName });

    //for colorbox pop up window code starts here
    $("a[rel='slide']").colorbox({ width: "600px", height: "450px" }); // end here

    $('#authorCarousel li div').first().addClass('dealer-carousel-pic-active');

    $('.cboxElement').click(function () {
        dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_desktop', act: 'image_popup', lab: dealerName });
    });

    $('#cboxNext').click(function () {
        dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_desktop', act: 'image_popup_next', lab: dealerName });
    });

    $('#cboxPrevious').click(function () {
        dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_desktop', act: 'image_popup_previous', lab: dealerName });
    });
}

function modelCarousel() {
    // carousel controls starts here
    modelsCarousel = $('#authorCarouselContainer').jcarousel({
        scroll: 1,
        auto: 0,
        animation: 800,
        wrap: "circular",
        initCallback: null, buttonNextHTML: null, buttonPrevHTML: null
    });

    $('#list_carousel_widget_prev').bind('click', function () {
        modelsCarousel.jcarousel("scroll", "-=1");
    });

    $('#list_carousel_widget_next').bind('click', function () {
        modelsCarousel.jcarousel("scroll", "+=1");
    });

    $("#authorCarousel li .dealer-carousel-pic").first().addClass("dealer-carousel-pic-active");

    $(".list_carousel_widget li a").click(function (evt) {
        var imgText;
        imgText = $(this).parent().siblings().text();
        $('.active-img-text').html(imgText);
        modelName = $.trim(imgText);
        modelId = $(this).parent().siblings().attr('modelid');
        evt.preventDefault();
        $(".highlighted-image").empty().append($("<img>", { src: this.href }));
        dataLayer.push({ event: 'locate_dealer_section', cat: 'dealer_microsite_desktop', act: 'model_image_selected', lab: modelName });
    });

    $('.list_carousel_widget li .dealer-carousel-pic').click(function () {
        $('.list_carousel_widget li .dealer-carousel-pic').removeClass('dealer-carousel-pic-active');
        $(this).addClass('dealer-carousel-pic-active');

    });
}

function dealershowroomTabs() {
    $('.dealer-tabs ul li').click(function () {
        console.log("dealerName:" + dealerName);
        action = $(this).attr('data-id') + "_tab";
        dataLayer.push({ event: 'locate_dealer_section', cat: category, act: action, lab: dealerName });
    });

    //dealer tabs starts here
    $(".dealer-tabs li").click(function () {
        var panel = $(this).closest(".panel-group");
        panel.find(".dealer-tabs li").removeClass("active");
        $(this).addClass("active");

        var panelId = $(this).attr("data-id");
        panel.find(".dealer-data").hide();
        $("#" + panelId).show();

        if (panelId == 'contactdetails' && isLatLongValid(latitude, longitude)) {
            $(".map-hide").show();
            if (!isMapInitialized) {
                window.setTimeout(function () { initialize(latitude, longitude, dealerName, address, dealerMobileNo) }, 1000);
                isMapInitialized = true;
            }
        }
        else {
            $(".map-hide").hide();
        }
    });
}

function setCookies(targetFormDiv) {
    if ($.trim($(targetFormDiv).find('.customeremail').val()) != "") {
        document.cookie = '_CustEmail=' + $.trim($(targetFormDiv).find('.customeremail').val()) + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    }
    document.cookie = '_CustomerName=' + $.trim($(targetFormDiv).find('#custName').val()) + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustMobile=' + $.trim($(targetFormDiv).find('#custMobile').val()) + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
}

function prefillFormfields() {
    if ($.cookie('_CustomerName') != null && $.cookie('_CustEmail') != null && $.cookie('_CustMobile') != null) {
        $('#custName').val($.cookie('_CustomerName'));
        $('#custEmailOptional').val($.cookie('_CustEmail'));
        $('#custMobile').val($.cookie('_CustMobile'));
    }
}

var dealerShowroom = {
    CITYID: "",
    CITYNAME: "",
    thankYouScreen: {
        hideScreen: function (targetFormDiv) {
            targetFormDiv = $(targetFormDiv).closest(".dealerlocator");
            var self = this;
            self.targetFormDiv = $(targetFormDiv);
            self.targetFormDiv.find('.thank-you-msg').hide();
            self.targetFormDiv.find('.tyHeading').hide();
            self.targetFormDiv.find('.contactdetails').show();
            self.targetFormDiv.find('.selectservices').show();
        }
    },
};

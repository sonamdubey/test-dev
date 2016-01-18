// JavaScript Document
var imgTitle, imgTotalCount;
var leadBtnBookNow = $("#leadBtnBookNow"), leadCapturePopup = $("#leadCapturePopup");
var fullname = $("#getFullName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");

var detailsSubmitBtn = $("#user-details-submit-btn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");

var prevEmail = "";
var prevMobile = "";

var getCityArea = GetGlobalCityArea();
var customerViewModel = new CustomerModel();

$(function () {

    leadBtnBookNow.on('click', function () {
        leadCapturePopup.show();
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window").show();

        $(".leadCapture-close-btn, .blackOut-window").on("click", function () {
            leadCapturePopup.hide();
            $('body').removeClass('lock-browser-scroll');
            $(".blackOut-window").hide();
        });

        $(document).on('keydown', function (e) {
            if (e.keyCode === 27) {
                $("#leadCapturePopup .leadCapture-close-btn").click();
            }
        });

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
    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": dealerId,
                "pqId": pqId,
                "customerName": self.fullName(),
                "customerMobile": self.mobileNo(),
                "customerEmail": self.emailId(),
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": versionId,
                "cityId": cityId,
                "leadSourceId": 6,
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
                "source": 2
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
        if (ValidateUserDetail()) {
            self.verifyCustomer();
            if (self.IsValid()) {
                var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                window.location.href = "/m/pricequote/BikeDealerDetails.aspx?MPQ=" + Base64.encode(cookieValue);
                //$("#personalInfo").hide();
                //$("#leadCapturePopup .leadCapture-close-btn").click();                
            }
            else {
                $("#contactDetailsPopup").hide();
                $("#otpPopup").show();
                var leadMobileVal = mobile.val();
                $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                otpContainer.removeClass("hide").addClass("show");
                //detailsSubmitBtn.hide();
                nameValTrue();
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Lead_Submitted', 'lab': bikeVersionLocation });
        }

    };

    otpBtn.click(function () {
        $('#processing').show();
        if (!validateOTP())
            $('#processing').hide();

        if (validateOTP() && ValidateUserDetail()) {
            customerViewModel.generateOTP();
            if (customerViewModel.IsVerified()) {
                // $.customizeState();
                $("#personalInfo").hide();
                $(".booking-dealer-details").removeClass("hide").addClass("show");
                $('#processing').hide();

                detailsSubmitBtn.show();
                otpText.val('');
                otpContainer.removeClass("show").addClass("hide");
                var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                window.location.href = "/m/pricequote/BikeDealerDetails.aspx?MPQ=" + Base64.encode(cookieValue);                
            }
            else {
                $('#processing').hide();
                otpVal("Please enter a valid OTP");
            }
        }
    });

}

function ValidateUserDetail() {
    var isValid = true;
    isValid = validateEmail();
    isValid &= validateMobile();
    isValid &= validateName();
    return isValid;
};


function validateName() {
    var isValid = true;
    var a = fullname.val().length;
    if ((/&/).test(fullname.val())) {
        isValid = false;
        setError(fullname, 'Invalid name');
    }
    else if (a == 0) {
        isValid = false;
        setError(fullname, 'Please enter your name');
    }
    else if (a >= 1) {
        isValid = true;
        nameValTrue()
    }
    return isValid;
}

function nameValTrue() {
    hideError(fullname)
    fullname.siblings("div").text('');
};

fullname.on("focus", function () {
    hideError(fullname);
});

emailid.on("focus", function () {
    hideError(emailid);
    prevEmail = emailid.val().trim();
});

mobile.on("focus", function () {
    hideError(mobile)
    prevMobile = mobile.val().trim();

});

emailid.on("blur", function () {
    if (prevEmail != emailid.val().trim()) {
        if (validateEmail()) {
            customerViewModel.IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(emailid);
        }
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
    }
});

mobile.on("blur", function () {
    if (mobile.val().length < 10) {
        $("#user-details-submit-btn").show();
        $(".mobile-verification-container").removeClass("show").addClass("hide");
    }
    if (prevMobile != mobile.val().trim()) {
        if (validateMobile(getCityArea)) {
            customerViewModel.IsVerified(false);
            detailsSubmitBtn.show();
            otpText.val('');
            otpContainer.removeClass("show").addClass("hide");
            hideError(mobile);
        }
        $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
        $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
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

function setError(ele, msg) {
    ele.addClass("border-red");
    ele.siblings("span, div").show();
    ele.siblings("div").text(msg);
}

function hideError(ele) {
    ele.removeClass("border-red");
    ele.siblings("span, div").hide();
}
/* Email validation */
function validateEmail() {
    var isValid = true;
    var emailID = emailid.val();
    var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailID == "") {
        setError(emailid, 'Please enter email address');
        isValid = false;
    }
    else if (!reEmail.test(emailID)) {
        setError(emailid, 'Invalid Email');
        isValid = false;
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Email', 'lab': getCityArea }); }
    return isValid;
}

function validateMobile() {
    var isValid = true;
    var reMobile = /^[0-9]{10}$/;
    var mobileNo = mobile.val();
    if (mobileNo == "") {
        isValid = false;
        setError(mobile, "Please enter your Mobile Number");
    }
    else if (!reMobile.test(mobileNo) && isValid) {
        isValid = false;
        setError(mobile, "Mobile Number should be 10 digits");
    }
    else {
        hideError(mobile)
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': getCityArea }); }
    return isValid;
}

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
    var val = fullname.val() + '&' + emailid.val() + '&' + mobile.val();
    SetCookie("_PQUser", val);
}

$("#otpPopup .edit-mobile-btn").on("click", function () {
    var prevMobile = $(this).prev("span.lead-mobile").text();
    $(".lead-otp-box-container").hide();
    $(".update-mobile-box").show();
    $("#getUpdatedMobile").val(prevMobile).focus();
});

$("#generateNewOTP").on("click", function () {
    if (validateUpdatedMobile()) {
        var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
        $(".update-mobile-box").hide();
        $(".lead-otp-box-container").show();
        $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
    }
});

var validateUpdatedMobile = function () {
    var isValid = true,
		mobileNo = $("#getUpdatedMobile"),
		mobileVal = mobileNo.val(),
		reMobile = /^[0-9]{10}$/;
    if (mobileVal == "") {
        setError(mobileNo, "Please enter your Mobile Number");
        isValid = false;
    }
    else if (!reMobile.test(mobileVal) && isValid) {
        setError(mobileNo, "Mobile Number should be 10 digits");
        isValid = false;
    }
    else
        hideError(mobileNo)
    return isValid;
};



//photos corousel function
var slideToClick = function (swiper) {
    var clickedSlide = swiper.slides[swiper.clickedIndex];
    $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
    $(clickedSlide).addClass('swiper-slide-active');
    galleryTop.slideTo(swiper.clickedIndex, 500);
};

var galleryThumbs = new Swiper('.carousel-navigation-photos', {
    slideActiveClass: '',
    spaceBetween: 10,
    centeredSlides: true,
    slidesPerView: 'auto',
    slideToClickedSlide: true,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    onTap: slideToClick
});

var slidegalleryThumbs = function (swiper) {
    galleryThumbs.slideTo(swiper.activeIndex, 500);
    galleryThumbs.slides.removeClass('swiper-slide-active');
    galleryThumbs.slides[swiper.activeIndex].className += ' swiper-slide-active';

    showImgTitle(galleryTop);
};

var galleryTop = new Swiper('.carousel-stage-photos', {
    nextButton: '.swiper-button-next',
    prevButton: '.swiper-button-prev',
    spaceBetween: 10,
    preloadImages: false,
    lazyLoading: true,
    lazyLoadingInPrevNext: true,
    watchSlidesProgress: true,
    watchSlidesVisibility: true,
    //onSlideChangeStart: showImgTitle,
    onSlideChangeEnd: slidegalleryThumbs
});

$("#bikeBannerImageCarousel .stage .swiper-slide").click(function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Photo_Clicked', 'lab': myBikeName });
    if (galleryTop.slides.length > 0) {
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window-model").show();
        $(".bike-gallery-popup").removeClass("hide").addClass("show");
        $(".modelgallery-close-btn").removeClass("hide").addClass("show");

        galleryTop.onResize();
        galleryThumbs.onResize();
        galleryTop.slideTo($(this).index(), 500);
        galleryThumbs.slideTo($(this).index(), 500);
        showImgTitle(galleryTop);

    }
});

$(".modelgallery-close-btn").click(function () {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window-model").hide();
    $(".bike-gallery-popup").removeClass("show").addClass("hide");
    $(".modelgallery-close-btn").removeClass("show").addClass("hide");
    videoiFrame.setAttribute("src", "");
    $('.sw-0').data('swiper').slideTo(galleryTop.activeIndex, 500);
});


function showImgTitle(swiper) {
    imgTitle = $(galleryThumbs.slides[swiper.activeIndex]).find('img').attr('title');
    //console.log(imgTitle);
    imgTotalCount = galleryThumbs.slides.length;
    $(".leftfloatbike-gallery-details").text(imgTitle);
    $(".bike-gallery-count").text(swiper.activeIndex + 1 + "/" + imgTotalCount.toString());
}

var videoiFrame = document.getElementById("video-iframe");

/* first video src */
$("#photos-tab, #videos-tab").click(function () {
    firstVideo();
});

$("#videos-tab").click(function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Video_Tab_Clicked', 'lab': myBikeName });
});

var firstVideo = function () {
    var a = $(".carousel-navigation-videos ul").first("li");
    var newSrc = a.find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
};

var navigationVideosLI = $(".carousel-navigation-videos ul li");
navigationVideosLI.click(function () {
    navigationVideosLI.removeClass("active");
    $(this).addClass("active");
    var newSrc = $(this).find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
});

//}
//        bindInsuranceText();
//    a.attr("href", "#features");
//    else a.attr("href", "javascript:void(0)");


function LoadTerms(offerId) {

    $(".termsPopUpContainer").css('height', '150')
    $('#termspinner').show();
    $('#terms').empty();
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();

    var url = abHostUrl + "/api/DealerPriceQuote/GetOfferTerms?offerMaskingName=&offerId=" + offerId;
    if (offerId != '' && offerId != null) {
        $.ajax({
            type: "GET",
            url: abHostUrl + "/api/DealerPriceQuote/GetOfferTerms?offerMaskingName=&offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $(".termsPopUpContainer").css('height', '500')
                $('#termspinner').hide();
                if (response.html != null)
                    $('#terms').html(response.html);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });
    }
    else {
        setTimeout(LoadTerms, 2000); // check again in a second
    }
}


$("#btnShowOffers").on("click", function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Show_Offers_Clicked', 'lab': myBikeName });
});

$(".viewMoreOffersBtn").on("click", function () {
    $(this).hide();
    $("ul.moreOffersList").slideToggle();
});

var bodHt, footerHt, scrollPosition;
$(window).scroll(function () {
    bodHt = $('body').height();
    footerHt = $('footer').height();
    scrollPosition = $(this).scrollTop();
    if (scrollPosition + $(window).height() > (bodHt - footerHt))
        $('.float-button').removeClass('float-fixed');
    else
        $('.float-button').addClass('float-fixed');
});

var sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortCriteria = $('#sort'),
    sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortListLI = $(".sort-selection-div ul li");

sortByDiv.click(function () {
    if (!sortByDiv.hasClass("open"))
        $.sortChangeDown(sortByDiv);
    else
        $.sortChangeUp(sortByDiv);
});

$.sortChangeDown = function (sortByDiv) {
    sortByDiv.addClass("open");
    sortListDiv.show();
};

$.sortChangeUp = function (sortByDiv) {
    sortByDiv.removeClass("open");
    sortListDiv.slideUp();
};
$("input[name*='btnVariant']").on("click", function () {
    if ($(this).attr('versionid') == $('#hdnVariant').val()) {
        return false;
    }
    $('#hdnVariant').val($(this).attr('title'));
    var bikeVersion = myBikeName + '_' + $(this).val();
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersion });
});

$("#viewBreakupText").on('click', function (e) {
    $("div#breakupPopUpContainer").show();
    $(".blackOut-window").show();
    appendHash("viewBreakup");
});
$(".breakupCloseBtn,.blackOut-window").on('click', function (e) {
    viewBreakUpClosePopup();
    window.history.back();
});

var viewBreakUpClosePopup = function () {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
    $("#contactDetailsPopup").show();
    $("#otpPopup").hide();
    leadPopupClose();
};

$(".termsPopUpCloseBtn").on('mouseup click', function (e) {
    $("div#termsPopUpContainer").hide();
    $(".blackOut-window").hide();
});

$("#getMoreDetailsBtn").on('click', function (e) {
    $("div#leadCapturePopup").show();
    $(".blackOut-window").show();
    appendHash("contactDetails");
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Clicked', 'lab': bikeVersionLocation });
});

$(".leadCapture-close-btn").on("click", function () {
    leadPopupClose();
    window.history.back();
});

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        $("div.breakupCloseBtn").click();
        $("div.termsPopUpCloseBtn").click();
        $("div.leadCapture-close-btn").click();
        leadPopupClose();
        $("div#termsPopUpContainer").hide();
        $(".blackOut-window").hide();
    }
});

var leadPopupClose = function () {
    leadCapturePopup.hide();
    $("#contactDetailsPopup").show();
    $("#otpPopup").hide();
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
};

$(".more-features-btn").click(function () {
    $(".more-features").slideToggle();
    var a = $(this).find("a");
    a.text(a.text() === "+" ? "-" : "+");
    if (a.text() === "+")
        a.attr("href", "#features");
    else a.attr("href", "javascript:void(0)");
});

$("a.read-more-btn").click(function () {
    if (!$(this).hasClass("open")) {
        $(".model-about-main").hide();
        $(".model-about-more-desc").show();
        var a = $(this).find("span");
        a.text(a.text() === "full story" ? "less" : "full story");
        $(this).addClass("open");
    }
    else if ($(this).hasClass("open")) {
        $(".model-about-main").show();
        $(".model-about-more-desc").hide();
        var a = $(this).find("span");
        a.text(a.text() === "full story" ? "less" : "full story");
        $(this).removeClass("open");
    }

});

$('#bookNowBtn').on('click', function (e) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Book_Now_Clicked', 'lab': bikeVersionLocation });
    var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;    
    window.location.href = "/m/pricequote/bookingSummary_new.aspx?MPQ=" + Base64.encode(cookieValue);;
});

ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

$(document).mouseup(function (e) {
    if (!$(".variantDropDown, .sort-div, .sort-div #upDownArrow, .sort-by-title").is(e.target)) {
        $.sortChangeUp($(".sort-div"));
    }
});

// GA Tags
$('#btnGetOnRoadPrice').on('click', function (e) {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Check_On_Road_Price_Click', 'lab': bikeVersionLocation });
});

function getBikeVersionLocation() {
    var versionName = getBikeVersion();
    var loctn = getCityArea;
    if (loctn != '')
        loctn = '_' + loctn;
    var bikeVersionLocation = myBikeName + '_' + versionName + loctn;
    return bikeVersionLocation;
}

function getBikeVersion() {
    var versionName = '';
    if ($('#defaultVariant').length > 0) {
        versionName = $('#defaultVariant').html();
    }
    else if ($('#versText').length > 0) {
        versionName = $('#versText').html()
    }
    return versionName;
}

$('.tnc').on('click', function (e) {
    LoadTerms($(this).attr("id"));
});

function LoadTerms(offerId) {
    //$(".termsPopUpContainer").css('height', '150')
    $('#termspinner').show();
    $('#terms').empty();
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();

    //var url = abHostUrl + "/api/DealerPriceQuote/GetOfferTerms?offerMaskingName=&offerId=" + offerId;
    if (offerId != '' && offerId != null) {
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerMaskingName=&offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                //$(".termsPopUpContainer").css('height', '500')
                $('#termspinner').hide();
                if (response != null)
                    $('#terms').html(response);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });
    }
    else {
        setTimeout(LoadTerms, 2000); // check again in a second
    }
}
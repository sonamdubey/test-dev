$(document).ready(function () {

    var $window = $(window),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        modelBottomCard = $('#model-bottom-card-wrapper'),
        modelSpecsFooter = $('#modelSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
    if (tabsLength < 3) {
        $('.overall-specs-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
    }

    $('.overall-specs-tabs-wrapper li').first().addClass('active');
    var bodHt, footerHt, scrollPosition;

    $(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            modelSpecsTabsOffsetTop = modelBottomCard.offset().top,
            modelSpecsFooterOffsetTop = modelSpecsFooter.offset().top;

        if (windowScrollTop > modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.addClass('fixed-tab-nav');
        }

        else if (windowScrollTop < modelSpecsTabsOffsetTop) {
            overallSpecsTabsContainer.removeClass('fixed-tab-nav');
        }

        if (overallSpecsTabsContainer.hasClass('fixed-tab-nav')) {
            if (windowScrollTop > modelSpecsFooterOffsetTop - topNavBarHeight) {
                overallSpecsTabsContainer.removeClass('fixed-tab-nav');
            }
        }


        $('#model-bottom-card-wrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - overallSpecsTabsContainer.height(),
                bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('li').removeClass('active');
                $('#model-bottom-card-wrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');

                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');

            }
        });

        var scrollToTab = $('#model-bottom-card-wrapper .bw-model-tabs-data:eq(3)');
        if (scrollToTab.length != 0) {
            if (windowScrollTop > scrollToTab.offset().top - 45) {
                if (!$('#overall-specs-tab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left');
                    scrollHorizontal(400);
                }
            }

            else if (windowScrollTop < scrollToTab.offset().top) {
                if ($('#overall-specs-tab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left');
                    scrollHorizontal(0);
                }
            }
        }

        $(window).scroll(function () {
            bodHt = $('body').height();
            footerHt = $('footer').height();
            scrollPosition = $(this).scrollTop();
            if (scrollPosition + $(window).height() > (bodHt - footerHt))
                $('.float-button').hide().removeClass('float-fixed');
            else
                $('.float-button').show().addClass('float-fixed');
        });

    });

    function scrollHorizontal(pos) {
        $('#overall-specs-tab').animate({ scrollLeft: pos + 'px' }, 500);
    }

    $('.overall-specs-tabs-wrapper li').click(function () {
        var target = $(this).attr('data-tabs');
        $('html, body').animate({ scrollTop: $(target).offset().top - overallSpecsTabsContainer.height() }, 1000);
        centerItVariableWidth($(this), '.overall-specs-tabs-container');
        return false;
    });

    function centerItVariableWidth(target, outer) {
        var out = $(outer);
        var tar = target;
        var x = out.width();
        var y = tar.outerWidth(true);
        var z = tar.index();
        var q = 0;
        var m = out.find('li');
        for (var i = 0; i < z; i++) {
            q += $(m[i]).outerWidth(true);
        }
        out.animate({ scrollLeft: Math.max(0, q - (x - y) / 2) }, 500, 'swing');
    }

    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active');

});

var requesterName = $('#requesterName'),
    requesterEmail = $('#requesterEmail'),
    requesterMobile = $('#requesterMobile');

$('#request-media-btn').on('click', function () {
    requestMediaPopup.open();
    appendHash("requestMedia");
    $('body, html').addClass('lock-browser-scroll');
});

$('#submit-requester-details-btn').on('click', function () {
    if (ValidateUserDetail(requesterName, requesterEmail, requesterMobile)) {
        requestMediaPopup.acknowledgmentSection();
        $('#request-media-btn').hide();
    }
});

$('#submit-request-sent-btn, .request-media-close').on('click', function () {
    requestMediaPopup.close();
    window.history.back();
    $('body, html').removeClass('lock-browser-scroll');
});

var requestMediaPopup = {
    popup: $('#request-media-popup'),

    userDetails: $('#requester-details-section'),

    acknowledgment: $('#request-sent-section'),

    open: function () {
        requestMediaPopup.popup.show();
    },

    close: function () {
        requestMediaPopup.popup.hide();
        requestMediaPopup.userDetailsSection();
    },

    userDetailsSection: function () {
        requestMediaPopup.acknowledgment.hide();
        requestMediaPopup.userDetails.show();
    },

    acknowledgmentSection: function () {
        requestMediaPopup.userDetails.hide();
        requestMediaPopup.acknowledgment.show();
    },
}

$('#submit-requester-details-btn').on('click', function () {
    if (ValidateUserDetail(requesterName, requesterEmail, requesterMobile)) {
        
    }
});

/* input focus */
requesterName.on("focus", function () {
    validate.onFocus(requesterName);
});

requesterEmail.on("focus", function () {
    validate.onFocus(requesterEmail);
});

requesterMobile.on("focus", function () {
    validate.onFocus(requesterMobile);
});

/* input blur */
requesterName.on("blur", function () {
    validate.onBlur(requesterName);
});

requesterEmail.on("blur", function () {
    validate.onBlur(requesterEmail);
});

requesterMobile.on("blur", function () {
    validate.onBlur(requesterMobile);
});


var getUserName = $('#getUserName'),
    getUserEmailID = $('#getUserEmailID'),
    getUserMobile = $('#getUserMobile'),
    getUpdatedUserMobile = $('#getUpdatedMobile'),
    getUserOTP = $('#getUserOTP');

$('#get-seller-button').on('click', function () {
    getSellerDetailsPopup.open();
    appendHash("sellerDealers");
    $('body, html').addClass('lock-browser-scroll');
});

$('.seller-details-close').on('click', function () {
    getSellerDetailsPopup.close();
    window.history.back();
    $('body, html').removeClass('lock-browser-scroll');
});

$('#submit-user-details-btn').on('click', function () {
    if (ValidateUserDetail(getUserName, getUserEmailID, getUserMobile)) {
        getSellerDetailsPopup.verifySection();
        var submittedMobile = getUserMobile.val();
        $('#mobile-verification-section').find('.user-submitted-mobile').text(submittedMobile);
    }
});

$('#edit-mobile-btn').on('click', function () {
    var prevMobile = getUserMobile.val();
    getSellerDetailsPopup.updateMobileSection();
    getUserOTP.val('');
    getUpdatedUserMobile.focus().val(prevMobile);
});

$('#submit-updated-mobile-btn').on('click', function () {
    if (validateMobile(getUpdatedUserMobile)) {
        var newMobile = getUpdatedUserMobile.val();
        $('#verify-otp-content').find('.user-submitted-mobile').text(newMobile);

        var inputBox = getUserOTP.closest('.input-box');
        if (inputBox.hasClass('invalid')) {
            inputBox.removeClass('invalid').find('.error-text').text('');
        }

        getSellerDetailsPopup.generateNewOTP();
    }
});

$('#submit-user-otp-btn').on('click', function () {
    if (validateOTP()) {
        getSellerDetailsPopup.sellerDetails();
    }
});

var getSellerDetailsPopup = {

    popup: $('#get-seller-details-popup'),

    userDetails: $('#user-details-section'),

    mobileVerification: $('#mobile-verification-section'),

    verifyOTP: $('#verify-otp-content'),

    updateMobile: $('#update-mobile-content'),

    seller: $('#seller-details-section'),

    open: function () {
        getSellerDetailsPopup.popup.show();
    },

    close: function () {
        getSellerDetailsPopup.popup.hide();
        getSellerDetailsPopup.userDetailsSection();
    },

    userDetailsSection: function () {
        getSellerDetailsPopup.mobileVerification.hide();
        getSellerDetailsPopup.seller.hide();
        getSellerDetailsPopup.userDetails.show();
        getSellerDetailsPopup.generateNewOTP();
    },

    verifySection: function () {
        getSellerDetailsPopup.userDetails.hide();
        getSellerDetailsPopup.mobileVerification.show();
    },

    updateMobileSection: function () {
        getSellerDetailsPopup.verifyOTP.hide();
        getSellerDetailsPopup.updateMobile.show();
    },

    generateNewOTP: function () {
        getSellerDetailsPopup.updateMobile.hide();
        getSellerDetailsPopup.verifyOTP.show();
    },

    sellerDetails: function () {
        getSellerDetailsPopup.mobileVerification.hide();
        getSellerDetailsPopup.seller.show();
    }
}

function ValidateUserDetail(name, email, mobile) {
    var isValid = true;
    isValid = validateEmail(email);
    isValid &= validateMobile(mobile);
    isValid &= validateName(name);
    return isValid;
};

/* name validation */
function validateName(name) {
    var isValid = true;
    var a = name.val().length;
    if ((/&/).test(name.val())) {
        isValid = false;
        validate.setError(name, 'Invalid name');
    }
    else if (a == 0) {
        isValid = false;
        validate.setError(name, 'Please enter your name');
    }
    else if (a >= 1) {
        validate.hideError(name);
    }
    return isValid;
}

/* Email validation */
function validateEmail(email) {
    var isValid = true;
    var emailID = email.val();
    var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

    if (emailID == "") {
        validate.setError(email, 'Please enter email address');
        isValid = false;
    }
    else if (!reEmail.test(emailID)) {
        validate.setError(email, 'Invalid Email');
        isValid = false;
    }
    
    return isValid;
}

/* mobile validation */
function validateMobile(mobile) {
    var isValid = true;
    var reMobile = /^[0-9]{10}$/;
    var mobileNo = mobile.val();
    if (mobileNo == "") {
        isValid = false;
        validate.setError(mobile, "Please enter your mobile number");
    }
    else if (!reMobile.test(mobileNo) && isValid) {
        isValid = false;
        validate.setError(mobile, "Mobile number should be 10 digits");
    }
    else {
        validate.hideError(mobile)
    }
    
    return isValid;
}

/* otp validation */
function validateOTP() {
    var retVal = true;
    var isNumber = /^[0-9]{5}$/;
    var cwiCode = getUserOTP.val();
    if (cwiCode == "") {
        retVal = false;
        validate.setError(getUserOTP, "Please enter your Verification Code");
    }
    else {
        if (isNaN(cwiCode)) {
            retVal = false;
            validate.setError(getUserOTP, "Verification Code should be numeric");
        }
        else if (cwiCode.length != 5) {
            retVal = false;
            validate.setError(getUserOTP, "Verification Code should be of 5 digits");
        }
    }
    return retVal;
}

/* input focus */
getUserName.on("focus", function () {
    validate.onFocus(getUserName);
});

getUserEmailID.on("focus", function () {
    validate.onFocus(getUserEmailID);
});

getUserMobile.on("focus", function () {
    validate.onFocus(getUserMobile);
});

getUpdatedUserMobile.on("focus", function () {
    validate.onFocus(getUpdatedUserMobile);
});

getUserOTP.on("focus", function () {
    validate.onFocus(getUserOTP);
});

/* input blur */
getUserName.on("blur", function () {
    validate.onBlur(getUserName);
});

getUserEmailID.on("blur", function () {
    validate.onBlur(getUserEmailID);
});

getUserMobile.on("blur", function () {
    validate.onBlur(getUserMobile);
});

getUpdatedUserMobile.on("blur", function () {
    validate.onBlur(getUpdatedUserMobile);
});

getUserOTP.on("blur", function () {
    validate.onBlur(getUserOTP);
});

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


$('#model-main-image').on('click', '.model-gallery-target', function () {
    $('#model-gallery-container').show();

    var slideToClick = function (swiper) {
        var clickedSlide = swiper.slides[swiper.clickedIndex];
        $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
        $(clickedSlide).addClass('swiper-slide-active');
        galleryTop.slideTo(swiper.clickedIndex, 500);
    };

    var galleryThumbs = new Swiper('.carousel-navigation-photos', {
        slideActiveClass: '',
        spaceBetween: 0,
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
        onSlideChangeEnd: slidegalleryThumbs
    });

    var currentStagePhoto, currentStageActiveImage;
    function showImgTitle(swiper) {
        imgTitle = $(galleryThumbs.slides[swiper.activeIndex]).find('img').attr('title');
        imgTotalCount = galleryThumbs.slides.length;
        $(".media-title").text(imgTitle);
        $(".gallery-count").text(swiper.activeIndex + 1 + " of " + imgTotalCount.toString());
        currentStagePhoto = $(".connected-carousels-photos .stage-photos");
        currentStageActiveImage = currentStagePhoto.find(".swiper-slide.swiper-slide-active img");
        currentStagePhoto.find('.carousel-stage-photos').css({ 'height': currentStageActiveImage.height() });
    }

    showImgTitle(galleryTop);
});

$('#model-gallery-container').on('click', '.gallery-close-btn', function () {
    $('#model-gallery-container').hide();
    $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active')
});
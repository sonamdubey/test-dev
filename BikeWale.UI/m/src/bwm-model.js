// JavaScript Document
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

ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

function CustomerModel() {
    var arr = setuserDetails();
    var self = this;
    if (arr != null && arr.length > 0) {
        self.fullname = ko.observable(arr[0]);
        self.emailId = ko.observable(arr[1]);
        self.mobileNo = ko.observable(arr[2]);
    }
    else {
        self.fullname = ko.observable();
        self.emailId = ko.observable();
        self.mobileNo = ko.observable();
    }
    self.IsVerified = ko.observable(false);
    self.NoOfAttempts = ko.observable(0);
    self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
    self.otpCode = ko.observable();
    self.fullName = ko.computed(function () {
        var _fullname = self.fullname() != undefined ? self.fullname() : "";

        return _fullname;
    }, this);

    self.verifyCustomer = function () {
        if (!self.IsVerified()) {
            var objCust = {
                "dealerId": dealerId,
                "pqId": pqId,
                "customerName": self.fullName,
                "customerMobile": self.mobileNo,
                "customerEmail": self.emailId,
                "clientIP": clientIP,
                "pageUrl": pageUrl,
                "versionId": versionId,
                "cityId": cityId
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                data: ko.toJSON(objCust),
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
                "customerMobile": self.mobileNo,
                "customerEmail": self.emailId,
                "cwiCode": self.otpCode,
                "branchId": dealerId,
                "customerName": self.fullName,
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

    self.fullName = ko.computed(function () {
        var _fullname = self.fullname() != undefined ? self.fullname() : "";
        return _fullname;
    }, this);

    self.submitLead = function () {
        if (ValidateUserDetail()) {
            self.verifyCustomer();
            if (self.IsValid()) {
                $("#personalInfo").hide();
                $("#leadCapturePopup .leadCapture-close-btn").click();
                window.location.href = "/pricequote/bookingConfig.aspx";
            }
            else {
                otpContainer.removeClass("hide").addClass("show");
                detailsSubmitBtn.hide();
                nameValTrue();
                hideError(mobile);
                otpText.val('').removeClass("border-red").siblings("span, div").hide();
            }
            setPQUserCookie();
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_Successful_Submit', 'lab': getCityArea });
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

                // OTP Success
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
                $("#leadCapturePopup .leadCapture-close-btn").click();
                window.location.href = "/pricequote/detaileddealerquotation.aspx";

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
    if (fullname.val().indexOf('&') != -1) {
        isValid = false;
        setError(fullname, 'Invalid name');
    }
    else if (a == 0) {
        isValid = false;
        setError(fullname, 'Please enter your first name');
    }
    else if (a >= 1) {
        isValid = true;
        nameValTrue()
    }
    if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Name', 'lab': getCityArea }); }
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

emailid.on("keyup keydown blur", function () {
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

mobile.on("keyup keydown blur", function () {
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

$(function () {
    $(".carousel-navigation ul li").slice(0, 4).find("img.lazy").trigger("imgLazyLoad");
    $(".jcarousel.stage ul li").slice(0, 3).find("img.lazy").trigger("imgLazyLoad");
});

jQuery(function () {

    var connector2 = function (itemNavigation2, carouselStage2) {
        return carouselStage2.jcarousel('items').eq(itemNavigation2.index());
    };
    var connector3 = function (itemNavigation3, carouselStage3) {
        //return carouselStage3.jcarousel('items').eq(itemNavigation3.index());
    };

    jQuery('.jcarousel-wrapper.model .jcarousel')
    .on('jcarousel:create jcarousel:reload', function () {
        var element = $(this),
            width = element.innerWidth();
        element.jcarousel('items').css('width', width + 'px');
    });

    $(".jcarousel.stage").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $('.jcarousel-wrapper.model .jcarousel li').removeClass('activeSlide');
        $(this).addClass('activeSlide');
    });
   
    $(".jcarousel-wrapper.model .jcarousel-control-right").click(function () {
        getModelImageNextIndex();
    });

    $(".jcarousel-wrapper.model .jcarousel-control-left").click(function () {
        getModelImagePrevIndex();
    });


    $(".alternatives-carousel").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $(this).find("img.lazy").trigger("imgLazyLoad");
    });

    var carouselStage2 = $('.carousel-stage-photos').jcarousel();
    var carouselNavigation2 = $('.carousel-navigation-photos').jcarousel();

   // var carouselStage3 = $('.carousel-stage-videos').jcarousel();
    var carouselNavigation3 = $('.carousel-navigation-videos').jcarousel();


    carouselNavigation2.jcarousel('items').each(function () {
        var item2 = $(this);
        var target = connector2(item2, carouselStage2);
        item2
            .on('jcarouselcontrol:active', function () {
                carouselNavigation2.jcarousel('scrollIntoView', this);
                item2.addClass('active');
            })
            .on('jcarouselcontrol:inactive', function () {
                item2.removeClass('active');
            })
            .jcarouselControl({
                target: target,
                carousel: carouselStage2
            });
    });

    carouselNavigation3.jcarousel('items').each(function () {
        var item3 = $(this);
        var target = connector3(item3);
        item3
            .on('jcarouselcontrol:active', function () {
                carouselNavigation3.jcarousel('scrollIntoView', this);
                item3.addClass('active');
            })
            .on('jcarouselcontrol:inactive', function () {
                item3.removeClass('active');
            })
    });

    $('.prev-stage, .photos-prev-stage, .videos-prev-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
            })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
            })
            .jcarouselControl({
                target: '-=1'
            });
    $('.next-stage, .photos-next-stage, .videos-next-stage')
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .jcarouselControl({
            target: '+=1'
        });
    $('.prev-navigation, .photos-prev-navigation, .videos-prev-navigation')
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .jcarouselControl({
            target: '-=4'
        });
    $('.next-navigation, .photos-next-navigation, .videos-next-navigation')
        .on('jcarouselcontrol:inactive', function () {
            $(this).addClass('inactive');
        })
        .on('jcarouselcontrol:active', function () {
            $(this).removeClass('inactive');
        })
        .jcarouselControl({
            target: '+=4'
        });


    $(".jcarousel.stage, .carousel-navigation-photos, .carousel-stage-photos,.carousel-navigation-videos").on('jcarousel:visiblein', 'li', function (event, carousel) {
        $(this).find("img.lazy").trigger("imgLazyLoad");
    });


    $(".carousel-stage-photos, .carousel-navigation-photos,.carousel-navigation-videos").swipe({
        fingers: 'all', swipeLeft: swipe2, swipeRight: swipe2, allowPageScroll: "auto",
        excludedElements: "label, button, input, select, textarea, .noSwipe",
    });


    function swipe2(event, direction, distance, duration, fingerCount) {
        if (direction == "left") {
            $(this).closest('.connected-carousels-photos .stage-photos,.navigation-photos,.navigation-videos').find("a.jcarousel-control-next,a.photos-next-stage,a.photos-next-navigation,a.videos-next-navigation").click();
        }
        else if (direction == "right") {
            $(this).closest('.connected-carousels-photos .stage-photos,.navigation-photos,.navigation-videos').find("a.jcarousel-control-prev,a.photos-prev-stage,a.photos-prev-navigation,a.videos-prev-navigation").click();

        }
    }


});

$(".photos-next-stage").click(function () {
    getImageNextIndex();
});

$(".photos-prev-stage").click(function () {
    getImagePrevIndex();
});

$(".carousel-navigation-photos").click(function () {
    getImageIndex();
});

$("#bikeBannerImageCarousel .stage li").click(function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Photo_Clicked', 'lab': myBikeName });
    if (imgTotalCount > 0) {
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window-model").show();
        $(".bike-gallery-popup").removeClass("hide").addClass("show");
        $(".modelgallery-close-btn").removeClass("hide").addClass("show");

        $('.carousel-stage-photos')
        .on('jcarousel:create jcarousel:reload', function () {
            var element = $(this),
                width = element.innerWidth();
            element.jcarousel('items').css('width', width + 'px');
        })
        .jcarousel();
        $(".carousel-navigation-photos ul li").slice(0, 5).find("img.lazy").trigger("imgLazyLoad");
        $(".carousel-stage-photos ul li").slice(0, 3).find("img.lazy").trigger("imgLazyLoad");
        $(".carousel-navigation-videos ul li").slice(0, 5).find("img.lazy").trigger("imgLazyLoad");
    }
});

$(".modelgallery-close-btn").click(function () {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window-model").hide();
    $(".bike-gallery-popup").removeClass("show").addClass("hide");
    $(".modelgallery-close-btn").removeClass("show").addClass("hide");
    videoiFrame.setAttribute("src", "");
});

$(document).ready(function () {
    imgTotalCount = $(".carousel-stage-photos ul li").length;
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
});


function getImageNextIndex() {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").next();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

function getImagePrevIndex() {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").prev();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

function getModelImageNextIndex() {
    var imgIndexA = $(".jcarousel-wrapper.model .jcarousel ul li.activeSlide").next();
    var imgIndex = imgIndexA.index();
    var imgTitle = imgIndexA.find("img").attr("title");
    setModelImageDetails(imgTitle, imgIndex);
}

function getModelImagePrevIndex() {
    var imgIndexA = $(".jcarousel-wrapper.model .jcarousel ul li.activeSlide").next();
    var imgIndex = imgIndexA.index();
    var imgTitle = imgIndexA.find("img").attr("title");
    setModelImageDetails(imgTitle, imgIndex);
}

function getImageIndex() {
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}
function setImageDetails(imgTitle, imgIndex) {
    $(".leftfloatbike-gallery-details").text(imgTitle);
    if (imgIndex > 0) {
        $(".bike-gallery-count").text(imgIndex.toString() + "/" + imgTotalCount.toString());
    }
}

function setModelImageDetails(imgTitle, imgIndex) {
   // $(".leftfloatbike-gallery-details").text(imgTitle);
    if (imgIndex > 0) {
        $(".bike-model-gallery-count").text(imgIndex.toString() + "/" + imgTotalCount.toString());
    }
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

//photos corousel function
(function ($) {

    var connector = function (itemNavigation, carouselStage) {
        return carouselStage.jcarousel('items').eq(itemNavigation.index());
};

    $(function () {

        var carouselStage = $('.carousel-stage').jcarousel();
        var carouselNavigation = $('.carousel-navigation').jcarousel();


        carouselNavigation.jcarousel('items').each(function () {
            var item = $(this);


            var target = connector(item, carouselStage);

            item
                .on('jcarouselcontrol:active', function () {
                    carouselNavigation.jcarousel('scrollIntoView', this);
                    item.addClass('active');
            })
                .on('jcarouselcontrol:inactive', function () {
                    item.removeClass('active');
            })
                .jcarouselControl({
                        target: target,
                        carousel: carouselStage
            });
        });


        $('.prev-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
        })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
        })
            .jcarouselControl({
                    target: '-=1'
        });

        $('.next-stage')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
        })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
        })
            .jcarouselControl({
                    target: '+=1'
        });


        $('.prev-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
        })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
        })
            .jcarouselControl({
                    target: '-=1'
        });

        $('.next-navigation')
            .on('jcarouselcontrol:inactive', function () {
                $(this).addClass('inactive');
        })
            .on('jcarouselcontrol:active', function () {
                $(this).removeClass('inactive');
        })
            .jcarouselControl({
                    target: '+=1'
        });
    });
})(jQuery);

$("#btnShowOffers").on("click", function () {
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Show_Offers_Clicked', 'lab': myBikeName });
});

$(".viewMoreOffersBtn").on("click", function () {
    $(this).hide();
    $(this).prev("ul.moreOffersList").slideToggle();
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
    $('#hdnVariant').val($(this).attr('title'));
});

$("#viewBreakupText").on('click', function (e) {
    $("div#breakupPopUpContainer").show();
    $(".blackOut-window").show();
});
$(".breakupCloseBtn,.blackOut-window").on('mouseup click', function (e) {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
});

$(".termsPopUpCloseBtn").on('mouseup click', function (e) {
    $("div#termsPopUpContainer").hide();
    $(".blackOut-window").hide();
});

$("#getMoreDetailsBtn").on('click', function (e) {
    $("div#leadCapturePopup").show();
    $(".blackOut-window").show();
});

$(".leadCapture-close-btn").on("click", function () {
    leadCapturePopup.hide();
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
});

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        $("div.breakupCloseBtn").click();
        $("div.termsPopUpCloseBtn").click();
        $("div.leadCapture-close-btn").click();
    }
});

$(".more-features-btn").click(function () {
    $(".more-features").slideToggle();
    $("html, body").animate({ scrollTop: $("#features").offset().top }, 1000);
    var a = $(this).find("span");
    a.text(a.text() === "+" ? "-" : "+");
    $("html, body").animate({ scrollTop: $("#features").offset().top }, 1000);
});

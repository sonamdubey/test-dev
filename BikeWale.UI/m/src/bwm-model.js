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

var imgTitle, imgTotalCount;
var leadBtnBookNow = $("#leadBtnBookNow"), leadCapturePopup = $("#leadCapturePopup");
var fullname = $("#getFullName");
var emailid = $("#getEmailID");
var mobile = $("#getMobile");
var otpContainer = $(".mobile-verification-container");

var detailsSubmitBtn = $("#user-details-submit-btn");
var otpText = $("#getOTP");
var otpBtn = $("#otp-submit-btn");
var getOffersClicked = false;

$("#leadBtnBookNow").on('click', function () {
    leadCapturePopup.show();
    $('body').addClass('lock-browser-scroll');
    $(".blackOut-window").show();
});
function openLeadPopup(ele) {
    leadSourceId = ele.attr("leadSourceId");
    leadCapturePopup.show();
    $('body').addClass('lock-browser-scroll');
    $(".blackOut-window").show();
}

$(".leadcapture").on('click', function () {
    openLeadPopup($(this));
});
var leadPopupClose = function () {
    leadCapturePopup.hide();
    $("#contactDetailsPopup").show();
    $("#otpPopup,#notify-response").hide();
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
};

$(".leadCapture-close-btn").on("click", function () {
    leadCapturePopupCloseBtn();
    window.history.back();
});

$(".blackOut-window").on("click", function () {
    if ($("#leadCapturePopup").css("display") == "block") {
        leadCapturePopupCloseBtn();
    }
    else if ($("div#termsPopUpContainer").css("display") == "block") {
        $("div#termsPopUpContainer").hide();
        $(".blackOut-window").hide();
    }
});

var leadCapturePopupCloseBtn = function () {
    $("#leadCapturePopup").hide();
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
}

$('#getMoreDetailsBtn,#getAssistance').on('click', function (e) {
    leadSourceId = $(this).attr("leadSourceId");
    $("#leadCapturePopup").show();
    $(".blackOut-window").show();
    appendHash("contactDetails");

    if ($(this).attr("id") == "getAssistance")
    {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Get_Offers_Clicked", "lab": bikeVersionLocation });
        getOffersClicked = true;
    }
});

function getBikeVersion() {
    return versionName;
}

function getBikeVersionLocation() {
    var versionName = getBikeVersion();
    var loctn = getCityArea;
    if (loctn != '')
        loctn = '_' + loctn;
    var bikeVersionLocation = myBikeName + '_' + versionName + loctn;
    return bikeVersionLocation;
}

$("#viewBreakupText").on('click', function (e) {
    triggerGA('Model_Page', 'View_Detailed_Price_Clicked', bikeVersionLocation);
    secondarydealer_Click(dealerId);
});
$(".breakupCloseBtn, #notifyOkayBtn").on('click', function (e) {
    viewBreakUpClosePopup();
    window.history.back();
});

var viewBreakUpClosePopup = function () {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
    $("#contactDetailsPopup").show();
    leadPopupClose();
};

$(".termsPopUpCloseBtn").on('click', function (e) {
    $("div#termsPopUpContainer").hide();
    $(".blackOut-window").hide();
});

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
    window.location.href = "/m/pricequote/bookingSummary_new.aspx?MPQ=" + Base64.encode(cookieValue);
});

$('#requestcallback').on('click', function (e) {
    openLeadPopup($(this));
});

$('#getofferspopup').on('click', function (e) {
    $('#dealer-offers-popup').hide();
    openLeadPopup($(this));
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
                "leadSourceId": leadSourceId,
                "deviceId": getCookie('BWC')
            }
            $.ajax({
                type: "POST",
                url: "/api/PQCustomerDetail/",
                data: ko.toJSON(objCust),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('utma', getCookie('__utma'));
                    xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
                },
                async: false,
                contentType: "application/json",
                dataType: 'json',
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
                dataType: 'json',
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
                dataType: 'json',
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
        var isValidCustomer = ValidateUserDetail();
        if (isValidCustomer && isDealerPriceAvailable == "True" && campaignId == 0) {
            self.verifyCustomer();
            if (self.IsValid()) {
                $("#contactDetailsPopup").hide();
                $("#otpPopup").hide();
                $('#notify-response .notify-leadUser').text(self.fullName());
                $('#notify-response').show();

                if (getOffersClicked) {
                    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": "Main_Form_" + bikeVersionLocation });
                    getOffersClicked = false;
                }

                else {
                    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": bikeVersionLocation });
                }

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
        }

        else if (isValidCustomer && isDealerPriceAvailable == "False" && campaignId > 0) {
            self.submitCampaignLead();
            setPQUserCookie();
            if (getOffersClicked) {
                dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": "Main_Form_" + bikeVersionLocation });
                getOffersClicked = false;
            }

            else {
                dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Lead_Submitted", "lab": bikeVersionLocation });
            }
        }

    };

    self.submitCampaignLead = function () {
        $('#processing').show();
        var objCust = {
            "dealerId": manufacturerId,
            "pqId": pqId,
            "name": self.fullName(),
            "mobile": self.mobileNo(),
            "email": self.emailId(),
            //"clientIP": clientIP,
            //"pageUrl": pageUrl,
            "versionId": versionId,
            "cityId": cityId,
            "leadSourceId": leadSourceId,
            "deviceId": getCookie('BWC')
        }
        $.ajax({
            type: "POST",
            url: "/api/ManufacturerLead/",
            data: ko.toJSON(objCust),
            beforeSend: function (xhr) {
                xhr.setRequestHeader('utma', getCookie('__utma'));
                xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
            },
            async: false,
            contentType: "application/json",
            dataType: 'json',
            success: function (response) {
                //var obj = ko.toJS(response);
                $("#personalInfo,#otpPopup").hide();
                $('#processing').hide();
                //validationSuccess($(".get-lead-mobile"));
                $("#contactDetailsPopup").hide();
                $('#notify-response .notify-leadUser').text(self.fullName());
                $('#notify-response').show();                
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#contactDetailsPopup,#otpPopup").hide();
                $('#processing').hide();
                var leadMobileVal = mobile.val(); otpContainer.removeClass("hide").addClass("show");
                //detailsSubmitBtn.hide();
                nameValTrue();
                hideError(mobile);
            }
        });
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
                $("#contactDetailsPopup").hide();
                $("#otpPopup").hide();
                $('#notify-response .notify-leadUser').text(self.fullName());
                $('#notify-response').show();
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
        $('.float-button').hide().removeClass('float-fixed');
    else
        $('.float-button').show().addClass('float-fixed');
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
    $('#hdnVariant').val($(this).attr('versionid'));
    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersionLocation });
});


ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

$(document).mouseup(function (e) {
    if (!$(".variantDropDown, .sort-div, .sort-div #upDownArrow, .sort-by-title").is(e.target)) {
        $.sortChangeUp($(".sort-div"));
    }
});

// GA Tags
$('#btnGetOnRoadPrice').on('click', function (e) {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Check_On_Road_Price_Clicked", "lab": bikeVersionLocation });
});

$("#btnDealerPricePopup").on("click", function () {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Show_On_Road_Price_Clicked", "lab": bikeVersionLocation });
});


$('.tnc').on('click', function (e) {
    LoadTerms($(this).attr("id"));
});

$('.changeCity').on('click', function (e) {
    try {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'City_Change_Initiated', 'lab': bikeVersionLocation });
    }
    catch (err) { }
});

function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $('#termspinner').show();
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerMaskingName=&offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
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
        $("#terms").load("/statichtml/tnc.html");
        //$('#terms').html($("#orig-terms").html());
    }
    $('#termspinner').hide();
}
$('#locslug').on('click', function (e) {
    triggerGA('Model_Page', 'Booking_Benefits_City_Link_Clicked', myBikeName + '_' + getBikeVersion());
});
$('#calldealer').on('click', function (e) {
    triggerGA('Model_Page', 'Call_Dealer_Clicked', myBikeName + '_' + bikeVersionLocation);
});

//
$('.more-dealers-link').on('click', function () {
    $(this).parent().prev('#moreDealersList').slideDown();
    $(this).hide().next('.less-dealers-link').show();
});

$('.less-dealers-link').on('click', function () {
    $(this).parent().prev('#moreDealersList').slideUp();
    $(this).hide().prev('.more-dealers-link').show();
});



//
$(document).ready(function () {
    
    var $window = $(window),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper'),
        modelSpecsFooter = $('#modelSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
    if (tabsLength < 3) {
        $('.overall-specs-tabs-wrapper li').css({'display': 'inline-block', 'width': 'auto'});
    }

    $('.overall-specs-tabs-wrapper li').first().addClass('active');

    $(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            modelSpecsTabsOffsetTop = modelSpecsTabsContentWrapper.offset().top,
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


        $('#modelSpecsTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - overallSpecsTabsContainer.height(),
                bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('li').removeClass('active');
                $('#modelSpecsTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');
                
                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');
                
            }
        });
        
        var scrollToTab = $('#modelSpecsTabsContentWrapper .bw-model-tabs-data:eq(4)');
        if (scrollToTab.length != 0) {
            if (windowScrollTop > scrollToTab.offset().top - 45) {
                if (!$('#overallSpecsTab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left');
                    scrollHorizontal(400);
                }
            }

            else if (windowScrollTop < scrollToTab.offset().top) {
                if ($('#overallSpecsTab').hasClass('scrolled-left')) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left');
                    scrollHorizontal(0);
                }
            }
        }

    });

    function scrollHorizontal(pos) {
        $('#overallSpecsTab').animate({ scrollLeft: pos + 'px' }, 500);
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

});

$('a.read-more-model-preview').click(function () {
    if (!$(this).hasClass('open')) {
        var self = $(this);
        $('.model-preview-main-content').hide();
        $('.model-preview-more-content').show();
        self.text(self.text() === 'Read more' ? 'Collapse' : 'Read more');
        self.addClass("open");
    }
    else if ($(this).hasClass('open')) {
        var self = $(this);
        $('.model-preview-main-content').show();
        $('.model-preview-more-content').hide();
        self.text(self.text() === 'Read more' ? 'Collapse' : 'Read more');
        self.removeClass('open');
    }
});

var dealersPopupDiv = $('#more-dealers-popup'),
    dealerOffersDiv = $('#dealer-offers-popup');

$('#more-dealers-target').on('click', function () {
    popupDiv.open(dealersPopupDiv);
    appendHash("moreDealers");
    $('body, html').addClass('lock-browser-scroll');
});

$('.dealers-popup-close-btn').on("click", function () {
    popupDiv.close(dealersPopupDiv);
    window.history.back();
});

$('#dealer-offers-list').on('click', 'li', function () {
    popupDiv.open(dealerOffersDiv);
    appendHash("dealerOffers");
    $('body, html').addClass('lock-browser-scroll');
});

$('.offers-popup-close-btn').on("click", function () {
    popupDiv.close(dealerOffersDiv);
    window.history.back();
});

var popupDiv = {
    open: function (div) {
        div.show();
    },

    close: function (div) {
        div.hide();
        $('body, html').removeClass('lock-browser-scroll');
    }
};


$(document).ready(function () {
    if (versionCount > 1) {
        dropdown.setDropdown();
        dropdown.dimension();
    }
});

$(window).resize(function () {
    dropdown.dimension();
});

$('.dropdown-select-wrapper').on('click', '.dropdown-label', function () {
    dropdown.active($(this));
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list.dropdown-with-select li', function () {
    var element = $(this);
    if (!element.hasClass('active')) {
        dropdown.selectItem($(this));
        dropdown.selectOption($(this));
    }
});

var dropdown = {
    setDropdown: function () {
        var selectDropdown = $('.dropdown-select');

        selectDropdown.each(function () {
            dropdown.setMenu($(this));
        });
    },

    setMenu: function (element) {
        $('<div class="dropdown-menu"></div>').insertAfter(element);
        dropdown.setStructure(element);
    },

    setStructure: function (element) {
        var elementValue = element.find('option:selected').text(),
			menu = element.next('.dropdown-menu');
        menu.append('<p id="defaultVariant" class="dropdown-label">' + elementValue + '</p><div class="dropdown-list-wrapper"><p class="dropdown-selected-item">' + elementValue + '</p><ul id="templist" class="dropdown-menu-list dropdown-with-select"></ul></div>');
        dropdown.setOption(element);
    },

    setOption: function (element) {
        var selectedIndex = element.find('option:selected').index(),
			menu = element.next('.dropdown-menu'),
			menuList = menu.find('ul');

        element.find('option').each(function (index) {
            if (selectedIndex == index) {
                menuList.append('<li><input value="' + $(this).text() + '" type="submit" runat="server" class="active fullwidth" id="temp_' + index + '" data-option-value="' + $(this).val() + '" title="' + $(this).text() + '"></li>');
            }
            else {
                menuList.append('<li><input value="' + $(this).text() + '" type="submit" runat="server" class="fullwidth" id="temp_' + index + '" data-option-value="' + $(this).val() + '" title="' + $(this).text() + '"></li>');
            }
        });
    },

    active: function (label) {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
        label.closest('.dropdown-menu').addClass('dropdown-active');
    },

    inactive: function () {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
    },

    selectItem: function (element) {
        var elementText = element.text(),
			menu = element.closest('.dropdown-menu'),
			dropdownLabel = menu.find('.dropdown-label'),
			selectedItem = menu.find('.dropdown-selected-item');

        element.siblings('li').removeClass('active');
        element.addClass('active');
        selectedItem.text(elementText);
        dropdownLabel.text(elementText);
    },

    selectOption: function (element) {
        var elementValue = element.attr('data-option-value'),
			wrapper = element.closest('.dropdown-select-wrapper'),
			selectDropdown = wrapper.find('.dropdown-select');

        selectDropdown.val(elementValue).trigger('change');

    },

    dimension: function () {
        var windowWidth = dropdown.deviceWidth();
        if (windowWidth > 480) {
            dropdown.resizeWidth(windowWidth);
        }
        else {
            $('.dropdown-select-wrapper').find('.dropdown-list-wrapper').css('width', 'auto');
        }
    },

    deviceWidth: function () {
        var windowWidth = $(window).width();
        return windowWidth;
    },

    resizeWidth: function (newWidth) {
        $('.dropdown-select-wrapper').find('.dropdown-list-wrapper').css('width', newWidth/2);
    }
};

$(document).on('click', function (event) {
    event.stopPropagation();
    var bodyElement = $('body'),
		dropdownLabel = bodyElement.find('.dropdown-label'),
		dropdownList = bodyElement.find('.dropdown-menu-list'),
		noSelectLabel = bodyElement.find('.dropdown-selected-item');

    if (!$(event.target).is(dropdownLabel) && !$(event.target).is(dropdownList) && !$(event.target).is(noSelectLabel)) {
        dropdown.inactive();
    }
});

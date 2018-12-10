var SIMILARCARSLISTING = ko.observableArray([]); var isSimilarCarsFetched = false; var temp = 5;
$(window).on('popstate', function (e) {
    if ($('#mck-message-cell').is(':visible')) {        //this condition prevent this popstate handler to execute, as buyerprocess.js file handles it
        return false;
    }
    else if ($('#financeIframe').is(':visible')) {
        carDetails.Finance.btnBackFinanceForm();
    }
    else if ($('.valuation-result-section').css('left') === '0px' && !$('#buyerForm').is(':visible')) {
        carDetails.Valuation.triggerPopUpBackArrow();
    }
});
var carDetails = {
    doc: $(document),
    trackingCategory: "UsedDetailsPage",
    WindowEventHandler: {
        resizeEvent: function () {
            carDetails.Valuation.resizePopUp();
        },
    },
    pageLoad: function () {
        carDetails.registerAllEvents();
        carDetails.utils.initializeReadMoreCollapsable();
        carDetails.Valuation.setPopup();
    },
    registerAllEvents: function () {
        carDetails.Finance.registerEvents();
        carDetails.Valuation.registerEvents();
        carDetails.utils.registerEvents();
    },
    utils: {
        registerEvents: function () {
            carDetails.doc.on('click', '.accordion-toggle', function () {
                $(this).next().slideToggle();
                $(this).find("span.up-down-arrow").toggleClass("change-arrow");
            });
        },
        initializeReadMoreCollapsable: function () {
            var detailsReviewReadMoreCollapse = new ReadMoreCollapse('#disclaimerText', {
                expandText: 'Read More',
                collapseText: 'Collapse',
                ellipsis: false
            });

        },
        
    },
    Finance: {
        registerEvents: function () {
            $(document).on("click", "#getFinance", function (event) {
                carDetails.Finance.triggerFinanceClick(this, event);
            });
            $(document).on("click", "#iframeTimeOutClick,#FinanceFormBack", function () {
                carDetails.Finance.btnBackFinanceForm();
            });
        },
        triggerFinanceClick: function (currentnode, event) {
            event.stopPropagation();
            $('div.popup-loading-pic').show();
            var node = $("#getSellerDetailsBtn .getSellerDetails");
            try {
                if (!isIphone) {
                    $('#financeIframe').show('slide', { direction: 'right' }, 500, function () {
                        classifiedFinance.getFinance($(currentnode).data("href"), node, $("#iframecontent")).then(function (response) {
                            $('div.popup-loading-pic').addClass('hideImportant');
                            window.history.pushState("cartrade", "", "");
                            $("#iframecontent").show();
                            $('div.popup-loading-pic').hide();
                        }).catch(function (errResponse) {
                            $("#iframeTimeOut").show();
                            $('div.popup-loading-pic').hide();
                        });
                    });
                } else {
                    classifiedFinance.openIframeInNewWindow($(currentnode).data("href").split('?')[1] + "&profileId=" + node.attr("profileid"));
                }
            } catch (err) {
                console.log("Some error occured while opening iframe :" + err.message);
            }
        },
        btnBackFinanceForm: function () {
            clearTimeout(classifiedFinance.iframeError);
            $("#iframecontent").empty();
            $("#iframeTimeOut").hide();
            $('#financeIframe').hide('slide', { direction: 'right' }, 500);

        }
    },
    Valuation: {
        loadingIconObj: $('.valuation-result-section-loading-pic'),
        valuationResultSectionObj: $('.valuation-result-section'),
        registerEvents: function () {
            $(document).on('click', '.view-market-price', carDetails.Valuation.triggerValuationClick);
            $(document).on('click', '#evaluateResultPopupBackArrow', carDetails.Valuation.triggerPopUpBackArrow);
        },
        triggerValuationClick: function (event) {
            event.stopPropagation();
            var valuationLink = $(this);
            var gsdButton = $("#getSellerDetailsBtn .getSellerDetails");
            history.pushState('valuation Pop Up', '', '');
            carDetails.Valuation.valuationResultSectionObj.animate({
                "left": 0
            });
            carDetails.Valuation.loadingIconObj.show();
            $('.valuation-result-content').load($(this).data('href'), function (response, status) {
                carDetails.Valuation.loadingIconObj.hide();
                Common.utils.lockPopup();
                addMarginBottomToGSDBtn('.getSellerDetailsBtn');
                carDetails.Valuation.trackClick(valuationLink);
                if (status == 'error') {
                    $('.valuation-result-content').html("Something went wrong. Please try again later");
                }
                // transfer gsd button attributes to valution popup
                var gsdButtonValuation = $("#valuation-seller-details #getsellerDetails");
                if (gsdButton.attr('profileId') && gsdButtonValuation) {
                    gsdButton.each(function () {
                        $.each(this.attributes, function () {
                            var name = this.name;
                            if (name != 'class' && name != 'data-bind') {
                                gsdButtonValuation.attr(name, this.value);
                            }
                        });
                    });
                    gsdButtonValuation.attr('oid', m_bp_process.originId.detailsPageRightPrice);
                    // change text of button if user is verified
                    if (typeof m_bp_additonalFn != 'undefined')
                        m_bp_additonalFn.sellerDetailsBtnTextChange();
                } else {
                    gsdButtonValuation.hide();
                }
            });
        },
        triggerPopUpBackArrow: function () {
            $('.valuation-result-content').empty();
            carDetails.Valuation.valuationResultSectionObj.animate({
                "left": $(document).width()
            });
            Common.utils.unlockPopup();
        },
        setPopup: function () {
            carDetails.Valuation.valuationResultSectionObj.css('left', $(document).width() + 'px').show();
        },
        resizePopUp: function () {
            if (carDetails.Valuation.valuationResultSectionObj.css('left') != '0px')
                carDetails.Valuation.valuationResultSectionObj.css('left', $(document).width() + 'px');
        },
        trackClick: function (valuationLink) {
            var trackingParam = {};
            trackingParam['profileId'] = valuationLink.attr('profileid');
            trackingParam['caseId'] = $(".right-price-box").attr("caseid");
            var rightPriceTrackingData = cwTracking.prepareLabel(trackingParam);
            cwTracking.trackCustomData(carDetails.trackingCategory, 'RightPriceClick', rightPriceTrackingData, true);
        }
    }
};

$(document).ready(function () {
    addMarginBottomToGSDBtn('#getSellerDetailsBtn');
    if (document.referrer && new URL(document.referrer).hostname === location.hostname) { //condition for showing back button
        $(".header-fixed-white").height($(".detail-ui-corner-top").height());
    }

    ko.applyBindings(SIMILARCARSLISTING, document.getElementById("detailsPageSimilarCars"));
    $.coachmarkcookie();
    carDetails.pageLoad();
    $(window).on('resize', carDetails.WindowEventHandler.resizeEvent);
    if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
        $(".callBtn").attr('target', '_blank');
    }
    $('div.loginCloseBtn').trigger('click');
    sellerDetailsBtnTextChange();

    $('#oc_getSellerDetails').on('click', function () {
        var node = $(this);
        if ($('div.seller-inline-content').length <= 0)
            $('#getsellerDetails').trigger('click');
        else
            window.location.hash = "buyerProcessSlider";
    });
    $('div.detailPageCircle').on('click', function () {
        $('#getsellerDetails').trigger('click');
    });
    // scroll js
    $(window).on('scroll', function () {
        scrollPosition = $(this).scrollTop();
        if (scrollPosition + $(window).height() > $('footer').offset().top) {
            $('.getSellerDetailsBtn').addClass('float');
        }
        else {
            $('.getSellerDetailsBtn').removeClass('float');
        }
    });
    var absDiv = $(".absCondition");
    for (var i = 0; i < absDiv.length; i++) {
        if ($(absDiv[i]).attr("CategoryPercentage") <= 50) {
            $(absDiv[i]).addClass('red');
            $(absDiv[i]).html('Poor');
        }
        else if ($(absDiv[i]).attr("CategoryPercentage") <= 75) {
            $(absDiv[i]).addClass('yellow');
            $(absDiv[i]).html('Good');
        }
        else if ($(absDiv[i]).attr("CategoryPercentage") <= 100) {
            $(absDiv[i]).addClass('green');
            $(absDiv[i]).html('Excellent');
        }
    }

    $('#imageContainer').click();

    //Overview, Features & Condition tabs 
    $(".cw-m-tabs-three li").click(function () {
        $(".cw-m-tabs-three-data").hide();
        $(".cw-m-tabs-three-data").eq($(this).index()).show();
        $(".cw-m-tabs-three li").removeClass("active-tab");
        $(".cw-m-tabs-three li").find(".cw-m-sprite").removeClass("tab-pointer-blue");
        $(this).addClass("active-tab");
        $(this).find(".cw-m-sprite").addClass("tab-pointer-blue");
    });

    $(".req-photo-btn").click(function () {
        if (_isDealer == "True")
            isDealer = "1";
        else
            isDealer = "0";
        $("#reqImgProcess").show();
        $.ajax({
            type: "POST",
            url: "/ajaxpro/MobileWeb.Ajax.Used,Carwale.ashx",
            data: '{"profileId":"' + _profileId + '","isDealer":"' + isDealer + '","inquiryId":"' + _inquiryId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessUsedCarPhotoRequest"); },
            success: function (response) {
                var res = eval('(' + response + ')');
                var retVal = res.value;
                if (retVal.toString() == "true") {
                    $("#reqImgProcess").hide();
                    $(".req-photo-btn").hide();
                    $(".reqImgProcess").hide();
                    $(".seller-req-msg").show();
                }
            }
        });
    });

    //Image Gallery Count on click of Next & Prev buttons
    var count = 1
    var numImgs = $(".m-carousel-inner img").length

    $("#imgCount").text("image" + " " + count + " " + "of" + " " + numImgs);

    $(".m-carousel-next").click(function (e) {
        $(".m-carousel-prev").show();
        if (count >= numImgs) {
            $(".m-carousel-next").hide();
        }
        else {
            count++;
            $("#imgCount").text("image" + " " + count + " " + "of" + " " + numImgs);
        }
    });

    $(".m-carousel-prev").click(function () {
        $(".m-carousel-next").show();
        if (count <= 1) {
            $(".m-carousel-prev").hide();
        }
        else {
            count--;
            $("#imgCount").text("image" + " " + count + " " + "of" + " " + numImgs);
        }
    });

    //Image Gallery Count on swipe of image
    $(".m-carousel img").on("swipeleft", function () {
        $(".m-carousel-prev").show();
        if (count >= numImgs) {
            $(".m-carousel-next").hide();
        }
        else {
            count++;
            $("#imgCount").text("image" + " " + count + " " + "of" + " " + numImgs);
        }
    });

    $(".m-item img").on("swiperight", function () {
        $(".m-carousel-next").show();
        if (count <= 1) {
            $(".m-carousel-prev").hide();
        }
        else {
            count--;
            $("#imgCount").text("image" + " " + count + " " + "of" + " " + numImgs);
        }
    });

    $(document).on("mastercitychange", function (event, cityName, cityId) {
        $("#m-blackout-window").show();
        $("#cwmLoadingIcon").show();
        window.location.href = "/m/used/cars-for-sale/?city=" + cityId + "&budget=0-&year=0-&kms=0-&so=-1&sc=-1&pn=1";
    });

    $(window).scroll(function () {
        if (!$('#buyerForm').is(':visible') && scrollPosition + $(window).height() > $(".cw-tabs.cw-tabs-flex >ul> li.active").offset().top && !isSimilarCarsFetched) {
            isSimilarCarsFetched = true;
            fetchSimilarCars($('a.getSellerDetails').attr('stockRecommendationUrl'));
            fetchTyreData($('a.getSellerDetails').attr('versionId'));
        }
    });

    var isChatSms = commonUtilities.getFilterFromQS("ischatsms", window.location.search);
    if (isChatSms) {
        $('.chat-btn').trigger('click');
    }
    if (typeof cwUsedTracking !== 'undefined') {
        cwUsedTracking.setEventCategory(cwUsedTracking.eventCategory.UsedDetailsPage);
    }

}); // end of document.ready

function trackingCarCategory() {
    if (_hasAbsureWarranty && _absureWarrantyUrl != "")   // Guarantee Car
        dataLayer.push({ event: 'CarWaleGuarantee_Inspected', cat: 'MSite_UsedDetailsPage', act: 'CarWaleGuarantee_DetailsViewed' });
    else if (_hasAbsureWarranty && _absureWarrantyUrl == "")  // Inspected Car
        dataLayer.push({ event: 'CarWaleGuarantee_Inspected', cat: 'MSite_UsedDetailsPage', act: 'CarWaleInspected_DetailsViewed' });
}
function trackingMaskingNumberClicks() {
    $('#oc_MaskingNumber').on("click", function () {    //Owner's Comment
        dataLayer.push({ event: 'MaskingNumberClicked', cat: 'MSite_UsedDetailsPage', act: 'MSiteDetailsPage_MaskingNumberClicked', lab: 'OwnersCommment' });
    });

    $('#ad_MaskingNumber').on("click", function () {    //About Dealer
        dataLayer.push({ event: 'MaskingNumberClicked', cat: 'MSite_UsedDetailsPage', act: 'MSiteDetailsPage_MaskingNumberClicked', lab: 'AboutDealer' });
    });

    $('#cert_MaskingNumber').on("click", function () {    //Certification Slug
        dataLayer.push({ event: 'MaskingNumberClicked', cat: 'MSite_UsedDetailsPage', act: 'MSiteDetailsPage_MaskingNumberClicked', lab: 'Certification' });
    });

    $('.rsaMaskingNumber').on("click", function () {    //RSA
        dataLayer.push({ event: 'MaskingNumberClicked', cat: 'MSite_UsedDetailsPage', act: 'MSiteDetailsPage_MaskingNumberClicked', lab: 'RSA' });
    });

    $('.dealerWarrantyMaskNumber').on("click", function () {    //Dealer Warranty
        dataLayer.push({ event: 'MaskingNumberClicked', cat: 'MSite_UsedDetailsPage', act: 'MSiteDetailsPage_MaskingNumberClicked', lab: 'DealerEnteredWarranty' });
    });
}

// this function add margin bottom to GSD Button (only for UC)
function addMarginBottomToGSDBtn(selector) {
    if (navigator.userAgent.match(/UCBrowser/i) && $.cookie('viewPortHeight')) {
        var floatBtnBottomValue = Math.abs($.cookie('viewPortHeight') - $(window).height());
        $(selector).css('bottom', floatBtnBottomValue);
        $('#buyerForm').css('bottom', floatBtnBottomValue);
    }
}

// function to changed the text of Get Seller Details Button to View Seller Details once the user verifies his credentials
function sellerDetailsBtnTextChange() {
    if ($.cookie('TempCurrentUser') != null) {
        $(".getSimilarCarSellerDetails").addClass('hideImportant');
        $(".oneClickDetails").removeClass('hideImportant');
    }
}

function BoxClicked(box) {
    HandleBoxClicked($(box));
}

function HandleBoxClicked(box) {
    var divIcon = box.find("div:nth-child(2)");
    if (divIcon.attr("class").toString() == "plus") {
        divIcon.attr("class", "minus");
        box.next().show();
        box.addClass("bot-rad-0");
    }
    else {
        divIcon.attr("class", "plus");
        box.next().hide();
        box.removeClass("bot-rad-0");
    }
}

function CloseWindow() {
    $("#divOverlay").hide();
    $("#divWindow").hide();
}

function getTopPos() {
    return getTopResults(window.pageYOffset ? window.pageYOffset : 0, document.documentElement ? document.documentElement.scrollTop : 0, document.body ? document.body.scrollTop : 0);
}

function getTopResults(n_win, n_docel, n_body) {
    var n_result = n_win ? n_win : 0;
    if (n_docel && (!n_result || (n_result > n_docel)))
        n_result = n_docel;
    return n_body && (!n_result || (n_result > n_body)) ? n_body : n_result;
}

function ShowLargePhotos(_thumbDiv) {
    fullUrl = $(_thumbDiv).attr("fullUrl").toString();

    var thumbDivs = $("#divCarPhotos .thumbDiv");
    totalThumbDivs = thumbDivs.length;

    currentIndex = 0;
    var i = 0;

    thumbDivs.each(function () {
        if ($(this).attr("fullUrl").toString() == fullUrl)
            currentIndex = i;
        else
            i++;
    });

    $("#divPhotosOverlay").attr("style", "position:absolute;top:0;left:0;width:100%;height:" + (parseInt($("#divCarPhotos").height()) + 10) + "px;z-index;1000;background-color:black;filter:alpha(opacity=80);opacity:0.80;background-image: url('/m/images/loader.gif');background-position:center center;background-repeat:no-repeat;");
    LoadLargePhoto();
}

function LoadLargePhoto() {
    var imgLarge = $(document.createElement("img"));
    imgLarge.attr("src", fullUrl)
    imgLarge.attr("style", "height: auto !important;max-width: 100% !important;width: 100%;");
    imgLarge.bind("load", function () {
        setTimeout(function () {
            $("#divLargeImg").html("");
            imgLarge.appendTo("#divLargeImg");
            $("#divPhotosOverlay").hide();
            $("#divCarPhotos").hide();
            $("#divLargeImgContainer").show();
            $("#divLargeImgContainer .prev").show();
            $("#divLargeImgContainer .next").show();
            if (currentIndex == 0)
                $("#divLargeImgContainer .prev").hide();

            if (currentIndex == (parseInt(totalThumbDivs) - 1))
                $("#divLargeImgContainer .next").hide();
        }, 2000);
    });
}

function NextClicked() {
    currentIndex++;
    fullUrl = $("#divCarPhotos .thumbDiv").eq(currentIndex).attr("fullUrl").toString();
    $("#divPhotosOverlay").attr("style", "position:absolute;top:0;left:0;width:100%;height:" + (parseInt($("#divLargeImgContainer").height()) + 20) + "px;z-index;1000;background-color:black;filter:alpha(opacity=80);opacity:0.80;background-image: url('/m/images/loader.gif');background-position:center center;background-repeat:no-repeat;");
    LoadLargePhoto();
}

function PrevClicked() {
    currentIndex--;
    fullUrl = $("#divCarPhotos .thumbDiv").eq(currentIndex).attr("fullUrl").toString();
    $("#divPhotosOverlay").attr("style", "position:absolute;top:0;left:0;width:100%;height:" + (parseInt($("#divLargeImgContainer").height()) + 20) + "px;z-index;1000;background-color:black;filter:alpha(opacity=80);opacity:0.80;background-image: url('/m/images/loader.gif');background-position:center center;background-repeat:no-repeat;");
    LoadLargePhoto();
}

function ShowThumbnails() {
    $("#divPhotosOverlay").hide();
    $("#divLargeImgContainer").hide();
    $("#divCarPhotos").show();
}
$('#frontImage').on('click', function () {
    if ($('#imageContainer').children().hasClass('plus')) {
        $('#imageContainer').click();
    }
});
$(document).on("citypopped", function () {
    $(".m-city-selection-pop").css('display', 'none');
    $("#m-blackOut-window").css('display', 'none');
    $("html,body").removeClass("lock-browser-scroll");
});

changeCityIconShow = false;
function triggerTrackingCode(cat, act) {
    dataLayer.push({ event: 'CWGuaranteeOffer', cat: cat, act: act });
}

function scrollToElement(ele) {
    $('html,body').animate({
        scrollTop: ele.offset().top
    },
    'slow');
}

$.coachmarkcookie = function () {
    if (isCookieExists('mUsedCarsCoachmark1')) {
        var cookie = $.cookie('mUsedCarsCoachmark1');
        if (cookie.indexOf('details') == -1) {
            SetCookieInDays('mUsedCarsCoachmark1', $.cookie('mUsedCarsCoachmark1') + "details", 30);
            $.showCoachMark('detailsBtnCoachmark');
        }
    }
    else {
        SetCookieInDays('mUsedCarsCoachmark1', "details|", 30);
        $.showCoachMark('detailsBtnCoachmark');
        $('div.detailPageCircle').addClass('hide');
    }
};

$.showCoachMark = function (element) {
    switch (element) {
        case 'detailsBtnCoachmark':
            showDetailsBtnCoachmark();
            break;
        case 'CoachMarkCallBtn':
            showCoachMarkCallBtn();
            break;
        case 'hideCoachmarkCallBtn':
            hideCoachmarkCallBtn();

    };
};

function showDetailsBtnCoachmark() {
    var detailCoachmark = $('.det-new-coachmark');
    var callBtnCoachmark = $('.det-call-btn-pos');
    if ($("#callNowConainerDetails").is(":visible") == false) {
        $('#detNewcm').html('Got it');
        $('#detNoMoreTips').css("display", "none");
    }
    detailCoachmark.removeClass('hide').css('display', 'block');
    $('#detNewcm').click(function () {
        $.showCoachMark('CoachMarkCallBtn');
    });

};

function showCoachMarkCallBtn() {
    $('.det-new-coachmark').css('display', 'none');
    $('.det-call-btn-pos').removeClass('hide').css('display', 'block');
    $('#detCallcm').click(function () {
        $.showCoachMark('hideCoachmarkCallBtn');
    });
};

function hideCoachmarkCallBtn() {
    $('.det-call-btn-pos').css('display', 'none');
};

$('.nomoreTips').click(function () {
    $('.coachmark').css('display', 'none');
});

function fetchSimilarCars(url) {
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) { xhr.setRequestHeader('sourceid', '1'); },
        success: function (response) {
            SIMILARCARSLISTING(response);
            setTimeout(function () {
                $(document).find('.similar-car-carousel').trigger("similarSwiperEvent");
                similarSwiper();
                $("#tyre-swiper-pagination").addClass("swiper-pagination");
            }, 1000);
        }
    });
}
function fetchTyreData(versionId) {

    var url = '/tyrelist/'+ versionId + '/tyres?makeyear=' + MakeYear + '&pagesize=4';
    $.when(Common.utils.ajaxCall(url)).done(function (data) {
        if (data != null && $.trim(data) !== "") {
            $('#tyresCarousal').append(data);
            tyreSwiper();
        }
    });
};
function similarSwiper() {
    $('.similar-car-carousel').swiper({
        nextButton: $(document).find('.swiper-button-next'),
        prevButton: $(document).find('.swiper-button-prev'),
        pagination: $(document).find('.swiper-pagination'),
        slidesPerView: 'auto',
        paginationClickable: true,
        spaceBetween: 10,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        onInit: function (swiper) { $(window).resize(function () { swiper.update(true); }) }
    });

};
function tyreSwiper() {
    $('.tyre-alternative-carousel').swiper({
        nextButton: $(document).find('.tyre-swiper-button-next'),
        prevButton: $(document).find('.tyre-swiper-button-prev'),
        pagination: $(document).find('#tyre-swiper-pagination'),
        slidesPerView: 'auto',
        paginationClickable: true,
        spaceBetween: 10,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        onInit: function (swiper) { $(window).resize(function () { swiper.update(true); }) }
    });
};
function getFilterFromQSWithURL(name) {
    var qs = location.search.replace('?', '');
    var params = qs.split('&');
    var result = {};
    var propval, filterName, value;
    var isFound = false;
    for (var i = 0; i < params.length; i++) {
        var propval = params[i].split('=');
        filterName = propval[0];
        if (filterName == name) {
            value = propval[1];
            isFound = true;
            break;
        }
    };
    if (isFound && value.length > 0) {
        if (value.indexOf('+') > 0)
            return value.replace(/\+/g, " ");
        else
            return value;
    }
    else
        return "";
}

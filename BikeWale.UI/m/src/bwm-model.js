var imgTitle, imgTotalCount, getOffersClicked = false, popupDiv, gallery;
var bodHt, footerHt, scrollPosition, selectDropdown;

var dealersPopupDiv, dealerOffersDiv, termsConditions;
var dropdown;
var window, overallSpecsTabsContainer, modelSpecsTabsContentWrapper, modelSpecsFooter, topNavBarHeight;
var backToTopBtn, halfBodyHeight, overViewContentHeight;
var lastScrollTop = 0;
var reg, vmUserReviews;

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

var viewBreakUpClosePopup = function () {
    $("div#breakupPopUpContainer").hide();
    $(".blackOut-window").hide();
    $("#contactDetailsPopup").show();
    leadPopupClose();
};

function secondarydealer_Click(dealerID) {
    try {
        var isSuccess = false;

        var objData = {
            "dealerId": dealerID,
            "modelId": bikeModelId,
            "versionId": versionId,
            "cityId": cityId,
            "areaId": areaId,
            "clientIP": clientIP,
            "pageUrl": pageUrl,
            "sourceType": 2,
            "pQLeadId": pqSourceId,
            "deviceId": getCookie('BWC'),
            "refPQId": typeof pqId != 'undefined' ? pqId : '',
        };

        isSuccess = dleadvm.registerPQ(objData);

        if (isSuccess) {
            var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + dleadvm.pqId() + "&VersionId=" + versionId + "&DealerId=" + dealerID;
            window.location.href = "/m/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
        }
    } catch (e) {
        console.warn("Unable to create pricequote : " + e.message);
    }
}

function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $('#termspinner').show();
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerId=" + offerId,
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
    }
    $('#termspinner').hide();
}

function scrollHorizontal(pos) {
    $('#overallSpecsTab').animate({ scrollLeft: pos - 15 + 'px' }, 500);
}

var appendState = function (state) {
    window.history.pushState(state, '', '');
};

docReady(function () {
    // ad blocker active than fallback method
    if (window.canRunAds === undefined) {
        callFallBackWriteReview();
    }
   var isMileageSectionVisible = $('#mileageContent');
    if (isMileageSectionVisible.length > 0) {
        triggerNonInteractiveGA("Model_Page", "Mileage_Card_Shown", myBikeName);
    }
    function callFallBackWriteReview() {
        $('#adBlocker').show();
        $('.sponsored-card').hide();
    };
    dealersPopupDiv = $('#more-dealers-popup'),
    dealerOffersDiv = $('#dealer-offers-popup'),
    termsConditions = $('#termsPopUpContainer');
    selectDropdown = $('.dropdown-select');

    navigationVideosLI = $(".carousel-navigation-videos .swiper-slide");

    $window = $(window),
	overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
	modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper'),
	modelSpecsFooter = $('#modelSpecsFooter'),
	topNavBarHeight = $('#modelOverallSpecsTopContent').height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
    if (tabsLength < 2) {
        $('.overall-specs-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
    }

    $('.overall-specs-tabs-wrapper li').first().addClass('active');

    var makeDealersContent = $('#makeDealersContent');

    if (makeDealersContent.length != 0) {
        makeDealersContent.removeClass('bw-model-tabs-data');
    }

    var tabElementThird = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(3)'),
        tabElementSixth = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(5)'),
        tabElementNinth = modelSpecsTabsContentWrapper.find('.bw-model-tabs-data:eq(8)');

    $("#viewprimarydealer, #dealername").on("click", function () {
        var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId + "&IsDealerAvailable=true";
        window.location.href = "/m/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
    });
    backToTopBtn = $('#scroll-to-top');
    overViewContentHeight = $('#overviewContent').height();
    halfBodyHeight = $('body').height() / 2;

    $(".leadcapturebtn").click(function (e) {
        ele = $(this);
        try {
            var leadOptions = {
                "dealerid": ele.attr('data-item-id'),
                "dealername": ele.attr('data-item-name'),
                "dealerarea": ele.attr('data-item-area'),
                "versionid": versionId,
                "leadsourceid": ele.attr('data-leadsourceid'),
                "pqsourceid": ele.attr('data-pqsourceid'),
                "isleadpopup": ele.attr('data-isleadpopup'),
                "mfgCampid": ele.attr('data-mfgcampid'),
                "pqid": pqId,
                "pageurl": pageUrl,
                "clientip": clientIP,
                "dealerHeading": ele.attr('data-item-heading'),
                "dealerMessage": ele.attr('data-item-message'),
                "dealerDescription": ele.attr('data-item-description'),
                "pinCodeRequired": ele.attr("data-ispincodrequired"),
                "emailRequired": ele.attr("data-isemailrequired"),
                "dealersRequired": ele.attr("data-dealersrequired"),
                "eventcategory": "Model_Page",
                "gaobject": {
                    cat: ele.attr("data-cat"),
                    act: ele.attr("data-act"),
                    lab: ele.attr("data-var")
                }
            };

            gaLabel = myBikeName + '_' + getCityArea;
            dleadvm.setOptions(leadOptions);
        } catch (e) {
            console.warn("Unable to get submit details : " + e.message);
        }

    });


    $("#templist input").on("click", function () {
        if ($(this).attr('data-option-value') == $('#hdnVariant').val()) {
            return false;
        }
        $('.dropdown-select-wrapper #defaultVariant').text($(this).val());
        $('#hdnVariant').val($(this).attr('data-option-value'));
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersionLocation });
    });

    if ($('#getMoreDetailsBtn').length > 0) {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Shown', 'lab': myBikeName + '_' + getBikeVersion() + '_' + getCityArea });
    }
    if ($('#btnGetOnRoadPrice').length > 0) {
        dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': 'Model_Page', 'act': 'Get_On_Road_Price_Button_Shown', 'lab': myBikeName + '_' + getBikeVersion() + '_' + getCityArea });
    }
    if ($("#getAssistance").length > 0) {
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_Offers_Shown", "lab": myBikeName + "_" + getBikeVersion() + '_' + getCityArea });
	}
	if ($('#expertReviewsContent').length > 0) {
		dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Expert_Review_CardShown", "lab": myBikeName });
	}


    if (bikeVersionLocation == '') {
        bikeVersionLocation = getBikeVersionLocation();
    }
    if (bikeVersion == '') {
        bikeVersion = getBikeVersion();
    }

    getCityArea = GetGlobalCityArea();

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
            var top = $(this).offset().top - topNavBarHeight,
                bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                overallSpecsTabsContainer.find('li').removeClass('active');
                $('#modelSpecsTabsContentWrapper .bw-model-tabs-data').removeClass('active');

                $(this).addClass('active');

                var currentActiveTab = overallSpecsTabsContainer.find('li[data-tabs="#' + $(this).attr('data-id') + '"]');
                overallSpecsTabsContainer.find(currentActiveTab).addClass('active');
            }
        });

        if (tabElementThird.length != 0) {
            focusFloatingTab(tabElementThird, 250, 0);
        }

        if (tabElementSixth.length != 0) {
            focusFloatingTab(tabElementSixth, 500, 250);
        }

        if (tabElementNinth.length != 0) {
            focusFloatingTab(tabElementNinth, 750, 500);
        }

        function focusFloatingTab(element, startPosition, endPosition) {
            if (windowScrollTop > element.offset().top - 45) {
                if (!$('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left-' + startPosition);
                    scrollHorizontal(startPosition);
                }
            }

            else if (windowScrollTop < element.offset().top) {
                if ($('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left-' + startPosition);
                    scrollHorizontal(endPosition);
                }
            }
        };

        if (windowScrollTop > halfBodyHeight) {
            if (windowScrollTop < lastScrollTop) {
                backToTopBtn.fadeIn();
            }
        }
        lastScrollTop = windowScrollTop;

        if (windowScrollTop < overViewContentHeight) {
            backToTopBtn.fadeOut();
        };

    });

    $('#ddlNewVersionList').on("change", function () {
        $('#hdnVariant').val($(this).val());
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Version_Change", "lab": "" });
        window.location.href = $(this).data("pageurl") + "?versionId=" + $(this).val();
    });

    $('.overall-specs-tabs-wrapper li').click(function () {
        var target = $(this).attr('data-tabs');
        $('html, body').animate({ scrollTop: $(target).offset().top - topNavBarHeight }, 1000);
        centerItVariableWidth($(this), '.overall-specs-tabs-container');        
    });

    // dropdown
    dropdown = {
        setDropdown: function () {
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
                    menuList.append('<li id="temp_' + index + '" data-option-value="' + $(this).val() + '" title="' + $(this).text() + '">' + $(this).text() + '</li>');
                }
                else {
                    menuList.append('<li data-option-value="' + $(this).val() + '" title="' + $(this).text() + '">' + $(this).text() + '</li>');
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
            var elementText = element.find('input[type="submit"]').val() || element.text(),
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
            $('.dropdown-select-wrapper').find('.dropdown-list-wrapper').css('width', newWidth / 2);
        }
    };

    reg = new RegExp('^[0-9]*$');

    vmUserReviews = new modelUserReviews();

    var gallerySwiper = new Swiper('#similar-mileage-swiper', {
        effect: 'slide',
        speed: 300,
        slidesPerView: 'auto',
        spaceBetween: 10,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        onSlideChangeStart: function (swiper, event) {
            triggerGA("Model_Page", "Clicked_On_SimilarMileage_Carousel", myBikeName);
        }
    });

});

docReady(function () {

    bwcache.setOptions({ 'EnableEncryption': true });

    var userEventSource = true;

    var gallerySwiper = new Swiper('#model-photos-swiper', {
        spaceBetween: 0,
        direction: 'horizontal',
        nextButton: '.gallery-type-next',
        prevButton: '.gallery-type-prev',
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        onSlideChangeEnd: function (swiper) {
            if (userEventSource) {
                if (swiper.activeIndex < swiper.previousIndex) {
                    triggerGA('Model_Page', 'Swiped_Left', myBikeName);
                } else if (swiper.activeIndex > swiper.previousIndex) {
                    triggerGA('Model_Page', 'Swiped_Right', myBikeName);
                }
            }
            else {
                if (swiper.activeIndex < swiper.previousIndex) {
                    triggerGA('Model_Page', 'Image_Carousel_Clicked', myBikeName + '_Previous');

                } else if (swiper.activeIndex > swiper.previousIndex) {
                    triggerGA('Model_Page', 'Image_Carousel_Clicked', myBikeName + '_Next');
                }
            }

            logBhrighuForImage($('#model-photos-swiper .swiper-slide-active'));

        },
        onTouchEnd: function (swiper, event) {
            var targetId = event.target.id;
            if (targetId == "next-btn" || targetId == "prev-btn") {
                userEventSource = false;
            }
            else {
                userEventSource = true;
            }
        },
    });

    if (photosCount > 0) {
        var overlayCount = '<span class="black-overlay text-white"><span class="font16 text-bold">+' + photosCount + '</span><br><span class="font14">images</span></span>';
        $("#model-photos-swiper .swiper-slide").last().find("a").append(overlayCount);
    }

    popupDiv = {
        open: function (div) {
            div.show();
        },

        close: function (div) {
            div.hide();
            $('body, html').removeClass('lock-browser-scroll');
        }
    };
    gallery = {
        open: function () {
            lockPopup();
            $('.model-gallery-container').show();
            $('body').addClass('gallery-popup-active');
        },

        close: function () {
            unlockPopup();
            $('.model-gallery-container').hide();
            $('body').removeClass('gallery-popup-active');
        }
    };

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
			$('html, body').animate({ scrollTop: $('#aboutContent').offset().top - 44 }, 500)
        }
    });

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

    $('#termsPopUpCloseBtn ').on("click", function () {
        popupDiv.close(termsConditions);
        popupDiv.open(dealerOffersDiv);
        window.history.back();
    });

    $(document).ready(function () {
        if (versionsCount > 1) {
            $('#defversion').hide();
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


    $(window).on('popstate', function (event) {
        if ($('.model-gallery-container').is(':visible')) {
            gallery.close();
        }
    });

    $('#getMoreDetailsBtn,#getAssistance').on('click', function (e) {
        leadSourceId = $(this).attr("leadSourceId");
        $("#leadCapturePopup").show();
        $(".blackOut-window").show();
        appendHash("contactDetails");

        if ($(this).attr("id") == "getAssistance") {
            dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Get_Offers_Clicked", "lab": bikeVersionLocation });
            getOffersClicked = true;
        }
    });


    $("#viewBreakupText").on('click', function (e) {
        triggerGA('Model_Page', 'View_Detailed_Price_Clicked', bikeVersionLocation);
        secondarydealer_Click(dealerId);
    });

    $(".termsPopUpCloseBtn,.blackOut-window").on('click', function (e) {
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


    $("#btnShowOffers").on("click", function () {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Show_Offers_Clicked', 'lab': myBikeName });
    });

    $(".viewMoreOffersBtn").on("click", function () {
        $(this).hide();
        $("ul.moreOffersList").slideToggle();
    });

    $("input[name*='btnVariant']").on("click", function () {
        if ($(this).attr('versionid') == $('#hdnVariant').val()) {
            return false;
        }
        $('#hdnVariant').val($(this).attr('versionid'));
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersionLocation });
    });

    // GA Tags
    $('#btnGetOnRoadPrice').on('click', function (e) {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Check_On_Road_Price_Clicked", "lab": bikeVersionLocation });
    });

    $("#btnDealerPricePopup").on("click", function () {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Show_On_Road_Price_Clicked", "lab": bikeVersionLocation });
    });


    $('.tnc').on('click', function (e) {
        appendHash("termsConditions");
        LoadTerms($(this).attr("id"));
        popupDiv.close(dealerOffersDiv);
    });

    $('.changeCity').on('click', function (e) {
        try {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'City_Change_Initiated', 'lab': bikeVersionLocation });
        }
        catch (err) { }
    });

    $(window).scroll(function () {
        bodHt = $('body').height();
        footerHt = $('footer').height();
        scrollPosition = $(this).scrollTop();
        if (scrollPosition + $(window).height() > (bodHt - footerHt))
            $('.floating-btn').hide();
        else
            $('.floating-btn').show();
    });

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

    $('#model-specs-list').on('click', '.model-accordion-tab', function () {
        var tab = $(this),
            allTabs = $('#model-specs-list .model-accordion-tab');

        if (!tab.hasClass('active')) {
            allTabs.removeClass('active');
            tab.addClass('active');
            $('html, body').animate({ scrollTop: tab.offset().top - 44 }, 500);
        }
        else {
            tab.removeClass('active');
        }
    });

    $('.view-features-link').on('click', function () {
        var target = $(this),
            featuresHeading = $('#model-features-heading'),
            moreFeatures = $('#model-more-features-list');

        if (!target.hasClass('active')) {
            target.addClass('active');
            $('html, body').animate({ scrollTop: featuresHeading.offset().top - 44 }, 500);
            moreFeatures.slideDown();
            target.text('Collapse');
        }
        else {
            target.removeClass('active');
            $('html, body').animate({ scrollTop: featuresHeading.offset().top - 44 }, 500);
            moreFeatures.slideUp();
            target.text('View all features');
        }
    });

    $('.view-cities-link').on('click', function () {
        $('#more-cities-list').show();
        $(this).closest('div').hide();
    });

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

    // tooltip
    $('.bw-tooltip').on('click', '.close-bw-tooltip', function () {
        var tooltipParent = $(this).closest('.bw-tooltip');

        tooltipParent.slideUp();
    });

    $('#scroll-to-top').click(function (event) {
        $('html, body').stop().animate({ scrollTop: 0 });
        event.preventDefault();
    });

    $('#report-background, .report-abuse-close-btn').on('click', function () {
        reportAbusePopup.close();
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 27) {
            if (reportAbusePopup.popupElement.is(':visible')) {
                reportAbusePopup.close();
            }
        }
    });

    var reportAbusePopup = {
        popupElement: $('#report-abuse'),

        bgContainer: $('#report-background'),

        open: function () {
            reportAbusePopup.popupElement.show();
            popup.lock();
            $(".blackOut-window").hide();
            reportAbusePopup.bgContainer.show();
        },
        close: function () {
            reportAbusePopup.popupElement.hide();
            popup.unlock();
            reportAbusePopup.bgContainer.hide();
        }
    };

    applyLikeDislikes();

    if (document.getElementById("modelsBySeriesWidget")) {
        try {
            var seriesEle = $("#modelsBySeriesWidget");
            triggerNonInteractiveGA(seriesEle.attr("data-cat"), seriesEle.attr("data-act"), seriesEle.attr("data-lab"));
        } catch (e) {

        }
    }

	$("#expertReviewsContent").on('click', function () {
		triggerGA('Model_Page', 'Expert_Review_CardClicked', myBikeName);
	});

}
);

function upVoteListReview(e) {
    try {
        var localReviewId = e.currentTarget.getAttribute("data-reviewid");
        if (!$('#upvoteBtn' + "-" + localReviewId).hasClass('active')) {
            bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "1" });
            $('#upvoteBtn' + "-" + localReviewId).addClass('active');
            $('#downvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');

            if (reg.test($('#upvoteCount' + "-" + localReviewId).text()))
                $('#upvoteCount' + "-" + localReviewId).text(parseInt($('#upvoteCount' + "-" + localReviewId).text()) + 1);

            voteListUserReview(1, localReviewId);
        }
    } catch (e) {
        console.warn(e);
    }
}

function downVoteListReview(e) {
    try {
        var localReviewId = e.currentTarget.getAttribute("data-reviewid");
        bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "0" });
        $('#downvoteBtn' + "-" + localReviewId).addClass('active');
        $('#upvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');

        if (reg.test($('#downvoteCount' + "-" + localReviewId).text()))
            $('#downvoteCount' + "-" + localReviewId).text(parseInt($('#downvoteCount' + "-" + localReviewId).text()) + 1);

        voteListUserReview(0, localReviewId);
    } catch (e) {
        console.warn(e);
    }
}

function voteListUserReview(vote, locReviewId) {
    try {
        $.ajax({
            type: "POST",
            url: "/api/user-reviews/voteUserReview/?reviewId=" + locReviewId + "&vote=" + vote,
            success: function (response) {
            }
        });
    } catch (e) {
        console.warn(e);
    }
}

function applyLikeDislikes() {
    try {
        $(".upvoteListButton").each(function () {
            var locReviewId = this.getAttribute("data-reviewid");
            var listVote = bwcache.get("ReviewDetailPage_reviewVote_" + locReviewId);

            if (listVote != null && listVote.vote) {
                if (listVote.vote == "0") {
                    $('#downvoteBtn' + "-" + locReviewId).addClass('active');
                    $('#upvoteBtn' + "-" + locReviewId).attr('disabled', 'disabled');
                }
                else {
                    $('#upvoteBtn' + "-" + locReviewId).addClass('active');
                    $('#downvoteBtn' + "-" + locReviewId).attr('disabled', 'disabled');
                }
            }
            else {
                $('#upvoteBtn' + "-" + locReviewId).removeClass('active');
                $('#downvoteBtn' + "-" + locReviewId).prop('disabled', false);
            }
        });
    } catch (e) {
        console.warn(e);
    }
}

ko.bindingHandlers.truncatedText = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        if (ko.utils.unwrapObservable(valueAccessor())) {
            var originalText = ko.utils.unwrapObservable(valueAccessor()),
                length = parseInt(element.getAttribute("data-trimlength")) || 20,
                truncatedText = originalText.length > length ? originalText.substring(0, length) + "..." : originalText;
            ko.bindingHandlers.text.update(element, function () {
                return truncatedText;
            });
        }
    }
};

function strip(html) {
    var tmp = document.createElement("div");
    tmp.innerHTML = html;
    return tmp.textContent || tmp.innerText || "";
}

ko.bindingHandlers.truncateDesc = {
    update: function (element, valueAccessor) {
        var originalText = strip(valueAccessor());
        var formattedText = originalText && originalText.length > 120 ? originalText.substring(0, 120) + '...' : originalText;
        $(element).text(formattedText);
    }
};

ko.bindingHandlers.formattedVotes = {
    update: function (element, valueAccessor) {
        try {
            var amount = valueAccessor();
            var formattedStringArray = (amount / 1000).toString().match(/\d+[.]+\d/);
            if (amount % 1000 == 0 && amount > 0) {
                var formattedVote = amount / 1000 + '.0k';
            }
            else {
                var formattedVote = ko.unwrap(amount) > 999 && formattedStringArray ? formattedStringArray[0] + 'k' : amount;
            }
            $(element).text(formattedVote);
        } catch (e) {
            console.warn(e);
        }
    }
};

$(document).on("click", ".read-more-target", function (e) {
    if (!vmUserReviews.IsInitialized()) {
        vmUserReviews.init(e);
    }
});

var modelUserReviews = function () {
    var self = this;
    self.reviewList = ko.observableArray(null);
    self.trimLengthText = ko.observable();
    self.isLoading = ko.observable(false);
    self.currentReviewList = ko.observableArray(null);
    self.IsInitialized = ko.observable(false);

    self.getMoreReviews = function () {
        try {
            if (bikeModelId) {
                var apiUrl = "/api/user-reviews/list/3/?reviews=true&pn=1&ps=5&so=1&model=" + bikeModelId;
                $.getJSON(apiUrl)
                .done(function (response) {
                    if (response && response.result) {
                        self.reviewList(response.result);
                        applyLikeDislikes();
                        $('.more-review-li').removeClass('hide');
                    }                    
                })
                .always(function () {
                    self.isLoading(false);
                });
            }
        } catch (e) {
            console.log(e);
        }
    };

    self.logBhrighuData = function (event, eventName) {
        var ele = $(event.currentTarget);
        var index = ele.data("index");
        logBhrighu(index, eventName);
        return true;
    };

    self.readMoreNew = function (event) {
        var ele = $(event.currentTarget);
        var reviewId = ele.data("reviewid");

        updateView(reviewId);

        var index = ele.data("index");

        logBhrighu(index, "ReadMoreClick");
        return true;
    };

    self.init = function (event) {
        if ($("#reviewsContent")[0])
            ko.applyBindings(vmUserReviews, $("#reviewsContent")[0]);

        self.readMore(event);
        self.IsInitialized(true);
        $('#loader').removeClass('hide');
    };

    self.readMore = function (event) {

        var ele = $(event.currentTarget);
        var reviewId = ele.data("reviewid");
        var itemNo = ele.data("id");

        if (!self.currentReviewList().length && bikeModelId) {
			var apiUrl = "/api/user-reviews/search/V2/?InputFilter.review=true&InputFilter.SO=1&InputFilter.PN=1&InputFilter.PS=3&ReviewFilter.RatingQuestion=true&ReviewFilter.ReviewQuestion=false&ReviewFilter.BasicDetails=false&InputFilter.Model=" + bikeModelId;
			
			$('#userReviewSpinner').show();
			
            $.getJSON(apiUrl)
            .done(function (response) {
                if (response && response.result) {
                    self.currentReviewList(response.result);
                }
            })
			.always(function () {
				$('#userReviewSpinner').hide();
			});
        }

        updateView(reviewId);
        logBhrighu(itemNo, "ReadMoreClick");

        if ($('#reviewsContent') && $('#reviewsContent').attr('data-readmore')) {
            $('#reviewsContent').attr('data-readmore', parseInt($('#reviewsContent').attr('data-readmore')) + 1);
        }

        if ($('#reviewsContent') && $('#reviewsContent').attr('data-readmore') == "3") {
            vmUserReviews.isLoading(true);
            vmUserReviews.getMoreReviews();
        }
        return true;
    };

}

function logBhrighu(itemNo, eventName) {
    label = 'modelId=' + bikeModelId + '|tabName=recent|reviewOrder=' + (++itemNo) + '|pageSource=' + $('#pageSource').val();
    cwTracking.trackUserReview(eventName, label);
}

function logBhrighuForImage(item) {
    if (item) {
        var imageid = item.attr("data-imgid"), imgcat = item.attr("data-imgcat"), imgtype = item.attr("data-imgtype");
        if (imageid) {
            var lb = "";
            if (imgcat) {
                lb += "|category=" + imgcat;
            }

            if (imgtype) {
                lb += "|type=" + imgtype;
            }

            label = 'modelId=' + bikeModelId + '|imageid=' + imageid + lb + '|pageid=' + (gaObj ? gaObj.id : 0);
            cwTracking.trackImagesInteraction("BWImages", "ImageViewed", label);
        }
    }

}

function updateView(reviewId) {
    try {
        if (reviewId) {
            $.post("/api/user-reviews/updateView/" + reviewId + "/");
        }

    } catch (e) {
        console.log(e);
    }
}




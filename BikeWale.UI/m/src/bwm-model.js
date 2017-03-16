var imgTitle, imgTotalCount, getOffersClicked = false, popupDiv, gallery;
var bodHt, footerHt, scrollPosition;
var sortByDiv, sortListDiv, sortCriteria, sortByDiv, sortListDiv, sortListLI;

var dealersPopupDiv, dealerOffersDiv, termsConditions;
var currentStagePhoto, currentStageActiveImage, dropdown, videosThumbs, galleryThumbs, slidegalleryThumbs, galleryTop, navigationVideosLI;
var videoiFrame = document.getElementById("video-iframe");
var window, overallSpecsTabsContainer, modelSpecsTabsContentWrapper, modelSpecsFooter, topNavBarHeight;

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

var sortChangeDown = function (sortByDiv) {
    sortByDiv.addClass("open");
    sortListDiv.show();
};

var sortChangeUp = function (sortByDiv) {
    sortByDiv.removeClass("open");
    sortListDiv.slideUp();
};

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

function showImgTitle(swiper) {
    try {
        if (swiper.activeIndex != null) {
            imgTitle = $(galleryTop.slides[swiper.activeIndex]).find('img').attr('title');
            imgTotalCount = galleryThumbs.slides.length;
            $(".media-title").text(imgTitle);
            $(".gallery-count").text(swiper.activeIndex + 1 + " of " + imgTotalCount.toString());
            currentStagePhoto = $(".connected-carousels-photos .stage-photos");
            currentStageActiveImage = currentStagePhoto.find(".swiper-slide.swiper-slide-active img");
            currentStagePhoto.find('.carousel-stage-photos').css({ 'height': currentStageActiveImage.height() });
        }
    } catch (e) {
        console.warn(e);
    }
}

function scrollHorizontal(pos) {
    $('#overallSpecsTab').animate({ scrollLeft: pos + 'px' }, 500);
}

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

var firstVideo = function () {
    var a = $(".carousel-navigation-videos .swiper-wrapper").first(".swiper-slide");
    var newSrc = a.find("img").attr("iframe-data");
    videoiFrame.setAttribute("src", newSrc);
};

var appendState = function (state) {
    window.history.pushState(state, '', '');
};

var slideToClick = function (swiper) {
    var clickedSlide = swiper.slides[swiper.clickedIndex];
    $('.carousel-navigation-photos .swiper-slide').removeClass('swiper-slide-active');
    $(clickedSlide).addClass('swiper-slide-active');
    galleryTop.slideTo(swiper.clickedIndex, 500);
};

docReady(function () {

    sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortCriteria = $('#sort'),
    sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortListLI = $(".sort-selection-div ul li");

    dealersPopupDiv = $('#more-dealers-popup'),
    dealerOffersDiv = $('#dealer-offers-popup'),
    termsConditions = $('#termsPopUpContainer');

    navigationVideosLI = $(".carousel-navigation-videos .swiper-slide");

    $window = $(window),
        overallSpecsTabsContainer = $('.overall-specs-tabs-container'),
        modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper'),
        modelSpecsFooter = $('#modelSpecsFooter'),
        topNavBarHeight = overallSpecsTabsContainer.height();

    var tabsLength = $('.overall-specs-tabs-wrapper li').length - 1;
    if (tabsLength < 2) {
        $('.overall-specs-tabs-wrapper li').css({ 'display': 'inline-block', 'width': 'auto' });
    }

    $('.overall-specs-tabs-wrapper li').first().addClass('active');

    // remove tabs highlight class for combined sections
    var newsContent = $('#makeNewsContent'),
        alternativeContent = $('#modelAlternateBikeContent'),
        makeDealersContent = $('#makeDealersContent');

    if (newsContent.length != 0) { // check if news content is present
        newsContent.removeClass('bw-model-tabs-data').addClass('model-news-content');
    }
    if (alternativeContent.length != 0) {
        alternativeContent.removeClass('bw-model-tabs-data margin-bottom20');
    }
    if (makeDealersContent.length != 0) {
        makeDealersContent.removeClass('bw-model-tabs-data');
    }

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

    $('.overall-specs-tabs-wrapper li').click(function () {
        var target = $(this).attr('data-tabs');
        $('html, body').animate({ scrollTop: $(target).offset().top - overallSpecsTabsContainer.height() }, 1000);
        centerItVariableWidth($(this), '.overall-specs-tabs-container');
        return false;
    });



    dropdown = {
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
            var elementText = element.find('input[type="submit"]').val(),
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


    videosThumbs = new Swiper('.carousel-navigation-videos', {
        slideActiveClass: '',
        spaceBetween: 0,
        slidesPerView: 'auto',
        slideToClickedSlide: true,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true
    });

    galleryThumbs = new Swiper('.carousel-navigation-photos', {
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

    slidegalleryThumbs = function (swiper) {
        galleryThumbs.slideTo(swiper.activeIndex, 500);
        galleryThumbs.slides.removeClass('swiper-slide-active');
        galleryThumbs.slides[swiper.activeIndex].className += ' swiper-slide-active';

        showImgTitle(galleryTop);
    };

    galleryTop = new Swiper('.carousel-stage-photos', {
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

});

docReady(function () {
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
            $('html, body').animate({ scrollTop: $('.model-preview-main-content').offset().top - 44 }, 500)
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
        if (versionCount > 1) {
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

    $(document).mouseup(function (e) {
        if (!$(".variantDropDown, .sort-div, .sort-div #upDownArrow, .sort-by-title").is(e.target)) {
            sortChangeUp($(".sort-div"));
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
            $('.float-button').hide().removeClass('float-fixed');
        else
            $('.float-button').show().addClass('float-fixed');
    });

    sortByDiv.click(function () {
        if (!sortByDiv.hasClass("open"))
            sortChangeDown(sortByDiv);
        else
            sortChangeUp(sortByDiv);
    });

    $("#photos-tab, #videos-tab").click(function () {
        firstVideo();
    });

    $("#videos-tab").click(function () {
        $('.carousel-navigation-videos .swiper-slide').removeClass('active');
        $('.carousel-navigation-videos .swiper-slide').first().addClass('active');
    });

    $(".gallery-close-btn").on('click', function () {
        gallery.close();
        history.back();
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

    /* model gallery */
    $('#model-main-image').on('click', function () {
        gallery.open();
        window.dispatchEvent(new Event('resize'));
        appendState('gallery');

        $("#photos-tab").trigger('click');
        // slide thumbnail to active gallery image
        galleryThumbs.slideTo(galleryTop.activeIndex, 500);
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

    navigationVideosLI.click(function () {
        navigationVideosLI.removeClass("active");
        $(this).addClass("active");
        var newSrc = $(this).find("img").attr("iframe-data");
        videoiFrame.setAttribute("src", newSrc);
        window.dispatchEvent(new Event('resize'));
    });

    // model gallery
    $('.carousel-navigation-photos .swiper-slide').first().addClass('swiper-slide-active');
    showImgTitle(galleryTop);
    $('#videos').hide();

});


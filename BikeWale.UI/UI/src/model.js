var assistFormSubmit, assistGetName, assistGetEmail, assistGetMobile;
var getOnRoadPriceBtn, onroadPriceConfirmBtn;
var getOffersClick = false, selectDropdownBox;
var $window, modelDetailsFloatingCard, modelSpecsTabsContentWrapper;
var abusereviewId, userreviewsListStr, vmUserReviews;
var isMileageSectionVisible;
// colour carousel
var colourCarousel, carouselColorList;

var overallTabsWrapper, overallTabs, overallSpecsDetailsFooter, topNavBar;
var floatingTabsHeight = 45;

function getBikeVersionLocation() {
    var versionName = getBikeVersion();
    var loctn = getCityArea;
    if (loctn != null) {
        if (loctn != '')
            loctn = '_' + loctn;
    }
    else {
        loctn = '';
    }
    var bikeVersionLocation = myBikeName + '_' + versionName + loctn;
    return bikeVersionLocation;
}

function getBikeVersion() {
    var versionName = '';
    if ($("#ddlVersion").length > 0) {
        versionName = $("#ddlVersion option:selected").text();
    } else {
        versionName = $('#singleversion').html();
    }
    return versionName;
}

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
            "sourceType": 1,
            "pQLeadId": pqSourceId,
            "deviceId": getCookie('BWC'),
            "refPQId": typeof pqId != 'undefined' ? pqId : '',
        };

        isSuccess = dleadvm.registerPQ(objData);

        if (isSuccess) {
          var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + dleadvm.pqGUId() + "&VersionId=" + versionId + "&DealerId=" + dealerID;
            window.location.href = "/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
        }
    } catch (e) {
        console.warn("Unable to create pricequote : " + e.message);
    }
}

function openLeadCaptureForm(dealerID) {
    triggerGA('Dealer_PQ', 'Secondary_Dealer_Get_Offers_Clicked', bikeVersionLocation);
    event.stopPropagation();
}

function logBhrighuForImage(imgId, imgCat, imgType) {

        if (imgId) {
            var lb = "";
            if (imgCat) {
                lb += "|category=" + imgCat;
            }

            if (imgType) {
                lb += "|type=" + imgType;
            }

            label = 'modelId=' + bikeModelId + '|imageid=' + imgId + lb + '|pageid=' + (gaObj ? gaObj.id : 0);
            cwTracking.trackImagesInteraction("BWImages", "ImageViewed", label);
        }
    }


function bindInsuranceText() {
    icityArea = GetGlobalCityArea();
    if (!viewModel.isDealerPQAvailable()) {
        var d = $("#bw-insurance-text");
        d.find("div.insurance-breakup-text").remove();
    }
    else if (viewModel.isDealerPQAvailable() && !(viewModel.priceQuote().isInsuranceFree && viewModel.priceQuote().insuranceAmount > 0)) {
        var e = $("table#model-view-breakup tr td:contains('Insurance')").first();
        e.find("div.insurance-breakup-text").remove();
    }
}

function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad",
        effect: "fadeIn"
    });
}

function animatePrice(ele, start, end) {
    $({ someValue: start }).stop(true).animate({ someValue: end }, {
        duration: 500,
        easing: 'easeInOutBounce',
        step: function () {
            $(ele).text(formatPrice(Math.round(this.someValue)));
        }
    }).promise().done(function () {
        $(ele).text(formatPrice(end));
    });
}

function LoadTerms(offerId) {
    $("div#termsPopUpContainer").show();
    $(".blackOut-window").show();
    if (offerId != 0 && offerId != null) {
        $(".termsPopUpContainer").css('height', '150')
        $('#termspinner').show();
        $('#terms').empty();
        $.ajax({
            type: "GET",
            url: "/api/Terms/?offerId=" + offerId,
            dataType: 'json',
            success: function (response) {
                $('#termspinner').hide();
                if (response != null)
                    $('#terms').html(response);
            },
            error: function (request, status, error) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            }
        });
    } else {
        $("#terms").load("/statichtml/tnc.html");
    }

    $(".termsPopUpContainer").css('height', '500');
}

docReady(function () {

    // ad blocker active than fallback method
    if (window.canRunAds === undefined) {
        callFallBackWriteReview();
    }
    isMileageSectionVisible = $('#modelMileageContent');

    if (isMileageSectionVisible.length>0)
    {
        triggerNonInteractiveGA("Model_Page", "Mileage_Card_Shown", myBikeName);
    }

    function sanitizeGALabel(str) {
      return str ? str : "null";
    }

    function checkPresentOrAbsent(id) {
      switch (typeof (id)) {
        case "number":
          return id > 0 ? "Present" : "Absent";
        case "string":
          return id.toLowerCase() == "true" ? "Present" : "Absent";
        case "boolean":
          return id ? "Present" : "Absent";
      }
    }
    if (isBikeNew.toLowerCase() == 'true') {
      var location = GetGlobalLocationObject();
      var eventAction = 'User_City ' + checkPresentOrAbsent(parseInt(cityId)) + '_Area ' + checkPresentOrAbsent(parseInt(areaId)) + '_Campaign_City Level ' + checkPresentOrAbsent(isCityLevelCampaignPresent) + '_Area Level ' + checkPresentOrAbsent(isAreaLevelCampaignPresent);
      var eventLabel = myBikeName + '_' + sanitizeGALabel(location.CityName) + '_' + sanitizeGALabel(location.AreaName) + '_' + shownCampaignType + " Campaign Shown";
      triggerNonInteractiveGA("Model_Page", eventAction, eventLabel);
    }

    function callFallBackWriteReview() {
        $('#adBlocker').show();
        $('.sponsored-card').hide();
    };

    // focus dealer offers
    $('#viewDealerOffers').on('click', function () {
        var offsetTop = $('#dealerDetailsWrapper').offset().top - $('#overallSpecsTab').height()

        $('html, body').animate({
            scrollTop: offsetTop
        }, 1000);
    });

    colourCarousel = $('#colourCarousel');
    carouselColorList = $('#model-color-list');
    var colorElements = carouselColorList.find('li');

    var canonical = $('#canonical').val();
    var imagePageUrl = $('#imageUrl').val();

    carouselColorList.on('click', 'li', function () {
        var colorId = $(this).find("div").data("colorid");
        if (colorId && !isNaN(colorId) && colorId != "0") {
            var image = $("#imageCarousel img[data-colorid=" + colorId + "]");
            if (image) {
                var imageUrl = image.attr("data-original") || image.attr("src");
                var imageCat = image.attr("data-imgcat");
                var imageType = image.attr("data-imgtype");
                var imageId = image.attr("data-imgid");
                if (imageUrl == "") {
                    imageUrl = "https://imgd.aeplcdn.com/393x221/bikewaleimg/images/noimage.png?q=85";
                }
                $('#colourCarousel span img').attr("src", imageUrl);
                $('#colourCarousel span').attr("href", imagePageUrl + '?q=' + Base64.encode('colorimageid=' + colorId + '&retUrl=' + canonical));
            }
        }
        if (imageId)
        {
            logBhrighuForImage(imageId,imageCat, imageType);
        }

        colorElements.removeClass('active');
        colorElements.eq([$(this).index()]).addClass('active');
    });

    $("span.carousel-img-container").click(function () {

        location.href = $(this).attr("href");


    });

    getCityArea = GetGlobalCityArea();

    $(".leadcapturebtn").click(function (e) {
        var ele = $(this);
        var leadOptions = {
            "dealerid": ele.attr('data-item-id'),
            "dealername": ele.attr('data-item-name'),
            "dealerarea": ele.attr('data-item-area'),
            "versionid": versionId,
            "leadsourceid": ele.attr('data-leadsourceid'),
            "pqsourceid": ele.attr('data-pqsourceid'),
            "isleadpopup": ele.attr('data-isleadpopup'),
            "mfgCampid": ele.attr('data-mfgcampid'),
            "pqid": 0,
            "pqguid": pqId,
            "pageurl": pageUrl,
            "clientip": clientIP,
            "dealerHeading": ele.attr('data-item-heading'),
            "dealerMessage": ele.attr('data-item-message'),
            "dealerDescription": ele.attr('data-item-description'),
            "pinCodeRequired": ele.attr("data-ispincodrequired"),
            "dealersRequired": ele.attr("data-dealersrequired"),
            "emailRequired": ele.attr("data-isemailrequired"),
            "eventcategory": ele.attr("data-cat"),
            "gaobject": {
                cat: ele.attr("data-cat"),
                act: ele.attr("data-act"),
                lab: bikeVersionLocation
            },
            "sendLeadSMSCustomer": ele.attr('data-issendleadsmscustomer'),
            "organizationName": ele.attr('data-item-organization'),
            "campaignId": ele.attr("data-campaignid")
        };
        gaLabel = getBikeVersionLocation();
        dleadvm.setOptions(leadOptions);
    });

    $('.chosen-select').on('change', function () {
        var selectField = $(this);
        if (selectField.val() > 0) {
            selectField.closest('.select-box').addClass('done');
        }
    });
    $('#getEmailID').on("focus", function () {
        $('#assistGetEmail').parent().addClass('not-empty');
    });

    $('#getFullName').on("focus", function () {
        $('#assistGetName').parent().addClass('not-empty');
    });

    $('#getMobile').on("focus", function () {
        $('#assistGetMobile').parent().addClass('not-empty');
    });
    $('#getEmailID').on("blur", function () {
        if ($('#assistGetEmail').val() == "")
            $('#assistGetEmail').parent().removeClass('not-empty');
    });

    $('#getFullName').on("blur", function () {
        if ($('#assistGetName').val() == "")
            $('#assistGetName').parent().removeClass('not-empty');
    });

    $('#getMobile').on("blur", function () {
        if ($('#assistGetMobile').val() == "")
            $('#assistGetMobile').parent().removeClass('not-empty');
    });

    if ($('.dealership-benefit-list li').length <= 2) {
        $('.dealership-benefit-list').addClass("dealer-two-offers");
    }

    if (bikeVersionLocation == '') {
        bikeVersionLocation = getBikeVersionLocation();
        if ($('#getOffersPrimary').length > 0)
            $('#getOffersPrimary').attr('v', bikeVersionLocation);
    }
    if (bikeVersion == '') {
        bikeVersion = getBikeVersion();
    }

    vmUserReviews = new modelUserReviews();

});

docReady(function () {
    assistFormSubmit = $('#assistFormSubmit'),
    assistGetName = $('#assistGetName'),
    assistGetEmail = $('#assistGetEmail'),
    assistGetMobile = $('#assistGetMobile');

    getOnRoadPriceBtn = $("#getOnRoadPriceBtn"),
    onroadPriceConfirmBtn = $("#onroadPriceConfirmBtn");

    (function ($) {

        var connector = function (itemNavigation, carouselStage) {
            return carouselStage.jcarousel('items').eq(itemNavigation.index());
        };
        var connector2 = function (itemNavigation2, carouselStage2) {
            return carouselStage2.jcarousel('items').eq(itemNavigation2.index());
        };
        var connector3 = function (itemNavigation3, carouselStage3) {
            return carouselStage3.jcarousel('items').eq(itemNavigation3.index());
        };
        $(function () {
            var carouselStage = $('.carousel-stage').jcarousel();
            var carouselNavigation = $('.carousel-navigation').jcarousel();
            var prev = -1;
            carouselNavigation.jcarousel('items').each(function () {
                var item = $(this);
                var target = connector(item, carouselStage);
                if (item) {
                    item
                        .on('jcarouselcontrol:active', function () {
                            carouselNavigation.jcarousel('scrollIntoView', this);

                            if (prev != -1) {
                                logBhrighuForImage(item.attr("data-imgid"), item.attr("data-imgcat"), item.attr("data-imgtype"));
                            }
                            prev = item.index();
                            item.addClass('active');

                        })
                        .on('jcarouselcontrol:inactive', function () {
                            item.removeClass('active');
                        })
                        .jcarouselControl({
                            target: target,
                            carousel: carouselStage
                        });
                }
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
                    target: '-=4'
                });
            $('.next-navigation')
                .on('jcarouselcontrol:inactive', function () {
                    $(this).addClass('inactive');
                })
                .on('jcarouselcontrol:active', function () {
                    $(this).removeClass('inactive');
                })
                .jcarouselControl({
                    target: '+=4'
                });
            $(".carousel-navigation, .carousel-stage").on('jcarousel:visiblein', 'li', function (event, carousel) {
                $(this).find("img.lazy").trigger("imgLazyLoad");
            });
        });
    })(jQuery);
});

docReady(function () {

    applyLazyLoad();

    // version dropdown
    $('.chosen-select').chosen();

    // version dropdown
    selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

    $(".carousel-navigation ul li").slice(0, 5).find("img.lazy").trigger("imgLazyLoad");
    $(".carousel-stage ul li").slice(0, 3).find("img.lazy").trigger("imgLazyLoad");

    document.location.href.split('?')[0];

    if ($('#getMoreDetailsBtn').length > 0) {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Get_More_Details_Shown", "lab": bikeVersionLocation });
    }
    if ($('#btnGetOnRoadPrice').length > 0) {
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_On_Road_Price_Button_Shown", "lab": myBikeName + "_" + getBikeVersion() });
    }
    if ($('#getOffersPrimary').length > 0) {
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_Offers_Button_Shown", "lab": bikeVersionLocation });
    }
    if ($('#getassistance').length > 0) {
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_Offers_Shown", "lab": bikeVersionLocation });
	}
	if ($('#modelExpertReviewsContent').length > 0) {
		dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Expert_Review_CardShown", "lab": myBikeName });
	}

    $window = $(window);
    overallTabsWrapper = $('#overallTabsWrapper');
    overallTabs = $('#overallSpecsTab');
    overallSpecsDetailsFooter = $('#overallSpecsDetailsFooter');
    topNavBar = overallTabs;

    // highlight 1st tab
    overallTabs.find('span').first().addClass('active');

    $(window).scroll(function () {
        try {
            var windowScrollTop = $window.scrollTop(),
                tabsWrapperOffsetTop = overallTabsWrapper.offset().top,
                specsFooterOffset = overallSpecsDetailsFooter.offset().top;

            if (windowScrollTop > tabsWrapperOffsetTop) {
                overallTabs.addClass('fixed-tab-nav');
            }

            else if (windowScrollTop < tabsWrapperOffsetTop) {
                overallTabs.removeClass('fixed-tab-nav');
            }

            if (windowScrollTop > specsFooterOffset - floatingTabsHeight) {
                overallTabs.removeClass('fixed-tab-nav');
            }

            $('#modelDetailsContainer .bw-model-tabs-data').each(function () {
                var top = $(this).offset().top - topNavBar.height(),
                    bottom = top + $(this).outerHeight();

                if (windowScrollTop >= top && windowScrollTop <= bottom) {
                    if (!$(this).hasClass('active')) {
                        topNavBar.find('span').removeClass('active');
                        $('#modelDetailsContainer .bw-model-tabs-data').removeClass('active');

                        $(this).addClass('active');
                        topNavBar.find('span[data-href="#' + $(this).attr('data-id') + '"]').addClass('active');

                    }
                }
            });
        } catch (e) {

        }

    });

    $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - topNavBar.height() + 1 }, 1000);
        return false;
    });

    var tabsHashParameter = window.location.hash;
    if (tabsHashParameter && $(tabsHashParameter).offset() != null) {

        $('html, body').scrollTop($(tabsHashParameter).offset().top - 650);
        //$('.overall-specs-tabs-wrapper a[href^=' + tabsHashParameter + ']').trigger('click');
    }

    // remove tabs highlight class for combined sections
    var alternativeContent = $('#modelAlternateBikeContent'),
        makeDealersContent = $('#makeDealersContent');

    if (alternativeContent.length != 0) {
        alternativeContent.removeClass('bw-model-tabs-data');
    }
    if (makeDealersContent.length != 0) {
        makeDealersContent.removeClass('bw-model-tabs-data');
    }

    $("#imageCarousel .stage li").click(function () {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Photo_Clicked", "lab": myBikeName });
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



    $("#getOnRoadPriceBtn, .city-area-edit-btn").on("click", function () {
        $("#onRoadPricePopup").show();
        $(".blackOut-window").show();
    });

    $(".onroadPriceCloseBtn").on("click", function () {
        $("#onRoadPricePopup").hide();
        $(".blackOut-window").hide();
    });

    onroadPriceConfirmBtn.on("click", function () {
        $("#modelPriceContainer .default-showroom-text").hide().siblings("#getOnRoadPriceBtn").hide();
        $("#modelPriceContainer .onroad-price-text").show().next("div.modelPriceContainer").find("span.viewBreakupText").show().next("span.showroom-text").show();
        $("#onRoadPricePopup").hide();
        $(".blackOut-window").hide();
    });

    $(".viewMoreOffersBtn").on("click", function () {
        $(this).hide();
        $("ul.moreOffersList").slideToggle()
    });

    // version dropdown
    function handleVersionChange(dropdown) {
        var optionValue = dropdown.activeOption.value;

        $('#hdnVariant').val(optionValue);
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Version_Change", "lab": "" });
        window.location.href = $(dropdown.container).data("pageurl") + "?versionId=" + optionValue;
    }

    var versionDropdown = new DropdownMenu('#ddlNewVersionList', {
        onChange: handleVersionChange
    });

    $('#ddlVersion').on("change", function () {
        $('#hdnVariant').val($(this).val());
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Version_Change", "lab": bikeVersionLocation });
        window.location.href = $(this).data("pageurl") + "?versionId=" + $(this).val();
    });


    $("#getMoreDetailsBtn, #getMoreDetailsBtnCampaign, #getassistance, #getOffersFromDealerFloating").on("click", function () {
        leadSourceId = $(this).attr("leadSourceId");
        $("#leadCapturePopup").show();
        popup.lock();
        if ($(this).attr("id") == "getassistance") {
            dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Get_Offers_Clicked", "lab": bikeVersionLocation });
            getOffersClick = true;
        }
        else if (leadSourceId != "24") {
            dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Get_More_Details_Clicked", "lab": bikeVersionLocation });
        }
    });

    $("#viewBreakupText").on('click', function (e) {
        triggerGA('Model_Page', 'View_Detailed_Price_Clicked', bikeVersionLocation);
    });

    $("#getdealerdetails").on('click', function (e) {
        triggerGA('Model_Page', 'View_Dealer_Details_Clicked', bikeVersionLocation);
        var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId + "&IsDealerAvailable=true";
        window.location.href = "/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
    });

    $(".breakupCloseBtn,.blackOut-window").on('click', function (e) {
        $("div#breakupPopUpContainer").hide();
        $(".blackOut-window").hide();
    });

    $(".termsPopUpCloseBtn,.blackOut-window").on('click', function (e) {
        $("div#termsPopUpContainer").hide();
        $(".blackOut-window").hide();
    });

    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) {
            $("div.breakupCloseBtn").click();
            $("div.termsPopUpCloseBtn").click();
            $("div.leadCapture-close-btn").click();
        }
    });

    $('#insuranceLink').on('click', function (e) {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Insurance_Clicked_Model", "lab": myBikeName + "_" + icityArea });
    });

    $('#bookNowBtn').on('click', function (e) {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Book_Now_Clicked", "lab": bikeVersionLocation });
        window.location.href = "/pricequote/bookingsummary_new.aspx";
    });

    $(".more-features-btn").click(function () {
        $(".more-features").slideToggle();
        var a = $(this).find("a");
        a.text(a.text() === "+" ? "-" : "+");
        if (a.text() === "+")
            a.attr("href", "#features");
        else
            a.attr("href", "javascript:void(0)");
    });

    /* GA Tags */
    $('#btnGetOnRoadPrice').on('click', function (e) {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Check_On_Road_Price_Clicked", "lab": myBikeName + "_" + getBikeVersion() });
    });

    $('#btnDealerPricePopup').on('click', function () {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Show_On_Road_Price_Clicked", "lab": myBikeName + "_" + getBikeVersion() + "_" + getCityArea });
    });

    $('#getdealerdetails').on('click', function () {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "View_Dealer_Details_Clicked", "lab": myBikeName + "_" + getBikeVersion() + "_" + getCityArea });
    });

    $('#getOffersPrimary').on('click', function () {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Get_Offers_Button_Clicked", "lab": myBikeName + "_" + getBikeVersion() + "_" + getCityArea });
    });

    $('.tnc').on('click', function (e) {
        LoadTerms($(this).attr("id"));
    });

    $('.changeCity').on('click', function (e) {
        try {
            dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "City_Change_Initiated", "lab": bikeVersionLocation });
        }
        catch (err) { }
    });

    $('#locslug').on('click', function (e) {
        triggerGA('Model_Page', 'Booking_Benefits_City_Link_Clicked', myBikeName + '_' + getBikeVersion());
    });

    $('.more-dealers-link').on('click', function () {
        $(this).parent().prev('#moreDealersList').slideDown();
        $(this).hide().next('.less-dealers-link').show();
    });

    $('.less-dealers-link').on('click', function () {
        $(this).parent().prev('#moreDealersList').slideUp();
        $(this).hide().prev('.more-dealers-link').show();
    });

    $('#read-more-preview').click(function () {
        if (!$(this).hasClass('open')) {
            $('#main-preview-content').hide();
            $('#more-preview-content').show();
            $(this).text('Collapse');
            $(this).addClass("open");
        }
        else if ($(this).hasClass('open')) {
            $('#main-preview-content').show();
            $('#more-preview-content').hide();
            $(this).text('Read more');
            $(this).removeClass('open');
            $('html, body').animate({ scrollTop: $('#modelAboutContent').offset().top - floatingTabsHeight }, 500);
        }

    });

    $('#model-specs-list').on('click', '.model-accordion-tab', function () {
        var tab = $(this),
            allTabs = $('#model-specs-list .model-accordion-tab');

        if (!tab.hasClass('active')) {
            allTabs.removeClass('active');
            tab.addClass('active');
            $('html, body').animate({ scrollTop: tab.offset().top - floatingTabsHeight }, 500);
        }
        else {
            tab.removeClass('active');
        }
    });

    $('.view-features-link').on('click', function () {
        var target = $(this),
            featuresHeading = $('#model-features-content'),
            moreFeatures = $('#model-more-features-list');

        if (!target.hasClass('active')) {
            target.addClass('active');
            $('html, body').animate({ scrollTop: featuresHeading.offset().top - floatingTabsHeight }, 500);
            moreFeatures.slideDown();
            target.text('Collapse');
        }
        else {
            target.removeClass('active');
            $('html, body').animate({ scrollTop: featuresHeading.offset().top - floatingTabsHeight }, 500);
            moreFeatures.slideUp();
            target.text('View all features');
        }
    });

    $('.navigation').on('click', '.all-photos-target', function () {
        var target = $(this).find('a').attr('href');
        window.location = target;
    });

    $('#partner-dealer-panel').on('click', function () {
        var panel = $(this);

        if (!panel.hasClass('open')) {
            panel.addClass('open');
            panel.siblings('#moreDealersList').slideDown();
        }
        else {
            panel.removeClass('open');
            panel.siblings('#moreDealersList').slideUp();
        }
    });

    // tooltip
    $('.bw-tooltip').on('click', '.close-bw-tooltip', function () {
        var tooltipParent = $(this).closest('.bw-tooltip');

        if (!tooltipParent.hasClass('slideUp-tooltip')) {
            tooltipParent.fadeOut();
        }
        else {
            tooltipParent.slideUp();
        }
    });
    applyLikeDislikes();

    $('#report-background, .report-abuse-close-btn').on('click', function () {
        reportAbusePopup.close();
    });

    if (document.getElementById("modelsBySeriesWidget")) {
        try {
            var seriesEle = $("#modelsBySeriesWidget");
            triggerNonInteractiveGA(seriesEle.attr("data-cat"), seriesEle.attr("data-act"), seriesEle.attr("data-lab"));
        } catch (e) {

        }
	}
	$("#modelExpertReviewsContent").on('click', function () {
		triggerGA('Model_Page', 'Expert_Review_CardClicked', myBikeName);
	});
    // For saving page in recently viewed models/make
	if (typeof pageData != "undefined" && pageData != null)
	    recentSearches.saveRecentSearches(pageData);

	//QnA Read more on model page
	ToggleReadMore.registerEvents();

});


function upVoteListReview(e) {
    var localReviewId = e.currentTarget.getAttribute("data-reviewid");
    bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "1" });
    $('#upvoteBtn' + "-" + localReviewId).addClass('active');
    $('#downvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');
    $('#upvoteCount' + "-" + localReviewId).text(parseInt($('#upvoteCount' + "-" + localReviewId).text()) + 1);
    voteListUserReview(1, localReviewId);
}

function downVoteListReview(e) {
    var localReviewId = e.currentTarget.getAttribute("data-reviewid");
    bwcache.set("ReviewDetailPage_reviewVote_" + localReviewId, { "vote": "0" });
    $('#downvoteBtn' + "-" + localReviewId).addClass('active');
    $('#upvoteBtn' + "-" + localReviewId).attr('disabled', 'disabled');
    $('#downvoteCount' + "-" + localReviewId).text(parseInt($('#downvoteCount' + "-" + localReviewId).text()) + 1);
    voteListUserReview(0, localReviewId);
}

function voteListUserReview(vote, locReviewId) {
    $.ajax({
        type: "POST",
        url: "/api/user-reviews/voteUserReview/?reviewId=" + locReviewId + "&vote=" + vote,
        success: function (response) {
        }
    });
}

function applyLikeDislikes() {
    $(".feedback-button").each(function () {
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
}

function reportReview(e) {
    abusereviewId = e.currentTarget.getAttribute("data-reviewid");
    reportAbusePopup.open();
}

function reportAbuse() {
    var isError = false;

    if ($("#txtAbuseComments").val().trim() == "") {
        $("#spnAbuseComments").html("Comments are required");
        isError = true;
    } else {
        $("#spnAbuseComments").html("");
    }

    var locReviewId;
    if (abusereviewId > 0 && !isError) {
        locReviewId = abusereviewId;
        document.getElementById("pReport-" + locReviewId).innerHTML = "Your request has been sent to the administrator.";
    }


    if (!isError) {
        var commentsForAbuse = $("#txtAbuseComments").val().trim();
        $.ajax({
            type: "POST",
            url: "/api/user-reviews/abuseUserReview/?reviewId=" + locReviewId + "&comments=" + commentsForAbuse,
            success: function (response) {
                reportAbusePopup.close();
            }
        });
    }
}

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
    self.currentReviewList = ko.observableArray(null);
    self.trimLengthText = ko.observable();
    self.isLoading = ko.observable(false);
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
        if ($("#user-review-div")[0])
            ko.applyBindings(vmUserReviews, $("#user-review-div")[0]);

        self.readMore(event);
        self.IsInitialized(true);
        $('#loader').removeClass('hide');
    };
    self.readMore = function (event) {
        var ele = $(event.currentTarget);
        var reviewId = ele.data("reviewid");
        var itemNo = ele.data("id");
        if (!self.currentReviewList().length && bikeModelId) {
            var apiUrl = "/api/user-reviews/search/V2/?InputFilter.review=true&InputFilter.SO=1&InputFilter.PN=1&InputFilter.PS=3&ReviewFilter.RatingQuestion=false&ReviewFilter.ReviewQuestion=true&ReviewFilter.BasicDetails=false&InputFilter.Model=" + bikeModelId;

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

        if ($('#user-review-div') && $('#user-review-div').attr('data-readmore')) {
            $('#user-review-div').attr('data-readmore', parseInt($('#user-review-div').attr('data-readmore')) + 1);
        }

        if ($('#user-review-div') && $('#user-review-div').attr('data-readmore') == "3") {
            self.isLoading(true);
            self.getMoreReviews();
        }

        return true;
    };
}


function logBhrighu(itemNo, eventName) {
    label = 'modelId=' + bikeModelId + '|tabName=recent|reviewOrder=' + (++itemNo) + '|pageSource=' + $('#pageSource').val();
    cwTracking.trackUserReview(eventName, label);
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

$(".navtab").click(function () {

    try {
        var scrollSectionId = $(this).data('href');
        $('html,body').animate({
            scrollTop: $(scrollSectionId).offset().top - 40
        },
      'slow');

    }
    catch (e) {
        console.log(e);
    }
});

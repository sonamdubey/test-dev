function bindInsuranceText() {
    icityArea = GetGlobalCityArea();
    if (!viewModel.isDealerPQAvailable()) {
        var d = $("#bw-insurance-text");
        d.find("div.insurance-breakup-text").remove();
        //d.append(" <div class='insurance-breakup-text' style='position: relative; color: #999; font-size: 11px; margin-top: 1px;'>Save up to 60% on insurance - <a target='_blank' href='/insurance/' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Model_Page', act: 'Insurance_Clicked',lab: '" + myBikeName + "_" + icityArea + "' });\">PolicyBoss</a> <span style='margin-left: 8px; vertical-align: super; font-size: 9px;'>Ad</span></div>");
    }
    else if (viewModel.isDealerPQAvailable() && !(viewModel.priceQuote().isInsuranceFree && viewModel.priceQuote().insuranceAmount > 0)) {
        var e = $("table#model-view-breakup tr td:contains('Insurance')").first();
        e.find("div.insurance-breakup-text").remove();
        //e.append("<div class='insurance-breakup-text' style='position: relative; color: #999; font-size: 11px; margin-top: 1px;'>Save up to 60% on insurance - <a target='_blank' href='/insurance/' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Model_Page', act: 'Insurance_Clicked',lab: '" + myBikeName + "_" + icityArea + "' });\">PolicyBoss</a> <span style='margin-left: 8px; vertical-align: super; font-size: 9px;'>Ad</span></div>");
    }
}



function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad",
        effect: "fadeIn"
    });
}
$(document).ready(function (e) {
    applyLazyLoad();

    $(".carousel-navigation ul li").slice(0, 5).find("img.lazy").trigger("imgLazyLoad");
    $(".carousel-stage ul li").slice(0, 3).find("img.lazy").trigger("imgLazyLoad");
    document.location.href.split('?')[0];
    if ($('#getMoreDetailsBtn').length > 0) {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Get_More_Details_Shown", "lab": bikeVersionLocation });
    }
    if ($('#btnGetOnRoadPrice').length > 0) {
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_On_Road_Price_Button_Shown", "lab": myBikeName + "_" + getBikeVersion() });
    }
    if ($('#getassistance').length > 0)
    {
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_Offers_Shown", "lab": bikeVersionLocation });
    }
});


$(document).ready(function (e) {

    if ($(".bw-overall-rating a").last().css("display") == "none") {
        var a = $(this);
        var b = $(this).attr("href");
        console.log(a);
        $(this).remove();
        $(a + ".bw-tabs-data.margin-bottom20.hide").remove();
    }

    $('.bw-overall-rating a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - 50 - $(".header-fixed").height() }, 1000);
        return false;

    });
    // ends	
});



$("#getFullName, #assistGetName").on("focus", function () {
    hideError($(this));
});

$("#getEmailID, #assistGetEmail").on("focus", function () {
    hideError($(this));
    prevEmail = $(this).val().trim();
});

$("#getMobile, #assistGetMobile").on("focus", function () {
    hideError($(this));
    prevMobile = $(this).val().trim();

});


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

        var carouselStage2 = $('.carousel-stage-photos').jcarousel();
        var carouselNavigation2 = $('.carousel-navigation-photos').jcarousel();

        var carouselStage3 = $('.carousel-stage-videos').jcarousel();
        var carouselNavigation3 = $('.carousel-navigation-videos').jcarousel();        

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
            var target = connector3(item3, carouselStage3);
            item3
				.on('jcarouselcontrol:active', function () {
				    carouselNavigation3.jcarousel('scrollIntoView', this);
				    item3.addClass('active');
				})
				.on('jcarouselcontrol:inactive', function () {
				    item3.removeClass('active');
				})
				.jcarouselControl({
				    target: target,
				    carousel: carouselStage3
				});
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
        $(".carousel-navigation, .carousel-stage, .carousel-stage-photos, .carousel-navigation-photos").on('jcarousel:visiblein', 'li', function (event, carousel) {
            $(this).find("img.lazy").trigger("imgLazyLoad");
        });     
    });
})(jQuery);

$(".photos-next-stage").click(function () {
    getImageNextIndex();
});

$(".photos-prev-stage").click(function () {
    getImagePrevIndex();
});

$(".carousel-navigation-photos").click(function () {
    getImageIndex();
});

$(".stage-photos").hover(function () {
    $(".photos-next-stage, .photos-prev-stage, .photos-prev-stage.inactive, .photos-next-stage.inactive").toggleClass("hide show");
});

$(".navigation-photos").hover(function () {
    $(".photos-prev-navigation, .photos-next-navigation, .photos-prev-navigation.inactive, .photos-next-navigation.inactive").toggleClass("hide show");
});

$(".stage-videos").hover(function () {
    $(".videos-next-stage, .videos-prev-stage, .videos-prev-stage.inactive, .videos-next-stage.inactive").toggleClass("hide show");
});

$(".navigation-videos").hover(function () {
    $(".videos-prev-navigation, .videos-next-navigation, .videos-prev-navigation.inactive, .videos-next-navigation.inactive").toggleClass("hide show");
});
function animatePrice(ele,start,end)
{
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

$("#bikeBannerImageCarousel .stage li").click(function () {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Photo_Clicked", "lab": myBikeName });
    if (imgTotalCount > 0) {
        $('body').addClass('lock-browser-scroll');
        $(".blackOut-window-model").show();
        $(".bike-gallery-popup").removeClass("hide").addClass("show");
        $(".modelgallery-close-btn").removeClass("hide").addClass("show");
        $(".carousel-stage-photos ul li").slice(0, 3).find("img.lazy").trigger("imgLazyLoad");
        $(".carousel-navigation-photos ul li").slice(0, 5).find("img.lazy").trigger("imgLazyLoad");
        $(document).on("keydown", function (e) {
            var $blackModel = $(".blackOut-window-model");
            var $bikegallerypopup = $(".bike-gallery-popup");
            if ($bikegallerypopup.hasClass("show") && e.keyCode === 27) {
                $(".modelgallery-close-btn").click();
            }
            if ($bikegallerypopup.hasClass("show") && e.keyCode == 39 && $("#photos-tab").hasClass("active")) {
                $(".photos-next-stage").click();
            }
            if ($bikegallerypopup.hasClass("show") && e.keyCode == 37 && $("#photos-tab").hasClass("active")) {
                $(".photos-prev-stage").click();
            }
        });
    }    
});

$(".modelgallery-close-btn, .blackOut-window-model").click(function () {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window-model").hide();
    $(".bike-gallery-popup").removeClass("show").addClass("hide");
    $(".modelgallery-close-btn").removeClass("show").addClass("hide");
    videoiFrame.setAttribute("src", "");
    var galleryThumbIndex = $(".carousel-navigation-photos ul li.active").index();
    $(".carousel-stage").jcarousel('scroll', galleryThumbIndex);
});

$(document).ready(function () {
    getImageDetails();
});

var mainImgIndexA;

$(".carousel-stage ul li").click(function () {
    mainImgIndexA = $(".carousel-navigation ul li.active").index();
    setGalleryImage(mainImgIndexA);
});

var setGalleryImage = function (currentImgIndex) {
    $(".carousel-stage-photos").jcarousel('scroll', currentImgIndex);
    getImageDetails();
};

var getImageDetails = function () {
    imgTotalCount = $(".carousel-stage-photos ul li").length;
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
};

var getImageNextIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").next();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

var getImagePrevIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active").prev();
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");    
    setImageDetails(imgTitle, imgIndex);
}

var getImageIndex = function () {
    var imgIndexA = $(".carousel-navigation-photos ul li.active");
    var imgIndex = imgIndexA.index() + 1;
    var imgTitle = imgIndexA.find("img").attr("title");
    setImageDetails(imgTitle, imgIndex);
}

var setImageDetails = function (imgTitle,imgIndex) {            
    $(".leftfloatbike-gallery-details").text(imgTitle);
    if (imgIndex > 0) {
        $(".bike-gallery-count").text(imgIndex.toString() + "/" + imgTotalCount.toString());
    }
}

var videoiFrame = document.getElementById("video-iframe");

/* first video src */
$("#photos-tab, #videos-tab").click(function () {
    firstVideo();
});

$("#videos-tab").click(function () {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Video_Tab_Clicked", "lab": myBikeName });
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


$("a.read-more-btn").click(function () {
    if(!$(this).hasClass("open")) {
        $(".model-about-main").hide();
        $(".model-about-more-desc").show();
        var a = $(this).find("span");
        a.text(a.text() === "full story" ? "less" : "full story");
        $(this).addClass("open");
    }
    else if($(this).hasClass("open")) {
        $(".model-about-main").show();
        $(".model-about-more-desc").hide();
        var a = $(this).find("span");
        a.text(a.text() === "full story" ? "less" : "full story");
        $(this).removeClass("open");
    }

});

var getOnRoadPriceBtn = $("#getOnRoadPriceBtn"),
	onroadPriceConfirmBtn = $("#onroadPriceConfirmBtn");

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
    if($(this).attr('versionid') == $('#hdnVariant').val()){
        return false;
    }
    $('#hdnVariant').val($(this).attr('versionid'));
    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Version_Change", "lab": bikeVersionLocation });
});

var getOffersClick = false;

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
    secondarydealer_Click(dealerId);
});

$(".breakupCloseBtn,.blackOut-window").on('click',function (e) {         
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

$(document).mouseup(function (e) {
    if (!$(".variantDropDown, .sort-div, .sort-div #upDownArrow, .sort-by-title").is(e.target)) {
        $.sortChangeUp($(".sort-div"));
    }
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
    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Check_On_Road_Price_Clicked", "lab": myBikeName + "_" + getBikeVersion() + "_" + getCityArea });
});

$('#btnDealerPricePopup').on('click', function () {
    dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "Show_On_Road_Price_Clicked", "lab": myBikeName + "_" + getBikeVersion() + "_" + getCityArea });
});

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
    if ($('#defaultVariant').length > 0) {
        versionName = $('#defaultVariant').html();
    }
    else if ($('#versText').length > 0) {
        versionName = $('#versText').html();
    }
    return versionName;
}

$('.tnc').on('click', function (e) {
    LoadTerms($(this).attr("id"));
});

$('.changeCity').on('click', function (e) {
    try {
        dataLayer.push({ "event": "Bikewale_all", "cat": "Model_Page", "act": "City_Change_Initiated", "lab": bikeVersionLocation });
    }
    catch (err) { }
});

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
$('#testimonialWrapper .jcarousel').jcarousel({ wrap: 'circular' }).jcarouselAutoscroll({ interval: 7000, target: '+=1', autostart: true });
$('#locslug').on('click', function (e) {
    triggerGA('Model_Page', 'Booking_Benefits_City_Link_Clicked', myBikeName + '_' + getBikeVersion());
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

var assistFormSubmit = $('#assistFormSubmit'),
    assistGetName = $('#assistGetName'),
    assistGetEmail = $('#assistGetEmail'),
    assistGetMobile = $('#assistGetMobile');

assistFormSubmit.on('click', function () {
    leadSourceId = $(this).attr("leadSourceId");
    ValidateUserDetail(assistGetName, assistGetEmail, assistGetMobile);    
});


//
$(document).ready(function () {

    var modelPrice = $('#scrollFloatingButton'),
        $window = $(window),
        modelDetailsFloatingCard = $('#modelDetailsFloatingCardContent'),
        modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper');

    var modelSpecsTabsContentWrapper = $('#modelSpecsTabsContentWrapper'),
        overallSpecsDetailsFooter = $('#overallSpecsDetailsFooter'),
        topNavBar = $('.model-details-floating-card');

    $(window).scroll(function () {
        var windowScrollTop = $window.scrollTop(),
            modelPriceOffsetTop = modelPrice.offset().top,
            modelSpecsTabsOffsetTop = modelSpecsTabsContentWrapper.offset().top;

        if (windowScrollTop > modelPriceOffsetTop + 40) {
            modelDetailsFloatingCard.addClass('fixed-card');
            if (windowScrollTop > modelSpecsTabsOffsetTop - topNavBar.height()) {
                modelDetailsFloatingCard.addClass('activate-tabs');
            }
        }
        else if (windowScrollTop < modelPriceOffsetTop + 40) {
            modelDetailsFloatingCard.removeClass('fixed-card');
        }

        if (modelDetailsFloatingCard.hasClass('activate-tabs')) {
            if (windowScrollTop < modelSpecsTabsOffsetTop + 43 - topNavBar.height())
                modelDetailsFloatingCard.removeClass('activate-tabs');
            if (windowScrollTop > overallSpecsDetailsFooter.offset().top - topNavBar.height())
                modelDetailsFloatingCard.removeClass('fixed-card');
        }


        $('#modelSpecsTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - topNavBar.height(),
            bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                topNavBar.find('a').removeClass('active');
                $('#modelSpecsTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');
                topNavBar.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
            }
        });

    });
    

    $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - topNavBar.height() + 1 }, 1000);
        return false;
    });

});

$('a.read-more-model-preview').click(function () {
    if (!$(this).hasClass('open')) {
        $('.model-preview-main-content').hide();
        $('.model-preview-more-content').show();
        $(this).text($(this).text() === 'Read more' ? 'Collapse' : 'Read more');
        $(this).addClass("open");
    }
    else if ($(this).hasClass('open')) {
        $('.model-preview-main-content').show();
        $('.model-preview-more-content').hide();
        $(this).text($(this).text() === 'Read more' ? 'Collapse' : 'Read more');
        $(this).removeClass('open');
    }

});

$('#model-specs-list').on('click', '.model-accordion-tab', function () {
    var tab = $(this),
        allTabs = $('#model-specs-list .model-accordion-tab');

    if (!tab.hasClass('active')) {
        allTabs.removeClass('active');
        tab.addClass('active');
        $('html, body').animate({ scrollTop: tab.offset().top - $("#modelDetailsFloatingCardContent").height() }, 500);
    }
    else {
        tab.removeClass('active');
    }
});

$('.view-features-link').on('click', function () {
    var target = $(this),
        featuresHeading = $('#model-features-content'),
        moreFeatures = $('#model-more-features-list'),
        floatingCard = $("#modelDetailsFloatingCardContent").height() + 10;

    if (!target.hasClass('active')) {
        target.addClass('active');
        $('html, body').animate({ scrollTop: featuresHeading.offset().top - floatingCard }, 500);
        moreFeatures.slideDown();
        target.text('Collapse');
    }
    else {
        target.removeClass('active');
        $('html, body').animate({ scrollTop: featuresHeading.offset().top - floatingCard }, 500);
        moreFeatures.slideUp();
        target.text('View all features');
    }
});

$(document).ready(function() {
    var comparisonCarousel = $("#modelComparisonContent");
    comparisonCarousel.find(".jcarousel").jcarousel();

    comparisonCarousel.find(".jcarousel-control-prev").jcarouselControl({
        target: '-=2'
    });

    comparisonCarousel.find(".jcarousel-control-next").jcarouselControl({
        target: '+=2'
    });
});

$('.navigation').on('click', '.all-photos-target', function () {
    var target = $(this).find('a').attr('href');
    window.location = target;
});
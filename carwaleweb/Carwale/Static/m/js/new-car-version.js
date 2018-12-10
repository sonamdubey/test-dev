$(document).ready(function () {
    $("img.lazy").lazyload({ skip_invisible: true });
    userCityId = $.cookie("_CustCityIdMaster");
    $("#divCarData li").click(function () {
        $(this).removeClass('list').addClass('listActive').siblings().removeClass('listActive').addClass('list');
        var showContent = $(this).attr('show_content');
        $("#" + showContent).removeClass('hide').addClass('box new-line5').siblings().removeClass('box new-line5').addClass('hide');
    });


    $('#discountslug').live('click', function () {
        cwTracking.trackAction('CWInteractive', 'deals_mobile', 'dealsaccess_mobile', 'variantpagemob');

    });

    trackUserHistory([modelId]);
    var features = $("table.features tr:first-child:has(span.versionmenu-icons)");

    $(features).click(function () {
        var span = $(this).find("span");
        if ($(this).has("span.minus-icon").length > 0) {
            span.removeClass("minus-icon").addClass("plus-icon")
            $(this).siblings().fadeOut(200);
        } else {
            span.removeClass("plus-icon").addClass("minus-icon");
            $(this).siblings().fadeIn(400);
        }
    });
    var page = new PageLoad();
    $(document).on("mastercitychange", function (event, cityName, cityId) {
        window.location.reload(window.location.href);
    });

    tracking.trackBhriguImpression();
    AddTyersAndWheels();
    registerSwiperCarousal();
});

var collectImpTrackData = {

    getCategory: function () {
        return 'VersionPage';
    },
    getAction: function () {
        return 'VersionImpression';
    },
    getLabel: function () {
        label = "modelid=" + modelId + "|versionid=" + defaultVerId + "|cityid=" + userCityId + "|source=43";
        label = label + (isPriceShown == 'True' ? "|isorpshown=1" : '');
        label = label + (window.campaignId > 0 ? "|campaignid=" + campaignId + "|iscampaignshown=1|campaignpanel=" + sponsorDlrLeadPanel + "|campaigntype=1" : '');
        return label;
    }
}

var PageLoad = function () {
                
    if ($('#advantagediv').is(":visible")) 
        cwTracking.trackAction('CWNonInteractive', 'deals_mobile', ' dealsimpression_mobile', 'variantpagemob');

    if ($('.advantageCardLink').is(":visible")) 
        cwTracking.trackAction('CWNonInteractive', 'deals_mobile', 'dealsimpression_mobile', 'variantpage_carsuggestions_mob');
    if ($('#advantageCallSlug').is(':visible')) {
        cwTracking.trackAction('CWNonInteractive', 'deals_mobile', ' dealsimpression_mobile', 'variantpage_floatingbuttonmob');
    }
}

function PhotoGalleryGATrack() {
    cwTracking.trackAction('Photo-Gallery-Mobile', 'Photo-Gallery-Mobile', 'Version-Page-Photo-Click', carName);
}

function FloatingSlugTracking(OnRoadPriceBtn, CallDealerBtn, PageName, divPrice, callslugnumber) {
    var CityName = $.cookie("_CustCityMaster");
    if (divPrice && OnRoadPriceBtn && CallDealerBtn) { // ORP-Button-Clicked-Dual-Display
        cwTracking.trackAction("FloatingSlugTracking", "Floating-Slug", "ORP-Button-Clicked-Dual-Display",  PageName + "-" + CarDetails.carModelName + "-" + CityName + "-" + campaignId);
        return;
    }
    if (divPrice && OnRoadPriceBtn && (!CallDealerBtn)) { // ORP-Button-Clicked-Solo-Display
        cwTracking.trackAction("FloatingSlugTracking", "Floating-Slug", "ORP-Button-Clicked-Solo-Display", PageName + "-" + CarDetails.carModelName + "-" + CityName);
        return;
    }
    if (callslugnumber) { // Call-Button-Clicked
        cwTracking.trackAction( "FloatingSlugTracking", "Floating-Slug", "Call-Button-Clicked",  PageName + "-" + CarDetails.carModelName + "-" + CityName + "-" + campaignId);
        return;
    }
    if (OnRoadPriceBtn && CallDealerBtn) {      // Shown-with-ORP-and-Call
        cwTracking.trackAction("FloatingSlugTracking", "Floating-Slug",  "Shown-with-ORP-and-Call",  PageName + "-" + CarDetails.carModelName + "-" + CityName + "-" + campaignId);
    }
    if (OnRoadPriceBtn && (!CallDealerBtn)) { // Shown-with-only-ORPButton
        cwTracking.trackAction("FloatingSlugTracking", "Floating-Slug", "Shown-with-only-ORPButton", PageName + "-" + CarDetails.carModelName + "-" + CityName);
    }
}

/* Car suggested for you js starts here */
$('ul#carsuggession').each(function () {
    var len = $(this).find("li").length;
    if (len > 1) {
        $(this).parent().addClass("swiper-container");
    }
    if (len == 1) {
        $(this).parent().addClass("cardSuggestSingle");
    }
});
/* Car suggested for you js ends here */
/* Color expandable js*/
$('.color-expandable').click(function () {
    $(this).toggleClass('color-truncated');
});
function AddTyersAndWheels() {
    var temp = $('#tabSpecs').find('tr[itemmasterid=256],tr[itemmasterid=202],tr[itemmasterid=255],tr[itemmasterid=276]');
    var logoUrl = '/specials/tyreguide/';
    if ($(temp).length > 0) {
        var wheelsAndTyersContainer = '<table width="100%" border="0" cellspacing="0" cellpadding="0" class="detail-table margin-bottom10"><tbody><tr><th class="containerBg" valign="top">Wheels & Tyres</th></tr><tr id="bridgeStone"><td class="line-border" colspan="2"></td></tr>';
        $('#tabSpecs table').last().after(wheelsAndTyersContainer);
        $("#bridgeStone").after(temp);
        $('#logolink').attr('href', logoUrl);
    }
}
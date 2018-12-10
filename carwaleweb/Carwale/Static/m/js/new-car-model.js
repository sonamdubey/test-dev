
var PageLoad = function () {
    AdBlockTracking();
    
    $("#esLeadFormSubmit").attr('data-label', 'submit').addClass("click_track");
    //$("#es-leadform").append('<div class="margin-top15">Or Call us on <a href="tel:18002090230" class="text-green font16 text-bold toll-free-number" title="Toll Free">1800-2090-230</a></div>');
    $($("#es-leadform").children()[2]).hide();
    $("#personEmail").parent().hide();
    $($("#es-leadform").children()[0]).text("Please share your contact details");
    $("#es-thankyou h2").html("Thank You for sharing your details. We will share your details with the manufacturer.");
    
    if ($('#divAdvantageLink').is(":visible")) {
        if (IsCity == "False")
            Common.utils.trackAction('CWNonInteractive', 'deals_mobile', ' dealsimpression_mobile', 'modelpagemob');
        else
            Common.utils.trackAction('CWNonInteractive', 'deals_mobile', ' dealsimpression_mobile', 'priceincitymob');
    }
    if ($('#advantageCallSlug').is(':visible')) {
        Common.utils.trackAction('CWNonInteractive', 'deals_mobile', ' dealsimpression_mobile', 'modelpage_floatingbuttonmob');
    }
    NewCar.advantage.tracking();
    if ($('.getOffersVersionLink').length > 0) {
        if (IsCity == "True") {
            Common.utils.trackAction('CWNonInteractive', 'MSite-PriceInCityPage-VersionList_CampaignLink', ' Link_Shown', ModelName + ',' + sponsorDlrName + ',' + sponsoredDealerId + ',' + CityName);
        } else {
            Common.utils.trackAction('CWNonInteractive', 'MSite-ModelPage-VersionList_CampaignLink', ' Link_Shown', ModelName + ',' + sponsorDlrName + ',' + sponsoredDealerId + ',' + (CityName == "" || CityName == "Select City" ? "No City" : CityName));
        }
    }

    if ($('.advantageCardLink').is(":visible")) {
        if (IsCity == "False")
            Common.utils.trackAction('CWNonInteractive', 'deals_mobile', ' dealsimpression_mobile', 'modelpage_carsuggestions_mob');
        else
            Common.utils.trackAction('CWNonInteractive', 'deals_mobile', ' dealsimpression_mobile', 'priceincitypage_carsuggestions_mob');
    }
    tracking.trackBhriguImpression();
    registerSwiperCarousal();
};

var collectImpTrackData = {

    getCategory: function () {
        return IsCity == 'True' ? 'CityPage' : 'ModelPage';
    },
    getAction: function () {
        return IsCity == 'True' ? 'CityImpression' : 'ModelImpression';
    },
    getLabel: function () {
        label = "modelid=" + (window.newFlag ? CarDetails.carModelId : "")  + "|cityid=" + userCityId + "|source=43";
        label = label + (isPriceShown == 'True' ? "|isorpshown=1" : '');
        label = label + (window.campaignId > 0 ? "|campaignid=" + campaignId + "|iscampaignshown=1|campaignpanel=" + sponsorDlrLeadPanel + "|campaigntype=1" : '');
        return label;
    }
}

$(function () {
    $("#divFullSynopsis").find("img").addClass("fullWidth");
    $('#divShortDescContent > p').contents().unwrap();
    saveUserHistory();
    if (window.campaignId) {     
        if (userCityId == $.cookie("_CustCityIdMaster")) {
            zoneId = $.cookie("_CustZoneIdMaster") || "";
        }
    }
    $(document).on("mastercitychange", function (event, cityName, cityId, item) {
        masterCityChange(cityName, cityId, item);
    });
    PageLoad();
});

function isCityDuplicate(item) {
    return item != undefined && item != null && item.payload.isDuplicate && (item.payload.cityId != 599 && item.payload.cityId != 1358);
}

function masterCityChange(cityName, cityId, item) {
    var url = window.location.href;
    if (url.split("price-in").length == 2) {
		url = url.split("price-in")[0] + ((item != undefined) ? "price-in-" + item.cityMaskingName + "/" : "");
        window.location.href = url;
        return false;
    }
    else if (url.split("price-in").length == 1) {
        window.location.reload();
    }
}

var shortReviewPosition;
function FullReview() {
    $("#divShortSynopsis").hide();
    $("#divFullSynopsis").show().find('img.lazy').lazyload();
    var scollTop = $(window).scrollTop();
    shortReviewPosition = scollTop;
}

function HideReview() {
    $("#divFullSynopsis").hide();
    $("#divShortSynopsis").show();
    if (shortReviewPosition != null) {
        $(window).scrollTop(shortReviewPosition);
    }
}

function assignId(selectedCity) { selectedModelId = $(selectedCity).attr('modelid'); }

function PhotoGalleryGATrack() {
    dataLayer.push({ event: 'CWInteractive', cat: 'contentcons', act: 'Model-Page-Photo-Click', lab: CarDetails.carModelName });
}

function FloatingSlugTracking(OnRoadPriceBtn, CallDealerBtn, PageName, divPrice, callslugnumber) {
    var CityName = $.cookie("_CustCityMaster");
    if (divPrice && OnRoadPriceBtn && CallDealerBtn) { // ORP-Button-Clicked-Dual-Display
        dataLayer.push({ event: "FloatingSlugTracking", cat: "Floating-Slug", act: "ORP-Button-Clicked-Dual-Display", lab: PageName + "-" + CarDetails.carModelName + "-" + CityName + "-" + window.campaignId || 0 });
        return;
    }
    if (divPrice && OnRoadPriceBtn && (!CallDealerBtn)) { // ORP-Button-Clicked-Solo-Display
        dataLayer.push({ event: "FloatingSlugTracking", cat: "Floating-Slug", act: "ORP-Button-Clicked-Solo-Display", lab: PageName + "-" + CarDetails.carModelName + "-" + CityName });
        return;
    }
    if (callslugnumber) { // Call-Button-Clicked
        dataLayer.push({ event: "FloatingSlugTracking", cat: "Floating-Slug", act: "Call-Button-Clicked", lab: PageName + "-" + CarDetails.carModelName + "-" + CityName + "-" + window.campaignId || 0 });
        return;
    }
    if (OnRoadPriceBtn && CallDealerBtn) {      // Shown-with-ORP-and-Call
        dataLayer.push({ event: "FloatingSlugTracking", cat: "Floating-Slug", act: "Shown-with-ORP-and-Call", lab: PageName + "-" + CarDetails.carModelName + "-" + CityName + "-" + window.campaignId || 0 });
    }
    if (OnRoadPriceBtn && (!CallDealerBtn)) { // Shown-with-only-ORPButton
        dataLayer.push({ event: "FloatingSlugTracking", cat: "Floating-Slug", act: "Shown-with-only-ORPButton", lab: PageName + "-" + CarDetails.carModelName + "-" + CityName });
    }
}

function saveUserHistory() {
    if (!futuristic && newFlag)
        trackUserHistory([CarDetails.carModelId]);
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

function AdBlockTracking() {
    if (window.adblockDetecter === undefined)
        dataLayer.push({ event: 'CWNonInteractive', cat: 'AdBlocker', act: 'Modelpage_m', lab: ModelName });
}

if (window.location.search.indexOf("vid=") >= 0 && defaultVersionId != undefined) {
    updateSelectedVersionCookie(defaultVersionId);
    window.location.search.indexOf("q=lead") < 0 && window.history.replaceState('', document.title, window.location.href.split('?')[0]);
}

if ($('#whatsNewDescription span.read-more-collapse-link').length) {
    var customReadMoreCollapse = new ReadMoreCollapse('#whatsNewDescription', {
        concatData: false,
        ellipsis: false,
        onExpandClick: function () {
            Common.utils.trackAction("CWInteractive", "CarWale_Opinion_whats_new_m", "more_clicked", ModelName);
            cwTracking.trackCustomData("CarWaleOpinionWhatsNew", "MoreClicked", ("modelid=" + Modelid + "|source=43"), false);
        },
        onCollapseClick: function () {
            Common.utils.trackAction("CWInteractive", "CarWale_Opinion_whats_new_m", "collapse_clicked", ModelName);
            cwTracking.trackCustomData("CarWaleOpinionWhatsNew", "CollapseClicked", ("modelid=" + Modelid + "|source=43"), false);
        }
    });
}


if ($('#verdictDescription span.read-more-collapse-link').length) {
    var verdictReadMoreCollapse = new ReadMoreCollapse('#verdictDescription', {
        concatData: false,
        ellipsis: false,
        onExpandClick: function () {
            Common.utils.trackAction("CWInteractive", "CarWale_Opinion_verdict_m", "more_clicked", ModelName);
            cwTracking.trackCustomData("CarWaleOpinionVerdict", "MoreClicked", ("modelid=" + Modelid + "|source=43"), false);
        },
        onCollapseClick: function () {
            Common.utils.trackAction("CWInteractive", "CarWale_Opinion_verdict_m", "collapse_clicked", ModelName);
            cwTracking.trackCustomData("CarWaleOpinionVerdict", "CollapseClicked", ("modelid=" + Modelid + "|source=43"), false);
        }
    });
}

if ($('#prosConsModelReview span.read-more-collapse-link').length) {
    var modelReviewReadMoreCollapse = new ReadMoreCollapse('#prosConsModelReview', {
        collapseText: 'Hide Review',
        concatData: false,
        ellipsis: false,
        onExpandClick: function () {
            Common.utils.trackAction("CWInteractive", "CarWale_Opinion_expert_review_m", "full_review_clicked", ModelName);
            cwTracking.trackCustomData("CarWaleOpinionExpertReview", "FullReviewClicked", ("modelid=" + Modelid + "|source=43"), false);
            $('#prosConsModelReview').find('img.lazy').lazyload();
        },
        onCollapseClick: function () {
            Common.utils.trackAction("CWInteractive", "CarWale_Opinion_expert_review_m", "hide_review_clicked", ModelName);
            cwTracking.trackCustomData("CarWaleOpinionExpertReview", "HideReviewClicked", ("modelid=" + Modelid + "|source=43"), false);
        }
    });
}

function getQueryStringValue(key) {
    var urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(key);
}

$(document).ready(function () {
    if (getQueryStringValue("showOfferUpfront") == "true") {
        var modelPageUrl = window.location.origin + window.location.pathname;
        window.history.replaceState(null, null, modelPageUrl);
        if ($(".offer-container").length == 0) {
            return;
        }
        AnimateRibbon.scrollToOffers();
    }
});

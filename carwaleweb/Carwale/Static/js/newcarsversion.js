$(document).ready(function () {
    if (newCar)
        userHistory.trackUserHistory([modelId]);

    init($('#lstOtherVersions'));
    if ($('#OtherVersions li a').length > 5) {
        $('#more').removeClass('hide').addClass('redirect-rt');
    }

    $(document).on("mastercitychange", function (event, cityName, cityId) {
        location.reload();
    });
    $(document).on("click", "table.features tr:first-child:has(span.versionmenu-icons)", function () {
        var span = $(this).find("span");
        if ($(this).has("span.minus-icon").length > 0) {
            span.removeClass("minus-icon").addClass("plus-icon")
            $(this).siblings().fadeOut(200);
        } else {
            span.removeClass("plus-icon").addClass("minus-icon");
            $(this).siblings().fadeIn(400);
        }
    });
    $('#more').click(function (e) {
        e.preventDefault();
        if ($(this).text() == "Show more") {
            showAll($('#lstOtherVersions'));
            $(this).text("Show less");
        } else {
            init($('#lstOtherVersions'));
            $(this).text("Show more");
        }
    });

    

    $("#version-tabs li").click(function () {
        var fuelType = $(this).html();
    });

    if (IsOverviewPage) {
        $('#overview').removeAttr("href");
    }
    selectSubNaviTab(window.location.hash.toLowerCase());

    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

    $("img.lazy-uc").lazyload();


    $("#luxuryCarImage,#luxuryCarTitle,#luxuryCarPriceKm").click(function (e) {
        dataLayer.push({ event: 'BBT_CarClick', cat: 'UsedCarOtherCWPages', act: 'BBT_CarClick' });
    });
    $("#showroomBtn").click(function (e) {
        dataLayer.push({ event: 'BBT_ShowroomClick', cat: 'UsedCarOtherCWPages', act: 'BBT_ShowroomClick' });
    });

    var prevNextHandleVar = Math.ceil(parseFloat($('#authorCarousel li').length));
    var prevNextHandleConst = prevNextHandleVar;
    var videoSlider = 1;
    prevNextDisplay(prevNextHandleConst);

    if ($("#divAdvantageLink").is(":visible")) {
        Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', "variantpagedesk");
    }

    if ($(".advantageCardLink").is(":visible"))   
        Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', 'variantpage_carsuggestions_desk');

    if ($('#advantageAd').is(':visible')) {
        Common.utils.trackAction('CWNonInteractive', 'deals_desktop', ' dealsimpression_desktop', 'variantpage_sideslugdesk');
    }

    tracking.trackBhriguImpression();

    initModelOverviewPrice();
    initCityPopUp();
    if (Number(VersionId) > 0) updateSelectedVersionCookie(Number(VersionId));

    if ($('.selectcustom-container').length) {
        var dropdown = new Dropdown('.selectcustom-container');
    }
});

var collectImpTrackData = {

    getCategory: function () {
        return 'VersionPage';
    },
    getAction: function () {
        return 'VersionImpression';
    },
    getLabel: function () {
        label = "modelid=" + modelId + "|versionid=" + VersionId + "|cityid=" + $.cookie("_CustCityIdMaster") + "|source=1";
        label = label + (isPriceShown == 'True' ? "|isorpshown=1" : '');
        label = label + (campaignId > 0 ? "|campaignid=" + campaignId + "|iscampaignshown=1|campaignpanel=" + sponsorDlrLeadPanel + "|campaigntype=1" : '');
        return label;
    }
}

function init(list) {
    var step = 5, from = 0;
    list
    .find('li').hide().end()
    .find('li:lt(' + (from + step) + '):not(li:lt(' + from + '))')
    .show();
}
function showAll(list) {
    list.find('li').show().end();
}

function versionDropDownClickHandler() {
    Common.utils.showLoading();
    var version = $(this);
    var versionUrl = location.href;
    var splitUrl = versionUrl.split("/");
    splitUrl[splitUrl.length - 2] = version.data("vermaskingname");
    location.href = splitUrl.join('/');
}

$(".versiondrp__item").click(versionDropDownClickHandler);
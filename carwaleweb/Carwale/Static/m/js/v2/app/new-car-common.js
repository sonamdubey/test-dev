var PriceStatusEnum= {
    PriceAvailable: 1,
};
var vwGroupMakeIdArray = typeof (vwGroupMakeIds) == "undefined" ? [] : vwGroupMakeIds;

function bindModelCity(selectedModelId, drpCityAttrId) {
    $('#' + drpCityAttrId).empty();
    if (isEligibleForORP) {
        bindModelCityCallBack([{ "CityId": +$.cookie('_CustCityIdMaster') }], drpCityAttrId);
    }
    else {
        $.ajax({
            type: 'GET',
            url: '/webapi/GeoCity/GetPQCitiesByModelId/?modelid=' + selectedModelId,
            dataType: 'Json',
            success: function (json) {
                bindModelCityCallBack(json, drpCityAttrId);
                hidePrefilLoading();
            }
        });
    }
}
var pqLocation = new Object();
function bindModelCityCallBack(json, drpCityAttrId) {

    if (isCityFoundInCookies() && CookieCityValidForModel(json) && (!isNaN(selectedModelId) && selectedModelId != null)) {

        $.cookie('_PQModelId', selectedModelId, { path: '/' });
        $.cookie('_PQPageId', "1", { path: '/' });

        if (prefill == prefillEnum.byversion) {
            if (typeof (window.versionId) != "undefined") $.cookie('_PQVersionId', window.versionId, { path: '/' });
        } else {
            $.cookie('_PQVersionId', null, { path: '/' });
        }

        window.location.href = (window.location.pathname.indexOf('/m/') == 0 ? "/m/research/quotation.aspx" : "/new/quotation.aspx");

        return true;
    }

    selectCityByCookies(drpCityAttrId, json);
}

function selectCityByCookies(drpCityAttrId, json) {
    getCityAndZonesFromCookie(drpCityAttrId, json);
    if (isEligibleForORP)
        preSelectCityDiv(pqLocation.cityName, drpCityAttrId);
    else {
        // TODO:Bangalore Zone Refactoring
        if (pqLocation.cityName != "Mumbai" && pqLocation.cityName != "New Delhi" && pqLocation.cityName != "Bengaluru") preSelectCityDiv(pqLocation.cityName, drpCityAttrId);
        else preSelectCityDiv("Select City", drpCityAttrId);
    }
}

function getCityAndZonesFromCookie(drpCityAttrId, json) {
    if (isCookieExists('_CustCityIdMaster') && Number($.cookie('_CustCityIdMaster')) > 0 && (cityValidForModel(json, $.cookie('_CustCityIdMaster')))) {
        pqLocation.cityId = $.cookie('_CustCityIdMaster');
        pqLocation.zoneId = Number($.cookie('_CustZoneIdMaster')) > 0 ? $.cookie('_CustZoneIdMaster') : '';
        if (isEligibleForORP)
            pqLocation.cityName = $.cookie('_CustCityMaster');
        else pqLocation.cityName = Number($.cookie('_CustZoneIdMaster')) > 0 && isCookieExists('_CustZoneMaster') ? $.cookie('_CustZoneMaster') : $.cookie('_CustCityMaster');
    }
    else {
        $('#' + drpCityAttrId).text('Select City');
        pqLocation.cityId = '-1';
        pqLocation.zoneId = '';
        pqLocation.cityName = 'Select City';
    }

    // global variable value assigned
    CityId = pqLocation.cityId;
    ZoneId = pqLocation.zoneId;

    return pqLocation;

}

function preSelectCityDiv(cityName, drpCityAttrId) {
    $('#' + drpCityAttrId).text(cityName);
}

function isCityFoundInCookies() {
    if (((isCookieExists('_CustCityIdMaster') && Number($.cookie('_CustCityIdMaster')) > 0)) && prefill != prefillEnum.nofill)
        return true
    else
        return false
}

function CookieCityValidForModel(json) {
    if (cityValidForModel(json, $.cookie('_CustCityIdMaster')) && prefill != prefillEnum.nofill) return true;
    return false;
}


function cityValidForModel(json, cookieCity) {
    for (var i = 0; i < json.length; i++) {
        if (json[i].CityId == parseInt(cookieCity)) return true;
    }
    return false;
}

function setCityCookie(drpCity) {
    var selectedCityId = CityId;
    var selectedZoneId = ZoneId;
    var selectedCityName = drpCity.text();
    document.cookie = '_CustCityId=' + selectedCityId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustCity=' + selectedCityName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_PQZoneId=' + selectedZoneId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';

}

function assignId(selectedCity) { selectedModelId = $(selectedCity).attr('modelid'); }

function setCityIdCookieModelPage(btn) {
    var pageId = $(btn).attr("pageId");
    var isCityPage = $(btn).attr('isCityPage').toLowerCase();
    var cityId = $(btn).attr('cityId');

    // TODO:Bangalore Zone Refactoring
    if (isCityPage == "true" && cityId != NewCar.PQ.city.MUMBAI && cityId != NewCar.PQ.city.DELHI && cityId != NewCar.PQ.city.BANGALORE) {
        document.cookie = '_CustCityId=' + cityId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        document.cookie = '_CustCity=' + $(btn).attr('cityName') + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    }
}

// tracking user history
function trackUserHistory(modelIdArray) {
    try {
        if (isCookieExists('_userModelHistory')) {
            var userHistory = getUserModelHistory();
            var userHistoryArray = userHistory.split(",");

            userHistoryArray = insertIntocookie(userHistoryArray, modelIdArray);

            document.cookie = '_userModelHistory=' + userHistoryArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        } else {

            document.cookie = '_userModelHistory=' + modelIdArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';

            var userHistory = getUserModelHistory();
            var userHistoryArray = userHistory.split("~");

            if (userHistoryArray.length > 20) {
                userHistoryArray.splice(0, modelArrayLen - 20);
                document.cookie = '_userModelHistory=' + userHistoryArray.join("~") + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            }
        }
    }
    catch (e) {
        console.log("exception in track user history");
    }
}

function insertIntocookie(userHistoryArray, modelIdArray) {
    try {
        var modelArrayLen = modelIdArray.length;
        var isModelInserted;

        for (var modelIndex = 0; modelIndex < modelArrayLen; modelIndex++) {
            isModelInserted = false;
            for (var userHisIndex = 0; userHisIndex < userHistoryArray.length; userHisIndex++) {
                if (modelIdArray[modelIndex] == userHistoryArray[userHisIndex]) {
                    userHistoryArray.splice(userHisIndex, 1);
                    userHistoryArray.push(modelIdArray[modelIndex]);
                    isModelInserted = true;
                    break;
                }
            }
            if (!isModelInserted) {
                if (userHistoryArray.length == 20) {
                    userHistoryArray.splice(0, 1);
                }
                userHistoryArray.push(modelIdArray[modelIndex]);
            }
        }
        return userHistoryArray;
    }
    catch (e) {
        console.log("exceptionin insert into cookie");
    }
}

function getUserModelHistory() {
    if (isCookieExists('_userModelHistory')) {
        var userHistoryString = $.cookie('_userModelHistory');
        var userHistory = userHistoryString.split('~').join(',');
        return userHistory;
    } else {
        return "";
    }
}

var NewCar = {
    advantage: {

        tracking: function () {
            $('.trackAction').each(function () {
                element = $(this);
                if (IsCity == "False")
                    cwTracking.trackAction('CWNonInteractive', 'deals_mobile', 'dealsimpression_mobile', 'modelpage_variantlistmob');
                else
                    cwTracking.trackAction('CWNonInteractive', 'deals_mobile', 'dealsimpression_mobile', 'priceincitypage_variantlistmob');
            });
        },

    },
    PQ: {
        city: {
            MUMBAI: 1,
            DELHI: 10,
            BANGALORE: 2 // TODO:Bangalore Zone Refactoring
        }
    },

    PicPage: {
        showHideEmiAndCampaignSlug: function (priceStatus) {
            var isAdAvailable = $(".price-breakup__emi").attr("data-dealercampaignexist") != undefined ? $(".price-breakup__emi").attr("data-dealercampaignexist").toLowerCase() : false;
            if (isAdAvailable === "true" || priceStatus === PriceStatusEnum.PriceAvailable) {
                $(".price-breakup__emi").show();
            } else {
                $(".price-breakup__emi").hide();
            }
        }
    }
}

function closeModelVersionPopup() {
    $('.versionDiv').addClass('hide');
    unlockPopup();
}

var tracking = {
    trackBhriguImpression: function () {
        cwTracking.trackCustomData(collectImpTrackData.getCategory(), collectImpTrackData.getAction(), collectImpTrackData.getLabel(), false);
    }
}
var openModelVersionPopup = function (modelId,cityId,isCityPage,isVersionPage) {
    if (!($("#versionListPopup .versionDiv").length > 0)) {
        $.get("/m/versionpopup/" + modelId, { cityId: cityId, isCityPage: (isCityPage == "True"), isVersionPage: (isVersionPage == "True"), makeId: CarDetails.carMakeId }).done(
            function (response) {
                if(response) $("#versionListPopup").append(response);
            });
    }
    else
        $(".versionDiv").removeClass("hide");

    lockPopup();
};

var modelVersionSelected = function (versionDetails, isVersionPage) {
    var versionName = versionDetails.find("#versionName").text();
    var vid = versionDetails.attr("data-verid");
    updateSelectedVersionCookie(vid);
    defaultVerId = vid;
    defaultVerName = versionName;
    defaultVersionId = vid;

    getOfferAndDealerAdSlug(vid);
    
    if (versionDetails.attr("data-breakup") || isVersionPage == "False") {
        closeModelVersionPopup();
        var loadingAndBlackout = $('#cwmLoadingIcon', '#globalPopupBlackOut');
        loadingAndBlackout.show();
        
        $(".newcar-version .city-version-content").text(versionName);
        if (versionDetails.attr("data-breakup")) {
            bindBreakUpOnVersionChange(versionDetails);            
        }
        else if(isVersionPage == "False"){
            bindPriceOnVersionChange(versionDetails);
            $(".select-city-link").css("cursor", "pointer");
        }
        if (Number(versionDetails.attr('data-price-status')) === PriceStatusEnum.PriceAvailable) {
            EmiCalculatorExtended.showEmiCalcSlug(versionDetails, vid, versionName);
            if (vwGroupMakeIdArray.includes(CarDetails.carMakeId)) {
                EmiCalculatorExtended.getThirdPartyEmiDetails(Number(vid), false, Number(versionDetails.attr('data-downpaymentmaxvalue')));
            }
        }
        else {
            EmiCalculatorExtended.hideEmiCalcSlug();
        }

        NewCar.PicPage.showHideEmiAndCampaignSlug(Number(versionDetails.attr('data-price-status')));
        loadingAndBlackout.hide();
    }
    else {
        var versionUrl = location.href;
        var slashIndex = versionUrl.split("/");
        slashIndex[slashIndex.length - 2] = versionDetails.attr("data-verForUrl");
        location.href = slashIndex.join('/');
    }
};
   
var bindBreakUpOnVersionChange = function (versionDetails) {
    var breakUp = versionDetails.attr("data-breakup").split("|");
    var versionOrp = breakUp.pop().split(":")[1];
    $(".price-breakup__table .break-up").remove();
    if (breakUp.length > 0) {
        var breakUpHtml = "";
        for (var i = 0; i < breakUp.length; i++) {
            var keyValue = breakUp[i].split(":");
            breakUpHtml += "<tr class='break-up " + (i == breakUp.length - 1 ? "bottomline'" : "'") + "><td>" + keyValue[0] + (keyValue[0] == "Insurance" ? " <a rel='noopener nofollow' href='/m/insurance/?car=" + versionDetails.attr("data-verid") + "&cityid=" + userCityId + "&utm=mpicpage' class='text-link text-bold'>(Buy Policy Now)</a>" : "") + "</td><td class='text-right'>&#8377; " + keyValue[1] + "</td></tr>"
        }
        $(".price-breakup__table").prepend(breakUpHtml);
        $(".price-total").show();
    } else {
        $(".price-total").hide();
    }
    $(".version-orp").html("&#8377; " + (versionOrp == "0" ? "N/A" : versionOrp));
    changePriceLabel($(".price-label"),versionDetails);
}
var changePriceLabel = function (priceLabel, versionDetails) {
    priceLabel.attr("class").split(" ").forEach(function (input) {
        if (input.match("Text$")) priceLabel.removeClass(input);
    });
    priceLabel.find(".reason-text").html(versionDetails.attr("data-tooltipHtml"));
    priceLabel.addClass(versionDetails.attr("data-labelColor"));
    priceLabel.find(".label-text").text(versionDetails.attr("data-priceLabel"));
    btObj.registerEvents();
    btObj.registerEventsClass();
}

var updateEmi = function (emiBlock, emiTextClass, emiValue) {
    if(parseInt(emiValue) > 0){
        emiBlock.show();
        emiBlock.find(emiTextClass).html("&#8377; " + emiValue + "/-");
        $(".price-breakup__emi").show();
    }
    else {
        emiBlock.hide();
        $("#divEmiAssistance").length == 0 && $(".price-breakup__emi").hide();
    }
}

var bindPriceOnVersionChange = function (versionDetails) {
    var priceEmiBlock = $(".price-emi-block");
    changePriceLabel(priceEmiBlock.find(".price-label"), versionDetails);
    priceEmiBlock.find(".version-price").text(versionDetails.attr("data-verPrice"));
    var priceBreakup = priceEmiBlock.find(".price-breakup-link");
    (versionDetails.attr("data-price-status") == 1) ? (priceBreakup.show(), priceBreakup.attr("versionid", versionDetails.attr("data-verId"))) : priceBreakup.hide();
};
function registerSwiperCarousal() {
    var swiper = $('.carmodel-image-swipper-container').swiper({

        nextButton: $(document).find('.model-swiper-next-btn'),
        prevButton: $(document).find('.model-swiper-prev-btn'),
        lazyLoadingInPrevNext: true,
        lazyLoadingInPrevNextAmount: 2,
        slidesPerView: 1,
        lazyLoading: true,
		loop: false,
		spaceBetween: 0
    });

    if (swiper) {
        swiper.on('onSlideNextEnd', function () {
            var element = $('div.carmodel-image-swipper li.swiper-slide-active');
            if (!element.data('tracked'))
            {
                element.data('tracked', true);
                cwTracking.trackAction('CWInteractive', 'contentcons', 'msite_model_image_carousel_index', element.data('index').toString());
            }
            cwTracking.trackAction('CWInteractive', 'contentcons', 'msite_model_image_carousel_next', window.ModelName || "");
        });

        swiper.on('onSlidePrevEnd', function () {
            cwTracking.trackAction('CWInteractive', 'contentcons', 'msite_model_image_carousel_prev', window.ModelName || "");
        });
    }
}

function updateSelectedVersionCookie(versionId) {
    var versionState = $.cookie("versionstate");
    versionState = versionState || "{}";
    versionState = JSON.parse(versionState);
    versionState[CarDetails.carModelId] = versionId;
    document.cookie = "versionstate=" + JSON.stringify(versionState) + ";path=/";
}

$(document).ready(function () {
	$('.toll-free__cross-icon').click(function () {
		$('.toll-free-slug').hide();
	});
	if (typeof EmiCalculator !== "undefined") {
	    EmiCalculator.EmiCalculatorDocReady();
	}
	if (typeof EmiCalculatorExtended !== "undefined") {
	    EmiCalculatorExtended.setInitialEMIModelResult();
	}
	if (typeof isModelCityPage !== "undefined" && isModelCityPage.toLowerCase() === "true") {
	    NewCar.PicPage.showHideEmiAndCampaignSlug(Number($(".price-breakup__emi").attr("data-price-status")));
	}

	$(document).on('click', '.checkYourEmiLink__js', function () {
	    new globalLocation.BL().openLocHint();
	});

	$('.offerupfront__js').click(function () {
	    var offerUpfrontUrl = $(this).data("offerupfronturl");
	    if (offerUpfrontUrl) {
	        window.location.href = offerUpfrontUrl;
	    }
	});
});

function getOfferAndDealerAdSlug(versionId) {
    var offerCampaignId = $('.campaign-offer-slug').attr('data-campaignid');
    var campaignId = (offerCampaignId != null) ? offerCampaignId : campaignId;
    Loader.showOxygenLoaderOnSection($('.campaign-offer-slug'));
    $.ajax({ 
        
        url: "/OfferAndDealerAd/?inputVersionId=" + versionId + "&inputPageId=" + pageId + "&inputCampaignId=" + (typeof campaignId == "undefined" ? 0 : campaignId) + "&inputCityId=" + userCityId,

        type: 'GET',
        complete: function (response) {
            Loader.hideOxygenLoaderFromSection($('.campaign-offer-slug'));
            if (response != null && response.responseText != null && response.responseText != "") {
                $('.campaign-offer-slug').html(response.responseText);
                OpenOffers.registerEvents();
                ShowToolTip.registerEvents();
                AnimateRibbon.registerEvents();
                PositionRibbon.registerEvents();
                AnimateIcons.registerEvents();
                var offerCampaignTemplate = document.getElementById('offerCampaignTemplate');
                if (offerCampaignTemplate != null) {
                    window.registerCampaignEvent(offerCampaignTemplate);
                }
                Common.utils.trackImpressionsBySection('.campaign-offer-slug');
                var offerAndDealerAdSection = document.getElementsByClassName("deal-link")[0];
                if (offerAndDealerAdSection != null) {
                    window.registerCampaignEvent(offerAndDealerAdSection);
                }
            } else {
                $('.campaign-offer-slug').html('');
            }
        },
        error: function (ex) {
            Loader.hideOxygenLoaderFromSection($('.campaign-offer-slug'));
            $('.campaign-offer-slug').html('');
        }
    });
}


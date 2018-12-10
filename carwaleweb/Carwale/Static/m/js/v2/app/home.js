newModelId = newMakeName = newModelName = null;
var objNewCars = new Object();
var focusedMakeModel = null;
var NewPricemodelId = null;
var CityId = "-1";
var ZoneId = "";

var prefillEnum = new Object();
prefillEnum.nofill = 0;
var prefill = prefillEnum.nofill;
// global-search code 
$("#newCarsList").click(function () {
    $("#global-search-popup-pq").removeClass('hide');
    $("#global-search-popup-pq input").focus();
    $.when(GetGlobalSearchCampaigns.bindCampaignData(true)).then(function () {
        $('.global-search-section').show();
    });
    GetGlobalSearchCampaigns.logImpression('#global-search-popup-pq', 'trending', true);
    lockPopup();
    voiceSearchSlug();
}).focus(function () {
    trackHomePage('HP_Search_icon_click', 'HP_Search_icon_click');
});
//#newCarsList

var globalSearchPQInputField = $('#globalSearchPQ');

$(globalSearchPQInputField).cw_easyAutocomplete({
    inputField: globalSearchPQInputField,
    isNew: 1,
    isNcf: 1,
    additionalTypes: 8,
    resultCount: 5,
    isOnRoadPQ: 1,
    pQPageId: 79,
    currentresult: [],
    textType: ac_textTypeEnum.model + ',' + ac_textTypeEnum.make,
    source: ac_Source.generic,
    onClear: function () {
        objNewCars = {};
        globalSearchAdTracking.featuredModelIdPrev = 0;
    },

    click: function (event) {        
        suggestions = {
            position: globalSearchPQInputField.getSelectedItemIndex() + 1,
            count: objNewCars.result.length
        };

        var selectionValue = globalSearchPQInputField.getSelectedItemData().value,
            splitVal = selectionValue.split('|'),
            label = globalSearchPQInputField.getSelectedItemData().label;
        
        if (label.indexOf(' vs ') > 0) {
            var model1 = "|modelid1=" + splitVal[0].split(':')[1];
            var model2 = "|modelid2=" + splitVal[1].split(':')[1];
            trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, label, (model1 + model2));
            Common.redirectToComparePage(splitVal);
            return false;
        }
        var make = {};
        make.name = splitVal[0].split(':')[0];
        make.id = splitVal[0].split(':')[1];

        var sourceElement = $(event.srcElement);
        if (event.srcElement == undefined) {
            sourceElement = $(event.originalEvent.target);
        }
        var modelLabel = "|modelid=" + splitVal[1].split(':')[1];
        if (sourceElement.hasClass('OnRoadPQ')) {
            var pqSearchLabel = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name + '_' + splitVal[1].split(':')[0];
            trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, (make.name + ' ' + splitVal[1].split(':')[0]), modelLabel);
            trackHomePage('NewCars-Successful-Selection-PQlink-Click', pqSearchLabel);  //add tracking
            return false;
        }
        if (selectionValue.indexOf('sponsor') > 0) {
            var sponsorLabel = globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName + '_' + suggestions.count + "_" + suggestions.position;
            trackBhriguSearchTracking('Home', 'Sponsored', suggestions.count, suggestions.position, suggestReqTerm, (globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName), modelLabel);
            trackGlobalSearchAd('New_m_Click', sponsorLabel, 'SearchResult_Ad_m');
            cwTracking.trackCustomData('SearchResult_Ad_m', 'New_m_Click', sponsorLabel, false);
        }
        if (selectionValue.indexOf('mobilelink:') > 0) {
            var mobilelineLabel = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + label;
            trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, label);
            trackHomePage('NewCars-Successful-Selection-Value-Click', mobilelineLabel);
            window.location.href = selectionValue.split("mobilelink:")[1].split("|")[0];
            return false;
        }

        if (splitVal[0].split(':')[0] === 'ncfLink') {
            trackBhriguSearchTracking('', '', suggestions.count, suggestions.position, suggestReqTerm, splitVal[0].split(':')[1]);
            Common.utils.trackAction('CWInteractive', 'NCFLinkageCategory', 'NCFGlobalsearchlinkclick', suggestReqTerm + '_' + suggestions.position + '/' + suggestions.count);
            window.location.href = splitVal[0].split(':')[1];
            return false;
        }

        var model = null;
        if (splitVal[1] != undefined && splitVal[1].indexOf(':') > 0) {
            model = {};
            model.name = splitVal[1].split(':')[0];
            model.id = splitVal[1].split(':')[1];
        }
        globalSearch(make, model);
    },

    afterFetch: function (result, searchText) {
        objNewCars.result = result;
        suggestReqTerm = searchText;
        if ((result.filter(function (suggestion) { return suggestion.value.toString().indexOf('sponsor') > 0 })).length > 0) {
            globalSearchAdTracking.trackData(result, 'SearchResult_Ad_m');
        }
        else {
            globalSearchAdTracking.featuredModelIdPrev = 0;
        }

        if (result.find(function (item) { return item.value.split(':')[0] === 'ncfLink';})) {
            //Impression tracking Here
            Common.utils.trackAction('CWNonInteractive', 'NCFLinkageCategory', 'NCFGlobalsearchlinkdisplayed', searchText);
        }
    },

    keyup: function () {
        if (globalSearchPQInputField.val().length != 0) {
            $("#gs-text-clear-pq").show();
            $('.global-search-section').hide();
        }
        else {
            $("#gs-text-clear-pq").hide();
        }
    },

    focusout: function () {
        globalSearchAdTracking.featuredModelIdPrev = 0;
        trackHomePage('HP_Search_outside_click', 'HP_Search_outside_click');
    }
});

function btnFindCarNewNav() {
    if (focusedMakeModel == undefined || focusedMakeModel == null)
        return false;
    if (focusedMakeModel.label.toLowerCase().indexOf(' vs ') > 0) {
        Common.redirectToComparePage(focusedMakeModel.id.split('|'));
        return true;
    }
    var splitVal = focusedMakeModel.id.split('|');
    var make = new Object();
    make.name = splitVal[0].split(':')[0];
    make.id = splitVal[0].split(':')[1];
    var model = null;
    if (splitVal[1] != undefined && splitVal[1].indexOf(':') > 0) {
        model = new Object();
        model.name = splitVal[1].split(':')[0];
        model.id = splitVal[1].split(':')[1];
    }
    return globalSearch(make, model);
}

$('#btnFindCarNew').on('click', function () {
    //btnFindCarNewNav() ? "" : window.location.href = '/m/new/';
    if (!btnFindCarNewNav()) {
        trackHomePage('NewCars-No-Result-Search-Find-Car', $('#newCarsList').val());
        window.location.href = '/m/new/';
    }
});

function checkForCity(cityId, drpCity, divToBind) {
    if (drpCity == 'drpPqCity') {
        if ($('#' + divToBind).find("#" + drpCity + " option[value='" + cityId + "']").length > 0)
            return true;
        return false;
    }
    else {
        if ($("#" + drpCity + " option[value='" + cityId + "']").length > 0)
            return true;
        return false;
    }
}

function globalSearch(make, model) {
    var userInput = '';
    if (model != null && model != undefined) {
        userInput = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name + ' ' + model.name;
        trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, (make.name + ' ' + model.name), ('|modelid=' + model.id));
        trackHomePage('NewCars-Successful-Selection-Value-Click', userInput);
        window.location.href = '/' + make.name + '-cars/' + model.name + '/';
        $('#btnFindCarNew').unbind("click");
        return true;
    }
    if (make != null && make != undefined) {
        userInput = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name;
        trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, make.name);
        trackHomePage('NewCars-Successful-Selection-Value-Click', userInput);
        window.location.href = '/m/' + make.name + '-cars/';
        $('#btnFindCarNew').unbind("click");
        return true;
    }
    return false;
}

function preselectPQDrpCity(idDrpCity) {
    var cityId = $.cookie('_CustCityId');
    var zoneId = $.cookie('_PQZoneId');
    if (zoneId != "") {
        $('#' + idDrpCity + ' option[zoneid=' + zoneId + ']').attr('selected', 'selected');
    }
    else {
        $('#' + idDrpCity).val(cityId);
    }
}

function getPQ(event, pageId) {
    var erricon = $(event).parent().find('.error-icon');
    var tooltip = $(event).parent().find('.cw-blackbg-tooltip');
    var flipDrpCity = $(event).parent().find('div[id^="drpCity_"]');
    var versionId = $(event).attr("versionid") || 0;
    
    if (flipDrpCity.text() != "Select City") {
        erricon.addClass('hide'); tooltip.addClass('hide');
        $.cookie('_PQPageId', pageId, { path: '/' });
        $.cookie('_PQVersionId', versionId, { path: '/' });
        setCityCookie(flipDrpCity);
        window.location.href = '/m/research/quotation.aspx';
    }
    else {
        erricon.removeClass('hide'); tooltip.removeClass('hide'); return false;
    }
}

function BindEvents() {
	$(".card").flip({
		axis: 'y',
		trigger: 'manual',
		reverse: true
	});
    $("#UNTCarouselContainer .closeBtn").click(function () {
        $(this).parents("li").flip(false);
    });
}

function UNTDrpChange(drp, container) {
    var selected_carousel = $(drp).val();
    var slides = '.swiper-container li.swiper-slide';
    if (selected_carousel == "Videos" && $.swiperYTApiApplied == undefined) {
        if (!navigator.userAgent.match(/iPhone|iPad|iPod/i)) {
            if ($('.swiper-wrapper iframe').length > 0)
                Swiper.YouTubeApi.addApiScript();
            $.swiperYTApiApplied = true;
        }
        topVideos.bindVideos(HomePage.topVideoContent);
    }
    UNTLazyLoad();
    selected_carousel = $("#" + container + " .cw-tabs-data[id=" + selected_carousel + "]");
    $("#" + container + " .cw-tabs-data").addClass("hide");
    selected_carousel.removeClass("hide");
    if ($(drp).val() == "Videos" && selected_carousel.find(slides).length > 1)
        selected_carousel.find(slides).removeClass('fullWidth');
    setTimeout(function () { selected_carousel.find('img.lazy:in-viewport').trigger("UNT"); }, 100);
    Common.utils.trackAction('M-Site-Homepage', 'featuredcardselection_mobile', 'featuredcardselection_mobile', $(drp).val());
}

$("#gs-close-pq").click(function () {
    trackHomePage('NewCar-Search-Back-Icon-Click', $('#globalSearchPQ').val());
    $("#globalSearchPopup").val("");
    $(".global-search-popup").addClass('hide');
    unlockPopup();
});

function UNTLazyLoad() {
    $("img.lazy").lazyload({
        event: "UNT"
    });
}
var objNewPrice = new Object();

var getFinalPriceInputField = $('#getFinalPrice');

function bindFinalPrice() {
    $(getFinalPriceInputField).cw_easyAutocomplete({
        inputField: getFinalPriceInputField,
        tooltip: $(getFinalPriceInputField).siblings('.cw-blackbg-tooltip'),
        errorIcon: $(getFinalPriceInputField).siblings(".error-icon"),
        resultCount: 5,
        //isNew: 1,
        isPriceExists: 1,
        textType: ac_textTypeEnum.model,
        source: ac_Source.generic,
        onClear: function () {
            this.tooltip.addClass('hide');
            this.errorIcon.addClass('hide');
            $("#drpCityFinalPrice").addClass('disable-div');
            objNewPrice = new Object();
            globalSearchAdTracking.featuredModelIdPrev = 0;
        },

        click: function (event) {
            suggestions = {
                position: getFinalPriceInputField.getSelectedItemIndex() + 1,
                count: objNewPrice.result.length
            };
            var selectionValue = getFinalPriceInputField.getSelectedItemData().value;

            objNewPrice.Name = getFinalPriceInputField.getSelectedItemData().label;
            objNewPrice.Id = selectionValue;
            NewPricemodelId = selectionValue.split(':')[2];

            $(getFinalPriceInputField).val(objNewPrice.Name);
            $('#getPQ').attr('modelid', NewPricemodelId);
            bindModelCity(NewPricemodelId, 'drpCityFinalPrice');
            $("#drpCityFinalPrice").removeClass('disable-div');
            hideFinalPriceErr();

            $.cookie('_PQModelId', NewPricemodelId, { path: '/' });
            $.cookie('_PQPageId', "72", { path: '/' });
            $.cookie('_PQVersionId', "0", { path: '/' });
            $("#drpCityFinalPrice").removeClass('border-red').siblings(".cw-blackbg-tooltip").addClass('hide').siblings(".error-icon").addClass('hide');

            if (selectionValue.indexOf('sponsor') > 0) {
                var sponsorLabel = globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName + '_' + suggestions.count + "_" + suggestions.position;                
                trackGlobalSearchAd('New_m_Click', sponsorLabel, 'SearchResult_Ad_PQ_m');
                cwTracking.trackCustomData('SearchResult_Ad_PQ_m', 'New_m_Click', sponsorLabel, false);
            }

            if (isEligibleForORP) {
                setRedirectUrl(objNewPrice.Id.split(':'));
            }
        },

        afterFetch: function (result, searchText) {
            objNewPrice.result = result;

            if (result != undefined && result.length > 0) {
                this.tooltip.addClass('hide');
                this.errorIcon.addClass('hide');
                $("#drpCityFinalPrice").removeClass('disable-div');
            }
            else {
                this.tooltip.removeClass('hide');
                this.errorIcon.removeClass('hide');
                $("#drpCityFinalPrice").addClass('disable-div');
            }
            if ((result.filter(function (suggestion) { return suggestion.value.toString().indexOf('sponsor') > 0 })).length > 0) {
                globalSearchAdTracking.trackData(result, 'SearchResult_Ad_PQ_m');
            }
            else {
                globalSearchAdTracking.featuredModelIdPrev = 0;
            }            
        },

        focusout: function () {
            if ((objNewPrice.Name == undefined || objNewPrice.Name == null || objNewPrice.Name == '') && objNewPrice.result != undefined && objNewPrice.result != null && objNewPrice.result.length > 0) {
                if (objNewPrice.result[0].label.toLowerCase().indexOf($(getFinalPriceInputField).val().toLowerCase()) != -1) {
                    objNewPrice.Name = formatSpecial(objNewPrice.result[0].label);
                    objNewPrice.Id = formatSpecial(objNewPrice.result[0].value);
                    $(getFinalPriceInputField).val(objNewPrice.result[0].label);
                    NewPricemodelId = objNewPrice.result[0].value.split(':')[2];

                    $('#getPQ').attr('modelid', NewPricemodelId);
                    $("#drpCityFinalPrice").removeClass('disable-div');
                    hideFinalPriceErr();
                }
            }
        }
    });
}
function hideFinalPriceErr() {
    $("#finalPriceContainer .error-icon,#finalPriceContainer .cw-blackbg-tooltip").addClass('hide')
}

function PQCityChange(select) {
    select = $('#' + $(select).attr('id') + " option:selected");
    var zoneId = $(select).attr('zoneid');
    zoneId = zoneId == undefined ? "" : zoneId;
    var cityId = $(select).val();
    var cityName = $(select).text();

    if (Number(cityId) > 0) {
        $.cookie('_CustCity', cityName, { path: '/' });
        $.cookie('_CustCityId', cityId, { path: '/' });
    }
    $.cookie('_PQZoneId', zoneId, { path: '/' });
}
function finalPQClick(e) {
    if ($.trim($("#getFinalPrice").val()) != "" && $.trim($("#getFinalPrice").val()) != $("#getFinalPrice").attr('placeholder')) {

        if ($("#drpCityFinalPrice").text() != "Select City") {
            $("#drpCityFinalPrice").removeClass('disable-div');
            $("#getFinalPrice").removeClass('border-red').siblings(".cw-blackbg-tooltip").addClass('hide').siblings(".error-icon").addClass('hide');
            setCityCookie($("#drpCityFinalPrice"));
            if (!isEligibleForORP) {
                location.href = "/m/research/quotation.aspx";
            }
            else {
                Common.utils.trackAction('CWInteractive', 'ShowORPTest', "CORPWidget_BtnClick", $("#getFinalPrice").val() + "-" + $.cookie("_CustCityMaster"));
                location.href = $(this).attr("data-redirect-url");
            }
        }
        else
            $("#drpCityFinalPrice").addClass('border-red').siblings(".cw-blackbg-tooltip").removeClass('hide').siblings(".error-icon").removeClass('hide');
    }
    else
        $("#getFinalPrice").addClass('border-red').siblings(".cw-blackbg-tooltip").removeClass('hide').siblings(".error-icon").removeClass('hide');
}

function trackHomePage(action, label) {
    dataLayer.push({ event: 'M-Site-Homepage', cat: 'FirstPanel-Mobile-HP', act: action, lab: label });
}
//FirstPanel-Mobile-HP

//Tracking for Popular Used car Section
function trackPopularUCHomePage(action, label) {
    dataLayer.push({ event: 'M-Site-Homepage', cat: 'PopularUsed-Mobile-HP', act: action, lab: label });
}

//for PQ widget; to pass the selectedModelId 
function assignHomeModelId(selectedCity) {
    selectedModelId = $(selectedCity).parent().parent().find('#getPQ').attr('modelid');
}

//for carousel(flip cards); to pass the selectedModelId
function assignModelIdtoBtn(selectedCity) {
    selectedModelId = $(selectedCity).attr('modelid');
    selectedVersionId = $(selectedCity).attr('versionid') || "0";
    $.cookie('_PQModelId', selectedModelId, { path: '/' });   
    $.cookie('_PQVersionId', selectedVersionId, { path: '/' });
    
    if (HomePage.isHomePage) {
        $(selectedCity).hasClass('newlaunch') ? Common.utils.setSessionCookie('_PQPageId', 63) : Common.utils.setSessionCookie('_PQPageId', 61);
    } else {
        $(selectedCity).hasClass('newlaunch') ? Common.utils.setSessionCookie('_PQPageId', 67) : Common.utils.setSessionCookie('_PQPageId', 62);
    }
}

function setRedirectUrl(carData) {
    var modelUrl = "/" + carData[0].toLowerCase() + "-cars/" + carData[1].split('|')[1].toLowerCase() + "/";
    $('#getPQ,#drpCityFinalPrice').attr("data-redirect-url", modelUrl);
}

/* top videos view model */
var topVideos = {
    bindVideos: function (json) {
        topVideos.videoViewModel.videos(json);
        ko.applyBindings(topVideos.videoViewModel, document.getElementById('Videos'));
    },
    videoViewModel: {
        videos: ko.observableArray([])
    }
}

var HomePageTrigger = (function () {
    function init() {
        $("#UNTDrp").val("upComingCars");
        $("#NRVDrp").val("News");
        $("#getFinalPrice").val("");

        $("#upComingCars .infoBtn ,#newLaunches .infoBtn,#topSelling .infoBtn").click(function (a, b, c) {
            var drpCity = $(this).parents('li').find('div[id^="drpCity_"]');
            $(this).parents('li').find('.error-icon').addClass('hide');
            $(this).parents('li').find('.cw-blackbg-tooltip').addClass('hide');
            $.cookie('_PQModelId', drpCity.attr("modelid"), { path: '/' });
            $.cookie('_PQVersionId', "0", { path: '/' });
            if (drpCity.attr("id") != undefined)
                bindModelCity(drpCity.attr("modelid"), drpCity.attr("id"));
        });

        bindFinalPrice();
        $(window).scroll(function () {
            if ($(this).scrollTop() > 40) {
                $('.header-fixed').addClass('header-fixed-with-bg');
            } else {
                $('.header-fixed').removeClass('header-fixed-with-bg');
            }
        });

        $("#more-brand-tab").click(function (e) {
			e.preventDefault();
			$(".brand-type-container ul").toggleClass("animate-brand-ul");
			$("html, body").animate({ scrollTop: $("#brand-nav").offset().top }, 1000);
            var b = $(this).find("span");
            b.text(b.text() === "more" ? "less" : "more");
        });
        if (typeof (HomePage.banner) != "undefined") {
            $('#newCarsList').val("");
            if (HomePage.banner.length > 0)
                $('#newCarsList').removeAttr('disabled').attr('placeholder', HomePage.banner);
            else
                $('#newCarsList').removeAttr('disabled').attr('placeholder', 'Type to select car name');
        }
        BindEvents();
        if (HomePage.isHomePage) {
            if (window.adblockDetecter === undefined)
                Common.utils.trackAction('CWNonInteractive', 'AdBlocker', 'Enabled_MSite', 'AdBlocker');
            else
                Common.utils.trackAction('CWNonInteractive', 'AdBlocker', 'Disabled_MSite', 'AdBlocker');
        }
    }
    return {
        init: init
    }
}());

function CarSelectionTab() {
    var offerLinkUnit = '.offer-link-unit';
    var selectedCarTab = '.selected-car-tab';
    var selectedCarTabUl = '.selected-car-tab ul';
    var selectedCarTabList = '.selected-car-tab li';
    var tabCarMenu = '.tab-car-menu';
    $(document).ready(function () {
        var firstTimeUser = false;
        var carSearchType = $.cookie('_carSearchType');
        if (carSearchType === null || carSearchType === undefined) {
            $(tabCarMenu).toggleClass('activeUl');
            firstTimeUser = true;
        }
        if ($(selectedCarTabList).hasClass('new-tab-text')) {
            $(offerLinkUnit).show();
            SetCookieInDays('_carSearchType', 1, 30);
        }
        if ((!$(offerLinkUnit).children('.advantage-link-unit').is(':visible')) && !$(offerLinkUnit).hasClass('hide')) {
            $(offerLinkUnit).addClass('right-offer-link');
        }
        $(selectedCarTab).on('click', function (e) {
            e.stopPropagation();
            $(tabCarMenu).toggleClass('activeUl');
        });

        $(document).on('click', '.tab-car-menu li.list-item', function (e) {
            e.stopPropagation();
            $(tabCarMenu).removeClass('activeUl');
            $(tabCarMenu).prepend($(selectedCarTabUl).html());
            $(selectedCarTabUl).html("").append($(this));
           
            //to maintain state for used and new car tab click. 0 for used and 1 for new
            SetCookieInDays('_carSearchType', $('.new-tab-text').hasClass('active') ? 1 : 0, 30);
            if ($(selectedCarTabList).hasClass('used-tab-text')) {
                $(offerLinkUnit).hide();
            }
            else {
                $(offerLinkUnit).show();
            }
        });
        $(document).on('click', function (e) {
            e.stopPropagation();
            $(tabCarMenu).removeClass('activeUl');
            if (firstTimeUser) {
                $(tabCarMenu).addClass('activeUl');
                firstTimeUser = false;
            }
        });
        //For handling of back navigation
        if (carSearchType == 1) {
            $(".tab-car-menu .new-tab-text").click();
        }
        else if (carSearchType == 0) {
            $(".tab-car-menu .used-tab-text").click();
        }
    });
    $("#getPQ").click(finalPQClick);
}
CarSelectionTab();
HomePageTrigger.init();
/* Used Cars Budget - Find carscode */
$("#btnFindCarHome").click(function () {
	var cityName = objUsedCar.Name;
	var searchUrl = "/m/used/cars-for-sale/";
	var custCityLandingpage = 0;//Since placeholder not 'Whats your location?'
	if (objUsedCar.Name)
		cityName = Common.utils.formatSpecial(objUsedCar.Name);
	var cityId = objUsedCar.Id;
	if (cityName != undefined) {
		if (cityId == "1")
			cityId = 3000;
		else if (cityId == "3001")
			cityId = 10;
		searchUrl += "?city=" + cityId;
		SetCookieInDays("_CustCityLandingPage", custCityLandingpage);
	}
	location.href = searchUrl;
});

$(".ncf__link").click(function () {
    Common.utils.trackAction("CWInteractive", "NCFLinkage", "NCFLinkage_click", $(this).text())
    window.location = "/find-car/";
});


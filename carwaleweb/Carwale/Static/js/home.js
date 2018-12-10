var newModelId = null;
var newModelName = null;
var newMakeName = null;
var NewPricemodelId = null;
var objNewCar = new Object();
var objNewPrice = new Object();

function hideErrorPQ(selector) {
    selector.find('.PriceCarErrIcn').hide();
    selector.find('.PriceCarErrMsg').hide();
    selector.find('#inputModelPQWidget').removeClass('border-red');
    selector.find('.error-icon').hide();
    selector.find('.cw-blackbg-tooltip').hide();
    selector.find('#inputCityPQWidget').removeClass('border-red');
}

$('#finalprice').on('click', function () {
    var cacheCityWidget = $('#inputCityPQWidget');
    var cacheModelWidget = $('#inputModelPQWidget');
    var cachePqWidgetDiv = $('.home-getFinalPrice-banner');
    var modelName = cacheModelWidget.val();
    modelName == cacheModelWidget.attr('placeholder') ? NewPricemodelId = null : "";

    if (cacheCityWidget.val() !== "" && cacheCityWidget.val() !== "Select City") {
        var cityId = cacheCityWidget.data("cityId");
        var cityName = cacheCityWidget.data("cityName");
        var areaId = cacheCityWidget.data("areaId");
        var areaName = cacheCityWidget.data("areaName");
        var locationObj;
        if (areaId > 0)
            locationObj = { cityId: cityId, cityName: cityName, areaId: areaId, areaName: areaName };
        else
            locationObj = { cityId: cityId, cityName: cityName };
    }

    if (Number(cityId) > 0 && Number(NewPricemodelId) > 0) {
        trackHomePage('PQWidget', 'Select-Car-Successful-button-Click', modelName + ' ' + cityName);
 
        var makeModelName = cacheModelWidget.attr("make-model");
        Common.autoComplete.redirectToModelPage(makeModelName);
    } else {
        ShakeFormView(cachePqWidgetDiv.find(".container"));
        if (NewPricemodelId == null) {
            showModelError(cacheModelWidget);
        }
        if (cityId === undefined || cityId == "-1") {
            showCityError(cacheCityWidget);
        }
        trackHomePage('PQWidget', 'Select-Car-UnSuccessful-button-Click', modelName + ' ' + cityName);
    }
});

function showModelError(selector) {
    selector.parent().find('.PriceCarErrIcn').show();
    selector.parent().find('.PriceCarErrMsg').show();
    selector.addClass('border-red');
}

function showHideDrpError(element, error) {
    if (error) {
        $(element[0]).removeClass('hide');
        $(element[1]).removeClass('hide');
    }
    else {
        $(element[0]).addClass('hide');
        $(element[1]).addClass('hide');
    }
}

function pushInquiry(event) {

    var modelId = $(event).attr("modelid");
    var nameEle = $(event).parent().find("input[name='userName']");
    var emailEle = $(event).parent().find("input[name='userEmail']");
    var mobileEle = $(event).parent().find("input[name='userMobile']");
    var err = ValidateContactDetails($.trim(nameEle.val()), $.trim(emailEle.val()), $.trim(mobileEle.val()));
    var isFormValid = true;

    if (err[0] == "") {
        nameEle.siblings().addClass('hide');
        nameEle.removeClass('border-red');
    }
    else {
        ShakeFormView($(".userNameInput"));
        nameEle.siblings().removeClass('hide');
        nameEle.addClass('border-red');
        var errSpan = nameEle.siblings()[1];
        $(errSpan).text(err[0]);
        isFormValid = false;
    }

    if (err[1] == "") {
        emailEle.siblings().addClass('hide');
        emailEle.removeClass('border-red');
    }
    else {
        ShakeFormView($(".userEmailInput"));
        emailEle.siblings().removeClass('hide');
        emailEle.addClass('border-red');
        var errSpan = emailEle.siblings()[1];
        $(errSpan).text(err[1]);
        isFormValid = false;
    }

    if (err[2] == "") {
        mobileEle.siblings('.cw-blackbg-tooltip, .error-icon').addClass('hide');
        mobileEle.removeClass('border-red');
    }
    else {
        ShakeFormView($(".userMobileInput"));
        mobileEle.siblings('.cw-blackbg-tooltip, .error-icon').removeClass('hide');
        mobileEle.addClass('border-red');
        var errSpan = mobileEle.siblings()[2];
        $(errSpan).text(err[2]);
        isFormValid = false;
    }
    if (!ValidateCityUpcoming(modelId)) {
        ShakeFormView($(".userCityInput"));
        isFormValid = false;
    }

    if (isFormValid) {
        PushCRMLead($.trim(nameEle.val()), $.trim(emailEle.val()), $.trim(mobileEle.val()), upcomingLeadCity.id, '', 4, '', '', modelId, '', event);
    }
    else {
        trackTNU('upcomingCars', 'Unsuccessful-validation-submit', modelId);
    }
}

//Push CRM Lead
function PushCRMLead(name, email, mobile, city, cityName, leadType, carName, makeId, modelId, versionId, event) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
        data: '{"carName":"' + carName + '", "custName":"' + name + '", "email":"' + email + '", "mobile":"' + mobile + '", "selectedCityId":"' + city + '", "versionId":"' + versionId + '", "modelId":"' + modelId + '", "makeId":"' + makeId + '", "leadtype":"' + leadType + '", "cityName":"' + cityName + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushCRM"); },
        success: function (response) {
            var responseObj = eval('(' + response + ')');
            if (responseObj.value) {
                $(event).parents("li").find('.formContent').addClass('hide');
                $(event).parents("li").find('.thankYouForm').removeClass('hide');
                trackTNU("upcomingCars", 'Successful-submit', cityName + "-" + modelId);
            }
            else {
                trackTNU("upcomingCars", 'Unsuccessful-submit', cityName + "-" + modelId);
            }
        }
    });
}

var objNewCar = new Object();
$(document).ready(function () {
    $('.new-cars-search').children('div').addClass('homepage-banner-search');
    PrefilCityUpcoming();
    $("#newCarsList").focus();
    UNTLazyLoad();
    var ulUpcoming = '#upComingCars';
    var ulNewLaunches = '#newLaunches';
    var topSelling = '#topSelling';
    var pageSize = 9;
    var cityIdCookie = $.cookie('_CustCityIdMaster');

    var pageNo = 1;
    $('#newCarsList').val('');
    $('#globalSearch').val('');
    $('#inputModelPQWidget').val('');
    $('#inputCityPQWidget').val('');
    var objNewCars = new Object();
    var focusedMakeModel = null;

    /* Isuzu zone homepage tracking */
    var label = "";
    $(".click-track-isuzu").on('click', function () {
        label = $(this).attr('data-label');
        dataLayer.push({
            event: 'CWInteractive',
            cat: 'HPSlug',
            act: 'isuzu d-max v-cross',
            lab: label
        });
    });
    /* Isuzu zone homepage tracking */

    $('#newCarsList').cw_autocomplete({
        isNew: 1,
        additionalTypes:8,
        isOnRoadPQ: 1,
        pQPageId: 57,
        resultCount: 10,
        currentresult: [],
        textType: ac_textTypeEnum.model + ',' + ac_textTypeEnum.make,
        source: ac_Source.generic,
        onClear: function () {
            objNewCars = new Object();
            globalSearchAdTracking.featuredModelIdPrev = 0;
        },
        click: function (event, ui, orgTxt) {
            var ul = event.originalEvent.target;
            if (ul != undefined) {
                suggestions = {
                    position: $(ul).find('li.ui-state-focus').index() + 1,
                    count: objNewCars.result.length
                };
            }
            var splitVal = ui.item.id.split('|');
            var label = ui.item.label.toLowerCase();//get label of suggest result
            if (label.indexOf(' vs ') > 0) {
                var model1 = "|modelid1=" + splitVal[0].split(':')[1];
                var model2 = "|modelid2=" + splitVal[1].split(':')[1];
                trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, ui.item.label, (model1 + model2));                
                Common.redirectToComparePage(splitVal);
                return false;
            }
            if (ui.item.id.indexOf('sponsor') > 0)
            {
                var modelLabel = "|modelid=" + splitVal[1].split(':')[1];
                var sponsorLabel = globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName + '_' + suggestions.count + "_" + suggestions.position;
                trackBhriguSearchTracking('Home', 'Sponsored', suggestions.count, suggestions.position, suggestReqTerm, (globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName), modelLabel);
                trackGlobalSearchAd('New_Click', sponsorLabel);
                cwTracking.trackCustomData('SearchResult_Ad', 'New_Click', sponsorLabel, false);
            }
            if (splitVal[0].indexOf('desktoplink:') == 0) {
                var desktoplinkLabel = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + ui.item.label;
                trackBhriguSearchTracking('Home', '',suggestions.count, suggestions.position, suggestReqTerm, ui.item.label);
                trackHomePage('FirstPanel-Desktop-HP', 'NewCars-Successful-Selection-Value-Click', desktoplinkLabel);
                window.location.href = splitVal[0].split("desktoplink:")[1].split("|")[0];
                return false;
            }
            var make = new Object();
            make.name = splitVal[0].split(':')[0];
            make.id = splitVal[0].split(':')[1];

            var make = new Object();
            make.name = splitVal[0].split(':')[0];
            make.id = splitVal[0].split(':')[1];

            var srcEle = $(event.srcElement);
            if (event.srcElement == undefined)
                srcEle = $(event.originalEvent.originalEvent.originalEvent.target);
            if (srcEle.hasClass('OnRoadPQ')) {
                var modelLabel = "|modelid=" + splitVal[1].split(':')[1];
                var pqSearchLabel = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name + '_' + splitVal[1].split(':')[0];
                trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, (make.name + '_' + splitVal[1].split(':')[0]), modelLabel);                
                trackHomePage('FirstPanel-Desktop-HP', 'NewCars-Successful-Selection-PQlink-Click', pqSearchLabel);
                return false;
            }

            var model = null;
            if (splitVal[1] != undefined && splitVal[1].indexOf(':') > 0) {
                model = new Object();
                model.name = splitVal[1].split(':')[0];
                model.id = splitVal[1].split(':')[1];
            }

            globalSearch(make, model);
        },
        open: function (result) {
            objNewCars.result = result;
        },
        afterfetch: function (result, resterm) {
            suggestReqTerm = resterm;
        },
        keyup: function () {
            if ($('li.ui-state-focus a:visible').text() != "" && objNewCars.result != undefined)//$("#globalCityPopUp").val())
            {
                var link = $('li.ui-state-focus a:visible');
                var li = link.parent();
                var ul = li.parent();
                suggestions = {
                    position: $(ul).find($(li)).index() + 1,
                    count: objNewCars.result.length
                };
            }
        },
        focusout: function () {
            globalSearchAdTracking.featuredModelIdPrev = 0;
            if ($('li.ui-state-focus a:visible').text() != "") {
                focusedMakeModel = new Object();
                focusedMakeModel = objNewCars.result[$('li.ui-state-focus').index()];
                var link = $('li.ui-state-focus a:visible');
                var li = link.parent();
                var ul = li.parent();
                suggestions = {
                    position: $(ul).find($(li)).index() + 1,
                    count: objNewCars.result.length
                };
            }
        }
    }).focus(function () {
        $.when(GetGlobalSearchCampaigns.bindCampaignData(false)).then(function () {
            $('.homepage-banner-search').show();
            $('.common-global-search').hide();
            $('body').addClass('trending-section');
            GetGlobalSearchCampaigns.logImpression('.common-global-search', 'trending', false);
        });
        trackHomePage('AreaLocation', 'HPBanner_search_click', 'HPBanner_search_click');
    });
    $('#btnFindCarNew').on('click', function () {
        if (!btnFindCarNewNav()) {
            trackHomePage('FirstPanel-Desktop-HP', 'NewCars-No-Result-Search-Find-Car', $('#newCarsList').val());
            window.location.href = /new/;
        }
    });

    $('#newCarsList').on('keypress', function (e) {
        var id = $('#newCarsList')
        var searchVal = id.val();
        var placeHolder = id.attr('placeholder');
        if (e.keyCode == 13)
            if (btnFindCarNewNav() || searchVal == placeHolder || searchVal == "")
                trackHomePage('FirstPanel-Desktop-HP', 'NewCars-No-Result-Search-Find-Car', searchVal);
            else {
                trackHomePage('FirstPanel-Desktop-HP', 'NewCars-No-Result-Search-Find-Car', searchVal);
                window.location.href = '/new/';
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
    var focusedPQCity;

    $('#inputModelPQWidget').cw_autocomplete({
        isPriceExists: 1,
        resultCount: 10,
        textType: ac_textTypeEnum.model,//+ ',' + ac_textTypeEnum.version,
        source: ac_Source.generic,
        onClear: function () {
            objNewPrice = new Object();
            $("#inputCityPQWidget").prop('disabled', true);
            $('#inputCityPQWidget').attr('value', 'Select City');
            NewPricemodelId = null;
        },
        click: function (event, ui, orgTxt) {
            objNewPrice.Name = ui.item.label;
            objNewPrice.Id = ui.item.id;
            NewPricemodelId = ui.item.id.split(':')[2];
            var node = $('#inputCityPQWidget');
            $('#inputModelPQWidget').attr("make-model", objNewPrice.Id);
            node.prop('disabled', false);
            PriceBreakUp.Quotation.prefillGlobalCity(node);
            hideErrorPQ($('.home-getFinalPrice-banner'));
            $.cookie('_PQPageId', "26", { path: '/' });
            $.cookie('_PQVersionId', "0", { path: '/' });
            trackHomePage('PQWidget', 'Select-Car-Autosuggest-Success', objNewPrice.Name);
        },
        open: function (result) {
            objNewPrice.result = result;
        },
        afterfetch: function (result, searchtext) {
            if (result != undefined && result.length > 0) {
                showHideDrpError($('#inputModelPQWidget').siblings(), false);
            }
            else {
                NewPricemodelId = null;
                showHideDrpError($('#inputModelPQWidget').siblings(), true);
            }
        },
    });

    var nextCount = 0, prevCount = 0, nextPageNo = 1;
    $('#upComingCars .jcarousel-control-prev').click(function () {
        prevCount += 1;
    });

    $('#upComingCars .jcarousel-control-next').click(function () {
        nextCount += 1;
        if (nextCount % 2 == 0 && nextCount > prevCount) {
            nextPageNo = nextPageNo + 1;
            var pageNo = nextPageNo;
            var url = '/CarWidgets/UpcomingCarsHomeScreen/?pageNo=' + pageNo + '&pageSize=' + pageSize;
            BindData(url, ulUpcoming, Category.Upcoming);
        }

        $(ulUpcoming).find('img.lazy').trigger("UNT");
    });

    var lauchNextCount = 0, lauchPrevCount = 0, lauchNextPageNo = 1;
    $('#newLaunches .jcarousel-control-prev').click(function () {
        lauchNextCount += 1;
    });

    $('#newLaunches .jcarousel-control-next').click(function () {
        lauchNextCount += 1;
        if (lauchNextCount % 2 == 0 && lauchNextCount > lauchPrevCount) {
            lauchNextPageNo = lauchNextPageNo + 1;
            var pageNo = lauchNextPageNo;
            var url = '/CarWidgets/JustLaunchedCars/?pageNo=' + pageNo + '&pageSize=' + pageSize + '&cityId=' + cityIdCookie;
            BindData(url, ulNewLaunches, Category.NewLaunches);
        }
        $(ulNewLaunches).find('img.lazy').trigger("UNT");
    });

    var sellNextCount = 0, sellPrevCount = 0, sellNextPageNo = 1;
    $('#topSelling .jcarousel-control-prev').click(function () {
        sellPrevCount += 1;
    });

    $('#topSelling .jcarousel-control-next').click(function () {
        sellNextCount += 1;
        if (sellNextCount % 2 == 0 && sellNextCount > sellPrevCount) {
            sellNextPageNo = sellNextPageNo + 1;
            var pageNo = sellNextPageNo;
            var url = '/CarWidgets/PopularCars/?pageNo=' + pageNo + '&pageSize=' + pageSize + '&cityId=' + cityIdCookie;
            BindData(url, topSelling, Category.TopSelling);
        }
        $(topSelling).find('img').trigger("UNT");
    });

    $('#inputModelPQWidget').on('focus', function () {
        if ($(this).val() != "" || $(this).val() != $(this).attr('placeholder'))
            trackHomePage('PQWidget', 'Select-Car-Box-Focus', '');
    })

    if (window.adblockDetecter === undefined)
        Common.utils.trackAction('CWNonInteractive', 'AdBlocker', 'Enabled_Desktop', 'AdBlocker');
    else
        Common.utils.trackAction('CWNonInteractive', 'AdBlocker', 'Disabled_Desktop', 'AdBlocker');

    initLocationPlugin();
    initTopSellingPQInstance();
    initJustLaunchesPQInstance();
});

function initLocationPlugin() {
    var div = document.getElementById('inputCityPQWidget');
    new LocationSearch((div), {
        showCityPopup: true,
        callback: function (locationObj) {
            var makeModelName = $('#inputModelPQWidget').attr('make-model');
            Common.autoComplete.redirectToModelPage(makeModelName);
        },
        isAreaOptional: true,
        ctaText: 'CHECK NOW',
    });
}

$(document).on('click', "#newLaunches .closeBtn,#topSelling .closeBtn, #upComingCars .closeBtn", function () {
    $(this).parents("li").flip(false);
});


function globalSearch(make, model) {
    var userInput = '';
    if (model != null && model != undefined) {
        userInput = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name + ' ' + model.name;
        trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, (make.name + ' ' + model.name), ('|modelid=' + model.id));
        trackHomePage('FirstPanel-Desktop-HP', 'NewCars-Successful-Selection-Value-Click', userInput);
        window.location.href = '/' + make.name + '-cars/' + model.name + '/';
        $('#btnFindCarNew').unbind("click");
        return true;
    }
    if (make != null && make != undefined) {
        userInput = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name;
        trackBhriguSearchTracking('Home', '', suggestions.count, suggestions.position, suggestReqTerm, make.name);
        trackHomePage('FirstPanel-Desktop-HP', 'NewCars-Successful-Selection-Value-Click', userInput);
        window.location.href = '/' + make.name + '-cars/'
        $('#btnFindCarNew').unbind("click");
        return true;
    }
    return false;
}
//tracking
function trackHomePage(category, action, label) {
    dataLayer.push({ event: 'Desktop-Homepage', cat: category, act: action, lab: label });
}
//TNU
function trackTNU(category, action, label) {
    dataLayer.push({ event: 'TopSelling-Upcoming-New', cat: category, act: action, lab: label });
}

$(".cw-tabs li[data-tabs='usedCars']").click(function () {
    if (objUsedCar.Name != undefined && $.trim(objUsedCar.Name) != "") {
        $("#usedCarsList").val(objUsedCar.Name);
        openUsedBudget();
    }
});

$(document).on("mastercitychange", function (event, cityName, cityId) {
    location.reload();
});
/*Code to Fetch Trending Cars And User History */
var GetGlobalSearchCampaigns = {
    latestUserHistoryModels: "",
    platformId: 0,
    registerEvents: function (platform) {
        $(document).on("click", ".trending-section", function (e) {
            if (!$(".ui-autocomplete-input").is(":focus")) {
                if (e.target.className !== "common-global-search")
                    $('.common-global-search').hide();
                if (e.target.className !== "homepage-banner")
                    $('.homepage-banner-search').hide();
                $('body').removeClass('trending-section');
            }
        });
        GetGlobalSearchCampaigns.platformId = platform;
        GetGlobalSearchCampaigns.fetchCampaignData();
    },

    platform: {
        Mobile: 43,
        Desktop: 1
    },

    globalCarClick: function (data, index, type) {
        var isMobile = GetGlobalSearchCampaigns.platformId == GetGlobalSearchCampaigns.platform.Mobile;
        GetGlobalSearchCampaigns.logClicks(data, type, 'global-search-popup-cars', index, isMobile);
        window.location = '/' + Common.utils.formatSpecial(data.makeName) + '-cars/' + data.maskingName + '/';
    },

    fetchCampaignData: function () {
        GetGlobalSearchCampaigns.fetchUserHistoryModels();
        GetGlobalSearchCampaigns.saveTrendingCarDatailsLS();
    },

    fetchTrendingCarsfromApi: function () {
        var config = {};
        config.contentType = "application/json";
        config.async = true;
        config.data = { count: 10, platformid: GetGlobalSearchCampaigns.platformId };
        return Common.utils.ajaxCall('/api/trending/models/', config);
    },

    fetchUserHistoryModelsData: function (userHistoryModels) {
        var config = {};
        config.contentType = "application/json";
        config.async = true;
        config.data = { modelIdList: userHistoryModels, platformId: GetGlobalSearchCampaigns.platformId };
        return Common.utils.ajaxCall('/api/userhistory/models/', config);
    },

    bindCampaignData: function (isMobile) {
        GetGlobalSearchCampaigns.fetchUserHistoryModels();
        GetGlobalSearchCampaigns.bindTrendingCarData();
        GetGlobalSearchCampaigns.bindUserHistoryModelsData(isMobile);
    },

    bindUserHistoryModelsData: function (isMobile) {
        var userHistoryModels = clientCache.get('userHistoryModels_' + GetGlobalSearchCampaigns.platformId, GetGlobalSearchCampaigns.latestUserHistoryModels);

        clientCache.setOptions({ 'FallBack': false });
        var getUserHistorykey = { 'key': 'userHistory_' + GetGlobalSearchCampaigns.platformId, 'expiry': 60 };
        var localStorageData = clientCache.get(getUserHistorykey, false);

        if (userHistoryModels != GetGlobalSearchCampaigns.latestUserHistoryModels)
            localStorageData = null;

        if (GetGlobalSearchCampaigns.latestUserHistoryModels != "" && localStorageData == null) {

            $.when(GetGlobalSearchCampaigns.fetchUserHistoryModelsData(GetGlobalSearchCampaigns.latestUserHistoryModels)).done(function (data) {
                if (data != null) {
                    var setUserHistorykey = { 'key': 'userHistory_' + GetGlobalSearchCampaigns.platformId, 'expiryTime': 60, 'value': data };
                    clientCache.setOptions({ 'FallBack': false });
                    clientCache.set(setUserHistorykey, data, false);
                    clientCache.set('userHistoryModels_' + GetGlobalSearchCampaigns.platformId, GetGlobalSearchCampaigns.latestUserHistoryModels);
                    GetGlobalSearchCampaigns.bindHistoryHtml(data);


                    GetGlobalSearchCampaigns.logImpression(isMobile ? '#global-search-popup-cars' : '.common-global-search', 'history', isMobile);
                }
                else {
                    GetGlobalSearchCampaigns.bindHistoryHtml([]);
                }
            });
        }
        else {
            if (localStorageData != null) {
                GetGlobalSearchCampaigns.bindHistoryHtml(localStorageData);
                GetGlobalSearchCampaigns.logImpression(isMobile ? '#global-search-popup-cars' : '.common-global-search', 'history', isMobile);
            }
            else {
                GetGlobalSearchCampaigns.bindHistoryHtml([]);
            }
        }

    },

    saveTrendingCarDatailsLS: function () {
        clientCache.setOptions({ 'FallBack': true });
        var trendingCarData = clientCache.get('trendindCars_' + GetGlobalSearchCampaigns.platformId, true);

        if (trendingCarData == null) {
            clientCache.setOptions({ 'FallBack': true });
            $.when(GetGlobalSearchCampaigns.fetchTrendingCarsfromApi()).done(function (jsonData) {
                if (jsonData != null)
                    clientCache.set('trendindCars_' + GetGlobalSearchCampaigns.platformId, jsonData, true);
            });
        }
    },

    fetchUserHistoryModels: function () {
        var arrUserHistory = [];
        var userModelHistoryCookie = $.cookie('_userModelHistory');
        if (userModelHistoryCookie != null && userModelHistoryCookie != undefined)
            arrUserHistory = userModelHistoryCookie.split('~');

        GetGlobalSearchCampaigns.latestUserHistoryModels = arrUserHistory.reverse().join();
    },

    bindTrendingCarData: function () {
        clientCache.setOptions({ 'FallBack': true });
        var ssTrendingData = clientCache.get('trendindCars_' + GetGlobalSearchCampaigns.platformId, true);

        if (ssTrendingData == null) {
            $.when(GetGlobalSearchCampaigns.fetchTrendingCarsfromApi()).done(function (jsonData) {
                if (jsonData != null) {
                    clientCache.setOptions({ 'FallBack': true });
                    clientCache.set('trendindCars_' + GetGlobalSearchCampaigns.platformId, jsonData, true);
                }
                GetGlobalSearchCampaigns.removeHistoryModelsFromTrending(jsonData);
            });
        }
        else {
            GetGlobalSearchCampaigns.removeHistoryModelsFromTrending(ssTrendingData);
        }
    },

    removeHistoryModelsFromTrending: function (jsonData) {
        var trendingCarData;
        if (typeof jsonData == 'object') {
            trendingCarData = jsonData.trendingCars;
        }
        else {
            trendingCarData = JSON.parse(jsonData);
        }
        var arrUserHistory = GetGlobalSearchCampaigns.latestUserHistoryModels != "" ? GetGlobalSearchCampaigns.latestUserHistoryModels.split(',') : [];

        if ($.type(trendingCarData) !== "array") {
            if (typeof jsonData == 'object') {
                trendingCarData = JSON.parse(trendingCarData);
            }
            else {
                trendingCarData = trendingCarData.trendingCars;
            }
        }
        if (trendingCarData != null) {
            for (var i = 0; i < arrUserHistory.length && i < 3; i++) {
                trendingCarData = trendingCarData.filter(function (obj) { return obj.modelId.toString() !== arrUserHistory[i] });
            }
            if (jsonData.sponsoredModel != null)
                trendingCarData.splice(jsonData.sponsoredModel.adPosition - 1, 0, jsonData.sponsoredModel);

            GetGlobalSearchCampaigns.bindTrendingHtml(trendingCarData.slice(0, 5));
        }
    },

    logImpression: function (current, section, isMobile) {
        var sponsoredSearchSelector = $(current + ' .' + section + '-search ul li');
        var carCount = sponsoredSearchSelector.length;
        var liAdClass = 'ad-search-list';
        var counter = 0;
        var ad = 0;
        var trackEvent = "CWNonInteractive";
        var cat;
        var act;
        var lab;

        sponsoredSearchSelector.each(function () {
            if ($(this).hasClass(liAdClass)) {
                cat = isMobile ? "SearchResult_Ad_m" : "SearchResult_Ad"; act = section + "_impression"; lab = $(this).find('span').eq(1).text() + '_' + carCount + '_' + (counter + 1);
                cwTracking.trackAction(trackEvent, cat, act, lab);
                ad = 1;
            }
            counter++;
        });

        cat = isMobile ? "Search_Global" : "Search_Global_Desktop"; act = section + "_shown"; lab = act + "_" + (carCount - ad);
        cwTracking.trackAction(trackEvent, cat, act, lab);
    },

    logClicks: function (data, category, id, pos, isMobile) {
        var length;
        var index;
        var catagoryClick;
        if (isMobile) {
            var selector;
            catagoryClick = "SearchResult_Ad_m";
            if (category == 'history') {
                selector = id == 'global-search-popup-cars' ? $('#global-search-popup-cars').find('.history-search') : $('#global-search-popup-pq').find('.recent-search');
            }
            else {
                selector = id == 'global-search-popup-cars' ? $('#global-search-popup-cars').find('.trending-search') : $('#global-search-popup-pq').find('.trending-search');
            }
            length = selector.find('li').length;
            index = selector.find('li.ad-search-list').index();
        }
        else {
            catagoryClick = "SearchResult_Ad";
            if (category === 'history') {
                length = $('.history-search ul:first li').length;
                index = $('.history-search li.ad-search-list').index();
            }
            else {
                length = $('.trending-search ul:first li').length;
                index = $('.trending-search li.ad-search-list').index();
            }
        }
        var label;
        if (data.isSponsored || data.adPosition) {
            label = data.makeName + "_" + data.modelName + "_" + length + "_" + (index + 1);
            dataLayer.push({
                event: "CWInteractive",
                cat: catagoryClick,
                act: category + "_click",
                lab: label
            });
            trackBhriguSearchTracking('', category.substr(0, 1).toUpperCase() + category.substr(1), length, (index + 1), '', (data.makeName + " " + data.modelName), ("|modelid=" + data.modelId));
        }
        else {
            label = data.makeName + "_" + data.modelName + "_" + length + "_" + (pos + 1);
            dataLayer.push({
                event: "CWInteractive",
                cat: isMobile ? "Search_Global" : "Search_Global_Desktop",
                act: category + "_click",
                lab: label
            });
            trackBhriguSearchTracking('', category.substr(0, 1).toUpperCase() + category.substr(1), length, (pos + 1), '', (data.makeName + " " + data.modelName), ("|modelid=" + data.modelId));
        }
        return true;
    },
    debounce: function (fn, wait) {
        var timeout;
        return function () {
            clearTimeout(timeout);
            timeout = setTimeout(function () {
                fn.apply(this, arguments)
            }, (wait || 1));
        }
    },
    bindTrendingHtml: function (data) {
        if (data.length == 0) {
            $('.trending-search').empty();
        }
        else {
            $('.trending-list').empty();
            for (index = 0; index < data.length; index++) {
                item = JSON.stringify(data[index]);
                var li = "<li class='" + (data[index].adPosition ? 'ad-search-list text-bold text-black' : '') + "'><span class='" + (data[index].adPosition ? 'ad-text-icon text-white padding-right10' : 'trending-icon cwmsprite padding-right10') + "'>" + (data[index].adPosition ? 'Ad' : '') + "</span><span class='collective-search-list cur-pointer' onclick='GetGlobalSearchCampaigns.globalCarClick(" + item + "," + index + "," + '"trending"' + ")'><span class='model-search-name'>" + data[index].makeName + ' ' + data[index].modelName + "</span></span></li>";
                $('.trending-list').append(li);
            }
        }
    },

    bindHistoryHtml: function (data) {
        if (data.length == 0) {
            $('.history-search').empty();
        }
        else {
            $('.recent-list').empty();
            for (index = 0; index < data.length; index++) {
                item = JSON.stringify(data[index]);
                var li = "<li class='" + (data[index].isSponsored ? 'ad-search-list text-bold text-black' : '') + "'><span class='" + (data[index].isSponsored ? 'ad-text-icon text-white padding-right10' : 'recent-clock cwmsprite padding-right10') + "'>" + (data[index].isSponsored ? 'Ad' : '') + "</span><span class='collective-search-list cur-pointer' onclick='GetGlobalSearchCampaigns.globalCarClick(" + item + "," + index + "," + '"history"' + ")'><span class='model-search-name'>" + data[index].makeName + ' ' + data[index].modelName + "</span></span></li>";
                $('.recent-list').append(li);
            }
        }
    }
}

$(".global-search").click(function () {
    Common.utils.trackTopMenu('Global-Search-Icon-Click', window.location.href);
    $("#global-search-popup-cars").removeClass('hide');
    $('#globalSearchPopup').focus();
    $.when(GetGlobalSearchCampaigns.bindCampaignData(true)).then(function () {
        $('.global-search-section').show();
        GetGlobalSearchCampaigns.logImpression('#global-search-popup-cars', 'trending', true);
    });
    lockPopup();
});

$(".blackOut-window,.blackOut-window-pq").click(function (e) {
    var globalSearchPopup = $("#global-search-popup-cars");
    $("#global-search-popup-pq").addClass('hide');
    if (globalSearchPopup.is(":visible")) {
        Common.utils.trackTopMenu('Global-Search-Outside-Click', $('#globalSearchPopup').val());
        $("#globalSearchPopup").val("");
        globalSearchPopup.addClass('hide');
        unlockPopup();
    }

    var nav = $("#nav");
    if (nav.is(":visible")) {
        nav.animate({ 'left': '-300px' });
        unlockPopup();
    }
});

$("#gs-close").click(function () {
    Common.utils.trackTopMenu('Global-Search-Back-Icon-Click', $('#globalSearchPopup').val());
    $("#globalSearchPopup").val("");
    $(".global-search-popup").addClass('hide');
    unlockPopup();
});

$("#gl-close").click(function () {
    $(".global-location-popup").addClass('hide');
    unlockPopup();
});

$("#gs-text-clear, #gs-text-clear-pq").click(function () {
    Common.utils.trackTopMenu('Global-Search-Close-Icon-Click', $("#globalSearchPopup").val(""));
    globalSearchInputField.val("").focus();
    $("#globalSearchPQ").val("").change().focus();
    $('.global-search-section').show();
});

function lockPopup() {
    $('body').addClass('lock-browser-scroll');
    $(".blackOut-window").show();
}
function unlockPopup() {
    $('body').removeClass('lock-browser-scroll');
    $(".blackOut-window").hide();
}




var ac_textTypeEnum = new Object({ make: "1", model: "2,4", version: "3,5,6", state: "", city: "7", link: "8", discontinuedModel: "9" });
var ac_Source = new Object({ generic: "1", usedModels: "2", usedCarCities: "3", valuationModels: "4", allCarCities: "6", areaLocation: "7", globalCityLocation: "8", accessories: "9" });
var ac_SourceName = { "8": "city", "7": "areas" };

(function ($) {
    $.fn.cw_easyAutocomplete = function (options) {
        return this.each(function () {
            if (options == null || options == undefined) {
                console.log("cwsearch: please define options");
                return;
            }
            else if (options.source == null || options.source == undefined || options.source == '') {
                console.log("cwsearch: please define source");
                return;
            }

            var spinner = $(options.inputField).closest('.form-control-box').find('.fa-spinner');

            var cache = {},
                cacheProp,
                requestTerm;

            $(this).easyAutocomplete({
                adjustWidth: false,
                url: function (value) {
                    if (options.beforefetch && typeof (options.beforefetch) == "function") {
                        options.beforefetch();
                    }

                    requestTerm = value.replace(/^\s\s*/, '').replace(/\s\s*$/, '').replace(/-/g, ' ').replace(/[^A-Za-z0-9 ]/g, '').toLowerCase();

                    var year = options.year;
                    if (year != null && year != undefined && year != '') {
                        year = year.val();
                    }
                    else {
                        year = '';
                    }

                    cacheProp = requestTerm + '_' + year;

                    if (options.source == ac_Source.areaLocation) {
                        cacheProp += "_" + options.cityId;
                    }

                    if (requestTerm.length > 0) {
                        if (!(cacheProp in cache) || cache[cacheProp] == undefined) {
                            var path;

                            if (options.source == ac_Source.globalCityLocation || options.source == ac_Source.areaLocation) {
                                path = "/api/v2/autocomplete/" + ac_SourceName[options.source] + "/?term=" + encodeURIComponent(requestTerm) + "&record=" + options.resultCount;
                            }
                            else {
                                path = "/webapi/autocomplete/GetResults/?source=" + options.source + "&value=" + encodeURIComponent(requestTerm);
                            }

                            var par = '';
                            par += __getValue('n', options.isNew);
                            par += __getValue('u', options.isUsed);
                            par += __getValue('p', options.isPriceExists);
                            par += __getValue('t', options.additionalTypes);
                            par += __getValue('y', year);
                            par += __getValue('cityId', options.cityId);
                            par += __getValue('size', options.resultCount);
                            par += __getValue('SourceId', "43");
                            par += __getValue('showFeaturedCar', typeof (options.showFeaturedCar) === "undefined" ? true : options.showFeaturedCar);
                            par += __getValue('isNcf', options.isNcf);
                            if (par != null && par != undefined && par != '') {
                                par = par.slice(0, -1);
                                path += '&' + par;
                            }

                            return path;
                        }
                        else {
                            return {
                                data: cache[cacheProp]
                            };
                        }
                    }
                },

                getValue: (options.source == ac_Source.globalCityLocation || options.source == ac_Source.areaLocation) ? "result" : "label",

                ac_Source: ac_Source,

                sourceType: options.source,

                ajaxSettings: {
                    async: true,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        spinner.show();
                    },
                    success: function (jsonData) {
                        spinner.hide();

                        cache[cacheProp] = [];


                        if (options.source == ac_Source.globalCityLocation || options.source == ac_Source.areaLocation) {
                            if (jsonData && jsonData.length > 0) {
                                cache[cacheProp] = $.map(jsonData, function (item) {
                                    return {
                                        result: item.result,
                                        payload: item.payload
                                    }
                                });
                            }
                            else {
                                cache[cacheProp] = undefined;
                            }
                        }
                        else {

                            if (jsonData != null && jsonData.length > 0) {
                                var isNewsFilter = options.newsFilter;

                                cache[cacheProp] = $.map(jsonData, function (item) {
                                    if (isNewsFilter) {
                                        item.label = item.label.replace("All", "").replace("Cars", "");
                                    }
                                    if (isNewsFilter && item.label.toLowerCase().indexOf(" vs") > 0) {
                                        return;
                                    }
                                    return {
                                        label: item.label,
                                        value: item.value,
                                        imgSrc: item.imgSrc
                                    }
                                });
                            }
                            else {
                                globalSearchAdTracking.featuredModelIdPrev = 0;
                                cache[cacheProp] = undefined;
                            }
                        }
                    }
                },

                template: {
                    type: "custom",
                    method: function (value, item) {
                        var isLocationSearch = (options.source == ac_Source.globalCityLocation || options.source == ac_Source.areaLocation);
                        var isNewsFilter = (typeof (options.newsFilter) != "undefined" && options.newsFilter),
                            listElement,
                            elementLabel;
                        var isNcfLink = null;
                        if (item.value) {
                            isNcfLink = item.value.split(':')[0];
                        }
                        var isLink = !isLocationSearch && item.value.indexOf("desktoplink") == 0;
                        if (isLink && item.value.split("mobilelink:")[1] == "") return;

                        if (isNewsFilter == true) {
                            item.label = item.label.replace("All", "").replace("Cars", "");
                        }

                        if (isNewsFilter && item.label.toLowerCase().indexOf(" vs") > 0) {
                            return;
                        }

                        if (isLocationSearch) {
                            listElement = '<a class="list-item" cityname="' + item.result.replace(/\s/g, '').toLowerCase() + '">' + value + '</a>';

                            return listElement;
                        }

                        if (isNcfLink === 'ncfLink') {
                            elementLabel = '<a style="color:#0288d1;" class="position-rel" cityname="' + item.label.replace(/\s/g, '').toLowerCase() + '">' + value + '</a>';
                        }
                        else {
                            elementLabel = '<a class="position-rel" cityname="' + item.label.replace(/\s/g, '').toLowerCase() + '">' + value + '</a>';
                        }


                        var val = item.value.split('|');
                        if (val.indexOf('sponsor') > 0 && !isNewsFilter) {
                            var elementImg = '';
                            if (item.imgSrc !== undefined && item.imgSrc != "") {
                                elementImg = '<img class="padding-right20" src="' + item.imgSrc + '">';
                            }

                            listElement = '<div class="list-item">' + elementImg + elementLabel + '<span class="font10 position-abt text-light-grey pos-top0 pos-right10">Ad</span>';
                        }
                        else {
                            listElement = '<div class="list-item">' + elementLabel;
                            globalSearchAdTracking.targetModelName = item.label;
                        }
                        if (options.isOnRoadPQ == 1 && !isLink) {
                            var isUpcoming = false;
                            if (val.indexOf('upcoming') > 0) {
                                isUpcoming = true;
                            }

                            var isComparision = false;
                            if (item.label.toLowerCase().indexOf(' vs ') > 0) {
                                isComparision = true;
                            }

                            var make = item.value.split('|')[0];
                            var model = item.value.split('|')[1];
                            var pqModelId = 0;
                            if (model != undefined && model.indexOf(':') > 0) {
                                pqModelId = model.split(':')[1];
                            }

                            if (isUpcoming) {
                                listElement += '<span class="rightfloat font12">Coming soon</span>';
                            }
                            else if (!isComparision && !isNewsFilter && (typeof (isEligibleForORP) == "undefined" || !isEligibleForORP)) {
                                if (pqModelId > 0) {
                                    listElement += '<a class="OnRoadPQ rightfloat text-link font12" modelid="' + pqModelId + '" onClick= "cwTracking.trackCustomData(' + ($(options.inputField).attr("id") === 'globalSearch' ? '\'GlobalSearch\'' : '\'HomePage\'') + ',\'OnRoadLinkClick\',\'make:' + make.split(':')[0] + '|model:' + model.split(':')[0] + '|city:' + $.cookie("_CustCityMaster") + '\');getOnRoadPQ(this,' + pqModelId + ',' + options.pQPageId + ');">Check On-Road Price</a>';
                                }
                            }
                            listElement += '<div class="clear"></div></div>';

                            return listElement;
                        }
                        else {
                            return listElement;
                        }
                    }
                },

                list: {
                    maxNumberOfElements: options.isNcf ? (options.resultCount + 1) : options.resultCount,
                    onChooseEvent: function (event) {
                        options.click(event);
                    },
                    onLoadEvent: function () {
                        var suggestionResult = $(options.inputField).getItems();

                        if (options.afterFetch != null && typeof (options.afterFetch) == "function") {
                            options.afterFetch(suggestionResult, requestTerm);
                        }
                    }
                }
            });

            $(this).keyup(function () {
                if (options.keyup != undefined) {
                    options.keyup();
                }

                if ($(options.inputField).val().replace(/\s/g, '').length == 0 && options.onClear != undefined) {
                    options.onClear();

                    $(options.inputField).closest('.easy-autocomplete').find('ul').hide();

                    GetGlobalSearchCampaigns.logImpression('#global-search-popup-cars', 'history', true);
                    GetGlobalSearchCampaigns.logImpression('#global-search-popup-cars', 'trending', true);
                    $('.global-search-section').show();
                }
            });

            $(this).focusout(function () {
                if (options.focusout != undefined) {
                    options.focusout();
                }
            });

            function __getValue(key, value) {
                if (value != null && value != undefined && value != '') {
                    return key + '=' + value + '&';
                }
                else {
                    return '';
                }
            }

        });
    };
}(jQuery));




function globalAutoSearch(make, model, version, google, suggestions) {
    var userInput = '';
    if (version != null && version != undefined) {
        userInput = make.name + ' ' + model.name + ' ' + version.name;
        trackBhriguSearchTracking('', '', '', '', '', userInput, ('|modelid=' + model.id));
        Common.utils.trackTopMenu('Global-AutoSuggest-Value-Click', userInput);
        window.location.href = '/m/' + make.name + '-cars/' + model.name + '/' + version.name + '/';
        return true;
    }
    if (model != null && model != undefined) {
        userInput = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name + ' ' + model.name;
        trackBhriguSearchTracking('', '', suggestions.count, suggestions.position, suggestReqTerm, (make.name + ' ' + model.name), ('|modelid=' + model.id));
        Common.utils.trackTopMenu('Global-AutoSuggest-Value-Click', userInput);
        window.location.href = '/' + make.name + '-cars/' + model.name + '/';
        return true;
    }
    if (make != null && make != undefined) {
        userInput = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + make.name;
        trackBhriguSearchTracking('', '', suggestions.count, suggestions.position, suggestReqTerm, make.name);
        Common.utils.trackTopMenu('Global-AutoSuggest-Value-Click', userInput);
        window.location.href = '/m/' + make.name + '-cars/';
        return true;
    }
    return false;
}

var objGlobalSearch = new Object();
var noResult = true;
var globalSearchInputField = $("#globalSearchPopup"),
    globalSearchClearButton = $("#gs-text-clear");

globalSearchInputField.track = GetGlobalSearchCampaigns.debounce(function () {
    Common.utils.trackTopMenu('Global-Search-With-value-Click', suggestReqTerm);
}, 1000);

var suggestions = {
    position: 0,
    count: 0
}
$(globalSearchInputField).cw_easyAutocomplete({
    inputField: globalSearchInputField,
    resultCount: 5,
    isNcf: 1,
    isNew: 1,
    isOnRoadPQ: 1,
    doOrpChecks: true,
    pQPageId: 80,
    additionalTypes: ac_textTypeEnum.link,
    source: ac_Source.generic,

    click: function (event) {
        suggestions = {
            position: globalSearchInputField.getSelectedItemIndex() + 1,
            count: objGlobalSearch.result.length
        }

        var selectionValue = globalSearchInputField.getSelectedItemData().value,
            splitVal = selectionValue.split('|'),
            label = globalSearchInputField.getSelectedItemData().label;

        if (selectionValue.indexOf("mobilelink:") > 0) {
            var mobilelinkLabel = suggestions.count + '_' + suggestions.position + '_' + suggestReqTerm + '_' + label;
            trackBhriguSearchTracking('', '', suggestions.count, suggestions.position, suggestReqTerm, label);
            Common.utils.trackTopMenu('Global-AutoSuggest-Value-Click', mobilelinkLabel);
            window.location.href = selectionValue.split("mobilelink:")[1];
            return false;
        }
        if (label.indexOf(' vs ') > 0) {
            var model1 = "|modelid1=" + splitVal[0].split(':')[1];
            var model2 = "|modelid2=" + splitVal[1].split(':')[1];
            trackBhriguSearchTracking('', '', suggestions.count, suggestions.position, suggestReqTerm, label, (model1 + model2));
            Common.redirectToComparePage(splitVal);
            var compareCars = {};
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
            trackBhriguSearchTracking('', '', suggestions.count, suggestions.position, suggestReqTerm, (make.name + ' ' + splitVal[1].split(':')[0]), modelLabel);
            Common .utils.trackTopMenu('Global-AutoSuggest-PQlink-Click', pqSearchLabel);
            return false;
        }

        if (selectionValue.indexOf('sponsor') > 0) {
            var sponsorLabel = globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName + '_' + suggestions.count + "_" + suggestions.position;
            trackBhriguSearchTracking('', 'Sponsored', suggestions.count, suggestions.position, suggestReqTerm, (globalSearchAdTracking.featuredModelName + '_' + globalSearchAdTracking.targetModelName), modelLabel);
            trackGlobalSearchAd('New_m_Click', sponsorLabel, 'SearchResult_Ad_m');
            cwTracking.trackCustomData('SearchResult_Ad_m', 'New_m_Click', sponsorLabel, false);
        }

        if (splitVal[0].split(':')[0] === 'ncfLink') {
            trackBhriguSearchTracking('', '', suggestions.count, suggestions.position, suggestReqTerm, splitVal[0].split(':')[1]);
            cwTracking.trackAction('CWInteractive', 'NCFLinkageCategory', 'NCFGlobalsearchlinkclick', suggestReqTerm + '_' + suggestions.position + '/' + suggestions.count);
            window.location.href = splitVal[0].split(':')[1];
            return false;
        }

        objGlobalSearch.Name = Common.utils.formatSpecial(label);
        objGlobalSearch.Id = selectionValue;

        var model = null;
        if (splitVal[1] != undefined && splitVal[1].indexOf(':') > 0) {
            model = {};
            model.name = splitVal[1].split(':')[0];
            model.id = splitVal[1].split(':')[1];
        }

        globalAutoSearch(make, model, null, globalSearchInputField.val(), suggestions);
    },

    afterFetch: function (result, searchText) {
        objGlobalSearch.result = result;
        noResult = (objGlobalSearch.result != undefined && objGlobalSearch.result.length > 0) ? false : true;
        suggestReqTerm = searchText;

        if ((result.filter(function (suggestion) { return suggestion.value.toString().indexOf('sponsor') > 0 })).length > 0) {
            globalSearchAdTracking.trackData(result, 'SearchResult_Ad_m');
        }
        else {
            globalSearchAdTracking.featuredModelIdPrev = 0;
        }

        if (result.find(function (item) { return item.value.split(':')[0] === 'ncfLink'; })) {
            cwTracking.trackAction('CWNonInteractive', 'NCFLinkageCategory', 'NCFGlobalsearchlinkdisplayed', searchText);
        }

        if (noResult == true) globalSearchInputField.track();
    },

    onClear: function () {
        objGlobalSearch = {};
        globalSearchAdTracking.featuredModelIdPrev = 0;

        $('.global-search-section').show();
    },

    keyup: function () {
        if (globalSearchInputField.val().length != 0) {
            globalSearchClearButton.show();
            $('.global-search-section').hide();
        }
        else {
            globalSearchClearButton.hide();
            globalSearchInputField.closest('.form-control-box').find('.fa-spinner').hide();
        }
    }
});

var globalSearchAdTracking = {
    targetModelName: "",
    featuredModelIdPrev: 0,
    featuredModelName: "",
    adPosition: 0,

    trackData: function (result, category) {
        var sponsoredObj = result.filter(function (suggestion) {
            return suggestion.value.indexOf('sponsor') > 0
        })[0].value.split('|');
        var sponsoredModelId = parseInt(sponsoredObj[1].split(':')[1]);
        globalSearchAdTracking.targetModelName = result[sponsoredObj[3] - 2].label;
        globalSearchAdTracking.featuredModelName = result[sponsoredObj[3] - 1].label;
        globalSearchAdTracking.adPosition = sponsoredObj[3] !== undefined ? sponsoredObj[3] : 0;

        if (globalSearchAdTracking.featuredModelIdPrev != sponsoredModelId) {
            globalSearchAdTracking.featuredModelIdPrev = sponsoredModelId;
            cwTracking.trackAction('CWNonInteractive', category, 'New_m_Shown', result[sponsoredObj[3] - 1].label + "_" + globalSearchAdTracking.targetModelName + "_" + result.length + "_" + globalSearchAdTracking.adPosition);
        }
    }
}
function trackBhriguSearchTracking(category, action, count, position, term, result, modelLabel) {
    cwTracking.trackCustomData(category + 'GlobalSearch', action + 'Click', getBhriguSearchLabel(count, position, term, result, modelLabel), true);
}
function getBhriguSearchLabel(count, position, term, result, modelLabel) {
    return (count != '' ? 'count=' + count : '') + (position != '' ? '|pos=' + position : '') + (term != '' ? '|term=' + term : '') + (result != '' ? '|result=' + result : '') + (modelLabel ? modelLabel : '');
}
$(document).ready(function () {
    GetGlobalSearchCampaigns.registerEvents(GetGlobalSearchCampaigns.platform.Mobile);
});
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
                Common.utils.trackAction(trackEvent, cat, act, lab);
                ad = 1;
            }
            counter++;
        });

        cat = isMobile ? "Search_Global" : "Search_Global_Desktop"; act = section + "_shown"; lab = act + "_" + (carCount - ad);
        Common.utils.trackAction(trackEvent, cat, act, lab);
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
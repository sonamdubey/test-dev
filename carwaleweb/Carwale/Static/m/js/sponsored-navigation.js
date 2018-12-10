/*Code to Fetch Sponsored Navigation Data */
var SponsoredNavigation = {
    platform: {
        Mobile: 43,
        Desktop: 1
    },
    section: {
        newCars: 1,
        specials: 2
    },
    showSponsoredNavigation: function (sectionId, platformId) {

        var localStorageData = clientCache.get('sponsoredNavigation_' + sectionId + '_' + platformId, true);

        if (localStorageData != null)
            SponsoredNavigation.bindSponsoredNavigation(localStorageData.val, sectionId, platformId);
        else {
            var localStorageKey = localStorage.getItem('cwc_sponsoredNavigation_' + sectionId + '_' + platformId);

            if (localStorageKey == null || localStorageKey == undefined) {
                $.when(SponsoredNavigation.fetchSponsoredNavigationData(sectionId, platformId)).done(function (data) {
                    if (data != null) {
                        var setSponsoredNavKey = { 'key': 'sponsoredNavigation_' + sectionId + '_' + platformId, 'expiryTime': 60, 'value': data };
                        clientCache.setOptions({ 'FallBack': false });
                        clientCache.set(setSponsoredNavKey, data, true);
                        if (data.length > 0)
                            SponsoredNavigation.bindSponsoredNavigation(data, sectionId, platformId);
                    }
                });
            }
        }
    },
    fetchSponsoredNavigationData: function (sectionId, platformId) {
        var config = {};
        config.contentType = "application/json";
        config.async = true;
        config.data = { platformId: platformId };
        return Common.utils.ajaxCall('/api/sponsored/navigation/' + sectionId + '/', config);
    },
    bindSponsoredNavigation: function (data, sectionId, platformId) {
        if (SponsoredNavigation.section.newCars == sectionId) {
            SponsoredNavigation.bindNewCarsModel(data, platformId);
        }
        else {
            SponsoredNavigation.bindSpecialModel(data, platformId);
        }
        $("#cw-top-navbar").find("img.lazy").lazyload(); // bind lazy load to knockout bindings
    },
    bindNewCarsModel: function (data, platformId) {
        $('#navNewCars ul.nestedUL').find('.sponsoredLi').remove();
        for (index = 0; index < data.length; index++) {
            var liToAppend = "<li class='sponsoredLi'><a href='" + data[index].linkUrl + "' data-label='" + data[index].title + "' data-action='" + (data[index].sectionId == 1 ? 'NewCars' : 'Specials') + "' data-role='click-tracking' data-event='CWInteractive' data-cat='" + (platformId == SponsoredNavigation.platform.Desktop ? 'SponsoredNavigation_d' : 'SponsoredNavigation_m') + "'><span>" + data[index].title + "</span><span class='font9 " + (data[index].isSponsored ? "ad-text-icon" : "hide") + "'>" + (data[index].isSponsored ? 'Ad' : '') + "</span></a></li>";
            if ($('#navNewCars ul.nestedUL').find('#menuAdvantage'))
            {
                $('#navNewCars ul.nestedUL li:nth-child(2)').append(liToAppend);
            }
            else {
                $('#navNewCars ul.nestedUL').eq(1).after(liToAppend);
            }
        }
    },
    bindSpecialModel: function (data, platformId) {
        $('#navSpecials ul.nestedUL').find('.sponsoredLi').remove();
        for (index = 0; index < data.length; index++) {
            var liToAppend = "<li class='sponsoredLi'><a href='" + data[index].linkUrl + "' data-label='" + data[index].title + "' data-action='" + (data[index].sectionId == 1 ? 'NewCars' : 'Specials') + "' data-role='click-tracking' data-event='CWInteractive' data-cat='" + (platformId == SponsoredNavigation.platform.Desktop ? 'SponsoredNavigation_d' : 'SponsoredNavigation_m') + "'><span>" + data[index].title + "</span><span class='font9 " + (data[index].isSponsored ? "ad-text-icon" : "hide") + "'>" + (data[index].isSponsored ? 'Ad' : '') + "</span></a></li>";
            $('#navSpecials ul.nestedUL').append(liToAppend);
        }
    }
}
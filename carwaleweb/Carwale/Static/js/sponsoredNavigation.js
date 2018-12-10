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
    bindSponsoredNavigation: function (data, sectionId, platformId)
    {
        if(SponsoredNavigation.section.newCars == sectionId )
        {
            SponsoredNavigation.koNewCarsModel.newCarsAds(data);
            if (platformId == SponsoredNavigation.platform.Desktop)
                ko.applyBindings(SponsoredNavigation.koNewCarsModel, document.getElementById("quickResearch"));
            ko.applyBindings(SponsoredNavigation.koNewCarsModel, document.getElementById("navNewCars"));
        }
        else
        {
            SponsoredNavigation.koSpecialsModel.specialsAds(data);
            if (platformId == SponsoredNavigation.platform.Desktop)
                ko.applyBindings(SponsoredNavigation.koSpecialsModel, document.getElementById("specialsAds"));
            ko.applyBindings(SponsoredNavigation.koSpecialsModel, document.getElementById("navSpecials"));
        }
        $("#cw-top-navbar").find("img.lazy").lazyload(); // bind lazy load to knockout bindings
    },
    koNewCarsModel: {
        newCarsAds: ko.observableArray()
    },
    koSpecialsModel: {
        specialsAds: ko.observableArray()
    }
}
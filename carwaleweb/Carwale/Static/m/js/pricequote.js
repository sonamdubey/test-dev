var PQ = {
        getCityZonesForBinding: function (json) {
            var cityZones = [];
            if (json == null || json.length == 0) {
                return cityZones;
            }
            $.each(json.cities, function (index, city) {
                cityZones.push({ 'cityId': city.id, 'cityName': city.name, 'zoneId': 0, 'zoneName': '', 'orderName': city.name });
            });
            $.each(json.groupCities, function (index, groupCity) {
                $.each(groupCity.zones, function (index, zone) {
                    cityZones.push({ 'cityId': groupCity.id, 'cityName': groupCity.name, 'zoneId': zone.id, 'zoneName': zone.name, 'orderName': zone.name });
                });
                $.each(groupCity.group, function (index, group) {
                    cityZones.push({ 'cityId': group.id, 'cityName': group.name, 'zoneId': 0, 'zoneName': '', 'orderName': group.name });
                });
            });

            return cityZones.sort(function (element1, element2) {
                var orderName1 = element1.orderName.toLowerCase();
                var orderName2 = element2.orderName.toLowerCase();
                if (orderName1 < orderName2)
                    return -1;
                if (orderName1 > orderName2)
                    return 1;
                return 0;
            });
        },
        setPQCityCookies: function (cityId, cityName, zoneId) {
            document.cookie = '_CustCityId=' + cityId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_CustCity=' + cityName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
            document.cookie = '_PQZoneId=' + zoneId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
        },
        PageId: { DealerLocatorModelCarousel: 114, PriceInCityPage: 135 },

        redirectToNewPqPage: function (modelId, versionId, cityId, areaId, isCrossSellPriceQuote, pageId, node, cardId, hideCampaign) {
            location.href = "/quotation/?m=" + modelId + "&v=" + versionId + "&c=" + cityId + "&a=" + areaId + "&p=" + pageId;
        }
};


function getOnRoadPQ(e, modelId, pageId) {
    redirectOrOpenPopup(e, pageId);
}
function redirectOrOpenPopup(btn, pqpageid) {
    var modelId = $(btn).attr('modelid');
    var versionId = $(btn).attr('versionid') || "0";
    var isCityPage = pqpageid == PQ.PageId.DealerLocatorModelCarousel || pqpageid == PQ.PageId.PriceInCityPage;
    var cityId = isCityPage ? $(btn).attr("cityid") : $.cookie('_CustCityIdMaster');
    var areaId = isCityPage && Number(cityId) != Number($.cookie('_CustCityIdMaster')) ? "" : $.cookie('_CustAreaId');
    var cityName = isCityPage ? $(btn).attr("cityname") : $.cookie('_CustCityMaster');
    var zoneId = isCityPage && Number(cityId) != Number($.cookie('_CustCityIdMaster')) ? "" : $.cookie('_CustZoneIdMaster');

    if ($.inArray(Number(cityId), askingAreaCityId) < 0 || Number(areaId) > 0) {
        PQ.redirectToNewPqPage(modelId, versionId, cityId, areaId, false, pqpageid, null, null, false);
    }
    else {
        new globalLocation.BL().openLocHint({ cityId: cityId, cityName: cityName }, globalLocation.expectedUserInput.MandatoryArea, function (payload) {
            var areaId = payload.areaId;
            if (typeof areaId === 'undefined') {
                areaId = 0;
            }
            PQ.redirectToNewPqPage(modelId, versionId, payload.cityId, areaId, false, pqpageid, null, null, false);
        }, null, false);
    }
}
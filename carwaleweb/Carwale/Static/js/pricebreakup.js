var PriceBreakUp = {
    Quotation: {
        RedirectToPQ: function (pqInput, openInNewTabParam) {
            var openInNewTab = (typeof openInNewTabParam !== 'undefined') ? openInNewTabParam : false;
            var isNewPqPageToBeShown = (Number(abTestValue) >= Number(abTestMinValForNewPqDesktop) && Number(abTestValue) <= Number(abTestMaxValForNewPqDesktop));
            PriceBreakUp.Quotation.setPQCityCookies(pqInput);

            var areaId = pqInput.location.areaId;
            if (typeof areaId === 'undefined') {
                areaId = 0;
            }

            var pageUrl = "";
            if (isNewPqPageToBeShown) {
                pageUrl = "/quotation/?m=" + pqInput.modelId + "&v=" + pqInput.versionId + "&c=" + pqInput.location.cityId + "&a=" + areaId + "&p=" + pqInput.pageId;
            }
            else {
                if (pqInput.location.areaId > 0) {
                    pageUrl = '/new/quotation.aspx?areaId=' + pqInput.location.areaId;
                }
                else {
                    pageUrl = '/new/quotation.aspx';
                }
            }

            if (openInNewTab) {
                window.open(pageUrl);
            }
            else {
                window.location.href = pageUrl;
            }
        },
            setPQCityCookies: function (pqInput) {
                PriceBreakUp.Quotation.setPQLocation(pqInput.location);
                document.cookie = '_PQModelId=' + (pqInput.modelId || -1) + '; expires = ' + permanentCookieTime() + '; domain=' + defaultCookieDomain + '; path =/';
                document.cookie = '_PQVersionId=' + (pqInput.versionId || -1) + '; expires = ' + permanentCookieTime() + '; domain=' + defaultCookieDomain + '; path =/';
                document.cookie = '_PQPageId=' + (pqInput.pageId || -1) + '; expires = ' + permanentCookieTime() + '; domain=' + defaultCookieDomain + '; path =/';
            },

            getGlobalLocation: function () {
                var cityId = -1, cityName = '', zoneId = -1, zoneName = '', areaId = -1, areaName = '';
                if (typeof isCityPage != "undefined" && isCityPage && CityId != $.cookie("_CustCityIdMaster")) {
                    cityId = CityId;
                    cityName = CityName;
                }
                else {
                    cityId = $.cookie("_CustCityIdMaster");
                    cityName = $.cookie("_CustCityMaster");
                    zoneId = $.cookie("_CustZoneIdMaster");
                    zoneName = $.cookie("_CustZoneMaster");
                    areaId = $.cookie("_CustAreaId");
                    areaName = $.cookie("_CustAreaName");
                }

                var locationObj;
                if (Number(cityId) > 0) {
                    if ($.inArray(Number(cityId), askingAreaCityId) >= 0 && Number(areaId) > 0) {
                        locationObj = { cityId: cityId, cityName: cityName, zoneId: zoneId, zoneName: zoneName, areaId: areaId, areaName: areaName, isComplete: true };
                        return locationObj;
                    } else if ($.inArray(Number(cityId), askingAreaCityId) < 0) {
                        locationObj = { cityId: cityId, cityName: cityName, isComplete: true };
                        return locationObj;
                    } else {
                        locationObj = { cityId: cityId, cityName: cityName, isComplete: false };
                        return locationObj;
                    }
                } else {
                    return null;
                }
            },

            getPQLocation: function () {
                var cityId = $.cookie('_CustCityId');
                var cityName = $.cookie('_CustCity');
                var zoneId = $.cookie('_PQZoneId');

                var locationObj;
                if (zoneId != null && zoneId.length > 0) {
                    locationObj = { cityId: cityId, cityName: cityName, zoneId: zoneId };
                }
                else {
                    locationObj = { cityId: cityId, cityName: cityName };
                }
                locationObj.isComplete = false;
                return locationObj;
            },

            setPQLocation: function (location) {

                var globalCityId = $.cookie("_CustCityIdMaster");
                var globalAreaId = $.cookie("_CustAreaId");

                var now = new Date();
                var Time = now.getTime();
                Time += 1000 * 60 * 60 * 24 * 30;
                now.setTime(Time);

                var label = location.cityName + "," + Location.USERIP;

                if (globalCityId === null || globalCityId === "-1" || globalCityId === "") {
                    document.cookie = '_CustCityIdMaster=' + location.cityId + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
                    document.cookie = '_CustCityMaster=' + location.cityName + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
                    document.cookie = '_CustZoneIdMaster=' + (location.zoneId || -1) + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
                    document.cookie = '_CustAreaId=' + (location.areaId || -1) + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
                    document.cookie = '_CustAreaName=' + (location.areaName || "Select Area") + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';

                    Common.utils.trackAction("CWNonInteractive", "Desktop_UserAction_GlobalCitySet", "PQCitySet", label);
                }
                if (location.zoneId > 0) {
                    if (Number(location.cityId) === Number(globalCityId) && Number(globalAreaId) <= 0) {
                        var areaName = location.areaName || "Select Area";
                        document.cookie = '_CustZoneIdMaster=' + location.zoneId + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
                        document.cookie = '_CustAreaId=' + (location.areaId || -1) + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
                        document.cookie = '_CustAreaName=' + areaName + '; expires = ' + now.toGMTString() + '; domain=' + defaultCookieDomain + '; path =/';
                        Common.utils.trackAction("CWNonInteractive", "Desktop_UserAction_GlobalCitySet", "PQOnlyZoneSet", label);
                    }
                }

                document.cookie = '_CustCityId=' + location.cityId + '; expires = ' + permanentCookieTime() + '; domain=' + defaultCookieDomain + '; path =/';
                document.cookie = '_CustCity=' + location.cityName + '; expires = ' + permanentCookieTime() + '; domain=' + defaultCookieDomain + '; path =/';
                document.cookie = '_PQZoneId=' + (location.zoneId || "") + '; expires = ' + permanentCookieTime() + '; domain=' + defaultCookieDomain + '; path =/';
            },

            prefillGlobalCity: function (selector) {
                var cityId = masterCityIdCookie;
                var cityName = masterCityNameCookie;
                var areaId = $.cookie('_CustAreaId');
                var areaName = $.cookie('_CustAreaName');
                if (cityId > 0) {
                    if (areaId > 0) {
                        selector.attr('value', areaName + ', ' + cityName);
                        selector.data('cityId', cityId);
                        selector.data('cityName', cityName);
                        selector.data('areaId', areaId);
                        selector.data('areaName', areaName);
                    }
                    else {
                        if ($.inArray(Number(cityId), askingAreaCityId) < 0) {
                            selector.attr('value', cityName);
                            selector.data('cityId', cityId);
                            selector.data('cityName', cityName);
                        }
                    }
                }
            },

            validLocation: function (selector, cityId, cityName) {
                if ($.inArray(Number(selector.attr('data-pageId')), [45, 46, 55]) >= 0) //price-in-city page-ids hardcoded
                {
                    if ($.inArray(Number(cityId), askingAreaCityId) >= 0) {
                        return { cityId: cityId, cityName: cityName, isComplete: false };
                    } else {
                        return { cityId: cityId, cityName: cityName, isComplete: true };
                    }
                } else {
                    return PriceBreakUp.Quotation.getGlobalLocation();
                }
            }
        }
    }

$(document).on('click', '.open-all-offers', function () {
    var $currentOffers = $(this);
    var closestOemOffers = $currentOffers.closest('.oemOfferList').find('.oem-offers__li');
    var offerList = $currentOffers.parent().find('.offerList');
    closestOemOffers.removeClass('hide');
    offerList.removeClass('truncate-offer');
    closestOemOffers.find('.open-all-offers').hide()
});
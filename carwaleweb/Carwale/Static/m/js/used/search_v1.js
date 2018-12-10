var search = (function () {
    var _isPageLoad = false;
    var $blackOutWindow = $("#m-blackOut-window");
    function setIsPageLoad(value) {
        _isPageLoad = value;
    };
    function getIsPageLoad() {
        return _isPageLoad;
    };
    function _showSimilarCars(response, btnSimilarCars) {
        if (response) {
            $('#popup-suggetions').html(response);
            $('#showSimilarCar').show('slide', { direction: 'right' }, 500, function () {
                $('.popup-suggestion-box').removeClass('hide');
                $blackOutWindow.hide();
                window.history.pushState("recommendedCarsPopup", "", "");
                m_bp_additonalFn.sellerDetailsBtnTextChange();
            });
        }
        else {
            btnSimilarCars.text("No Similar Cars").addClass('no-similar-cars').removeClass('text-link car-choices');
        }
        m_bp_additonalFn.sellerDetailsBtnTextChange();
        Common.utils.hideLoading();
    };
    function _isCityNotAvailable() {
        return (typeof globalCity !== 'undefined' && !globalCity.isGlobalCityPresent() && isNaN($('#listingTitle').attr('cityid')) && !commonUtilities.getFilterFromQS('city'));
    }
    function _showCityPopup() {
        $(document).on('locationloaded', function () {
            if (typeof LOCATION_EVENTS !== 'undefined' && typeof locationValue !== 'undefined') {
                if (_isCityNotAvailable() && commonUtilities.getFilterFromQS('utm_source') === "facebook") { //disable city popup cross icon for facebook traffic without city
                    LOCATION_EVENTS.hideCrossIcon();
                }
                LOCATION_EVENTS.openPopup(locationValue.getCarsNearMeCities()); // to show areas of only bangalore city.
            }
        });
    }
    function _removeIsSoldInQS() {
        var queryString = window.location.search.slice(1);
        if (commonUtilities.getFilterFromQS("issold", queryString)) {
            setTimeout(function () { $("#soldoutBox").hide("slow"); }, 15000);
            var currentQueryString = commonUtilities.removeFilterFromQS({ "issold": true }, queryString);
            var currURL = window.location.pathname;
            if (currentQueryString) {
                currURL = "?" + currentQueryString;
            }
            history.replaceState(window.location.pathname + currentQueryString, "", currURL);
        }
    };
    function _stopScrollAtback() {
        $('#carDetails').bind('mousewheel DOMMouseScroll', function (e) {
            var e0 = e.originalEvent,
                delta = e0.wheelDelta || -e0.detail;
            this.scrollTop += (delta < 0 ? 1 : -1) * 30;
            e.preventDefault();
        });
    };
    function _setQueryString() {
        if (currQs) {
            history.replaceState(currQs, "", window.location.pathname + "?" + currQs);
        }
        var qs = window.location.search;
        if (!commonUtilities.getFilterFromQS("city", qs) && _isCityNotAvailable())
            _showCityPopup();
    };
    function pageLoad() {
        _setQueryString();
        _removeIsSoldInQS();
        m_bp_additonalFn.sellerDetailsBtnTextChange();
        _stopScrollAtback();
        var cityId = commonUtilities.getFilterFromQS("city", window.location.search);
        sort.setSortValues();
        search.setLocationInfoInSessionStorage();
    };
    function _publishRecommendedCarsClick(element, callback) {
        var $btnSimilarCars = $(element);
        var similarCarsUrl = $btnSimilarCars.attr('popupurl');
        if (similarCarsUrl && similarCarsUrl.indexOf("/used/dealers") > -1) {
            window.location.href = similarCarsUrl;
        }
        else {
            Common.utils.showLoading();
            $blackOutWindow.show();
            $.ajax({
                url: similarCarsUrl,
                success: function (response) {
                    callback(response, $btnSimilarCars);
                }
            });
        }
    };
    function _similarCarBackBtn() {
        history.back();
    };
    function showSimilarCarsLink($node, profileId) {
        var minLimitForLink = 17;
        if ($node.find(".recommendedCarsPopupBtn").length === 0 && $("#searchListingCount").attr("count") > minLimitForLink) {
            var linkText, similarCarPopupUrl;
            var gsdBtn = $node.find(".getSellerDetails");
            var dealerCarsUrl = gsdBtn.data("dealercarsurl");
            var dataAction, dataAct;
            if (dealerCarsUrl && dealerCarsUrl.length > 1) {
                linkText = "More Cars of this Dealer";
                similarCarPopupUrl = dealerCarsUrl;
                dataAction = "ShowMoreCars_Click";
                dataAct = "ShowMoreCars_Impressions";
            }
            else {
                linkText = "Similar Cars";
                similarCarPopupUrl = gsdBtn.attr("popupurl");
                dataAction = "ShowSimilarCars_Click";
                dataAct = "ShowSimilarCars_Impressions";
            }
            var similarCarLinkHtml = '<p class="car-choices recommendedCarsPopupBtn" data-role="click-tracking" data-event="CWNonInteractive" data-label="' + profileId +'" data-action="' + dataAction + '" data-cat="MSite_UsedCarSearch" data-act= "' + dataAct + '" popupUrl=' + similarCarPopupUrl + '>' + linkText + '</p>';
            $node.append(similarCarLinkHtml);
        }
    };
    function _changeCityFromWarning() {
        $("div.global-location").trigger('click');
        commonUtilities.addToHistory("globalLocation");
    };

    function getShouldFetchNearByCarQS(filterSet) {
        var removeFilterSet = {};
        if (toString.call(filterSet) === '[object Object]') {
            removeFilterSet = filterSet;
        }
        removeFilterSet["shouldfetchnearbycars"] = true;
        var currQS = commonUtilities.removeFilterFromQS(removeFilterSet, window.location.search.slice(1));
        var shouldFetchNearByCarsQS = filterValues.getShouldFetchNearByCarQS();
        if (shouldFetchNearByCarsQS.length > 0) {
            currQS += '&' + shouldFetchNearByCarsQS;
        }
        return currQS;
    }

    function _toggleCarsNearMe() {
        var currQS = getShouldFetchNearByCarQS();
        var apiUrl = "/m/search/getsearchresults/?" + currQS;
        search.listing.getData(apiUrl, false, { isFilter: true });
    }
    function isAutoDetectCityChanged(params) {
        if (typeof LOCATION_EVENTS !== 'undefined' && params.newValue === false && params.newStore.location.autoDetect === true) {
            LOCATION_EVENTS.openPopup();
        } else {
            errorProcessLocation();
        }
    }
    function errorProcessLocation() {
        $("#cw_loading_icon").addClass('hide');
        // show error toast
    }

    var _infiniteScroll = (function () {
        var _PAGESIZE = 19;
        var _lastScrollTopPosition = 0;
        var _listingApiHitFired = false;

        function _setGoToTopIconVisibility() {
            var currentScrollTopPosition = $(window).scrollTop();
            if (currentScrollTopPosition > _lastScrollTopPosition || currentScrollTopPosition < 100) {
                $('#gotoTop').hide();
            }
            else {
                $('#gotoTop').show();
            }
        };

        function _scrollApiHit() {
            if (!$('#buyerForm').is(':visible') && !_listingApiHitFired && _getScrollValueRelativeToTop() > 0.8) {
                _listingApiHitFired = true;
                if (nextPageUrl) {

                    $("#listingEndText").text("Loading...");
                    listing.getData(nextPageUrl, true);

                }
                else {
                    $("#listingEndText").text("End");
                }
            }
        };
        function _setLastScrollPos(value) {
            _lastScrollTopPosition = value;
        };
        function _updateApiHit() {
            _listingApiHitFired = false;
        };
        function _getScrollValueRelativeToTop() {
            return (($(window).scrollTop()) / ($(document).height() - $(window).height()));
        };
        return {
            _PAGESIZE: _PAGESIZE,
            _setGoToTopIconVisibility: _setGoToTopIconVisibility,
            _scrollApiHit: _scrollApiHit,
            _updateApiHit: _updateApiHit,
            _setLastScrollPos: _setLastScrollPos
        };
    })();

    var listing = (function () {
        function lazyLoadImages() {
            $('img.lazy').lazyload({
                load: function () {
                    var $x = $(this)[0];
                    var currentListing = $(this).parents('li');
                    var listingsCount = document.getElementById("searchListingCount").getAttribute('count');
                    searchTracking.triggerTracking(currentListing[0], listingsCount, 'SearchListing', true); // true => if you want to track query string patameters
                }
            });
        };
        function getData(apiUrl, isScroll, data) {
            var rootId = commonUtilities.getFromLocalStorage("userPreferredRootId");
            if (rootId)
                apiUrl = apiUrl + (apiUrl.indexOf('?') >= 0 ? "&" : "?") + "userPreferredRootId=" + rootId;
            commonUtilities.apiRequestHandler(apiUrl, "", data, { "sourceid": "43" }, function (response) {
                _apiResponseHandler(response, isScroll, data);
                Common.utils.hideLoading();
                var cityId = commonUtilities.getFilterFromQS("city", window.location.search);
            });
        };

        function _apiResponseHandler(response, isScroll, data) {
            if (response) {
                if (data && data.isFilter) {
                    $("#body").html(response);
                    window.scrollTo(0, 0);
                } else {
                    $("#listing .pagination").last().hide();
                    (isScroll) ? $(".search-list ul").append(response) : $(".search-list ul").html(response);
                }
                if (!commonUtilities.isScrollBarPresent()) {
                    sortFilterButton.showFilterModel();
                }
                m_bp_additonalFn.sellerDetailsBtnTextChange();;
                lazyLoadImages();
                _infiniteScroll._updateApiHit();
            }
        };

        return {
            getData: getData,
            lazyLoadImages: lazyLoadImages
        };
    })();

    (function () {
        var sortBar = $(".sort-filter-div").offset().top;
        $(window).scroll(debounce(function () {
            var winScroll = $(window).scrollTop();
            if (winScroll > sortBar) {
                $('.sort-filter-div').addClass("sort-filter-div-fixed");
            } else {
                $('.sort-filter-div').removeClass("sort-filter-div-fixed");
            }
            _infiniteScroll._setGoToTopIconVisibility();
            _infiniteScroll._setLastScrollPos(winScroll);
            _infiniteScroll._scrollApiHit();
        }, 100));
    })();

    function registerEvents() {
        $(document).on('click', '.mozillaBlankTabBugFix', function () {
            window.open($(this).attr('href'));
            return false;
        });
        $("#btnSoldOutClose").on("click", function () {
            $("#soldoutBox").hide("slow");
        });
        $('#nocars span.changeCityLink,#warnCity span.changeCityLink').on('click', function () {
            _changeCityFromWarning();
        });
        $('#gotoTop').click(function () {
            $('body,html').animate({
                scrollTop: 0
            }, 1000);
        });
        $(document).on('click', '.recommendedCarsPopupBtn', function () {
            msite_ORIGIN_ID = 19;
            _publishRecommendedCarsClick(this, _showSimilarCars);
        });
        $('#simillarCarsPopupBackArrow').on('click', _similarCarBackBtn);
        $(document).on('click', 'li[id^="searchListingMobile-"]', function () {
            var $node = $(this).find(".stockDetailsBlock");
            var profileId = $(this).attr("profileid");
            showSimilarCarsLink($node, profileId);
        });
        $(document).on('click', '.common-certification-program-slug', function (e) {
            Common.utils.callTracking($(this));
            e.stopPropagation();
        });

        $(document).on('change', '.area-near-cars input[type=checkbox]', function () {
            var isChecked = $(this).is(':checked');
            if (typeof locationStorage !== 'undefined') {
                var sessionObj = locationStorage.getLocationSessionStorage()
                var trackingLabel = (sessionObj[locationStorage.locationSessionStorageKeys.cityName] ?
                    sessionObj[locationStorage.locationSessionStorageKeys.cityName] : "") +
                    (sessionObj[locationStorage.locationSessionStorageKeys.areaName] ? ("|" + sessionObj[locationStorage.locationSessionStorageKeys.areaName]) : "");
                Common.utils.trackAction("CWInteractive", "MSite_UsedCarSearch", isChecked ? "CarsNearMe_LeftTick_Check" : "CarsNearMe_LeftTick_Uncheck", trackingLabel);
            }
            _toggleCarsNearMe();
        });
        $(document).on('click', '.detect-location-btn', function () {
            if (typeof LOCATION_EVENTS !== 'undefined' && typeof locationValue !== 'undefined' && navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    LOCATION_EVENTS.openPopup(locationValue.getCarsNearMeCities());
                    LOCATION_EVENTS.detectLocation(position);
                }, function () { errorProcessLocation(); });
            }
        });
        $(document).on('click', '.city-area-btn,.change-area-btn', function () {
            typeof LOCATION_EVENTS !== 'undefined' && typeof locationValue !== 'undefined' && LOCATION_EVENTS.openPopup(locationValue.getCarsNearMeCities());
        });
}

    function setLocationInfoInSessionStorage() {
        if (search.getIsPageLoad()) {
            var currentQueryString = location.search.replace('?', '');
            var cityId = parseInt(commonUtilities.getFilterFromQS("city", currentQueryString));
            var cityName = filterValues.getCityName(cityId);
            if (cityId && cityName) {
                cityId = cityId !== 3000 ? cityId : 1;
                var cityObj = {};
                cityObj[locationStorage.locationSessionStorageKeys.cityName] = cityName;
                cityObj[locationStorage.locationSessionStorageKeys.cityId] = cityId;
                locationStorage.setLocationSessionStorage(cityObj);
                var cookieCityId = parseInt($.cookie('_CustCityIdMaster'));
                var areaObj;
                if (cityId === cookieCityId) {
                    var areaId = parseInt($.cookie('_CustAreaId'));
                    var areaName = $.cookie('_CustAreaName');
                    if (areaId && areaName) {
                        areaObj = {};
                        areaObj[locationStorage.locationSessionStorageKeys.areaId] = areaId;
                        areaObj[locationStorage.locationSessionStorageKeys.areaName] = areaName;
                        locationStorage.setLocationSessionStorage(areaObj);
                    }
                }
                if (typeof LOCATION_EVENTS !== 'undefined') {
                    LOCATION_EVENTS.setCity(cityObj);
                    if (areaObj) {
                        LOCATION_EVENTS.setArea(areaObj);
                    }
                }
            }
        }
    }
    return {
    registerEvents: registerEvents,
    setIsPageLoad: setIsPageLoad,
    getIsPageLoad: getIsPageLoad,
    listing: listing,
    pageLoad: pageLoad,
    showSimilarCarsLink: showSimilarCarsLink,
    setLocationInfoInSessionStorage: setLocationInfoInSessionStorage,
    isAutoDetectCityChanged: isAutoDetectCityChanged,
    getShouldFetchNearByCarQS: getShouldFetchNearByCarQS
};
})();
var masterCity = function () {

    function _goToSearchPage() {
        $('.valuation-result-section').hide('slide', { direction: 'right' }, 500);
        Common.utils.unlockPopup();
        $("#showSimilarCar").hide('slide', { direction: 'right' }, 500);
        m_bp_additonalFn.sellerDetailsBtnTextChange();
    };
    function _globalCityChanged(cityId, areaId, latitude, longitude) {

        var currQS = window.location.search;
        var removeQsParamsKeys = {};
        removeQsParamsKeys["city"] = true;
        removeQsParamsKeys["latitude"] = true;
        removeQsParamsKeys["longitude"] = true;
        removeQsParamsKeys["area"] = true;
        removeQsParamsKeys["shouldfetchnearbycars"] = true;
        currQS = "?" + commonUtilities.removeFilterFromQS(removeQsParamsKeys, currQS.slice(1));

        if (cityId > 0) {
            currQS = commonUtilities.replaceOrInsertInQs("city", cityId, currQS);
        }
        if (_isValidLatitude(latitude) && _isValidLongitude(longitude)) {
            currQS = commonUtilities.replaceOrInsertInQs("latitude", latitude, currQS);
            currQS = commonUtilities.replaceOrInsertInQs("longitude", longitude, currQS);
        }

        if (areaId > 0) {
            currQS = commonUtilities.replaceOrInsertInQs("area", areaId, currQS);
        }

        var url = "/m/search/getsearchresults/" + currQS;
        search.listing.getData(url, false, { isFilter: true });
        _goToSearchPage();
        window.history.pushState("globalcityChanged", "", "/m/used/cars-for-sale/" + currQS);
        cwTracking.trackCustomData('PageViews', '', 'NA', true);
    };
    function _isValidLatitude(latitude) {
        return latitude && !(latitude > 90 || latitude < -90);
    }

    function _isValidLongitude(longitude) {
        return longitude && !(longitude > 180 || longitude < -180);
    }

    function _masterCityChanged(cityId, areaId, latitude, longitude) {
        $('#buyerForm').removeClass('changeCityPosition');
        m_bp_getSellerDetails.closeFormPopup();
        if (typeof globalLocation.coreInstance !== "undefined" && globalLocation.coreInstance)
            globalLocation.coreInstance = null;
        events.publish('applyFilterclicked', true);
        _globalCityChanged(cityId, areaId, latitude, longitude);
        window.scrollTo(0, 0);
    }
    function locationChanged(params) {
        if (params.newValue === false &&
            !filters.isFilterPopupOpened &&
            !commonUtilities.areObjectsEqual(params.newStore.location, params.oldStore.location, locationStorage.locationIngnoredKeyForEquality)
        ) {
            var location = locationStorage.getLocationObjFromStore(params.newStore);

            _masterCityChanged(location.cityId, location.areaId, location.latitude, location.longitude);
            locationStorage.setLocationSessionStorage(location);
            if (typeof LOCATION_EVENTS !== 'undefined') {
                LOCATION_EVENTS.showCrossIcon();
            }
        }
    }
    return {
        locationChanged: locationChanged
    };
}();

var sortFilterButton = function () {
    var $btnSortFilter;

    function _setSelector() {
        $btnSortFilter = $('#btnSortFilter');
    }

    function _handleVisibility() {
        var timeOut;
        window.addEventListener('scroll', debounce(function () {
            _setSelector();
            $btnSortFilter.addClass('sort-filter--animate');
            clearTimeout(timeOut);
            timeOut = setTimeout(function () {
                if (commonUtilities.isScrollBarPresent()) {
                    $btnSortFilter.removeClass('sort-filter--animate');
                }
            }, 3000);
        }, 100))
        showFilterModel();
    }
    function showFilterModel() {
        _setSelector();
        if (!commonUtilities.isScrollBarPresent()) {
            $btnSortFilter.addClass('sort-filter--animate');
        }
    }
    function registerEvents() {
        _setSelector();
        _handleVisibility();
    }
    return {
        registerEvents: registerEvents,
        showFilterModel: showFilterModel
    }
}();

var cardHide = function () {
    function registerEvents() {
        $(document).on('click', '.close-icon', function () {
            $(this).closest('.near-cars-card').addClass('card--inactive');
        });
    }
    return {
        registerEvents: registerEvents
    };
}();

$(document).ready(function () {
    cookie.setCookie("viewPortHeight", $(window).height());
    sortFilterButton.registerEvents();
    search.registerEvents();
    search.listing.lazyLoadImages();
    search.setIsPageLoad(true);
    cardHide.registerEvents();
    $(window).on('resize', search.resizeEvent);
    if (typeof globalLocation !== "undefined") {
        globalLocation.onLocationStorePropertyChange("isActive", masterCity.locationChanged);
        globalLocation.onLocationStorePropertyChange("location.isFetching", search.isAutoDetectCityChanged);
    }
    search.pageLoad();
    search.setIsPageLoad(false);
    if (typeof cwUsedTracking !== 'undefined') {
        cwUsedTracking.setEventCategory(cwUsedTracking.eventCategory.UsedSearchPage);
    }
});
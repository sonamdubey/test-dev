$(document).on("mastercitychange", function (event, cityName, cityId) {
    var url = window.location.href;
    url = url.replace(/cityId=(-)?[0-9]*/g, 'cityId=' + cityId);
    window.location.href = url;
});

$("#more-brand-tab").click(function (e) {
    e.preventDefault();
    $(".brand-type-container ul").toggleClass("animate-brand-ul");
    $("html, body").animate({ scrollTop: $("#brand-nav").offset().top }, 1000);
    var b = $(this).find("span");
    b.text(b.text() === "more" ? "less" : "more");
});

function initCityPopUp() {
    var div = $('.btnCityPopUp');
    if (window.LocationSearch) {
        LocationSearch(div, {
            showCityPopup: true,
            callback: function (locationObj) {
                var areaId = typeof locationObj.areaId !== "undefined" ? locationObj.areaId : "";
                var areaName = typeof locationObj.areaName !== "undefined" ? locationObj.areaName : "Select Area";
                Location.globalSearch.setLocationCookies(locationObj.cityId, locationObj.cityName, areaId, areaName, locationObj.zoneId, locationObj.zoneName);
                window.location.reload();
            },
            isDirectCallback: true,
            isAreaOptional: true,
            validationFunction: function () {
                return;
            }
        });
    }
}

function initViewPriceBreakUp() {
    var div = $('.btnChkOnRoadPrice');
    if (window.LocationSearch) {
        var location = new LocationSearch((div), {
            showCityPopup: true,
            prefillPopup: true,
            isAreaOptional: true,
            callback: function (locationObj) {
                var carModelId = location.selector().attr('data-modelid');
                var pageId = location.selector().attr('data-pageId');

                PriceBreakUp.Quotation.RedirectToPQ({ 'modelId': carModelId, 'location': locationObj, 'pageId': pageId });
            },
            isDirectCallback: true,
            validationFunction: function () {
                return PriceBreakUp.Quotation.getGlobalLocation();
            }
        });
    }
}

var _infiniteScroll = (function () {
    var _PAGESIZE = 19;
    var _lastScrollTopPosition = 0;
    var _topCarsApiHitFired = false;

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
        if (!_topCarsApiHitFired && _getScrollValueRelativeToTop() > 0.8) {
            if (nextPageUrl) {
                _topCarsApiHitFired = true;
                modelList.appendList();
            }
            else {
                $("#listingEndText").text("End");
            }
        }
    };
    function _setLastScrollPos(value) {
        _lastScrollTopPosition = value;
    };
    function _setApiHitFlag(value) {
        _topCarsApiHitFired = value;
    };
    function _updateApiHit() {
        _listingApiHitFired = false;
    };
    function _getScrollValueRelativeToTop() {
        return (($(window).scrollTop()) / ($("#topCarList").height() - $(window).height()));
    };
    return {
        _PAGESIZE: _PAGESIZE,
        _setGoToTopIconVisibility: _setGoToTopIconVisibility,
        _scrollApiHit: _scrollApiHit,
        _updateApiHit: _updateApiHit,
        _setLastScrollPos: _setLastScrollPos,
        _setApiHitFlag: _setApiHitFlag
    };
})();


var loadModelList = function () {
    var _queryString = location.search.split('?')[1];
    var _loadModelList = {};
    var _apiUrl = "/topcars/filtered/";
    $(window).scroll(debounce(function () {
        var winScroll = $(window).scrollTop();
        _infiniteScroll._setGoToTopIconVisibility();
        _infiniteScroll._setLastScrollPos(winScroll);
        _infiniteScroll._scrollApiHit();
    }, 50));

    $('#gotoTop').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 1000);
    });

    window.onpopstate = function (state) {
        var search = location.search.split('?')[1];
        if ((search == "" || search == undefined) && isFilterApplied) {
            Loader.showFullPageLoader();
            window.location.reload();
        }
        else if (modelList.queryString != search) {
            replaceList(search);
        }

    }

    var replaceList = function (queryString) {
        modelList.queryString = queryString;
        Loader.showFullPageLoader();
        $(window).scrollTop(0);
        queryString = queryString + "&pageNo=1&pageSize=5";
        $.get(_apiUrl + "?" + queryString, function (data) {
            $('.filterPluginBtn').each(function () { $(this).attr('data-filterpreselected', '') });
            isFilterApplied = true;
            $('.js-replace-li').html(data)
            setTimeout(function () {
                _infiniteScroll._setApiHitFlag(false);
                $('#headingContent').remove();
                $('#topCarsCarousal').remove();
                $('#ncfSlugBottom').remove();
                $('.filter-count').text(filterAppliedCount);
                $('.toast-box').text(totalModels + (totalModels == 1 ? ' car' : ' cars') + ' matching your criteria').show();
                setTimeout(function () {
                    $('.toast-box').fadeOut('slow');
                }, 4000);
                if (totalModels > 0) {
                    $('.listing__no-result').hide();
                    $('#filterPluginBtn').show();
                }
                else {
                    $('.listing__no-result').show();
                    $('#filterPluginBtn').hide();
                }
                Loader.hideFullPageLoader();
            }, 0);
        });
    }

    var appendList = function () {
        Loader.showFullPageLoader();
        $.get(nextPageUrl, function (data) {
            _infiniteScroll._setApiHitFlag(false);
            $('.js-replace-li').append(data);
            Loader.hideFullPageLoader();
        });
    }

    _loadModelList.replaceList = replaceList;
    _loadModelList.appendList = appendList;
    _loadModelList.queryString = _queryString;
    return _loadModelList;
}

var GetFilteredModels = function (queryParams) {
    if (queryParams != undefined && queryParams.split("&").length == 2 && queryParams.indexOf("bodyStyleIds=") >= 0 && queryParams.indexOf("bodyStyleIds=" + bodyStyleId) >= 0 && (queryParams.split("bodyStyleIds=" + bodyStyleId)[1] == "" || queryParams.split("bodyStyleIds=" + bodyStyleId)[1] == undefined)) {
        if (!isFilterApplied) {
            window.history.replaceState("", null, location.pathname);
        }
        else {
            modelList.replaceList(queryParams);
        }
    }
    else if (queryParams != undefined) {
        modelList.replaceList(queryParams);
    }
}

$(document).ready(function () {
    initViewPriceBreakUp();
    initCityPopUp();
    if (useFilterPlugin) {
        modelList = new loadModelList();
    }
    userSliderChange = false;
});

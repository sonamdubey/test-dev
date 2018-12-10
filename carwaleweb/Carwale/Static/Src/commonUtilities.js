var commonUtilities = (function () {
    function numberWithCommas(num) {
        if (num)
            return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return undefined;
    };

    function findAndRemove(array, property, value) {
        array = array.filter(function (obj) {
            return obj[property] !== value;
        });
        return array;
    };

    function findArrayElement(array,property, element) {
        for (var i = 0; i < array.length; i++)
            if (array[i][property] == element)
                return true;
        return false;
    };

    function isInteger(x) {
        return x % 1 === 0;
    };

    function addToHistory(currentState) {
        window.history.pushState({ currentState: currentState }, "", "");
    };
    function formatSpecData(input) {
        return removeTilde(val.replace('CUSTOM', '').replace('BIT', ''))
    };

    function apiRequestHandler(apiUrl, requestType, requestData, requestHeader, responseHandler) {
        if (requestData)
            Common.utils.showLoading();
        $.ajax({
            url: apiUrl,
            type: requestType || "GET",
            data: requestData,
            async: true,
            headers: requestHeader || "",
            success: function (response, status) {
                responseHandler(response, status);
            },
            error: function (response, status) {
                responseHandler(response, status);
            }
        });
    };

    function getFromLocalStorage(key) {
        if (window.localStorage) {
            return window.localStorage.getItem(key);
        }
    };
    function removeFilterFromQS(removeFilterSet, url) {
        var params = url.split("&");
        for (var i = params.length ; i-- > 0  ;) {
            if (removeFilterSet[params[i].split("=")[0]]) {
                params.splice(i, 1);
            }
        }
        return params.join("&");
    };
    function getBulkValuesFromQs(querySet, qs) {
        var params = qs.split('&');
        for (var i = 0 ; i < params.length ; i++) {
            if (querySet[params[i].split('=')[0]]) {
                querySet[params[i].split('=')[0]] = params[i].split('=')[1];
            }
        }
    };
    function getFilterFromQS(name, url) {
        url = url || location.search;
        var params = url.replace('?', '').split('&');
        for (var i = 0; i < params.length; i++) {
            var propval = params[i].split('=');
            if (propval[0] == name) {
                return propval[1] ? propval[1].replace(/\+/g, " ") : null;
            }
        }
        return false;
    };
    function executeTimely(func, interval, startAfter, executionCount) {
        interval = interval || 1000,
        setTimeout(function fn() {
            if (!func() && (executionCount == undefined || --executionCount > 0)) {
                setTimeout(fn, interval);
            }
        }, startAfter);
    };
    function isScrollBarPresent() {
        return $("body").height() > $(window).height();
    };
    //keysToBeIngnored:- pass array of keys
    function areObjectsEqual(obj1, obj2, keysToBeIgnored) {
        return JSON.stringify(_comparable(obj1, keysToBeIgnored)) === JSON.stringify(_comparable(obj2, keysToBeIgnored));
    }
    function getBaseUrl() {
        return window.location ? window.location.origin : undefined;
    }
    function _comparable(obj, keysToBeIgnored) {
        if (toString.call(obj) !== '[object Object]' || !obj){
            return obj;
        }
        return Object.keys(obj).sort().reduce(function (c, key) {
            if(!_isElementExistsInArray(key, keysToBeIgnored)) {
                c[key] = _comparable(obj[key]);
            }
            return c;
        }, {})
    }
    function _isElementExistsInArray(element, array) {
        return array.some(function (key) {
            return key === element;
        });
    }
    function replaceOrInsertInQs(key, value, qs) {
        qs = qs || location.search;
        if (qs.charAt(0) === '?') {
            qs = qs.slice(1);
        }
        var regex = new RegExp("&?" + key + "=[^&]*", "ig");
        qs = qs.replace(regex, "");
        if (qs) {
            qs = qs.charAt(0) === '&' ? qs.slice(1) : qs;
            qs = "?" + qs + "&" + key + "=" + value;
        }
        else {
            qs = "?" + key + "=" + value;
        }
        return qs;
    }
    return {
        getFilterFromQS: getFilterFromQS,
        getBulkValuesFromQs: getBulkValuesFromQs,
        removeFilterFromQS: removeFilterFromQS,
        isInteger: isInteger,
        findArrayElement: findArrayElement,
        findAndRemove: findAndRemove,
        numberWithCommas: numberWithCommas,
        apiRequestHandler: apiRequestHandler,
        addToHistory: addToHistory,
        formatSpecData: formatSpecData,
        getFromLocalStorage: getFromLocalStorage,
        executeTimely: executeTimely,
        isScrollBarPresent: isScrollBarPresent,
        areObjectsEqual: areObjectsEqual,
        getBaseUrl: getBaseUrl,
        replaceOrInsertInQs: replaceOrInsertInQs
    }
})();


var searchTracking = (function () {
    function triggerTracking(currentListing, listingsCount, act, isTrackQs, qs) {
        if (act === 'SearchListing') {
            var trackingParams = _getTrackingParams(currentListing, listingsCount);
            var label = _prepareLabel(trackingParams);
            if (label && cwTracking) {
                if (qs) {
                    cwTracking.trackCustomDataWithQs('UsedCars', act, label, qs);
                }
                else {
                    cwTracking.trackCustomData('UsedCars', act, label, isTrackQs);
                }
            }
        }
    };

    var _getTrackingParams = function (currentListing, listingsCount) {
        var trackingParams = {};
        trackingParams['cid'] = searchTracking.listingsTrackingCategoryEnum.Impression;
        trackingParams['pid'] = currentListing.getAttribute('profileid');
        trackingParams['rkAbs'] = currentListing.getAttribute('rankAbs');
        trackingParams['isPremium'] = currentListing.getAttribute('isPremium') === 'true' ? 1 : 0;
        trackingParams['isTopRatedSeller'] = currentListing.getAttribute('isTopRatedSeller') === 'true' ? 1 : 0;
        trackingParams['isCertified'] = currentListing.getAttribute('isCertified') === 'true' ? 1 : 0;
        trackingParams['isNearbyCityListing'] = currentListing.getAttribute('isNearbyCityListing') === 'true' ? 1 : 0;
        trackingParams['cnt'] = listingsCount;
        trackingParams['slotId'] = currentListing.dataset.slotId;
        trackingParams['ctePackageId'] = currentListing.dataset.ctePackageId;
        return trackingParams;
    }

    function _prepareLabel(trackingparam) {
        var label = '';
        if (trackingparam && typeof (trackingparam) == 'object') {
            for (var property in trackingparam) {
                label = label + property + '=' + trackingparam[property] + '|';
            }
        }
        return label;
    };

    var listingsTrackingCategoryEnum = {
        Impression: 1,
        DetailView: 2,
        PhotoView: 3,
        BtnSellerView: 4,
        Response: 5,
        SimilarCarsView: 6,
    };

    return {
        listingsTrackingCategoryEnum: listingsTrackingCategoryEnum,
        triggerTracking: triggerTracking
    }
})();
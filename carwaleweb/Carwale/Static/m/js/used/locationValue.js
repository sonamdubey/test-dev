var locationValue = (function () {
    var _CustLatitudeCookieName = "_CustLatitude";
    var _CustLongitudeCookieName = "_CustLongitude";
    var _carsNearMeCities = [2]; // currently show area only for bangalore(cityid : 2)

    function getCityValue() {
        var cityValObj = {"cityId": 0, "cityName": ""}
        var $citySelection = $("#citySelection");// This element is filter pop up's city box
        cityValObj.cityId = parseInt($citySelection.attr("data-cityid")) || 0;
        cityValObj.cityName = $citySelection.attr("data-cityname");
        if (cityValObj.cityId === 0 && !filterValues.isApplyFilterClicked()) {

            if (typeof globalCity !== 'undefined' && globalCity.isGlobalCityPresent()) {
                $citySelection.attr("cityid", globalCity.getGlobalCityId());
                $citySelection.find("div.selected-text").text(globalCity.getGlobalCityName());
                cityValObj.cityId = globalCity.getGlobalCityId();
            }
        }
        return cityValObj;
    }

    function getAreaValue() {
        var areaValObj = { "areaId": 0, "areaName": "" };
        var $citySelection = $("#citySelection");
        areaValObj.areaId = parseInt($citySelection.attr("data-areaid")) || 0;
        areaValObj.areaName = $citySelection.attr("data-areaname");
        return areaValObj;
    }

    function getLatLongValue() {
        var latLongValObj = { "latitude": -100, "longitude": -200 };
        latLongValObj.latitude = cookie.getCookie(_CustLatitudeCookieName) || -100;
        latLongValObj.longitude = cookie.getCookie(_CustLongitudeCookieName) || -200;
        return latLongValObj;
    }

    function getCarsNearMeCities() {
        return _carsNearMeCities;
    }
    return {
        getCityValue: getCityValue,
        getAreaValue: getAreaValue,
        getLatLongValue: getLatLongValue,
        getCarsNearMeCities: getCarsNearMeCities
    }
})();

var locationStorage = (function () {
    var _locationSessionStorageKeyName = "searchCustomerLocation";
    var locationIngnoredKeyForEquality = ["userConfirmed", "cityMaskingName"];
    var locationSessionStorageKeys = {
        "cityId": "cityId",
        "cityName": "cityName",
        "areaId": "areaId",
        "areaName": "areaName",
        "longitude": "longitude",
        "latitude": "latitude",
        "pincode": "pincode"
    }

    function setLocationSessionStorage(value) {
        if (typeof clientCache != 'undefined') {
            if (toString.call(value) !== "[object Object]") {
                return false;
            }
            var mergedObj = Object.assign({}, getLocationSessionStorage(), value);
            clientCache.set(_locationSessionStorageKeyName, mergedObj, true);
            return true;
        }
        return false;
    }

    function getLocationSessionStorage() {
        var location = {};
        if (typeof clientCache != 'undefined') {
            location = clientCache.get(_locationSessionStorageKeyName, true);
        }
        return location;
    }

    function getLocationObjFromStore(locationStore) {
        var location = {};
        location[locationSessionStorageKeys.cityName] = locationStore.location.cityName;
        location[locationSessionStorageKeys.cityId] = locationStore.location.cityId;
        location[locationSessionStorageKeys.areaName] = locationStore.location.areaName;
        location[locationSessionStorageKeys.areaId] = locationStore.location.areaId;
        location[locationSessionStorageKeys.latitude] = locationStore.location.latitude;
        location[locationSessionStorageKeys.longitude] = locationStore.location.longitude;
        location[locationSessionStorageKeys.pincode] = locationStore.location.pincode;
        return location;
    }

    return {
        locationSessionStorageKeys: locationSessionStorageKeys,
        setLocationSessionStorage: setLocationSessionStorage,
        getLocationSessionStorage: getLocationSessionStorage,
        locationIngnoredKeyForEquality: locationIngnoredKeyForEquality,
        getLocationObjFromStore: getLocationObjFromStore
    }
})();
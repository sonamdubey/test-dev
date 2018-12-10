var globalCity = (function () {
    var _cityIdMaster = "_CustCityIdMaster";
    var _cityNameMaster = "_CustCityMaster";

    if (typeof cookie != 'undefined' && cookie) {
        function isGlobalCityPresent() {
            return cookie.isCookiePresent(_cityIdMaster) && cookie.getCookie(_cityIdMaster) != "-1";
        };
        function getGlobalCityId() {
            if (cookie.isCookiePresent(_cityIdMaster)) {
                var cityid = cookie.getCookie(_cityIdMaster);
                return cityid == 1 ? 3000 : cityid;
            }
            else return -1;
        };
        function getGlobalCityName() {
            if (cookie.isCookiePresent(_cityNameMaster)) {
                var cityname = cookie.getCookie(_cityNameMaster);
                return cityname;
            }
        };
    }
    return {
        isGlobalCityPresent: isGlobalCityPresent,
        getGlobalCityId: getGlobalCityId,
        getGlobalCityName: getGlobalCityName
    }
})();
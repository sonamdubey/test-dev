function permanentCookieTime() {
    var now = new Date();
    var time = now.getTime();
    time += 1000 * 60 * 60 * 4320;
    now.setTime(time);
    return (now.toGMTString());
}

function SetCookieInDays(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    document.cookie = cookieName + "=" + cookieValue
                    + ";expires=" + expire.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
    if (cookieName == "_CustCityIdMaster" && Number(cookieValue) > 0) {
        var name = $.cookie('_CustCityMaster');
        var id = $.cookie('_CustCityIdMaster');
    }
}

function isCookieExists(cookiename) {
    var coockieVal = $.cookie(cookiename);
    if (coockieVal == undefined || coockieVal == null || coockieVal == '-1' || coockieVal == '-2')
        return false;
    return true;
}

function setCookie(CustCityMaster, CustCityIdMaster) {
    var now = new Date();
    var Time = now.getTime();
    Time += 1000 * 60 * 60 * 24 * 30;
    now.setTime(Time);
    document.cookie = '_CustCityMaster=' + CustCityMaster + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustCityIdMaster=' + CustCityIdMaster + '; expires = ' + now.toGMTString() + "; domain=" + defaultCookieDomain + '; path =/';
}

function setCookies(selectedCityId, selectedCityName, selectedZoneId, selectedZoneName) {
    document.cookie = '_CustCityId=' + selectedCityId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustCity=' + selectedCityName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    if (selectedZoneId != "")
        document.cookie = '_PQZoneId=' + selectedZoneId + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    else document.cookie = '_PQZoneId=' + "" + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';

    if (selectedZoneName != "")
        document.cookie = '_CustCity=' + selectedZoneName + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
}

function setCookieByNameValue(cname, cvalue) {
    var expires = "expires=" + permanentCookieTime();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; domain=" + defaultCookieDomain + "; path =/";
}

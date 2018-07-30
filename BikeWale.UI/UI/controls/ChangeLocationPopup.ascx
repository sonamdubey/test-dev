<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ChangeLocationPopup"  EnableViewState="false" %>

<% if (IsLocationChange)
   {%>

<!-- global Change city pop up code starts here -->
<div class="globalchangecity-popup rounded-corner2" id="globalchangecity-popup">
    <div class="globalchangecity-popup-data text-center font14">
        <div class="icon-outer-circle">
            <div class="icon-inner-circle">
                <span class="bwsprite cityPopup-icon"></span>
            </div>
        </div>
        <p class="margin-top15 margin-bottom5">You had chosen your location as <%= CookieCityName %>.</p>
        <p class="margin-bottom15">However, you are viewing information related to <span class="font16 text-bold"><%= UrlCityName %></span>.</p>
        <a href="javascript:void(0)" class="btn btn-grey btn-md font14 city-ignore margin-right15" id="changecity-ignore" rel="nofollow">Dismiss</a>
        <a href="javascript:void(0)" class="btn btn-orange font14 city-accept text-truncate" id="changecity-accept" rel="nofollow">Change to <%= UrlCityName %></a>
    </div>
    <div class="clear"></div>
</div>

<script type="text/javascript"> 
    docReady(function () {
        var locationChangePopup = (function () {
            try {
                var options = {
                    urlCityName: '<%: UrlCityName %>',
                    cookieCityName: '<%: CookieCityName %>',
                    urlCityId: parseInt('<%: UrlCityId %>', 10),
                    cookieCityId: parseInt('<%: CookieCityId %>', 10),
                    sessionKey: "bwchangecity",
                    locationChangeKey: "userchangedlocation",
                    ignoreEle: document.getElementById("changecity-ignore"),
                    acceptEle: document.getElementById("changecity-accept"),
                    popupEle: document.getElementById("globalchangecity-popup"),
                    blackoutEle: document.getElementsByClassName("changecity-blackout-window")[0],
                    navGlobalLocation: document.getElementsByClassName("gl-default-stage")[0],
                    bodyEle: document.getElementsByTagName("body")[0]
                };
                var ignoreCityChange = function () {
                    bwcache.set(options.sessionKey, "1", true);
                    hideChangeCityPopup();
                };
                var acceptCityChange = function () {
                    var cookieValue = options.urlCityId + '_' + options.urlCityName;
                    cookieValue = cookieValue.replace(/\s+/g, '-');
                    bwcache.remove("userchangedlocation", true);
                    setCookieInDays("location", cookieValue, 365);
                    document.getElementById("cityName").innerText = options.urlCityName;
                    hideChangeCityPopup();
                };
                var getHost = function () {
                    var host = document.domain;

                    if (host.match("bikewale.com$"))
                        host = ".bikewale.com";
                    return host;
                };
                var setCookieInDays = function (cookieName, cookieValue, nDays) {
                    var today = new Date();
                    var expire = new Date();
                    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
                    cookieValue = cookieValue.replace(/\s+/g, '-');
                    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent) || /Trident/.test(navigator.userAgent))
                        document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + '; path =/';
                    else
                        document.cookie = cookieName + "=" + cookieValue + ";expires=" + expire.toGMTString() + ';domain=' + getHost() + '; path =/';
                };

                var closeChangeCityPopup = function () {
                    ignoreCityChange();
                    hideChangeCityPopup();
                };

                var closeOnEsc = function (e) {
                    if (e.keyCode === 27)
                        closeChangeCityPopup();
                };

                var showChangeCityPopup = function () {
                    options.blackoutEle.style.display = "block";
                    options.popupEle.style.display = "block";
                    options.navGlobalLocation.style.pointerEvents = "none";
                    options.bodyEle.style.overflow = "hidden";
                };

                var hideChangeCityPopup = function () {
                    options.blackoutEle.style.display = "none";
                    options.popupEle.style.display = "none";
                    options.navGlobalLocation.style.pointerEvents = "auto";
                    options.bodyEle.style.overflow = "";
                };
                var CheckCookies = function () {
                    try {
                        var c = document.cookie.split('; ');
                        for (i = c.length - 1; i >= 0; i--) {
                            var C = c[i].split('=');
                            if (C[0] == "location") {
                                var cData = (String(C[1])).split('_');
                                options.cookieCityId = parseInt(cData[0]) || 0;
                                options.cookieCityName = cData[1] || "";
                                break;
                            }
                        }
                    } catch (e) {
                        console.warn(e);
                    }
                };

                var init = function () {
                    try {
                        CheckCookies();
                        bwcache.setOptions({ StorageScope: "", FallBack: true });
                        if (options.urlCityId > 0 && options.cookieCityId > 0 && (options.urlCityId != options.cookieCityId)) {
                            //surpress location chnage prompt for the session
                            if (!bwcache.get(options.sessionKey, true) && bwcache.get(options.locationChangeKey, true) != window.location.href) {
                                showChangeCityPopup();
                            }
                        }
                            //in case global city is not selected
                        else if (options.cookieCityId == 0) {
                            acceptCityChange();
                        }

                        options.ignoreEle.addEventListener("click", ignoreCityChange, false);
                        options.acceptEle.addEventListener("click", acceptCityChange, false);
                        document.addEventListener("keydown", closeOnEsc, false);
                        options.blackoutEle.addEventListener("click", closeChangeCityPopup, false);

                    } catch (e) {
                        console.warn(e);
                    }
                }();
            }
            catch (e) {
                console.log("Something went wrong with location change popup : " + e.message);
            }
        })();
    });
</script>

<% } %>
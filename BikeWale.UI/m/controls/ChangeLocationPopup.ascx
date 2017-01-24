﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ChangeLocationPopup"  EnableViewState="false" %>

<% if (IsLocationChange)
   {%>
<!-- global Change city pop up code starts here -->
<div class="globalchangecity-popup" id="globalchangecity-popup">
    <div class="globalchangecity-popup-data text-center font14">
        <p>You had chosen your location as <%= CookieCityName %>.</p>
        <p class="margin-bottom15">However, you are viewing information related to <span class="font16 text-bold"><%= UrlCityName %></span>.</p>
        <a href="javascript:void(0)" class="btn btn-white font14 city-ignore margin-right10" id="changecity-ignore" rel="nofollow">Dismiss</a>
        <a href="javascript:void(0)" class="btn btn-orange font14 city-accept text-truncate" id="changecity-accept" rel="nofollow">Change to <%= UrlCityName %></a>
    </div>
    <div class="clear"></div>
</div>

<script type="text/javascript">
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
                navGlobalLocation: document.getElementsByClassName("global-location")[0],
                bodyEle: document.getElementsByTagName("body")[0]
            };
            var ignoreCityChange = function () {
                bwcache.set(options.sessionKey, "1", true);
                hideChangeCityPopup();
            };
            var acceptCityChange = function () {
                try {
                    var today = new Date(), expire = today, cookieValue = options.urlCityId + '_' + options.urlCityName;
                    expire.setTime(today.getTime() + 3600000 * 24 * 365);
                    cookieValue = cookieValue.replace(/\s+/g, '-');
                    bwcache.remove("userchangedlocation", true);
                    document.cookie = "location=" + cookieValue + ";expires=" + expire.toGMTString() + ';domain=' + document.domain + '; path =/';
                    document.getElementById("globalCityPopUp").value = options.urlCityName;
                    hideChangeCityPopup();
                } catch (e) {
                    console.log(e.message);
                }
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
                try {

                    options.blackoutEle.style.display = "block";
                    options.popupEle.style.display = "block";
                    options.navGlobalLocation.style.pointerEvents = "none";
                    options.bodyEle.style.overflow = "hidden";
                    var child = document.getElementById("appBanner");   // remove app banner as already a prompt is shown
                    child.parentNode.removeChild(child);
                } catch (e) {
                    console.log(e.message);
                }
            };

            var hideChangeCityPopup = function () {
                try {
                    options.blackoutEle.style.display = "none";
                    options.popupEle.style.display = "none";
                    options.navGlobalLocation.style.pointerEvents = "auto";
                    options.bodyEle.style.overflow = "";
                } catch (e) {
                    console.log(e.message);
                }
            };

            var init = function () {
                try {
                    bwcache.setOptions({ StorageScope: "", FallBack: true });
                    if (options.urlCityId > 0 && options.cookieCityId > 0 && (options.urlCityId != options.cookieCityId)) {
                        if (!bwcache.get(options.sessionKey, true) && bwcache.get(options.locationChangeKey, true) != window.location.href)   //surpress location chnage prompt for the session
                        {
                            showChangeCityPopup();
                            window.history.pushState('locationChange', '', '');
                        }
                    }
                    else if (options.cookieCityId == 0)  //in case global city is not selected 
                    {
                        acceptCityChange();
                    }

                    options.ignoreEle.addEventListener("click", ignoreCityChange, false);
                    options.acceptEle.addEventListener("click", acceptCityChange, false);
                    document.addEventListener("keydown", closeOnEsc, false);
                    options.blackoutEle.addEventListener("click", closeChangeCityPopup, false);

                } catch (e) {

                }
            }();
        }
        catch (e) {
            console.log("Something went wrong with location change popup : " + e.message);
        };

        window.onpopstate = function (event) {
            if (event.state == 'locationChange') {
                closeChangeCityPopup();
            }
        }
    })();

</script>


<% } %>
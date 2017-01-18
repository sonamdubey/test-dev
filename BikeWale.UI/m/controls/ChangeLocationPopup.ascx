<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ChangeLocationPopup"  EnableViewState="false" %>

<% if (IsLocationChange)
   {%>

 <style type="text/css">
.globalchangecity-popup { display: none; width:300px; min-height:220px; background: #fff; margin: 0 auto; overflow-y: auto; position: fixed; top: 50px; right: 10px; z-index: 10; }
.globalchangecity-popup:before { content: ''; position: fixed; border-right: 8px solid transparent; border-bottom: 8px solid #fff; border-left: 8px solid transparent; position: fixed; top: 43px; right: 18px; }
.globalchangecity-popup-data { padding: 20px; }
.globalchangecity-popup-data .cityPopup-icon{ -webkit-transform: scale(0.4); -moz-transform: scale(0.4); -o-transform: scale(0.4); -ms-transform: scale(0.4); transform: scale(0.4);position: relative;left: -12px;top: -9px;background-color: #fff;}
.globalchangecity-popup-data .icon-outer-circle { width:40px; height:40px; margin:0 auto; background:#fff; border:1px solid #ccc; }
.globalchangecity-popup-data .icon-inner-circle { width:36px; height:36px; margin:1px auto; background:#fff; border:1px solid #666; }
.icon-outer-circle, .icon-inner-circle { -moz-border-radius: 50%; -webkit-border-radius: 50%; -o-border-radius: 50%; -ms-border-radius: 50%; border-radius: 50%;}
#changecity-ignore.btn { padding: 5px 12px; }
#changecity-accept.btn { padding: 5px 15px; }
</style>

 <!-- global Change city pop up code starts here -->
<div class="globalchangecity-popup" id="globalchangecity-popup">
    <div class="globalchangecity-popup-data text-center">
        <div class="icon-outer-circle margin-bottom20">
            <div class="icon-inner-circle">
                <span class="bwmsprite cityPopup-icon"></span>
            </div>
        </div>
        <p class="font14 margin-top10">You had chosen your location as</p>
        <p class="font16 margin-bottom5 text-bold" id="popup-cookiecity"><%= CookieCityName %></p>
         <p class="font14 margin-bottom15">However, you are viewing information related to <%= UrlCityName %>. Do you want to change your location to <%= UrlCityName %>?</p>
        <div>
            <a  class="btn btn-white font14 globalchangecity-close-btn city-ignore margin-right10" id="changecity-ignore">Dismiss</a>
            <a  class="btn btn-orange btn-md font14 globalchangecity-close-btn city-accept" id="changecity-accept">Change my location</a>
        </div>
    </div>
    <div class="clear"></div>
</div>

<script type="text/javascript" src="/src/bwcache.js"></script>
<script type="text/javascript">
    var locationChangePopup = (function () {
        try {
            var options = {
                urlCityName: '<%: UrlCityName %>',
                cookieCityName: '<%: CookieCityName %>',
                urlCityId: parseInt('<%: UrlCityId %>', 10),
                cookieCityId: parseInt('<%: CookieCityId %>', 10),
                sessionKey: "bwchangecity",
                ignoreEle: document.getElementById("changecity-ignore"),
                acceptEle: document.getElementById("changecity-accept"),
                popupEle: document.getElementById("globalchangecity-popup"),
                blackoutEle: document.getElementsByClassName("changecity-blackout-window")[0],
                navGlobalLocation: document.getElementsByClassName("global-location")[0],
                bodyEle: document.getElementsByTagName("body")[0]
            };
            var ignoreCityChange = function () {
                bwcache.set(options.sessionKey, "1", true);
                options.blackoutEle.style.display = "none";
                options.popupEle.style.display = "none";
            };
            var acceptCityChange = function () {
                var today = new Date(), expire = today, cookieValue = options.urlCityId + '_' + options.urlCityName;
                expire.setTime(today.getTime() + 3600000 * 24 * 365);
                cookieValue = cookieValue.replace(/\s+/g, '-');
                document.cookie = "location=" + cookieValue + ";expires=" + expire.toGMTString() + ';domain=' + document.domain + '; path =/';
                document.getElementById("globalCityPopUp").value = options.urlCityName;
                options.blackoutEle.style.display = "none";
                options.popupEle.style.display = "none";
            };

            var closeChangeCityPopup = function () {
                ignoreCityChange();
                options.blackoutEle.style.display = "none";
                options.blackoutEle.style.display = "none";
                options.navGlobalLocation.style.pointerEvents = "auto";
                options.bodyEle.style.overflow = "";
            };

            var closeOnEsc = function (e) {
                if (e.keyCode === 27)
                    closeChangeCityPopup();
            };
            var init = function () {
                try {
                    bwcache.setOptions({ StorageScope: "", FallBack: true });
                    if (options.urlCityId > 0 && (options.urlCityId != options.cookieCityId)) {
                        if (!bwcache.get(options.sessionKey, true))   //surpress location chnage prompt for the session
                        {
                            options.blackoutEle.style.display = "block";
                            options.popupEle.style.display = "block";
                            options.navGlobalLocation.style.pointerEvents = "none";
                            options.bodyEle.style.overflow = "hidden";
                            window.history.pushState('locationChange', '', '');
                        }
                    }
                    else if (cookieCityId == 0)  //in case global city is not selected 
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
        }

        window.onpopstate = function (event) {
            if (event.state == 'locationChange') {
                closeChangeCityPopup();
            }
        }
    })();

</script>


<% } %>
<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ChangeLocationPopup"  EnableViewState="false" %>

<% if (IsLocationChange)
   {%>

  <style type="text/css">
.globalchangecity-popup { width:360px; min-height:255px; background: #fff; margin: 0 auto; overflow-y: auto; position: fixed; top: 7.57%; right: 8%; z-index: 10; }
.globalchangecity-popup-data { padding: 20px 10px }
.globalchangecity-popup-data .cityPopup-icon{ transform : scale(0.7)}
.globalchangecity-popup-data .icon-outer-container { width:74px; height:74px; margin:0 auto; background:#fff; border:1px solid #ccc; }
.globalchangecity-popup-data .icon-inner-container { width:64px; height:64px; margin:4px auto; background:#fff; border:1px solid #666; }
  </style>

 <!-- global Change city pop up code starts here -->
<div class="globalchangecity-popup rounded-corner2 hide" id="globalchangecity-popup">
    <div class="globalchangecity-popup-data text-center">
        <div class="icon-outer-container rounded-corner50 margin-bottom20">
            <div class="icon-inner-container rounded-corner50">
                <span class="bwsprite cityPopup-icon margin-top5"></span>
            </div>
        </div>
        <p class="font14 margin-top10">You had chosen your location as</p>
        <p class="font16 margin-bottom5 text-bold" id="popup-cookiecity"><%= CookieCityName %></p>
         <p class="font14 margin-bottom15">However, you are viewing information related to <%= UrlCityName %>. Do you want to change your location to <%= UrlCityName %>?</p>
        <div>
            <a  class="btn btn-grey btn-md font14 globalchangecity-close-btn city-ignore" id="changecity-ignore">Dismiss</a>
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
                blackoutEle: document.getElementsByClassName('blackOut-window')[0]
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
                igonoreCityChange();
                options.blackoutEle.style.display = "none";
                options.blackoutEle.style.display = "none";
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
                        }
                    }
                    else if (cookieCityId == 0)  //in case global city is not selected 
                    {
                        acceptCityChange();
                    }


                    options.ignoreEle.addEventListener("click", ignoreCityChange, false);
                    options.acceptEle.addEventListener("click", acceptCityChange, false);
                    document.addEventListener("keydown", closeOnEsc, false);
                    options.blackoutEle.addEventListener("mouseup", closeOnEsc, false);

                } catch (e) {

                }
            }();
        }
        catch (e) {
            console.log("Something went wrong with location change popup : " + e.message);
        }
    })();

</script>


<% } %>
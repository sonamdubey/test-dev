<BW:MPopupWidget runat="server" id="PopupWidget" />

<% if(Ad_Bot_320x50){ %>
<section>            
    <div class="container">
        <div>
            <!-- #include file="/ads/Ad320x50_Bottom_mobile.aspx" -->
        </div>
    </div>
</section>
<% } %>

<div class="blackOut-window"></div>

<!-- #include file="/includes/Navigation_Mobile.aspx" -->
<div class="globalcity-popup bwm-fullscreen-popup hide" id="globalcity-popup">
    <div class="globalcity-popup-data text-center">
        <div class="globalcity-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
        <p class="font20 margin-bottom5 text-capitalize">Please tell us your city</p>
        <p class="text-light-grey margin-bottom5">This allows us to provide relevant content for you.</p>
        <div class="form-control-box">
            <div class="margin-bottom20 position-rel">
                <span class="position-abt pos-right15 pos-top15 bwmsprite cross-sm-dark-grey cur-pointer"></span>
                <input type="text" class="form-control padding-right30" name="globalCityPopUp" placeholder="Type to select city" id="globalCityPopUp" autocomplete="off">
                <span id="loaderMakeModel" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none;" ></span>
                <span class="bwmsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">No city found. Try a different search.</div>
            </div>
        </div>
        <div>
            <button id="btnGlobalCityPopup" class="btn btn-orange btn-full-width font18">Confirm city</button>
        </div>
    </div>
    <div class="clear"></div>
</div>

<!-- global-search-popup code starts here -->
<div id="global-search-popup" class="global-search-popup" style="display:none"> 
    <div class="form-control-box">
        <span class="back-arrow-box" id="gs-close">
            <span class="bwmsprite back-long-arrow-left"></span>
        </span>           
        <span class="cross-box hide" id="gs-text-clear">
            <span class="bwmsprite cross-md-dark-grey" ></span>
        </span>
        <input type="text" name="globalSearch" placeholder="Search" id="globalSearch" class="form-control padding-right30" autocomplete="off">
        <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none;right:35px;top:13px"></span>
        <ul id="errGlobalSearch" class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content hide">
            <li class="ui-menu-item" tabindex="-1">
                    <span class="text-bold">Oops! No suggestions found</span><br /> <span class="text-light-grey font12">Search by bike name e.g: Honda Activa</span>
            </li>
        </ul>
        <div id="new-global-search-section" class="bg-white hide">
            <div id="history-search">
                <div class="search-title">Recently Viewed</div>
                <ul id="new-global-recent-searches" class="recent-searches-dropdown bw-ui-menu"></ul>
            </div>
            <div id="trending-search">
                <div class="search-title">Trending Searches</div>
                <ul id="new-trending-bikes" class="recent-searches-dropdown bw-ui-menu"></ul>
            </div>
        </div>
    </div>
</div> <!-- global-search-popup code ends here -->

<footer class="bg-footer padding-top10 padding-bottom15"><!-- Footer section code starts here -->
    <div class="container">
                <div class="text-center">
                <p class="font13 text-white margin-bottom10">Popular Brands</p>
                <ul>
                    <li><a href="/m/honda-bikes/" title="Honda Bikes">Honda Bikes</a></li>
                    <li><a href="/m/hero-bikes/" title="Hero Bikes">Hero Bikes</a></li>
                    <li><a href="/m/suzuki-bikes/" title="Suzuki Bikes">Suzuki Bikes</a></li>
                </ul>
            </div>
        <div class="text-center padding-bottom15 border-solid-top padding-top20">
            <div class="grid-4">
                <a href="/m/" class="bwmsprite bw-footer-icon" title="Bikewale"></a>
            </div>
            <div class="grid-4">
                <a href="https://www.carwale.com/m/" target="_blank" rel="noopener" class="bwmsprite cw-footer-icon" title="CarWale"></a>
                <p class="cw-logo-label">ask the experts</p>
            </div>
            <div class="grid-4 omega">
                <a href="https://m.cartrade.com/" target="_blank" rel="noopener" class="bwmsprite ct-footer-icon" title="CarTrade"></a>
            </div>
            <div class="clear"></div>
        </div>
        <div class="border-solid-top text-center">
            <div class="margin-top15">
                <ul>
                    <li><a href="/m/contactus.aspx" rel="nofollow">Contact Us</a></li>
                    <li><a href="/m/advertisewithus.aspx" rel="nofollow">Advertise with Us</a></li>
                    
                </ul>
                <p class="font13 text-white margin-bottom10">Download Mobile App</p>
                    
                <div>
                    <a class="bwmsprite google-play-logo gplay-icon margin-right5" href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DBikeWaleMobileWebsite%26utm_medium%3DFooter%26utm_campaign%3DBikeWale%2520MobileWebsite%2520Footer" target="_blank" rel="noopener nofollow" >
                    </a>
                </div>
            </div>
            <div class="margin-top15 margin-bottom15">
                <a href="/?site=desktop" target="_blank" rel="noopener" class="text-white">View Desktop Version</a>
            </div>
                
        </div>
    </div>
    <div class="border-solid-top text-white font11 margin-top5 padding-top10 grid-12">
        <div class="grid-4 alpha text-left">&copy; BikeWale India</div>
        <div class="grid-8 omega text-right">
            <a href="https://www.bikewale.com/visitoragreement.aspx" class="text-white" rel="nofollow">Visitor Agreement </a>&
            <a href="https://www.bikewale.com/privacypolicy.aspx" class="text-white" rel="nofollow">Privacy Policy</a>
        </div>
    </div>
    <div class="clear"></div>
</footer> <!-- Ends here -->
<script type="text/javascript" defer src="<%= staticUrl  %>/src/bwcache.js?<%= staticFileVersion %>"></script>
<BW:LocationWidget runat="server" id="ctrlChangeLocation" />
<script type="text/javascript">var loadAsyncCss = function () { var a = document.getElementById("asynced-css"); if (a) { var b = document.createElement("div"); b.style.display='none', b.innerHTML = a.textContent, document.body.appendChild(b), a.parentElement.removeChild(a) } }, raf = window.requestAnimationFrame || window.mozRequestAnimationFrame || window.webkitRequestAnimationFrame || window.msRequestAnimationFrame; raf ? raf(function () { window.setTimeout(loadAsyncCss, 0) }) : window.addEventListener("load", loadAsyncCss);</script>

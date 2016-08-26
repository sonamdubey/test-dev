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
        <ul id="errGlobalSearch" style="width:100%;margin-left:0" class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content hide">
            <li class="ui-menu-item" tabindex="-1">
                    <span class="text-bold">Oops! No suggestions found</span><br /> <span class="text-light-grey font12">Search by bike name e.g: Honda Activa</span>
            </li>
        </ul>
    </div>
</div> <!-- global-search-popup code ends here -->

<footer class="bg-footer padding-top30 padding-bottom15"><!-- Footer section code starts here -->
    <div class="container">
        <div class="grid-12 text-center">
            <div>
                <a href="/m/" class="bwmsprite bw-logo"></a>              
            </div>
            <div class="margin-top15">
                <ul>
                    <li><a href="/m/contactus.aspx" rel="nofollow">Contact Us</a></li>
                    <li><a href="/m/advertisewithus.aspx" rel="nofollow">Advertise with Us</a></li>
                    <li><a target="_blank" href="http://www.carwale.com/m/">CarWale.com</a></li>
                </ul>
                <p class="font13 text-white margin-bottom10">Download Mobile App</p>
                    
                <div>
                    <a class="bwmsprite google-play-logo" href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DBikeWaleMobileWebsite%26utm_medium%3DFooter%26utm_campaign%3DBikeWale%2520MobileWebsite%2520Footer" target="_blank" class="bwsprite gplay-icon margin-right5" rel="nofollow">
                    </a>
                </div>
            </div>
            <div class="margin-top15 margin-bottom15">
                <a href="/?site=desktop" target="_blank" class="text-white">View Desktop Version</a>
            </div>
                
        </div>
        <div class="clear"></div>
    </div>
    <div class="border-solid-top text-white font11 margin-top5 padding-top10 grid-12">
        <div class="grid-4 alpha text-left opacity50">&copy; BikeWale India</div>
        <div class="grid-8 omega text-right">
            <a href="http://www.bikewale.com/visitoragreement.aspx" class="text-white" rel="nofollow">Visitor Agreement </a>&
            <a href="http://www.bikewale.com/privacypolicy.aspx" class="text-white" rel="nofollow">Privacy Policy</a>
        </div>
    </div>
    <div class="clear"></div>
</footer> <!-- Ends here -->
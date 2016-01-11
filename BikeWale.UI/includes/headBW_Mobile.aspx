	<div class="blackOut-window"></div>
<!-- #include file="/includes/Navigation_Mobile.aspx" -->
    <div class="globalcity-popup bwm-fullscreen-popup hide" id="globalcity-popup">
    	<div class="globalcity-popup-data text-center">
        	<div class="globalcity-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
        	<%--<div class="icon-outer-container rounded-corner50percent margin-bottom15">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="bwmsprite cityPopup-icon margin-top15"></span>
                </div>
        	</div>--%>
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

    <!---->
    <div id="appBanner" class="hide">
        <div class="banner-close-btn" id="btnCrossApp"></div>
        <div class="app-banner-img-container">
            <img src="http://img.aeplcdn.com/bikewaleimg/m/images/bw-app-phone.png" alt="BikeWale Android App" border="0" style="width:100%;" />
        </div>
        <div class="app-banner-text-container margin-top5">
            <h2 class="text-grey">India’s #1</h2>
            <p class="font12 text-bold text-grey margin-bottom5">Bike Research Destination</p>
            <a  id="btnInstallApp" href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source=BikeWaleWebsite&utm_medium=HeaderSlug&utm_campaign=MobileHeaderSlug" target="_blank" class="app-banner-btn-container">
                <span class="btn btn-orange font12 text-bold">Install our app</span>
            </a>
        </div>
        <div class="clear"></div>
    </div>
    <!---->

    <header>
    	<div class="header-fixed"> <!-- Fixed Header code starts here -->
        	<a href="/m/" id="bwheader-logo" class="bwmsprite bw-logo bw-lg-fixed-position"></a>
            <span class="ae-logo-border"></span>
            <a href="http://www.carwale.com/m/autoexpo2016/" class="ae-sprite ae-logo"></a>
            <div class="leftfloat">
                <span class="navbarBtn bwmsprite nav-icon margin-right10"></span>                
                <span id="book-back" class="white-back-arrow margin-right10 leftfloat hide"></span>
                <h2 class="headerTitle font18 text-white leftfloat hide">On-road price quote</h2>
            </div>
            <div class="rightfloat">
                <div class="global-search" id="global-search" style="display:none">
                    <span class="fa fa-search text-white" style="font-size:16px"></span>
                </div>
                <div class="global-location">
                    <span class="fa fa-map-marker"></span>
                </div>
                <a class="sort-btn rightfloat hide" id="sort-btn">
                    <span class="bwmsprite sort-icon"></span>
                </a>
            </div>
            <div class="clear"></div>
        </div> <!-- ends here -->
    	<div class="clear"></div>        
    </header>

<% if(Ad_320x50){ %>
<section>            
    <div class="container">
        <div>
            <!-- #include file="/ads/Ad320x50_mobile.aspx" -->
        </div>
    </div>
</section>
<% } %>

<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>

    <div id="appBanner" class="hide">
        <div class="banner-close-btn" id="btnCrossApp"></div>
        <div class="app-banner-img-container">
            <img src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/bw-app-phone.png" alt="BikeWale Android App" border="0" style="width:100%;" />
        </div>
        <div class="app-banner-text-container margin-top5">
            <p class="text-grey text-bold font20">India’s #1</p>
            <p class="font12 text-bold text-grey margin-bottom5">Bike Research Destination</p>
            <a id="btnInstallApp" href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source=BikeWaleWebsite&utm_medium=HeaderSlug&utm_campaign=MobileHeaderSlug" target="_blank" class="app-banner-btn-container">
                <span class="btn btn-orange font12 text-bold">Install our app</span>
            </a>
        </div>
        <div class="clear"></div>
    </div>
    <!---->

    <header>
        <div id="bw-header">
    	    <div class="header-fixed"> <!-- Fixed Header code starts here -->
        	    <a href="/m/" id="bwheader-logo" title="BikeWale" class="bwmsprite bw-logo bw-lg-fixed-position"></a>
           
                <div class="leftfloat">
                    <span class="navbarBtn bwmsprite nav-icon margin-right10"></span>
                    <span id="book-back" class="bwmsprite white-back-arrow margin-right10 leftfloat hide"></span>                   
                    <div class="headerTitle text-white leftfloat">
                        <p class="font12" id="headerText">Detailed Price Quote</p>
                        <p class="header-dealername" id="header-dealername"></p>
                    </div>                   
                </div>
                <div class="rightfloat">
                    <div class="global-search" id="global-search" style="display:none">
                        <span class="bwmsprite search-bold-icon"></span>
                    </div>
                    <div class="global-location">
                        <span class="bwmsprite map-loc-icon"></span>
                    </div>
                    <a href="javascript:void(0)" class="sort-btn rightfloat hide" id="sort-btn" rel="nofollow">
                        <span class="bwmsprite sort-icon"></span>
                    </a>
                </div>
                <div class="clear"></div>
            </div> <!-- ends here -->
    	    <div class="clear"></div>
        </div>
    </header>

<% if(Ad_320x50){ %>
    <section>            
        <div>
            <!-- #include file="/ads/Ad320x50_mobile.aspx" -->
        </div>
    </section>
<% } %>
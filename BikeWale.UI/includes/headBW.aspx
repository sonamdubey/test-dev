<%@ Register Src="~/controls/LoginControlNew.ascx" TagPrefix="BW" TagName="Login" %>
<%@ Register Src="~/controls/LoginStatusNew.ascx" TagPrefix="BW" TagName="LoginStatus" %>
	<div class="blackOut-window"></div>
    <!-- #include file="/includes/Navigation.aspx" -->
    <BW:Login ID="ctrlLogin" runat="server" />    
    <div class="globalcity-popup rounded-corner2 hide" id="globalcity-popup"><!-- global city pop up code starts here -->
    	<div class="globalcity-popup-data text-center">
        	<div class="globalcity-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="cityPopup-box rounded-corner50 margin-bottom20">
            	<span class="bwsprite cityPopup-icon margin-top10"></span>
            </div>
            <p class="font20 margin-bottom15">Please tell us your city</p>
            <p class="text-light-grey margin-bottom15">This allows us to provide relevant content for you.</p>
            <div class="form-control-box globalcity-input-box">
                <div class="margin-bottom20">
                	<span class="position-abt pos-right15 pos-top15 cwmsprite cross-sm-dark-grey cur-pointer"></span>
                    <input type="text" class="form-control padding-right30" name="globalCityPopUp" placeholder="Type to select city" id="globalCityPopUp">
                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none"></span>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter your city</div>
                </div>
            </div>
            <div>
            	<a id="btnGlobalCityPopup" class="btn btn-orange font18">Confirm city</a>
            </div>
        </div>
        <div class="clear"></div>
    </div>
    <div id="header" class='<%= isHeaderFix ? "header-fixed": "header-not-fixed" %>'> <!-- Fixed Header code starts here -->
        <div class="leftfloat">
            <span class="navbarBtn bwsprite nav-icon margin-right25"></span>
            <a href="/" id="bwheader-logo" class="bwsprite bw-logo"></a>
        </div>
        <div class="rightfloat">
            <div class="global-search position-rel ">
                <span class="bwsprite search-icon-grey" id="btnGlobalSearch"></span>
                <input type="text" name="globalSearch" placeholder="Search" id="globalSearch" class="blur ui-autocomplete-input" autocomplete="off">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top10 text-black" style="display:none;right:35px;"></span>
            </div>
            <div class="global-location">
                <div class="gl-default-stage">
                	<div id="globalCity-text">
                        <span class="cityName" id="cityName">Select City</span>
                        <span class="fa fa-map-marker margin-left10"></span>
                    </div>
                </div>            
            </div>
            <BW:LoginStatus ID="ctrlLoginStatus" runat="server" />
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div> <!-- ends here -->
    <div class="clear"></div>    
<% if(isAd970x90Shown){ %>
    <section class="bg-white <%= isHeaderFix ? "header-fixed-inner": "padding-top10" %>">
        <div class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <!-- #include file="/ads/Ad970x90.aspx" -->
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
<% } %>

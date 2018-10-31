<%@ Register Src="~/UI/controls/LoginStatusNew.ascx" TagPrefix="BW" TagName="LoginStatus" %>
<%@ Register Src="~/UI/controls/PopupWidget.ascx" TagPrefix="BW" TagName="PopupWidget" %>
<%@ Register Src="~/UI/controls/ChangeLocationPopup.ascx" TagPrefix="BW" TagName="LocationWidget" %>
    <div id="header" class='<%= isHeaderFix ? "header-fixed": "header-not-fixed" %> <%=  isTransparentHeader?" header-landing header-transparent":String.Empty   %>'> <!-- Fixed Header code starts here -->
        
		<div class="leftfloat">
            <span class="navbarBtn bwsprite nav-icon margin-right25"></span>
            <a href="/" id="bwheader-logo" class="bw-logo" title="BikeWale">BikeWale</a>
			</div>
		
        <div class="rightfloat">
            <div class="global-search" >
                <span class="bwsprite search-icon-grey" id="btnGlobalSearch" style="z-index:2"></span>
                <input type="text" name="globalSearch" placeholder="Search" id="globalSearch" class="blur ui-autocomplete-input" autocomplete="off">
                <span class="fa fa-spinner fa-spin position-abt  text-black" style="display:none;right:14px;top:7px;background:#fff"></span>
                 <ul id="errGlobalSearch" style="width:420px;position:absolute;top:30px;left:3px" class="ui-autocomplete ui-front ui-menu hide">
                    <li class="ui-menu-item" tabindex="-1">
                       <span class="text-bold">Oops! No suggestions found</span><br /> <span class="text-light-grey font12">Search by bike name e.g: Honda Activa</span>
                    </li>
                </ul>
                <div id="global-search-section" class="bg-white hide">
                    <div id="history-search">
                        <div class="search-title">Recently Viewed</div>
                        <ul id="global-recent-searches" class="recent-searches-dropdown bw-ui-menu"></ul>
                    </div>
                    <div id="trending-search">
                        <div class="search-title">Trending Searches</div>
                        <ul id="trending-bikes" class="recent-searches-dropdown bw-ui-menu"></ul>
                    </div>
                </div>
            </div>
            <div class="global-location">
                <div class="gl-default-stage">
                	<div id="globalCity-text">
                        <span class="cityName" id="cityName">Select City</span>
                        <span class="bwsprite global-map-marker margin-left10"></span>
                    </div>
                </div>
            </div>

            <BW:LoginStatus ID="ctrlLoginStatus" runat="server" />
            <div class="changecity-blackout-window"></div>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
		<!-- Doodle start-->
		<span class="doodle__container bw-ga" data-cat="Doodle" data-act="Doodle_Click" data-lab="Diwali">
			<span class="doodle__animation-block">
				<span class="doodle__hexagaon">
					<span class="doodle__left-cloud"></span>
					<span class="doodle__right-cloud"></span>
					<span class="doodle__flag"></span>
					<span class="doodle__star star-one"></span>
					<span class="doodle__star star-two"></span>
					<span class="doodle__star star-three"></span>
					<span class="doodle__star star-four"></span>
					<span class="doodle__star star-five"></span>
					<span class="doodle__firework firework-one"></span>
					<span class="doodle__firework firework-two"></span>
					<span class="doodle__firework firework-three"></span>
					<span class="doodle__lantern lantern-one"></span>
					<span class="doodle__lantern lantern-two"></span>
				</span>
			</span>
		</span>
		<!--Doodle end-->
    </div> <!-- ends here -->
    <div class="clear"></div>    
<% if(isAd970x90Shown){ %>
    <div class="bg-white ">
        <div class="container">
            <div class="grid-12">
                <div>
                    <!-- #include file="/UI/ads/Ad970x90.aspx" -->
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
<% } %>

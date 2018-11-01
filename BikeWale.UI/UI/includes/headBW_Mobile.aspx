<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/UI/m/controls/MPopupWidget.ascx" %>
<%@ Register Src="~/UI/m/controls/ChangeLocationPopup.ascx" TagPrefix="BW" TagName="LocationWidget" %>

    <header>
        <div id="bw-header">
    	    <div class="header-fixed"> <!-- Fixed Header code starts here -->
				<!-- Doodle start-->
				<span class="doodle__container bw-ga" data-cat="Doodle" data-act="Doodle_Click" data-lab="Diwali">
					<span class="doodle__animation-block">
						<span class="doodle__hexagaon">
							<span class="doodle__left-cloud"></span>
							<span class="doodle__right-cloud"></span>
							<span class="doodle__flag"></span>
							<span class="doodle__star doodle__star-one"></span>
							<span class="doodle__star doodle__star-two"></span>
							<span class="doodle__star doodle__star-three"></span>
							<span class="doodle__star doodle__star-four"></span>
							<span class="doodle__star doodle__star-five"></span>
							<span class="doodle__firework firework-one"></span>
							<span class="doodle__firework firework-two"></span>
							<span class="doodle__firework firework-three"></span>
							<span class="doodle__lantern lantern-one"></span>
							<span class="doodle__lantern lantern-two"></span>
						</span>
					</span>
				</span>
				<!--Doodle end-->
        	    <a href="/m/" id="bwheader-logo" title="BikeWale" class="bw-logo bw-lg-fixed-position">BikeWale</a>

                <div class="leftfloat">
                    <span id="navbarBtn" class="navbarBtn nav-icon margin-right10"></span>
                    <span id="book-back" class="bwmsprite white-back-arrow margin-right10 leftfloat hide"></span>                   
                    <div class="headerTitle text-white leftfloat">
                        <p class="font12" id="headerText">Detailed Price Quote</p>
                        <p class="header-dealername" id="header-dealername"></p>
                    </div>                   
                </div>
               
                    
                    
                <div class="rightfloat">
                     <% if(ShowSellBikeLink){ %>
                    <a href="/m/used/sell/" title="Sell your bike" class="btn header-sell-btn">Sell bike</a> 
                      <%} %>                 
                    <div class="global-search" id="global-search">
                        <span class="global-search-icon"></span>
                    </div>
                    <div class="global-location">
                        <span class="map-loc-icon"></span>
                    </div>
                    <a href="javascript:void(0)" class="sort-btn rightfloat hide" id="sort-btn" rel="nofollow">
                        <span class="bwmsprite sort-icon"></span>
                    </a>
                </div>
                <div class="changecity-blackout-window"></div>
                <div class="clear"></div>
            </div> <!-- ends here -->
    	    <div class="clear"></div>
        </div>
    </header>

<% if(Ad_320x50){ %>
    <section>            
        <div>
            <!-- #include file="/UI/ads/Ad320x50_mobile.aspx" -->
        </div>
    </section>
<% } %>
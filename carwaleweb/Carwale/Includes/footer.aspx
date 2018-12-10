
 <% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderAction("Footer", "CarWidgets"); %>

<!-- Ask Expert Section Start -->
<script language="c#" runat="server">
    private bool ShowAskTheExpert = true;
</script>


<%if (ShowAskTheExpert)
  {%>
<div id="askexpertsidebanner" class="ask-expert text-white text-center padding-bottom5 padding-top5 padding-left10 padding-right15 rounded-corner-no-right box-shadow cur-pointer">
    <span class="cwsprite cross-sm-white position-abt pos-right5 pos-top10 expert-close-btn"></span>
    <div class="ask-expert-content inline-block">
        <span class="ask-expert-title font14 block">Ask the experts</span>
        <span class="toll-free-no block font18">1800 2090 230</span>
        <span id="ATEConditional" class="toll-free-text font10 block">Mon-Fri &amp; Sun (10 AM - 7 PM) <br /> Sat (10 AM - 5:30 PM)</span>
        <span class="toll-free-text font12 block">Toll free</span>
    </div>
</div>
 <!-- Advantage City PopUp starts here -->
    <div class="deals-city-popup advantage-city-popup rounded-corner2 hide" >
        <!-- global city pop up code starts here -->
        <div class="globalcity-popup-data text-center">
            <div class="city-close-button position-abt pos-top10 pos-right10 cwsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="cityPopup-box rounded-corner50 margin-bottom25">
                <span class="cwsprite cityPopup-icon margin-top10"></span>
            </div>
            <p class="font20 margin-bottom10 text-capitalize text-black">Select your city to avail offers</p>
            <p class="text-light-grey margin-bottom15">Currently available only in</p>
            <div class="city-select-btn margin-top20">
               <select id="advantage-city-select" class="form-control input-xs selected-text selectCity margin-top5">
                 <option value="-1"  selected="selected">Select City</option> 
               </select>
            </div>
        </div>
        <div class="clear"></div>
    </div>
   <!-- Advantage City PopUp ends here -->
<%}%>


<!-- Ask Expert Section End -->

<!-- Google Code for Remarketing Tag -->
<script type="text/javascript" src="//www.googleadservices.com/pagead/conversion_async.js"></script>

<!-- Google Code for Remarketing Tag -->
    <script type="text/javascript">
        /* <![CDATA[ */
        var google_conversion_id = 999894061;
        var google_custom_params = window.google_tag_params;
        var google_remarketing_only = true;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
        <div style="display:inline;">
            <img height="1" width="1" style="border-style:none;" alt="" src="//googleads.g.doubleclick.net/pagead/viewthroughconversion/999894061/?guid=ON&amp;script=0" />
        </div>
    </noscript>
    <!-- Google Code for Remarketing Tag Ends -->
<!-- Ends here -->


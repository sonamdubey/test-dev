<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Carwale.UI.Controls.GetUserCity" %>
<!--Citypopup User control code -->       
<div class="hide" id="findCitypopup"></div>
<div class="hide cw-new-pop-style" id="findCityContentNew" >
    <div class="city-popup">
        <div class="city-content" style="padding-bottom:60px;">
            <div class="city-content-close">
                <span class="cw-sprite close-icon-cw-new-pop"></span>
            </div>
            <div>
                <h2>Please tell us your city</h2>
                <div class="font14 dark-gray-text">Knowing your city allows us to provide relevant content for you</div>
                <div class="clear"></div>
            </div>
            <div class="margin-top15">
            	<div style="vertical-align:top;">
                	<div>
                    	<div class="leftfloat">
                        <span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span>
                        <span id="autoDetectedCity" onclick='dataLayer.push({event: "master-city-popup",cat: "mastercitypopup",act: "click-on-geolocated",lab: $("#autoCity").text().toLowerCase()})' class="rounded-corner5"><span id="autoCity"></span>&nbsp;&nbsp;&nbsp;<span id="autoText" class="font12"></span></span>
                        <input type="text" autocomplete="off" id="cwCitypopup" class="ui-autocomplete-input rounded-corner5 hide" placeholder="-- Type to Select Your City --">
                            
                        </div>
                        <span class="rightfloat"><!--<a href="#" class="cw-pop-up-btn rounded-corner5 ">Confirm City</a>-->
                            <button id="btnConfirmCity" class="cw-pop-up-btn rounded-corner5">Confirm City</button>
                        </span>
                        <div class="clear"></div>
                        <div id="cityIPtoLoc" class="hide"><span class="font11">City auto-detected. Confirm selection or change here if you wish.</span></div>
                        <p class="error font12 margin-top5 hide" id="message" style="display: none;">No city match.</p>
                        <p class="error font12 margin-top5 hide" id="errMessage">Please enter your city.</p>  
                    </div>
                </div>
<%--                <div class="margin-top10" id="userPopularCities">
                </div>--%>
            </div>   
            <div class="clear"></div>
         </div>
    </div>
</div>
    <script type="text/javascript">
        var CitySelectedbyIP = '<%= CitySelectedbyIp%>';
        var CityNameSelectedbyIp = '<%= CityNameSelectedbyIp%>';
    </script>
  
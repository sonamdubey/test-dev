<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="BookingListing.aspx.cs" Inherits="Bikewale.m.bikebooking.BookingListing" Trace="false"%>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!doctype html>
<html>
    <head>
        <%
            title = "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
            keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
            description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
            canonical = "http://www.bikewale.com";
            AdPath = "/1017752/Bikewale_Mobile_Homepage";
            AdId = "1398766000399";
         %>
        <!-- #include file="/includes/headscript_mobile.aspx" -->
        <script>ga_pg_id = '5';</script>
    </head>
    <body class="bg-light-grey">
        <!-- #include file="/includes/Navigation_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/new/bwm-bookinglisting.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" />
        <div class="blackOut-window"></div>
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
        <header>
    	    <div class="header-fixed"> <!-- Fixed Header code starts here -->
                <div class="leftfloat">
                    <span class="navbarBtn bwmsprite nav-icon margin-right5"></span>                
                    <span class="booking-listing-nav font16 text-white position-rel">Book your bike</span>
                </div>
                <div class="rightfloat">
                    <a class="global-search" id="global-search" style="display: none">
                        <span class="bwmsprite search-icon-grey"></span>
                    </a> 
                    <a class="filter-btn" id="filter-btn">
                        <span class="bwmsprite filter-icon"></span>
                    </a>
                    <a class="sort-btn" id="sort-btn">
                        <span class="bwmsprite sort-icon"></span>
                    </a>
                </div>
            
                <div class="clear"></div>
            </div> <!-- ends here -->
    	    <div class="clear"></div>
       
        </header>
    
        <section><!--  Used Search code starts here -->
            <div class="container">
                <div><!--  class="grid-12"-->
                    <div class="hide" id="sort-by-div">
                    	<div  class="filter-sort-div font14 bg-white">
                            <div sc="1" so="">
                                <a data-title="sort" class="price-sort position-rel">
                                	Price<span class="hide" so="0" class="sort-text"></span>
                                </a>
                            </div>
                            <div sc="" class="border-solid-left">
                                <a data-title="sort" class="position-rel">
                                	Popularity 
                                </a>
                            </div>
                            <div sc="2" class="border-solid-left">
                                <a data-title="sort" class="position-rel">
                                	Mileage 
                                </a>
                            </div>
                        </div>
                    </div>
                    <div id="listingCountContainer" class="font14 padding-top20 padding-bottom20 text-center">
                        <span><span id="bikecount" class="font18"></span><span class="text-light-grey"> in</span></span><br />
                        <span class="change-city-area-target"><span id="Userlocation"></span><span class="margin-left5 bwmsprite loc-change-blue-icon icon-adjustment"></span></span>
                        <div id="listingLocationPopup" class="font13 bwm-fullscreen-popup">
                            <div class="bwmsprite location-popup-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
                            <div id="listingPopupHeading">
                                <p class="font18 margin-top10 margin-bottom5 text-capitalize">Please Tell Us Your Location</p>
                                <p class="text-light-grey margin-bottom5">See Bikes Available for Booking in Your Area!</p>
                                <div id="listingCitySelection" class="form-control text-left input-sm position-rel margin-bottom10">
                                    <div class="selected-city">Select City</div>
                                    <span class="fa fa-angle-right position-abt pos-top10 pos-right10"></span>
                                </div>

                                <div id="listingAreaSelection" class="form-control text-left input-sm position-rel margin-bottom10">
                                    <div class="selected-area">Select Area</div>
                                    <span class="fa fa-angle-right position-abt pos-top10 pos-right10"></span>
                                </div>
                                
                                <div class="margin-top20 text-center">
                                    <a id="btnBookingListingPopup" class="btn btn-orange btn-full-width font18">Show bikes</a>
                                </div>
                            </div>

                            <div id="listingPopupContent" class="bwm-city-area-popup-wrapper">
                                <div class="bw-city-popup-box bwm-city-area-box city-list-container form-control-box text-left">
                                    <div class="user-input-box">
                                        <span class="back-arrow-box">
                                            <span class="bwmsprite back-long-arrow-left"></span>
                                        </span>
                                        <input class="form-control" type="text" id="listingPopupCityInput" placeholder="Select City" />
                                    </div>
                                    <ul id="listingPopupCityList" class="margin-top40" >
                                        <asp:Repeater ID="rptCities" runat="server">
                                            <ItemTemplate>
                                                <li cityId="<%# DataBinder.Eval(Container.DataItem, "CityId") %>" >
                                                    <%# DataBinder.Eval(Container.DataItem, "CityName") %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>

                                <div class="bw-area-popup-box bwm-city-area-box area-list-container form-control-box text-left">
                                    <div class="user-input-box">
                                        <span class="back-arrow-box">
                                            <span class="bwmsprite back-long-arrow-left"></span>
                                        </span>
                                        <input class="form-control" type="text" id="listingPopupAreaInput" placeholder="Select Area" data-bind="attr: { value: (SelectedArea() != undefined) ? SelectedArea().areaName : '' }" />
                                    </div>
                                    <ul id="listingPopupAreaList" class="margin-top40">
                                        <asp:Repeater ID="rptAreas" runat="server">
                                            <ItemTemplate>
                                                <li areaId="<%# DataBinder.Eval(Container.DataItem, "AreaId") %>">
                                                    <%# DataBinder.Eval(Container.DataItem, "AreaName") %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div id="searchBikeList" class="bike-search">
                        <div id="divSearchResult" data-bind="template: { name: 'listingTemp', foreach: bikes }" class="search-bike-container">

                        </div>
                        <div style="text-align:center;">
                            <div id="nobike" class="hide">
                                <img src="/images/no_result_m.png" />
                            </div>
                            <div id="loading" class="hide">
                                <img src="/images/search-loading.gif" />
                            </div>
                        </div>
                    </div>
                    <script type="text/html" id="listingTemp">
                        <div class="search-bike-item">
                            <div class="front">
                                <div class="contentWrapper">
                                    <!--<div class="position-abt pos-right10 pos-top10 infoBtn bwmsprite alert-circle-icon"></div>-->
                                    <div class="imageWrapper margin-top10">
                                        
                                        <div data-bind="visible : offers().length > 0"  class="offers-tag-wrapper position-abt">
                                            <span><span data-bind="text : offers().length"></span> offers available</span>
                                            <span class="offers-left-tag"></span>
                                        </div>
                                        <a data-bind="attr: { href: '/m/' + makeEntity.maskingName() + '-bikes/' + modelEntity.maskingName() + '/'+ versionEntity.versionId() + '/' }, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Model_Click', 'lab': modelEntity.modelName() }); return true; }">
                                            <img class="lazy" data-bind="attr: { title: bikeName(), alt: bikeName(), src: '' }, lazyload: hostUrl() + '/310X174/' + originalImagePath() ">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom10">
                                                <h3><a data-bind="attr: { href: '/m/' + makeEntity.maskingName() + '-bikes/' + modelEntity.maskingName() + '/'+ versionEntity.versionId() + '/', title: bikeName }, text: bikeName, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Model_Click', 'lab': modelEntity.modelName }); return true; }"></a></h3>
                                            </div>
                                        <p class="font14 text-light-grey">BikeWale on-road price</p>
                                        <div class="margin-top10 text-light-grey" data-bind="visible: discount() > 0">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font13 margin-right5 text-line-through" data-bind="CurrencyText: onRoadPrice()"></span>
                                            <span>
                                                (<span class="text-red">
                                                    <span class="fa fa-rupee"></span>
                                                    <span class="font13 margin-right5" data-bind="CurrencyText: discount()"></span> Off
                                                </span>)
                                            </span>
                                        </div>
                                        <div class="font18 text-grey margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font20" data-bind="CurrencyText: discountedPrice()"></span>
                                        </div>
                                        <div class="font14 margin-top5 margin-bottom5" data-bind="visible: offers().length > 0">
                                            <span class="text-default margin-right5" data-bind="text: offers().length + ' offers available'"></span>
                                            <span class="text-link view-offers-target">view offers</span>
                                        </div>
                                        <div id="offersPopup" class="bwm-fullscreen-popup text-center">
                                            <div class="offers-popup-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
                                            <div class="margin-top20 icon-outer-container rounded-corner50percent">
                                                <div class="icon-inner-container rounded-corner50percent">
                                                    <span class="bwmsprite offers-box-icon margin-top20"></span>
                                                </div>
                                            </div>
                                            <p class="font18 margin-top25 margin-bottom20 text-default">Available offers on this bike</p>
                                            <ul class="offers-list-ul" data-bind="foreach : offers()">
                                                <li data-bind="text : offerText()"></li>
                                            </ul>
                                            <input class="book-now-popup-btn margin-top30 btn btn-orange font16" data-bind="click: function () { registerPQ($data); }" value="Book now"/>
                                        </div>
                                        <p class="font14 text-light-grey">Now book your bike online at <span class="text-default text-bold"><span class="margin-left5 fa fa-rupee"></span> <span class="font15" data-bind="text: bookingAmount"></span></span></p>
                                        <input class="margin-top10 btn btn-orange btn-full-width margin-top10" data-bind="click: function () { registerPQ($data); }" value="Book now" />
                                    </div>
                                    </div>
                                </div>
                            <div class="border-top1 margin-left20 margin-right20 padding-top10 clear"></div>
                        </div>
                    </script>
                </div>
                <div class="clear"></div>
            </div>
    
        </section><!-- Used Search code  Ends here -->
   
            <!--Filters starts here-->
                <div id="filter-div" class="popup_layer hide">
                    <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                        <div id="hidePopup" class="filterBackArrow" popupname="filterpopup" onclick="CloseWindow(this)">
                            <!--<span class="bwmsprite back-long-arrow-left-white"></span>-->
                            <span class="fa fa-angle-left"></span>
                        </div>
                        <div class="floatleft cw-m-sprite city-back-btn" id="back-btn"></div>
                        <div class="filterTitle">Filters</div>
                        <div id="btnReset" class="resetrTitle">Reset</div>
                        <div class="clear"></div>
                    </div>
                    <div class="content-inner-block-20 margin-bottom40 clearfix">
                	    <!--Brand section starts here-->
                        <div class="dropdown form-control-box margin-bottom20"> 
                            <h3 class="text-black margin-bottom10">Brand</h3>
                            <div class="form-control">
                              <span class="hida">Brand</span>
                              <div class="multiSel"></div>
                            </div>
                          
                            <div name="makeIds" class="multiSelect">
                                <ul>
                                    <li class="unchecked" filterId="2"><span>Aprilia</span></li>
                                    <li class="unchecked" filterId="1"><span>Bajaj</span></li>
                                    <li class="unchecked" filterId="40"><span>Benelli</span></li>
                                    <li class="unchecked" filterId="3"><span>BMW</span></li>
                                    <li class="unchecked" filterId="4"><span>Ducati</span></li>
                                    <li class="unchecked" filterId="5"><span>Harley Davidson</span></li>
                                    <li class="unchecked" filterId="39"><span>Hero Electric</span></li>
                                    <li class="unchecked" filterId="6"><span>Hero</span></li>
                                    <li class="unchecked" filterId="7"><span>Honda</span></li>
                                    <li class="unchecked" filterId="8"><span>Hyosung</span></li>
                                    <li class="unchecked" filterId="34"><span>Indian</span></li>
                                    <li class="unchecked" filterId="17"><span>Kawasaki</span></li>
                                    <li class="unchecked" filterId="9"><span>KTM</span></li>
                                    <li class="unchecked" filterId="19"><span>LML</span></li>
                                    <li class="unchecked" filterId="10"><span>Mahindra</span></li>
                                    <li class="unchecked" filterId="20"><span>Moto Guzzi</span></li>
                                    <li class="unchecked" filterId="41"><span>MV Agusta</span></li>
                                    <li class="unchecked" filterId="11"><span>Royal Enfield</span></li>
                                    <li class="unchecked" filterId="12"><span>Suzuki</span></li>
                                    <li class="unchecked" filterId="22"><span>Triumph</span></li>
                                    <li class="unchecked" filterId="15"><span>TVS</span></li>
                                    <li class="unchecked" filterId="16"><span>Vespa</span></li>
                                    <li class="unchecked" filterId="13"><span>Yamaha</span></li>
                                    <li class="unchecked" filterId="14"><span>Yo</span></li>
                            </ul>
                            </div>
                        </div>
                        <!--Brand section starts here-->

                        <!--Budget section starts here-->
                        <div class="margin-bottom20">
                            <h3 class="text-black margin-bottom10">Budget</h3>
                            <div class="slider-box content-box-shadow content-inner-block-10">
                                <div class="leftfloat">
                                    <span id="rangeAmount">0 - Any value</span>
                                </div>
                                <div class="clear"></div>
                                <div name="budget" id="mSlider-range" class="bwm-sliders"></div>
                            </div>
                        </div>
                        <!--Budget section ends here-->
                    
                        <!--Displacement section starts here-->
                        <div class="dropdown form-control-box margin-bottom20"> 
                            <h3 class="text-black margin-bottom10">Displacement</h3>
                            <div class="form-control">
                              <span class="hida">Displacement</span>    
                              <div class="multiSel"></div>  
                            </div>
                          
                            <div name="displacement" class="multiSelect">
                                <ul>
                                    <li class="unchecked" filterId="1"><span>Up to 110 cc</span></li>
                                    <li class="unchecked" filterId="2"><span>110-150 cc</span></li>
                                    <li class="unchecked" filterId="3"><span>150-200 cc</span></li>
                                    <li class="unchecked" filterId="4"><span>200-250 cc</span></li>
                                    <li class="unchecked" filterId="5"><span>250-500 cc</span></li>
                                    <li class="unchecked" filterId="6"><span>500 cc and more</span></li>
                                </ul>
                            </div>
                        </div>
                        <!--Displacement section starts here-->
                    
                       <!--ride section starts here-->
                        <div class="dropdown form-control-box margin-bottom20"> 
                            <h3 class="text-black margin-bottom10">Ride style</h3>
                            <div class="form-control">
                                <span class="hide">Ride Style</span>
                                <span class="hida">Ride Style</span>    
                                <div class="multiSel"></div>  
                            </div>
                          
                            <div name="ridestyle" class="multiSelect">
                                <ul>
                                    <li class="unchecked" filterId="1"><span>Cruisers</span></li>
                                    <li class="unchecked" filterId="2"><span>Sports</span></li>
                                    <li class="unchecked" filterId="3"><span>Street</span></li>
                                    <li class="unchecked" filterId="5"><span>Scooters</span></li>
                                </ul>
                            </div>
                        </div>
                        <!--ride section starts here-->
                    
                    
                        <!--ride section starts here-->
                        <div class="form-control-box margin-bottom20 clearfix">
                    	    <h3 class="text-black margin-bottom10">Mileage</h3>
                            <div name="mileage" class="grid-12 mileage-box">
                        	    <div class="grid-3 content-inner-block-5">
                            	    <span filterid="1" class="form-control mileage">70+</span>
                                </div>
                                <div class="grid-3 content-inner-block-5">
                            	    <span filterid="2" class="form-control mileage">70-50</span>
                                </div>
                                <div class="grid-3 content-inner-block-5">
                            	    <span filterid="3" class="form-control mileage">50-30</span>
                                </div>
                                <div class="grid-3 content-inner-block-5">
                            	    <span filterid="4" class="form-control mileage">30-0</span>
                                </div>
                            </div>
                        </div>
                        <!--ride section starts here-->
                    
                         <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>ABS</h3>
                                </div>
                                <div name="AntiBreakingSystem" class="grid-7 omega">
                            	    <span filterid="1" class="form-control grid-6 checkOption">Yes</span>
                            	    <span filterid="0" class="form-control grid-6 checkOption">No</span>                                
                                </div>
                            </div>
                    
                         <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>Brakes</h3>
                                </div>
                                <div name="braketype" class="grid-7 omega">
                            	    <span filterid="1" class="form-control grid-6 checkOption">Disc</span>
                            	    <span filterid="0" class="form-control grid-6 checkOption">Drum</span>                                
                                </div>
                            </div>
                        
                             <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>Wheels</h3>
                                </div>
                                <div name="alloywheel" class="grid-7 omega">
                            	    <span filterid="1" class="form-control grid-6 checkOption">Alloy</span>
                            	    <span filterid="0" class="form-control grid-6 checkOption">Spoke</span>                                
                                </div>
                            </div>
                        
                             <div class="grid-12 alpha omega margin-bottom20 clear">
                        	    <div class="grid-5 alpha">
                            	    <h3>Start type</h3>
                                </div>
                                <div name="starttype" class="grid-7 omega">
                            	    <span filterid="1" class="form-control grid-6 checkOption">Electric</span>
                            	    <span filterid="0" class="form-control grid-6 checkOption">Kick</span>                                
                                </div>
                            </div>

			    </div>
            
                <!--Button starts here-->
                <div class="popup-btn-filters hide text-center">
                    <div class="margin-left10 margin-right10">
                        <input type="button" id="btnApplyFilters" class="btn btn-orange btn-full-width" value="Apply Filters" />
                    </div>
                </div>
                <!--Button ends here-->
                <!--Filters ends here-->
                <div class="clear"></div>
            </div>
            <!--Main container ends here-->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/lscache.min.js?<%= staticFileVersion%>"></script>
        <script>
            var selectedCityId = <%= cityId %> ,selectedAreaId = <%= areaId %> ;
            var cityName = $("#listingPopupCityList li[cityid='" + selectedCityId + "']").text(),
                areaName = $("#listingPopupAreaList li[areaid='" + selectedAreaId + "']").text()
            $("#Userlocation").text(cityName + ', ' + areaName);
            $("#listingCitySelection").text(cityName);
            $("#listingAreaSelection").text(areaName);

            var key = "bCity_";
            lscache.setBucket('BLPage');

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                if (optList != null) {
                    $("#listingPopupAreaList").append($('<li>').text(" Select Area ").attr({ 'cityId': "0" }));
                    $.each(optList, function (i, value) {
                        $("#listingPopupAreaList").append($('<li>').text(value.areaName).attr('value', value.areaId));
                    });
                }

                //$ddlAreas.trigger('chosen:updated');
                //$("#bookingAreasList_chosen .chosen-single.chosen-default span").text("No Areas available");
            }


        </script>
        <!-- all other js plugins -->    
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/BikeBooking/bookinglisting.js?<%= staticFileVersion %>" type="text/javascript"></script>
        <script>
            
            //$("#listingPopupCityList").chosen({ no_results_text: "No matches found!!" });
            //$("#listingPopupAreaList").chosen({ no_results_text: "No matches found!!" });
            $('.chosen-container').attr('style', 'width:100%;');
            $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("Please Select City");
        </script>

        <script type="text/javascript">
            var PQSourceId = '<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_BookingListing%>';
            var clientIP = '<%= clientIP %>';
        </script>
        <div class="back-to-top" id="back-to-top"><a><span></span></a></div>       
    </body>
</html>
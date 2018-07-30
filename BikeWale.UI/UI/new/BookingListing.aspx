<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.BookingListing" %>
<!DOCTYPE html>
<html>
<head>
    <%
        isHeaderFix = false;
     %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/new/bookinglisting.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    <script>ga_pg_id = '5';</script>
</head>
<body class="bg-light-grey">
<form runat="server">
<!-- #include file="/includes/headBW.aspx" -->
    <section class="bg-light-grey padding-top10">
    	<div class="container">
        	<div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <!-- breadcrumb code starts here -->
                    <ul>
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span>New Bikes</li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <h1 class="font30 text-black margin-bottom10">Book your bike</h1>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
    <section>
    	<div class="container">
        	<div class="grid-12">
            	<div id="filter-container">
                	<div class="filter-container content-box-shadow">
                        <div class="grid-10 omega padding-left20">
                        	<div class="grid-3 alpha">
                            	<div class="filter-div rounded-corner2">
                                	<div class="filter-select-title">
                                        <span class="hide">Select brand</span>
                                        <span class="leftfloat filter-select-btn default-text">Select brand</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                </div>
                                <div id="filter-select-brand" name="bike" class="filter-selection-div filter-brand-list list-items hide">
                                	<span class="top-arrow"></span>
                                	<ul class="content-inner-block-10">
                                        <li class="uncheck" filterId="7"><span>Honda</span></li>
                                        <li class="uncheck" filterId="1"><span>Bajaj</span></li>
                                        <li class="uncheck" filterId="6"><span>Hero</span></li>
                                        <li class="uncheck" filterId="15"><span>TVS</span></li>
                                        <li class="uncheck" filterId="11"><span>Royal Enfield</span></li>
                                        <li class="uncheck" filterId="13"><span>Yamaha</span></li>
                                        <li class="uncheck" filterId="12"><span>Suzuki</span></li>
                                        <li class="uncheck" filterId="9"><span>KTM</span></li>
                                        <li class="uncheck" filterId="10"><span>Mahindra</span></li>
                                    </ul>
                                    <div class="clear"></div>
                                    <div class="border-solid-top margin-left10 margin-right10"></div>
                                    <ul class="content-inner-block-10">
                                        <li class="uncheck" filterId="2"><span>Aprilia</span></li>
                                        <li class="uncheck" filterId="39"><span>Hero Electric</span></li>
                                        <li class="uncheck" filterId="20"><span>Moto Guzzi</span></li>
                                        <li class="uncheck" filterId="40"><span>Benelli</span></li>
                                        <li class="uncheck" filterId="8"><span>Hyosung</span></li>
                                        <li class="uncheck" filterId="41"><span>MV Agusta</span></li>
                                        <li class="uncheck" filterId="3"><span>BMW</span></li>
                                        <li class="uncheck" filterId="34"><span>Indian</span></li>
                                        <li class="uncheck" filterId="22"><span>Triumph</span></li>
                                        <li class="uncheck" filterId="4"><span>Ducati</span></li>
                                        <li class="uncheck" filterId="17"><span>Kawasaki</span></li>
                                        <li class="uncheck" filterId="16"><span>Vespa</span></li>
                                        <li class="uncheck" filterId="5"><span>Harley Davidson</span></li>
                                        <li class="uncheck" filterId="19"><span>LML</span></li>
                                        <li class="uncheck" filterId="14"><span>Yo</span></li>
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-3 alpha">
                                <div class="rounded-corner2 budget-box">
                                    <div id="minMaxContainer" class="filter-select-title">
                                        <span class="hide">Select budget</span>
                                        <span class="default-text" id="budgetBtn">Select budget</span>
                                        <span class="minAmount"></span>
                                        <span class="maxAmount"></span>
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                        <span class="clear"></span>
                                    </div>
                                </div>

                                <div name="budget" id="budgetListContainer" class="hide">
                                    <div id="userBudgetInput">
                                        <input type="text" id="minInput" class="priceBox" maxLength="9" placeholder="Min">
                                        <input type="text" id="maxInput" class="priceBox" maxLength="9" placeholder="Max">
                                        <div class="bw-blackbg-tooltip bw-blackbg-tooltip-max text-center hide">
        	                                Max budget should be greater than Min budget.
                                        </div>
                                    </div>
                                    <ul id="minList" class="text-left">
                                    </ul>
                                    <ul id="maxList" class="text-right">
                                    </ul>
                                </div>
                            </div>
                            <div class="grid-3 alpha">
                            	<div class="filter-div rounded-corner2">
                                	<div id="filter-select-mileage" class="filter-select-title">
                                        <span class="hide">Select mileage</span>
                                        <span class="leftfloat filter-select-btn default-text">Select mileage</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                </div>
                                <div name="mileage" class="filter-selection-div filter-mileage-list list-items hide">
                                	<span class="top-arrow"></span>
                                	<ul class="content-inner-block-10">
                                    	<li class="uncheck" filterId="1"><span>70 kmpl +</span></li>
                                        <li class="uncheck" filterId="2"><span>70 - 50 kmpl</span></li>
                                        <li class="uncheck" filterId="3"><span>50 - 30 kmpl</span></li>
                                        <li class="uncheck" filterId="4"><span>30 - 0 kmpl</span></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="grid-3 alpha">
                            	<div class="filter-div rounded-corner2">
                                	<div id="filter-select-displacement" class="filter-select-title">
                                        <span class="hide">Select displacement</span>
                                        <span class="leftfloat filter-select-btn default-text">Select displacement</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                </div>
                                <div name="displacement" class="filter-selection-div filter-displacement-list list-items hide">
                                	<span class="top-arrow"></span>
                                	<ul class="content-inner-block-10">
                                    	<li class="uncheck" filterId="1"><span>Up to 110 cc</span></li>
                                        <li class="uncheck" filterId="2"><span>110-150 cc</span></li>
                                        <li class="uncheck" filterId="3"><span>150-200 cc</span></li>
                                        <li class="uncheck" filterId="4"><span>200-250 cc</span></li>
                                        <li class="uncheck" filterId="5"><span>250-500 cc</span></li>
                                        <li class="uncheck" filterId="6"><span>500 cc and more</span></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="grid-2 alpha padding-right20">
	                        <div class="leftfloat">
		                        <div id="reset-btn-container" class="margin-top20">
			                        <p id="btnReset" class="filter-reset-btn font14">Reset</p>
		                        </div>
	                        </div>
	                        <div class="rightfloat">
		                        <div class="more-filters-btn position-rel rounded-corner2">
			                        <span class="font14"><span id="more-less-filter-text">More</span> Filters</span>
			                        <div class="filter-count-container">
				                        <div class="filter-counter">0</div>
				                        <span></span>
			                        </div>
		                        </div>
	                        </div>
	                        <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                	</div>
                    <div class="more-filters-container content-box-shadow hide">
                        <div class="grid-3 padding-top10">
                        	<div class="more-filter-ride">
                            	
                                <div class="more-filter-item-title">
                                	<h3>Ride Style</h3>
                                </div>
                                
                                <div class="filter-div rounded-corner2">
                                	<div id="filter-style-displacement" class="filter-select-title">
                                        <span class="hide">Select ride style</span>
                                        <span class="leftfloat filter-select-btn default-text">Select ride style</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                </div>
                                <div name="ridestyle" class="filter-selection-div more-filter-item-data ride-style-list list-items hide">
                                    <span class="top-arrow"></span>
                                	<ul class="content-inner-block-10">
                                    	<li class="uncheck" filterId="1"><span>Cruisers</span></li>
                                        <li class="uncheck" filterId="2"><span>Sports</span></li>
                                        <li class="uncheck" filterId="3"><span>Street</span></li>
                                        <li class="uncheck" filterId="5"><span>Scooters</span></li>
                                    </ul>
                                </div>
                            </div>      
                        </div>
                        <div class="grid-2 padding-top10">
                       		<div class="more-filter-abs">
                            	<div class="more-filter-item-title">
                                	<h3>ABS</h3>
                                </div>
                                <div name="AntiBreakingSystem" class="more-filter-item-data margin-top10">
                                	<div class="bw-tabs-panel">
                                    	<div class="bw-tabs home-tabs">
                                            <ul>
                                                <li filterid="1" class="first" data-tabs="yes">Yes</li>
                                                <li filterid="2" data-tabs="no" class="second">No</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>    
                        </div>
                        <div class="grid-2 padding-top10">
                        	<div class="more-filter-brakes">
                            	<div class="more-filter-item-title">
                                	<h3>Brakes</h3>
                                </div>
                                <div name="braketype" class="more-filter-item-data margin-top10">
                                	<div class="bw-tabs-panel">
                                    	<div class="bw-tabs home-tabs">
                                            <ul>
                                                <li filterid="2" class="first" data-tabs="disc">Disc</li>
                                                <li filterid="1"  data-tabs="drum" class="second">Drum</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>      
                        </div>
                        <div class="grid-2 padding-top10">
                        	<div class="more-filter-wheels">
                            	<div class="more-filter-item-title">
                                	<h3>Wheels</h3>
                                </div>
                                <div name="alloywheel" class="more-filter-item-data margin-top10">
                                	<div class="bw-tabs-panel">
                                    	<div class="bw-tabs home-tabs">
                                            <ul>
                                                <li filterid="2" class="first" data-tabs="alloy">Alloy</li>
                                                <li filterid="1" data-tabs="spoke" class="second">Spoke</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>      
                        </div>
                        <div class="grid-3 padding-top10">
                       		<div class="more-filter-start-type">
                            	<div class="more-filter-item-title">
                                	<h3>Start type</h3>
                                </div>
                                <div name="starttype" class="more-filter-item-data margin-top10">
                                	<div class="bw-tabs-panel">
                                    	<div class="bw-tabs home-tabs">
                                            <ul>
                                                <li filterid="1" class="first" data-tabs="electric">Electric</li>
                                                <li filterid="2" data-tabs="kick" class="second">Kick</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>     
                        </div>
                        <div class="clear"></div>
                        <div class="padding-left10 margin-top10 margin-bottom10">
                        	<input type="button" class="filter-done-btn btn btn-orange margin-right15" value="Done"/>
                            <div class="clear"></div>                          
                        </div>
                    </div>
            	</div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <section>
    	<div class="container margin-bottom20 margin-top10">
        	<div class="grid-12">
            	<div class="search-result-container content-box-shadow rounded-corner2">
                	<div class="search-count-container border-solid-bottom padding-top10 padding-bottom10">
                    	<div class="leftfloat grid-8 padding-left20 margin-top5">
                        	<h2 class="font18"><span id="bikecount"></span><span class="text-light-grey font16"> in </span><span class="change-city-area-target cur-pointer"><span>Area, City</span><span class="margin-left5 bwsprite loc-change-blue-icon"></span></span></h2>
                        </div>
                        <div class="rightfloat padding-left25 grid-3">
                            <div class="sort-div rounded-corner2">
                            	<div class="sort-by-title" id="sort-by-container">
                                	<span class="leftfloat sort-select-btn">Popular</span>
                                    <span class="clear"></span>
                                </div>
                                <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                            </div>
                            <div class="sort-selection-div sort-list-items hide">
                            	<ul>
                                	<li class="selected" so="" sc="" sortqs="">Popular</li>
                                    <li so="0" sc="1" sortqs="so=0&sc=1" >Price: Low to High</li>
                                    <li so="1" sc="1" sortqs="so=1&sc=1">Price: High to Low</li>
                                    <li so="0" sc="2" sortqs="so=0&sc=2">Mileage: High to Low</li>
                                </ul>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div id="listingLocationPopup" class="text-center rounded-corner2">
                            <div class="location-popup-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                            <div class="icon-outer-container rounded-corner50 margin-bottom20">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite cityPopup-icon margin-top20"></span>
                                </div>
                            </div>
                            <p class="font20 margin-top15 text-capitalize text-center">Please Tell Us Your Location</p>
                            <p class="text-light-grey margin-bottom15 margin-top15 text-capitalize text-center">See All the Bikes Available for Booking in Your Area!</p>
                            <div class="inner-content-popup">
                                <div>
                                    <select class="form-control">
                                        <option>City 1</option>
                                        <option>City 2</option>
                                    </select>
                                    <span class="bwsprite error-icon hide"></span>
                                    <div class="bw-blackbg-tooltip hide">Please Select City</div>
                                </div>
                                <div class="margin-top10">
                                    <select class="form-control">
                                        <option>Area 1</option>
                                        <option>Area 2</option>
                                    </select>
                                    <span class="bwsprite error-icon hide"></span>
                                    <div class="bw-blackbg-tooltip hide">Please Select Area</div>
                                </div>
                                <input class="btn btn-orange margin-top15" type="button" value="Show bikes">
                            </div>
                        </div>
                    </div>
                    <div id="searchBikeList" class="search-bike-list">
                    	<div class="grid-12 alpha omega margin-top20 margin-bottom10">
                        	<ul id="divSearchResult" data-bind="template: { name: 'listingTemp', foreach: searchResult }">
                                
                            </ul>
                        </div>
                        <div class="clear"></div>
                        <div style="text-align:center;">
                            <div id="NoBikeResults" class="hide">
                                <img src="/images/no_result_d.png" />  
                            </div>
                            <div id="loading" class="hide">
                                <img src="/images/search-loading.gif" />
                            </div>
                        </div>
                    </div>
                    
                    <script type="text/html" id="listingTemp">
                        <li>
                            <div class="contentWrapper">
                                <div class="imageWrapper position-rel">
                                    <!-- ko if: true -->
                                    <div class="offers-tag-wrapper position-abt">
                                        <span><span>3</span> Offers</span>
                                        <span class="offers-left-tag"></span>
                                    </div>
                                    <!-- /ko -->
                                    <a  data-bind="attr:{href:'/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/'},click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Model_Click', 'lab': bikemodel.modelName() }); return true; }">
                                        <img class="lazy" data-bind="attr: { title: bikeName, alt: bikeName, src: '' }, lazyload: bikemodel.hostUrl() + '/310X174/' + bikemodel.imagePath()">
                                    </a>
                                </div>
                                <div class="bikeDescWrapper font14 text-light-grey">
                                    <div class="booking-list-item-details">
                                        <div class="bikeTitle margin-bottom10">
                                            <h3><a data-bind="attr: { href:'/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/',title: bikeName }, text: bikeName, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Model_Click', 'lab': bikemodel.modelName() }); return true;}"></a></h3>
                                        </div>
                                        <p>BikeWale on-road price</p>
                                        <!-- ko if: true -->
                                        <div class="margin-top10">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font13 margin-right5 text-line-through" data-bind="text: price"></span>
                                            <span>
                                                (<span class="text-red">
                                                    <span class="fa fa-rupee"></span>
                                                    <span class="font13 margin-right5">5,000</span> Off
                                                 </span>)
                                            </span>
                                        </div>
                                        <!-- /ko -->
                                        <div class="font20 text-default">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22" data-bind="text: price"></span>
                                        </div>
                                        <!-- ko if: true -->
                                        <div class="text-default margin-top5 margin-bottom5">
                                            <span class="margin-right5">3 offers available</span>
                                            <span class="text-link view-offers-target">view offers</span>
                                        </div>
                                        <div id="offersPopup" class="text-center rounded-corner2">
                                            <div class="offers-popup-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                            <div class="icon-outer-container rounded-corner50">
                                                <div class="icon-inner-container rounded-corner50">
                                                    <span class="bwsprite offers-box-icon margin-top20"></span>
                                                </div>
                                            </div>
                                            <p class="font18 margin-top25 margin-bottom20 text-default">Available offers on this bike</p>
                                            <ul class="offers-list-ul">
                                                <li>Free Vega Cruiser Helmet worth Rs.1500 from BikeWale</li>
                                                <li>Free Zero Dep Insurance worth Rs.1200 from Dealership</li>
                                                <li>Get free helmet from the dealer</li>
                                                <li>Free Vega Cruiser Helmet worth Rs.1500 from BikeWale</li>
                                            </ul>
                                            <a class="book-now-popup-btn margin-top25 btn btn-orange font16">Book now</a>
                                        </div>
                                        <!-- /ko -->
                                        <p>Now book your bike online at <span class="text-default text-bold"><span class="margin-left5 fa fa-rupee"></span> <span>1,000</span></span></p>
                                    </div>
                                    <a class="btn btn-grey-orange btn-full-width margin-top15">Book now</a>
                                </div>
                            </div>
                        </li>
                    </script>
                </div>
            </div>
           	<div class="clear"></div>
        </div>
    </section>
    
    
<!-- #include file="/includes/footerBW.aspx" -->
<!-- #include file="/includes/footerscript.aspx" -->
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common/chosen.jquery.min.js?<%= staticFileVersion %>"></script>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/new/bookinglisting.js?<%= staticFileVersion %>"></script>
<script type="text/javascript">
    var PQSourceId = '<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_NewBikeSearch%>';
</script>
</form>
</body>
</html>

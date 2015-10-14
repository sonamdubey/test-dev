<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Search" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<html>
<head>
    <%
        isHeaderFix = false;
     %>
    <!-- #include file="/includes/headscript.aspx" -->
</head>
<body class="bg-light-grey">
<form runat="server">
<!-- #include file="/includes/headBW.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/new/search.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    
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
                <h1 class="font30 text-black margin-top10">Search New Bikes</h1>
                <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
    <section>
    	<div class="container">
        	<div class="grid-12">
            	<div id="filter-container">
                	<div class="filter-container content-box-shadow">
                        <div class="grid-10">
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
                                        <li class="uncheck" filterId="22"><span>Triumph</span></li>
                                        <li class="uncheck" filterId="3"><span>BMW</span></li>
                                        <li class="uncheck" filterId="34"><span>Indian</span></li>
                                        <li class="uncheck" filterId="16"><span>Vespa</span></li>
                                        <li class="uncheck" filterId="4"><span>Ducati</span></li>
                                        <li class="uncheck" filterId="17"><span>Kawasaki</span></li>
                                        <li class="uncheck" filterId="14"><span>Yo</span></li>
                                        <li class="uncheck" filterId="5"><span>Harley Davidson</span></li>
                                        <li class="uncheck" filterId="19"><span>LML</span></li>
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-3 alpha">
                            	<div class="rounded-corner2 budget-box">
                                	<div id="minMaxContainer" class="filter-select-title">
                                        <span class="hide">Select budget</span>
                                        <span class="leftfloat default-text" id="budgetBtn">Select budget</span>
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                        <span class="clear"></span>
                                    </div>
                                </div>
                                <div name="budget" id="budgetListContainer" class="hide">
                                    <div id="userBudgetInput">
                                        <input type="text" class="priceBox" id="minInput" placeholder="Min" maxlength="7">
                                        <div class="bw-blackbg-tooltip bw-blackbg-tooltip-min text-center hide">Min budget should be filled.</div>
                                        <input type="text" class="priceBox" id="maxInput" placeholder="Max" maxlength="7">
                                        <div class="bw-blackbg-tooltip bw-blackbg-tooltip-max text-center hide">Max budget should be greater than Min budget.</div>
                                    </div>
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
                        <div class="grid-2">
                            <div class="rightfloat">
                               	<div class="more-filters-btn position-rel rounded-corner2">
                                	<span class="bwsprite filter-icon inline-block"></span>
                                    <span class="font14 inline-block">More</span>
                                    <div class="filter-count-container">
                                    	<div class="filter-counter">0</div>
                                        <span></span>
                                    </div>
                                </div>
                            </div>
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
                        	<input type="button"  id="btnReset" class="filter-reset-btn btn btn-grey"  value="Reset"/>
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
                    	<div class="leftfloat grid-8">
                        	<h2><span id="bikecount"></span></h2>
                        </div>
                        <!--
                        <div class="rightfloat padding-right10 grid-3">
                            <div class="form-control-box">
                                <select id="sort" class="form-control">
                                    <option so="" sc="" value="" >Popular</option>
                                    <option so="0" sc="1" value="so=0&sc=1" >Price :Low to High</option>
                                    <option so="1" sc="1" value="so=1&sc=1">Price :High to Low</option>
                                    <option so="0" sc="2" value="so=0&sc=2" >Mileage :High to Low</option>
                                </select>
                            </div>
                        </div>
                        -->
                        <div class="rightfloat padding-right10 padding-left30 grid-3">
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
                    </div>
                    <div class="search-bike-list content-inner-block-10">
                    	<div class="grid-12 margin-top20 margin-bottom10">
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
                                <div class="imageWrapper">
                                    <a  data-bind="attr:{href:'/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/'},click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Model_Click', 'lab': bikemodel.modelName() }); return true; }">
                                        <img class="lazy" data-bind="attr: {title: bikeName, alt: bikeName, src:'../images/circleloader.gif'},lazyload: bikemodel.hostUrl() + '/310X174/' + bikemodel.imagePath()">
                                    </a>
                                </div>
                                <div class="bikeDescWrapper">
                                    <div class="bikeTitle margin-bottom10">
                                        <h3><a data-bind="attr: { href:'/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/',title: bikeName }, text: bikeName, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Model_Click', 'lab': bikemodel.modelName() }); return true;}"></a></h3>
                                    </div>
                                    <div class="font20">
                                        <span class="fa fa-rupee"></span>
                                        <span class="font22" data-bind="text: price"></span> <span class="font16">onwards</span>
                                    </div>
                                    <div class="font12 text-light-grey margin-bottom10">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"] %></div>
                                    <div class="font14 margin-bottom10">
                                        <span><span data-bind="html: availSpecs"> </span></span>
                                    </div>
                                    <!-- ko if:bikemodel.reviewCount() != 0  -->
                                        <div class="leftfloat">
                                            <p class=" inline-block border-solid-right padding-right10" >
                                                <span data-bind="html: AppendCertificationStar(bikemodel.reviewRate())"></span>
                                            </p>
                                        </div>
                                    <!-- /ko -->
                                    <div class="leftfloat margin-left10 font16 text-light-grey">
                                        <span data-bind="text: ShowReviewCount(bikemodel.reviewCount())"> </span>
                                    </div>
                                    <div class="clear"></div>
                                    <a data-bind="attr: { modelId: bikemodel.modelId }, click: function () { FillCitiesPopup(bikemodel.modelId(), bikemodel.makeBase.makeName(), bikemodel.modelName(), '4'); $.PricePopUpClickGA(bikemodel.modelName()); }" class="btn btn-grey margin-top10 fillPopupData">Get on road price</a>
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
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/framework/knockout.js?<%= staticFileVersion %>"></script>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common/chosen.jquery.min.js?<%= staticFileVersion %>"></script>
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/new/search.js?<%= staticFileVersion %>"></script>
    <PW:PopupWidget runat="server" ID="PopupWidget" />
</form>
</body>
</html>

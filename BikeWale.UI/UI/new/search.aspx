<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.SearchOld" %>

<html>
<head>
    <%
        title = "Search New Bikes by Brand, Budget, Mileage and Ride Style - BikeWale";
        description = "Search through all the new bike models by various criteria. Get instant on-road price for the bike of your choice";
        keywords = "search new bikes, search bikes by brand, search bikes by budget, search bikes by price, search bikes by style, street bikes, scooters, commuter bikes, cruiser bikes";
        canonical = "https://www.bikewale.com/new/bike-search/";
        alternate = "https://www.bikewale.com/m/new/bike-search/";
        isHeaderFix = false;
        AdId = "1442913773076";
        AdPath = "/1017752/Bikewale_NewBike_";
        isAd970x90Shown = true;
        isAd970x90BottomShown = true;
        

        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BTFShown = false;
         
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <script>ga_pg_id = '5';</script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <link href="<%= staticUrl  %>/css/new/search.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <link href="<%= staticUrl  %>/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url">
                                    <span itemprop="title">Home</span>
                                </a>
                            </li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>New Bikes</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="margin-top10">Search New Bikes</h1>
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
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                    </div>
                                    <div id="filter-select-brand" name="bike" class="filter-selection-div filter-brand-list list-items hide">
                                        <span class="top-arrow"></span>
                                        <ul class="content-inner-block-10">
                                            <asp:Repeater ID="rptPopularBrand" runat="server">
                                                <ItemTemplate>
                                                    <li class="uncheck" filterid="<%# DataBinder.Eval(Container.DataItem, "MakeId").ToString() %>"><span><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() %></span></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <div class="clear"></div>
                                        <div class="border-solid-top margin-left10 margin-right10"></div>
                                        <ul class="content-inner-block-10">
                                            <asp:Repeater ID="rptOtherBrands" runat="server">
                                                <ItemTemplate>
                                                    <li class="uncheck" filterid="<%# DataBinder.Eval(Container.DataItem, "MakeId").ToString() %>"><span><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() %></span></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
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
                                            <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                            <span class="clear"></span>
                                        </div>
                                    </div>

                                    <div name="budget" id="budgetListContainer" class="hide">
                                        <div id="userBudgetInput">
                                            <input type="text" id="minInput" class="priceBox" maxlength="9" placeholder="Min">
                                            <input type="text" id="maxInput" class="priceBox" maxlength="9" placeholder="Max">
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
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                    </div>
                                    <div name="mileage" class="filter-selection-div filter-mileage-list list-items hide">
                                        <span class="top-arrow"></span>
                                        <ul class="content-inner-block-10">
                                            <li class="uncheck" filterid="1"><span>70 kmpl +</span></li>
                                            <li class="uncheck" filterid="2"><span>70 - 50 kmpl</span></li>
                                            <li class="uncheck" filterid="3"><span>50 - 30 kmpl</span></li>
                                            <li class="uncheck" filterid="4"><span>30 - 0 kmpl</span></li>
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
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                    </div>
                                    <div name="displacement" class="filter-selection-div filter-displacement-list list-items hide">
                                        <span class="top-arrow"></span>
                                        <ul class="content-inner-block-10">
                                            <li class="uncheck" filterid="1"><span>Up to 110 cc</span></li>
                                            <li class="uncheck" filterid="2"><span>110-150 cc</span></li>
                                            <li class="uncheck" filterid="3"><span>150-200 cc</span></li>
                                            <li class="uncheck" filterid="4"><span>200-250 cc</span></li>
                                            <li class="uncheck" filterid="5"><span>250-500 cc</span></li>
                                            <li class="uncheck" filterid="6"><span>500 cc and more</span></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-2 alpha">
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
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                    </div>
                                    <div name="ridestyle" class="filter-selection-div more-filter-item-data ride-style-list list-items hide">
                                        <span class="top-arrow"></span>
                                        <ul class="content-inner-block-10">
                                            <li class="uncheck" filterid="1"><span>Cruisers</span></li>
                                            <li class="uncheck" filterid="2"><span>Sports</span></li>
                                            <li class="uncheck" filterid="3"><span>Street</span></li>
                                            <li class="uncheck" filterid="5"><span>Scooters</span></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-2 padding-top10">
                                <div class="more-filter-abs">
                                    <div class="more-filter-item-title">
                                        <h3>ABS</h3>
                                    </div>
                                    <div name="ABS" class="more-filter-item-data margin-top10">
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
                                                    <li filterid="1" data-tabs="drum" class="second">Drum</li>
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
                                <input type="button" class="filter-done-btn btn btn-orange margin-right15" value="Done" />
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
                            <div class="leftfloat grid-8 padding-top5">
                                <h2><span id="bikecount"></span></h2>
                            </div>
                            <div class="rightfloat padding-right10 padding-left30 grid-3">
                                <div class="sort-div rounded-corner2">
                                    <div class="sort-by-title" id="sort-by-container">
                                        <span class="leftfloat sort-select-btn">Popular</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                </div>
                                <div class="sort-selection-div sort-list-items hide">
                                    <ul>
                                        <li class="selected" so="" sc="" sortqs="">Popular</li>
                                        <li so="0" sc="1" sortqs="so=0&sc=1">Price: Low to High</li>
                                        <li so="1" sc="1" sortqs="so=1&sc=1">Price: High to Low</li>
                                        <li so="0" sc="2" sortqs="so=0&sc=2">Mileage: High to Low</li>
                                    </ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="search-bike-list">
                            <div class="grid-12 alpha omega padding-left20 margin-top20 margin-bottom10">
                                <ul id="divSearchResult" data-bind="template: { name: 'listingTemp', foreach: searchResult }">
                                </ul>
                            </div>
                            <div class="clear"></div>
                            <div style="text-align: center;">
                                <div id="NoBikeResults" class="hide">
                                    <img src="/image/no_result_d.png" />
                                </div>
                                <div id="loading" class="hide">
                                    <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif" />
                                </div>
                            </div>
                        </div>

                        <script type="text/html" id="listingTemp">
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a data-bind="attr: { href: '/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/' }, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Model_Click', 'lab': bikemodel.modelName() }); return true; }">
                                            <img class="lazy" data-bind="attr: { title: bikeName, alt: bikeName, src: '' }, lazyload: bikemodel.hostUrl() + '/310X174/' + bikemodel.imagePath()">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <h3 class="bikeTitle margin-bottom10"><a data-bind="attr: { href: '/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/', title: bikeName }, text: bikeName, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Model_Click', 'lab': bikemodel.modelName() }); return true; }"></a></h3>
                                        <div class="text-xt-light-grey font14 margin-bottom15">
                                            <span><span data-bind="html: availSpecs"></span></span>
                                        </div>
                                        <div class="font14 text-light-grey margin-bottom5">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"] %></div>
                                        <div class="text-bold">
                                            <span class="bwsprite inr-lg"></span>
                                            <span class="font18" data-bind="text: price"></span><span class="font14"> onwards</span>
                                        </div>
                                        <a data-bind="visible: price() != 'N/A', attr: { 'data-modelId': bikemodel.modelId, 'data-pqSourceId': PQSourceId }, click: function () { $.PricePopUpClickGA(bikemodel.modelName()); }" class="btn btn-grey btn-sm margin-top15 font14 getquotation">Check on-road price</a>
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
        <script type="text/javascript" src="<%= staticUrl  %>/src/new/search.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var PQSourceId = '<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_NewBikeSearch%>';
        </script>
    </form>
</body>
</html>

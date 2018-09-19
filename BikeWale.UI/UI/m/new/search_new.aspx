<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.Search" %>

<%@ Register Src="~/UI/m/controls/ChangeLocationPopup.ascx" TagPrefix="BW" TagName="LocationWidget" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/UI/m/controls/MPopupWidget.ascx" %>

<!doctype html>
<html>
<head>
    <%
        title = "Search New Bikes by Brand, Budget, Mileage and Ride Style - BikeWale";
        keywords = "search new bikes, search bikes by brand, search bikes by budget, search bikes by price, search bikes by style, street bikes, scooters, commuter bikes, cruiser bikes";
        description = "Search through all the new bike models by various criteria. Get instant on-road price for the bike of your choice";
        canonical = "https://www.bikewale.com/new/bike-search/";
        AdPath = "/1017752/Bikewale_Mobile_Homepage_";
        AdId = "1398766000399";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        Ad_300x250 = true;
    %>


    <!-- #include file="/UI/includes/headscript_mobile.aspx" -->
    <script>ga_pg_id = '5'; var PQSourceId = '<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_NewBikeSearch %>';</script>
</head>
<body class="bg-light-grey">

    <link href="<%= staticUrl  %>/UI/m/css/new/bwm-search.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <link href="<%= staticUrl  %>/UI/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" />
    <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

    <div class="blackOut-window"></div>
    <!-- global-search-popup code starts here -->

    <div id="global-search-popup" class="global-search-popup" style="display: none">
        <div class="form-control-box">
            <span class="back-arrow-box" id="gs-close">
                <span class="bwmsprite back-long-arrow-left"></span>
            </span>
            <span class="cross-box hide" id="gs-text-clear">
                <span class="bwmsprite cross-md-dark-grey"></span>
            </span>
            <input type="text" name="globalSearch" placeholder="Search" id="globalSearch" class="form-control padding-right30" autocomplete="off">
            <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none; right: 35px; top: 13px"></span>
            <ul id="errGlobalSearch" style="width: 100%; margin-left: 0" class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content hide">
                <li class="ui-menu-item" tabindex="-1">
                    <span class="text-bold">Oops! No suggestions found</span><br />
                    <span class="text-light-grey font12">Search by bike name e.g: Honda Activa</span>
                </li>
            </ul>
        </div>
    </div>
    <!-- global-search-popup code ends here -->
    <header>
        <div class="header-fixed">
            <!-- Fixed Header code starts here -->
            <span id="bikecount" class="font18 text-white brand-total"></span>
            <div class="leftfloat">
                <span id="navbarBtn" class="navbarBtn bwmsprite nav-icon margin-right10"></span>
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
        </div>
        <!-- ends here -->
        <div class="clear"></div>

    </header>

    <section>
        <div class="container">
            <div>
                <!-- #include file="/UI/ads/Ad320x50_mobile.aspx" -->
            </div>
        </div>
    </section>

    <section>
        <!--  Used Search code starts here -->
        <div class="container">
            <div>
                <!--  class="grid-12"-->

                <div class="hide" id="sort-by-div">
                    <div class="filter-sort-div font14 bg-white">
                        <div sc="1" so="">
                            <a data-title="sort" class="price-sort position-rel">Price<span class="hide" so="0" class="sort-text"></span>
                            </a>
                        </div>
                        <div sc="" class="border-solid-left">
                            <a data-title="sort" class="position-rel">Popularity 
                            </a>
                        </div>
                        <div sc="2" class="border-solid-left">
                            <a data-title="sort" class="position-rel">Mileage 
                            </a>
                        </div>
                    </div>
                </div>
                <div class="bike-search">
                    <div id="divSearchResult" data-bind="template: { name: 'listingTemp', foreach: searchResult }" class="search-bike-container">
                    </div>
                    <div style="text-align: center;">
                        <div id="nobike" class="hide">
                            <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/no-result-m.png" alt="No match found">
                        </div>
                        <div id="loading" class="hide">
                            <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif" />
                        </div>
                    </div>
                </div>
                <script type="text/html" id="listingTemp">
                    <div class="search-bike-item">
                        <div class="front">
                            <div class="contentWrapper">
                                <!--<div class="position-abt pos-right10 pos-top10 infoBtn bwmsprite alert-circle-icon"></div>-->
                                <div class="imageWrapper">
                                    <a data-bind="click: function () { $.ModelClickGaTrack(bikemodel.modelName(), '/m/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/') }">
                                        <img data-bind="attr: { title: bikeName, alt: bikeName, src: 'https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif' }, lazyload: bikemodel.hostUrl() + '/310X174/' + bikemodel.imagePath()" />
                                    </a>
                                </div>
                                <div class="bikeDescWrapper">
                                    <div class="bikeTitle margin-bottom10">
                                        <h3><a data-bind="attr: { title: bikeName }, text: bikeName, click: function () { $.ModelClickGaTrack(bikemodel.modelName(), '/m/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/') }"></a></h3>
                                    </div>

                                    <div id="reviewRatingsDiv" class=" font13 margin-bottom10 position-rel pos-right5">
                                        <span data-bind="css: { 'rate-count-5': bikemodel.reviewRate() >= 4.5, 'rate-count-4': bikemodel.reviewRate() >= 3.5 && bikemodel.reviewRate() < 4.5, 'rate-count-3': bikemodel.reviewRate() >= 2.5 && bikemodel.reviewRate() < 3.5, 'rate-count-2': bikemodel.reviewRate() >= 1.5 && bikemodel.reviewRate() < 2.5, 'rate-count-1': bikemodel.reviewRate() >= 0.5 && bikemodel.reviewRate() < 1.5, 'rate-count-0': bikemodel.reviewRate() < .5 }">
                                            <span class="bwmsprite star-icon star-size-16"></span
                                                ><span data-bind="template: {if: bikemodel.reviewRate()}"><span class="font14 text-bold inline-block" data-bind="text: bikemodel.reviewRate()"></span></span>
                                                <span data-bind="template: {if: bikemodel.reviewRate() == 0}"><span class="font13 text-light-grey inline-block" >Not rated yet</span></span>
                                            </span>
                                        <span class='font11  inline-block padding-left3' data-bind="template: { if: bikemodel.ratingCount() }">&nbsp;(<span data-bind="    text: bikemodel.ratingCount()"></span><span data-bind="    text: bikemodel.ratingCount() == 1 ? ' rating' : ' ratings'"></span>)</span>
                                        <span data-bind="template: { if: bikemodel.reviewCount() }">
                                            <a class='text-xt-light  inline-block' data-bind="    attr: { href: '/m/' + bikemodel.makeBase.maskingName() + '-bikes/' + bikemodel.maskingName() + '/reviews/', title: bikeName() + ' user reviews' }">
                                        <span class="review-left-divider" data-bind="text: bikemodel.reviewCount()"></span><span data-bind="    text: bikemodel.reviewCount() == 1 ? ' review' : ' reviews'"></span></a></span>
                                       
                                    </div>

                                    <div class="margin-bottom5 font14 text-light-grey">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"] %></div>
                                    <div>
                                        <span class="bwmsprite inr-sm-icon"></span>
                                        <span class="text-bold font18" data-bind="text: price"></span>
                                    </div>
                                    <a data-bind=" visible: price() != 'N/A', attr: { 'data-modelId': bikemodel.modelId, 'data-pqSourceId': PQSourceId }" class="btn btn-sm btn-white font14 margin-top20 getquotation" rel="nofollow">Check on-road price</a>
                                </div>
                            </div>
                        </div>
                        <div class="border-top1 margin-left20 margin-right20 padding-top20 clear"></div>
                    </div>
                </script>
            </div>
            <div class="clear"></div>
        </div>

    </section>
    <!-- Used Search code  Ends here -->

    <!--Filters starts here-->
    <div id="filter-div" class="popup_layer hide">
        <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
            <div id="hidePopup" class="filterBackArrow" popupname="filterpopup" onclick="CloseWindow(this)">
                <!--<span class="bwmsprite back-long-arrow-left-white"></span>-->
                <span class="bwmsprite fa-angle-left"></span>
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

                <div name="bike" class="multiSelect">
                    <ul>
                        <%foreach (var item in rptPopularBrand)
                            {%>
                        <li class="unchecked" filterid="<%=item.MakeId%>"><span><%=item.MakeName%></span></li>
                        <% } %>
                        <hr class="border-solid" />
                        <%foreach (var item in rptOtherBrands)
                            {%>
                        <li class="unchecked" filterid="<%=item.MakeId%>"><span><%=item.MakeName%></span></li>
                        <% } %>
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
                        <li class="unchecked" filterid="1"><span>Up to 110 cc</span></li>
                        <li class="unchecked" filterid="7"><span>110-125 cc</span></li>
                        <li class="unchecked" filterid="8"><span>125-150 cc</span></li>
                        <li class="unchecked" filterid="3"><span>150-200 cc</span></li>
                        <li class="unchecked" filterid="4"><span>200-250 cc</span></li>
                        <li class="unchecked" filterid="5"><span>250-500 cc</span></li>
                        <li class="unchecked" filterid="6"><span>500 cc and more</span></li>
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
                        <li class="unchecked" filterid="1"><span>Cruisers</span></li>
                        <li class="unchecked" filterid="2"><span>Sports</span></li>
                        <li class="unchecked" filterid="3"><span>Street</span></li>
                        <li class="unchecked" filterid="5"><span>Scooters</span></li>
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
                <div name="ABS" class="grid-7 omega">
                    <span filterid="1" class="form-control grid-6 checkOption">Yes</span>
                    <span filterid="2" class="form-control grid-6 checkOption">No</span>
                </div>
            </div>

            <div class="grid-12 alpha omega margin-bottom20 clear">
                <div class="grid-5 alpha">
                    <h3>Brakes</h3>
                </div>
                <div name="braketype" class="grid-7 omega">
                    <span filterid="2" class="form-control grid-6 checkOption">Disc</span>
                    <span filterid="1" class="form-control grid-6 checkOption">Drum</span>
                </div>
            </div>

            <div class="grid-12 alpha omega margin-bottom20 clear">
                <div class="grid-5 alpha">
                    <h3>Wheels</h3>
                </div>
                <div name="alloywheel" class="grid-7 omega">
                    <span filterid="2" class="form-control grid-6 checkOption">Alloy</span>
                    <span filterid="1" class="form-control grid-6 checkOption">Spoke</span>
                </div>
            </div>

            <div class="grid-12 alpha omega margin-bottom20 clear">
                <div class="grid-5 alpha">
                    <h3>Start type</h3>
                </div>
                <div name="starttype" class="grid-7 omega">
                    <span filterid="1" class="form-control grid-6 checkOption">Electric</span>
                    <span filterid="2" class="form-control grid-6 checkOption">Kick</span>
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
    <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
    <!-- all other js plugins -->
    <!-- #include file="/UI/includes/footerscript_Mobile.aspx" -->
    <script src="<%= staticUrl  %>/UI/m/src/new/search.js?<%= staticFileVersion %>" type="text/javascript"></script>
    <div class="back-to-top" id="back-to-top"></div>
</body>
</html>

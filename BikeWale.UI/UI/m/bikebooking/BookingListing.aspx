<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.bikebooking.BookingListing" Trace="false" %>
<%@ Register Src="~/UI/m/controls/ChangeLocationPopup.ascx" TagPrefix="BW" TagName="LocationWidget" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/UI/m/controls/MPopupWidget.ascx" %>
<!doctype html>
<html>
<head>
    <%
        title = "Book Bikes, Scooters in India and Avail Great Benefits - BikeWale";
        keywords = "book bikes, book scooters, buy bikes, buy scooters, bikes prices, avail offers, avail discounts, instant bike on-road price";
        description = "BikeWale - India's favourite bike portal. Book your bikes, scooters and avail exciting offers and benefits exclusively on BikeWale.";
    %>
    <!-- #include file="/UI/includes/headscript_mobile.aspx" -->

</head>
<body class="bg-light-grey">
    <link href="<%= staticUrl  %>/UI/m/css/new/bwm-bookinglisting.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <link href="<%= staticUrl  %>/UI/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" />
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
        <div class="header-fixed book-your-bike-doodle">
            <!-- Fixed Header code starts here -->
            <div class="leftfloat">
                <span id="navbarBtn" class="navbarBtn nav-icon margin-right5"></span>
                <span class="booking-listing-nav font16 text-white position-rel">Book your bike</span>
            </div>
			<!-- Doodle start-->
			<span class="doodle__container bw-ga" data-cat="Doodle" data-act="Doodle_Click" data-lab="Navratri">
				<span class="doodle__asset-one"></span>
				<span class="doodle__asset-two"></span>
				<span class="doodle__asset-three"></span>
				<span class="doodle__block">
					<span class="doodle__image"></span>
					<span class="doodle__bg"></span>
				</span>
			</span>
			<!--Doodle end-->
            <div class="rightfloat">
                <a class="global-search" id="global-search" style="display: none">
                    <span class="global-search-icon"></span>
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
                        <div sc="3" class="border-solid-left">
                            <a data-title="sort" class="position-rel">Popularity 
                            </a>
                        </div>
                    </div>
                </div>
                <div id="listingCountContainer" class="font14 padding-top20 padding-bottom20 text-center">
                    <span><span id="bikecount" class="font18"></span><span class="text-light-grey">&nbsp;in</span></span><br />
                    <span class="change-city-area-target"><span id="Userlocation"></span><span class="margin-left5 bwmsprite loc-change-blue-icon icon-adjustment"></span></span>
                    <div id="listingLocationPopup" class="font13 bwm-fullscreen-popup">
                        <div class="bwmsprite location-popup-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
                        <div id="listingPopupHeading">
                            <p class="font18 margin-top10 margin-bottom5 text-capitalize">Please Tell Us Your Location</p>
                            <p class="text-light-grey margin-bottom5">See Bikes Available for Booking in Your Area!</p>
                            <div id="listingCitySelection" class="form-control text-left position-rel margin-bottom10">
                                <div class="selected-city input-sm">Select City</div>
                                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>

                            <div id="listingAreaSelection" class="form-control text-left position-rel margin-bottom10">
                                <div class="selected-area input-sm">Select Area</div>
                                <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
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
                                <ul id="listingPopupCityList" class="margin-top40">
                                    <asp:Repeater ID="rptCities" runat="server">
                                        <ItemTemplate>
                                            <li cityid="<%# DataBinder.Eval(Container.DataItem, "CityId") %>"><%# DataBinder.Eval(Container.DataItem, "CityName")%></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>

                            <div class="bw-area-popup-box bwm-city-area-box area-list-container form-control-box text-left">
                                <div class="user-input-box">
                                    <span class="back-arrow-box">
                                        <span class="bwmsprite back-long-arrow-left"></span>
                                    </span>
                                    <input class="form-control" type="text" id="listingPopupAreaInput" placeholder="Select Area" data-bind="attr: { value: SelectedArea() ? SelectedArea().areaName : '' }" />
                                </div>
                                <ul id="listingPopupAreaList" class="margin-top40">
                                    <asp:Repeater ID="rptAreas" runat="server">
                                        <ItemTemplate>
                                            <li areaid="<%# DataBinder.Eval(Container.DataItem, "AreaId") %>"><%# DataBinder.Eval(Container.DataItem, "AreaName")%></li>
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
                                <div class="imageWrapper margin-top10">

                                    <div data-bind="visible: offers().length > 0" class="offers-tag-wrapper position-abt">
                                        <span><span data-bind="text: offers().length == 1 ? offers().length + ' offer' : offers().length + ' offers'"></span></span>
                                    </div>
                                    <span class="offers-left-tag"></span>
                                    <a data-bind="attr: { href: '/m/' + makeEntity.maskingName() + '-bikes/' + modelEntity.maskingName() + '/?vid=' + versionEntity.versionId() }, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Model_Click', 'lab': modelEntity.modelName() }); return true; }">
                                        <img class="lazy" data-bind="attr: { title: bikeName(), alt: bikeName(), src: '' }, lazyload: hostUrl() + '/310X174/' + originalImagePath() ">
                                    </a>
                                </div>
                                <div class="bikeDescWrapper">
                                    <div class="bikeTitle margin-bottom10">
                                        <h3><a data-bind="attr: { href: '/m/' + makeEntity.maskingName() + '-bikes/' + modelEntity.maskingName() + '/?vid=' + versionEntity.versionId(), title: bikeName }, text: bikeName, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Model_Click', 'lab': modelEntity.modelName }); return true; }"></a></h3>
                                    </div>
                                    <p class="font14 text-light-grey">BikeWale on-road price</p>
                                    <div class="margin-top10 text-light-grey" data-bind="visible: discount() > 0">
                                        <span class="bwmsprite inr-grey-xxsm-icon"></span>
                                        <span class="font13 margin-right5 text-line-through" data-bind="CurrencyText: onRoadPrice()"></span>
                                        <span>( <span class="text-red">
                                            <span class="bwmsprite inr-red-xxsm-icon"></span>
                                            <span class="font13 margin-right5" data-bind="CurrencyText: discount()"></span>Off
                                        </span>)
                                        </span>
                                    </div>
                                    <div class="margin-bottom5">
                                        <span class="bwmsprite inr-sm-icon"></span>
                                        <span class="font18 text-bold" data-bind="CurrencyText: discountedPrice()"></span>
                                    </div>
                                    <div class="font14 margin-top5 margin-bottom5" data-bind="visible: offers().length > 0">
                                        <span class="text-default margin-right5" data-bind="text: offers().length == 1 ? offers().length + ' offer available' : offers().length + ' offers available'"></span>
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
                                        <ul class="offers-list-ul" data-bind="foreach: offers()">
                                            <li>
                                                <span data-bind="text: offerText()"></span>
                                                <span class="tnc font9" data-bind="visible: isOfferTerms(), attr: { 'offerId': offerId }, click: function () { loadTerms(offerId()); }">view terms</span>
                                            </li>
                                        </ul>
                                        <input type="button" class="book-now-popup-btn margin-top30 btn btn-orange font16" data-bind="click: function () { window.history.back(); registerPQ($data); }" value="Book now" />
                                    </div>
                                    <p class="font14 text-light-grey">Now book your bike online at <span class="text-default text-bold"><span class="bwmsprite inr-xxsm-icon margin-left5"></span><span class="font15" data-bind="text: bookingAmount"></span></span></p>
                                    <input type="button" class="margin-top10 btn btn-orange btn-full-width margin-top10" data-bind="click: function () { registerPQ($data); }" value="Book now" />
                                </div>
                            </div>
                        </div>
                        <div class="border-top1 margin-left20 margin-right20 padding-top10 clear"></div>
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

                <div name="makeIds" class="multiSelect">
                    <ul>
                        <asp:Repeater ID="rptPopularBrand" runat="server">
                            <ItemTemplate>
                                <li class="unchecked" filterid="<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"><span><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span></li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <hr class="border-solid" /> 
                        <asp:Repeater ID="rptOtherBrands" runat="server">
                            <ItemTemplate>
                                <li class="unchecked" filterid="<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"><span><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span></li>
                            </ItemTemplate>
                        </asp:Repeater>
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
    <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                <div id="termsPopUpCloseBtn" class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <h3>Terms and Conditions</h3>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class="hide">
            </div>
        </div>
        <!-- Terms and condition Popup end -->


    <!--Main container ends here-->
    <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
    <script>
        var selectedCityId = '<%= cityId %>', selectedAreaId = '<%= areaId %>';
        var cityName = $("#listingPopupCityList li[cityid='" + selectedCityId + "']").text(),
            areaName = $("#listingPopupAreaList li[areaid='" + selectedAreaId + "']").text();
        $("#Userlocation").text(cityName + ', ' + areaName);
        $("#listingCitySelection .selected-city").text(cityName);
        $("#listingAreaSelection .selected-area").text(areaName);

        var key = "bCity_";
        lscache.setBucket('BLPage');

        function checkCacheCityAreas(cityId) {
            bKey = key + cityId;
            if (lscache.get(bKey)) return true;
            else return false;
        }

        function setOptions(optList) {
            if (optList != null) {
                $.each(optList, function (i, value) {
                    $("#listingPopupAreaList").append($('<li>').text(value.areaName.toString().trim()).attr('areaid', value.areaId));
                });
            }
        }


    </script>
    <!-- all other js plugins -->
    <!-- #include file="/UI/includes/footerscript_Mobile.aspx" -->
    <script src="<%= staticUrl  %>/UI/m/src/BikeBooking/bookinglisting.js?<%= staticFileVersion %>" type="text/javascript"></script>
    <script>
        $('.chosen-container').attr('style', 'width:100%;');
        $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("Please Select City");
        $('#termsPopUpCloseBtn ').on("click", function () {
            $('#termsPopUpContainer').hide();
            $(".blackOut-window").hide();
        });
        $('.blackOut-window').on("click", function () {
            $("div#termsPopUpContainer").hide();
            $(".blackOut-window").hide();
        });
    </script>

    <script type="text/javascript">
        var pqLeadId = '<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_BookingListing%>';
        var clientIP = '<%= clientIP %>';
        ga_pg_id = '39';
        var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.BookingListing_Page%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.BookingListing_Page%>' };
    </script>
    <div class="back-to-top" id="back-to-top"></div>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.BookingListing" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = "Book Bikes, Scooters in India and Avail Great Benefits - BikeWale";
        keywords = "book bikes, book scooters, buy bikes, buy scooters, bikes prices, avail offers, avail discounts, instant bike on-road price";
        description = "BikeWale - India's favourite bike portal. Book your bikes, scooters and avail exciting offers and benefits exclusively on BikeWale.";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
    %>
    <!-- #include file="/UI/includes/headscript.aspx" -->
    <link href="<%= staticUrl %>/UI/css/new/bookinglisting.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl  %>/UI/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        ga_pg_id = '39';
        var clientIP = '<%= clientIP %>';
    </script>
</head>
<body class="bg-light-grey" data-contestslug="true">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->
        <script type="text/javascript">document.getElementById("header").children[1].innerHTML = "";</script>
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><a href="/bikebooking/"><span class="bwsprite fa-angle-right margin-right10"></span>Booking</a></li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>Bikes</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="margin-bottom10">Book your bike</h1>
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
                                <div class="grid-4 alpha">
                                    <div class="filter-div rounded-corner2">
                                        <div class="filter-select-title">
                                            <span class="hide">Select brand</span>
                                            <span class="leftfloat filter-select-btn default-text">Select brand</span>
                                            <span class="clear"></span>
                                        </div>
                                        <span class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10 upDownArrow"></span>
                                    </div>
                                    <div id="filter-select-brand" name="makeIds" class="filter-selection-div filter-brand-list list-items hide">
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
                                <div class="grid-4 alpha">
                                    <div class="rounded-corner2 budget-box">
                                        <div id="minMaxContainer" class="filter-select-title">
                                            <span class="hide">Select budget</span>
                                            <span class="default-text" id="budgetBtn">Select budget</span>
                                            <span class="minAmount"></span>
                                            <span class="maxAmount"></span>
                                            <span class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10 upDownArrow"></span>
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
				<div class="grid-4">
					<div class="more-filter-ride">
						<div class="filter-div rounded-corner2">
							<div class="filter-select-title">
								<span class="hide">Select ride style</span>
								<span class="leftfloat filter-select-btn default-text">Select ride style</span>
								<span class="clear"></span>
							</div>
							<span class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10 upDownArrow"></span>
						</div>
						<div name="rideStyle" class="filter-selection-div more-filter-item-data ride-style-list list-items hide">
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
                                <div class="clear"></div>
                            </div>
                            <div class="grid-2 alpha padding-right20">
                                <div>
                                    <div id="reset-btn-container" class="margin-top20">
                                        <p id="btnReset" class="filter-reset-btn font14 text-center">Reset</p>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
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
                                <h2 class="text-default"><span id="bikecount"></span><span class="text-light-grey font16 margin-left5 ">in </span><span class="change-city-area-target cur-pointer"><span id="Userlocation"></span><span class="margin-left5 bwsprite loc-change-blue-icon"></span></span></h2>
                            </div>
                            <div class="rightfloat padding-left25 grid-3">
                                <div class="sort-div rounded-corner2">
                                    <div class="sort-by-title" id="sort-by-container">
                                        <span class="leftfloat sort-select-btn">Price: Low to High</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10 upDownArrow"></span>
                                </div>
                                <div class="sort-selection-div sort-list-items hide">
                                    <ul>
                                        <li so="1" sc="3" sortqs="so=1&sc=3">Popular</li>
                                        <li class="selected" so="0" sc="1" sortqs="so=0&sc=1">Price: Low to High</li>
                                        <li so="1" sc="1" sortqs="so=1&sc=1">Price: High to Low</li>
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
                                    <asp:DropDownList ID="bookingCitiesList" class="form-control chosen-select" runat="server" />
                                    <span class="bwsprite error-icon hide"></span>
                                    <div class="bw-blackbg-tooltip hide">Please Select City</div>
                                    <div class="margin-top10">
                                        <asp:DropDownList ID="bookingAreasList" class="form-control chosen-select" runat="server" />
                                        <span class="bwsprite error-icon hide"></span>
                                        <div class="bw-blackbg-tooltip hide">Please Select Area</div>
                                    </div>
                                    <input class="btn btn-orange margin-top15 cityAreaBtn" type="button" value="Show bikes">
                                </div>
                            </div>
                        </div>
                        <div id="searchBikeList" class="search-bike-list">
                            <div class="grid-12 alpha omega margin-top20 margin-bottom10">
                                <ul id="divSearchResult" data-bind="template: { name: 'listingTemp', foreach: bikes }">
                                </ul>
                            </div>
                            <div class="clear"></div>
                            <div style="text-align: center;">
                                <div id="NoBikeResults" class="hide">
                                    <img src="/UI/image/no_result_d.png" />
                                </div>
                                <div id="loading" class="hide">
                                    <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif" />
                                </div>
                            </div>
                        </div>

                        <script type="text/html" id="listingTemp">
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper position-rel">

                                        <div data-bind="visible : offers().length > 0" class="offers-tag-wrapper position-abt">
                                            <span><span  data-bind="text: offers().length.toString() + ((offers().length > 1) ? ' offers' : ' offer&nbsp;&nbsp;')"></span></span>
                                            <span class="offers-left-tag"></span>
                                        </div>

                                        <a data-bind="attr: { href: '/' + makeEntity.maskingName() + '-bikes/' + modelEntity.maskingName() + '/?vid=' + versionEntity.versionId() }, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Model_Click', 'lab': modelEntity.modelName() }); return true; }">
                                            <img class="lazy" data-bind="attr: { title: bikeName(), alt: bikeName(), src: '' }, lazyload: hostUrl() + '/310X174/' + originalImagePath() ">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper font14 text-light-grey">
                                        <div class="booking-list-item-details">
                                            <div class="bikeTitle margin-bottom10">
                                                <h3><a data-bind="attr: { href: '/' + makeEntity.maskingName() + '-bikes/' + modelEntity.maskingName() + '/?vid='+ versionEntity.versionId(), title: bikeName }, text: bikeName, click: function () { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'BookingListing_Page', 'act': 'Model_Click', 'lab': modelEntity.modelName }); return true; }"></a></h3>
                                            </div>
                                            <div class="bike-book-now-wrapper">
                                                <div class="bike-desc-details-wrapper">
                                                <p>BikeWale on-road price</p>

                                                <div class="margin-top10" data-bind="visible: discount() > 0">
                                                    <span class="bwsprite inr-sm-grey"></span>
                                                    <span class="font13 margin-right5 text-line-through" data-bind="CurrencyText: onRoadPrice()"></span>
                                                    <span>(<span class="text-red">
                                                        <span class="bwsprite inr-sm-red"></span>
                                                        <span class="font13" data-bind="CurrencyText: discount()"></span>&nbsp;Off
                                                    </span>)
                                                    </span>
                                                </div>

                                                <div class="text-bold text-default">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18" data-bind="CurrencyText: discountedPrice()"></span>
                                                </div>

                                                <div class="text-default margin-top5 margin-bottom5" data-bind="visible: offers().length > 0">
                                                <span class="margin-right5" data-bind="text: offers().length.toString() + ((offers().length > 1) ? ' offers' : ' offer')"></span>available
                                                <span class="text-link view-offers-target">view <span data-bind="text:(offers().length > 1) ? ' offers' : ' offer'"></span></span>
                                                </div>
                                                
                                                <p>Now book your bike online at <span class="text-default text-bold"><span class="bwsprite inr-sm"></span>&nbsp;<span data-bind="text: bookingAmount"></span></span></p>
                                                </div>
                                                <input type="button" class="book-now btn btn-white margin-top15" value="Book Now" data-bind="click: function () { registerPQ($data); }" />
                                            </div>
                                            <div id="offersPopup" class="text-center rounded-corner2">
                                                <div class="offers-popup-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                                <div class="icon-outer-container rounded-corner50">
                                                    <div class="icon-inner-container rounded-corner50">
                                                        <span class="bwsprite offers-box-icon margin-top20"></span>
                                                    </div>
                                                </div>
                                                <p class="font18 margin-top25 margin-bottom20 text-default">Available <span data-bind="text: (offers().length > 1) ? ' offers' : ' offer'"></span> on this bike</p>
                                                <ul class="offers-list-ul" data-bind="foreach : offers()">
                                                    <li><span data-bind="text : offerText()" />
                                                        <span class="tnc font9" data-bind="visible: isOfferTerms(), attr: { 'offerId': offerId }, click: function () { LoadTerms(offerId()); }">view terms</span>
                                                    </li>
                                                </ul>
                                                <input type="button" class="btn btn-orange margin-top15" value="Book Now" data-bind="click: function () { registerPQ($data); }" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </script>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!-- Terms and condition Popup start -->
            <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                <div class="fixed-close-btn-wrapper">
                    <div id="termsPopUpCloseBtn" class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
                </div>
                <h3>Terms and conditions</h3>
                <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                    <img class="lazy" data-original="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  src="" />
                </div>
                <div id="terms" class="breakup-text-container padding-bottom10 font14">
                </div>
                <div id='orig-terms' class='hide'>
                </div>
            </div>
            <!-- Terms and condition Popup Ends -->
        

        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <script src="<%= staticUrl %>/UI/src/lscache.min.js?<%= staticFileVersion%>"></script>
        <script>
            var $ddlCities = $("#bookingCitiesList"), $ddlAreas = $("#bookingAreasList");
            var selectedCityId = '<%= cityId %>', selectedAreaId = '<%= areaId %>';
            $("#Userlocation").text($ddlAreas.find("option:selected").text() + ", " + $ddlCities.find("option:selected").text());
            
            var key = "bCity_";
            lscache.setBucket('BLPage');
            $(function () {

                var selCityId = '<%= (cityId > 0)?cityId:0%>';
                var selAreaId = '<%= (areaId > 0)?areaId:0%>';

                $ddlCities.change(function () {
                    selCityId = $ddlCities.val();
                    $ddlAreas.empty();
                    selAreaId = "0";
                    if (!isNaN(selCityId) && selCityId != "0") {
                        if (!checkCacheCityAreas(selCityId)) {
                            $.ajax({
                                type: "GET",
                                url: "/api/BBAreaList/?cityId=" + selCityId,
                                dataType: 'json',
                                beforeSend: function () {
                                    return;
                                },
                                success: function (data) {
                                    lscache.set(key + selCityId, data.areas, 30);
                                    setOptions(data.areas);
                                },
                                complete: function (xhr) {
                                    if (xhr.status == 404 || xhr.status == 204) {
                                        lscache.set(key + selCityId, null, 30);
                                        setOptions(null);
                                    }
                                }
                            });
                        }
                        else {
                            data = lscache.get(key + selCityId.toString());
                            setOptions(data);
                        }
                    }
                    else {
                        setOptions(null);
                    }
                });

                $ddlAreas.change(function () {
                    if (!isNaN(selCityId) && selCityId != "0") {
                        selAreaId = $ddlAreas.find("option:selected").val();
                    }

                });

                $("input[type='button'].cityAreaBtn").click(function () {
                    if (!isNaN(selCityId) && selCityId != "0") {
                        if (!isNaN(selAreaId) && selAreaId != "0") {
                            var CookieValue = selCityId + "_" + $ddlCities.find("option:selected").text() + '_' + selAreaId + "_" + $ddlAreas.find("option:selected").text();
                            SetCookieInDays("location", CookieValue, 365);
                            window.location.href = "/bikebooking/bookinglisting.aspx"
                        }
                        else {
                            alert("Please select area !");
                        }
                    }
                    else {
                        alert("Please Select City !")
                    }
                });

            });

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                if (optList != null) {
                    $ddlAreas.append($('<option>').text(" Select Area ").attr({ 'value': "0" }));
                    $.each(optList, function (i, value) {
                        $ddlAreas.append($('<option>').text(value.areaName).attr('value', value.areaId));
                    });
                }

                $ddlAreas.trigger('chosen:updated');
                $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("No Areas available");
            }
        </script>

        <!-- #include file="/UI/includes/footerscript.aspx" -->

        <script>
            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlAreas.chosen({ no_results_text: "No matches found!!" });
            $('.chosen-container').attr('style', 'width:100%;');
            $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("Please Select City");
        </script>

        <script type="text/javascript" src="<%= staticUrl %>/UI/src/BikeBooking/bookinglisting.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var PQSourceId = '<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_NewBikeSearch%>';
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.BookingListing_Page%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.BookingListing_Page%>' };
        </script>
    </form>
</body>
</html>

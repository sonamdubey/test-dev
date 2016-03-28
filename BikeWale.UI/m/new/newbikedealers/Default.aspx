<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealerLocator.LocateNewBikesDealers" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = "New Bike Dealers in India - Locate Authorized Showrooms - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships, price quote";
        description = "Locate New bike dealers and authorized bike showrooms in India. Find new bike dealer information for more than 200 cities. Authorized company showroom information includes full address, phone numbers, email address, pin code etc.";
        canonical = "http://www.bikewale.com/new/locate-dealers/";
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        //menu = "10";
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        .locator-landing-banner {
            background: #8d8c8a url(http://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/m/m-booking-landing-banner.jpg) no-repeat center top;
            background-size: cover;
            height: 330px;
            padding-top: 1px;
        }

        .locator-search-container {
            margin-bottom: 20px;
            width: 100%;
        }

            .locator-search-container .form-control {
                padding: 8px;
            }

        .locator-search-brand, .locator-search-city {
            width: 100%;
            height: 40px;
        }

            .locator-search-brand select, .locator-search-city select, .locator-search-brand-form, .locator-search-city-form {
                width: 100%;
                height: 38px;
                color: #555;
                background: #fff;
            }

        .locator-search-brand-form, .locator-search-city-form {
            padding: 8px 25px 8px 8px;
            text-align: left;
            cursor: pointer;
            background: #fff url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/m/dropArrowBg.png?v1=19082015) no-repeat 96% 50%;
            text-align: left;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            border-radius: 2px;
        }

        .locator-search-brand select, .locator-search-city select {
            border: none;
            display: none;
        }

        .locator-search-city select {
            border: 1px solid #ccc;
        }

        .locator-submit-btn {
            width: 41%;
        }

            .locator-submit-btn.btn-lg {
                padding: 8px 24px;
            }

        .locator-search-container .errorIcon, .locator-search-container .errorText {
            display: none;
        }

        .brandlogosprite {
            background-image: url(http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/brand-type-sprite.png?22Mar2016v1);
            background-repeat: no-repeat;
            display: inline-block;
        }

        .brand-type-container li {
            display: inline-block;
            vertical-align: top;
            width: 96px;
            height: 70px;
            margin-bottom: 8px;
            text-align: center;
            font-size: 12px;
        }

        .brand-type-container a {
            text-decoration: none;
            color: #1a1a1a;
            display: inline-block;
        }

        .brand-type-title {
            display: block;
            text-transform: capitalize;
        }

        #locatorSearchBar.bwm-fullscreen-popup {
            padding: 0;
            background: #f5f5f5;
            z-index: 11;
            position: fixed;
            left: 100%;
            top: 0;
            overflow-y: scroll;
            width: 100%;
            height: 100%;
        }

        #locatorSearchBar li {
            border-top: 1px solid #ccc;
            font-size: 14px;
            padding: 15px 10px;
            color: #333333;
            cursor: pointer;
        }

            #locatorSearchBar li:hover {
                background: #ededed;
            }

        .booking-area-slider-wrapper {
            display: none;
        }

        .bwm-brand-city-box .back-arrow-box, .bwm-brand-city-box .cross-box {
            height: 30px;
            width: 40px;
            position: absolute;
            top: 5px;
            z-index: 11;
            cursor: pointer;
        }

        .bwm-brand-city-box span.back-long-arrow-left {
            position: absolute;
            top: 7px;
            left: 10px;
        }

        .bwm-brand-city-box .back-arrow-box {
            position: absolute;
            left: 5px;
        }

        .bwm-brand-city-box .form-control {
            padding: 10px 50px;
        }

        .activeBrand, .activeCity {
            font-weight: bold;
            background-color: #ddd;
        }
    </style>
</head>
<body class="bg-white">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container locator-landing-banner content-inner-block-20 text-center text-white">
                <h1 class="text-uppercase text-white padding-top40 padding-bottom15">Dealer locator</h1>
                <p class="margin-bottom25 font14">Locate dealers near you</p>
                <div class="locator-search-container">
                    <div class="locator-search-brand form-control-box margin-bottom10">
                        <div class="locator-search-brand-form"><span>Select brand</span></div>
                        <span class="bwmsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText"></div>
                    </div>
                    <div class="locator-search-city form-control-box">
                        <div class="locator-search-city-form border-solid-left"><span>Select city</span></div>
                        <span class="bwmsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText"></div>
                    </div>
                    <input type="button" class="btn btn-orange btn-lg font16 locator-submit-btn margin-top20" value="Submit" />
                </div>
            </div>
        </section>

        <div id="locatorSearchBar" class="bwm-fullscreen-popup">
            <div class="locator-brand-slider-wrapper bwm-brand-city-box form-control-box text-left">
                <div class="user-input-box">
                    <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                    <input class="form-control" type="text" id="locatorBrandInput" placeholder="Select brand" />
                </div>
                <ul id="sliderBrandList" class="slider-brand-list margin-top40">
                    <asp:Repeater ID="rptMakes" runat="server">
                        <ItemTemplate>
                            <li maskingname="<%# DataBinder.Eval(Container.DataItem,"MaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>"><%# DataBinder.Eval(Container.DataItem,"MakeName") %> </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div class="locator-city-slider-wrapper bwm-brand-city-box form-control-box text-left">
                <div class="user-input-box">
                    <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                    <input class="form-control" type="text" id="locatorCityInput" placeholder="Select City" />
                </div>
                <ul id="sliderCityList" class="slider-city-list margin-top40">
                    <asp:Repeater ID="rptCities" runat="server">
                        <ItemTemplate>
                            <li maskingname="<%# DataBinder.Eval(Container.DataItem,"CityMaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"CityId") %>" <%# ((DataBinder.Eval(Container.DataItem,"CityId")).ToString() != cityId.ToString())?string.Empty:"selected" %>><%# DataBinder.Eval(Container.DataItem,"CityName") %></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>

        <section>
            <div class="container text-center">
                <h2 class="margin-top25 margin-bottom20">Locate dealers by brand</h2>
                <div class="brand-type-container">
                    <ul class="text-center">
                        <asp:Repeater ID="rptPopularBrands" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>" data-original="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <ul class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                        <asp:Repeater ID="rptOtherBrands" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>" data-original="http://imgd2.aeplcdn.com/0x0/bw/static/sprites/m/brand-type-sprite.png?<%= staticFileVersion %>"></span>

                                        </span>
                                        <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="view-brandType text-center padding-bottom30">
                    <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16">View more brands</a>
                </div>
            </div>
        </section>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            var locatorSearchBar = $("#locatorSearchBar"),
                searchBrandDiv = $(".locator-search-brand"),
                searchCityDiv = $(".locator-search-city");
            searchBrandDiv.on('click', function () {
                $('.locator-city-slider-wrapper').hide();
                $('.locator-brand-slider-wrapper').show();
                locatorSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
                $(".user-input-box").animate({ 'left': '0px' }, 500);
                $("#locatorBrandInput").focus();
                hideError(searchBrandDiv.find("div.locator-search-brand-form"));
                appendHash("locatorsearch");
            });
            searchCityDiv.on('click', function () {
                if ($('#sliderCityList li').length > 0) {
                    $('.locator-brand-slider-wrapper').hide();
                    $('.locator-city-slider-wrapper').show();
                    locatorSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
                    $(".user-input-box").animate({ 'left': '0px' }, 500);
                    $("#locatorCityInput").focus();
                    hideError(searchCityDiv.find("div.locator-search-city-form"));
                    appendHash("locatorsearch");
                }
                else {
                    setError($("div.locator-search-brand-form"), "Please select brand!");
                }

            });

            var setError = function (element, msg) {
                element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
                element.siblings("div.errorText").text(msg);
            };

            var hideError = function (element) {
                element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
            };

            $("#sliderBrandList").on("click", "li", function () {
                var _self = $(this),
                    selectedElement = _self.text();
                setSelectedElement(_self, selectedElement);
                _self.addClass('activeBrand').siblings().removeClass('activeBrand');
                $("div.locator-search-brand-form").find("span").text(selectedElement);
                $(".user-input-box").animate({ 'left': '100%' }, 500);
            });

            $("#sliderCityList").on("click", "li", function () {
                var _self = $(this),
                    selectedElement = _self.text();
                setSelectedElement(_self, selectedElement);
                _self.addClass('activeCity').siblings().removeClass('activeCity');
                $(".user-input-box").animate({ 'left': '100%' }, 500);
                $("div.locator-search-city-form").find("span").text(selectedElement);
            });

            $(".bwm-brand-city-box .back-arrow-box").on("click", function () {
                locatorSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
                $(".user-input-box").animate({ 'left': '100%' }, 500);
            });

            function locatorSearchClose() {
                $(".bwm-brand-city-box .back-arrow-box").trigger("click");
            }

            $("#locatorBrandInput, #locatorCityInput").on("keyup", function () {
                locationFilter($(this));
            });

            function setSelectedElement(_self, selectedElement) {
                _self.parent().prev("input[type='text']").val(selectedElement);
                locatorSearchBar.addClass('open').animate({ 'left': '100%' }, 500);
            };

            $("#view-brandType").click(function () {
                $(".brandTypeMore").slideToggle();
                var targetLink = $(this);
                targetLink.text(targetLink.text() == 'View more brands' ? 'View less brands' : 'View more brands');
                if (targetLink.text() === "View more brands")
                    targetLink.attr("href", "#more");
                else
                    targetLink.attr("href", "javascript:void(0)");
            });
        </script>

        <script>
            var $ddlCities = $("#bookingCitiesList"), $ddlAreas = $("#bookingAreasList"), $liCities = $("#sliderCityList"), $liAreas = $("#sliderAreaList");
            var key = "dealerCitiesByMake_";
            lscache.flushExpired();
            lscache.setBucket('BLPage');
            var selCityId = '<%= (cityId > 0)?cityId:0%>';
            var selAreaId = '<%= (makeId > 0)?makeId:0%>';
            $(function () {

                if (($liCities.find("li.activeCity")).length > 0) {

                    $("div.booking-search-city-form span").text($liCities.find("li.activeCity:first").text());
                    if (($liAreas.find("li.activeArea")).length > 0) {
                        $("div.booking-search-area-form span").text($liAreas.find("li.activeArea:first").text());
                    }
                    else {
                        $("div.booking-search-area-form span").text("Select Area");
                    }

                }
                else {

                    $("div.booking-search-city-form span").text("Select City");
                    $("div.booking-search-area-form span").text("Please select city first");

                }




                $("#sliderCityList").on("click", "li", function () {
                    var _self = $(this),
                        selectedElement = _self.text();
                    setSelectedElement(_self, selectedElement);
                    _self.addClass('activeCity').siblings().removeClass('activeCity');
                    $("div.booking-search-city-form").find("span").text(selectedElement);
                    aid = _self.attr("cityId");
                    selCityId = aid;
                    $(".user-input-box").animate({ 'left': '100%' }, 500);
                    getAreas(aid);
                });

                $("#sliderAreaList").on("click", "li", function () {
                    var _self = $(this),
                        selectedElement = _self.text();
                    setSelectedElement(_self, selectedElement);
                    _self.addClass('activeArea').siblings().removeClass('activeArea');
                    if (!isNaN(selCityId) && selCityId != "0") {
                        selAreaId = _self.attr("areaId");
                    }
                    $(".user-input-box").animate({ 'left': '100%' }, 500);
                    $("div.booking-search-area-form").find("span").text(selectedElement);

                });

                $("input[type='button'].booking-landing-search-btn").click(function () {
                    if (!isNaN(selCityId) && selCityId != "0") {
                        if (!isNaN(selAreaId) && selAreaId != "0") {
                            var CookieValue = selCityId + "_" + $liCities.find("li.activeCity").text() + '_' + selAreaId + "_" + $liAreas.find("li.activeArea").text();
                            SetCookieInDays("location", CookieValue, 365);
                            window.location.href = "/m/bikebooking/bookinglisting.aspx"
                        }
                        else {
                            setError($("div.booking-search-area-form"), "Please select area !");
                        }
                    }
                    else {
                        setError($("div.booking-search-city-form"), "Please Select City !");
                    }
                });

            });

            function getAreas(cid) {
                $liAreas.empty();
                selAreaId = "0";
                if (!isNaN(selCityId) && selCityId != "0") {
                    if (!checkCacheCityAreas(cid)) {
                        $.ajax({
                            type: "GET",
                            url: "/api/BBAreaList/?cityId=" + cid,
                            contentType: "application/json",
                            beforeSend: function () {
                                $("div.booking-search-area-form span").text("Loading areas..");
                            },
                            success: function (data) {
                                lscache.set(key + cid, data.areas, 30);
                                $("div.booking-search-area-form span").text("Select an area");
                                setOptions(data.areas);
                            },
                            complete: function (xhr) {
                                if (xhr.status == 404 || xhr.status == 204) {
                                    $("div.booking-search-area-form span").text("No areas available");
                                    lscache.set(key + cid, null, 30);
                                    setOptions(null);

                                }
                            }
                        });
                    }
                    else {
                        $("div.booking-search-area-form span").text("Select an area")
                        data = lscache.get(key + cid);
                        setOptions(data);
                    }

                }
            }

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                if (optList != null) {

                    $.each(optList, function (i, value) {
                        $liAreas.append($('<li>').text(value.areaName).attr('areaId', value.areaId));
                    });
                }
                else {
                    $("div.booking-search-area-form span").text("No areas available");
                }
            }

            var setError = function (element, msg) {
                element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
                element.siblings("div.errorText").text(msg);
            };

            var hideError = function (element) {
                element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
            };

        </script>

        <script type="text/javascript">
            var bookingSearchBar = $("#bookingSearchBar"),
                searchCityDiv = $(".booking-search-city"),
                searchAreaDiv = $(".booking-search-area");
            searchCityDiv.on('click', function () {
                $('.booking-area-slider-wrapper').hide();
                $('.booking-city-slider-wrapper').show();
                bookingSearchBar.addClass('open').animate({
                    'left': '0px'
                }, 500);
                $(".user-input-box").animate({ 'left': '0px' }, 500);
                $("#bookingCityInput").focus();
                hideError(searchCityDiv.find("div.booking-search-city-form"));
                appendHash("bookingsearch");
            });
            searchAreaDiv.on('click', function () {
                if ($liAreas.find("li").length > 0) {
                    $('.booking-city-slider-wrapper').hide();
                    $('.booking-area-slider-wrapper').show();
                    bookingSearchBar.addClass('open').animate({
                        'left': '0px'
                    }, 500);
                    $(".user-input-box").animate({ 'left': '0px' }, 500);
                    $("#bookingAreaInput").focus();
                    hideError(searchAreaDiv.find("div.booking-search-area-form"));
                    appendHash("bookingsearch");
                }
                else {
                    setError($("div.booking-search-city-form"), "Please Select City!");
                }


            });
            $(".bwm-city-area-box .back-arrow-box").on("click", function () {
                bookingSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
                $(".user-input-box").animate({ 'left': '100%' }, 500);
            });

            function bookingSearchClose() {
                $(".bwm-city-area-box .back-arrow-box").trigger("click");
            }

            $("#bookingCityInput, #bookingAreaInput").on("keyup", function () {
                locationFilter($(this));
            });

            function setSelectedElement(_self, selectedElement) {
                _self.parent().prev("input[type='text']").val(selectedElement);
                bookingSearchBar.addClass('open').animate({
                    'left': '100%'
                }, 500);
            };
        </script>  

        
    </form>
</body>
</html>

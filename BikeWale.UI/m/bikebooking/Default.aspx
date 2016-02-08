﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.bikebooking.Default" %>
<%@ Register Src="~/m/controls/UsersTestimonials.ascx" TagPrefix="BW" TagName="UsersTestimonials" %>
<!DOCTYPE html>

<html>
<head>
    <title></title>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-booking-landing.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    <script>
    </script>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <script type="text/javascript">$("header").find(".rightfloat ").hide();</script>
        <section>
            <div class="container booking-landing-banner content-inner-block-10 text-center text-white">
                <h1 class="text-uppercase text-white padding-top40 padding-bottom15">Book your dream bike</h1>
                <p class="font14">Online booking is now available in</p>
                <p class="font16 text-bold margin-bottom20">Mumbai, Pune and Bengaluru</p>
                <div class="booking-landing-search-container">
                    <div class="booking-search-city form-control-box">
                        <div class="booking-search-city-form"><span>City</span></div>
                        <asp:DropDownList ID="bookingCitiesList" class="form-control rounded-corner-no-right no-border" runat="server" />
                    </div>
                    <div class="booking-search-area form-control-box">
                        <div class="booking-search-area-form border-solid-left"><span>Area</span></div>
                        <asp:DropDownList ID="bookingAreasList" class="form-control rounded-corner-no-left no-border" runat="server" />
                    </div>
                    <input type="button" class="btn btn-orange btn-lg font16 booking-landing-search-btn margin-top20" value="Search" />
                </div>
            </div>
        </section>

        <div id="bookingSearchBar" class="bwm-fullscreen-popup">
            <div class="booking-city-slider-wrapper bwm-city-area-box form-control-box text-left">
                <span class="back-arrow-box">
                    <span class="bwmsprite back-long-arrow-left"></span>
                </span>
                <input class="form-control" type="text" id="bookingCityInput" placeholder="Select City" />
                <ul id="sliderCityList" class="sliderCityList">
                   <%= cityListData %>
                </ul>
            </div>
            <div class="booking-area-slider-wrapper bwm-city-area-box form-control-box text-left">
                <span class="back-arrow-box">
                    <span class="bwmsprite back-long-arrow-left"></span>
                </span>
                <input class="form-control" type="text" id="bookingAreaInput" placeholder="Select Area" />
                <ul id="sliderAreaList" class="sliderAreaList">
                    <%= areaListData %>
                </ul>
            </div>
        </div>

        <section>
            <div class="container text-center">
                <h2 class="margin-top25 margin-bottom20">Comforts of booking online</h2>
                <div class="swiper-container font18">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <div class="benefit-icon-outer-container rounded-corner50percent">
                                <div class="benefit-icon-inner-container rounded-corner50percent">
                                    <span class="benefits-icon-span booking-landing-sprite benefit-offers-icon margin-top20"></span>
                                </div>
                            </div>
                            <div class="margin-top20">Exclusive<br />offers</div>
                        </div>
                        <div class="swiper-slide">
                            <div class="benefit-icon-outer-container rounded-corner50percent">
                                <div class="benefit-icon-inner-container rounded-corner50percent">
                                    <span class="benefits-icon-span booking-landing-sprite benefit-dealer-icon margin-top25"></span>
                            </div>
                        </div>
                        <div class="margin-top20">Save on<br />dealer visits</div>
                        </div>
                        <div class="swiper-slide">
                            <div class="benefit-icon-outer-container rounded-corner50percent">
                                <div class="benefit-icon-inner-container rounded-corner50percent">
                                    <span class="benefits-icon-span booking-landing-sprite benefit-assistance-icon margin-top15"></span>
                                </div>
                            </div>
                            <div class="margin-top20">Complete<br />buying assistance</div>
                        </div>
                        <div class="swiper-slide">
                            <div class="benefit-icon-outer-container rounded-corner50percent">
                                <div class="benefit-icon-inner-container rounded-corner50percent">
                                    <span class="benefits-icon-span booking-landing-sprite benefit-cancellation-icon margin-top20"></span>
                                </div>
                            </div>
                            <div class="margin-top20">Easy<br />cancellation</div>
                        </div>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>
            </div>
        </section>

        <section>
            <div class="avail-offers-container container bg-white">
                <h2 class="padding-top25 text-center padding-bottom20">Offers you can avail</h2>
                <ul>
                    <li>
                        <span class="booking-icon-container">
                            <span class="booking-landing-sprite offer-insurance-icon"></span>
                        </span>
                        <span class="booking-text-container">
                            <span class="font16 text-bold">Insurance</span><br />
                            <span class="font14 text-light-grey">Get free comprehensive insurance worth Rs.1000</span>
                        </span>
                    </li>
                    <li>
                        <span class="booking-icon-container">
                            <span class="booking-landing-sprite offer-voucher-icon"></span>
                        </span>
                        <span class="booking-text-container">
                            <span class="font16 text-bold">Gift Voucher</span><br />
                            <span class="font14 text-light-grey">Get free Flipkart voucher worth Rs.1000</span>
                        </span>
                    </li>
                    <li>
                        <span class="booking-icon-container">
                            <span class="booking-landing-sprite offer-doorstep-icon"></span>
                        </span>
                        <span class="booking-text-container">
                            <span class="font16 text-bold">Doorstep service</span><br />
                            <span class="font14 text-light-grey">Get free document collection & bike delivery</span>
                        </span>
                    </li>
                </ul>
            </div>
        </section>
        
        <section id="bookingWorksWrapper">
            <div class="booking-work-container container text-white padding-left20 padding-right20">
                <h2 class="padding-top25 text-center text-white padding-bottom25">How it works?</h2>
                <ul>
                    <li>
                        <span class="booking-icon-container text-center">
                            <span class="booking-landing-sprite work-payment-icon"></span>
                        </span>
                        <span class="booking-text-container padding-left10">
                            <span class="font16 text-bold">Book your bike by paying booking amount</span>
                        </span>
                    </li>
                    <li><span class="booking-landing-sprite work-arrow-icon"></span></li>
                    <li>
                        <span class="booking-icon-container text-center">
                            <span class="booking-landing-sprite work-document-icon"></span>
                        </span>
                        <span class="booking-text-container padding-left10">
                            <span class="font16 text-bold">Provide documents and payment at dealership</span>
                        </span>
                    </li>
                    <li><span class="booking-landing-sprite work-arrow-icon"></span></li>
                    <li>
                        <span class="booking-icon-container text-center">
                            <span class="booking-landing-sprite work-delivery-icon"></span>
                        </span>
                        <span class="booking-text-container padding-left10">
                            <span class="font16 text-bold">Get your bike delivered & avail offers</span>
                        </span>
                    </li>
                </ul>
            </div>
        </section>

        
         <% if (ctrlUsersTestimonials.FetchedCount > 0 )
           { %>
        <section class="bg-white">
            <div id="testimonialWrapper" class="container padding-bottom20">
                <h2 class="text-bold text-center padding-top30 margin-bottom15 font24">What do our customers say</h2>
                <div class="swiper-container text-center">
                    <div class="swiper-wrapper padding-bottom20">
                        <BW:UsersTestimonials ID="ctrlUsersTestimonials" runat="server"></BW:UsersTestimonials>
                    </div>
                    <div class="swiper-pagination"></div>
                </div>
            </div>
        </section>
        <%
           }        
        %>


        <section>
            <div id="faqsWraper" class="container margin-bottom30">
                <h2 class="padding-top25 text-center padding-bottom20">FAQs</h2>
                <div class="swiper-container padding-left10 margin-bottom15">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <p class="font16">How can I book a bike on BikeWale?</p>
                            <p class="font14 text-light-grey">To book a bike, you have to pay a fixedbooking amount online mentioned against the...<a href="/m/faq.aspx#2">read more</a></p>
                        </div>
                        <div class="swiper-slide">
                            <p class="font16">Where do I have to pay the balance amount? How much will it be?</p>
                            <p class="font14 text-light-grey">You will pay the balance amount directly to the assigned...<a href="/m/faq.aspx#14">read more</a></p>
                        </div>
                        <div class="swiper-slide">
                            <p class="font16">How will I get the benefits of the offers?</p>
                            <p class="font14 text-light-grey">Depending upon the offer, you will get the benefit of some offers directly at the...<a href="/m/faq.aspx#16">read more</a></p>
                        </div>
                    </div>
                </div>
                <p class="padding-left10 padding-right10 font14 text-center">We’re here to help. Read our <a href="/faq.aspx">FAQs</a>, <a href="mailto:contact@bikewale.com">email</a> or call us on <a href="tel:18001208300" class="text-grey text-bold">1800 120 8300</a></p>
            </div>
        </section>
        
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/lscache.min.js?<%= staticFileVersion%>"></script>

        <script>
            var $ddlCities = $("#bookingCitiesList"), $ddlAreas = $("#bookingAreasList"), $liCities = $("#sliderCityList"), $liAreas = $("#sliderAreaList");
            var key = "bCity_";
            lscache.setBucket('BLPage');  

            $(function () {

                var selCityId = '<%= (cityId > 0)?cityId:0%>';
                var selAreaId = '<%= (areaId > 0)?areaId:0%>';

                $("div.booking-search-city-form span").text($liCities.find("li.activeCity:first").text());
                $("div.booking-search-area-form span").text($liAreas.find("li.activeArea:first").text());

                $("#sliderCityList").on("click", "li", function () {
                    var _self = $(this),
                        selectedElement = _self.text();
                    setSelectedElement(_self, selectedElement);
                    _self.addClass('activeCity').siblings().removeClass('activeCity');
                    $("div.booking-search-city-form").find("span").text(selectedElement);
                    aid = _self.attr("cityId");
                    selCityId = aid;
                    getAreas(aid);
                });

                $("#sliderAreaList").on("click", "li", function () {                     
                    var _self = $(this),
                        selectedElement = _self.text();
                    setSelectedElement(_self, selectedElement);
                    _self.addClass('activeArea').siblings().removeClass('activeArea');
                    if (!isNaN(selCityId) && selCityId != "0") {
                        selAreaId =_self.attr("areaId");
                    }

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
                            alert("Please select area !");
                        }
                    }
                    else {
                        alert("Please Select City !")
                    }
                });

            });

            function getAreas(cid)
            {                
                $liAreas.empty();
                if (cid > 0) {
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
                if (optList != null)
                {
                    $liAreas.append($('<li>').text(" Select Area ").attr({ 'areaId': "0" }));
                    $.each(optList, function (i, value) {
                        $liAreas.append($('<li>').text(value.areaName).attr('areaId', value.areaId));
                    });
                }
                else {
                    $liAreas.append($('<li>').text(" No areas available ").attr({ 'areaId': "0" }));
                }
            }


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
            });
            searchAreaDiv.on('click', function () {
                $('.booking-city-slider-wrapper').hide();
                $('.booking-area-slider-wrapper').show();
                bookingSearchBar.addClass('open').animate({
                    'left': '0px'
                }, 500);

            });
            $(".bwm-city-area-box .back-arrow-box").on("click", function () {
                bookingSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
            });
           

            function setSelectedElement(_self, selectedElement) {
                _self.parent().prev("input[type='text']").val(selectedElement);
                bookingSearchBar.addClass('open').animate({
                    'left': '100%'
                }, 500);
            };
        </script>
        
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        
    </form>
</body>
</html>
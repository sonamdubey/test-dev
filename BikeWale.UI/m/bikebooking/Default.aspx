﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.bikebooking.Default" %>
<%@ Register Src="~/m/controls/UsersTestimonials.ascx" TagPrefix="BW" TagName="UsersTestimonials" %>
<!DOCTYPE html>

<html>
<head>
    <title></title>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-booking-landing.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
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
                <ul class="sliderCityList">
                    <li>City 1</li>
                    <li>City 2</li>
                    <li>City 3</li>
                    <li>City 4</li>
                </ul>
            </div>
            <div class="booking-area-slider-wrapper bwm-city-area-box form-control-box text-left">
                <span class="back-arrow-box">
                    <span class="bwmsprite back-long-arrow-left"></span>
                </span>
                <input class="form-control" type="text" id="bookingAreaInput" placeholder="Select Area" />
                <ul class="sliderAreaList">
                    <li>Area 1</li>
                    <li>Area 2</li>
                    <li>Area 3</li>
                    <li>Area 4</li>
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
            <div class="booking-work-container container text-white">
                <h2 class="padding-top25 text-center text-white padding-bottom20">How it works?</h2>
                
            </div>
        </section>

        
         <% if (ctrlUsersTestimonials.FetchedCount > 0 )
           { %>
        <section class="bg-white">
            <div id="testimonialWrapper" class="container margin-bottom10">
                <h2 class="text-bold text-center padding-top30 margin-bottom20 font24">What do our customers say</h2>
                <div class="swiper-container text-center">
                    <div class="swiper-wrapper margin-bottom10">
                        <BW:UsersTestimonials ID="ctrlUsersTestimonials" runat="server"></BW:UsersTestimonials>
                    </div>
                </div>
            </div>
        </section>
        <%
           }        
        %>


        <section>
            <div id="faqsWraper" class="container margin-bottom30">
                <h2 class="padding-top25 text-center padding-bottom20">FAQs</h2>
                <div class="swiper-container">
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
            </div>
        </section>
        
        <script src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/lscache.min.js?<%= staticFileVersion%>"></script>

        <script>
            var $ddlCities = $("#bookingCitiesList"), $ddlAreas = $("#bookingAreasList");
            var key = "bCity_";
            lscache.setBucket('BLPage');
            $(function () {

                var selCityId = 0;
                if (!($ddlAreas.find("option")).length > 0)
                    $ddlAreas.append($('<option>').text(" Please select city first ").attr({ 'value': "0" }));

                $ddlCities.change(function () {
                    selCityId = parseInt($ddlCities.val(), 16);
                    $ddlAreas.empty();
                    if (selCityId > 0) {
                        if (!checkCacheCityAreas(selCityId)) {
                            $.ajax({
                                type: "GET",
                                url: "/api/BBAreaList/?cityId=" + selCityId,
                                contentType: "application/json",
                                beforeSend: function () {

                                },
                                success: function (data) {
                                    lscache.set(key + selCityId.toString(), data.areas, 30);
                                    setOptions(data.areas);
                                },
                                complete: function (xhr) {
                                    if (xhr.status == 404 || xhr.status == 204) {
                                        lscache.set(key + selCityId.toString(), null , 30);
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
                });

            });

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                if (optList != null)
                {
                    $ddlAreas.append($('<option>').text(" Select Area ").attr({ 'value': "0" }));
                    $.each(optList, function (i, value) {
                        $ddlAreas.append($('<option>').text(value.areaName).attr('value', value.areaId));
                    });
                }
                else {
                    $ddlAreas.append($('<option>').text(" No areas available ").attr({ 'value': "0" }));
                }
            }


        </script>
        
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
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
            $(".sliderCityList").on("click", "li", function () {
                var _self = $(this),
                    selectedElement = _self.text();
                setSelectedElement(_self, selectedElement)
                $(".booking-landing-search-container .booking-search-city-form").find("span").text(selectedElement);
            });

            $(".sliderAreaList").on("click", "li", function () {
                var _self = $(this),
                    selectedElement = _self.text();
                setSelectedElement(_self, selectedElement)
                $(".booking-landing-search-container .booking-search-area-form").find("span").text(selectedElement);
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
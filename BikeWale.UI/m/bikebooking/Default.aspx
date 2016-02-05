<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.bikebooking.Default" %>
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
                        <asp:DropDownList ID="bookingCitiesList" class="form-control rounded-corner-no-right no-border" runat="server" />
                    </div>
                    <div class="booking-search-area form-control-box">
                        <asp:DropDownList ID="bookingAreasList" class="form-control rounded-corner-no-left no-border" runat="server" />
                    </div>
                    <input type="button" class="btn btn-orange btn-lg font16 booking-landing-search-btn margin-top20" value="Search" />
                </div>
            </div>
        </section>
        
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

         <% if (ctrlUsersTestimonials.FetchedCount > 0 )
           { %>
        <section>
            <div id="testimonialWrapper" class="container margin-bottom10">
                <h2 class="text-bold text-center margin-top30 margin-bottom20 font24">What do our customers say</h2>
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
    </form>
</body>
</html>
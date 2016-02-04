<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bikewale.BikeBooking.Default" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bookinglanding.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
    <%
        isAd970x90Shown = false;
    %>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="booking-landing-banner">    	
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Book your dream bike</h1>
                    <p class="font20">Online booking is now available in</p>
                    <p class="font22 text-bold margin-bottom45">Mumbai, Pune and Bengaluru</p>
                    <div class="booking-landing-search-container">

                    </div>
                </div>
            </div>
        </header>


        <section>
            <div class="container margin-bottom30 margin-top30">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <asp:DropDownList ID="bookingCitiesList" class="form-control" runat="server" />
                        <asp:DropDownList ID="bookingAreasList" class="form-control" runat="server" />
                        <input type="button" class="btn btn-red" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div id="onlineBenefitsWrapper" class="container margin-bottom40">
                <div class="grid-12 text-center font18">
                    <h2 class="text-bold text-center margin-top40 margin-bottom30 font28">Benefits of booking online</h2>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-offers-icon margin-top25"></span>
                            </div>
                        </div>
                        <div class="margin-top20">Exclusive<br />offers</div>
                    </div>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-dealer-icon margin-top30"></span>
                            </div>
                        </div>
                        <div class="margin-top20">Save on<br />dealer visits</div>
                    </div>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-assistance-icon margin-top25"></span>
                            </div>
                        </div>
                        <div class="margin-top20">Complete<br />buying assistance</div>
                    </div>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-cancellation-icon margin-top25"></span>
                            </div>
                        </div>
                        <div class="margin-top20">Easy<br />cancellation</div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="bg-white">
            <div class="container padding-bottom70">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom50 font28">Offers you can avail</h2>
                    <div class="grid-4">
                        <div class="inline-block offers-icon-wrapper">
                            <span class="booking-landing-sprite offer-insurance-icon"></span>
                        </div>
                        <div class="inline-block offers-desc-wrapper">
                            <h2 class="text-default text-bold margin-bottom5">Insurance</h2>
                            <p class="font14 text-light-grey">Get free comprehensive<br />insurance worth Rs.1000</p>
                        </div>
                    </div>
                    <div class="grid-4">
                        <div class="inline-block offers-icon-wrapper">
                            <span class="booking-landing-sprite offer-voucher-icon"></span>
                        </div>
                        <div class="inline-block offers-desc-wrapper">
                            <h2 class="text-default text-bold margin-bottom5">Gift Voucher</h2>
                            <p class="font14 text-light-grey">Get free Flipkart voucher<br />worth Rs.1000</p>
                        </div>
                    </div>
                    <div class="grid-4">
                        <div class="inline-block offers-icon-wrapper">
                            <span class="booking-landing-sprite offer-doorstep-icon"></span>
                        </div>
                        <div class="inline-block offers-desc-wrapper">
                            <h2 class="text-default text-bold margin-bottom5">Doorstep service</h2>
                            <p class="font14 text-light-grey">Get free document collection &<br />bike delivery</p>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section id="bookingWorksWrapper">
            <div class="container">
                <div class="grid-12">

                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="bg-white">
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom50 font28">Offers you can avail</h2>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script>
            var $ddlCities = $("#bookingCitiesList"), $ddlAreas = $("#bookingAreasList");

            $(function () {
                $ddlCities.change(function () {
                    selCityId = parseInt($ddlCities.val(),16);
                    $ddlAreas.empty();
                    if (selCityId > 0)
                    {
                        $.ajax({
                            type: "GET",
                            url: "/api/BBAreaList/?cityId=" + selCityId,
                            contentType: "application/json",
                            beforeSend: function () {

                            },
                            success: function (data) {
                                $ddlAreas.append($('<option>').text("----Select Area----").attr({ 'value': "0" }));
                                $.each(data.areas, function (i, value) {
                                    $ddlAreas.append($('<option>').text(value.areaName).attr('value', value.areaId));
                                });
                            },
                            complete: function (xhr) {
                                if (xhr.status == 404 || xhr.status == 204) {
                                    $ddlAreas.append($('<option>').text(" No Areas available").attr({ 'value': "0" }));
                                }
                            }
                        });
                    }
                });
            });
        </script>

    </form>
</body>
</html>


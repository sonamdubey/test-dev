﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.ModelSpecsFeatures" EnableViewState="false" %>

<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/m/controls/GenericBikeInfoControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%
        description = String.Format("Know more about {0} Specifications and Features. See details about mileage, engine displacement, power, kerb weight and other specifications.", bikeName);
        title = String.Format("{0} Specifications and Features - Check out mileage and other technical specifications - BikeWale", bikeName);
        canonical = String.Format("https://www.bikewale.com/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        keywords = string.Format("{0} specifications, {0} specs, {0} features, {0} mileage, {0} fuel efficiency", bikeName);
        EnableOG = true;
        OGImage = modelImage;

    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        @charset "utf-8";

        .content-inner-block-1420 {
            padding: 14px 20px;
        }

        .content-inner-block-120 {
            padding: 10px 20px 0;
        }

        .text-dark-black {
            color: #1a1a1a;
        }

        .text-truncate {
            width: 100%;
            text-align: left;
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
        }

        .model-price-content {
            width: 110px;
        }

        .model-area-content {
            width: 60%;
            position: relative;
            top: 6px;
        }

        .specs-features-wrapper {
            height: 44px;
        }

        #specsFeaturesTabsWrapper {
            width: 100%;
            background: #fff;
            z-index: 3;
            display: block;
            border-bottom: 1px solid #e2e2e2;
            overflow-x: auto;
        }

        .model-specs-features-tabs-wrapper {
            display: table;
            background: #fff;
        }

            .model-specs-features-tabs-wrapper li {
                padding: 10px 20px;
                display: table-cell;
                text-align: center;
                white-space: nowrap;
                font-size: 14px;
                color: #82888b;
            }

                .model-specs-features-tabs-wrapper li.active {
                    border-bottom: 3px solid #ef3f30;
                    font-weight: bold;
                    color: #4d5057;
                }

        .border-divider {
            border-top: 1px solid #e2e2e2;
        }

        .specs-features-list {
            overflow: hidden;
        }

            .specs-features-list li {
                margin-bottom: 20px;
            }

            .specs-features-list p {
                width: 50%;
                float: left;
                text-align: left;
                text-overflow: ellipsis;
                white-space: nowrap;
                overflow: hidden;
            }

        .specs-features-label {
            color: #82888b;
        }

        .specs-features-value {
            padding-left: 20px;
            font-weight: bold;
        }

        .fixed-topNav {
            position: fixed;
            top: 0;
            left: 0;
        }

        .float-button {
            background-color: #fff;
            padding: 10px;
        }

            .float-button.float-fixed {
                position: fixed;
                bottom: 0;
                z-index: 8;
                left: 0;
                right: 0;
                background: #f5f5f5;
            }

        .star-span {
            position: relative;
            top: -2px;
            transform: scale(.9);
        }

        .border-light-left {
            border-left: 1px solid #ededed;
        }

        .star-icon {
            width: 26px;
            height: 25px;
            vertical-align: middle;
            -webkit-transform: scale(.8);
            -moz-transform: scale(.8);
            -o-transform: scale(.8);
            transform: scale(.8);
        }

            .star-icon.star-size-16 {
                -webkit-transform: scale(.6);
                -moz-transform: scale(.6);
                -o-transform: scale(.6);
                transform: scale(.6);
            }

        .rate-count-1 .star-icon {
            background-position: 0 -667px;
        }

        .rate-count-2 .star-icon {
            background-position: -38px -667px;
        }

        .rate-count-3 .star-icon {
            background-position: 0 -278px;
        }

        .rate-count-4 .star-icon {
            background-position: -74px -667px;
        }

        .rate-count-5 .star-icon {
            background-position: -110px -667px;
        }

        .rate-count-5 {
            color: #198D55;
        }

        .rate-count-4 {
            color: #13B65D;
        }

        .rate-count-3 {
            color: #EFD700;
        }

        .rate-count-2 {
            color: #FD971C;
        }

        .rate-count-1 {
            color: #EF3F30;
        }

        .review-left-divider {
            position: relative;
            margin-left: 15px;
            padding-left: 15px;
        }

            .review-left-divider:before {
                content: "";
                position: absolute;
                top: 4px;
                left: 0;
                width: 1px;
                height: 12px;
                border-left: 1px solid #e2e2e2;
            }

        .breadcrumb {
            margin: 10px;
            padding: 10px;
            background: #eee;
        }

        .breadcrumb-title {
            display: block;
            font-size: 12px;
            color: #82888B;
            padding-bottom: 5px;
        }

        .breadcrumb li {
            position: relative;
            display: inline-block;
            vertical-align: middle;
            margin-right: 5px;
            margin-bottom: 10px;
        }

        .breadcrumb-link::after {
            content: '';
            height: 5px;
            width: 5px;
            border: solid grey;
            border-width: 0px 1px 1px 0px;
            -webkit-transform: rotate(320deg);
            -moz-transform: rotate(45deg);
            -o-transform: rotate(45deg);
            transform: rotate(45deg);
            display: inline-block;
            position: absolute;
            top: 7px;
            right: 0px;
        }

        .breadcrumb-link__label {
            padding-right: 12px;
        }
    </style>

</head>
<body>
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section class="bg-white box-shadow margin-bottom10">
            <div id="modelPriceDetails" class="content-inner-block-120">
                <h1 class="margin-bottom5"><%= bikeName %> Specifications and Features</h1>

                <% if (price > 0)
                    { %>
                <p class="font20 text-bold model-price-content leftfloat">
                    <span class="bwmsprite inr-sm-icon"></span>&nbsp;<%= Bikewale.Utility.Format.FormatPrice(price.ToString())%>
                </p>
                <% }
                    else
                    { %>
                <p>Price not available</p>
                <% } %>
                <% if (price > 0)
                    { %>
                <%if (isDiscontinued)
                    { %>
                <p class="font14 text-truncate text-light-grey model-area-content leftfloat">Last known Ex-showroom Price</p>
                <% }
                    else if (!IsExShowroomPrice)
                    { %>
                <p class="font14 text-truncate text-light-grey model-area-content leftfloat">On-road price in <%= string.IsNullOrEmpty(areaName) ? cityName : string.Format("{0}, {1}", areaName, cityName)%></p>
                <% }
                    else
                    { %>
                <p class="font14 text-truncate text-light-grey model-area-content leftfloat">Ex-showroom price in <%= string.IsNullOrEmpty(areaName) ? cityName : string.Format("{0}, {1}", areaName, cityName)%></p>
                <%} %>
                <%} %>
                <div class="clear"></div>
            </div>

            <% if (specs != null)
                { %>
            <div class="specs-features-wrapper">
                <div id="specsFeaturesTabsWrapper">
                    <ul class="model-specs-features-tabs-wrapper">
                        <li class="active" data-tabs="#specs">Specifications</li>
                        <li data-tabs="#features">Features</li>
                    </ul>
                </div>
            </div>

            <div id="specsFeaturesDetailsWrapper" class="padding-right20 padding-left20 font14">
                <div id="specs" class="bw-model-tabs-data padding-top15">
                    <h2 class="margin-bottom15">Specifications</h2>
                    <h3 class="margin-bottom20">Engine and Transmission</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Displacement</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Displacement,"cc") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Cylinders</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Cylinders) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Max Power</p>
                            <p class="specs-features-value">
                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.MaxPower, "bhp", 
                                                                    specs.MaxPowerRPM, "rpm") %>
                            </p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Maximum Torque</p>
                            <p class="specs-features-value">
                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.MaximumTorque, "Nm",
                                                                    specs.MaximumTorqueRPM, "rpm") %>
                            </p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Bore</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Bore, "mm") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Stroke</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Stroke, "mm") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Valves Per Cylinder</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ValvesPerCylinder) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Delivery System</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelDeliverySystem) %></p>
                            <div class="clear"></div>
                        </li>
                    </ul>

                    <h3 class="margin-bottom20">Brakes, Wheels and Suspension</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Brake Type</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.BrakeType) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Front Disc</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FrontDisc) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Front Disc/Drum Size</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FrontDisc_DrumSize, "mm") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Rear Disc</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.RearDisc) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Rear Disc/Drum Size</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.RearDisc_DrumSize, "mm") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Calliper Type</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.CalliperType) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Wheel Size</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.WheelSize, "inches") %></p>
                            <div class="clear"></div>
                        </li>
                    </ul>

                    <h3 class="margin-bottom20">Dimensions and Chassis</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Kerb Weight</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.KerbWeight, "kg") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Overall Length</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.OverallLength, "mm") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Overall Width</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.OverallWidth, "mm") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Overall Height</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.OverallHeight, "mm") %></p>
                            <div class="clear"></div>
                        </li>
                    </ul>

                    <h3 class="margin-bottom20">Fuel efficiency and Performance</h3>
                    <ul class="specs-features-list margin-bottom5">
                        <li>
                            <p class="specs-features-label">Fuel Tank Capacity</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelTankCapacity, "litres") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Reserve Fuel Capacity</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ReserveFuelCapacity, "litres") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Efficiency Overall</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelEfficiencyOverall, "kmpl") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Efficiency Range</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelEfficiencyRange, "km") %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Top Speed</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TopSpeed, "kmph") %></p>
                            <div class="clear"></div>
                        </li>
                    </ul>
                </div>
                <div class="border-divider"></div>

                <div id="features" class="padding-top15 bw-model-tabs-data">
                    <h2 class="margin-bottom20">Features</h2>
                    <ul class="specs-features-list">
                        <li>
                            <p class="specs-features-label">Speedometer</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Speedometer) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Fuel Guage</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelGauge) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Tachometer Type</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Tachometer) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Digital Fuel Guage</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.DigitalFuelGauge) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Tripmeter</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Tripmeter) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Electric Start</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ElectricStart) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Tachometer</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Tachometer) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Shift Light</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ShiftLight) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">No. of Tripmeters</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.NoOfTripmeters) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Tripmeter Type</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TripmeterType) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Low Fuel Indicator</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.LowFuelIndicator) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Low Oil Indicator</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.LowOilIndicator) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Low Battery Indicator</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.LowBatteryIndicator) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Pillion Seat</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.PillionSeat) %></p>
                            <div class="clear"></div>
                        </li>
                        <li>
                            <p class="specs-features-label">Pillion Footrest</p>
                            <p class="specs-features-value"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.PillionFootrest) %></p>
                            <div class="clear"></div>
                        </li>
                    </ul>
                </div>

            </div>

            <div id="specsFeaturesFooter"></div>
            <% } %>
        </section>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin margin-bottom10">
                <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />
            </div>
        </section>

        <section>
            <div class="breadcrumb">
                <span class="breadcrumb-title">You are here:</span>
                <ul>
                    <% var url = string.Format("{0}/m/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl); %>

                    <li>
                        <a class="breadcrumb-link" href="/m/" title="Home">
                            <span class="breadcrumb-link__label" itemprop="name">Home</span>
                        </a>
                    </li>
                    <li>
                        <a class="breadcrumb-link" href="/m/<%= makeMaskingName %>-bikes/" title="<%= makeName %> Bikes">
                            <span class="breadcrumb-link__label" itemprop="name"><%= makeName %> Bikes</span>
                        </a>
                    </li>
                    <% if (IsScooter && !IsScooterOnly)
                        { %>
                    <li>
                        <a class="breadcrumb-link" href="/m/<%= makeMaskingName %>-scooters/" title="<%= String.Format("{0} {1}", makeName, modelName) %>">
                            <span class="breadcrumb-link__label" itemprop="name"><%= makeName %> Scooters</span>
                        </a>
                    </li>
                    <%  }
                    %>
                    <li>
                        <a class="breadcrumb-link" href="/m/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" title="<%= String.Format("{0} {1}", makeName, modelName) %>">
                            <span class="breadcrumb-link__label" itemprop="name"><%= String.Format("{0} {1}", makeName, modelName) %> </span>
                        </a>
                    </li>
                    <li>
                        <span class="breadcrumb-link__label">Specifications & Features</span>
                    </li>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>


        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            ga_pg_id = "15";
            $(document).ready(function () {
                var $window = $(window),
                topNavBarWrapper = $('.specs-features-wrapper'),
                topNavBar = $('#specsFeaturesTabsWrapper'),
                specsFeaturesFooter = $('#specsFeaturesFooter'),
                floatButton = $('.float-button'),
                footer = $('footer'),
                floating = false;

                if (floatButton.length != 0) {
                    floating = true;
                }

                $window.scroll(function () {
                    var windowScrollTop = $window.scrollTop(),
                        topNavBarWrapperOffset = topNavBarWrapper.offset(),
                        topNavBarOffset = topNavBar.offset(),
                        specsFeaturesFooterOffset = specsFeaturesFooter.offset();

                    if (windowScrollTop > topNavBarOffset.top) {
                        topNavBar.addClass('fixed-topNav');
                    }

                    else if (windowScrollTop < topNavBarWrapperOffset.top) {
                        topNavBar.removeClass('fixed-topNav');
                    }

                    if (topNavBar.hasClass('fixed-topNav')) {
                        if (windowScrollTop > specsFeaturesFooterOffset.top - topNavBar.height())
                            topNavBar.removeClass('fixed-topNav');
                    }

                    if (floating) {
                        if (floatButton.offset().top < footer.offset().top - 50)
                            floatButton.addClass('float-fixed');
                        if (floatButton.offset().top > footer.offset().top - 50)
                            floatButton.removeClass('float-fixed');
                    }

                    $('#specsFeaturesDetailsWrapper .bw-model-tabs-data').each(function () {
                        var top = $(this).offset().top - topNavBar.height(),
                            bottom = top + $(this).outerHeight();
                        if (windowScrollTop >= top && windowScrollTop <= bottom) {
                            topNavBar.find('li').removeClass('active');
                            $('#specsFeaturesDetailsWrapper .bw-mode-tabs-data').removeClass('active');

                            $(this).addClass('active');

                            var currentActiveTab = topNavBar.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                            topNavBar.find(currentActiveTab).addClass('active');
                        }
                    });
                });

                $('.model-specs-features-tabs-wrapper li').click(function () {
                    var target = $(this).attr('data-tabs');
                    $('html, body').animate({ scrollTop: $(target).offset().top - topNavBar.height() }, 1000);
                    return false;
                });
            });
        </script>
    </form>
</body>
</html>

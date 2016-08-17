<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ModelSpecsFeatures" EnableViewState="false" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadPopUp" TagPrefix="BW" %>
<!DOCTYPE html>

<html>
<head>
    <%
        isHeaderFix = false;
        title = string.Format("{0} Specifications and Features - Check out mileage and other technical specifications - BikeWale", bikeName);
        description = string.Format("Know more about {0} Specifications and Features. See details about mileage, engine displacement, power, kerb weight and other specifications.", bikeName);
        keywords = string.Format("{0} specifications, {0} specs, {0} features, {0} mileage, {0} fuel efficiency", bikeName);
        alternate = string.Format("http://www.bikewale.com/m/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        ogImage = modelImage;
        isAd970x90Shown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        
        AdId = "1442913773076";
        AdPath = "/1017752/Bikewale_NewBike_";
          %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/specsandfeature.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/" itemprop="url"><span itemprop="title"><%= makeName %> Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" itemprop="url"><span itemprop="title"><%= makeName %> <%= modelName %></span></a>
                            </li>
                            <li>
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <span>Specifications & Features</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section id="bikeModelHeading" class="container">
            <div class="grid-12">
                <h1 class="content-box-shadow content-inner-block-1420 box-shadow"><%= bikeName %> Specifications and Features</h1>
            </div>
            <div class="clear"></div>
        </section>
        
        <section id="modelCardAndDetailsWrapper" class="container margin-bottom30 font14">
            <div class="grid-12">
                <div id="modelFloatingCardContent">
                    <div class="model-details-floating-card content-box-shadow">
                        <div class="content-inner-block-1020">
                            <div class="grid-5 alpha omega">
                                <div class="model-card-image-content inline-block-top margin-right20">
                                    <img src="<%= modelImage %>" 
                                        title="<%= String.Format("{0} {1}",bikeName, versionName) %> Photos" alt="<%= String.Format("{0} {1}",bikeName, versionName) %> Photos"  />
                                </div>
                                <div class="model-card-title-content inline-block-top">
                                    <p class="font16 text-bold margin-bottom5"><%= bikeName %></p>
                                    <p class="font14 text-light-grey"><%= versionName %></p>
                                </div>
                            </div>
                            
                            <% if (isDiscontinued)
                                { %>
                                <div class="grid-7 padding-left30">
                                    <p class="font14 text-light-grey text-truncate">Last known Ex-showroom price</p>
                                    <div>
                                        <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                    </div>
                                    <p class="font14 text-light-grey"><%= bikeName %> is now discontinued in India.</p>
                                </div>
                                <div class="clear"></div>
                            <%  }
                            else
                            { %>
                                <div class="grid-4 padding-left30">
                                    <p class="font14 text-light-grey text-truncate"><%=IsExShowroomPrice ? "Ex-showroom price in Mumbai" : string.Format("On-road price in {0} {1}", areaName, cityName) %></p>
                                    <span class="bwsprite inr-lg"></span>
                                    <span class="font18 text-bold">
                                        <% if (price > 0)
                                            { %>
                                        <%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %>
                                        <% }
                                            else
                                            { %>
                                        Price not available
                                        <% } %>
                                    </span>
                                </div>
                            <%} %>

                            <% if (!isDiscontinued) { 
                                if (  dealerDetail != null && dealerDetail.PrimaryDealer != null && dealerDetail.PrimaryDealer != null && dealerDetail.PrimaryDealer.DealerDetails.DealerPackageType == Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium)
                                {%>
                            <div class="grid-3 model-orp-btn alpha omega">
                                <a href="javascript:void(0)" c="SpecsandFeature" a="Get_Offers_Clicked" v="BkCityArea"data-leadsourceid="26" data-pqsourceid="50" data-item-name="<%= dealerDetail.PrimaryDealer.DealerDetails.Organization %>" data-item-area="<%= areaName %>" data-item-id="<%= dealerDetail.PrimaryDealer.DealerDetails.DealerId %>"  class="btn btn-orange font14 margin-top5 leadcapturebtn bw-ga">Get offers from this dealer</a>
                                <!-- if no 'powered by' text is present remove margin-top5 add margin-top10 in offers button -->
                                <p class="model-powered-by-text font12 margin-top10 text-truncate"><span class="text-light-grey">Powered by </span><%= dealerDetail.PrimaryDealer.DealerDetails.Organization %></p>
                            </div>
                            <% }
                                else if (!isCitySelected || !isAreaSelected) 
                                {%>
                                <div class="grid-3 model-orp-btn alpha omega">
                                    <a href="javascript:void(0)" v="bikeVersionName" c="SpecsandFeature" a="Check_On_Road_Price_Clicked" isModel="true" data-pqsourceid="49" pqSourceId="49" modelId="<%= modelId %>" class="btn btn-orange font14 margin-top5 fillPopupData  bw-ga">Check on-road price</a>
                                </div>
                            <% } %>
                            <div class="clear"></div>
                            <% } %>
                        </div>
                        <div class="overall-specs-tabs-wrapper">
                            <a class="active" href="#specs">Specifications</a>
                            <a href="#features">Features</a>
                        </div>
                    </div>
                 </div>

                <div id="modelSpecsAndFeaturesWrapper" class="content-box-shadow">
                    <div class="border-divider"></div>
                    <div id="specs" class="bw-model-tabs-data padding-top20">
                        <h2 class="padding-left20 padding-right20">Specifications</h2>
                        <h3 class="padding-left20">Engine and Transmission</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Displacement</p>
                            <p>Cylinders</p>
                            <p>Max Power</p>
                            <p>Maximum Torque</p>
                            <p>Bore</p>
                            <p>Stroke</p>
                            <p>Valves Per Cylinder</p>
                            <p>Fuel Delivery System</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Displacement) %> <span>cc</span></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Cylinders) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.MaxPower, "bhp", specs.MaxPowerRPM, "rpm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable( specs.MaximumTorque, "Nm", specs.MaximumTorqueRPM,"rpm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Bore,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Stroke,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ValvesPerCylinder) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelDeliverySystem) %></p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Fuel Type</p>
                            <p>Ignition</p>
                            <p>Spark Plugs</p>
                            <p>Cooling System</p>
                            <p>Gearbox Type</p>
                            <p>No. of Gears</p>
                            <p>Transmission Type</p>
                            <p>Clutch</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Ignition) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.SparkPlugsPerCylinder, "Per Cylinder") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.CoolingSystem) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.GearboxType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.NoOfGears) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TransmissionType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Clutch) %></p>
                        </div>
                        <div class="clear"></div>

                        <h3 class="margin-top30 padding-left20">Brakes, Wheels and Suspension</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Brake Type</p>
                            <p>Front Disc</p>
                            <p>Front Disc/Drum Size</p>
                            <p>Rear Disc</p>
                            <p>Rear Disc/Drum Size</p>
                            <p>Calliper Type</p>
                            <p>Wheel Size</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.BrakeType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FrontDisc) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FrontDisc_DrumSize,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.RearDisc) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.RearDisc_DrumSize,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.CalliperType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.WheelSize,"inches") %></p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Front Tyre</p>
                            <p>Rear Tyre</p>
                            <p>Tubeless Tyres</p>
                            <p>Radial Tyres</p>
                            <p>Alloy Wheels</p>
                            <p>Front Suspension</p>
                            <p>Rear Suspension</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FrontTyre) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.RearTyre) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TubelessTyres) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.RadialTyres) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.AlloyWheels) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FrontSuspension) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.RearSuspension) %></p>
                        </div>
                        <div class="clear"></div>

                        <h3 class="margin-top30 padding-left20">Dimensions and Chassis</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Kerb Weight</p>
                            <p>Overall Length</p>
                            <p>Overall Width</p>
                            <p>Overall Height</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.KerbWeight,"kg") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.OverallLength,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.OverallWidth,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.OverallHeight,"mm") %></p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Wheelbase</p>
                            <p>Ground Clearance</p>
                            <p>Seat Height</p>
                            <p>Chassis Type</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Wheelbase,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.GroundClearance, "mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.SeatHeight,"mm") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ChassisType) %></p>
                        </div>
                        <div class="clear"></div>

                        <h3 class="margin-top30 padding-left20">Fuel efficiency and Performance</h3>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Fuel Tank Capacity</p>
                            <p>Reserve Fuel Capacity</p>
                            <p>Fuel Efficiency Overall</p>
                            <p>Fuel Efficiency Range</p>
                            <p>Top Speed</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelTankCapacity,"litres") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ReserveFuelCapacity,"litres") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelEfficiencyOverall,"kmpl") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelEfficiencyRange,"km") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TopSpeed,"kmph") %></p>
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>0 to 60 kmph</p>
                            <p>0 to 80 kmph</p>
                            <p>0 to 40 kmph</p>
                            <p>60 to 0 kmph</p>
                            <p>80 to 0 kmph</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Performance_0_60_kmph,"seconds") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Performance_0_80_kmph,"seconds") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Performance_0_40_m,"seconds") %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Performance_60_0_kmph) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Performance_80_0_kmph) %></p>
                        </div>
                        <div class="clear"></div>
                        <div class="margin-top30 margin-right10 margin-left10 border-divider"></div>
                    </div>

                    <div id="features" class="bw-model-tabs-data padding-top20 padding-bottom40">
                        <h2 class="padding-left20 padding-right20">Features</h2>
                        <div class="grid-3 padding-left20 text-light-grey">
                            <p>Speedometer</p>
                            <p>Fuel Guage</p>
                            <p>Tachometer Type</p>
                            <p>Digital Fuel Guage</p>
                            <p>Tripmeter</p>
                            <p>Electric Start</p>
                            <p>Tachometer</p>
                            <p>Shift Light</p>
                            <p>No. of Tripmeters</p>
                            <p>Tripmeter Type</p>
                            <p>Low Fuel Indicator</p>
                            <p>Low Oil Indicator</p>
                            <p>Low Battery Indicator</p>
                            <p>Pillion Seat</p>
                            <p>Pillion Footrest</p>
                        </div>
                        <div class="grid-3 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Speedometer) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.FuelGauge) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TachometerType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.DigitalFuelGauge) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Tripmeter) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ElectricStart) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Tachometer) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ShiftLight) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.NoOfTripmeters) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TripmeterType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.LowFuelIndicator) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.LowOilIndicator) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.LowBatteryIndicator) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.PillionSeat) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.PillionFootrest) %></p>                            
                        </div>
                        <div class="grid-3 padding-left30 text-light-grey">
                            <p>Pillion Backrest</p>
                            <p>Pillion Grabrail</p>
                            <p>Stand Alarm</p>
                            <p>Stepped Seat</p>
                            <p>Antilock Braking System</p>
                            <p>Killswitch</p>
                            <p>Clock</p>
                            <p>Electric System</p>
                            <p>Battery</p>
                            <p>Headlight Type</p>
                            <p>Headlight Bulb Type</p>
                            <p>Brake/Tail Light</p>
                            <p>Turn Signal</p>
                            <p>Pass Light</p>
                        </div>
                        <div class="grid-3 padding-right20 text-bold">
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.PillionBackrest) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.PillionGrabrail) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.StandAlarm) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.SteppedSeat) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.AntilockBrakingSystem) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Killswitch) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Clock) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.ElectricSystem) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Battery) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.HeadlightType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.HeadlightBulbType) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.Brake_Tail_Light) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.TurnSignal) %></p>
                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(specs.PassLight) %></p>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div id="modelSpecsFeaturesFooter"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <BW:LeadPopUp ID="ctrlLeadPopUp" runat="server" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript">
            var pageUrl = window.location.href;
            var clientIP = '<%= clientIP %>';
            var bikename='<%= bikeName %>'
            var bikeVersionName=bikename+'_'+'<%= versionName %>' 
            var cityArea='<%=cityName%>'+'_'+'<%=areaName%>'
            var BkCityArea=bikename+'_'+cityArea;
            ga_pg_id=15;
            $(document).ready(function () {
                var ShowOffer='<%=isGetOfferShown%>'
                var isOfferShown = (ShowOffer.toLowerCase() === 'true');
                    if(isOfferShown)
                    {    
                        triggerGA('SpecsandFeature', 'Get_Offers_Shown', "<%= string.Format("{0}_{1}_{2}_{3}", makeName, modelName,cityName,areaName)%>");
                    }
                    $("#btnDealerPricePopup").click(function () {
                        var selArea = '';
                        if ($('#ddlAreaPopup option:selected').index() > 0) {
                            selArea = '_' + $('#ddlAreaPopup option:selected').html();
                        }
                        triggerGA('SpecsandFeature', 'Show_On_Road_Price_Clicked', bikeVersionName+'_'+ $('#versions .active').text() + '_' + $('#ddlCitiesPopup option:selected').html() + selArea);
                    });
                    $("#user-details-submit-btn").click(function () {
                        if ($("#getFullName").val() != '' && $("#getEmailID").val() != '' && $("#getMobile").val() != '')
                        {
                            triggerGA('SpecsandFeature', 'Lead_Submitted', "<%= string.Format("{0}_{1}_{2}_{3}", makeName, modelName,cityName,areaName)%>");
                        }

                    });

           
               
                var hashValue = window.location.hash.substr(1);
                if (hashValue.length > 0) {
                    $("body, html").animate({
                        scrollTop: $("#" + hashValue).offset().top - $('.model-details-floating-card').height() + 110
                    }, 500);
                }

                var $window = $(window),
                    modelCardAndDetailsWrapper = $('#modelCardAndDetailsWrapper'),
                    modelDetailsFloatingCard = $('.model-details-floating-card'),
                    modelSpecsFeaturesFooter = $('#modelSpecsFeaturesFooter');

                $('#modelFloatingCardContent').css({ 'height': modelDetailsFloatingCard.height() });

                $(window).scroll(function () {
                    var windowScrollTop = $window.scrollTop(),
                        modelCardAndDetailsOffsetTop = modelCardAndDetailsWrapper.offset().top,
                        modelSpecsFeaturesFooterOffsetTop = modelSpecsFeaturesFooter.offset().top;

                    if (windowScrollTop > modelCardAndDetailsOffsetTop)
                        modelDetailsFloatingCard.addClass('fixed-card');

                    else if (windowScrollTop < modelCardAndDetailsOffsetTop)
                        modelDetailsFloatingCard.removeClass('fixed-card');

                    if (modelDetailsFloatingCard.hasClass('fixed-card')) {
                        if (windowScrollTop > modelSpecsFeaturesFooterOffsetTop - modelDetailsFloatingCard.height())
                            modelDetailsFloatingCard.removeClass('fixed-card');
                    }

                    $('#modelSpecsAndFeaturesWrapper .bw-model-tabs-data').each(function () {
                        var top = $(this).offset().top - modelDetailsFloatingCard.height(),
                            bottom = top + $(this).outerHeight();
                        if (windowScrollTop >= top && windowScrollTop <= bottom) {
                            modelDetailsFloatingCard.find('a').removeClass('active');
                            $('#modelSpecsAndFeaturesWrapper .bw-mode-tabs-data').removeClass('active');

                            $(this).addClass('active');
                            modelDetailsFloatingCard.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
                        }
                    });

                });

                $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
                    var target = $(this.hash);
                    if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
                    if (target.length == 0) target = $('html');
                    $('html, body').animate({ scrollTop: target.offset().top - modelDetailsFloatingCard.height() }, 1000);
                    return false;
                });

                $(".leadcapturebtn").click(function () {
                    ele = $(this);
                    var leadOptions = {
                        "dealerid": ele.attr('data-item-id'),
                        "dealername": ele.attr('data-item-name'),
                        "dealerarea": ele.attr('data-item-area'),
                        "versionid": <%= versionId %>,
                        "leadsourceid": ele.attr('data-leadsourceid'),
                        "pqsourceid": ele.attr('data-pqsourceid'),
                        "pageurl": pageUrl,
                        "clientip": clientIP,
                        "isregisterpq": true
                    };
                    dleadvm.setOptions(leadOptions);
                });
                $("#user-details-submit-btn").click(function(){
                    if(dleadvm.IsVerified)
                    {
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Specs_Page', 'act': 'Lead_Submitted', 'lab': "<%= string.Format("{0}_{1}_{2}_{3}_{4}", makeName, modelName, versionName, cityName, areaName )%>" });
                    }
                });

            });
        </script>

    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ModelSpecsFeatures" EnableViewState="false" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/controls/GenericBikeInfoControl.ascx" %>
<!DOCTYPE html>

<html>
<head>
    <%
        isHeaderFix = false;

        title = pgTitle;
        description = string.Format("Know more about {0} Specifications and Features. See details about mileage, engine displacement, power, kerb weight and other specifications.", bikeName);
        keywords = string.Format("{0} specifications, {0} specs, {0} features, {0} mileage, {0} fuel efficiency", bikeName);
        alternate = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        canonical = string.Format("https://www.bikewale.com/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        ogImage = modelImage;
        isAd970x90Shown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        AdId = "1442913773076";
        AdPath = "/1017752/Bikewale_NewBike_";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%=  staticUrl  %>/css/specsandfeature.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    
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
                                <a href="/new-bikes-in-india/" itemprop="url"><span itemprop="title">New Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/" itemprop="url"><span itemprop="title"><%= makeName %> Bikes</span></a>
                            </li>

                              <% if(IsScooter && !IsScooterOnly)
                                { %>
                              <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-scooters/" itemprop="url"><span itemprop="title"><%= String.Format("{0} Scooters", makeName) %></span></a>
                            </li>
                              <%  }
                                 %>

                            <% if (!string.IsNullOrEmpty(seriesUrl))
                                { %>
                                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                    <span class="bwsprite fa-angle-right margin-right10"></span>
                                    <a href="/<%= seriesUrl %>" itemprop="url"><span itemprop="title"><%= Series.SeriesName %></span></a>
                                </li>
                            <% } %>

                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" itemprop="url"><span itemprop="title"><%= String.Format("{0} {1}", makeName, modelName) %></span></a>
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
        <div>
    </div>

        <section id="bikeModelHeading" class="container">
            <div class="grid-12">
                <% if (Bikewale.Utility.BWConfiguration.Instance.MetasMakeId.Split(',').Contains(_makeId.ToString())){%> 
                <h1 class="content-box-shadow content-inner-block-1420 box-shadow">Specifications & Feature of <%= bikeName%></h1>
                
                <%}else{ %>
                <h1 class="content-box-shadow content-inner-block-1420 box-shadow"><%= bikeName %> Specifications and Features</h1>
                <%} %>
            </div>
            <div class="clear"></div>
        </section>
        
        <section id="modelCardAndDetailsWrapper" class="container margin-bottom20 font14">
            <div class="grid-12">
                <div id="modelFloatingCardContent">
                    <div class="model-details-floating-card content-box-shadow">
                        <div class="content-inner-block-1020">
                            <div class="grid-5 alpha omega">
                                <div class="model-card-image-content inline-block-top margin-right20">
                                    <img src="<%= modelImage %>" 
                                        title="<%= String.Format("{0} {1}",bikeName, versionName) %> Images" alt="<%= String.Format("{0} {1}",bikeName, versionName) %> Photos"  />
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
                             <div class="clear"></div>
              
                        </div>
                        <div class="overall-specs-tabs-wrapper">
                            <a class="active" href="#specs">Specifications</a>
                            <a href="#features">Features</a>
                        </div>
                    </div>
                 </div>

                <div id="modelSpecsAndFeaturesWrapper" class="content-box-shadow">
                    <div class="border-divider"></div>
                    <% if(specs!= null){ %>
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
                    <% } %>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <BW:GenericBikeInfo runat="server" ID="ctrlGenericBikeInfo" />
        <section class="container">
            <div class="grid-12">
                <% if (similarBikes != null && similarBikes.Bikes != null && similarBikes.Bikes.Any())
                    { %>
                <div id="modelSimilarContent" class="bw-model-tabs-data content-box-shadow padding-bottom20 card-bottom-margin font14">
                    <h2 class="h2-heading padding-top20 padding-right20 padding-left20 margin-bottom15 font18"><%= bodyStyleText %> similar to <%= modelPg.ModelDetails.ModelName %></h2>
                    <div class="jcarousel-wrapper inner-content-carousel">
                        <div class="jcarousel">
                            <ul>
                                <% foreach (var bike in similarBikes.Bikes)
                                    {  %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName) %>" title="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" class="jcarousel-card">
                                        <div class="model-jcarousel-image-preview">
                                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._310x174,Bikewale.Utility.QualityFactor._75) %>" alt="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" title="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" src="" border="0" />
                                        </div>
                                        <div class="card-desc-block">
                                            <p class="bikeTitle"><%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %></p>
                                            <p class="text-xt-light-grey margin-bottom10">
                                                <%= Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(bike.Displacement), Convert.ToString(bike.FuelEfficiencyOverall), Convert.ToString(bike.MaxPower), Convert.ToString(bike.KerbWeight)) %>
                                            </p>

                                            <% if (bike.VersionPrice == 0 && bike.AvgExShowroomPrice == 0)
                                                { %>
                                            <p class="text-bold text-default">
                                                <span class="font14">Price not available</span>
                                            </p>
                                            <% }
                                                else
                                                {
                                                    if (bike.VersionPrice > 0)
                                                    { %>
                                                        <p class="text-light-grey margin-bottom5"><%= string.Format("Ex-showroom, {0}", bike.CityName) %></p>
                                                     <% }
                                                    else
                                                    { %>
                                                    <p>
                                                    <span class="text-light-grey margin-bottom5 margin-right5">Avg. Ex-showroom price</span><span class="bwsprite info-icon tooltip-icon-target tooltip-top">
                                                    <span class="bw-tooltip info-tooltip">
                                                        <span class="bw-tooltip-text"><%= string.Format("Price is not available in {0}", bike.CityName) %></span>
                                                    </span>
                                                </span>
                                            </p>
                                            <% } %>

                                            <span class='font18 text-default'>&#x20B9;</span>
                                            <span class="font18 text-default text-bold">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(bike.VersionPrice > 0 ? bike.VersionPrice.ToString() : bike.AvgExShowroomPrice.ToString())%></span>
                                            <% } %>
                                        </div>
                                    </a>
                                    <% if (similarBikes.ShowCheckOnRoadCTA)
                                        { %>
                                    <div class="margin-left20 margin-bottom20">
                                        <a href="javascript:void(0);" data-pqsourceid="<%= ((int)similarBikes.PQSourceId) %>" data-makename="<%= makeName %>" data-modelname="<%= modelName %>" data-modelid="<%= bike.ModelBase.ModelId %>" class="btn btn-grey btn-sm font14  <%= (bike.AvgExShowroomPrice!=0 ?"":"hide") %> getquotation" rel="nofollow">Check on-road price</a>
                                    </div>
                                    <% } %>
                                    <% if (similarBikes.ShowPriceInCityCTA)
                                        { %>
                                    <div class="margin-left20 margin-bottom20">
                                        <a href="<%= Bikewale.Utility.UrlFormatter.PriceInCityUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName,bike.CityMaskingName) %>" class="btn btn-white btn-truncate font14 btn-size-2" title="<%= String.Format("{0} {1} On-road price in {2}",makeName,modelName,bike.CityName) %>"><%= String.Format("On-road price in {0}", bike.CityName) %></a>
                                    </div>
                                    <% } %>
                                    <% if (similarBikes.Make != null && similarBikes.Model != null && similarBikes.IsNew)
                                        {
                                           string fullUrl = string.Format("/{0}",Bikewale.Utility.UrlFormatter.CreateCompareUrl(similarBikes.Make.MaskingName, similarBikes.Model.MaskingName, bike.MakeBase.MaskingName, bike.ModelBase.MaskingName, Convert.ToString(similarBikes.VersionId),  Convert.ToString(bike.VersionBase.VersionId), (uint)similarBikes.Model.ModelId, (uint)bike.ModelBase.ModelId, Bikewale.Entities.Compare.CompareSources.Desktop_Model_MostPopular_Compare_Widget));
                                            %>
                                    <a title="<%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.ModelBase.ModelName, similarBikes.Model.ModelName) %>" href="<%=Bikewale.Utility.UrlFormatter.RemoveQueryString(fullUrl) %>" data-url="<%=fullUrl  %>" class="compare-with-target text-truncate redirect-url">
                                        <span class="bwsprite compare-sm"></span>Compare with <%= similarBikes.Model.ModelName %><span class="bwsprite next-grey-icon"></span>
                                    </a>
                                    <% } %>
                                </li>
                                <% } %>
                                <li>
                                    <a href="<%= ((bodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter))? "/scooters/" : "/new-bikes-in-india/") %>" title="<%= (string.Format("Explore more {0}", (similarBikes.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter))? "scooters" : "bikes")) %>" class="jcarousel-card bw-ga" c="<%=similarBikes.Page %>" a="Clicked_ExploreMore_Card" l="<%= similarBikes.Model.ModelName %>">
                                        <div class="model-jcarousel-image-preview">
                                            <div class="exploremore__imagebackground">
                                                <div class="exploremore__icon-background">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-desc-block">
                                            <div class="exploremore-detailblock">
                                                <p class="detailblock__title">Couldn’t find what you were looking for?</p>
                                                <p class="detailblock__description"><%= (bodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter) ? "View 60+ scooters from over 10 brands" : " View 200+ bikes from over 30 brands") %></p>
                                            </div>
                                        </div>
                                        <% if (similarBikes.IsNew)
                                            { %>
                                        <div class="compare-with-target text-truncate">
                                            <%= (string.Format("Explore more {0}", (bodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)) ? "scooters" : "bikes")) %><span class="bwsprite next-grey-icon"></span>
                                        </div>
                                        <% } %>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                    </div>
                </div>
                <% }
                    else if ((popularBodyStyle != null && popularBodyStyle.PopularBikes != null && popularBodyStyle.PopularBikes.Any()) && (similarBikes.IsNew || similarBikes.IsUpcoming))
                    { %>
                <div id="modelPopularContent" class="bw-model-tabs-data content-box-shadow card-bottom-margin padding-bottom15">
                    <div class="carousel-heading-content padding-top20">
                        <div class="swiper-heading-left-grid inline-block">
                            <h2 class="h2-heading">Other popular <%= popularBodyStyle.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)? "scooters" : "bikes" %></h2>
                        </div>
                        <div class="swiper-heading-right-grid inline-block text-right">
                            <a href="<%= Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(popularBodyStyle.BodyStyle) %>" title="Best <%= popularBodyStyle.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)? "scooters" : "bikes" %> in India" class="btn view-all-target-btn">View all</a>
                        </div>
                    </div>
                    <% if (popularBodyStyle != null && popularBodyStyle.PopularBikes != null && popularBodyStyle.PopularBikes.Count() > 0)
                        { %>
                    <div class="jcarousel-wrapper inner-content-carousel padding-bottom20">
                        <div class="jcarousel">
                            <ul>
                                <% foreach (var bike in popularBodyStyle.PopularBikes)
                                    { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName) %>" title="<%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %>" class="jcarousel-card">
                                        <div class="model-jcarousel-image-preview">
                                            <span class="card-image-block">
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath, bike.HostURL, Bikewale.Utility.ImageSize._310x174, Bikewale.Utility.QualityFactor._75) %>" alt="<%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %>" src="" border="0">
                                            </span>
                                        </div>
                                        <div class="card-desc-block">
                                            <h3 class="bikeTitle"><%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %></h3>
                                            <% if (bike.VersionPrice == 0 && bike.AvgPrice == 0)
                                                { %>
                                            <span class="font16 text-default text-light-grey">Price not available</span>
                                            <% }
                                                else
                                                {
                                                    if (bike.VersionPrice > 0)
                                                    { %>
                                            <p class="font14 text-light-grey margin-bottom5"><%= string.Format("Ex-showroom, {0}", (!string.IsNullOrEmpty(bike.CityName) ? bike.CityName : Bikewale.Utility.BWConfiguration.Instance.GetDefaultCityName)) %></p>
                                            <span class='font16 text-default'>&#x20B9;</span>
                                            <span class="font16 text-bold text-default">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                                            <% }
                                                else
                                                { %>
                                            <p class="font14 text-light-grey margin-bottom5">
                                                <span class="margin-right5">Avg. Ex-showroom price</span>
                                                <span class="bwsprite info-icon tooltip-icon-target tooltip-top">
                                                    <span class="bw-tooltip info-tooltip">
                                                        <span class="bw-tooltip-text"><%= string.Format("Price is not available in {0}", bike.CityName) %></span>
                                                    </span>
                                                </span>
                                            </p>

                                            <span class='font16 text-default'>&#x20B9;</span>
                                            <span class="font16 text-bold text-default">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.AvgPrice))%></span>
                                            <% }
                                                }%>
                                        </div>
                                    </a>
                                    <% if (popularBodyStyle.ShowCheckOnRoadCTA && (bike.AvgPrice > 0 || bike.VersionPrice > 0))
                                        {%>
                                    <div class="margin-left20 margin-bottom20">
                                        <a href="javascript:void(0);" data-pqsourceid="<%= ((int)popularBodyStyle.PQSourceId) %>" data-makename="<%= bike.MakeName %>" data-modelname="<%= bike.objModel.ModelName %>" data-modelid="<%= bike.objModel.ModelId %>" class="btn btn-grey btn-sm font14 getquotation" rel="nofollow">Check on-road price</a>
                                    </div>
                                    <% } %>
                                </li>
                                <% } %>
                            </ul>

                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                    </div>
                    <% } %>
            </div>
            <% } %>
            </div>
            <div class="clear"></div>
        </section>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript">
            var pageUrl = window.location.href;
            var clientIP = '<%= clientIP %>';
            var bikename = '<%= bikeName %>';
            var bikeVersionName = bikename + '_' + '<%= versionName %>'; 
            var cityArea = '<%=cityName%>' + '_' + '<%=areaName%>';
            var BkCityArea=bikename+'_'+cityArea;
            ga_pg_id=15;
            $(document).ready(function () {

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
           });
        </script>

    </form>
</body>
</html>

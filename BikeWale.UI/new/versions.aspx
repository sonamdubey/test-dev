<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="versions.aspx.cs" Inherits="Bikewale.New.versions" %>
<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<!doctype html>
<html>
<head>
    <!-- #include file="/includes/headscript.aspx" -->
    <% isHeaderFix = false; %>
    <link href="/css/model.css" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey">    
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-white">
            <div class="container">
                <div class="grid-12">
                    <div class="padding-bottom15 text-center">
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>New Bikes</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10 margin-bottom10"><%= bikeName %></h1>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-10 padding-top20 padding-bottom20 rounded-corner2">
                        <div class="grid-6 alpha margin-minus10">
                            <div class="connected-carousels">
                                <div class="stage">
                                    <div class="carousel carousel-stage">
                                        <ul>
                                            <asp:Repeater ID="rptModelPhotos" runat="server">
                                                <HeaderTemplate>
                                                    <li>
                                                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath,modelPage.ModelDetails.HostUrl,Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName %>" alt="<%= bikeName %>">
                                                    </li>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li>
                                                        <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>">
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <a href="#" class="prev prev-stage bwsprite"></a>
                                    <a href="#" class="next next-stage bwsprite"></a>
                                </div>

                                <div class="navigation">
                                    <div class="carousel carousel-navigation">
                                        <ul>
                                            <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                                <HeaderTemplate>
                                                    <li>
                                                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath,modelPage.ModelDetails.HostUrl,Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName %>" alt="<%= bikeName %>">
                                                    </li>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li>
                                                        <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>"></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                            </div>

                            <div class="margin-top20">
                                <p class="margin-left50	leftfloat margin-right20">
                                    <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPage.ModelDetails.ReviewRate)) %>
                                </p>
                                <a href="<%= FormatShowReview(modelPage.ModelDetails.MakeBase.MaskingName,modelPage.ModelDetails.MaskingName) %>" class="review-count-box border-solid-left leftfloat margin-right20 padding-left20"><%= modelPage.ModelDetails.ReviewCount %> Reviews
                                </a>
                                <a href="<%= FormatWriteReviewLink() %>" class="border-solid-left leftfloat margin-right20 padding-left20">Write a review
                                </a>
                                <div class="clear"></div>
                            </div>

                        </div>

                        <div class="grid-6 padding-left40" id="dvBikePrice">
                            <div class="bike-price-container font28 margin-bottom15">
                                <span class="fa fa-rupee"></span>
                                <span id="bike-price" class="font30 text-black"><%= Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MinPrice.ToString()) %></span> <span class="font12 text-light-grey default-showroom-text">Ex-showroom <%= ConfigurationManager.AppSettings["defaultName"] %></span>
                            </div>
                            <div id="city-list-container" class="city-list-container margin-bottom20">
                                <div class="text-left margin-bottom15">
                                    <p class="font16 offer-error">Select city for accurate on-road price and exclusive offers</p>
                                </div>
                                <ul id="mainCity">
                                    <li cityId="1"><span>Mumbai</span></li>
                                    <li cityId="12"><span>Pune</span></li>
                                    <li cityId="2"><span>Banglore</span></li>
                                    <li cityId="40"><span>Thane</span></li>
                                    <li cityId="13"><span>Navi Mumbai</span></li>
                                    <li class="city-other-btn"><span>Others</span></li>
                                </ul>
                            </div>
                            <div id="city-area-select-container" class="city-area-select-container margin-bottom20 hide">
                                <div class="city-select-text text-left margin-bottom15 hide">
                                    <p class="font16">Select city for accurate on-road price and exclusive offers</p>
                                </div>
                                <div class="area-select-text text-left margin-bottom15 hide">
                                    <p class="font16">Select area for on-road price and exclusive offers</p>
                                </div>
                                <div class="city-onRoad-price-container font16 margin-bottom15 hide">
                                    <p class="margin-bottom10">On-road price in <span id="pqArea">Andheri</span>, <span id="pqCity">Mumbai</span><span class="city-edit-btn font12 margin-left10">Edit</span></p>
                                    <p class="font12 margin-bottom15 text-light-grey" id="breakup">(Ex-showroom + RTO + Insurance + Handling charges)</p>
                                    <button class="btn btn-orange" id="btnBookNow">Avail offers</button>
                                </div>
                                <div class="city-area-wrapper">
                                    <div class="city-select leftfloat margin-right20">
                                        <select id="ddlCity" data-bind="options: cities, optionsText: 'cityName', optionsValue: 'cityId', value: selectedCity, optionsCaption: 'Select City', event : { change : LoadArea }"></select>
                                    </div>
                                    <div class="area-select leftfloat">
                                        <select id="ddlArea" data-bind="options: areas, optionsText: 'areaName', optionsValue: 'areaId', value: selectedArea, optionsCaption: 'Select Area', enable: selectedCity, event: { change: OnAreaChange }"></select>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="city-unveil-offer-container position-rel">
                                <div class="available-offers-container content-inner-block-10">
                                    <h4 class="border-solid-bottom padding-bottom5 margin-bottom5">Available Offers</h4>
                                    <div class="offer-list-container" id="dvAvailableOffer">
                                        
                                    </div>
                                </div>
                                <div class="unveil-offer-btn-container position-abt pos-left0 pos-top0 text-center">
                                    <button class="btn btn-orange unveil-offer-btn">Show Offers</button>
                                </div>
                                <div class="notify-btn-container position-abt pos-left0 pos-top0 hide">
                                    <div class="margin-top50 margin-left40">
                                        <input type="text" placeholder="Notify me" class="notify-input">
                                        <button class="btn btn-orange btn-xs">Notify me</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="container">
            <!--  Discover bikes section code starts here -->
            <div class="grid-12">
                <div class="content-box-shadow content-inner-block-10">
                    <div class="bw-overall-rating">
                        <a class="active" href="#overview">Overview</a>
                        <a href="#specifications">Specifications</a>
                        <a href="#features">Features</a>
                        <a href="#variants">Variants</a>
                        <a href="#colours">Colours</a>
                    </div>
                    <!-- specification code starts here -->
                    <div class="bw-tabs-data margin-bottom20" id="overview">
                        <h2 class="font24 margin-top10 margin-bottom20 text-center">Overview</h2>
                        <div class="grid-3 border-solid-right">
                            <div class="text-center padding-top20 padding-bottom20">
                                <%= FormatOverview(modelPage.ModelVersionSpecs.Displacement,Bikewale.New.Overviews.Capacity) %>
                                <p class="font20 text-black">Capacity</p>
                            </div>
                        </div>
                        <div class="grid-3 border-solid-right padding-top20 padding-bottom20">
                            <div class="text-center">
                                <%= FormatOverview(modelPage.ModelVersionSpecs.FuelEfficiencyOverall,Bikewale.New.Overviews.Mileage) %>
                                <p class="font20 text-black">Mileage</p>
                            </div>
                        </div>
                        <div class="grid-3 border-solid-right padding-top20 padding-bottom20">
                            <div class="text-center">
                                <%= FormatOverview(modelPage.ModelVersionSpecs.MaxPower,Bikewale.New.Overviews.MaxPower) %>
                                <p class="font20 text-black">Max power</p>
                            </div>
                        </div>
                        <div class="grid-3">
                            <div class="text-center padding-top20 padding-bottom20">
                                <%= FormatOverview(modelPage.ModelVersionSpecs.KerbWeight,Bikewale.New.Overviews.Weight) %>
                                <p class="font20 text-black">Weight</p>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <p class="font14 margin-top20 text-grey padding-left10 padding-right10">
                            <span class="model-about-main"><%= modelPage.ModelDesc.SmallDescription %>
                            </span>
                            <span class="model-about-more-desc hide"><%= modelPage.ModelDesc.FullDescription %>
                            </span>
                            <span><a href="javascript:void(0)" class="read-more-btn">Read <span>more</span></a></span>
                        </p>
                    </div>
                    <!-- specification code starts here -->
                    <div class="bw-tabs-data margin-bottom20" id="specifications">
                        <h2 class="font24 margin-top10 margin-bottom20 text-center">Specifications</h2>
                        <div class="bw-tabs-panel margin-left10 margin-right10">
                            <div class="leftfloat bw-horz-tabs">
                                <div class="bw-tabs">
                                    <ul>
                                        <li class="active" data-tabs="summary"><span class="model-sprite bw-summary-ico"></span>Summary</li>
                                        <li data-tabs="engineTransmission"><span class="model-sprite bw-engine-ico"></span>Engine & Transmission </li>
                                        <li data-tabs="brakeWheels"><span class="model-sprite bw-brakeswheels-ico"></span>Brakes, Wheels and Suspension</li>
                                        <li data-tabs="dimensions"><span class="model-sprite bw-dimensions-ico"></span>Dimensions and Chassis</li>
                                        <li data-tabs="fuelEffiency"><span class="model-sprite bw-performance-ico"></span>Fuel effieciency and Performance</li>
                                    </ul>
                                </div>
                            </div>
                            <div class="leftfloat bw-horz-tabs-data font16">
                                <div class="bw-tabs-data" id="summary">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Displacement (cc)</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.Displacement %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Max Power</div>
                                            <%= FormatMaxPower(modelPage.ModelVersionSpecs.MaxPower,modelPage.ModelVersionSpecs.MaxPowerRPM) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Maximum Torque</div>
                                            <%= FormatMaxTorque(modelPage.ModelVersionSpecs.MaximumTorque,modelPage.ModelVersionSpecs.MaximumTorqueRPM) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">No. of gears</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.NoOfGears) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Efficiency (kmpl)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Brake Type</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.BrakeType %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Disc</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.FrontDisc ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Disc</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.RearDisc ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Alloy Wheels</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.AlloyWheels ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Kerb Weight (Kg)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.KerbWeight) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Chassis Type</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.ChassisType)  %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Top Speed</div>
                                            <%= FormatSpeed(modelPage.ModelVersionSpecs.TopSpeed) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Tubeless Tyres</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.TubelessTyres ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Tank Capacity (Litres)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FuelTankCapacity) %>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="engineTransmission">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Displacement (cc)</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.Displacement %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Cylinders</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Cylinders) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Max Power</div>
                                            <%= FormatMaxPower(modelPage.ModelVersionSpecs.MaxPower,modelPage.ModelVersionSpecs.MaxPowerRPM) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Maximum Torque</div>
                                            <%= FormatMaxTorque(modelPage.ModelVersionSpecs.MaximumTorque,modelPage.ModelVersionSpecs.MaximumTorqueRPM) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Bore (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Bore) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Stroke (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Stroke) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Valves Per Cylinder</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.ValvesPerCylinder) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Delivery System</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FuelDeliverySystem) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Type</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FuelType) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Ignition</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Ignition) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Spark Plugs (Per Cylinder)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.SparkPlugsPerCylinder) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Cooling System</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.CoolingSystem) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Gearbox Type</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.GearboxType) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Transmission Type</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.TransmissionType) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Clutch</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Clutch) %>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="brakeWheels">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Front Disc</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.FrontDisc ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Disc/Drum Size (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FrontDisc_DrumSize) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Disc</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.RearDisc ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Disc/Drum Size (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.RearDisc_DrumSize) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Calliper Type</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.CalliperType) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Wheel Size (inches)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.WheelSize) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Tyre</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FrontTyre) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Tyre</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.RearTyre) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Tubeless Tyres</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.TubelessTyres ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Radial Tyres</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.RadialTyres ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Alloy Wheels</div>
                                            <div class="text-bold"><%= modelPage.ModelVersionSpecs.AlloyWheels ? "Yes" : "-" %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Suspension</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FrontSuspension) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Suspension</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.RearSuspension) %>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="dimensions">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Kerb Weight (Kg)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.KerbWeight) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Valves Per Cylinder</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.ValvesPerCylinder) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Overall Length (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.OverallLength) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Overall Width (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.OverallWidth) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Overall Height (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.OverallHeight) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Wheelbase (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Wheelbase) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Ground Clearance (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.GroundClearance) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Seat Height (mm)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.SeatHeight) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Chassis Type</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.ChassisType) %>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="fuelEffiency">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Fuel Tank Capacity (Litres)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FuelTankCapacity) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Reserve Fuel Capacity (Litres)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.ReserveFuelCapacity) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Efficiency Overall (kmpl)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Efficiency Range (km)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.FuelEfficiencyRange) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">0 to 60 kmph (Seconds)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Performance_0_60_kmph) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">0 to 80 kmph (Seconds)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Performance_0_80_kmph) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">0 to 40 m (Seconds)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Performance_0_40_m) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Top Speed (kmph)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.TopSpeed) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">60 to 0 kmph (Seconds, metres)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Performance_60_0_kmph) %>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">80 to 0 kmph (Seconds, metres)</div>
                                            <%= FormatValue(modelPage.ModelVersionSpecs.Performance_80_0_kmph) %>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <!-- features code starts here -->
                    <div class="bw-tabs-data margin-bottom20" id="features">
                        <div class="border-solid-top margin-left10 margin-right10"></div>
                        <h2 class="font24 margin-top10 margin-bottom20 text-center">Features</h2>
                        <div class="equal-width-list">
                            <ul>
                                <li>
                                    <div class="text-light-grey">Speedometer</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.Speedometer) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Fuel Guage</div>
                                    <div class="text-bold"><%= modelPage.ModelVersionSpecs.FuelGauge ? "Yes" : "-" %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tachometer Type</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.TachometerType) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Digital Fuel Guage</div>
                                    <div class="text-bold"><%= modelPage.ModelVersionSpecs.DigitalFuelGauge ? "Yes" : "-" %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tripmeter</div>
                                    <div class="text-bold"><%= modelPage.ModelVersionSpecs.Tripmeter ? "Yes" : "-" %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Electric Start</div>
                                    <div class="text-bold"><%= modelPage.ModelVersionSpecs.ElectricStart ? "Yes" : "-" %></div>
                                    <div class="clear"></div>
                                </li>
                                <div class="clear"></div>
                            </ul>
                            <ul class="more-features hide">
                                <li>
                                    <div class="text-light-grey">Bore (mm)</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.Bore) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Stroke (mm)</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.Stroke) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Valves Per</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.ValvesPerCylinder) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Fuel Delivery System</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.FuelDeliverySystem) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Fuel Type</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.FuelType) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Ignition</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.Ignition) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Spark Plugs Per Cylinder</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.SparkPlugsPerCylinder) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Cooling System</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.CoolingSystem) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Displacement (cc)</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.Displacement) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Ground Clearance (mm)</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.GroundClearance) %>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Overall Length (mm)</div>
                                    <%= FormatValue(modelPage.ModelVersionSpecs.OverallLength) %>
                                    <div class="clear"></div>
                                </li>
                                <div class="clear"></div>
                            </ul>
                        </div>
                        <div class="or-text">
                            <div class="more-features-btn"><span>+</span></div>
                        </div>
                    </div>
                    <!-- variant code starts here -->
                    <div class="bw-tabs-data" id="variants">
                        <h2 class="font24 margin-bottom20 text-center">Variants</h2>
                        <asp:Repeater runat="server" ID="rptVarients">
                            <ItemTemplate>
                                <div class="grid-6">
                                    <div class="border-solid content-inner-block-10 margin-bottom20">
                                        <div class="grid-8 alpha">
                                            <h3 class="font16 margin-bottom10"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionName")) %></h3>
                                            <p class="font14"><%# FormatVarientMinSpec(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")),Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                        </div>
                                        <div class="grid-4 omega">
                                            <p class="font18 margin-bottom10"><span class="fa fa-rupee margin-right5"></span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %></p>
                                            <p class="font12 text-light-grey">Ex-showroom, Mumbai</p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="clear"></div>
                    </div>
                    <!-- colours code starts here -->
                    <div class="bw-tabs-data margin-bottom20" id="colours">
                        <div class="border-solid-top margin-left10 margin-right10"></div>
                        <h2 class="font24 margin-top10 margin-bottom20 text-center">Colours</h2>
                        <div class="text-center">
                            <asp:Repeater ID="rptColor" runat="server">
                                <ItemTemplate>
                                    <div class="available-colors">
                                        <div class="color-box" <%# String.Format("style='background-color: #{0}'",Convert.ToString(DataBinder.Eval(Container.DataItem, "HexCode"))) %>></div>
                                        <p class="font16"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ColorName")) %></p>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12 alternative-section">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30"><%= bikeName %> alternatives</h2>
                    <div class="content-box-shadow">
                        <div class="jcarousel-wrapper alternatives-carousel margin-top20">
                            <div class="jcarousel">
                                <ul>
                                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section class="container">
            <!--  News Bikes latest updates code starts here -->
            <div class="newBikes-latest-updates-container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top30 margin-bottom30">Latest updates on <%= bikeName %></h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="bw-tabs bw-tabs-flex" id="reviewCount">
                            <ul>
                                <li class="active" data-tabs="News">News</li>
                                <li data-tabs="expertReviews">Expert Reviews</li>
                                <li data-tabs="userReviews">User Reviews</li>
                                <li data-tabs="Videos">Videos</li>
                            </ul>
                        </div>
                        <div class="bw-tabs-data" id="News">
                            <!-- News data code starts here-->
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="img-preview">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com/600x337/bw/ec/19895/Harley-Davidson-India-56381.jpg?wm=2" title="Acura NSX" alt="Acura NSX"></a>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">Yamaha MT-03 specs and photos revealed</a></h2>
                                    <p class="margin-bottom10 text-xt-light-grey font14">2 hours ago, by <span class="text-light-grey">Sagar Bhanushali</span></p>
                                    <p class="margin-bottom15 font14 line-height">Yamaha’s plans of launching the MT-03 was more of an open secret, but the company had refused to comment on it. Now though, the company has officially...</p>
                                    <div class="margin-bottom15">
                                        <a href="#" class="margin-right25 font14">Read full story</a>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="img-preview">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com//640x348//bw/ec/19895/Harley-Davidson-India-56380.jpg?wm=2" title="Acura NSX" alt="Acura NSX"></a>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">Yamaha MT-03 specs and photos revealed</a></h2>
                                    <p class="margin-bottom10 text-xt-light-grey font14">2 hours ago, by <span class="text-light-grey">Sagar Bhanushali</span></p>
                                    <p class="margin-bottom15 font14 line-height">Yamaha’s plans of launching the MT-03 was more of an open secret, but the company had refused to comment on it. Now though, the company has officially...</p>
                                    <div class="margin-bottom15">
                                        <a href="#" class="margin-right25 font14">Read full story</a>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="img-preview">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com/600x337/bw/ec/19895/Harley-Davidson-India-56381.jpg?wm=2" title="Acura NSX" alt="Acura NSX"></a>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">Yamaha MT-03 specs and photos revealed</a></h2>
                                    <p class="margin-bottom10 text-xt-light-grey font14">2 hours ago, by <span class="text-light-grey">Sagar Bhanushali</span></p>
                                    <p class="margin-bottom15 font14 line-height">Yamaha’s plans of launching the MT-03 was more of an open secret, but the company had refused to comment on it. Now though, the company has officially...</p>
                                    <div class="margin-bottom15">
                                        <a href="#" class="margin-right25 font14">Read full story</a>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom50 text-center">
                                <a href="#" class="font16">View more news</a>
                            </div>
                        </div>
                        <!-- Ends here-->
                        <div class="bw-tabs-data hide" id="expertReviews">
                            <!-- Reviews data code starts here-->
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="img-preview">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com//640x348//bw/ec/19881/Harley-Davidson-Street-750-Front-56351.jpg?wm=0" title="Hyundai Creta" alt="Hyundai Creta"></a>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">Harley-Davidson recalls Street 750 and Street 500</a></h2>
                                    <p class="margin-bottom10 text-xt-light-grey font14">2 hours ago, by <span class="text-light-grey">Sagar Bhanushali</span></p>
                                    <p class="margin-bottom15 font14 line-height">Harley-Davidson has recalled over 10,500 Street 750 and the Street 500 in the US. The recall covers Street 750 motorcycles manufactured between May 12, 2014, and June 24, 2015.</p>
                                    <div class="margin-bottom15">
                                        <a href="#" class="margin-right25 font14">Read More</a>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="img-preview">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com/600x337/bw/ec/19874/Harley-Davidson-Road-King-First-Look-Review-56330.jpg?wm=0" title="Hyundai Creta" alt="Hyundai Creta"></a>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">Harley-Davidson recalls Street 750 and Street 500</a></h2>
                                    <p class="margin-bottom10 text-xt-light-grey font14">2 hours ago, by <span class="text-light-grey">Sagar Bhanushali</span></p>
                                    <p class="margin-bottom15 font14 line-height">Harley-Davidson has recalled over 10,500 Street 750 and the Street 500 in the US. The recall covers Street 750 motorcycles manufactured between May 12, 2014, and June 24, 2015.</p>
                                    <div class="margin-bottom15">
                                        <a href="#" class="margin-right25 font14">Read More</a>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="img-preview">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com//640x348//bw/ec/19881/Harley-Davidson-Street-750-Front-56351.jpg?wm=0" title="Hyundai Creta" alt="Hyundai Creta"></a>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">Harley-Davidson recalls Street 750 and Street 500</a></h2>
                                    <p class="margin-bottom10 text-xt-light-grey font14">2 hours ago, by <span class="text-light-grey">Sagar Bhanushali</span></p>
                                    <p class="margin-bottom15 font14 line-height">Harley-Davidson has recalled over 10,500 Street 750 and the Street 500 in the US. The recall covers Street 750 motorcycles manufactured between May 12, 2014, and June 24, 2015.</p>
                                    <div class="margin-bottom15">
                                        <a href="#" class="margin-right25 font14">Read More</a>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom50 text-center">
                                <a href="#" class="font16">View more reviews</a>
                            </div>
                        </div>
                        <!-- Ends here-->
                        <div class="bw-tabs-data hide" id="userReviews">
                            <!-- Reviews data code starts here-->
                            <div class="user-reviews">
                                <div class="padding-bottom20 font14">
                                    <div class="grid-2">
                                        <div class="content-inner-block-5 border-solid text-center">
                                            <p class="inline-block margin-bottom5 margin-top5">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/half.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                            </p>
                                            <p>3.5</p>
                                        </div>
                                    </div>
                                    <div class="grid-10">
                                        <p class="margin-bottom5 font18 text-bold">Super Awesome, Hell of a beast <span class="font14 text-unbold text-light-grey margin-left5">2 hours ago, by Sagar Bhanushali</span></p>
                                        <p>Honda has made its foray into the lucrative sports touring segment with the CBR650F. This middleweight fully-faired motorcycle has been pegged...<a href="#">Read full story</a></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="padding-bottom20 font14">
                                    <div class="grid-2">
                                        <div class="content-inner-block-5 border-solid text-center">
                                            <p class="inline-block margin-bottom5 margin-top5">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/half.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                            </p>
                                            <p>3.5</p>
                                        </div>
                                    </div>
                                    <div class="grid-10">
                                        <p class="margin-bottom5 font18 text-bold">Super Awesome, Hell of a beast <span class="font14 text-unbold text-light-grey margin-left5">2 hours ago, by Sagar Bhanushali</span></p>
                                        <p>Honda has made its foray into the lucrative sports touring segment with the CBR650F. This middleweight fully-faired motorcycle has been pegged...<a href="#">Read full story</a></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="padding-bottom20 font14">
                                    <div class="grid-2">
                                        <div class="content-inner-block-5 border-solid text-center">
                                            <p class="inline-block margin-bottom5 margin-top5">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/half.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                            </p>
                                            <p>3.5</p>
                                        </div>
                                    </div>
                                    <div class="grid-10">
                                        <p class="margin-bottom5 font18 text-bold">Super Awesome, Hell of a beast <span class="font14 text-unbold text-light-grey margin-left5">2 hours ago, by Sagar Bhanushali</span></p>
                                        <p>Honda has made its foray into the lucrative sports touring segment with the CBR650F. This middleweight fully-faired motorcycle has been pegged...<a href="#">Read full story</a></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="padding-bottom20 font14">
                                    <div class="grid-2">
                                        <div class="content-inner-block-5 border-solid text-center">
                                            <p class="inline-block margin-bottom5 margin-top5">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/1.png" alt="Rate">
                                                <img src="images/ratings/half.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                                <img src="images/ratings/0.png" alt="Rate">
                                            </p>
                                            <p>3.5</p>
                                        </div>
                                    </div>
                                    <div class="grid-10">
                                        <p class="margin-bottom5 font18 text-bold">Super Awesome, Hell of a beast <span class="font14 text-unbold text-light-grey margin-left5">2 hours ago, by Sagar Bhanushali</span></p>
                                        <p>Honda has made its foray into the lucrative sports touring segment with the CBR650F. This middleweight fully-faired motorcycle has been pegged...<a href="#">Read full story</a></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="padding-bottom50 text-center">
                                    <a href="#" class="font16">View more reviews</a>
                                </div>
                            </div>
                        </div>
                        <div class="bw-tabs-data hide" id="Videos">
                            <!-- Videos data code starts here-->
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="yt-iframe-preview">
                                        <iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">First Look Ford Figo Aspire</a></h2>
                                    <p class="margin-bottom10 text-light-grey font14">Updated on <span>June 30, 2015</span></p>
                                    <div class="margin-bottom15 text-light-grey"><span class="bwsprite review-sm-lgt-grey"></span>Views <span>398</span></div>
                                    <div class="text-light-grey"><span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span>Likes <span>120</span></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="yt-iframe-preview">
                                        <iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">First Look Ford Figo Aspire</a></h2>
                                    <p class="margin-bottom10 text-light-grey font14">Updated on <span>June 30, 2015</span></p>
                                    <div class="margin-bottom15 text-light-grey"><span class="bwsprite review-sm-lgt-grey"></span>Views <span>398</span></div>
                                    <div class="text-light-grey"><span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span>Likes <span>120</span></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="padding-bottom30">
                                <div class="grid-4 alpha">
                                    <div class="yt-iframe-preview">
                                        <iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                    </div>
                                </div>
                                <div class="grid-8 omega">
                                    <h2 class="margin-bottom10 font20"><a href="#" class="text-black">First Look Ford Figo Aspire</a></h2>
                                    <p class="margin-bottom10 text-light-grey font14">Updated on <span>June 30, 2015</span></p>
                                    <div class="margin-bottom15 text-light-grey"><span class="bwsprite review-sm-lgt-grey"></span>Views <span>398</span></div>
                                    <div class="text-light-grey"><span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span>Likes <span>120</span></div>
                                </div>
                                <div class="clear"></div>
                            </div>

                            <div class="padding-bottom50 text-center">
                                <a href="#" class="font16">View more videos</a>
                            </div>
                        </div>
                        <!-- Ends here-->
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">

            $(document).ready(function (e) {
                $('.bw-overall-rating a[href^="#"], a[href^="#"].review-count-box').click(function () {
                    $('.bw-overall-rating a').removeClass("active");
                    $(this).addClass("active");
                    var target = $(this.hash);
                    if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
                    if (target.length == 0) target = $('html');
                    $('html, body').animate({ scrollTop: target.offset().top - 50 - $(".header-fixed").height() }, 1000);
                    return false;
                });
                // ends
            });
            // Cache selectors outside callback for performance.
            var $window = $(window),
                $menu = $('.bw-overall-rating'),
                $menu2 = $('.alternative-section'),
                menu2Top = $menu2.offset().top,
                menuTop = $menu.offset().top;
            $window.scroll(function () {
                $menu.toggleClass('affix', menu2Top >= $window.scrollTop() && $window.scrollTop() > menuTop);
            });

            $("a.read-more-btn").click(function () {
                $(".model-about-more-desc").toggle();
                var a = $(this).find("span");
                a.text(a.text() === "more" ? "less" : "more");
            });
            $(".more-features-btn").click(function () {
                $(".more-features").slideToggle();
                var a = $(this).find("span");
                a.text(a.text() === "+" ? "-" : "+");
            });
        </script>
        <script type="text/javascript" src="/src/model.js"></script>
        <script type="text/javascript">
            function pqViewModel(modelId,cityId) {
                var self = this;
                self.cities = ko.observableArray([]);
                self.areas = ko.observableArray([]);
                self.selectedCity = ko.observable(cityId);
                self.selectedArea = ko.observable();
                self.selectedModel = ko.observable(modelId);
                self.priceQuote = ko.observable();
                self.LoadCity = function () {
                    loadCity(self);
                }
                self.LoadArea = function(){
                    loadArea(self);
                }

                self.OnAreaChange = function () {
                    fetchPriceQuote(self);
                }

                self.FetchPriceQuote = function () {
                    fetchPriceQuote(self);
                }
            }

            function loadCity(vm) {
                if (vm.selectedModel()) {
                    $.get("/api/PQCityList/?modelId=" + vm.selectedModel(),
                        function (data) {
                            if (data) {
                                var city = ko.toJS(data);
                                vm.cities(city.cities);
                                $(".city-select-text").removeClass("hide").addClass("show");                                
                            }
                        });
                }
            }

            function loadArea(vm) {                
                if (vm.selectedCity()) {
                    $.get("/api/PQAreaList/?modelId=" + vm.selectedModel() + "&cityId=" + vm.selectedCity())
                    .done(function (data) {
                            if (data) {
                                var area = ko.toJS(data);
                                vm.areas(area.areas);
                                $(".city-select-text").removeClass("show").addClass("hide");
                                $(".area-select-text").removeClass("hide").addClass("show");
                            }
                            else {
                                vm.areas([]);
                                $(".area-select-text").removeClass("show").addClass("hide");
                                vm.FetchPriceQuote();
                            }
                    })
                    .fail(function () {
                        vm.areas([]);
                        $(".area-select-text").removeClass("show").addClass("hide");
                        vm.FetchPriceQuote();
                    });
                }
                else {
                    vm.areas([]);
                    $(".city-area-wrapper").removeClass("hide").addClass("show");
                    $(".city-select").removeClass("hide").addClass("show");
                    $(".area-select").removeClass("show").addClass("hide");
                    $(".city-select-text").removeClass("hide").addClass("show");
                    $(".area-select-text").removeClass("show").addClass("hide");
                    $(".city-onRoad-price-container").removeClass("show").addClass("hide");
                    $(".unveil-offer-btn-container").attr('style', '');
                    $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
                    $(".default-showroom-text").html("");
                }                    
            }

            function fetchPriceQuote(vm) {
                var clientIP = '<%= clientIP%>';         
                $("#dvAvailableOffer").empty();
                if (vm.selectedModel() && vm.selectedCity()) {                    
                    $.get("/api/OnRoadPrice/?cityId=" + vm.selectedCity() + "&modelId=" + vm.selectedModel() + "&clientIP=" + clientIP + "&sourceType=" + 1 + "&areaId=" + (vm.selectedArea() ? vm.selectedArea() : ""))
                    .done(function (data) {
                        if (data) {
                            var pq = ko.toJS(data);
                            vm.priceQuote(pq);
                            if (pq && pq.IsDealerPriceAvailable) {
                                $(".unveil-offer-btn-container").attr('style', '');
                                $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
                                var totalPrice = 0;
                                var priceBreakText = '';
                                for (var i = 0; i < pq.dealerPriceQuote.priceList.length; i++) {
                                    totalPrice += pq.dealerPriceQuote.priceList[i].price;
                                    priceBreakText += pq.dealerPriceQuote.priceList[i].categoryName + " + "
                                }
                                priceBreakText = priceBreakText.substring(0, priceBreakText.length - 2);
                                $("#bike-price").html(totalPrice);
                                $("#breakup").text("(" + priceBreakText + ")");
                                $("#pqCity").html($("#ddlCity option[value=" + vm.selectedCity() + "]").text())
                                $("#pqArea").html($("#ddlArea option[value=" + vm.selectedArea() + "]").text())
                                $(".city-select-text").removeClass("show").addClass("hide");
                                $(".area-select-text").removeClass("show").addClass("hide");
                                $(".city-onRoad-price-container").removeClass("hide").addClass("show");
                                $(".city-area-wrapper").addClass("hide");
                                if (pq.dealerPriceQuote.offers && pq.dealerPriceQuote.offers.length > 0) {
                                    $('.available-offers-container').removeClass("hide").addClass("show");
                                    $("#dvAvailableOffer").append("<ul id='dpqOffer' data-bind=\"foreach: priceQuote().dealerPriceQuote.offers\"><li data-bind=\"text: offerText\"></li></ul>");
                                    ko.applyBindings(vm, $("#dpqOffer")[0]);
                                }
                                else {
                                    $('.available-offers-container').removeClass("hide").addClass("show");
                                    $("#dvAvailableOffer").append("<ul><li>No offers available</li></ul>");
                                }
                                $(".default-showroom-text").html("+ View Breakup");
                            }                            
                            else {
                                if(pq.bwPriceQuote.onRoadPrice > 0) {
                                    totalPrice = pq.bwPriceQuote.onRoadPrice;
                                    priceBreakText = "Ex-showroom + Insurance + RTO";
                                }                                
                                $("#bike-price").html(totalPrice);
                                $("#breakup").text("(" + priceBreakText + ")");
                                $(".unveil-offer-btn-container").attr('style', '');
                                $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
                                $(".city-onRoad-price-container").removeClass("show").addClass("hide");
                                $(".city-select-text").removeClass("hide").addClass("show");
                                $(".area-select-text").removeClass("show").addClass("hide");
                                $(".city-area-wrapper").removeClass("hide").addClass("show");
                                $(".city-select").removeClass("hide").addClass("show");
                                $(".area-select").removeClass("show").addClass("hide");
                                $('.available-offers-container').removeClass("hide").addClass("show");
                                $("#dvAvailableOffer").empty();
                                $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
                            }
                            $(".default-showroom-text").html("+ View Breakup");
                        }
                        else {
                            vm.areas([]);
                            $(".unveil-offer-btn-container").attr('style', '');
                            $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
                            $(".city-onRoad-price-container").removeClass("show").addClass("hide");
                            $(".city-select-text").removeClass("hide").addClass("show");
                            $(".area-select-text").removeClass("show").addClass("hide");
                            $(".city-area-wrapper").removeClass("hide").addClass("show");
                            $(".city-select").removeClass("hide").addClass("show");
                            $(".area-select").removeClass("show").addClass("hide");
                            $('.available-offers-container').removeClass("hide").addClass("show");
                            $("#dvAvailableOffer").empty();
                            $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
                        }
                    })
                    .fail(function () {
                        vm.areas([]);
                        $(".unveil-offer-btn-container").attr('style', '');
                        $(".unveil-offer-btn-container").removeClass("show").addClass("hide");
                        $(".city-onRoad-price-container").removeClass("show").addClass("hide");
                        $(".city-select-text").removeClass("hide").addClass("show");
                        $(".area-select-text").removeClass("show").addClass("hide");
                        $(".city-area-wrapper").removeClass("hide").addClass("show");
                        $(".city-select").removeClass("hide").addClass("show");
                        $(".area-select").removeClass("show").addClass("hide");
                        $('.available-offers-container').removeClass("hide").addClass("show");
                        $("#dvAvailableOffer").empty();
                        $("#dvAvailableOffer").append("<ul><li>Currently there are no offers in your city. We hope to serve your city soon!</li></ul>");
                    });
                }
            }

            $(document).ready(function () {
                InitVM(0);
                $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
                $(".unveil-offer-btn-container").attr('style', '');
            });

            $("#mainCity li").click(function () {
                var val = $(this).attr('cityId');                
                    $("#city-list-container").removeClass("show").addClass("hide");
                    $(".city-select-text").removeClass("hide").addClass("show");
                    $("#city-area-select-container").removeClass("hide").addClass("show");
                    $(".offer-error").removeClass("show").addClass("hide");                                        
                    $(".area-select").removeClass("show").addClass("hide");
                    $(".city-select").removeClass("hide").addClass("show");
                    $(".city-area-wrapper").removeClass("hide").addClass("show");
                    $(".city-onRoad-price-container").removeClass("show").addClass("hide");
                    $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
                    if (val) {
                        $("#ddlCity option[value=" + val + "]").attr('selected', 'selected');
                        $('#ddlCity').trigger('change');
                        $(".area-select").removeClass("hide").addClass("show");
                    }
            });

            $(".city-edit-btn").click(function () {
                if ($("#ddlCity").val() && $("#ddlArea").val()) {
                    $(".city-select-text").removeClass("hide").addClass("show");
                    $(".area-select").addClass("hide");
                    $(".city-onRoad-price-container").removeClass("show").addClass("hide");                    
                }
                $(".available-offers-container").removeClass("show").addClass("hide");
                $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
            });

            function InitVM(cityId) {
                var viewModel = new pqViewModel('<%= modelId%>',cityId);
                ko.applyBindings(viewModel, $('#dvBikePrice')[0]);
                viewModel.LoadCity();
            }

        </script>
    </form>
</body>
</html>

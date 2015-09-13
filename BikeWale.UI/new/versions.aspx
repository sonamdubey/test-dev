<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="versions.aspx.cs" Inherits="Bikewale.New.versions" %>

<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/UserReviewsList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/controls/PopupWidget.ascx" TagPrefix="BW" TagName="PriceQuotePopup" %>
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/<%= modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/"><%= modelPage.ModelDetails.MakeBase.MakeName %></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= modelPage.ModelDetails.ModelName %></li>
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
                            <div class="<%= modelPage.ModelDetails.Futuristic ? "" : "hide" %>">
                                <span class="model-sprite bw-upcoming-bike-ico"></span>
                            </div>
                            <div class="<%= !modelPage.ModelDetails.Futuristic && !modelPage.ModelDetails.New ? "" : "hide" %>">
                                <span class="model-sprite bw-discontinued-bike-ico"></span>
                            </div>
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
                            <% if(modelPage.ModelDetails.New) { %>
                            <div class="margin-top20 <%= modelPage.ModelDetails.Futuristic ? "hide" : "" %>">
                                <p class="margin-left50	leftfloat margin-right20">
                                    <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPage.ModelDetails.ReviewRate)) %>
                                </p>
                                <a href="<%= FormatShowReview(modelPage.ModelDetails.MakeBase.MaskingName,modelPage.ModelDetails.MaskingName) %>" class="review-count-box border-solid-left leftfloat margin-right20 padding-left20"><%= modelPage.ModelDetails.ReviewCount %> Reviews
                                </a>
                                <a href="<%= FormatWriteReviewLink() %>" class="border-solid-left leftfloat margin-right20 padding-left20">Write a review
                                </a>
                                <div class="clear"></div>
                            </div>
                            <% } %>
                        </div>
                        <div class="grid-6 padding-left40" id="dvBikePrice">
                            <% if (!modelPage.ModelDetails.Futuristic)
                               { %>
                            <div class="bike-price-container font28 margin-bottom15">
                                <span class="fa fa-rupee"></span>
                                <span id="bike-price" class="font30 text-black"><%= Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MinPrice.ToString()) %></span> <span class="font12 text-light-grey default-showroom-text">Ex-showroom <%= ConfigurationManager.AppSettings["defaultName"] %></span>
                                <!-- View BreakUp Popup Starts here-->


                                <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                                    <div class="breakupCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                    <div class="breakup-text-container padding-bottom10">
                                        <h3 class="breakup-header font26 margin-bottom20"><%= bikeName %> <span class="font14 text-light-grey ">(On road price breakup)</span></h3>

                                        <!-- ko if : !isDealerPQAvailable() && BWPriceList -->
                                        <table class="font16">
                                            <tbody>
                                                <tr>
                                                    <td width="350" class="padding-bottom10">Ex-showroom (Mumbai)</td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: BWPriceList().exShowroomPrice"></span></td>
                                                </tr>
                                                <tr>
                                                    <td class="padding-bottom10">RTO</td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: BWPriceList().rto"></span></td>
                                                </tr>
                                                <tr>
                                                    <td class="padding-bottom10">Insurance (comprehensive)</td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: BWPriceList().insurance"></span></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="border-solid-top padding-bottom10"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!-- ko if :BWPriceList -->
                                                    <td class="padding-bottom10 text-bold">Total on road price</td>
                                                    <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: (parseInt(BWPriceList().insurance) + parseInt(BWPriceList().rto) + parseInt(BWPriceList().exShowroomPrice))"></span></td>
                                                    <!-- /ko -->
                                                </tr>
                                            </tbody>
                                        </table>
                                        <!-- /ko -->

                                        <!-- ko if : isDealerPQAvailable() -->
                                        <table class="font16">
                                            <tbody>
                                                <!-- ko foreach : DealerPriceList -->
                                                <tr>
                                                    <td width="350" class="padding-bottom10" data-bind="text: categoryName"></td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span> <span data-bind="text: price"></span></td>
                                                </tr>
                                                <!-- /ko  -->
                                                <!-- ko if :DealerPriceList -->
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="border-solid-top padding-bottom10"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="padding-bottom10">Total on road price</td>
                                                    <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span>  <span data-bind="text: DealerOnRoadPrice "></span></td>
                                                </tr>
                                                 <!-- /ko -->  
                                                <tr>
                                                    <td class="padding-bottom10">Minus insurance</td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span> 0</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="border-solid-top padding-bottom10"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!-- ko if :DealerPriceList -->
                                                    <td class="padding-bottom10 text-bold">Total on road price</td>
                                                    <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span> <span data-bind="text: DealerOnRoadPrice "></span></td>
                                                    <!-- /ko -->                                                    
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="border-solid-top padding-bottom10"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <!-- /ko -->

                                    </div>
                                </div>

                                <!--View Breakup popup ends here-->
                            </div>
                            <% if (!modelPage.ModelDetails.New)
                               { %>
                                    <div class="margin-top20 <%= modelPage.ModelDetails.Futuristic ? "hide" : "" %>">
                                        <p class="leftfloat margin-right20">
                                            <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPage.ModelDetails.ReviewRate)) %>
                                        </p>
                                        <a href="<%= FormatShowReview(modelPage.ModelDetails.MakeBase.MaskingName,modelPage.ModelDetails.MaskingName) %>" class="review-count-box border-solid-left leftfloat margin-right20 padding-left20"><%= modelPage.ModelDetails.ReviewCount %> Reviews
                                        </a>
                                        <a href="<%= FormatWriteReviewLink() %>" class="border-solid-left leftfloat margin-right20 padding-left20">Write a review
                                        </a>
                                        <div class="clear"></div>
                                    </div>                                    
                                    <div class="margin-top20 bike-price-container margin-bottom15">                                                                                        
                                        <span class="font14 text-light-grey default-showroom-text"><%= bikeName %> is discontinued in India.</span>                                                                        
                            </div>
                                    <div class="clear"></div>
                            <% } %>
                            <% if (modelPage.ModelDetails.New)
                               { %>
                            <div id="city-list-container" class="city-list-container margin-bottom20">
                                <div class="text-left margin-bottom15">
                                    <p class="font16 offer-error">Select city for accurate on-road price and exclusive offers</p>
                                </div>
                                <ul id="mainCity">
                                    <li cityid="1"><span>Mumbai</span></li>
                                    <li cityid="12"><span>Pune</span></li>
                                    <li cityid="2"><span>Banglore</span></li>
                                    <li cityid="40"><span>Thane</span></li>
                                    <li cityid="13"><span>Navi Mumbai</span></li>
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
                                    <input type="button" class="btn btn-orange" id="btnBookNow" value="Avail offers" />
                                </div>
                                <div class="city-area-wrapper">
                                    <div class="city-select leftfloat margin-right20">
                                        <select id="ddlCity" data-bind="options: cities, optionsText: 'cityName', optionsValue: 'cityId', value: selectedCity, optionsCaption: 'Select City', event: { change: LoadArea }"></select>
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
                                    <input type="button" class="btn btn-orange unveil-offer-btn" value="Show Offers" />
                                </div>
                                <div class="notify-btn-container position-abt pos-left0 pos-top0 hide">
                                    <div class="margin-top50 margin-left40">
                                        <input type="text" placeholder="Notify me" class="notify-input">
                                        <input type="button" class="btn btn-orange btn-xs" value="Notify me" />
                                    </div>
                                </div>
                            </div>
                            <% } %>
                            <% } %>
                            <% if (modelPage.ModelDetails.Futuristic && modelPage.UpcomingBike != null)
                               { %>
                            <div class="upcoming-bike-details-container margin-top30">
                                <div class="upcoming-bike-price-container font28 margin-bottom20">
                                    <span class="fa fa-rupee"></span>
                                    <span id="bike-price" class="font30 text-black"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %></span>
                                    <span class="font30 text-black">-</span>
                                    <span class="fa fa-rupee"></span>
                                    <span id="bike-price" class="font30 text-black"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                                    <span class="font12 text-light-grey default-showroom-text">Expected price</span>
                                </div>
                                <div class="upcoming-bike-date-container margin-bottom20">
                                    <span class="font20 text-black"><%= modelPage.UpcomingBike.ExpectedLaunchDate %></span>
                                    <span class="font12 text-light-grey">Expected launch date</span>
                                </div>
                                <div class="upcoming-bike-default-text">
                                    <p class="font14"><%= bikeName %> is not launched in India yet. Information on this page is tentative.</p>
                                </div>
                            </div>
                            <% } %>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section class="container <%= (modelPage.ModelDesc == null || string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription)) ? "hide" : "" %>">
            <div id="SneakPeak" class="grid-12 margin-bottom20">
                <h2 class="text-bold text-center margin-top20 margin-bottom30">Sneak-peak</h2>
                <div class="content-box-shadow content-inner-block-20">
                    <p class="font14 text-grey padding-left10 padding-right10">
                        <span class="model-about-main">
                            <%= modelPage.ModelDesc.SmallDescription %>
                        </span>
                        <span class="model-about-more-desc hide" style="display: none;">
                            <%= modelPage.ModelDesc.FullDescription %>
                        </span>
                        <span><a href="#SneakPeak" class="read-more-btn">Read <span>more</span></a></span>
                    </p>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <% if (modelPage.ModelVersionSpecs != null)
           { %>
        <section class="container">
            <!--  Discover bikes section code starts here -->
            <div class="grid-12">
                <div class="content-box-shadow content-inner-block-10">
                    <div class="bw-overall-rating">
                        <a class="active" href="#overview">Overview</a>
                        <a href="#specifications">Specifications</a>
                        <a href="#features">Features</a>
                        <a href="#variants" style="<%= (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0) ? "": "display:none;" %>">Variants</a>
                        <a href="#colours" style="<%= (modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0) ? "": "display:none;" %>">Colours</a>
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
                        <%--<p class="font14 margin-top20 text-grey padding-left10 padding-right10 <%= string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription) ? "hide" : "" %>">--%>
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
                                    <div class="text-light-grey">Tachometer</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Shift Light</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ShiftLight) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">No Of Tripmeters</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfTripmeters) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tripmeter Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TripmeterType) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Low Fuel Indicator</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowFuelIndicator) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Low Oil Indicator</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowOilIndicator) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Low Battery Indicator</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowBatteryIndicator) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Pillion Seat</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionSeat) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Pillion Footrest</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionFootrest) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Pillion Backrest</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionBackrest) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Pillion Grabrail</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionGrabrail) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Stand Alarm</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.StandAlarm) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Stepped Seat</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.SteppedSeat) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Antilock Braking System</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.AntilockBrakingSystem) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Killswitch</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Killswitch) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Clock</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Clock) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Electric System</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ElectricSystem) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Battery</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Battery) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Headlight Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.HeadlightType) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Headlight Bulb</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.HeadlightBulbType) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Brake/Tail Light</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Brake_Tail_Light) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Turn Signal</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TurnSignal) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Pass Light</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PassLight) %></div>
                                </li>
                                <div class="clear"></div>
                            </ul>
                        </div>
                        <div class="or-text">
                            <div class="more-features-btn"><span>+</span></div>
                        </div>
                    </div>
                    <!-- variant code starts here -->
                    <div class="bw-tabs-data <%= modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0 ? "" : "hide" %>" id="variants">
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
                    <div class="bw-tabs-data margin-bottom20 <%= modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0 ? "" : "hide" %>" id="colours">
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
        <% } %>
        <section class="<%= (Convert.ToInt32(ctrlAlternativeBikes.FetchedRecordsCount) > 0) ? "" : "hide" %>">
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
        <% 
            if (ctrlNews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlExpertReviews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlVideos.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlUserReviews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
        %>
        <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News Bikes latest updates code starts here -->
            <div class="newBikes-latest-updates-container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top30 margin-bottom30">Latest updates on <%= bikeName %></h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                <ul>
                                    <li class="active" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>
                                    <li style="<%= (Convert.ToInt32(ctrlUserReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlUserReviews">User Reviews</li>
                                    <li style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                </ul>
                            </div>
                        </div>
                        <BW:News runat="server" ID="ctrlNews" />
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                        <BW:Videos runat="server" ID="ctrlVideos" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <BW:PriceQuotePopup ID="ctrlPriceQuotePopup" runat="server" />
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

            <% if (!modelPage.ModelDetails.Futuristic)
               { %>
            var $window = $(window),
                $menu = $('.bw-overall-rating'),
                $menu2 = $('.alternative-section'),
                menu2Top = $menu2.offset().top,
                menuTop = $menu.offset().top;
            $window.scroll(function () {
                $menu.toggleClass('affix', menu2Top >= $window.scrollTop() && $window.scrollTop() > menuTop);
            });
            <% } %>
            $("a.read-more-btn").click(function () {
                $(".model-about-more-desc").slideToggle();
                $(".model-about-main").toggle();
                var a = $(this).find("span");
                a.text(a.text() === "more" ? "less" : "more");
            });
            $(".more-features-btn").click(function () {
                $(".more-features").slideToggle();
                var a = $(this).find("span");
                a.text(a.text() === "+" ? "-" : "+");
            });

            $("div#dvBikePrice").on('click','span.view-breakup-text', function () {
                faqPopupShow();
            });

            $(".breakupCloseBtn,.blackOut-window").mouseup(function () {
                $("div#breakupPopUpContainer").hide();
                $(".blackOut-window").hide();
            });

            function faqPopupShow() {
                $("div#breakupPopUpContainer").show();
                $(".blackOut-window").show();
            };

        </script>
        <script type="text/javascript" src="/src/model.js"></script>
        <script type="text/javascript">
            var PQCitySelectedId = 0;
            var PQCitySelectedName = ""; 
            function pqViewModel(modelId, cityId) {
                var self = this;
                self.cities = ko.observableArray([]);
                self.areas = ko.observableArray([]);
                self.selectedCity = ko.observable(cityId);
                self.selectedArea = ko.observable();
                self.selectedModel = ko.observable(modelId);
                self.priceQuote = ko.observable();
                self.DealerPriceList = ko.observableArray([]);
                self.BWPriceList = ko.observable();
                self.isDealerPQAvailable = ko.observable(false);
                self.DealerOnRoadPrice = ko.computed(function () {
                    var total = 0;
                    for (i = 0; i < self.DealerPriceList().length; i++) {
                        total += self.DealerPriceList()[i].price;
                    }
                    return total;
                }, this);
                self.LoadCity = function () {
                    loadCity(self);
                }
                self.LoadArea = function () {
                    loadArea(self);
                }

                self.OnAreaChange = function () {
                    fetchPriceQuote(self);
                }

                self.FetchPriceQuote = function () {
                    fetchPriceQuote(self);
                }
            }

            function loadDealerBreakUp(vm)
            {
                if(vm.DealerPriceList!=null && vm.DealerPriceList.length > 0)
                {
                    total = 0;
                    for(i=0;i<vm.DealerPriceList.length;i++)
                    {
                        total += vm.DealerPriceList.price;
                    }
                    alert(total);
                    return total;
                }
            }

            function loadCity(vm) {
                if (vm.selectedModel()) {
                    $.get("/api/PQCityList/?modelId=" + vm.selectedModel(),
                        function (data) {
                            if (data) {
                                var city = ko.toJS(data);
                                var citySelectedNow = null;
                                vm.cities(city.cities);
                                PQcheckCookies();
                                for (i = 0; i < city.cities.length;i++)
                                {
                                    c = city.cities[i].cityId;
                                    console.log(PQCitySelectedId + "    " + c);
                                    if (PQCitySelectedId == c )
                                    {
                                        console.log(PQCitySelectedId + "    " + c);
                                        citySelectedNow = city.cities[i];
                                        break;
                                    }
                                }

                                if (citySelectedNow!=null)
                                {
                                    vm.selectedCity(citySelectedNow.cityId);
                                    loadArea(vm);
                                    $(".city-select-text").removeClass("hide").addClass("show");
                                    if (parseInt($('#mainCity li[cityId' + citySelectedNow.cityId + ']').attr('cityId')) > 0)
                                    {
                                        $('#mainCity li[cityId' + citySelectedNow.cityId + ']').click();
                                    }
                                    else {
                                        $('#mainCity li:last-child').click();
                                    }
                                    
                                    if (vm.areas.length > 0)
                                    {
                                        $(".area-select-text").removeClass("hide").addClass("show");
                                        fetchPriceQuote(vm);
                                    }
                                       

                                    
                                }
                                else {
                                    $(".city-select-text").removeClass("hide").addClass("show");
                                }
                                
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
                    $(".default-showroom-text").html("").removeClass('view-breakup-text');
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
                            vm.isDealerPQAvailable(pq.IsDealerPriceAvailable);
                            if (pq.IsDealerPriceAvailable) {
                                vm.DealerPriceList(pq.dealerPriceQuote.priceList);                                
                                vm.isDealerPQAvailable(pq.IsDealerPriceAvailable);
                            }
                            else {
                                vm.BWPriceList(pq.bwPriceQuote);
                            }

                            if (pq && pq.IsDealerPriceAvailable) {
                                var cookieValue = "CityId=" + vm.selectedCity() + "&AreaId=" + vm.selectedArea() + "&PQId=" + pq.priceQuote.quoteId + "&VersionId=" + pq.priceQuote.versionId + "&DealerId=" + pq.priceQuote.dealerId;
                                SetCookie("_MPQ", cookieValue);
                                SetCookieInDays("location", vm.selectedCity() + '_' + pq.bwPriceQuote.city);
                                $("#btnBookNow").show();
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
                                $(".default-showroom-text").html("+ View Breakup").addClass('view-breakup-text');
                            }
                            else {
                                if (pq.bwPriceQuote.onRoadPrice > 0) {
                                    totalPrice = pq.bwPriceQuote.onRoadPrice;
                                    priceBreakText = "Ex-showroom + Insurance + RTO";
                                }
                                $("#btnBookNow").hide();
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
                <% if(modelPage.ModelDetails.New) { %>                
                var cityId = '<%= cityId%>'
                InitVM(cityId);
                <% } %>
                $(".unveil-offer-btn-container").removeClass("hide").addClass("show");
                $(".unveil-offer-btn-container").attr('style', '');

                $("#btnBookNow").on("click", function () {
                    window.location.href = "/pricequote/bookingsummary_new.aspx";
                });
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
                var viewModel = new pqViewModel('<%= modelId%>', cityId);
                ko.applyBindings(viewModel, $('#dvBikePrice')[0]);
                viewModel.LoadCity();
            }            

            function PQcheckCookies()
            {
                c = document.cookie.split('; ');
                for(i=c.length-1; i>=0; i--)
                {
                    C = c[i].split('=');
                    if(C[0]=="location")
                    {
                        var cData = (String(C[1])).split('_');
                        PQCitySelectedId = parseInt(cData[0]);
                        PQCitySelectedName = cData[1];
                    }
                } 
            }

        </script>
    </form>
</body>
</html>

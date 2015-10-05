<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.BikeModels" %>

<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="/m/controls/UserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <%
        title = modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " Price in India - Rs."
                    + Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MinPrice.ToString()) + " - " + Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MaxPrice.ToString())
                    + ". Check out " + modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " on road price, reviews, mileage, variants, news & photos at Bikewale.";

        canonical = "http://www.bikewale.com/" + modelPage.ModelDetails.MakeBase.MaskingName + "-bikes/" + modelPage.ModelDetails.MaskingName + "/";
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1017752";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        Ad_300x250 = true;
        TargetedModel = modelPage.ModelDetails.ModelName;
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-model.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">        
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container bg-white clearfix">
                <div class="<%= !modelPage.ModelDetails.New ? "padding-top20 position-rel" : ""%>">
                    <% if (modelPage.ModelDetails.New)
                       { %><h1 class="padding-top15 padding-bottom20 padding-left20 padding-right20"><%= bikeName %></h1>
                    <% } %>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %><div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Upcoming</div>
                    <% } %>
                    <% if (!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                       { %><div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Discontinued</div>
                    <% } %>
                    <div class="jcarousel-wrapper model">
                        <div class="jcarousel">
                            <ul id="ulModelPhotos">
                                <li>
                                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath,modelPage.ModelDetails.HostUrl,Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName %>" alt="<%= bikeName %>" />
                                </li>
                                <asp:Repeater ID="rptModelPhotos" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="http://img1.aeplcdn.com/grey.gif" />
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                        <% if (modelPage.Photos != null && modelPage.Photos.Count > 1)
                           { %>
                        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <% } %>
                    </div>
                    <% if (modelPage.ModelDetails.New)
                       { %>
                    <div class="padding-left10 padding-right10">
                        <p class="leftfloat margin-right10 rating-wrap">
                            <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble((modelPage.ModelDetails == null || modelPage.ModelDetails.ReviewRate == null) ? 0 : modelPage.ModelDetails.ReviewRate )) %>
                        </p>
                        <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> border-solid-left leftfloat margin-right10 padding-left10 line-Ht22">
                            <%= modelPage.ModelDetails.ReviewCount %> Reviews
                        </a>
                        <div class="clear"></div>
                    </div>
                    <% } %>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="bikeDescWrapper text-center">
                        <div class="bikeTitle">
                            <h1 class="padding-bottom10"><%= bikeName %></h1>
                        </div>
                        <div class="font22 text-grey">
                            <span class="fa fa-rupee"></span>
                            <span class="font24"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                        </div>
                        <div class="margin-bottom10 font12 text-light-grey">Expected price</div>
                        <div class="font18 text-grey margin-bottom5 margin-top15">
                            <span><%= modelPage.UpcomingBike.ExpectedLaunchDate %></span>
                        </div>
                        <div class="margin-bottom10 font12 text-light-grey">Expected launch date</div>
                        <p class="font14 text-grey"><%= bikeName %> is not launched in India yet. Information on this page is tentative.</p>
                    </div>
                    <% } %>
                </div>
                <% if (modelPage.ModelDetails.New)
                   { %>
                <div class="grid-12 bg-white box-shadow" id="dvBikePrice">
                    <div class="bike-price-container font22 margin-bottom15">
                        <span class="fa fa-rupee"></span>
                        <span id="bike-price" class="font24 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPage.ModelDetails.MinPrice)) %></span> <span class="font10 default-showroom-text">Ex-showroom <%= Bikewale.Common.Configuration.GetDefaultCityName %></span>
                        <!-- View BreakUp Popup Starts here-->
                        <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                            <div class="breakupCloseBtn position-abt pos-top10 pos-right10 bwmsprite  cross-lg-lgt-grey cur-pointer"></div>
                            <div class="breakup-text-container padding-bottom10">
                                <h3 class="breakup-header font26 margin-bottom20"><%= bikeName %> <span class="font14 text-light-grey ">(On road price breakup)</span></h3>

                                <!-- ko if : !isDealerPQAvailable() && BWPriceList -->
                                <table class="font14" width="100%">
                                    <tbody>
                                        <tr>
                                            <td width="60%" class="padding-bottom10">Ex-showroom (Mumbai)</td>
                                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(BWPriceList().exShowroomPrice)"></span></td>
                                        </tr>
                                        <tr>
                                            <td class="padding-bottom10">RTO</td>
                                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(BWPriceList().rto)"></span></td>
                                        </tr>
                                        <tr>
                                            <td class="padding-bottom10">Insurance (comprehensive)</td>
                                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(BWPriceList().insurance)"></span></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!-- ko if :BWPriceList -->
                                            <td class="padding-bottom10 text-bold">Total on road price</td>
                                            <td align="right" class="padding-bottom10 font20 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(parseInt(BWPriceList().insurance) + parseInt(BWPriceList().rto) + parseInt(BWPriceList().exShowroomPrice))"></span></td>
                                            <!-- /ko -->
                                        </tr>
                                    </tbody>
                                </table>
                                <!-- /ko -->

                                <!-- ko if : isDealerPQAvailable() -->
                                <table class="font14" width="100%">
                                    <tbody>
                                        <!-- ko foreach : DealerPriceList -->
                                        <tr>
                                            <td width="60%" class="padding-bottom10" data-bind="text: categoryName"></td>
                                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(price)"></span></td>
                                        </tr>
                                        <!-- /ko  -->
                                        <!-- ko if : priceQuote().isInsuranceFree  && priceQuote().insuranceAmount > 0 -->
                                        <tr>
                                            <td colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="padding-bottom10">Total on road price</td>
                                            <td align="right" class="padding-bottom10 text-bold text-right" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(DealerOnRoadPrice()) "></span></td>
                                        </tr>

                                        <tr>
                                            <td class="padding-bottom10">Minus insurance</td>
                                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(priceQuote().insuranceAmount)"></span></td>
                                        </tr>
                                        <!-- /ko -->
                                        <tr>
                                            <td colspan="2">
                                                <div class="border-solid-top padding-bottom10"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!-- ko if : DealerPriceList -->
                                            <td class="padding-bottom10 text-bold">Total on road price</td>
                                            <td align="right" class="padding-bottom10 font20 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: ((priceQuote().insuranceAmount > 0) ? $root.FormatPricedata((DealerOnRoadPrice() - priceQuote().insuranceAmount)) : $root.FormatPricedata(DealerOnRoadPrice())) "></span></td>
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
                    <div class="bike-price-container font22 margin-bottom15 hide">
                        <span class="font24 text-bold ">Price not available</span>
                    </div>
                     <!-- ko if : !popularCityClicked()-->
                    <div id="city-list-container" class="city-list-container margin-bottom10 " >
                        <div class="text-left margin-bottom15">
                            <p class="font14 offer-error">Select city for accurate on-road price and exclusive offers</p>
                        </div>
                        <ul id="mainCity">
                            <li cityid="1"><span>Mumbai</span></li>
                            <li cityid="12"><span>Pune</span></li>
                            <li cityid="2"><span>Bangalore</span></li>
                            <li cityid="40"><span>Thane</span></li>
                            <li cityid="13"><span>Navi Mumbai</span></li>
                            <li class="city-other-btn"><span>Others</span></li>
                        </ul>
                    </div>
                    <!-- /ko -->
                    <div id="city-area-select-container" class="city-area-select-container margin-bottom20 " data-bind="visible: popularCityClicked()">
                        <div class="city-select-text text-left margin-bottom15 "  data-bind="visible: !selectedCity() || cities()">
                            <p class="font14">Select city for accurate on-road price and exclusive offers</p>
                        </div>
                        <!-- ko if : selectedCity() && areas()  && areas().length > 0-->
                        <div class="area-select-text text-left margin-bottom15 ">
                            <p class="font14">Select area for on-road price and exclusive offers</p>
                        </div>
                        <!-- /ko -->
                        <!-- ko if : BWPriceList() || DealerPriceList() -->
                        <div class="city-onRoad-price-container font14 margin-bottom15 hide">
                            <p class="margin-bottom10">On-road price in <span id="pqArea"></span><span id="pqCity"></span><span class="city-edit-btn font12 margin-left10" <%--data-bind="click: $root.EditButton"--%>>change location</span></p>
                            <p class="font12 margin-bottom15"></p>
                            <!-- ko if : priceQuote() && priceQuote().IsDealerPriceAvailable && priceQuote().dealerPriceQuote.offers.length > 0 -->
                            <input type="button" class="btn btn-orange btn-full-width" id="btnBookNow" data-bind="event: { click: $root.availOfferBtn }" value="Avail Offers" />
                        	<!-- /ko -->
                        </div>
                        <!-- /ko -->
                        <div class="city-area-wrapper">
                        	<!-- ko if : cities()  && cities().length > 0 -->  
                            <div class="city-select position-rel">
                                <select id="ddlCity" class="form-control " data-bind="options: cities, optionsText: 'cityName', optionsValue: 'cityId', value: selectedCity, optionsCaption: 'Select City'"></select>
                            	<span class="fa fa-spinner fa-spin position-abt pos-right5 pos-top15 text-black bg-white" style="display:none"></span>
                            </div>
                            <!-- /ko -->
                            <!-- ko if : selectedCity() && areas()  && areas().length > 0 -->
                            <div class="area-select margin-top20 position-rel">
                                <select id="ddlArea" class="form-control" data-bind="options: areas, optionsText: 'areaName', optionsValue: 'areaId', value: selectedArea, optionsCaption: 'Select Area'"></select>
                            	<span class="fa fa-spinner fa-spin position-abt pos-right5 pos-top15 text-black bg-white" style="display:none"></span>
                            </div>
                            <!-- /ko -->
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div id="offersBlock" class="city-unveil-offer-container position-rel margin-top20 margin-bottom20">
                        <div class="available-offers-container content-inner-block-10">
                            <h4 class="border-solid-bottom padding-bottom5 margin-bottom5">Available Offers</h4>
                            <div class="offer-list-container" id="dvAvailableOffer">
                             <!-- ko if:priceQuote() -->
	                         <!-- ko if : priceQuote().IsDealerPriceAvailable  -->
	                        	<ul data-bind="visible: priceQuote().dealerPriceQuote.offers.length > 0, foreach: priceQuote().dealerPriceQuote.offers">
	                            <li data-bind="text: offerText"></li>
	                        </ul>
	                        <ul data-bind="visible: priceQuote().dealerPriceQuote.offers.length == 0">
	                            <li >No offers available</li>
	                        </ul>
	                        <!-- /ko -->
	                         <!-- ko if : !priceQuote().IsDealerPriceAvailable -->
	                        <ul >
                                <li data-bind="visible: areas() && areas().length > 0">
                                Currently there are no offers in your area. We hope to serve your area soon!
                                </li> 
                                <li data-bind="visible: !(areas() && areas().length > 0)">
                                Currently there are no offers in your city. We hope to serve your city soon!
                                </li> 
                            </ul>
	                        <!-- /ko -->
	                        <!-- /ko -->
                            </div>
                        </div>
                        <div class="unveil-offer-btn-container position-abt pos-left0 pos-top0 text-center">
                            <input type="button" id="btnShowOffers"  class="btn btn-md btn-orange unveil-offer-btn" value="Show Offers" />
                        </div>                        
                    </div>
                </div>
                <% } %>
                <% if (!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                   { %>
                <div class="container clearfix box-shadow text-center">
                    <div class="bikeTitle">
                        <h1 class="padding-bottom15 padding-left15"><%= bikeName %></h1>
                    </div>
                    <div class="grid-6 alpha">
                        <div class="padding-left5 padding-right5 ">
                            <div>
                                <span class="margin-bottom10 ">
                                    <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble((modelPage.ModelDetails == null || modelPage.ModelDetails.ReviewRate == null) ? 0 : modelPage.ModelDetails.ReviewRate )) %>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="grid-6 omega border-left1">
                        <div class="padding-left5 padding-right5 ">
                            <span class="font16 text-light-grey">
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> margin-right10 padding-left10 line-Ht22">
                                    <%= modelPage.ModelDetails.ReviewCount %> Reviews</a>
                            </span>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="bike-price-container margin-top10 font22 margin-bottom10 padding-left10 text-center">
                        <div class="bike-price-container font22 margin-bottom15">
                            <span class="fa fa-rupee"></span>
                            <span class="font24 text-grey"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPage.ModelDetails.MinPrice)) %></span> <span class="font10">Last Recorded Price</span>
                        </div>
                    </div>
                    <div class="bike-price-container margin-bottom15 padding-left10 text-center">
                        <div class="bike-price-container margin-bottom15">
                            <span class="font14 text-grey">Hero Xtreme is discontinued in India.</span>
                        </div>
                    </div>
                </div>
                <% } %>
            </div>
        </section>
        <section class="container <%= (modelPage.ModelDesc == null || string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription)) ? "hide" : "" %>">
            <div id="SneakPeak" class="container clearfix box-shadow margin-bottom20 margin-top20">
                <% if (modelPage.ModelDetails.Futuristic && modelPage.UpcomingBike != null)
                   { %>
                <h2 class="padding-bottom15  text-center">Sneak-peek</h2>
                <% } %>
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
        <section class="<%= modelPage.ModelVersionSpecs == null ? "hide" : "" %>">
            <div class="container bg-white text-center clearfix">
                <div class="grid-12">
                    <h2 class="text-center margin-top20 margin-bottom20">Overview</h2>
                    <div class="overview-box">
                        <div class="odd btmAftBorder">
                            <span class="font22"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %> 
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>cc</small></span>
                            <span class="font14">Capacity</span>
                        </div>
                        <div class="even btmAftBorder">
                            <span class="font22"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %> 
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>kmpl</small></span>
                            <span class="font14">Mileage</span>
                        </div>
                        <div class="odd">
                            <span class="font22"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %> 
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>PS</small></span>
                            <span class="font14">Max power</span>
                        </div>
                        <div class="even">
                            <span class="font22"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight) %> 
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>kg</small></span>
                            <span class="font14">Weight</span>
                        </div>
                    </div>
                    <div class="border-top1"></div>
                </div>
            </div>
        </section>
        <section>
            <div class="container bg-white clearfix">
                <div class="grid-12">
                    <h2 class="text-center margin-top30 margin-bottom20">Specifications</h2>


                    <div class="bw-tabs bw-tabs-panel clearfix">
                        <select class="form-control margin-bottom30">
                            <option value="summary">Summary</option>
                            <option value="engineTransmission">Engine &amp; Transmission </option>
                            <option value="brakeWheels">Brakes, Wheels and Suspension</option>
                            <option value="dimensions">Dimensions and Chassis</option>
                            <option value="fuelEffiency">Fuel effieciency and Performance</option>
                        </select>

                        <div class="leftfloat bw-horz-tabs-data font16">
                            <div class="bw-tabs-data" id="summary">
                                <ul>
                                    <li>
                                        <div class="text-light-grey">Displacement</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement,"cc") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Max Power</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", 
                                                                modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Maximum Torque</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm",
                                                                modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %> </div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">No. of gears</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfGears) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Efficiency</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall, "kmpl") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Brake Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.BrakeType) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Front Disc</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Rear Disc</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Alloy Wheels</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.AlloyWheels) %></div>
                                    </li>                                    
                                    <li>
                                        <div class="text-light-grey">Kerb Weight</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight, "kg") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Chassis Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ChassisType) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Top Speed</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TopSpeed, "kmph") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Tubeless Tyres</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TubelessTyres) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Tank Capacity</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelTankCapacity, "litres") %></div>
                                    </li>                                 
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="engineTransmission">
                                <ul>
                                    <li>
                                        <div class="text-light-grey">Displacement</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement, "cc") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Cylinders</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Cylinders) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Max Power</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", 
                                                               modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Maximum Torque</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm",
                                                                   modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %> </div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Bore</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Bore, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Stroke</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Stroke, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Valves Per Cylinder</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ValvesPerCylinder) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Delivery System</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelDeliverySystem) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelType) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Ignition</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Ignition) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Spark Plugs</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.SparkPlugsPerCylinder, "Per Cylinder") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Cooling System</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.CoolingSystem) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Gearbox Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.GearboxType) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">No. of Gears</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfGears) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Transmission Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TransmissionType) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Clutch</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Clutch) %></div>
                                    </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="brakeWheels">
                                <ul>
                                    <li>
                                        <div class="text-light-grey">Brake Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.BrakeType) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Front Disc</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Front Disc/Drum Size</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc_DrumSize, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Rear Disc</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Rear Disc/Drum Size</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc_DrumSize, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Calliper Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.CalliperType) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Wheel Size</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.WheelSize, "inches") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Front Tyre</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontTyre) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Rear Tyre</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearTyre) %></div>
                                    </li>                                    
                                    <li>
                                        <div class="text-light-grey">Tubeless Tyres</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TubelessTyres) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Radial Tyres</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RadialTyres) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Alloy Wheels</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.AlloyWheels) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Front Suspension</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontSuspension) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Rear Suspension</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearSuspension) %></div>
                                    </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="dimensions">
                                <ul>                                                                        
                                    <li>
                                        <div class="text-light-grey">Kerb Weight</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight, "kg") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Overall Length</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallLength, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Overall Width</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallWidth, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Overall Height</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallHeight, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Wheelbase</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Wheelbase, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Ground Clearance</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.GroundClearance, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Seat Height</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.SeatHeight, "mm") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Chassis Type</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ChassisType) %></div>
                                    </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                            <div class="bw-tabs-data hide" id="fuelEffiency">
                                <ul>
                                    <li>
                                        <div class="text-light-grey">Fuel Tank Capacity</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelTankCapacity, "litres") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Reserve Fuel Capacity</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ReserveFuelCapacity, "litres") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Efficiency Overall</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall, "kmpl") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Fuel Efficiency Range</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyRange, "km") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">0 to 60 kmph</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_0_60_kmph, "seconds") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">0 to 80 kmph</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_0_80_kmph, "seconds") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">0 to 40 kmph</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_0_40_m, "seconds") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">Top Speed</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TopSpeed, "kmph") %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">60 to 0 kmph</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_60_0_kmph) %></div>
                                    </li>
                                    <li>
                                        <div class="text-light-grey">80 to 0 kmph</div>
                                        <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_80_0_kmph) %></div>
                                    </li>
                                    <div class="clear"></div>
                                </ul>
                            </div>
                        </div>

                    </div>
                    <!-- bw-tabs ends here -->

                    <!-- features code starts here -->
                    <div class="bw-tabs-data margin-bottom20" id="features">
                        <div class="border-top1"></div>
                        <h2 class="text-center margin-top30 margin-bottom20 text-center">Features</h2>
                        <div class="bw-horz-tabs-data font16">
                            <ul class="bw-tabs-data">
                                <li>
                                    <div class="text-light-grey">Speedometer</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Speedometer) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Fuel Guage</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelGauge) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tachometer Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Digital Fuel Gauge</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.DigitalFuelGauge) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tripmeter</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tripmeter) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Electric Start</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ElectricStart) %></div>
                                </li>
                                <div class="clear"></div>
                            </ul>
                            <ul class="more-features bw-tabs-data hide">
                                <li>
                                    <div class="text-light-grey">Tachometer</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Shift Light</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ShiftLight) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">No. of Tripmeters</div>
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
                    <div class="bw-tabs-data <%= (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0) ? "" : "hide" %>" id="variants">
                        <h2 class="text-center margin-top30 margin-bottom20 text-center">Variants</h2>
                        <asp:Repeater ID="rptVarients" runat="server">
                            <ItemTemplate>
                                <div>
                                    <div class="border-solid content-inner-block-10 margin-bottom20">
                                        <div class="grid-8 alpha">
                                            <h3 class="font16 margin-bottom10"><%# DataBinder.Eval(Container.DataItem, "VersionName") %></h3>
                                            <%--<p class="font14">220 CC, 38 Kmpl, 103 bhp @ 11000 rpm</p>--%>
                                            <p class="font14"><%# Bikewale.Utility.FormatMinSpecs.GetMinVersionSpecs(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")), Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                        </div>
                                        <div class="grid-4 alpha omega">
                                            <p class="font16 margin-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %></p>
                                            <p class="font12 text-light-grey">Ex-showroom, <%= Bikewale.Common.Configuration.GetDefaultCityName %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="clear"></div>
                    </div>

                    <!-- colours code starts here -->
                    <div class="colours-wrap margin-bottom20 <%= modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0 ? "" : "hide" %>">
                        <h2 class="margin-top30 margin-bottom20 text-center">Colours</h2>
                        <div class="jcarousel-wrapper">
                            <div class="jcarousel">
                                <ul class="text-center">
                                    <asp:Repeater ID="rptColors" runat="server">
                                        <ItemTemplate>
                                            <li class="available-colors">
                                                <div class="color-box" style="background-color: #<%# DataBinder.Eval(Container.DataItem, "HexCode")%>;"></div>
                                                <p class="font16 text-medium-grey"><%# DataBinder.Eval(Container.DataItem, "ColorName") %></p>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                            <p class="text-center jcarousel-pagination"></p>

                        </div>
                    </div>

                </div>
            </div>
        </section>
        <% } %>
        <% 
            if (ctrlNews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlExpertReviews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlVideos.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlUserReviews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
        %>
        <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News, reviews and videos code starts here -->
            <div class="container padding-bottom10">
                <div class="grid-12">
                    <h2 class="text-center margin-top30 margin-bottom20">Latest Updates</h2>
                    <div class="bw-tabs-panel">
                        <div class="bw-tabs margin-bottom15 <%= reviewTabsCnt == 1 ? "hide" : "" %>">
                            <div class="form-control-box">
                                <select class="form-control">
                                    <option class=" <%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "" : "hide" %> active" value="ctrlNews">News</option>
                                    <option class="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "" : "hide" %>" value="ctrlExpertReviews">Reviews</option>
                                    <option class="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "" : "hide" %>" value="ctrlVideos">Videos</option>
                                    <option class="<%= (Convert.ToInt32(ctrlUserReviews.FetchedRecordsCount) > 0) ? "" : "hide" %>" value="ctrlUserReviews">User Reviews</option>
                                </select>
                            </div>
                        </div>
                        <BW:News runat="server" ID="ctrlNews" />
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <!-- Most Popular Bikes Starts here-->
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= bikeName %> alternatives</h2>

                    <div class="jcarousel-wrapper discover-bike-carousel alternatives-carousel">
                        <div class="jcarousel">
                            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
                        </div>
                        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <p class="text-center jcarousel-pagination"></p>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>
        <BW:MPopupWidget runat="server" ID="MPopupWidget1" /> 
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->       
    	<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-model.js?<%= staticFileVersion %>"></script>
		<script type="text/javascript">
		    vmModelId = '<%= modelId%>';
		    clientIP = '<%= clientIP%>';
		    cityId = '<%= cityId%>';
		    isUsed = '<%= !modelPage.ModelDetails.New %>';
            var myBikeName = '<%= this.bikeName %>';
            ga_pg_id = '2';
        </script>
        
    </form>
</body>
</html>

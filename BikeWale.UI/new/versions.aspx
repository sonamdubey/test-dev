<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="versions.aspx.cs" Inherits="Bikewale.New.versions" %>

<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/UserReviewsList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/controls/PopupWidget.ascx" TagPrefix="BW" TagName="PriceQuotePopup" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!doctype html>
<html>
<head>
    <%
        title = modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " Price in India - Rs."
                    + Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MinPrice.ToString()) + " - " + Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MaxPrice.ToString())
                    + ". Check out " + modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " on road price, reviews, mileage, variants, news & photos at Bikewale.";

        canonical = "http://www.bikewale.com/" + modelPage.ModelDetails.MakeBase.MaskingName + "-bikes/" + modelPage.ModelDetails.MaskingName + "/";
        AdId = "1017752";
        AdPath = "/1017752/Bikewale_NewBike_";        
        TargetedModel = modelPage.ModelDetails.ModelName;
        fbTitle = title;
        alternate = "http://www.bikewale.com/m/" + modelPage.ModelDetails.MakeBase.MaskingName + "-bikes/" + modelPage.ModelDetails.MaskingName + "/";
        isAd970x90Shown = true;
    %>

    <!-- #include file="/includes/headscript.aspx" -->
    <% isHeaderFix = false; %>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/model.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <style>
 .chosen-results::-webkit-scrollbar { width: 10px;border-radius:5px; }
.chosen-results::-webkit-scrollbar-track { -webkit-box-shadow: inset 0 0 2px rgba(0,0,0,0.2); }
.chosen-results::-webkit-scrollbar-thumb { background-color: rgba(204, 204, 204,0.7); }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->        
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
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-10 padding-top20 padding-bottom20 rounded-corner2">
                        <div class="grid-6 alpha margin-minus10">
                            <div class="<%= modelPage.ModelDetails.Futuristic ? "" : "hide" %>">
                                <span class="model-sprite bw-upcoming-bike-ico"></span>
                            </div>
                            <div class="<%= !modelPage.ModelDetails.Futuristic && !modelPage.ModelDetails.New ? "" : "hide" %>">
                                <span class="model-sprite bw-discontinued-bike-ico"></span>
                            </div>
                            <div class="connected-carousels" id="bikeBannerImageCarousel">
                                <div class="stage">
                                    <div class="carousel carousel-stage">
                                        <ul>
                                            <li>
                                                <div class="carousel-img-container">
                                                <span>
                                                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath,modelPage.ModelDetails.HostUrl,Bikewale.Utility.ImageSize._476x268) %>" title="<%= bikeName %>" alt="<%= bikeName %>" />
                                                </span>
                                                </div>
                                            </li>
                                            <asp:Repeater ID="rptModelPhotos" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="carousel-img-container">
                                                        <span>
                                                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="http://img.aeplcdn.com/bikewaleimg/images/loader.gif"/>
                                                        </span>
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <a href="#" class="prev prev-stage bwsprite"></a>
                                    <a href="#" class="next next-stage bwsprite"></a>
                                </div>

                                <div class="navigation">
                                    <a href="#" class="prev prev-navigation bwsprite"></a>
		                            <a href="#" class="next next-navigation bwsprite"></a>
                                    <div class="carousel carousel-navigation">
                                        <ul>
                                            <li>
                                                <div class="carousel-nav-img-container">
                                                <span>
                                                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath,modelPage.ModelDetails.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" title="<%# bikeName %>" alt="<%= bikeName %>" />
                                                </span>
                                                </div>
                                            </li>
                                            <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="carousel-nav-img-container">
                                                        <span>
                                                            <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="/images/loader.gif"/>
                                                        </span>
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <% if (modelPage.ModelDetails.New)
                               { %>
                            <div class="margin-top20 <%= modelPage.ModelDetails.Futuristic ? "hide" : "" %>">
                                <p class="margin-left50	leftfloat margin-right20">
                                    <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPage.ModelDetails.ReviewRate)) %>
                                </p>
                                <a href="<%= FormatShowReview(modelPage.ModelDetails.MakeBase.MaskingName,modelPage.ModelDetails.MaskingName) %>" class="review-count-box border-solid-left leftfloat margin-right20 padding-left20 <%= (modelPage.ModelDetails.ReviewCount > 0)?"":"hide" %>"><%= modelPage.ModelDetails.ReviewCount %> Reviews
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
                                <span id="bike-price" class="font30 text-black"><%= Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MinPrice.ToString()) %></span>
                                <span class="font12 text-light-grey default-showroom-text">Ex-showroom <%= ConfigurationManager.AppSettings["defaultName"] %></span>    
                                <span style="font-size:14px;display:none" class="price-loader fa fa-spinner fa-spin  text-black"  ></span>

                                <!-- Terms and condition Popup start -->
                                <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                                    <h3>Terms and Conditions</h3>
                                    <div style="vertical-align: middle; text-align: center;" id="termspinner">
                                        <%--<span class="fa fa-spinner fa-spin position-abt text-black bg-white" style="font-size: 50px"></span>--%>
                                        <img src="/images/search-loading.gif" />
                                    </div>
                                    <div class="termsPopUpCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                    <div id="terms" class="breakup-text-container padding-bottom10 font14">
                                    </div>
                                </div>
                                <!-- Terms and condition Popup Ends -->

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
                                                    <td width="150" align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(BWPriceList().exShowroomPrice)"></span></td>
                                                </tr>
                                                <tr>
                                                    <td class="padding-bottom10">RTO</td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(BWPriceList().rto)"></span></td>
                                                </tr>
                                                <tr>
                                                    <td class="padding-bottom10">Insurance (comprehensive)</td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(BWPriceList().insurance)"></span></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="border-solid-top padding-bottom10"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!-- ko if :BWPriceList -->
                                                    <td class="padding-bottom10 text-bold">Total on road price</td>
                                                    <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata((parseInt(BWPriceList().insurance) + parseInt(BWPriceList().rto) + parseInt(BWPriceList().exShowroomPrice)))"></span></td>
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

                                        <!-- ko if : isDealerPQAvailable() -->
                                        <table class="font16">
                                            <tbody>
                                                <!-- ko foreach : DealerPriceList -->
                                                <tr>
                                                    <td width="350" class="padding-bottom10" data-bind="text: categoryName"></td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(price)"></span></td>
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
                                                    <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(DealerOnRoadPrice()) "></span></td>
                                                </tr>

                                                <tr>
                                                    <td class="padding-bottom10">Minus insurance</td>
                                                    <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: $root.FormatPricedata(priceQuote().insuranceAmount) "></span></td>
                                                </tr>
                                                <!-- /ko -->
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="border-solid-top padding-bottom10"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!-- ko if :DealerPriceList() -->
                                                    <td class="padding-bottom10 text-bold">Total on road price</td>
                                                    <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="text: ((priceQuote().insuranceAmount > 0) ? $root.FormatPricedata((DealerOnRoadPrice() - priceQuote().insuranceAmount)) : $root.FormatPricedata(DealerOnRoadPrice())) "></span></td>
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
                            <div class="bike-price-container font28 margin-bottom15 hide">
                                <span class="font30 text-black ">Price not available</span>
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
                            <!-- ko if :!popularCityClicked()-->
                            <div id="city-list-container" class="city-list-container margin-bottom20 ">
                                <div class="text-left margin-bottom15">
                                    <p class="font16 offer-error">Select city for accurate on-road price and exclusive offers</p>
                                </div>
                                <ul id="mainCity">
                                    <li cityid="1" ><span>Mumbai</span></li>
                                    <li cityid="12"><span>Pune</span></li>
                                    <li cityid="2"><span>Bangalore</span></li>
                                    <li cityid="40"><span>Thane</span></li>
                                    <li cityid="13"><span>Navi Mumbai</span></li>
                                    <li class="city-other-btn"><span>Others</span></li>
                                </ul>
                            </div>
                            <!-- /ko -->
                            <!-- City and Area  msgs and select controls starts-->
                            <div id="city-area-select-container" class="city-area-select-container margin-bottom20 " data-bind="visible:popularCityClicked()">
                               
                                <div id="locationError" >
                                <div class="city-select-text text-left margin-bottom15 " data-bind="visible:!selectedCity() || cities()">
                                    <p class="font16">Select city for accurate on-road price and exclusive offers</p>
                                </div>
                                
                                <!-- ko if : selectedCity() && areas()  && areas().length > 0-->
                                <div class="area-select-text text-left margin-bottom15 " >
                                    <p class="font16">Select area for on-road price and exclusive offers</p>
                                </div>
                                <!-- /ko -->
                                </div> 
                                <!-- On Road Price mesasge starts -->
                                <!-- ko if : BWPriceList() || DealerPriceList() -->
                                <div class="city-onRoad-price-container font16 margin-bottom15 hide">
                                    <p class="margin-bottom10">On-road price in <span id="pqArea"></span><span id="pqCity"></span><span class="city-edit-btn font12 margin-left10">change location</span></p>
                                    <p class="font12 margin-bottom15 text-light-grey" id="breakup"></p>
                                    <!-- ko if : priceQuote() && priceQuote().IsDealerPriceAvailable && priceQuote().dealerPriceQuote.offers.length > 0 -->
                                    <input type="button" class="btn btn-orange" id="btnBookNow" data-bind="event: { click: $root.availOfferBtn }" value="Avail Offers" />
                                    <!-- /ko -->
                                </div>
                                <!-- /ko -->
                                <!-- On Road Price mesasge ends  -->
                                <!-- City/Area Select controls starts -->
                                <div class="city-area-wrapper">
                                    <!-- ko if : cities()  && cities().length > 0 -->            
                                    <div class="city-select leftfloat margin-right20 position-rel">
                                          <span class="fa fa-spinner fa-spin position-abt pos-right5 pos-top15 text-black bg-white" style="display:none"></span>
                                        <select id="ddlCity" data-bind="options: cities, optionsText: 'cityName', optionsValue: 'cityId', value: selectedCity, optionsCaption: 'Select City', chosen: { width: '190px' }"></select>                                   
                                    </div>
                                    <!-- /ko -->
                                    <!-- ko if : selectedCity() && areas()  && areas().length > 0 -->
                                    <div class="area-select leftfloat position-rel">
                                        <span class="fa fa-spinner fa-spin position-abt pos-right5 pos-top15 text-black bg-white" style="display:none"></span>
                                        <select id="ddlArea" data-bind="options: areas, optionsText: 'areaName', optionsValue: 'areaId', value: selectedArea, optionsCaption: 'Select Area', chosen: { width: '190px' }"></select>                                        
                                    </div>
                                    <!-- /ko -->
                                    <div class="clear"></div>
                                </div>
                                <!-- City/Area Select controls ends -->
                            </div>
                            <!-- City and Area  msgs and select controls ends  -->
                            <div id="offersBlock" class="city-unveil-offer-container position-rel">
                                <div class="available-offers-container content-inner-block-10">
                                    <h4 class="border-solid-bottom padding-bottom5 margin-bottom5">Available Offers</h4>
                                    <div class="offer-list-container" id="dvAvailableOffer">
                                        <!-- ko if:priceQuote() -->
                                         <!-- ko if : priceQuote().IsDealerPriceAvailable  -->
                                        <ul data-bind="visible: priceQuote().dealerPriceQuote.offers.length > 0, foreach: priceQuote().dealerPriceQuote.offers">
                                           <li>
                                               <span data-bind="text: offerText" >
                                               </span>
                                               <span class="viewterms" data-bind="visible: isOfferTerms == true,  click: $root.termsConditions.bind(offerId)" >View Terms</span>
                                           </li>
                                            
                                        </ul>
                                        <ul data-bind="visible: priceQuote().dealerPriceQuote.offers.length == 0">
                                            <li >No offers available</li>
                                        </ul>
                                        <!-- /ko -->
                                         <!-- ko if : !priceQuote().IsDealerPriceAvailable -->
                                        <ul >
                                             <li data-bind="visible:areas() && areas().length > 0">
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
                                    <input type="button" id="btnShowOffers" class="btn btn-orange unveil-offer-btn" value="Show Offers" />
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
                <% if (modelPage.ModelDetails.Futuristic && modelPage.UpcomingBike != null)
                   { %>
                <h2 class="text-bold text-center margin-top30 margin-bottom30">Sneak-peek</h2>
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
        <section class="container">
            <!--  Discover bikes section code starts here -->
            <div class="grid-12">
                <div class="content-box-shadow content-inner-block-10 discover-bike-tabs-container">
                    <div class="bw-overall-rating">
                        <a class="active" href="#overview">Overview</a>
                        <a href="#specifications">Specifications</a>
                        <a href="#features">Features</a>
                        <a href="#variants" style="<%= (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0) ? "": "display:none;" %>">Variants</a>
                        <a href="#colours" style="<%= (modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0) ? "": "display:none;" %>">Colours</a>
                    </div>
                    <!-- Overview code starts here -->
                    <div class="bw-tabs-data margin-bottom20 active" id="overview">
                        <h2 class="font24 margin-top10 margin-bottom20 text-center">Overview</h2>
                        <div class="grid-3 border-solid-right">
                            <div class="font22 text-center padding-top20 padding-bottom20">
                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %> 
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>cc</small>
                                <p class="font20 text-black">Capacity</p>
                            </div>
                        </div>
                        <div class="grid-3 border-solid-right padding-top20 padding-bottom20">
                            <div class="font22 text-center">
                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>kmpl</small>
                                <p class="font20 text-black">Mileage</p>
                            </div>
                        </div>
                        <div class="grid-3 border-solid-right padding-top20 padding-bottom20">
                            <div class="font22 text-center">
                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %>
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>bhp</small>
                                <p class="font20 text-black">Max power</p>
                            </div>
                        </div>
                        <div class="grid-3">
                            <div class="font22 text-center padding-top20 padding-bottom20">
                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight) %> 
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>kg</small>
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
                                            <div class="text-light-grey">Displacement</div>
                                            <div class="text-bold">
                                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement,"cc") %> 
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Max Power</div>
                                            <div class="text-bold">
                                            <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %>
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Maximum Torque</div>
                                            <div class="text-bold">
                                            <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable( modelPage.ModelVersionSpecs.MaximumTorque, "Nm", modelPage.ModelVersionSpecs.MaximumTorqueRPM,"rpm") %>
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">No. of gears</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfGears) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Efficiency</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall,"kmpl") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Brake Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.BrakeType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Disc</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Disc</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Alloy Wheels</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.AlloyWheels) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Kerb Weight</div>
                                            <div class="text-bold">
                                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight,"kg") %>                                                
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Chassis Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ChassisType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Top Speed</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TopSpeed, "kmph") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Tubeless Tyres</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TubelessTyres) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Tank Capacity</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelTankCapacity, "litres") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="engineTransmission">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Displacement</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement,"cc") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Cylinders</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Cylinders) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Max Power</div>
                                            <div class="text-bold">
                                            <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %>                                             
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Maximum Torque</div>
                                            <div class="text-bold">
                                            <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm", modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %>
                                            </div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Bore</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Bore,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Stroke</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Stroke,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Valves Per Cylinder</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ValvesPerCylinder) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Delivery System</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelDeliverySystem) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Ignition</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Ignition) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Spark Plugs</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.SparkPlugsPerCylinder, "Per Cylinder") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Cooling System</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.CoolingSystem) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Gearbox Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.GearboxType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">No. of Gears</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfGears) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Transmission Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TransmissionType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Clutch</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Clutch) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="brakeWheels">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Brake Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.BrakeType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Disc</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Disc/Drum Size</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc_DrumSize,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Disc</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Disc/Drum Size</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc_DrumSize,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Calliper Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.CalliperType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Wheel Size</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.WheelSize,"inches") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Tyre</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontTyre) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Tyre</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearTyre) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Tubeless Tyres</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TubelessTyres) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Radial Tyres</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RadialTyres) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Alloy Wheels</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.AlloyWheels) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Front Suspension</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontSuspension) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Rear Suspension</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearSuspension) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="dimensions">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Kerb Weight</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight,"kg") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Overall Length</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallLength,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Overall Width</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallWidth,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Overall Height</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallHeight,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Wheelbase</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Wheelbase,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Ground Clearance</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.GroundClearance, "mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Seat Height</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.SeatHeight,"mm") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Chassis Type</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ChassisType) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <div class="clear"></div>
                                    </ul>
                                </div>
                                <div class="bw-tabs-data hide" id="fuelEffiency">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Fuel Tank Capacity</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelTankCapacity,"litres") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Reserve Fuel Capacity</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ReserveFuelCapacity,"litres") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Efficiency Overall</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall,"kmpl") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Fuel Efficiency Range</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyRange,"km") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">0 to 60 kmph</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_0_60_kmph,"seconds") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">0 to 80 kmph</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_0_80_kmph,"seconds") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">0 to 40 kmph</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_0_40_m,"seconds") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Top Speed</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TopSpeed,"kmph") %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">60 to 0 kmph</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_60_0_kmph) %></div>
                                            <div class="clear"></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">80 to 0 kmph</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Performance_80_0_kmph) %></div>
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
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Speedometer) %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Fuel Guage</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelGauge) %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tachometer Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TachometerType) %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Digital Fuel Guage</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.DigitalFuelGauge) %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tripmeter</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tripmeter) %></div>
                                    <div class="clear"></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Electric Start</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ElectricStart) %></div>
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
                                    <div class="text-light-grey">Headlight Bulb Type</div>
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
                    <div class="bw-tabs-data margin-bottom20 <%= modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0 ? "" : "hide" %>" id="variants">
                        <h2 class="font24 margin-bottom20 text-center">Variants</h2>
                        <asp:Repeater runat="server" ID="rptVarients">
                            <ItemTemplate>
                                <div class="grid-6">
                                    <div class="border-solid content-inner-block-10 margin-bottom20">
                                        <div class="grid-8 varient-desc-container alpha">
                                            <h3 class="font16 margin-bottom10"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionName")) %></h3>
                                            <p class="font14"><%# FormatVarientMinSpec(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")),Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                        </div>
                                        <div class="grid-4 omega">
                                            <p class="font18 margin-bottom10"><span class="fa fa-rupee margin-right5"></span><span id="<%# "price_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %></span></p>
                                            <p class="font12 text-light-grey" id="<%# "locprice_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>">Ex-showroom, Mumbai</p>
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
        <%            
            if (ctrlUserReviews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isUserReviewZero = false;
                isUserReviewActive = true;
                            
            }
            if (ctrlExpertReviews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isExpertReviewZero = false;
                if (!isUserReviewActive)
                {
                    isExpertReviewActive = true;                    
                }                
            }
            if (ctrlNews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isNewsZero = false;
                if (!isUserReviewActive && !isExpertReviewActive)
                {
                    isNewsActive = true;
                }                
            }
            if (ctrlVideos.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isVideoZero = false;
                if (!isUserReviewActive && !isExpertReviewActive && !isNewsActive)
                {
                    isVideoActive = true;
                }                
            } 
        %>
        <section class="container <%= (reviewTabsCnt == 0) ? "hide" : "" %>">
            <!--  News Bikes latest updates code starts here -->
            <div class="newBikes-latest-updates-container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest updates on <%= bikeName %></h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                <ul>
                                    <li class= "<%= isUserReviewActive ? "active" : "hide" %>" style="<%= Convert.ToInt32(ctrlUserReviews.FetchedRecordsCount) > 0 ? "" : "display:none;" %>" data-tabs="ctrlUserReviews">User Reviews</li>
                                    <li class= "<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>                                    
                                    <li class= "<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li class= "<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos" >Videos</li>
                                </ul>
                            </div>
                        </div>                        
                        <%if (!isUserReviewZero) { %> <BW:UserReviews runat="server" ID="ctrlUserReviews" />  <% } %>                    
                        <%if (!isExpertReviewZero) { %> <BW:ExpertReviews  runat="server" ID="ctrlExpertReviews" /> <% } %>
                        <%if (!isNewsZero) { %> <BW:News runat="server" ID="ctrlNews" /> <% } %>
                        <%if (!isVideoActive) { %> <BW:Videos runat="server" ID="ctrlVideos" /> <% } %>                      
                        
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="margin-bottom30 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container">
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
        
         <BW:PriceQuotePopup ID="ctrlPriceQuotePopup" runat="server" />
        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/model.js?<%= staticFileVersion %>">"></script>
        <script type="text/javascript">
            var myBikeName = "<%= this.bikeName %>";
        var clientIP = "<%= clientIP%>";
            function applyLazyLoad() {
                $("img.lazy").lazyload({
                    event: "imgLazyLoad",
                    effect: "fadeIn"
                });
            }            
            $(document).ready(function (e) {
                applyLazyLoad();

                $(".carousel-navigation ul li").slice(0, 5).find("img.lazy").trigger("imgLazyLoad");
                $(".carousel-stage ul li").slice(0, 3).find("img.lazy").trigger("imgLazyLoad");

            });
        </script>
        <script type="text/javascript">

            $(document).ready(function (e) {

            if ($(".bw-overall-rating a").last().css("display") == "none") {
                var a = $(this);
                var b = $(this).attr("href");
                console.log(a);
                $(this).remove();
                $(a + ".bw-tabs-data.margin-bottom20.hide").remove();
            }

            $('.bw-overall-rating a[href^="#"]').click(function () {
                var target = $(this.hash);
                if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
                if (target.length == 0) target = $('html');
                $('html, body').animate({ scrollTop: target.offset().top -50- $(".header-fixed").height() }, 1000);
                return false;

            });
            // ends                                

            <% if (modelPage.ModelDetails.New)
            { %>
            var cityId = '<%= cityId%>'
            InitVM(cityId);
            <% } %>

            });
            // Cache selectors outside callback for performance.

            <% if (!modelPage.ModelDetails.Futuristic && modelPage.ModelVersionSpecs != null)
               { %>
            var $window = $(window),
	        $menu = $('.bw-overall-rating'),
	        menuTop = $menu.offset().top + 50;
           
            var sections = $('.discover-bike-tabs-container .bw-tabs-data.margin-bottom20'),
                nav = $('div.bw-overall-rating'),
                nav_height = nav.outerHeight(),
                section_height = $(".discover-bike-tabs-container"),
                sectionContainer_height = section_height.outerHeight() + menuTop - 250,
                sectionStart = section_height.offset().top - 150;
                    
            section_height.bind('heightChangeBlock', function () {
                $(".more-features").css("display","block");
                sectionContainer_height = section_height.outerHeight() + menuTop - 250;
                $(".more-features").css("display", "none");
            });

            section_height.bind('heightChangeNone', function () {
                $(".more-features").css("display", "none");
                sectionContainer_height = section_height.outerHeight() + menuTop - 250;
                $(".more-features").css("display", "block");
            });

            $(".more-features-btn").click(function () {
                if($(".more-features").css("display") == "none")
                    section_height.trigger('heightChangeBlock');
                else if($(".more-features").css("display") == "block") 
                    section_height.trigger('heightChangeNone');
            });
                
            $window.scroll(function () {
                $menu.toggleClass('affix', sectionContainer_height >= $window.scrollTop() && $window.scrollTop() > sectionStart);
                var cur_pos = $(this).scrollTop();

                sections.each(function () {
                    var top = $(this).offset().top -10- nav_height,
                    bottom = top + $(this).outerHeight();

                    if (cur_pos >= top && cur_pos <= bottom) {
                        nav.find('a').removeClass('active');
                        sections.removeClass('active');

                        $(this).addClass('active');
                        nav.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
                    }
                });
            });

            <% } %> 
            ga_pg_id = '2';
            
            function InitVM(cityId) {
                var viewModel = new pqViewModel('<%= modelId%>', cityId);
                modelViewModel = viewModel;
                ko.applyBindings(viewModel, $('#dvBikePrice')[0]);
                viewModel.LoadCity();
            }       

            if('<%=isUserReviewActive%>'== 'False') $("#ctrlUserReviews").addClass("hide");
            if('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");
        </script>      
    </form>
</body>
</html>

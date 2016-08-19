﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeModels" EnableViewState="false" Trace="false" %>
<%@ Register Src="/m/controls/NewNewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/NewAlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="/m/controls/NewUserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/m/controls/MPriceInTopCities.ascx" TagPrefix="BW" TagName="TopCityPrice" %>
<!DOCTYPE html>
<html>
<head>
    <%
        description = String.Format("{0} Price in India - Rs. {1}. Check out {0} on road price, reviews, mileage, versions, news & photos at Bikewale.com", bikeName, Bikewale.Utility.Format.FormatPrice(price.ToString()));
        title = String.Format("{0} Price, Mileage & Reviews - BikeWale", bikeName);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/{1}/", modelPage.ModelDetails.MakeBase.MaskingName, modelPage.ModelDetails.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        Ad_300x250 = true;
        TargetedModel = bikeModelName;
        TargetedCity = cityName;
        keywords = string.Format("{0}, {0} Price, {0} Reviews, {0} Photos, {0} Mileage", bikeName);
        EnableOG = true;
        OGImage = modelImage;
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <script type="text/javascript">
        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var versionId = '<%= versionId%>';
        var cityId = '<%= cityId%>';
        var clientIP = "<%= clientIP%>";
        var pageUrl = "www.bikewale.com/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;
        var areaId = '<%= areaId%>';
        var bikeVersionLocation = '';
        var bikeVersion = '';
        var isBikeWalePq = "<%= isBikeWalePQ%>";
        var isDealerPriceAvailable = "<%= pqOnRoad != null ? pqOnRoad.IsDealerPriceAvailable : false%>";
        var campaignId = "<%= campaignId%>";
        var manufacturerId = "<%= manufacturerId%>";
        var isDealerPQ = "<%= isDealerPQ %>";
        var versionCount = "<%=versionCount  %>";
    </script>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-model.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div itemscope="" itemtype="http://schema.org/Product" class="container bg-white clearfix">
                <span itemprop="name" class="hide"><%= bikeName %></span>
                <div class="<%= !modelPage.ModelDetails.New ? "padding-top20 position-rel" : ""%>">
                    <% if (modelPage.ModelDetails.New)
                       { %><h1 class="padding-top10 padding-left20 padding-right20"><%= bikeName %></h1>
                    <% } %>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Upcoming</div>
                    <div class="bikeTitle">
                        <h1 class="padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>
                    <% if(!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Discontinued</div>
                    <div class="bikeTitle">
                        <h1 class="padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>

                    <% if (modelPage.ModelDetails.New || !modelPage.ModelDetails.New)
                       { %>
                    <div class="padding-left20 padding-right10 margin-top5 margin-bottom10">
                        <p class=" <%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> leftfloat margin-right10 rating-wrap">
                            <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble((modelPage.ModelDetails == null || modelPage.ModelDetails.ReviewRate == null) ? 0 : modelPage.ModelDetails.ReviewRate )) %>
                        </p>
                        <p class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "hide" : ""  %> leftfloat margin-right10 rating-wrap">
                            Not rated yet
                        </p>
                        <span itemprop="aggregateRating" itemscope="" itemtype="http://schema.org/AggregateRating">
                            <meta itemprop="ratingValue" content="<%=modelPage.ModelDetails.ReviewRate %>">
                            <meta itemprop="worstRating" content="1">
                            <meta itemprop="bestRating" content="5">
                            <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> border-solid-left leftfloat margin-right10 padding-left10 line-Ht22">
                                <span itemprop="reviewCount"><%= modelPage.ModelDetails.ReviewCount %>
                                </span>Reviews
                            </a>
                        </span>
                        <div class="clear"></div>
                    </div>
                    <% } %>
                    <div id="model-image-wrapper">
                        <div class="model-main-image">
                            <img src="<%=modelImage %>" alt="<%= bikeName %> images" title="<%= bikeName %> model image" />
                            <% if (modelPage !=null && modelPage.Photos != null && modelPage.Photos.Count > 1)
                               { %>
                            <div class="model-media-details">
                                <a href="./photos" class="model-media-item">
                                    <span class="bwmsprite gallery-photo-icon"></span>
                                    <span class="model-media-count"><%= modelPage.Photos.Count %></span>
                                </a>
                                <%--<a href="./photos#videos" class="model-media-item">
                                    <span class="bwmsprite gallery-video-icon"></span>
                                    <span class="model-media-count">7</span>
                                </a>--%>
                            </div>
                            <% } %>
                        </div>
                    </div>

                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="bikeDescWrapper">
                          <% if (modelPage.UpcomingBike != null) {%>
                         <div class="margin-bottom10 font14 text-light-grey">Expected price</div>
                         <div>
                             <span class="font24 text-bold"><span class="bwmsprite inr-lg-icon"></span>
                                 <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                         </div>
                         <div class="border-solid-bottom margin-top15 margin-bottom15"></div>
                         <div class="margin-bottom10 font14 text-light-grey">Expected launch date</div>
                         <div class="font18 text-grey margin-bottom5">
                             <span class="text-bold"><%= modelPage.UpcomingBike.ExpectedLaunchDate %></span>
                         </div>
                        <% } %>
                        <p class="font14 text-grey"><%= bikeName %> is not launched in India yet. Information on this page is tentative.</p>
                    </div>
                    <% } %>
                </div>
                
                <!-- previous top model card -->
            <% if (!isDiscontinued)
               {
                   if (toShowOnRoadPriceButton)
                   {   %>
            <div class="grid-12 float-button float-fixed clearfix padding-bottom10">

                <a id="btnGetOnRoadPrice" href="javascript:void(0)" ismodel="true" modelid="<%=modelId %>" style="width: 100%" class="btn btn-orange margin-top10 fillPopupData">Check on-road price</a>
                <% }
                   else
                   {   %>
                <div class="grid-12 float-button float-fixed clearfix">
                    
                    <% if (modelPage.ModelDetails.New && viewModel != null && !isBikeWalePQ )
                        {   
                        %>
                        <% if ( viewModel.IsPremiumDealer)
                            { 
                        %>  <div class="grid-<%=viewModel.MaskingNumber == string.Empty? "12":"6" %> alpha omega padding-top10 padding-right5 padding-bottom10">
                                <a id="getAssistance" leadSourceId="19" class="btn btn-white btn-full-width btn-sm rightfloat" href="javascript:void(0);">Get offers</a>
                            </div>
                            <%  if(viewModel.MaskingNumber != string.Empty)
                                { %>
                                <div class="grid-6 alpha omega padding-top10 padding-bottom10 padding-left5">
                                    <a id="calldealer" class="btn btn-orange btn-full-width btn-sm rightfloat" href="tel:+91<%= viewModel.MaskingNumber == string.Empty? viewModel.MobileNo: viewModel.MaskingNumber %>"><span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
                                </div>
                            <% } %>
                        <% }                             
                        }  
                    %>
                </div>
                <%
                }
               }
               %>
            </div>
        </section>      

        <section>
            <asp:HiddenField ID="hdnVariant" Value="0" runat="server" />
            <div class="container bg-white clearfix elevated-shadow">
                <% if (!isDiscontinued)
                   { %>
                <div class="grid-12 padding-top5 padding-bottom5 border-solid-bottom">
                    <div class="grid-6 alpha border-solid-right">
                        <p class="font12 text-light-grey padding-left10">Version:</p>
                        <% if (versionCount > 1)
                           { %>
                        <div class="dropdown-select-wrapper">
                            <asp:DropDownList CssClass="dropdown-select" ID="ddlNewVersionList" runat="server" />
                        </div>
                        <% }
                           else
                           {%>
                        <p id="singleversion" class="single-version-label font14 margin-left5"><%=variantText %></p>
                        <% } %>
                    </div>

                    <div class="grid-6 padding-left20">
                        <p class="font12 text-light-grey">Location:</p>
                        <p class="font14 text-bold">
                            <span class="selected-location-label inline-block text-truncate"><span><%= location %></span></span>

                            <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity" rel="nofollow">
                                <span class="bwmsprite loc-change-blue-icon"></span></a>
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <% }
                   else
                   { %>
                <div class="bike-price-container padding-10-20">
                    <div class="bike-price-container ">
                        <span class="font14 text-grey"><%= bikeName %> is now discontinued in India.</span>
                    </div>
                </div>
                <% } %>
                <div class="padding-10-20">
                    <p class="font12 text-light-grey"><%=priceText %> price in <%= location %></p>
                    <p class="">
                        <span class="bwmsprite inr-md-icon"></span>
                        <span class="font22 text-bold"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %>&nbsp;</span>
                        <%if (isOnRoadPrice && price > 0)
                          {%>
                        <%--<span id="viewBreakupText" class="font16 text-bold viewBreakupText">View detailed price</span>--%>
                        <a href="/m/pricequote/dealerpricequote.aspx?MPQ=<%= detailedPriceLink %>" class="font16 text-bold viewBreakupText" rel="nofollow" >View detailed price</a>
                        <% } %>
                    </p>
                </div>
                <%
                    if (viewModel != null && !isBikeWalePQ)
                    { 
                %>
                <div id="model-dealer-card">
                    <div class="dealer-details margin-bottom10">
                        <div class="inline-block margin-right10">
                            <span class="offers-sprite dealership-icon"></span>
                        </div>
                        <div class="inline-block">
                            <h2 id="dealername" class="text-default"><%=viewModel.Organization %></h2>
                            <p class="font14 text-light-grey"><%=viewModel.AreaName %></p>
                        </div>
                    </div>
                    <% if (viewModel.Offers != null && viewModel.OfferCount > 0)
                       { %>
                    <div class="dealer-offers-content margin-bottom10">
                        <p class="offers-content-label">Offers available:</p>
                        <ul id="dealer-offers-list">
                            <asp:Repeater ID="rptNewOffers" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <p class="margin-bottom5">
                                            <span class="offers-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>_sm"></span>
                                        </p>
                                        <p>
                                            <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerType")) %>
                                        </p>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (moreOffersCount > 3)
                                       {%>
                                    <li class="more-offers-card">
                                        <p class="font18 text-bold text-light-grey margin-top5 margin-bottom5">+<%= moreOffersCount %> </p>
                                        <p>more offers</p>
                                    </li>
                                    <% } %>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <%} %>
                    <% if (viewModel.IsPremiumDealer)
                       {
                           if (isBookingAvailable)
                           { %>
                    <div class="margin-bottom20">
                        <div class="vertical-top">
                            <a rel="nofollow" href="/m/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-teal btn-sm-0">Book now</a>
                        </div>
                        <p class="booknow-label font11 line-height-1-5 text-xx-light vertical-top">
                            Pay <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bookingAmt.ToString()) %> to book online and <br />balance of <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice((price - bookingAmt).ToString()) %> at dealership
                        </p>
                    </div>
                    <% }
                                     else
                                     { %>
                    <div class="margin-bottom20">
                        <div class="vertical-top">
                            <a id="requestcallback" c="Model_Page" a="Request_Callback_Details_Clicked" v="bikeVersionLocation" leadSourceId="30" href="javascript:void(0)" class="btn btn-white callback-btn btn-sm-0 bw-ga">Request callback</a>
                        </div>
                        <p class="callback-label font11 line-height-1-5 text-xx-light vertical-top">Get EMI options, test rides other services from dealer</p>
                    </div>
                    <% 
                    }
                       }
                       else
                       { %>
                    <div class="margin-bottom20">
                        <div class="inline-block nearby-partner-left-col">
                            <div class="inline-block margin-right10">
                                <span class="offers-sprite dealership-icon"></span>
                            </div>
                            <div class="inline-block nearby-partner-label">
                                <p class="font14">One partner dealer near you</p>
                            </div>
                        </div>
                        <a id="viewprimarydealer" c="Model_Page" a="View_Dealer_Details_Clicked" v="bikeVersionLocation"  href="javascript:void(0)" class="btn btn-orange btn-sm-0 bw-ga">View dealer details</a>
                    </div>
                    <% } %>
                </div>
                <% if(viewModel.SecondaryDealerCount > 0){ %>
                    <div class="content-inner-block-20 border-solid-top font16">
                        <a href="javascript:void(0)" rel="nofollow" id="more-dealers-target">Prices from <%=viewModel.SecondaryDealerCount %> more partner dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                <% } %>
            <% } %>
            </div>
        </section>

        <section>
            <div id="modelSpecsTabsContentWrapper" class="container bg-white clearfix box-shadow margin-top30 margin-bottom30">
                <div id="modelOverallSpecsTopContent">
                    <div id="overallSpecsTab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <% if ((modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription)) || modelPage.ModelVersionSpecs != null)
                           { %>
                            <li data-tabs="#modelSummaryContent">Summary</li>
                            <% } %>
                            <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                            { %>
                            <li data-tabs="#modelPricesContent">Prices</li>
                            <%} %>
                            <% if(modelPage.ModelVersionSpecs!= null){ %>
                            <li data-tabs="#modelSpecsFeaturesContent">Specs & Features</li>
                            <% } %>
                            <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                             { %>
                            <li data-tabs="#modelReviewsContent">Reviews</li>
                              <%} %>
                            <% if (ctrlVideos.FetchedRecordsCount > 0)
                                { %>
                                <li data-tabs="#modelVideosContent">Videos</li>
                            <%} %>
                             <% if (ctrlNews.FetchedRecordsCount > 0)
                             { %>
                                <li data-tabs="#makeNewsContent">News</li>
                            <%} %>
                             <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                              { %>
                                 <li data-tabs="#modelAlternateBikeContent">Alternatives</li>
                            <%} %>
                        </ul>
                    </div>
                </div>

                <%if (modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription) || (modelPage.ModelVersionSpecs != null))
                  { %>
                <div id="modelSummaryContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom15 border-solid-bottom">
                    <%if (modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription))
                      { %>
                    <h2><%=bikeName %> Summary</h2>
                    <h3>Preview</h3>
                    <p class="font14 text-light-grey line-height17">
                        <span class="model-preview-main-content">
                            <%= modelPage.ModelDesc.SmallDescription %>   
                        </span>
                        <span class="model-preview-more-content">
                            <%= modelPage.ModelDesc.FullDescription %>
                        </span>

                        <%if (!string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription))
                          { %>
                        <a href="javascript:void(0)" class="read-more-model-preview" rel="nofollow">Read more</a>
                        <% } %>
                    </p>
                    <% } %>
                    <% if (modelPage.ModelVersionSpecs != null)
                       { %>
                    <h3 class="margin-top15">Specification summary</h3>
                    <div class="text-center">
                        <div class="summary-overview-box">
                            <div class="odd btmAftBorder">
                                <span class="inline-block model-sprite specs-capacity-icon margin-right10" title="<%=bikeName %> Engine Capacity"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "hide":"" %>'>cc</span>
                                    </p>
                                    <p class="font16 text-light-grey">Capacity</p>
                                </div>
                            </div>
                            <div class="even btmAftBorder">
                                <span class="inline-block model-sprite specs-mileage-icon margin-right10" title="<%=bikeName %> Mileage"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall).Equals("--") ? "hide":"" %>'>kmpl</span>
                                    </p>
                                    <p class="font16 text-light-grey">Mileage</p>
                                </div>
                            </div>
                            <div class="odd">
                                <span class="inline-block model-sprite specs-maxpower-icon margin-right10" title="<%=bikeName %> Max Power"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "hide":"" %>'>bhp</span>
                                    </p>
                                    <p class="font16 text-light-grey">Max power</p>
                                </div>
                            </div>
                            <div class="even">
                                <span class="inline-block model-sprite specs-weight-icon margin-right10" title="<%=bikeName %> Kerb Weight"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight).Equals("--") ? "hide":"" %>'>kg</span>
                                    </p>
                                    <p class="font16 text-light-grey">Weight</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
                <% } %>

                <div id="modelPricesContent" class="bw-model-tabs-data">
                    <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                       { %>
                    <h2 class="padding-top15 padding-right20 padding-left20"><%= bikeName %> Prices</h2>
                    <!-- varient code starts here -->
                    <h3 class="padding-right20 padding-left20">Prices by versions</h3>

                    <div class="swiper-container">
                        <div class="swiper-wrapper font14">
                            <asp:Repeater ID="rptVarients" runat="server" OnItemDataBound="rptVarients_ItemDataBound2">
                                <ItemTemplate>                         
                                        <div class="swiper-slide model-prices-version-content rounded-corner2">
                                            <p class="text-bold text-truncate margin-bottom13"><%# DataBinder.Eval(Container.DataItem, "VersionName") %></p>
                                            <p class="text-truncate text-xt-light-grey margin-bottom13"><%# Bikewale.Utility.FormatMinSpecs.GetMinVersionSpecs(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")), Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                            <p class="text-truncate text-light-grey margin-bottom10" id="<%# "locprice_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>">
                                                 <asp:Label ID="lblExOn" Text="Ex-showroom price" runat="server"></asp:Label>,
                                                    <% if (cityId != 0 && cityName != string.Empty)
                                                    { %>
                                                    <%= cityName %>
                                                    <% }
                                                        else
                                                        { %>
                                                    <%= Bikewale.Common.Configuration.GetDefaultCityName %>
                                                    <% } %>
                                            </p>
                                            <p class="font18 text-bold text-black">
                                                <span class="bwmsprite inr-dark-md-icon"></span>
                                                <span id="<%# "priced_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>"> <asp:Label Text='<%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %>' ID="txtComment" runat="server"></asp:Label></span>
                                            </p>
                                        </div>
                                        <asp:HiddenField ID="hdnVariant" runat="server" Value='<%#Eval("VersionId") %>' />                           
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <!-- varient code ends here -->
                    <% } %>
                   <BW:TopCityPrice ID="ctrlTopCityPrices" runat="server" />
                   <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                </div>

                <% if(modelPage.ModelVersionSpecs != null){ %>
                <div id="modelSpecsFeaturesContent" class="bw-model-tabs-data font14">
                    <div class="padding-15-20">
                        <h2><%=bikeName %> Specifications & Features</h2>
                        <h3>Specifications</h3>

                        <ul id="modelSpecsList">
                            <li>
                                <div class="text-light-grey padding-right10">Displacement</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement,"cc") %></div>
                            </li>       
                            <li>
                                <div class="text-light-grey padding-right10">Max Power</div>
                                <div class="text-bold"> <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", 
                                                                    modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Maximum Torque</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm",
                                                                    modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">No. of gears</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfGears) %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Fuel Tank Capacity</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelTankCapacity, "litres") %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Top Speed</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TopSpeed, "kmph") %></div>
                            </li>
                        </ul>
                        <div class="margin-top25">
                            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(modelPage.ModelDetails.MakeBase.MaskingName, modelPage.ModelDetails.MaskingName, "modelSpecifications",versionId) %>"  class="bw-ga" c="Model_Page" a="View_full_specifications_link_cliked" v="myBikeName" title="<%=bikeName %> Specifications">View full specifications<span class="bwmsprite blue-right-arrow-icon"></span></a>
                            
                        </div>

                        <h3 class="margin-top25">Features</h3>

                        <ul id="modelFeaturesList">
                            <li>
                                <div class="text-light-grey padding-right10">Speedometer</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Speedometer) %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Fuel Guage</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelGauge) %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Tachometer Type</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Digital Fuel Gauge</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.DigitalFuelGauge) %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Tripmeter</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tripmeter) %></div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Electric Start</div>
                                <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ElectricStart) %></div>
                            </li>
                        </ul>
                        <div class="margin-top25">
                            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(modelPage.ModelDetails.MakeBase.MaskingName, modelPage.ModelDetails.MaskingName, "modelFeatures",versionId) %>"  class="bw-ga" c="Model_Page" a="View_all_features_link_cliked" v="myBikeName" title="<%=bikeName %> Features">View all features<span class="bwmsprite blue-right-arrow-icon"></span></a>
                        </div>
                          <%if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                          { %>   
                        <!-- colours code starts here -->    
                        <h3 class="margin-top25">Colours</h3>

                        <ul id="modelColorsList" class="margin-top5">
                        <asp:Repeater ID="rptColors" runat="server">
                                <ItemTemplate>                        
                                    <li>
                                        <div class="color-box <%# (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count == 1 )?"color-count-one": (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count >= 3 )?"color-count-three":"color-count-two" %> inline-block">
                                           <asp:Repeater runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "HexCodes") %>'>
                                                <ItemTemplate>
                                                        <span <%# String.Format("style='background-color: #{0}'",Convert.ToString(Container.DataItem)) %>></span>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <p class="font16 inline-block"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ColorName")) %></p>
                                    </li>
                              </ItemTemplate>
                        </asp:Repeater>
                        </ul>
                        <%} %>
                         <!-- colours code ends here -->   
                    </div>
                 <%if (Ad_300x250)
                   { %>
                <section>
                    <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
                </section>
                <% } %>
                </div>
                <% } %>

                <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                    { %>
                <div id="modelReviewsContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top10 padding-bottom20 border-solid-bottom font14">
                <h2><%=bikeName %> Reviews</h2>                      
                       
                    <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                        { %>
                    <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                    <% } %>
                    <%if (ctrlUserReviews.FetchedRecordsCount > 0)
                            { %>

                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />

                    <% } %>
                </div>
                <% } %>
                <%if (ctrlVideos.FetchedRecordsCount > 0)
                    { %>
                    <div id="modelVideosContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 border-solid-bottom font14">
                        <h2><%= bikeName %> Videos</h2>
                            <BW:Videos runat="server" ID="ctrlVideos" />
                    </div>
                <% } %>

                <%if (ctrlNews.FetchedRecordsCount > 0)
                 { %>
                 <BW:News runat="server" ID="ctrlNews" />
                <% } %>      

                <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                   { %>
                    <BW:AlternateBikes ID="ctrlAlternativeBikes" runat="server" />           
                <%} %>
                <div id="modelSpecsFooter"></div>
            </div>
        </section>
      
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

     
        <!-- Terms and condition Popup Ends -->

        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <!-- Contact details Popup starts here -->
                    <p class="font18 margin-bottom5">Provide contact details</p>
                    <p class="text-light-grey margin-bottom5">Dealership will get back to you with offers</p>

                    <div class="personal-info-form-container margin-top10">
                        <div class="form-control-box">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="value: fullName" />
                            <span class="bwmsprite error-icon "></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="value: emailId" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="value: mobileNo" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                    </div>
                    <input type="button" class="btn btn-full-width btn-orange hide" value="Submit" onclick="validateDetails();" class="rounded-corner5" data-role="none" id="btnSubmit" />
                </div>
                <!-- Contact details Popup ends here -->
                 <!-- thank you message starts here -->
                <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                        <p class="font18 text-bold margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                        <%if(viewModel != null){ %>
                            <p class="font16 margin-bottom40"><%=viewModel.Organization %>, <%=viewModel.AreaName %> will get in touch with you soon</p>
                        <%} %>
                        <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
                </div>
                <!-- thank you message ends here -->
                <div id="otpPopup">
                    <p class="font18 margin-bottom5">Verify your mobile number</p>
                    <p class="text-light-grey margin-bottom5">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                            <span class="bwmsprite tel-grey-icon"></span>
                            <span class="text-light-grey font24">+91</span>
                            <span class="lead-mobile font24">9876543210</span>
                            <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                        </div>
                        <div class="otp-box lead-otp-box-container">
                            <div class="form-control-box margin-bottom10">
                                <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" maxlength="5" data-bind="value: otpCode" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                            <p class="margin-left10 margin-top10 otp-notify-text text-light-grey font12" data-bind="visible: (NoOfAttempts() >= 2)">
                                OTP has been already sent to your mobile
                            </p>
                            <a class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                        </div>
                        <div class="update-mobile-box">
                            <div class="form-control-box text-left">
                                <p class="mobile-prefix">+91</p>
                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                        </div>
                    </div>

                </div>
                <!-- OTP Popup ends here -->
            </div>
        </div>
        <!-- Lead Capture pop up end  -->
        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                <div class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="http://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class="hide">
            </div>
        </div>
        <!-- Terms and condition Popup end -->

        <div id="dealer-offers-popup" class="bwm-fullscreen-popup">
            <div class="offers-popup-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50percent margin-bottom10">
                <div class="icon-inner-container rounded-corner50percent text-center">
                    <span class="offers-sprite offers-box-icon margin-top12"></span>
                </div>
            </div>
            <p class="font16 text-bold text-center margin-bottom20">Offers from this dealer</p>
            <ul class="dealer-offers-list margin-bottom25">
                <li>
                    <asp:Repeater ID="rptOffers" runat="server">
                        <ItemTemplate>
                            <li>
                                <span class="dealer-offer-image offers-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>_sm"></span>
                                <span class="dealer-offer-label"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %></span>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
            </ul>
            <div class="text-center">
                <a id="getofferspopup" leadSourceId="31"  href="javascript:void(0);" class="btn btn-orange text-bold">Get offers from dealer</a>
            </div>
        </div>
        <% if(viewModel != null){ %>
        <div id="more-dealers-popup" class="bwm-fullscreen-popup">
            <div class="dealers-popup-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50percent margin-bottom10">
				<div class="icon-inner-container rounded-corner50percent text-center">
					<span class="offers-sprite offers-box-icon margin-top12"></span>
				</div>
			</div>
            <p class="font16 text-bold text-center margin-bottom15"><%=viewModel.SecondaryDealerCount %> partner dealers near you</p>
            <ul>
                <asp:Repeater ID="rptSecondaryDealers" runat="server">
                    <ItemTemplate>
                        <li class="secondary-dealer-card">
                            <a href="javascript:void(0)" onclick="secondarydealer_Click(<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "DealerId")) %>)">
						        <div>
							        <span class="grid-9 alpha omega font14 text-default text-bold"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Name")) %></span>
							        <span class="grid-3 omega text-light-grey text-right">5.4 kms</span>
							        <div class="clear"></div>
							        <span class="font12 text-light-grey"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Area")) %></span>
							        <div class="margin-top15">
								        <div class="grid-4 alpha omega border-solid-right">
									        <p class="font12 text-light-grey margin-bottom5">On-road price</p>
									        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold">1,02,887</span>
								        </div>
								        <div class="grid-8 padding-top10 padding-left20 omega">
									        <span class="bwmsprite offers-sm-box-icon"></span>
									        <span class="font14 text-default text-bold">2</span>
									        <span class="font12 text-light-grey">Offers available</span>
								        </div>
								        <div class="clear"></div>
							        </div>
						        </div>
					        </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <% } %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-model.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var leadSourceId;
            vmModelId = '<%= modelId%>';
            clientIP = '<%= clientIP%>';
            cityId = '<%= cityId%>';
            isUsed = '<%= !modelPage.ModelDetails.New %>';
            var pageUrl = "<%= canonical %>";
            var myBikeName = "<%= this.bikeName %>";
            var versionName = "<%= variantText %>"
            ga_pg_id = '2';
            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
            var getCityArea = GetGlobalCityArea();
            $(document).ready(function (e) {

                $(".leadcapturesubmit").on('click', function () {
                    openLeadPopup($(this));
                });
                $("#templist input").on("click", function () {
                    if ($(this).attr('data-option-value') == $('#hdnVariant').val()) {
                        return false;
                    }
                    $('#hdnVariant').val($(this).attr('data-option-value'));
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Version_Change', 'lab': bikeVersionLocation });
                });

                if ($('#getMoreDetailsBtn').length > 0) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Shown', 'lab': myBikeName + '_' + getBikeVersion() + '_' + getCityArea });
                }
                if ($('#btnGetOnRoadPrice').length > 0) {
                    dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': 'Model_Page', 'act': 'Get_On_Road_Price_Button_Shown', 'lab': myBikeName + '_' + getBikeVersion() + '_' + getCityArea });
                }
                if ($("#getAssistance").length > 0) {
                    dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_Offers_Shown", "lab": myBikeName + "_" + getBikeVersion() + '_' + getCityArea });
                }

            });
            function secondarydealer_Click(dealerID) {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerID + "&IsDealerAvailable=true";
                window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            }
            $("#viewprimarydealer, #dealername").on("click", function () {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId + "&IsDealerAvailable=true";
                window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            });
            
        </script>
    </form>
</body>
</html>
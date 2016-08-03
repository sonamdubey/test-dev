<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeModels" EnableViewState="false" Trace="false" %>
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
                        <%-- <div class="bikeTitle">
                            <h1 class="padding-bottom10"><%= bikeName %></h1>
                        </div>--%>
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
                <% if (modelPage.ModelDetails.New)
                   { %>
                <div class="grid-12 bg-white box-shadow padding-bottom10" id="dvBikePrice">

                    <div class="clearfix padding-right10 padding-left10 margin-bottom10">
                        <div class="font14 text-light-grey alpha omega grid-3 version-label-text margin-top5">Version:</div>
                        <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 1)
                           { %>
                        <div class="form-control-box variantDropDown leftfloat grid-9 alpha omega">
                            <div class="sort-div rounded-corner2">
                                <div class="sort-by-title" id="sort-by-container">
                                    <span class="leftfloat sort-select-btn">
                                        <asp:Label runat="server" ID="defaultVariant"></asp:Label>
                                    </span>
                                    <span class="clear"></span>
                                </div>
                                <span id="upDownArrow" class="rightfloat bwmsprite fa-angle-down position-abt pos-top13 pos-right10"></span>
                            </div>
                            <div class="sort-selection-div sort-list-items hide">
                                <ul id="sortbike">
                                    <asp:Repeater ID="rptVariants" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <asp:Button Style="width: 100%; text-align: left" ID="btnVariant" ToolTip='<%#Eval("VersionName") %>' OnCommand="btnVariant_Command" versionid='<%#Eval("VersionId") %>' CommandName='<%#Eval("VersionId") %>' CommandArgument='<%#Eval("VersionName") %>' runat="server" Text='<%#Eval("VersionName") %>'></asp:Button>
                                            </li>
                                            <asp:HiddenField ID="hdn" Value='<%#Eval("VersionId") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <asp:HiddenField ID="hdnVariant" Value="0" runat="server" />
                            </div>
                        </div>
                        <% }
                           else
                           {  %>
                        <p id='versText' class="variantText text-grey grid-10 alpha font14 margin-top5"><%= variantText %></p>
                        <% } %>
                    </div>
                    <div class="padding-right10 padding-left10">

                        <% if (isDiscontinued)
                           { %>
                        <p class="margin-top15 margin-left10 font14 text-light-grey clear fillPopupData">Last known Ex-showroom price</p>
                        <% } %>
                        <% else
                               if (!isCitySelected)
                               {%>
                        <p class="font14 fillPopupData text-light-grey margin-top10 margin-bottom7">
                            Ex-showroom price in <span href="javascript:void(0)" class="text-light-grey clear">
                                <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span>
                            <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity" rel="nofollow">
                                <span class="bwmsprite loc-change-blue-icon"></span>
                            </a>
                            <% } %>
                            <% else
                                   if (!isOnRoadPrice)
                                   {%>
                            <p class="margin-top10 margin-bottom7 font14 text-light-grey clear">
                                Ex-showroom price in <span><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity" rel="nofollow"><span class="bwmsprite loc-change-blue-icon"></span></a>
                            </p>
                            <% } %>
                            <% else
                                   {%>
                            <p class="margin-top10 margin-bottom10 font14 text-light-grey clear">
                                On-road price in <span><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity" rel="nofollow"><span class="bwmsprite loc-change-blue-icon"></span></a>
                            </p>
                            <% } %>
                            <div itemprop="offers" itemscope itemtype="http://schema.org/Offer">
                                <p class="line-Ht18 padding-bottom5">

                                    <%if (price > 0)
                                      { %>

                                    <span class="font22 text-bold">
                                        <span itemprop="priceCurrency" content="INR"><span class="bwmsprite inr-md-icon"></span></span>
                                        <span itemprop="price" content="<%=price %>">
                                            <%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %>
                                        </span>
                                    </span>

                                    <% }
                                      else
                                      { %>
                                    <span class="font18 text-bold">Price unavailable</span>
                                    <%  } %>
                                </p>
                            </div>
                            <%if (isOnRoadPrice && price > 0)
                              {%>
                            <span id="viewBreakupText" class="font16 text-bold viewBreakupText">View detailed price</span>
                            <p class="font12 text-light-grey clear <%= dealerId > 0 ? "margin-bottom20" : "" %>" />
                            <% } %>
                            <% if (!toShowOnRoadPriceButton && isBikeWalePQ && dealerId == 0)
                               { %>
                            <%--<p class="margin-top10 margin-bottom20 clear">
                                <a class="text-bold" style="position: relative; font-size: 14px; margin-top: 1px;" target="_blank" href="/m/insurance/" id="insuranceLink">Save up to 60% on insurance - PolicyBoss
                                </a>
                            </p>--%>
                            <% } %>
                    </div>                    
                    <%
                       if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) { 
                     %>
                    <div class="margin-top15 content-inner-block-10 border-solid">
                        <h2><%=viewModel.Organization %></h2>
                        <p class="font14 text-light-grey padding-bottom10 "><%=viewModel.AreaName %></p>
                        <%
                           if (viewModel.Offers != null && viewModel.OfferCount > 0)
                           { 
                         %>
                        <p class="font16 text-bold padding-top15 margin-bottom15 border-light-top">Exclusive offers on this bike:</p>
                        <ul class="dealers-benefits-list text-light-grey margin-bottom10">     
                            <asp:Repeater ID="rptOffers" runat="server">
                              <ItemTemplate>
                                <li>
                                    <span class="dealers-benefits-image offer-benefit-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>"></span>
                                    <span class="dealers-benefits-title padding-left15"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %></span>
                                </li>
                            </ItemTemplate>
                            </asp:Repeater>
                        </ul>                        
                        <%
                            }
                         %>
                        <% if(isBookingAvailable){ %>
                        <div class="font14 padding-top5 padding-bottom15">
                            <p class="text-light-grey">Book this bike by paying <span class="bwmsprite inr-grey-xxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bookingAmt.ToString()) %> online.</p>
                            <a href="/m/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>">Book Now</a>
                        </div>
                        <%} %>
                        <p class="font14 padding-top15 margin-bottom5 text-light-grey border-light-top">This dealer is featured by BikeWale.</p>
                        <div class="clear"></div>
                    </div>
                    <%
                       }
                     %>
                    <% if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ && viewModel.SecondaryDealerCount > 0)
                           { 
                     %>
                    <ul id="moreDealersList">
                        <asp:Repeater ID="rptSecondaryDealers" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="javascript:void(0)" class="font18 text-bold text-darker-black margin-right20" onclick="secondarydealer_Click(<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "DealerId")) %>)"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Name")) %></a><br />
                                    <span class="font14 text-light-grey"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Area")) %></span>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <div class="text-center margin-top20 margin-bottom20 font14">
                        <a href="javascript:void(0)" class="more-dealers-link">Check price from <%=viewModel.SecondaryDealerCount %> more dealers <span class="bwmsprite fa-chevron-down"></span></a>
                        <a href="javascript:void(0)" class="less-dealers-link">Show less dealers <span class="bwmsprite fa-chevron-up"></span></a>
                    </div>
                    <%
                       }
                     %>
                    </div>
                </div>
                <% } %>
                <% if(!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                   { %>
                <div class="container clearfix box-shadow">
                    <div class="bike-price-container margin-bottom15">
                        <span class="font14 text-grey padding-left10">Last known Ex-showroom Price</span>
                    </div>
                    <div class="bike-price-container margin-top10 font22 margin-bottom10 padding-left10">
                        <div class="bike-price-container margin-bottom15">
                            <span class="bwmsprite inr-lg-icon"></span>
                            <span class="font24 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPage.ModelDetails.MinPrice)) %></span>
                        </div>
                    </div>
                    <div class="bike-price-container margin-bottom15 padding-left10">
                        <div class="bike-price-container margin-bottom15">
                            <span class="font14 text-grey"><%= bikeName %> is now discontinued in India.</span>
                        </div>
                    </div>
                </div>
                <% } %>

            <!-- floating buttons -->
            <%-- <div class="grid-12 float-button float-fixed clearfix">--%>
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
                        %>  <div class="grid-6 alpha omega padding-top10 padding-right5 padding-bottom10">
                                <a id="getAssistance" leadSourceId="19" class="btn btn-white btn-full-width btn-sm rightfloat" href="javascript:void(0);">Get offers</a>
                            </div>
                            <div class="grid-6 alpha omega padding-top10 padding-bottom10 padding-left5">
                                <a id="calldealer" class="btn btn-orange btn-full-width btn-sm rightfloat" href="tel:+91<%= viewModel.MaskingNumber == string.Empty? viewModel.MobileNo: viewModel.MaskingNumber %>"><span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
                            </div>
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
                    <div class="content-inner-block-1520">
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
            ga_pg_id = '2';
            <%--if ('<%=isUserReviewActive%>' == "False") $("#ctrlUserReviews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");--%>
            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
            var getCityArea = GetGlobalCityArea();
            $(document).ready(function (e) {
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
        </script>
    </form>
</body>
</html>
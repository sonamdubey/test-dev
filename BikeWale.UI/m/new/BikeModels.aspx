 <%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeModels" EnableViewState="false" Trace="false" %>
<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/NewAlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="/m/controls/NewUserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!DOCTYPE html> 
<html>
<head>
    <%
        description = String.Format("{0} Price in India - Rs. {1}. Check out {0} on road price, reviews, mileage, versions, news & photos at Bikewale.com", bikeName, Bikewale.Utility.Format.FormatPrice(price.ToString()));
        title = String.Format("{0} Price in India, Review, Mileage & Photos - Bikewale", bikeName);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/{1}/", modelPage.ModelDetails.MakeBase.MaskingName, modelPage.ModelDetails.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1017752";
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
            <div itemscope="" itemtype="http://auto.schema.org/Motorcycle" class="container bg-white clearfix">
                <span itemprop="name" class="hide"><%= bikeName %></span>
                <div class="<%= !modelPage.ModelDetails.New ? "padding-top20 position-rel" : ""%>">
                    <% if (modelPage.ModelDetails.New)
                       { %><h1 class="font16 text-darker-black padding-top10 padding-left20 padding-right20"><%= bikeName %></h1>
                    <% } %>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Upcoming</div>
                    <div class="bikeTitle">
                        <h1 class="font18 text-darker-black padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>
                    <% if(!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Discontinued</div>
                    <div class="bikeTitle">
                        <h1 class="font18 text-darker-black padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>

                    <% if (modelPage.ModelDetails.New || !modelPage.ModelDetails.New)
                       { %>
                    <div class="padding-left20 padding-right10 margin-top5 margin-bottom5">
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

                    <div class="swiper-container model margin-bottom10" id="bikeBannerImageCarousel">
                        <div class="swiper-wrapper stage" id="ulModelPhotos">
                            <asp:Repeater ID="rptModelPhotos" runat="server">
                                <ItemTemplate>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <% if (modelPage.Photos != null && modelPage.Photos.Count > 1)
                           { %>
                        <p class="pagination-number text-center font12 text-white position-abt"><span class="bike-model-gallery-count">1 of <%= modelPage.Photos.Count %></span></p>
                        <% } %>
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
                <div class="grid-12 bg-white box-shadow" id="dvBikePrice">

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
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity"><span class="bwmsprite loc-change-blue-icon"></span></a>
                            </p>
                            <% } %>
                            <% else
                                   {%>
                            <p class="margin-top10 margin-bottom10 font14 text-light-grey clear">
                                On-road price in <span><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity"><span class="bwmsprite loc-change-blue-icon"></span></a>
                            </p>
                            <% } %>
                            <div itemprop="offers" itemscope itemtype="http://schema.org/Offer" class="margin-bottom5">
                                <p class="line-Ht18">

                                    <%if (price > 0)
                                      { %>

                                    <span class="font22 text-bold padding-bottom10">
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
                            <p class="font12 text-light-grey clear" />
                            <% } %>
                            <% if (!toShowOnRoadPriceButton && isBikeWalePQ)
                               { %>
                            <p class="margin-top10 margin-bottom20 clear">
                                <a class="text-bold" style="position: relative; font-size: 14px; margin-top: 1px;" target="_blank" href="/m/insurance/" id="insuranceLink">Save up to 60% on insurance - PolicyBoss
                                </a>
                            </p>
                            <% } %>
                    </div>                    
                    <%
                       if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) { 
                     %>
                    <div class="margin-top15 content-inner-block-10 border-solid">
                        <h2 class="font18 text-darker-black"><%=viewModel.Organization %></h2>
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
            <div class="grid-12 float-button float-fixed clearfix">

                <a id="btnGetOnRoadPrice" href="javascript:void(0)" ismodel="true" modelid="<%=modelId %>" style="width: 100%" class="btn btn-orange margin-top10 fillPopupData">Check On-Road Price</a>
                <% }
                   else
                   {   %>
                <div class="grid-12 float-button float-fixed clearfix">
                    <div class="show padding-top10">
                        <% if (modelPage.ModelDetails.New && viewModel != null && !isBikeWalePQ )
                           {   
                         %>
                            <% if ( viewModel.IsPremiumDealer)
                               { 
                            %>
                                    <div class="grid-6 alpha omega padding-right5">
                                        <a id="getAssistance" leadSourceId="19" class="btn btn-white btn-full-width btn-sm rightfloat" href="javascript:void(0);">Get offers</a>
                                    </div>
                                    <div class="grid-6 alpha omega padding-left5">
                                        <a id="calldealer" class="btn btn-orange btn-full-width btn-sm rightfloat" href="tel:+91<%= viewModel.MaskingNumber == string.Empty? viewModel.MobileNo: viewModel.MaskingNumber %>"><span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
                                    </div>
                            <% }                             
                           }  
                        %>
                    </div>
                </div>
                <%
                }
               }
               %>
            </div>
        </section>
        <% if (Ad_300x250)
           { %>
        <section>
            <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
        </section>
        <% } %>

        <section>
            <div id="modelSpecsTabsContentWrapper" class="container bg-white clearfix box-shadow margin-top30 margin-bottom30">
                <div id="modelOverallSpecsTopContent">
                    <div id="overallSpecsTab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <li class="active" data-tabs="#modelSummaryContent"><h3>Summary</h3></li>
                            <li data-tabs="#modelPricesContent"><h3>Prices</h3></li>
                            <li data-tabs="#modelSpecsFeaturesContent"><h3>Specs & Features</h3></li>
                            <li data-tabs="#modelReviewsContent"><h3>Reviews</h3></li>
                            <li data-tabs="#makeNewsContent"><h3>News</h3></li>
                            <li data-tabs="#modelAlternateBikeContent"><h3>Alternatives</h3></li>                         
                        </ul>
                    </div>
                </div>

                <div id="modelSummaryContent" class="bw-model-tabs-data content-inner-block-1520">
                    <h2>Bajaj Pulsar RS200 Summary</h2>
                    <h3>Preview</h3>
                    <p class="font14 text-light-grey line-height17 margin-bottom15">
                        <span class="model-preview-main-content">After number of spy pictures doing the round of the internet, Bajaj Motorcycles has finally 
                        launched its first fully-faired motorcycle, the Pulsar RS 200 for the Indian market. Previously 
                        touted to be called as the Pulsar SS200, this bike has been the most anticipated...
                        </span>
                        <span class="model-preview-more-content">After number of spy pictures doing the round of the internet, Bajaj Motorcycles has finally 
                        launched its first fully-faired motorcycle, the Pulsar RS 200 for the Indian market. Previously 
                        touted to be called as the Pulsar SS200, this bike has been the most anticipated launch from 
                        the company.<br /><br />
                        Marketed as the fastest Pulsar yet, the Pulsar RS200 designed to be a compact sportsbike
                        and features clip-on handlebars. Unlike other fully-faired motorcycle like the Yamaha YZF-R15,
                        the RS200 doesn’t have as aggressive riding stance as of a super sport motorcycle.
                        </span>
                        <a href="javascript:void(0)" class="read-more-model-preview" rel="nofollow">Read more</a>
                    </p>

                    <h3>Specification summary</h3>
                    <div class="text-center">
                        <div class="summary-overview-box">
                        <div class="odd btmAftBorder">
                            <span class="inline-block model-sprite specs-capacity-icon margin-right15"></span>
                            <div class="inline-block">
                                <p class="font18 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %>
                                    <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "hide":"" %>'>cc</span>
                                </p>
                                <p class="font16 text-light-grey">Capacity</p>
                            </div>
                        </div>
                        <div class="even btmAftBorder">
                            <span class="inline-block model-sprite specs-mileage-icon margin-right15"></span>
                            <div class="inline-block">
                                <p class="font18 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                    <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall).Equals("--") ? "hide":"" %>'>kmpl</span>
                                </p>
                                <p class="font16 text-light-grey">Mileage</p>
                            </div>
                        </div>
                        <div class="odd">
                            <span class="inline-block model-sprite specs-maxpower-icon margin-right15"></span>
                            <div class="inline-block">
                                <p class="font18 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %>
                                    <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "hide":"text-uppercase" %>'>ps</span>
                                </p>
                                <p class="font16 text-light-grey">Max power</p>
                            </div>
                        </div>
                        <div class="even">
                            <span class="inline-block model-sprite specs-weight-icon margin-right15"></span>
                            <div class="inline-block">
                                <p class="font18 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight) %>
                                    <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight).Equals("--") ? "hide":"" %>'>kg</span>
                                </p>
                                <p class="font16 text-light-grey">Weight</p>
                            </div>
                        </div>
                    </div>
                    </div>
                </div>

                <div class="margin-right20 margin-left20 border-solid-top"></div>

                <div id="modelPricesContent" class="bw-model-tabs-data">
                    <h2 class="padding-top15 padding-right20 padding-left20">Bajaj Pulsar RS200 Prices</h2>
                    <h3 class="padding-right20 padding-left20">Prices by versions</h3>

                    <div class="swiper-container">
                        <div class="swiper-wrapper font14">
                            <div class="swiper-slide model-prices-version-content rounded-corner2">
                                <p class="text-bold text-truncate margin-bottom13">Self Start Double Disc Brake Alloy Wheels</p>
                                <p class="text-truncate text-xt-light-grey margin-bottom13">Alloy wheel, Disc brake, Electric Start, ABS</p>
                                <p class="text-truncate text-light-grey margin-bottom10">On-road price in Andheri, Mumbai</p>
                                <p class="font18 text-bold text-black">
                                    <span class="bwmsprite inr-dark-md-icon"></span>
                                    <span>50,551</span>
                                </p>
                            </div>
                            <div class="swiper-slide model-prices-version-content rounded-corner2">
                                <p class="text-bold text-truncate margin-bottom13">Self Start Double Disc Brake Alloy Wheels</p>
                                <p class="text-truncate text-xt-light-grey margin-bottom13">Alloy wheel, Disc brake, Electric Start, ABS</p>
                                <p class="text-truncate text-light-grey margin-bottom10">On-road price in Andheri, Mumbai</p>
                                <p class="font18 text-bold text-black">
                                    <span class="bwmsprite inr-dark-md-icon"></span>
                                    <span>50,551</span>
                                </p>
                            </div>
                            <div class="swiper-slide model-prices-version-content rounded-corner2">
                                <p class="text-bold text-truncate margin-bottom13">Self Start Double Disc Brake Alloy Wheels</p>
                                <p class="text-truncate text-xt-light-grey margin-bottom13">Alloy wheel, Disc brake, Electric Start, ABS</p>
                                <p class="text-truncate text-light-grey margin-bottom10">On-road price in Andheri, Mumbai</p>
                                <p class="font18 text-bold text-black">
                                    <span class="bwmsprite inr-dark-md-icon"></span>
                                    <span>50,551</span>
                                </p>
                            </div>
                        </div>
                    </div>

                    <div>
                        <h3 class="padding-top15 padding-right20 padding-left20 margin-bottom20">Prices by cities<span class="text-light-grey text-unbold"> (Ex-showroom)</span></h3>
                        <ul class="prices-by-cities-list font14">
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Pune</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Bangalore</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Chennai</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Hyderabad</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Pune</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Bangalore</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Chennai</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                            <li>
                                <a href="javascript:void(0)" class="text-truncate">Hyderabad</a>
                                <span class="price-in-city-price">
                                    <span class="bwmsprite inr-dark-grey-xsm-icon"></span>
                                    <span>1.62 L</span>
                                </span>
                            </li>
                        </ul>
                    </div>

                </div>

                <div class="margin-right20 margin-left20 border-solid-top"></div>

                <div id="modelSpecsFeaturesContent" class="bw-model-tabs-data font14">
                    <div class="content-inner-block-1520">
                        <h2>Bajaj Pulsar RS200 Specifications & Features</h2>
                        <h3>Specifications</h3>

                        <ul id="modelSpecsList">
                            <li>
                                <div class="text-light-grey padding-right10">Displacement</div>
                                <div class="text-bold">150 cc</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Max Power</div>
                                <div class="text-bold">14.30 bhp@2500 rpm</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Maximum Torque</div>
                                <div class="text-bold">12.50 Nm@2500 rpm</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">No. of gears</div>
                                <div class="text-bold">5</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Fuel Tank Capacity</div>
                                <div class="text-bold">10 litres</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Top Speed</div>
                                <div class="text-bold">110 kmph</div>
                            </li>
                        </ul>
                        <div class="margin-top25">
                            <a href="javascript:void(0)">View full specifications<span class="bwmsprite blue-right-arrow-icon"></span></a>
                        </div>

                        <h3 class="margin-top25">Features</h3>

                        <ul id="modelFeaturesList">
                            <li>
                                <div class="text-light-grey padding-right10">Speedometer</div>
                                <div class="text-bold">Analogue</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Fuel Guage</div>
                                <div class="text-bold">Yes</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Tachometer Type</div>
                                <div class="text-bold">--</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Digital Fuel Guage</div>
                                <div class="text-bold">No</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Tripmeter</div>
                                <div class="text-bold">Yes</div>
                            </li>
                            <li>
                                <div class="text-light-grey padding-right10">Electric Start</div>
                                <div class="text-bold">Yes</div>
                            </li>
                        </ul>
                        <div class="margin-top25">
                            <a href="javascript:void(0)">View full features<span class="bwmsprite blue-right-arrow-icon"></span></a>
                        </div>
                            
                        <h3 class="margin-top25">Colours</h3>

                        <ul id="modelColorsList" class="margin-top5">
                            <li>
                                <div class="color-box color-count-one inline-block">
                                    <span style="background-color:#c83333"></span>
                                </div>
                                <p class="inline-block">Red</p>
                            </li>
                            <li>
                                <div class="color-box color-count-one inline-block">
                                    <span style="background-color:#3a5cee"></span>
                                </div>
                                <p class="inline-block">Blue</p>
                            </li>
                            <li>
                                <div class="color-box color-count-one inline-block">
                                    <span style="background-color:#1dc97e"></span>
                                </div>
                                <p class="inline-block">Green</p>
                            </li>
                            <li>
                                <div class="color-box color-count-two inline-block">
                                    <span style="background-color:#c83333"></span>
                                    <span style="background-color:#040004"></span>
                                </div>
                                <p class="inline-block">Dual Tone Red</p>
                            </li>
                        </ul>

                    </div>

                    <div class="margin-top15 margin-bottom15 text-center">
                        <div style="width:300px; height:250px; background:#ccc; margin:0 auto"></div>
                    </div>
                </div>
                    <div id="modelReviewsContent" class="bw-model-tabs-data padding-top10 padding-right20 padding-left20 font14">
                    <h2><%=bikeName %> Reviews</h2>
                       
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                           { %>
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <% } %>
                        <%if (ctrlUserReviews.FetchedRecordsCount > 0)
                              { %>
                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                        <% } %>
                        
                        <%if (ctrlVideos.FetchedRecordsCount > 0)
                            { %>
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <% } %>

                    </div>
                <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                   { %>
                    <BW:AlternateBikes ID="ctrlAlternativeBikes" runat="server" />           
                <%} %>
                <div class="margin-top20 margin-right20 margin-left20 border-solid-top"></div>

                <div id="modelForSaleContent" class="bw-model-tabs-data content-inner-block-1222 font14">
                    <h2>Used Bajaj Pulsar RS200 for sale</h2>
                    <ul class="margin-bottom10">
                        <li>
                            <a href="">2009, Bajaj Pulsar 220 Fi Standard</a>
                            <p class="margin-top10 text-black"><span class="bwmsprite inr-dark-grey-xsm-icon"></span> <span>1,67,673</span> in Mumbai</p>
                        </li>
                        <li>
                            <a href="">2009, Bajaj Pulsar 220 Fi Standard</a>
                            <p class="margin-top10 text-black"><span class="bwmsprite inr-dark-grey-xsm-icon"></span> <span>1,67,673</span> in Mumbai</p>
                        </li>
                        <li>
                            <a href="">2009, Bajaj Pulsar 220 Fi Standard</a>
                            <p class="margin-top10 text-black"><span class="bwmsprite inr-dark-grey-xsm-icon"></span> <span>1,67,673</span> in Mumbai</p>
                        </li>
                    </ul>
                </div>
                
                <div id="modelSpecsFooter"></div>
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
                        <span><a href="javascript:void(0)" class="read-more-btn">Read <span>full story</span></a></span>
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
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>bhp</small></span>
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
                <div class="grid-12 alpha omega">
                    <h2 class="text-center margin-top30 margin-bottom20">Specifications</h2>


                    <div class="bw-tabs-panel clearfix">

                        <div class="bw-tabs bw-tabs-flex">
                            <ul>
                                <li class="active" data-tabs="summary"><h3>Summary</h3></li>
                                <li data-tabs="engineTransmission"><h3>Engine &amp; Transmission </h3></li>
                                <li data-tabs="brakeWheels"><h3>Brakes, Wheels and Suspension</h3></li>
                                <li data-tabs="dimensions"><h3>Dimensions and Chassis</h3></li>
                                <li data-tabs="fuelEffiency"><h3>Fuel efficiency and Performance</h3></li>
                            </ul>
                        </div>
                        <div class="grid-12">
                            <div class="leftfloat bw-horz-tabs-data font16">
                                <div class="bw-tabs-data" id="summary">
                                    <ul>
                                        <li>
                                            <div class="text-light-grey">Displacement</div>
                                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement,"cc") %></div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Max Power</div>
                                            <div class="text-bold">
                                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", 
                                                                    modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Maximum Torque</div>
                                            <div class="text-bold">
                                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm",
                                                                    modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %>
                                            </div>
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
                                            <div class="text-bold">
                                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", 
                                                                   modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="text-light-grey">Maximum Torque</div>
                                            <div class="text-bold">
                                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm",
                                                                       modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %>
                                            </div>
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
                    </div>
                    <!-- bw-tabs ends here -->

                    <!-- features code starts here -->
                    <div class="grid-12">
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
                                <div class="more-features-btn"><a href="javascript:void(0)">+</a></div>
                            </div>
                        </div>
                    </div>

                    <!-- variant code starts here -->
                    <div class="grid-12">
                        <div class="bw-tabs-data <%= (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0) ? "" : "hide" %>" id="variants">
                            <h2 class="text-center margin-top30 margin-bottom20 text-center">Versions</h2>
                            <asp:Repeater ID="rptVarients" runat="server" OnItemDataBound="rptVarients_ItemDataBound2">
                                <ItemTemplate>
                                    <div>
                                        <div class="border-solid content-inner-block-10 margin-bottom20">
                                            <div class="grid-8 alpha">
                                                <h3 class="font16 margin-bottom10"><%# DataBinder.Eval(Container.DataItem, "VersionName") %></h3>
                                                <%--<p class="font14">220 CC, 38 Kmpl, 103 bhp @ 11000 rpm</p>--%>
                                                <p class="font14"><%# Bikewale.Utility.FormatMinSpecs.GetMinVersionSpecs(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")), Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")), Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                            </div>
                                            <div class="grid-4 alpha omega">
                                                <p class="font16 margin-bottom10 text-bold">
                                                    <span class="bwmsprite inr-xsm-icon"></span>
                                                    <span id="<%# "priced_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>">
                                                        <asp:Label Text='<%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %>' ID="txtComment" runat="server"></asp:Label>
                                                    </span>
                                                </p>
                                                <p class="font12 text-light-grey" id="<%# "locprice_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>">
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
                                                <asp:HiddenField ID="hdnVariant" runat="server" Value='<%#Eval("VersionId") %>' />
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <!-- colours code starts here -->
                    <div class="grid-12">
                        <div class="colours-wrap margin-bottom20 <%= modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0 ? "" : "hide" %>">
                            <h2 class="margin-top30 margin-bottom20 text-center">Colours</h2>
                            <div class="swiper-container padding-bottom60">
                                <div class="swiper-wrapper text-center">
                                    <%-- <asp:Repeater ID="rptColors" runat="server">
                                        <ItemTemplate>
                                            <div class="swiper-slide available-colors">
                                                <div class="color-box" style="background-color: #<%# DataBinder.Eval(Container.DataItem, "HexCode")%>;"></div>
                                                <p class="font16 text-medium-grey"><%# DataBinder.Eval(Container.DataItem, "ColorName") %></p>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>--%>

                                    <asp:Repeater ID="rptColors" runat="server">
                                        <ItemTemplate>
                                            <div class="swiper-slide available-colors">
                                                <div class="color-box <%# (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count == 1 )?"color-count-one": (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count >= 3 )?"color-count-three":"color-count-two" %>">
                                                    <asp:Repeater runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "HexCodes") %>'>
                                                        <ItemTemplate>
                                                            <span <%# String.Format("style='background-color: #{0}'",Convert.ToString(Container.DataItem)) %>></span>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                                <p class="font16"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ColorName")) %></p>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                </div>
                                <!-- Add Pagination -->
                                <div class="swiper-pagination"></div>
                                <!-- Navigation -->
                                <div class="bwmsprite swiper-button-next hide"></div>
                                <div class="bwmsprite swiper-button-prev hide"></div>

                            </div>
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
        <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News, reviews and videos code starts here -->
            <div class="container padding-bottom10">
                <div class="grid-12 alpha omega">
                    <h2 class="text-center margin-top30 margin-bottom20">Latest Updates</h2>
                    <div class="bw-tabs-panel">
                        <div class="bw-tabs bw-tabs-flex margin-bottom15 <%= reviewTabsCnt == 1 ? "hide" : "" %>">
                            <ul>

                                <li style="<%= (Convert.ToInt32(ctrlUserReviews.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%=isUserReviewActive ? "active" : "hide" %>" data-tabs="ctrlUserReviews"><h3>User Reviews</h3></li>
                                <li style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%=isExpertReviewActive ? "active" : "hide" %>" data-tabs="ctrlExpertReviews"><h3>Expert Reviews</h3></li>
                                <li style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%= isNewsActive ? "active" : "hide" %>" data-tabs="ctrlNews"><h3>News</h3></li>
                                <li style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%= isVideoActive ? "active" : "hide" %>" data-tabs="ctrlVideos"><h3>Videos</h3></li>

                            </ul>
                        </div>
                        <div class="grid-12">
                           
                            <%--<%if (!isExpertReviewZero)
                              { %>
                            <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                            <% } %>--%>
                            <%if (!isNewsZero)
                              { %>
                            <BW:News runat="server" ID="ctrlNews" />
                            <% } %>                           
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="<%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom10">
                <div class="grid-12">
                    <!-- Most Popular Bikes Starts here-->
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= bikeName %> Alternate Bikes </h2>

                    <div class="swiper-container discover-bike-carousel alternatives-carousel padding-bottom60">
                        <div class="swiper-wrapper">                          
                        </div>
                        <!-- Add Pagination -->
                        <div class="swiper-pagination"></div>
                        <!-- Navigation -->
                        <div class="bwmsprite swiper-button-next hide"></div>
                        <div class="bwmsprite swiper-button-prev hide"></div>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- Terms and condition Popup Ends -->

        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <!-- Contact details Popup starts here -->
                    <h2 class="margin-bottom5">Provide contact details</h2>
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

        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />

        <!-- all other js plugins -->

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
                if ($("#getAssistance").length > 0)
                {
                    dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Model_Page", "act": "Get_Offers_Shown", "lab": myBikeName + "_" + getBikeVersion() + '_' + getCityArea });
                }
            });
            function secondarydealer_Click(dealerID) {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerID;
                window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            }
        </script>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeModels" EnableViewState="false" Trace="false" %>
<%@ Register Src="/m/controls/NewNewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/NewAlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="/m/controls/NewUserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/m/controls/MPriceInTopCities.ascx" TagPrefix="BW" TagName="TopCityPrice" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="/m/controls/PopularModelComparison.ascx" TagName="SimilarBikesCompare" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/UsedBikes.ascx" TagName="MostRecentusedBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <%
        description = pgDescription;
        title = String.Format("{0} Price, Reviews, Spec, Images, Mileage, Colours | Bikewale", bikeName);
        canonical = String.Format("https://www.bikewale.com/{0}-bikes/{1}/", modelPage.ModelDetails.MakeBase.MaskingName, modelPage.ModelDetails.MaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        Ad_300x250 = true;
        TargetedModel = bikeModelName;
        TargetedCity = cityName;
        keywords = string.Format("{0}, {0} Bike, bike, {0} Price, {0} Reviews, {0} Images, {0} Mileage", bikeName);
        EnableOG = true;
        OGImage = modelImage;
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/bwm-model-atf.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->

        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var versionId = '<%= versionId%>';
        var cityId = '<%= cityId%>';
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
    
</head>
<body itemscope itemtype="http://schema.org/Product">
     <% if (modelPage != null && modelPage.ModelDesc != null)
                                       { %>
    <meta itemprop="description" itemtype="https://schema.org/description" content= "<%= modelPage.ModelDesc.SmallDescription %>" />
    <% } %>
    <meta itemprop="image" content="<%= modelImage %>" />
    <meta itemprop="manufacturer" name="manufacturer" content="<%= modelPage.ModelDetails.MakeBase.MakeName %>">  
    <meta itemprop="model" content="<%= modelPage.ModelDetails.ModelName %>"/>
    <meta itemprop="brand" content="<%= modelPage.ModelDetails.MakeBase.MakeName %>"> 
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container bg-white clearfix">
                <span itemprop="name" class="hide"><%= bikeName %></span>
                <div class="<%= !modelPage.ModelDetails.New ? "padding-top20 position-rel" : ""%>">
                    <% if (modelPage.ModelDetails.New)
                       { %><h1 class="padding-top10 padding-left20 padding-right20"><%= bikeName %></h1>
                    <% } %>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt text-white text-center">Upcoming</div>
                    <div class="bikeTitle">
                        <h1 class="padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>
                    <% if(!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt text-white text-center">Discontinued</div>
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
                        <% if (modelPage.ModelDetails.ReviewCount > 0)
                           { %>
                        <span itemprop="aggregateRating" itemscope itemtype="http://schema.org/AggregateRating">
                        <meta itemprop="ratingValue" content="<%=modelPage.ModelDetails.ReviewRate %>">
                        <meta itemprop="worstRating" content="1">
                        <meta itemprop="bestRating" content="5">
                        <meta itemprop="itemreviewed" content="<%= bikeName %>" />
                            <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> border-solid-left leftfloat margin-right10 padding-left10 line-Ht22">
                                <span itemprop="ratingCount"><%= modelPage.ModelDetails.ReviewCount %>
                                </span>Reviews
                            </a>
                        </span>
                        <% } %>
                        <div class="clear"></div>
                    </div>
                    <% } %>
                    <div id="model-image-wrapper">
                        <div class="model-main-image">
							<% if (!String.IsNullOrEmpty(modelPage.ModelDetails.OriginalImagePath)) { %>
							<a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/images/?modelpage=true#modelGallery" title="<%= bikeName + " images"%>" class="block">
							<% } %>
                            <img src="<%=modelImage %>" alt="<%= bikeName %> images" title="<%= bikeName %> model image " class="cursor-pointer" />
							<% if (!String.IsNullOrEmpty(modelPage.ModelDetails.OriginalImagePath)) { %>
							</a>
							<% } %>
                            <div class="model-media-details">
                                <% if (modelPage.ModelDetails.PhotosCount>0)
                                { %>
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/images/" class="model-media-item">
                                    <span class="bwmsprite photos-sm"></span>
                                    <span class="model-media-count"><%= modelPage.ModelDetails.PhotosCount %></span>
                                </a>
                                <% } %>
                               <% if (modelPage.ModelDetails.VideosCount>0)
                                { %>
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/videos/" class="model-media-item">
                                    <span class="bwmsprite videos-sm"></span>
                                    <span class="model-media-count"><%=modelPage.ModelDetails.VideosCount%></span>
                                </a>                              
                               <% } %>
                            </div>                           
                        </div>
                        <!-- Upcoming bike start -->
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                             <div class="bikeDescWrapper">
                          <% if (modelPage.UpcomingBike != null) {%>
                         <div class="margin-bottom10 font14 text-light-grey">Expected price</div>
                         <div itemscope itemtype="http://schema.org/Offer">
                            <div itemprop="priceSpecification" itemscope itemtype="http://schema.org/PriceSpecification">
                             <span class="font24 text-bold"><span itemprop="priceCurrency" content="INR"><span class="bwmsprite inr-lg-icon"></span></span>
                                <span itemprop="price minPrice"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %></span>  - 
                                <span itemprop="maxPrice"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                         </div>
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
                        <!-- Upcoming bike ends -->
                </div>
                
                <!-- previous top model card -->
            <% if (!isDiscontinued)
               {
                   if (toShowOnRoadPriceButton)
                   {   %>
            <div class="grid-12 float-button float-fixed clearfix padding-bottom10">

                <a id="btnGetOnRoadPrice" href="javascript:void(0)" data-reload="true"  data-persistent="true" data-modelid="<%=modelId %>" style="width: 100%" class="btn btn-orange getquotation">Check on-road price</a>
                <% }
                   else
                   {   %>
                <div class="grid-12 float-button float-fixed clearfix">
                    
                    <% if (modelPage.ModelDetails.New && viewModel != null && !isBikeWalePQ)
                        {   
                        %>
                        <% if (viewModel!=null && viewModel.IsPremiumDealer)
                            { 
                        %>  <div class="grid-<%= String.IsNullOrEmpty(viewModel.MaskingNumber)? "12":"6" %> alpha omega padding-right5">
                                <a  class="btn btn-orange btn-full-width rightfloat leadcapturebtn"  data-leadsourceid="19"  data-item-id="<%= dealerId %>" data-item-name="<%= viewModel.Organization %>"  data-item-area="<%= viewModel.AreaName %> " href="javascript:void(0);"><%= viewModel.LeadBtnTextSmall %></a>
                            </div>
                            <%  if(!string.IsNullOrEmpty(viewModel.MaskingNumber))
                                { %>
                                <div class="grid-6 alpha omega padding-left5">
                                    <a id="calldealer" class="btn btn-green btn-full-width rightfloat" href="tel:+91<%= String.IsNullOrEmpty(viewModel.MaskingNumber)? viewModel.MobileNo: viewModel.MaskingNumber %>"><span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
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
                <!-- new bikes section -->
                <div class="grid-12 padding-top5 padding-bottom5 border-solid-bottom">
                    <div class="grid-6 alpha border-solid-right">
                        <p class="font12 text-light-grey padding-left10">Version:</p>
                        <p id="defversion" class="single-version-label font14"><%=variantText %></p>
                        <% if (versionCount > 1)
                           { %>
                            <div class="dropdown-select-wrapper">
                                <asp:DropDownList CssClass="dropdown-select" ID="ddlNewVersionList" runat="server" />
                            </div>
                        <% } %>
                    </div>
                    <div class="grid-6 padding-left20">
                        <p class="font12 text-light-grey">Location:</p>
                        <% if(!isDiscontinued) {%>
                        <div class="font14 text-bold">
                            <a href="javascript:void(0)" data-reload="true" data-persistent="true" data-modelid='<%= modelId %>' class="getquotation changeCity" rel="nofollow">
                                <span class="selected-location-label inline-block text-default text-truncate"><%= location %></span>                            
                                <span class="bwmsprite loc-change-blue-icon"></span>
                            </a>
                        </div>
                        <% } else{ %>
                        <span class="text-bold font14 text-truncate block margin-top5"><%= location %></span>  
                        <% } %>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <% 
                   if(isDiscontinued && !modelPage.ModelDetails.Futuristic) { %>
                       <div class="bike-price-container padding-left20 padding-top10">
                                <span class="font14 text-grey"><%= bikeName %> is now discontinued in India.</span>
                        </div>
                 <% } 
                   if (!modelPage.ModelDetails.Futuristic)
                   { %>
                        <div class="padding-10-20" itemprop="offers" itemscope itemtype="http://schema.org/Offer">
                            <p class="font12 text-light-grey"><%=priceText %> price in <%= location%></p>
                            <p><span itemprop="priceCurrency" content="INR">
                                <% if(price == 0) { %>
                                <span class="font18 text-bold padding-right5" itemprop="price" content="0">Price not available</span>
                                <% } else { %>
                                <span class="bwmsprite inr-md-icon"></span>
                                </span>
                                <span class="font22 text-bold padding-right5" itemprop="price" content="<%= price %>"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                <% } %>
                                <%if (isOnRoadPrice && price > 0)
                                  {%>
                                <a href="/m/pricequote/dealerpricequote.aspx?MPQ=<%= detailedPriceLink %>" class="font16 text-bold viewBreakupText" rel="nofollow" >View detailed price</a>
                                <% } %>
                            </p>
                        </div>
                <% } %>
                <%
                    if (viewModel != null && !isBikeWalePQ)
                    { 
                %>
                         <div id="model-dealer-card">
                    <% if(viewModel.IsPremiumDealer){ %>
                    <div class="dealer-details margin-bottom10">
                        <div class="dealership-icon-wrapper inline-block margin-right5">
                            <span class="offers-sprite dealership-icon"></span>
                        </div>
                        <div class="inline-block">
                            <h2 id="dealername" class="text-default"><%=viewModel.Organization %></h2>
                            <p class="font14 text-light-grey"><%=(!viewModel.IsDSA ? "Authorized Dealer in " : "Multi-brand Dealer in ") %><%=viewModel.AreaName %></p>
                        </div>
                        <%if(viewModel.IsDSA){ %>
                        <div class="bw-tooltip multi-brand-tooltip tooltip-bottom slideUp-tooltip">
                            <p class="bw-tooltip-text position-rel font14">This dealer sells bikes of multiple brands.<br />Above price is not final and may vary at the dealership.</p>
                            <span class="position-abt pos-top10 pos-right10 bwmsprite cross-sm-dark-grey cur-pointer close-bw-tooltip"></span>
                        </div>
                        <%} %>
                    </div>
                    <% } %>
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
                                        <p class="font18 text-bold text-light-grey margin-top5 margin-bottom7">+<%= moreOffersCount %> </p>
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
                                    <p class="booknow-label font11 line-height-1-7 text-xx-light vertical-top">
                                        Pay <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bookingAmt.ToString()) %> to book online and <br />balance of <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice((price - bookingAmt).ToString()) %> at dealership
                                    </p>
                                </div>
                            <% }
                                     else
                            { %>
                             <div class="margin-bottom20">
                                <div class="vertical-top">
                                    <a id="requestcallback" c="Model_Page" a="Request_Callback_Details_Clicked" v="bikeVersionLocation" data-leadsourceid="30"  data-item-id="<%= dealerId %>" data-item-name="<%= viewModel.Organization %>"  data-item-area="<%= viewModel.AreaName %>" href="javascript:void(0)" class="btn btn-white callback-btn btn-sm-0 bw-ga leadcapturebtn">Request callback</a>
                                </div>
                                <p class="callback-label font11 line-height-1-7 text-xx-light vertical-top">Get EMI options, test rides other services from dealer</p>
                             </div>
                            <% 
                            }
                        }
                       else
                       { %>
                    <div class="margin-bottom20">
                        <div class="inline-block nearby-partner-left-col">
                            <div class="dealership-icon-wrapper inline-block margin-right5">
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
                 <% } %>
                <% if (viewModel!= null && viewModel.SecondaryDealerCount > 0)
                   { %>
                    <div class="content-inner-block-20 border-solid-top font16">
                        <a href="javascript:void(0)" rel="nofollow" id="more-dealers-target">Prices from <%=viewModel.SecondaryDealerCount %> more partner dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                <% } %>
            </div>
        </section>

          <% if (pqOnRoad != null && pqOnRoad.BPQOutput != null && viewModel == null&& isOnRoadPrice && !string.IsNullOrEmpty(pqOnRoad.BPQOutput.ManufacturerAd))
            { %>
        <section>
            
            <%=String.Format(pqOnRoad.BPQOutput.ManufacturerAd) %>
       
        </section>
        <%} %>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <section>
            <div id="modelSpecsTabsContentWrapper" class="container bg-white clearfix box-shadow margin-top10 margin-bottom20 content-details-wrapper">
                <div id="modelOverallSpecsTopContent">
                    <div id="overallSpecsTab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <% if ((modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription)) || modelPage.ModelVersionSpecs != null)
                           { %>
                            <li data-tabs="#modelSummaryContent">Summary</li>
                            <% } %>
                            <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                            { %>
                            <li data-tabs="#modelPricesContent">Price</li>
                            <%} %>
                            <%if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                            { %>
                            <li data-tabs="#modelColoursContent">Colours</li>
                            <%} %>
                            <% if(modelPage.ModelVersionSpecs!= null){ %>
                            <li data-tabs="#modelSpecsFeaturesContent">Specs & Features</li>
                            <% } %>                            
                           <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                           { %>
                             <li data-tabs="#modelReviewsContent">Reviews</li>
                        <%} %>
                        <% if (ctrlExpertReviews.FetchedRecordsCount == 0 && ctrlUserReviews.FetchedRecordsCount == 0 && ctrlNews.FetchedRecordsCount > 0) { %>
                            <li data-tabs="#modelReviewsContent">News</li>
                        <% } %>
                            <% if (ctrlVideos.FetchedRecordsCount > 0)
                                { %>
                                <li data-tabs="#modelVideosContent">Videos</li>
                            <%} %>
                         
                            <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                              { %>
                                 <li data-tabs="#modelSimilarContent">Similar Bikes</li>
                            <%} %>
                            <% if ((!isDiscontinued && !modelPage.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0))) { %>
                            <li data-tabs="#dealerAndServiceContent">
                                <% if (ctrlDealerCard.showWidget){%>Dealers<%} %>
                                <%if (ctrlDealerCard.showServiceCenter || (ctrlServiceCenterCard.showWidget && cityId > 0)) { %>
                                    <% if (ctrlDealerCard.showWidget){%> &<%}%> Service Centers
                                <%} %>
                            </li>
                            <%} %>
                            
                            <% if (ctrlRecentUsedBikes.fetchedCount>0) {%>
                            <li data-tabs="#makeUsedBikeContent">Used</li>
                            <%} %>
                        </ul>
                    </div>
                </div>

                <%if (modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription) || (modelPage.ModelVersionSpecs != null))
                  { %>
                <div id="modelSummaryContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom15 border-solid-bottom">
                  <%if (!modelPage.ModelDetails.Futuristic && modelPage.ModelDetails.MinPrice > 0)
                              { %>
                    <h2><%=bikeName %> summary</h2>
                    <p class="font14 text-light-grey line-height17 margin-bottom15"><%=summaryDescription %></p>

                    <div class="border-solid-bottom padding-bottom10 margin-bottom15">
                        <table id="model-key-highlights" cellspacing="0" cellpadding="0" width="100%" border="0" class="font14 text-left">
                            <thead>
                                <tr>
                                    <th colspan="2"><%= modelPage.ModelDetails.ModelName %> key highlights</th>
                                </tr>
                            </thead>
                            <tbody>
                                  <% if (modelPage.ModelDetails.MinPrice > 0){%> 
                                <tr>
                                    <td valign="top" width="36%">Price</td>
                                    <td valign="top" width="64%">
                                        <span class="bwmsprite inr-dark-grey-xsm-icon"></span><span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPage.ModelDetails.MinPrice)) %></span><br />
                                        <span class="font12 text-light-grey">Ex-showroom <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span>
                                    </td>
                                </tr>
                                <%} %>
                                <% if(modelPage!=null && modelPage.ModelVersionSpecs!=null && modelPage.ModelVersionSpecs.TopSpeed>0) {%>
                                <tr>
                                    <td valign="top">Top speed</td>
                                    <td valign="top" class="text-bold"><%=modelPage.ModelVersionSpecs.TopSpeed%> kmph</td>
                                </tr>
                                <%} %>
                                <%if (modelPage != null && modelPage.ModelVersionSpecs != null && modelPage.ModelVersionSpecs.FuelEfficiencyOverall > 0)
                                  { %>
                                <tr>
                                    <td valign="top">Mileage</td>
                                    <td valign="top" class="text-bold"><%= modelPage.ModelVersionSpecs.FuelEfficiencyOverall%> kmpl</td>
                                </tr>
                                <%} %>
                                   <%if (colorCount>0){ %> 
                                <tr>
                                    <td valign="top">Colours</td>
                                    <td valign="top" class="text-bold">
                                        <ul class="model-color-list">
                                            <%foreach(var colorName in  modelPage.ModelColors){ %>
                                            <li class="leftfloat"><%=colorName.ColorName%></li>
                                        <%} %>
                                             </ul>
                                    </td>
                                </tr>
                                <%} %>
                            </tbody>
                        </table>
                    </div>
                    <%} %>
                    <h3><%= modelPage.ModelDetails.ModelName %> preview</h3>
                    <p class="font14 text-light-grey line-height17 inline">
                        <span class="model-preview-main-content">
                            <%= modelPage.ModelDesc.SmallDescription %>
                        </span>                                
                        <span class="model-preview-more-content hide">
                            <%= modelPage.ModelDesc.FullDescription %>
                        </span>
                        <a href="javascript:void(0)" class="font14 read-more-model-preview">Read more</a>
                    </p>
                    <%if(!modelPage.ModelDetails.Futuristic && bikeRankObj!=null && bikeRankObj.Rank > 0){ %>
                    <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Top 10 <%=styleName%> in India" class="model-rank-slug margin-top20 margin-bottom10">
                        <div class="inline-block icon-red-bg">
                            <span class="bwmsprite rank-graph"></span>
                        </div>
                        <div class="rank-slug-label inline-block text-bold text-default">
                            <p class="font14"><%=bikeRankObj.Rank>1?rankText:"" %> Most Popular <%=bikeType %></p>
                            <p class="font11">Check out the complete list.</p>
                        </div>
                        <span class="trend-arrow"></span>
                        <span class="bwmsprite right-arrow"></span>
                    </a>
                    <%} %>
                    <% if (modelPage.ModelVersionSpecs != null)
                       { %>
                    <h3 class="margin-top15">Specification summary</h3>
                    <div class="text-center">
                        <div class="summary-overview-box">
                            <div class="odd btmAftBorder">
                                <span class="inline-block offers-sprite specs-capacity-icon margin-right10" title="<%=bikeName %> Engine Capacity"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "hide":"" %>'>cc</span>
                                    </p>
                                    <p class="font16 text-light-grey">Capacity</p>
                                </div>
                            </div>
                            <div class="even btmAftBorder">
                                <span class="inline-block offers-sprite specs-mileage-icon margin-right10" title="<%=bikeName %> Mileage"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall).Equals("--") ? "hide":"" %>'>kmpl</span>
                                    </p>
                                    <p class="font16 text-light-grey">Mileage</p>
                                </div>
                            </div>
                            <div class="odd">
                                <span class="inline-block offers-sprite specs-maxpower-icon margin-right10" title="<%=bikeName %> Max Power"></span>
                                <div class="inline-block">
                                    <p class="font18 text-bold margin-bottom5">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %>
                                        <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "hide":"" %>'>bhp</span>
                                    </p>
                                    <p class="font16 text-light-grey">Max power</p>
                                </div>
                            </div>
                            <div class="even">
                                <span class="inline-block offers-sprite specs-weight-icon margin-right10" title="<%=bikeName %> Kerb Weight"></span>
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
              <%} %>

                <div id="modelPricesContent" class="bw-model-tabs-data">
                    <% if (modelPage !=null && modelPage.ModelVersions != null && modelPage.ModelVersions.Count() > 0)
                       { %>
                    <h2 class="padding-top15 padding-right20 padding-left20"><%= bikeName %> Price List</h2>
                    <!-- varient code starts here -->
                    <h3 class="padding-right20 padding-left20">Price by versions</h3>

                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="padding-right20 padding-left20 margin-bottom5">
                        <thead>
                            <tr>
                                <th align="left" width="65%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom"><%= modelPage.ModelDetails.ModelName %> Version</th>
                                <th align="left" width="35%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptVarients" runat="server" OnItemDataBound="rptVarients_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
							            <td width="65%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-bottom" valign="top"><%# DataBinder.Eval(Container.DataItem, "VersionName") %></td>
							            <td width="35%" class="padding-bottom10 padding-top10 divider-bottom" valign="top">
                                            <span class="bwmsprite inr-dark-md-icon"></span>
                                            <span id="<%# "priced_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>" class="font16 text-bold">
                                                <asp:Label Text='<%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %>' ID="txtComment" runat="server"></asp:Label>
                                            </span>
							            </td>
						            </tr>
                                    <asp:HiddenField ID="hdnVariant" runat="server" Value='<%#Eval("VersionId") %>' />                           
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
						        <td colspan="2" class="padding-top5 font12 text-x-light">Above mentioned prices are <%=priceText %>, <%=location %></td>
                            
					        </tr>
                        </tbody>
                    </table>
                    <!-- varient code ends here -->
                    <% } %>
                   <BW:TopCityPrice ID="ctrlTopCityPrices" runat="server" />
                   <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                </div>

                <!-- colours code starts here -->
                <%if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                  { %>
                <div id="modelColoursContent" class="bw-model-tabs-data font14">
                    <h2 class="padding-top15 padding-right20 padding-left20"><%=bikeName %> Colours</h2>
                    <ul id="modelColorsList" class="padding-top5 padding-right20 padding-left20">
                        <% foreach (var modelColor in modelPage.ModelColors)
                           { %>
                        <li>
                            <%  if (modelColor.ColorImageId > 0 && modelPage.ModelDetails != null && modelPage.ModelDetails.MakeBase != null)
                                { %>
                            <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/images/?modelpage=true&colorImageId=<%=modelColor.ColorImageId %>#modelGallery">
                                <%} %>
                                <div class="color-box <%= (((IList)modelColor.HexCodes).Count == 1 )?"color-count-one": (((IList)modelColor.HexCodes).Count >= 3 )?"color-count-three":"color-count-two" %> inline-block">
                                    <% if (modelColor.HexCodes != null && modelColor.HexCodes.Count() > 0)
                                       {
                                           foreach (var HexCode in modelColor.HexCodes)
                                           { %>
                                    <span <%= String.Format("style='background-color: #{0}'",Convert.ToString(HexCode)) %>></span>
                                    <%}
                                   } %>
                                </div>
                                <p class="font16 inline-block"><%= Convert.ToString(modelColor.ColorName) %></p>
                                <%  if (modelColor.ColorImageId > 0)
                                    { %>  
                            </a>
                            <%} %> 
                        </li>
                        <%} %>
                    </ul>
                </div>
                <div class="margin-right20 margin-left20 padding-top15 border-solid-bottom"></div>
                <%} %>
                <!-- colours code ends here -->

                <% if(modelPage.ModelVersionSpecs != null){ %>
                <div id="modelSpecsFeaturesContent" class="bw-model-tabs-data font14">
                    <h2 class="padding-top15 padding-right20 padding-left20"><%=modelPage.ModelDetails.ModelName%> Specifications & Features</h2>
                    <h3 class="padding-right20 padding-left20 model-specs-header">Specifications</h3>

                    <ul id="model-specs-list">
                        <li>
                            <div class="model-accordion-tab active">
                                <span class="offers-sprite engine-sm-icon margin-right10"></span>
                                <span>Engine & transmission</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Displacement</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement,"cc") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Cylinders</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Cylinders) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Max Power</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower, "bhp", modelPage.ModelVersionSpecs.MaxPowerRPM, "rpm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Maximum Torque</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaximumTorque, "Nm", modelPage.ModelVersionSpecs.MaximumTorqueRPM, "rpm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Bore</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Bore, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Stroke</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Stroke, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Valves Per Cylinder</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ValvesPerCylinder) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Fuel Delivery System</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelDeliverySystem) %></div>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <div class="model-accordion-tab brakes-accordion-tab">
                                <span class="offers-sprite brakes-sm-icon margin-right10"></span>
                                <span>Brakes, wheels & suspension</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Brake Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.BrakeType) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Front Disc</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Front Disc/Drum Size</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FrontDisc_DrumSize, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Rear Disc</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Rear Disc/Drum Size</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.RearDisc_DrumSize, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Calliper Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.CalliperType) %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Wheel Size</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.WheelSize, "inches") %></div>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <div class="model-accordion-tab">
                                <span class="offers-sprite dimension-sm-icon margin-right10"></span>
                                <span>Dimensions & chassis</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Kerb Weight</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight, "kg") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Overall Length</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallLength, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Overall Width</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallWidth, "mm") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Overall Height</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.OverallHeight, "mm") %></div>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <div class="model-accordion-tab">
                                <span class="offers-sprite fuel-sm-icon margin-right10"></span>
                                <span>Fuel efficiency & performance</span>
                                <span class="bwmsprite fa-angle-down"></span>
                            </div>
                            <ul class="specs-features-list">
                                <li>
                                    <div class="specs-features-label">Fuel Tank Capacity</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelTankCapacity, "litres") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Reserve Fuel Capacity</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ReserveFuelCapacity, "litres") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Fuel Efficiency Overall</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall, "kmpl") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Fuel Efficiency Range</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyRange, "km") %></div>
                                </li>
                                <li>
                                    <div class="specs-features-label">Top Speed</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TopSpeed, "kmph") %></div>
                                </li>
                            </ul>
                        </li>
                    </ul>

                    <h3 id="model-features-heading" class="margin-top20 padding-right20 padding-left20">Features</h3>

                    <ul class="specs-features-list model-features-list" id="model-main-features-list">
                        <li>
                            <div class="specs-features-label">Speedometer</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Speedometer) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Fuel Guage</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelGauge) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Tachometer Type</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Digital Fuel Guage</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.DigitalFuelGauge) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Tripmeter</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tripmeter) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Electric Start</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ElectricStart) %></div>
                        </li>
                    </ul>
                    <ul id="model-more-features-list" class="specs-features-list model-features-list">
                        <li>
                            <div class="specs-features-label">Tachometer</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Tachometer) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Shift Light</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.ShiftLight) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">No. of Tripmeters</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.NoOfTripmeters) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Tripmeter Type</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TripmeterType) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Low Fuel Indicator</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowFuelIndicator) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Low Oil Indicator</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowOilIndicator) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Low Battery Indicator</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.LowBatteryIndicator) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Pillion Seat</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionSeat) %></div>
                        </li>
                        <li>
                            <div class="specs-features-label">Pillion Footrest</div>
                            <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.PillionFootrest) %></div>
                        </li>
                    </ul>
                      <div class="padding-top10 padding-right20 padding-left20">		
                         <a href="javascript:void(0)"  class="view-features-link bw-ga" c="Model_Page" a="View_all_features_link_cliked" v="myBikeName" title="<%=bikeName %> Features" rel="nofollow">View all features</a>
                    </div>
                    <div class="margin-right20 margin-left20 border-solid-bottom padding-bottom20"></div>
                </div>
                <% } %>
                
                <%if (Ad_300x250)
                   { %>
                <section>
                    <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
                </section>
                <% } %>

                <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 || ctrlNews.FetchedRecordsCount > 0)
                    { %>   
                <div id="modelReviewsContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top20 padding-bottom20 font14">
                    <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                       { %>
                    <h2><%=bikeName %> Reviews</h2>
                    <% } %>

                    <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                       { %>
                    <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                    <div class="padding-top15 border-solid-bottom"></div>
                    <% } %>

                    <%if (ctrlUserReviews.FetchedRecordsCount > 0)
                       { %>
                    <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                    <div class="padding-top15 border-solid-bottom"></div>
                    <% } %>

                    <%if (ctrlNews.FetchedRecordsCount > 0)
                       { %>
                    <div class="padding-top15">
                        <BW:News runat="server" ID="ctrlNews" />
                    </div>
                    <div class="padding-top15 border-solid-bottom"></div>
                    <% } %>                        
                </div>
                <% } %>

                <%if (ctrlVideos.FetchedRecordsCount > 0)
                    { %>
                <div id="modelVideosContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 border-solid-bottom font14">
                    <h2><%= bikeModelName %> Videos</h2>
                    <BW:Videos runat="server" ID="ctrlVideos" />
                </div>
                <% } %>
              
                <div id="modelSimilarContent" class="bw-model-tabs-data padding-bottom20 font14">
                      <% if ((ctrlCompareBikes.fetchedCount > 0 || ctrlAlternativeBikes.FetchedRecordsCount > 0) && !isDiscontinued)
                   { %>
                    <h2 class="padding-left20 padding-right20 margin-bottom20 padding-top15">Bikes Similar to <%=modelPage.ModelDetails.ModelName%></h2>
                    <% if (ctrlCompareBikes.fetchedCount > 0){ %>
                    <h3 class="padding-left20 padding-right20 margin-bottom20">Most compared alternatives</h3>
                    <BW:SimilarBikesCompare runat="server" ID="ctrlCompareBikes" />
                    <% } %>
                    
                    <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                    { %>
                    <BW:AlternateBikes ID="ctrlAlternativeBikes" runat="server" />
                   <% } %>
                    <% } %>
                        <%if(!modelPage.ModelDetails.Futuristic && bikeRankObj!=null){ %>
                    <%if(bikeRankObj.Rank>0){ %>
                          <div class="margin-left20 margin-right20 margin-top15">
                        <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName %> in India" class="model-rank-slug">
                            <div class="inline-block">
                                <span class="item-rank">#<%=bikeRankObj.Rank%></span>
                            </div>
                            <p class="rank-slug-label inline-block text-default font14"><%=bikeModelName%> is the <%=bikeRankObj.Rank>1?rankText:"" %> most popular <%=bikeType.ToLower() %>. Check out other <%=styleName.ToLower() %> which made it to Top 10 list.</p>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </div>
                    <%} %>
                    <%else{ %>
                    <div class="margin-left20 margin-right20 margin-top15">
                        <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName %> in India" class="model-rank-slug">
                            <div class="inline-block icon-red-bg">
                                <span class="bwmsprite rank-graph"></span>
                            </div>
                            <p class="rank-slug-label inline-block text-default font14">Not sure what to buy?<br />List of Top 10 <%=styleName.ToLower() %><br/> can come in handy.</p>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </div>
                    <%} %>
                                 
                    <%} %>
                </div>
                 <%if(!modelPage.ModelDetails.Futuristic){%>
                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                <%} %>

                <% if ((!isDiscontinued && !modelPage.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)))
                   { %>
                <div id="dealerAndServiceContent" class="bw-model-tabs-data">
                    <% if (ctrlDealerCard.showWidget) { %>
                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                    <%} %>

                    <% if (ctrlServiceCenterCard.showWidget&& cityId>0) { %>
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />    
                    <% }  %>

                    <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                </div>
                <% }  %>
                  
                <% if (ctrlRecentUsedBikes.fetchedCount > 0) { %> 
                <BW:MostRecentUsedBikes runat="server" ID="ctrlRecentUsedBikes" />
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

        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                <div id="termsPopUpCloseBtn" class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <h3>Terms and Conditions</h3>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="https://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class="hide">
            </div>
        </div>
        <!-- Terms and condition Popup end -->
         <% if(viewModel != null){ %>
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
                                <span class="dealer-offer-label"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %>
                                    <span class="tnc font9 <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsOfferTerms"))? string.Empty: "hide" %>" id="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferId")) %>">View terms</span>
                                </span>
                                
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
            </ul>
            <div class="text-center">
                <a  data-leadsourceid="31"  data-item-id="<%= dealerId %>" data-item-name="<%= (viewModel!=null) ? viewModel.Organization : string.Empty %>"  data-item-area="<%= (viewModel!=null) ? viewModel.AreaName : string.Empty %> " href="javascript:void(0);" class="btn btn-orange text-bold leadcapturebtn"><%=viewModel.LeadBtnTextLarge %></a>
            </div>
        </div>
       
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
							        <span class="grid-3 omega text-light-grey text-right"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Distance")) %> kms</span>
							        <div class="clear"></div>
                                    <span class="bwmsprite dealership-loc-icon vertical-top"></span>
							        <span class="font12 text-light-grey"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Area")) %></span>
							        <div class="margin-top15">
                                        <div class="grid-4 alpha omega">
                                            <p class="font12 text-light-grey margin-bottom5">On-road price</p>
                                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-default text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "SelectedVersionPrice").ToString()) %></span>
                                        </div>
                                        <div class="border-solid-left grid-8 padding-top10 padding-left20 omega <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsPremiumDealer")) && (Convert.ToUInt16(DataBinder.Eval(Container.DataItem,"OfferCount"))> 0)?  string.Empty : "hide" %>">
                                            <span class="bwmsprite offers-sm-box-icon"></span>
                                            <span class="font14 text-default text-bold"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCount")) %></span>
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
        
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-model-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-model.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />
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

                $("#templist input").on("click", function () {
                    if ($(this).attr('data-option-value') == $('#hdnVariant').val()) {
                        return false;
                    }
                    $('.dropdown-select-wrapper #defaultVariant').text($(this).val());
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
                try {
                    var isSuccess = false;

                    var objData = {
                        "dealerId": dealerID,
                        "modelId": <%= modelId%>,
                        "versionId": versionId,
                        "cityId": cityId,
                        "areaId": areaId,
                        "clientIP": clientIP,
                        "pageUrl": pageUrl,
                        "sourceType": 2,
                        "pQLeadId": pqSourceId,
                        "deviceId": getCookie('BWC')
                    };

                    isSuccess = dleadvm.registerPQ(objData);

                    if (isSuccess) {
                        var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + dleadvm.pqId() + "&VersionId=" + versionId + "&DealerId=" + dealerID;
                        window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
                    }
                } catch (e) {
                    console.warn("Unable to create pricequote : " + e.message);
                }
            }


            $("#viewprimarydealer, #dealername").on("click", function () {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId + "&IsDealerAvailable=true";
                window.location.href = "/m/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            });

            $(".leadcapturebtn").click(function (e) {
                ele = $(this);
                try {                    
                    var leadOptions = {
                        "dealerid": ele.attr('data-item-id'),
                        "dealername": ele.attr('data-item-name'),
                        "dealerarea": ele.attr('data-item-area'),
                        "versionid": versionId,
                        "leadsourceid": ele.attr('data-leadsourceid'),
                        "pqsourceid": ele.attr('data-pqsourceid'),
                        "isleadpopup": ele.attr('data-isleadpopup'),
                        "mfgCampid": ele.attr('data-mfgcampid'),
                        "pqid": pqId,
                        "pageurl": pageUrl,
                        "clientip": clientIP,
                        "dealerHeading" : ele.attr('data-item-heading'), 
                        "dealerMessage" : ele.attr('data-item-message'), 
                        "dealerDescription" : ele.attr('data-item-description'), 
                        "pinCodeRequired":ele.attr("data-ispincodrequired"),
                        "gaobject": {
                            cat: ele.attr("c"),
                            act: ele.attr("a"),
                            lab: ele.attr("v")
                        }
                    };

                    dleadvm.setOptions(leadOptions);
                } catch (e) {
                    console.warn("Unable to get submit details : " + e.message);
                }

            });
            
        </script>
    </form>
</body>
</html>

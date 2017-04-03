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
    <meta itemprop="description" itemtype="https://schema.org/description" content="<%= modelPage.ModelDesc.SmallDescription %>" />
    <% } %>
    <meta itemprop="image" content="<%= modelImage %>" />
    <meta itemprop="manufacturer" name="manufacturer" content="<%= modelPage.ModelDetails.MakeBase.MakeName %>">
    <meta itemprop="model" content="<%= modelPage.ModelDetails.ModelName %>" />
    <meta itemprop="brand" content="<%= modelPage.ModelDetails.MakeBase.MakeName %>">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="contaniner clearfix">
                <span itemprop="name" class="hide"><%= bikeName %></span>

                <div class="padding-top10 padding-left20 padding-right20 bg-white">
                    <h1 class="padding-bottom5"><%= bikeName %></h1>

                    <% if (modelPage.ModelDetails.New || !modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                       { %>
                        <% if (Convert.ToDouble(modelPage.ModelDetails.ReviewRate) > 0)
                            { %>
                            <div class="inline-block margin-right10">
                                <span class="star-span star-one-icon"></span>
                                <span class="font14 text-bold"><%= modelPage.ModelDetails.ReviewRate %></span><span class="font12 text-light-grey">/5</span>
                            </div>
                            <div class="inline-block">
                                <span itemprop="aggregateRating" itemscope itemtype="http://schema.org/AggregateRating">
                                    <meta itemprop="ratingValue" content="<%=modelPage.ModelDetails.ReviewRate %>">
                                    <meta itemprop="worstRating" content="1">
                                    <meta itemprop="bestRating" content="5">
                                    <meta itemprop="itemreviewed" content="<%= bikeName %>" />
                                    <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> border-light-left leftfloat margin-right10 padding-left10 font12">
                                        <span itemprop="ratingCount"><%=modelPage.ModelDetails.ReviewCount%></span>&nbsp;<span>Reviews</span>
                                    </a>
                                </span>
                            </div>
                            <% }
                                else
                                { %>
                            <div class="rating-box inline-block">
                                <span class="star-span star-zero-icon"></span>
                                <span class="font12 text-light-grey">Not rated yet</span>
                            </div>
                        <% } %>
                    <% } %>
                </div>

                <div id="modelSpecsTabsContentWrapper">
                    <div id="modelOverallSpecsTopContent" class="bg-white position-rel">
                        <div id="overallSpecsTab" class="overall-specs-tabs-container">
                            <ul class="overall-specs-tabs-wrapper">
                                <li data-tabs="#overviewContent">Overview</li>
                                <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                                   { %>
                                <li data-tabs="#pricesContent">Price</li>
                                <%} %>
                                <%if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                                  { %>
                                <li data-tabs="#coloursContent">Colours</li>
                                <%} %>
                                <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                                   { %>
                                <li data-tabs="#similarContent">Similar Bikes</li>
                                <%} %>
                                <% if (modelPage.ModelVersionSpecs != null)
                                   { %>
                                <li data-tabs="#specsFeaturesContent">Specs & Features</li>
                                <% } %>
                                <% if (modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription))
                                   { %>
                                <li data-tabs="#aboutContent">About</li>
                                <% } %>
                                <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 && ctrlNews.FetchedRecordsCount > 0)
                                   { %>
                                <li data-tabs="#reviewsContent">Reviews & News</li>
                                <%} %>
                                <% if ((ctrlExpertReviews.FetchedRecordsCount > 0 && ctrlNews.FetchedRecordsCount == 0) || (ctrlUserReviews.FetchedRecordsCount > 0 && ctrlNews.FetchedRecordsCount == 0))
                                   { %>
                                <li data-tabs="#reviewsContent">Reviews</li>
                                <%} %>
                                <% if (ctrlExpertReviews.FetchedRecordsCount == 0 && ctrlUserReviews.FetchedRecordsCount == 0 && ctrlNews.FetchedRecordsCount > 0)
                                   { %>
                                <li data-tabs="#reviewsContent">News</li>
                                <% } %>
                                <% if (ctrlVideos.FetchedRecordsCount > 0)
                                   { %>
                                <li data-tabs="#videosContent">Videos</li>
                                <%} %>
                                <% if ((!isDiscontinued && !modelPage.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)))
                                   { %>
                                <li data-tabs="#dealerAndServiceContent">
                                    <% if (ctrlDealerCard.showWidget)
                                       {%>Dealers<%} %>
                                    <%if (ctrlDealerCard.showServiceCenter || (ctrlServiceCenterCard.showWidget && cityId > 0))
                                      { %>
                                    <% if (ctrlDealerCard.showWidget)
                                       {%> &<%}%> Service Centers
                                    <%} %>
                                </li>
                                <%} %>

                                <% if (ctrlRecentUsedBikes.fetchedCount > 0)
                                   {%>
                                <li data-tabs="#makeUsedBikeContent">Used Bikes</li>
                                <%} %>
                            </ul>
                        </div>
                    </div>

                    <!-- model overview start -->
                    <div id="overviewContent" class="bw-model-tabs-data card-bottom-margin content-details-wrapper">
                        <div class="content-box-shadow card-bottom-margin">
                            <div id="model-photos-swiper" class="swiper-container noSwiper model-photos-swiper">
                                <% if (modelPage.ModelDetails.Futuristic)
                                    { %>
                                <p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
                                <% } %>

                                <% if (!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                                    { %>
                                <p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
                                <% } %>

                                <div class="swiper-wrapper">
                                    <div class="swiper-slide">
                                        <img src="https://imgd.aeplcdn.com//642x361//bw/models/honda-activa-3g-standard-806.jpg?20151209184243" alt="Honda Activa 3G Model Image" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img src="https://imgd5.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Rear-three-quarter-50289.jpg?20151004170215&t=170215387&t=170215387" alt="Honda Activa 3G Model Image" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img src="https://imgd.aeplcdn.com//642x361//n/bw/models/colors/honda-activa-3g-geny-grey-metallic-1486354951566.jpg" alt="Honda Activa 3G Model Image" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img src="https://imgd.aeplcdn.com//642x361//n/bw/models/colors/honda-activa-3g-pearl-amazing-white-1486354960374.jpg" alt="Honda Activa 3G Model Image" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img src="https://imgd.aeplcdn.com//642x361//n/bw/models/colors/honda-activa-3g-black-1486354967726.jpg" alt="Honda Activa 3G Model Image" />
                                    </div>
                                </div>
                                <div class="bwmsprite swiper-button-next gallery-type-next"></div>
                                <div class="bwmsprite swiper-button-prev gallery-type-prev"></div>
                            </div>

                            <%--<div class="model-main-image">
                                <% if (!String.IsNullOrEmpty(modelPage.ModelDetails.OriginalImagePath))
                                    { %>
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/images/?modelpage=true#modelGallery" title="<%= bikeName + " images"%>" class="block">
                                    <% } %>
                                    <img src="<%=modelImage %>" alt="<%= bikeName %> images" title="<%= bikeName %> model image " class="cursor-pointer" />
                                    <% if (!String.IsNullOrEmpty(modelPage.ModelDetails.OriginalImagePath))
                                        { %>
                                </a>
                                <% } %>
                            </div>--%>

                            <!-- media links start -->
                            <ul class="model-media-list text-center padding-right20 padding-left20 padding-bottom15">
                                <% if (modelPage.ModelDetails.PhotosCount > 0)
                                    { %>
                                <li>
                                    <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/images/">
                                        <span class="bwmsprite photos-sm"></span>
                                        <span class="inline-block">
                                            <span class="font12"><%= modelPage.ModelDetails.PhotosCount %></span>&nbsp;<span class="font11">images</span>
                                        </span>
                                    </a>
                                </li>
                                <% } %>
                                <% if (modelPage.ModelDetails.VideosCount > 0)
                                    { %>
                                <li>
                                    <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/videos/">
                                        <span class="bwmsprite videos-sm"></span>
                                        <span class="inline-block">
                                            <span class="font12"><%= modelPage.ModelDetails.VideosCount %></span>&nbsp;<span class="font11">videos</span>
                                        </span>
                                    </a>
                                </li>
                                <% } %>
                                <% if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                                    { %>
                                <li>
                                    <a href="">
                                        <span class="bwmsprite colors-sm"></span>
                                        <span class="inline-block">
                                            <span class="font12"><%= modelPage.ModelColors.Count() %></span>&nbsp;<span class="font11">colours</span>
                                        </span>
                                    </a>
                                </li>
                                <% } %>
                            </ul>
                            <!-- media links end -->

                            <!-- Upcoming bike start -->
                            <% if (modelPage.ModelDetails.Futuristic)
                                { %>
                            <div class="elevated-shadow-top padding-15-20">
                                <% if (modelPage.UpcomingBike != null)
                                    {%>
                                <p class="font12 text-light-grey">Expected price</p>
                                <div itemscope itemtype="http://schema.org/Offer">
                                    <div itemprop="priceSpecification" itemscope itemtype="http://schema.org/PriceSpecification">
                                        <span class="font24 text-bold"><span itemprop="priceCurrency" content="INR"><span class="bwmsprite inr-lg-icon"></span></span>
                                            <span itemprop="price minPrice"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %></span>  - 
                                    <span itemprop="maxPrice"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                                    </div>
                                </div>
                                <div class="border-solid-bottom margin-top10 margin-bottom10"></div>
                                <p class="font12 text-light-grey">Expected launch date</p>
                                <p class="font18 margin-bottom5 text-bold"><%= modelPage.UpcomingBike.ExpectedLaunchDate %></p>
                                <% } %>
                                <p class="font14 text-grey"><%= bikeName %> is not launched in India yet. Information on this page is tentative.</p>
                            </div>
                            <% } %>
                            <!-- Upcoming bike ends -->

                            <div class="floating-btn clearfix">
                                <div class="grid-6 alpha omega">
                                    <a class="btn btn-orange btn-full-width" href="javascript:void(0);">Get offers</a>
                                </div>
                                <div class="grid-6 alpha omega">
                                    <a class="btn btn-green btn-full-width" href=""><span class="bwmsprite tel-white-icon margin-right10"></span>Call dealer</a>
                                </div>
                                <div class="clear"></div>
                            </div>

                            <!--floating buttons  -->
                            <%--<% if (!isDiscontinued)
                               {
                                   if (toShowOnRoadPriceButton)
                                   {   %>
                            <div class="grid-12  float-button float-fixed clearfix padding-bottom10">

                                <a id="btnGetOnRoadPrice" href="javascript:void(0)" data-reload="true" data-persistent="true" data-modelid="<%=modelId %>" style="width: 100%" class="btn btn-orange getquotation">Check on-road price</a>
                                <% }
                           else
                           {   %>
                                <div class="grid-12 float-button float-fixed clearfix">

                                    <% if (modelPage.ModelDetails.New && viewModel != null && !isBikeWalePQ)
                                       {   
                                    %>
                                    <% if (viewModel != null && viewModel.IsPremiumDealer)
                                       { 
                                    %>
                                    <div class="grid-<%= String.IsNullOrEmpty(viewModel.MaskingNumber)? "12":"6" %> alpha omega padding-right5">
                                        <a class="btn btn-orange btn-full-width rightfloat leadcapturebtn" data-leadsourceid="19" data-item-id="<%= dealerId %>" data-item-name="<%= viewModel.Organization %>" data-item-area="<%= viewModel.AreaName %> " href="javascript:void(0);"><%= viewModel.LeadBtnTextSmall %></a>
                                    </div>
                                    <%  if (!string.IsNullOrEmpty(viewModel.MaskingNumber))
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
                            </div>--%>

                            <asp:HiddenField ID="hdnVariant" Value="0" runat="server" />
                            <div class="elevated-shadow-top">
                                <% if (!modelPage.ModelDetails.Futuristic)
                                    { %>
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
                                        <% if (!isDiscontinued)
                                            {%>
                                        <div class="font14 text-bold">
                                            <a href="javascript:void(0)" data-reload="true" data-persistent="true" data-modelid='<%= modelId %>' class="getquotation changeCity" rel="nofollow">
                                                <span class="selected-location-label inline-block text-default text-truncate"><%= location %></span>
                                                <span class="bwmsprite loc-change-blue-icon"></span>
                                            </a>
                                        </div>
                                        <% }
                                            else
                                            { %>
                                        <span class="text-bold font14 text-truncate block margin-top5"><%= location %></span>
                                        <% } %>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                                <% }
                                    if (isDiscontinued && !modelPage.ModelDetails.Futuristic)
                                    { %>
                                <div class="bike-price-container padding-left20 padding-top10">
                                    <span class="font14 text-grey"><%= bikeName %> is now discontinued in India.</span>
                                </div>
                                <% }
                                    if (!modelPage.ModelDetails.Futuristic)
                                    { %>
                                <div class="padding-10-20" itemprop="offers" itemscope itemtype="http://schema.org/Offer">
                                    <p class="font12 text-light-grey"><%=priceText %> price in <%= location%></p>
                                    <div>
                                        <span itemprop="priceCurrency" content="INR">
                                            <% if (price == 0)
                                                { %>
                                            <span class="font18 text-bold" itemprop="price" content="0">Price not available</span>
                                            <% }
                                                else
                                                { %>
                                            <span class="bwmsprite inr-lg-icon"></span>
                                        </span>
                                        <span class="font24 text-bold padding-right5" itemprop="price" content="<%= price %>"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                        <% } %>
                        <%if (isOnRoadPrice && price > 0 && !isDiscontinued)
                                            { %>
                                        <a href="/m/pricequote/dealerpricequote.aspx?MPQ=<%= detailedPriceLink %>" class="font14 text-bold viewBreakupText" rel="nofollow">View detailed price</a>
                                        <% } %>
                                    </div>
                                </div>
                                <% } %>
                                <%
                                    if (viewModel != null)
                                    { 
                                %>
                                <div id="model-dealer-card">
                                    <% if (viewModel.IsPremiumDealer)
                                        { %>
                                    <div class="dealer-details margin-bottom10">
                                        <div class="dealership-icon-wrapper inline-block margin-right5">
                                            <span class="offers-sprite dealership-icon"></span>
                                        </div>
                                        <div class="dealership-title inline-block">
                                            <h2 id="dealername" class="text-default"><%=viewModel.Organization %></h2>
                                            <p class="font14 text-light-grey"><%=(!viewModel.IsDSA ? "Authorized Dealer in " : "Multi-brand Dealer in ") %><%=viewModel.AreaName %></p>
                                        </div>
                                        <%if (viewModel.IsDSA)
                                            { %>
                                        <div class="bw-tooltip multi-brand-tooltip tooltip-bottom slideUp-tooltip">
                                            <p class="bw-tooltip-text position-rel font14">This dealer sells bikes of multiple brands.<br />
                                                Above price is not final and may vary at the dealership.</p>
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
                                            Pay <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice(bookingAmt.ToString()) %> to book online and
                                            <br />
                                            balance of <span class="bwmsprite inr-grey-xxxsm-icon"></span><%= Bikewale.Utility.Format.FormatPrice((price - bookingAmt).ToString()) %> at dealership
                                        </p>
                                    </div>
                                    <% }
                                        else
                                            { %>
                                    <div class="margin-bottom20">
                                        <div class="vertical-top">
                                            <a id="requestcallback" c="Model_Page" a="Request_Callback_Details_Clicked" v="bikeVersionLocation" data-leadsourceid="30" data-item-id="<%= dealerId %>" data-item-name="<%= viewModel.Organization %>" data-item-area="<%= viewModel.AreaName %>" href="javascript:void(0)" class="btn btn-white callback-btn btn-sm-0 bw-ga leadcapturebtn">Request callback</a>
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
                                        <a id="viewprimarydealer" c="Model_Page" a="View_Dealer_Details_Clicked" v="bikeVersionLocation" href="javascript:void(0)" class="btn btn-orange btn-sm-0 bw-ga">View dealer details</a>
                                    </div>
                                    <% } %>
                                </div>
                                <% } %>
                                <% if (viewModel != null && viewModel.SecondaryDealerCount > 0)
                                    { %>
                                <div class="padding-15-20 border-solid-top font16">
                                    <a href="javascript:void(0)" rel="nofollow" id="more-dealers-target">Prices from <%=viewModel.SecondaryDealerCount %> more partner dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
                                </div>
                                <% } %>
                            </div>

                        </div>

                        <% if (modelPage.ModelVersionSpecs != null)
                            { %>
                        <div class="content-box-shadow padding-top15 padding-right20 padding-bottom20 padding-left20">
                            <% if (!modelPage.ModelDetails.Futuristic && modelPage.ModelDetails.MinPrice > 0)
                                { %>
                            <h2><%= bikeName %> summary</h2>
                            <% } %>

                            <% if (modelPage.ModelVersionSpecs != null)
                                { %>
                            <ul class="summary-specs-list text-center padding-bottom10">
                                <li>
                                    <span class="offers-sprite engine-sm-icon"></span>
                                    <p class="font12 text-light-grey">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %>
                                                <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "hide":"" %>'>&nbsp;cc</span>
                                    </p>
                                </li>
                                <li>
                                    <span class="offers-sprite engine-sm-icon"></span>
                                    <p class="font12 text-light-grey">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %>
                                                <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower).Equals("--") ? "hide":"" %>'>&nbsp;bhp</span>
                                    </p>
                                </li>
                                <li>
                                    <span class="offers-sprite engine-sm-icon"></span>
                                    <p class="font12 text-light-grey">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %>
                                                <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall).Equals("--") ? "hide":"" %>'>&nbsp;kmpl</span>
                                    </p>
                                </li>
                                <li>
                                    <span class="offers-sprite engine-sm-icon"></span>
                                    <p class="font12 text-light-grey">
                                        <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight) %>
                                                <span class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight).Equals("--") ? "hide":"" %>'>&nbsp;kg</span>
                                    </p>
                                </li>
                            </ul>
                            <div class="border-solid-bottom margin-bottom15"></div>
                            
                            <% } %>

                            <%if (!modelPage.ModelDetails.Futuristic && modelPage.ModelDetails.MinPrice > 0)
                                { %>
                            <div class="border-solid-bottom padding-bottom10 margin-bottom15">
                                <table id="model-key-highlights" cellspacing="0" cellpadding="0" width="100%" border="0" class="font14 text-left">
                                    <thead>
                                        <tr>
                                            <th colspan="2" class="text-bold padding-bottom5"><%= modelPage.ModelDetails.ModelName %> key highlights</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% if (modelPage.ModelDetails.MinPrice > 0)
                                            {%>
                                        <tr>
                                            <td valign="top" width="36%">Price</td>
                                            <td valign="top" width="64%">
                                                <span class="bwmsprite inr-dark-grey-xsm-icon"></span><span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPage.ModelDetails.MinPrice)) %></span><br />
                                                <span class="font12 text-light-grey">Ex-showroom <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span>
                                            </td>
                                        </tr>
                                        <%} %>
                                        <% if (modelPage != null && modelPage.ModelVersionSpecs != null && modelPage.ModelVersionSpecs.TopSpeed > 0)
                                            {%>
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
                                        <%if (colorCount > 0)
                                            { %>
                                        <tr>
                                            <td valign="top">Colours</td>
                                            <td valign="top" class="text-bold">
                                                <ul class="model-color-list">
                                                    <%foreach (var colorName in modelPage.ModelColors)
                                                        { %>
                                                    <li class="leftfloat"><%=colorName.ColorName%></li>
                                                    <%} %>
                                                </ul>
                                            </td>
                                        </tr>
                                        <%} %>
                                    </tbody>
                                </table>
                            </div>

                            <p class="font14 text-light-grey line-height16"><%=summaryDescription %></p>
                            <% } %>

                            <%if (!modelPage.ModelDetails.Futuristic && bikeRankObj != null && bikeRankObj.Rank > 0)
                                { %>
                            <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Top 10 <%=styleName%> in India" class="model-rank-slug margin-top10">
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

                        </div>
                        <% } %>

                    </div>
                    <!-- model overview end -->

                    <% if (pqOnRoad != null && pqOnRoad.BPQOutput != null && viewModel == null && isOnRoadPrice && !string.IsNullOrEmpty(pqOnRoad.BPQOutput.ManufacturerAd))
                       { %>
                    <%=String.Format(pqOnRoad.BPQOutput.ManufacturerAd) %>
                    <%} %>

                    <!-- model price list start -->
                    <div id="pricesContent" class="bw-model-tabs-data content-box-shadow card-bottom-margin padding-top15 content-details-wrapper">
                        <% if (modelPage != null && modelPage.ModelVersions != null && modelPage.ModelVersions.Count() > 0)
                           { %>
                        <h2 class="padding-right20 padding-left20"><%= bikeName %> Price List</h2>
                        <!-- varient code starts here -->
                        <h3 class="padding-right20 padding-left20">Price by versions</h3>

                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="padding-right20 padding-left20 margin-bottom20">
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
                        <table id="more-cities-list" width="100%" border="0" cellspacing="0" cellpadding="0" class="padding-right20 padding-left20 font14 hide">
                            <tbody>
                                <tr>
                                    <td align="left" width="60%" class="city-name padding-bottom20">
                                        <a title="Bajaj Pulsar RS200 Price in Bangalore" href="">Bangalore</a>
                                    </td>
                                    <td align="right" width="40%" class="city-price padding-bottom20">
                                        <span class="bwmsprite inr-dark-grey-xsm-icon"></span>1,42,987
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="60%" class="city-name padding-bottom20">
                                        <a title="Bajaj Pulsar RS200 Price in Mumbai" href="">Mumbai</a>
                                    </td>
                                    <td align="right" width="40%" class="city-price padding-bottom20">
                                        <span class="bwmsprite inr-dark-grey-xsm-icon"></span>93,750
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="60%" class="city-name padding-bottom20">
                                        <a title="Bajaj Pulsar RS200 Price in Pune" href="">Pune</a>
                                    </td>
                                    <td align="right" width="40%" class="city-price padding-bottom20">
                                        <span class="bwmsprite inr-dark-grey-xsm-icon"></span>93,750
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="padding-top10 padding-right20 padding-left20 padding-bottom5">
                            <a href="javascript:void(0)" class="view-cities-link position-rel block font14" title="" rel="nofollow">View more cities</a>
                        </div>
                    </div>
                    <!-- model price list end -->

                    <%if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                      { %>
                    <!-- model color start -->
                    <div id="coloursContent" class="bw-model-tabs-data content-box-shadow padding-15-20-20 card-bottom-margin content-details-wrapper">
                        <h2><%=bikeName %> Colours</h2>
                        <ul id="modelColorsList" class="padding-top5">
                            <% foreach (var modelColor in modelPage.ModelColors)
                               { %>
                            <li>
                                <%  if (modelColor.ColorImageId > 0 && modelPage.ModelDetails != null && modelPage.ModelDetails.MakeBase != null)
                                    { %>
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/images/?modelpage=true&colorImageId=<%=modelColor.ColorImageId %>#modelGallery" class="block">
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
                                    <p class="text-light-grey font14 inline-block"><%= Convert.ToString(modelColor.ColorName) %></p>
                                    <%  if (modelColor.ColorImageId > 0)
                                        { %>  
                                </a>
                                <%} %> 
                            </li>
                            <%} %>
                        </ul>
                    </div>
                    <!-- model color end -->
                    <%} %>

                    <!-- model similar start -->
                    <div id="similarContent" class="bw-model-tabs-data padding-top15 padding-bottom15 content-box-shadow card-bottom-margin content-details-wrapper">
                        <h2 class="padding-left20 padding-right20">Bikes Similar to <%= bikeName %></h2>
                        <div class="swiper-container card-container swiper-type-similar">
                            <div class="swiper-wrapper">
                                <div class="swiper-slide">
                                    <div class="swiper-card">
                                        <a href="" title="Bajaj Pulsar RS200">
                                            <div class="swiper-image-preview">
                                                <img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//174x98//bw/models/bajaj-pulsar-rs200.jpg?20150710124439" alt="">
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-details-block">
                                                <p class="target-link font12 text-truncate margin-bottom5">Bajaj Pulsar RS200</p>
                                                <p class="text-truncate text-light-grey font11">Ex-showroom, Mumbai</p>
                                                <p class="text-default">
                                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16">90,000</span>
                                                </p>
                                            </div>
                                        </a>
                                        <div class="swiper-btn-block">
                                            <a href="javascript:void(0)" class="btn btn-card btn-full-width btn-white">Check on-road price</a>
                                        </div>
                                        <a class="compare-with-target text-truncate">
                                            <span class="bwmsprite compare-sm"></span>Compare with Honda Unicorn<span class="bwmsprite right-arrow"></span>
                                        </a>
                                    </div>
                                </div>

                                <div class="swiper-slide">
                                    <div class="swiper-card">
                                        <a href="" title="Bajaj Pulsar RS200">
                                            <div class="swiper-image-preview">
                                                <img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//174x98//bw/models/bajaj-pulsar-rs200.jpg?20150710124439" alt="">
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-details-block">
                                                <p class="target-link font12 text-truncate margin-bottom5">Bajaj Pulsar RS200</p>
                                                <p class="text-truncate text-light-grey font11">Ex-showroom, Mumbai</p>
                                                <p class="text-default">
                                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16">90,000</span>
                                                </p>
                                            </div>
                                        </a>
                                        <div class="swiper-btn-block">
                                            <a href="javascript:void(0)" class="btn btn-card btn-full-width btn-white">Check on-road price</a>
                                        </div>
                                        <a class="compare-with-target text-truncate">
                                            <span class="bwmsprite compare-sm"></span>Compare with Unicorn<span class="bwmsprite right-arrow"></span>
                                        </a>
                                    </div>
                                </div>

                                <div class="swiper-slide">
                                    <div class="swiper-card">
                                        <a href="" title="Bajaj Pulsar RS200">
                                            <div class="swiper-image-preview">
                                                <img class="swiper-lazy" data-src="https://imgd.aeplcdn.com//174x98//bw/models/bajaj-pulsar-rs200.jpg?20150710124439" alt="">
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-details-block">
                                                <p class="target-link font12 text-truncate margin-bottom5">Bajaj Pulsar RS200</p>
                                                <p class="text-truncate text-light-grey font11">Ex-showroom, Mumbai</p>
                                                <p class="text-default">
                                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16">90,000</span>
                                                </p>
                                            </div>
                                        </a>
                                        <div class="swiper-btn-block">
                                            <a href="javascript:void(0)" class="btn btn-card btn-full-width btn-white">Check on-road price</a>
                                        </div>
                                        <a class="compare-with-target text-truncate">
                                            <span class="bwmsprite compare-sm"></span>Compare with Unicorn<span class="bwmsprite right-arrow"></span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- model similar end -->

                    <% if (modelPage.ModelVersionSpecs != null)
                       { %>
                    <!-- model specs and features start -->
                    <div id="specsFeaturesContent" class="bw-model-tabs-data padding-top15 padding-bottom20 content-box-shadow card-bottom-margin content-details-wrapper font14">
                        <h2 class="padding-right20 padding-left20"><%=modelPage.ModelDetails.ModelName%> Specifications & Features</h2>
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
                            <a href="javascript:void(0)" class="view-features-link bw-ga" c="Model_Page" a="View_all_features_link_cliked" v="myBikeName" title="<%=bikeName %> Features" rel="nofollow">View all features</a>
                        </div>
                    </div>
                    <!-- model specs and features end -->
                    <% } %>

                    <%if (modelPage.ModelDesc != null && !string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription))
                    { %>
                    <!-- model about start -->
                    <div id="aboutContent" class="bw-model-tabs-data padding-15-20-20 content-box-shadow card-bottom-margin content-details-wrapper">
                        <h2>About <%= modelPage.ModelDetails.ModelName %></h2>
                        <% if(!modelPage.ModelDetails.Futuristic)
                           { %>
                        <p class="font14 text-light-grey line-height-16 inline">
                            <span class="model-preview-main-content">
                                <%= modelPage.ModelDesc.SmallDescription %>
                            </span>
                            <span class="model-preview-more-content hide">
                                <%= modelPage.ModelDesc.FullDescription %>
                            </span>
                            <a href="javascript:void(0)" class="read-more-model-preview">Read more</a>
                        </p>
                        <% } else { %>
                        <p class="font14 text-light-grey line-height-16 inline">
                            <%= modelPage.ModelDesc.FullDescription %>
                        </p>
                        <% } %>
                    </div>
                    <!-- model about end -->
                    <% } %>

                    <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 || ctrlNews.FetchedRecordsCount > 0)
                       { %>
                    <!-- model reviews start -->
                    <div id="reviewsContent" class="bw-model-tabs-data padding-15-20-20 content-box-shadow card-bottom-margin content-details-wrapper font14">
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                           { %>
                        <h2><%=bikeName %> Reviews</h2>
                        <% } %>

                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                           { %>
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <% } %>

                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 && (ctrlUserReviews.FetchedRecordsCount > 0 || ctrlNews.FetchedRecordsCount > 0))
                           { %>
                            <div class="padding-top20 margin-bottom15 border-solid-bottom"></div>
                        <% } %>

                        <%if (ctrlUserReviews.FetchedRecordsCount > 0)
                          { %>
                        <div class="user-review-subContent">
                            <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                        </div>
                        <% } %>

                        <% if (ctrlUserReviews.FetchedRecordsCount > 0 && ctrlNews.FetchedRecordsCount > 0)
                           { %>
                            <div class="padding-top20 margin-bottom15 border-solid-bottom"></div>
                        <% } %>

                        <%if (ctrlNews.FetchedRecordsCount > 0)
                          { %>
                            <BW:News runat="server" ID="ctrlNews" />
                        <% } %>

                        <!-- model reviews end -->
                    </div>
                    <% } %>

                    <%if (ctrlVideos.FetchedRecordsCount > 0)
                      { %>
                    <!-- model videos start -->
                    <div id="videosContent" class="bw-model-tabs-data padding-15-20-20 content-box-shadow card-bottom-margin content-details-wrapper font14">
                        <h2><%= bikeModelName %> Videos</h2>
                        <BW:Videos runat="server" ID="ctrlVideos" />
                    </div>
                    <!-- model videos end -->
                    <% } %>

                    <% if ((!isDiscontinued && !modelPage.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)))
                       { %>
                    <!-- model dealers and service center start -->
                    <div id="dealerAndServiceContent" class="bw-model-tabs-data content-box-shadow card-bottom-margin content-details-wrapper">
                        <% if (ctrlDealerCard.showWidget)
                           { %>
                        <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                        <% } %>

                        <% if (ctrlDealerCard.showWidget && ctrlServiceCenterCard.showWidget && cityId > 0)
                           { %>
                        <div class="margin-right10 margin-left10 margin-bottom15 border-solid-bottom"></div>
                        <% } %>

                        <% if (ctrlServiceCenterCard.showWidget && cityId > 0)
                           { %>
                        <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                        <% }  %>
                    </div>
                    <!-- model dealers and service center end -->
                    <% }  %>

                    <% if (ctrlRecentUsedBikes.fetchedCount > 0)
                       { %>
                    <!-- model used bikes start -->
                    <div class="content-box-shadow card-bottom-margin content-details-wrapper">
                        <BW:MostRecentusedBikes runat="server" ID="ctrlRecentUsedBikes" />
                    </div>
                    <!-- model used bikes end -->
                    <%} %>
                    <div id="modelSpecsFooter"></div>
                </div>

            </div>
        </section>

        <section>
            <div class="container padding-15-20-20 content-box-shadow card-bottom-margin">
                <h2 class="text-bold margin-bottom10">Top 10 Mileage bikes in India</h2>
                <ul class="best-bike-list margin-bottom5">
                    <li class="list-item">
                        <a href="" class="list-item-target">
                            <div class="item-image-block inline-block">
                                <span class="item-rank-label">#1</span>
                                <img class="lazy" src="https://imgd.aeplcdn.com//110x61//bw/models/benelli-tornado-302.jpg?20152511122705" />
                            </div>
                            <div class="item-details-block inline-block">
                                <p class="font14 text-bold text-default text-truncate">Benelli Tornado 302</p>
                                <ul class="key-specs-list font12 text-xx-light margin-bottom5">
                                    <li>
                                        <span>199.5 cc</span>
                                    </li>
                                    <li>
                                        <span>35 kmpl</span>
                                    </li>
                                    <li>
                                        <span>24.5 bhp</span>
                                    </li>
                                </ul>
                                <p class="font11 text-grey text-truncate">Ex-showroom price, Mumbai</p>
                                <span class="bwmsprite inr-xsm-icon"></span>
                                <span class="font16 text-default text-bold">90,000</span>
                            </div>
                        </a>
                    </li>

                    <li class="list-item">
                        <a href="" class="list-item-target">
                            <div class="item-image-block inline-block">
                                <span class="item-rank-label">#2</span>
                                <img class="lazy" src="https://imgd.aeplcdn.com//110x61//bw/models/benelli-tornado-302.jpg?20152511122705" />
                            </div>
                            <div class="item-details-block inline-block">
                                <p class="font14 text-bold text-default text-truncate">Benelli Tornado 302</p>
                                <ul class="key-specs-list font12 text-xx-light margin-bottom5">
                                    <li>
                                        <span>199.5 cc</span>
                                    </li>
                                    <li>
                                        <span>35 kmpl</span>
                                    </li>
                                    <li>
                                        <span>24.5 bhp</span>
                                    </li>
                                </ul>
                                <p class="font11 text-grey text-truncate">Ex-showroom price, Mumbai</p>
                                <span class="bwmsprite inr-xsm-icon"></span>
                                <span class="font16 text-default text-bold">90,000</span>
                            </div>
                        </a>
                    </li>

                    <li class="list-item">
                        <a href="" class="list-item-target">
                            <div class="item-image-block inline-block">
                                <span class="item-rank-label">#3</span>
                                <img class="lazy" src="https://imgd.aeplcdn.com//110x61//bw/models/benelli-tornado-302.jpg?20152511122705" />
                            </div>
                            <div class="item-details-block inline-block">
                                <p class="font14 text-bold text-default text-truncate">Benelli Tornado 302</p>
                                <ul class="key-specs-list font12 text-xx-light margin-bottom5">
                                    <li>
                                        <span>199.5 cc</span>
                                    </li>
                                    <li>
                                        <span>35 kmpl</span>
                                    </li>
                                    <li>
                                        <span>24.5 bhp</span>
                                    </li>
                                </ul>
                                <p class="font11 text-grey text-truncate">Ex-showroom price, Mumbai</p>
                                <span class="bwmsprite inr-xsm-icon"></span>
                                <span class="font16 text-default text-bold">90,000</span>
                            </div>
                        </a>
                    </li>
                </ul>
                <div class="view-all-btn-container">
                    <a href="" title="" class="btn view-all-target-btn">View complete list<span class="bwmsprite teal-right"></span></a>
                </div>
            </div>
        </section>
        
        
        <section>
            <div id="" class="container bg-white clearfix box-shadow margin-top10 margin-bottom20 content-details-wrapper">
                
                                
                <%if (Ad_300x250)
                  { %>
                <section>
                    <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
                </section>
                <% } %>
                
                               

                <div id="modelSimilarContent" class="bw-model-tabs-data padding-bottom20 font14">
                    <% if ((ctrlCompareBikes.fetchedCount > 0 || ctrlAlternativeBikes.FetchedRecordsCount > 0) && !isDiscontinued)
                       { %>
                    <h2 class="padding-left20 padding-right20 margin-bottom20 padding-top15">Bikes Similar to <%=modelPage.ModelDetails.ModelName%></h2>
                      <% if (ctrlCompareBikes.fetchedCount > 0)
                       { %>
                         <div class="carousel-heading-content">
                       <div class="swiper-heading-left-grid inline-block">   
                         <h3 >Most compared alternatives</h3>
                        </div>
                        <div class="swiper-heading-right-grid inline-block text-right">
                       <a href="/m/comparebikes/" title="View more comparisons" class="btn view-all-target-btn">View all</a>
                       </div>
                         <div class="clear"></div>
                     </div>       
        
                    <BW:SimilarBikesCompare runat="server" ID="ctrlCompareBikes" />
                    <% } %>

                    <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                       { %>
                    <BW:AlternateBikes ID="ctrlAlternativeBikes" runat="server" />
                    <% } %>
                    <% } %>
                    <%if (!modelPage.ModelDetails.Futuristic && bikeRankObj != null)
                      { %>
                    <%if (bikeRankObj.Rank > 0)
                      { %>
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
                    <%else
                      { %>
                    <div class="margin-left20 margin-right20 margin-top15">
                        <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName %> in India" class="model-rank-slug">
                            <div class="inline-block icon-red-bg">
                                <span class="bwmsprite rank-graph"></span>
                            </div>
                            <p class="rank-slug-label inline-block text-default font14">Not sure what to buy?<br />
                                List of Top 10 <%=styleName.ToLower() %><br />
                                can come in handy.</p>
                            <span class="bwmsprite right-arrow"></span>
                        </a>
                    </div>
                    <%} %>

                    <%} %>
                </div>
                <%if (!modelPage.ModelDetails.Futuristic)
                  {%>
                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                <%} %>

                <%--<% if ((!isDiscontinued && !modelPage.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)))
                   { %>
                <div id="dealerAndServiceContent" class="bw-model-tabs-data">
                    <% if (ctrlDealerCard.showWidget)
                       { %>
                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                    <%} %>

                    <% if (ctrlServiceCenterCard.showWidget && cityId > 0)
                       { %>
                    <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                    <% }  %>

                    <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                </div>
                <% }  %>--%>

                <%--<% if (ctrlRecentUsedBikes.fetchedCount > 0)
                   { %>
                <BW:MostRecentusedBikes runat="server" ID="ctrlRecentUsedBikes" />
                <%} %>--%>

                <%--<div id="modelSpecsFooter"></div>--%>
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
        <% if (viewModel != null)
           { %>
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
                <a data-leadsourceid="31" data-item-id="<%= dealerId %>" data-item-name="<%= (viewModel!=null) ? viewModel.Organization : string.Empty %>" data-item-area="<%= (viewModel!=null) ? viewModel.AreaName : string.Empty %> " href="javascript:void(0);" class="btn btn-orange text-bold leadcapturebtn"><%=viewModel.LeadBtnTextLarge %></a>
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

        <noscript id="asynced-css">
            <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%=staticFileVersion %>" />
            <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-model-btf.css?<%=staticFileVersion %>" />
            <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />
        </noscript>

        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/m/src/Plugins.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/m/src/common.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-model.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var leadSourceId,vmModelId = '<%= modelId%>',clientIP = '<%= clientIP%>',cityId = '<%= cityId%>',isUsed = '<%= !modelPage.ModelDetails.New%>';
            var pageUrl = "<%= canonical%>",myBikeName = "<%= this.bikeName%>",versionName = "<%= variantText %>",ga_pg_id = '2',getCityArea;

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
                        window.location.href = "/m/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
                    }
                } catch (e) {
                    console.warn("Unable to create pricequote : " + e.message);
                }
            }

            docReady(function(){
                $("#viewprimarydealer, #dealername").on("click", function () {
                    var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId + "&IsDealerAvailable=true";
                    window.location.href = "/m/pricequote/dealer/?MPQ=" + Base64.encode(rediurl);
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

                
                if (bikeVersionLocation == '') {
                    bikeVersionLocation = getBikeVersionLocation();
                }
                if (bikeVersion == '') {
                    bikeVersion = getBikeVersion();
                }
                getCityArea = GetGlobalCityArea();
            });

        </script>
    </form>
</body>
</html>

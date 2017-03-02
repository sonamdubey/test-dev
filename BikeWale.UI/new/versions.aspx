<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="versions.aspx.cs" Inherits="Bikewale.New.BikeModel" EnableViewState="false" Trace="false" %>

<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/News.ascx" TagName="LatestNews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewUserReviewsList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/controls/PriceInTopCities.ascx" TagPrefix="BW" TagName="TopCityPrice" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/controls/PopularModelCompare.ascx" TagName="PopularCompare" TagPrefix="BW" %>
<%@ Register Src="~/controls/UsedBikes.ascx" TagName="UsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<!doctype html>
<html>
<head>
    <%
        var modDetails = modelPageEntity.ModelDetails;
        title = String.Format("{0} Price, Reviews, Spec, Images, Mileage, Colors | Bikewale", bikeName);
        description = pgDescription;
        canonical = String.Format("https://www.bikewale.com/{0}-bikes/{1}/", modelPageEntity.ModelDetails.MakeBase.MaskingName, modelPageEntity.ModelDetails.MaskingName);
        AdId = "1442913773076";
        AdPath = "/1017752/Bikewale_NewBike_";
        TargetedModel = modDetails.ModelName;
        fbTitle = title;
        alternate = "https://www.bikewale.com/m/" + modDetails.MakeBase.MaskingName + "-bikes/" + modDetails.MaskingName + "/";
        isAd970x90Shown = true;
        TargetedCity = cityName;
        keywords = string.Format("{0},{0} Bike, bike, {0} Price, {0} Reviews, {0} Images, {0} Mileage", bikeName);
        ogImage = modelImage; 
        isAd970x90BTFShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/model-atf.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var versionId = '<%= variantId%>';
        var cityId = '<%= cityId%>';
        var bikeVersionLocation = '';
        var bikeVersion = '';
        var isBikeWalePq = "<%= isBikeWalePQ%>";
        var areaId = "<%= areaId %>";
        var isDealerPriceAvailable = "<%= pqOnRoad != null ? pqOnRoad.IsDealerPriceAvailable : false%>";
        var campaignId = "<%= campaignId%>";
        var manufacturerId = "<%= manufacturerId%>";
        var myBikeName = "<%= this.bikeName %>";
        var clientIP = "<%= clientIP%>";
        var pageUrl = "<%= canonical %>";
     </script>

</head>
<body class="bg-light-grey" itemscope itemtype="http://schema.org/Product">
    <form runat="server">
        <% if (modelPageEntity != null && modelPageEntity.ModelDesc != null)
           { %>
        <meta itemprop="description" itemtype="https://schema.org/description" content="<%=modelPageEntity.ModelDesc.SmallDescription %>" />
        <% } %>
        <meta itemprop="name" content="<%= bikeName %>" />
        <meta itemprop="image" content="<%= modelImage %>" />

        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= modelPageEntity.ModelDetails.MakeBase.MaskingName %>-bikes/" itemprop="url">
                                    <span itemprop="title"><%= modelPageEntity.ModelDetails.MakeBase.MakeName %> Bikes</span>
                                </a></li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                                <span><%= modelPageEntity.ModelDetails.MakeBase.MakeName %> <%= modelPageEntity.ModelDetails.ModelName %></span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container" id="modelDetailsContainer">
                <div class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1 class="inline-block margin-right15"><%= bikeName %></h1>
                            <% if (!modelPageEntity.ModelDetails.Futuristic || modelPageEntity.ModelDetails.New)
                               { %>
                                <div class="inline-block <%= modelPageEntity.ModelDetails.Futuristic ? "hide " : string.Empty %>">
                                    <div class="rating-review-content">
                                        <% if (Convert.ToDouble(modelPageEntity.ModelDetails.ReviewRate) > 0)
                                           { %>
                                            <div class="rating-box inline-block">
                                                <span class="star-one-icon margin-right5"></span>
                                                <span class="font16 text-bold"><%=modelPageEntity.ModelDetails.ReviewUIRating %></span><span class="padding-left2 font12 text-light-grey">/5</span>
                                            </div>
                                            <div class="review-box font14 inline-block">
                                                <span itemprop="aggregateRating" itemscope itemtype="http://schema.org/AggregateRating">
                                                    <meta itemprop="ratingValue" content="<%= modelPageEntity.ModelDetails.ReviewRate %>">
                                                    <meta itemprop="worstRating" content="1">
                                                    <meta itemprop="bestRating" content="5">
                                                    <meta itemprop="itemreviewed" content="<%= bikeName %>" />
                                                    <a href="<%= FormatShowReview(modelPageEntity.ModelDetails.MakeBase.MaskingName,modelPageEntity.ModelDetails.MaskingName) %>" class="review-count-target">
                                                    <span itemprop="ratingCount"><%= modelPageEntity.ModelDetails.ReviewCount %></span> Reviews</a>
                                                </span>
                                                <a href="<%= FormatWriteReviewLink() %>">Write a review</a>
                                            </div>
                                        <% }
                                           else
                                           { %>
                                            <div class="review-box font14 inline-block">
                                                <span class="no-rating text-light-grey">No reviews yet</span>
                                                <a href="<%= FormatWriteReviewLink() %>">Write a review</a>
                                            </div>
                                        <% } %>
                                    </div>
                                </div>
                            <% } %>
                        </div>
                        <div class="padding-top20 padding-right20 padding-bottom10 padding-left20">
                            <div class="grid-7 model-details-wrapper omega rightfloat padding-top5">
                                <%if (modelPageEntity.ModelVersionSpecs != null)
                                  { %>
                                <p class="font12 text-light-grey margin-bottom5">Key Specs</p>
                                <ul id="key-specs-list" class="font14 text-light-grey">
                                    <%if (modelPageEntity.ModelVersionSpecs.Displacement != 0)
                                      { %>
                                        <li>
                                            <span class="bwsprite capacity-sm"></span>
                                            <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement) %> cc</span>
                                        </li>
                                    <% } %>
                                    <%if (modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall != 0)
                                      { %>
                                        <li>
                                            <span class="bwsprite mileage-sm"></span>
                                            <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall) %> kmpl</span>
                                        </li>
                                    <% } %>
                                    <%if (modelPageEntity.ModelVersionSpecs.MaxPower != 0)
                                      { %>
                                        <li>
                                            <span class="bwsprite power-sm"></span>
                                            <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower) %> bhp</span>
                                        </li>
                                    <% } %>
                                    <%if (modelPageEntity.ModelVersionSpecs.KerbWeight != 0)
                                      { %>
                                        <li>
                                            <span class="bwsprite weight-sm"></span>
                                            <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight) %> kgs</span>
                                        </li>
                                    <% } %>
                                </ul>
                                <% } %>
                                
                                <% if (!modelPageEntity.ModelDetails.Futuristic)
                                   {
                                       if (modelPageEntity.ModelVersions != null && modelPageEntity.ModelVersions.Count > 1)
                                       { %>
                                    <div id="model-version-dropdown" class="padding-top25">
                                        <div class="select-box select-box-no-input done size-small">
                                            <p class="select-label">Version</p>
                                            <asp:DropDownList AutoPostBack="true" runat="server" ID="ddlVersion" CssClass="chosen-select" data-title="Version" />
                                            <asp:HiddenField ID="hdnVariant" Value="0" runat="server" />           
                                        </div>
                                    </div>
                                    <% } %>
                                    <% else
                                    { %>
                                    <p class="font12 text-light-grey">Version</p>
                                    <p id="singleversion" class="font14 text-bold margin-bottom15"><%= variantText %></p>
                                    <% }
                                   } %>
                                <!-- Variant div ends -->

                                <div id="scrollFloatingButton"></div>
                                <% if (!modelPageEntity.ModelDetails.Futuristic)
                                   { %>
                                <div id="modelPriceContainer">
                                    <% if (isDiscontinued)
                                       { %>
                                    <p class="font14 text-light-grey">Last known Ex-showroom price in<span class="city-area-name"><%= location %></span> </p>
                                    <% } %>
                                    <% else if (!isOnRoadPrice)
                                       {%>
                                    <p class="font14 text-light-grey">Ex-showroom price in<span><span class="city-area-name"><%= location %></span></span><a data-persistent="true" data-reload="true" data-modelid="<%=modelId %>" class="margin-left5 getquotation changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>
                                    <% } %>
                                    <% else
                                       {%>
                                    <p class="font14 text-light-grey">On-road price in<span><span class="city-area-name"><%= location %></span></span><a data-persistent="true" data-reload="true" data-modelid="<%=modelId %>" class="margin-left5 getquotation changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>
                                    <% } %>
                                    <%  if (price == 0)
                                        { %>
                                    <span class="font16">Price not available</span>
                                    <%  }
                                        else
                                        { %>
                                    <div class="leftfloat margin-right15 <%= (isBookingAvailable && isDealerAssitance) ? "model-price-book-now-wrapper" : string.Empty %> " itemprop="offers" itemscope itemtype="http://schema.org/Offer">
                                        <span itemprop="priceCurrency" content="INR">
                                            <span class="bwsprite inr-md-lg"></span>
                                        </span>
                                        <span id="new-bike-price" class="font22 text-bold" itemprop="price" content="<%=price %>"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                        <%if (isOnRoadPrice && !isDiscontinued)
                                          {%>
                                        <a id="viewBreakupText" href="/pricequote/dealerpricequote.aspx?MPQ=<%=detailedPriceLink %>" rel="nofollow" class="font14 text-bold viewBreakupText">View detailed price</a>
                                        <br>
                                        <% } %>
                                    </div>
                                    <%  } %>
                                    <%if (isBookingAvailable && isDealerAssitance)
                                      { %>
                                    <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-grey leftfloat margin-top20" id="bookNowBtn">Book now </a>
                                    <%}%>
                                    <div class="clear"></div>
                                    <% if (isDiscontinued)
                                       { %>
                                    <p class="default-showroom-text font14 text-light-grey margin-top5"><%= bikeName %> is now discontinued in India.</p>
                                    <% } %>
                                    <% 
                                       else
                                           if (toShowOnRoadPriceButton)
                                           { %>
                                    <a id="btnGetOnRoadPrice" href="javascript:void(0)" data-persistent="true" data-reload="true" data-modelid="<%=modelId %>" class="btn btn-orange margin-top10 getquotation">Check on-road price</a>
                                    <div class="clear"></div>

                                    <% } %>
                                </div>

                                <% if (viewModel != null && viewModel.IsPremiumDealer)
                                   { %>
                                <div class="margin-top15">
                                    <a href="javascript:void(0)" class="btn btn-orange margin-right15 get-offers-main-btn leftfloat leadcapturebtn bw-ga" data-leadsourceid="12" data-item-id="<%= dealerId %>" data-item-name="<%= viewModel.Organization %>" data-item-area="<%= viewModel.AreaName %>" c="Model_Page" a="Get_Offers_Clicked" v="bikeVersionLocation"><%= viewModel.LeadBtnTextLarge %></a>
                                    <div class="grid-6 alpha omega">
                                        <span class="font12 text-light-grey">Powered by</span>
                                        <p class="font14 text-truncate" title="<%= viewModel.Organization %>, <%=viewModel.AreaName %>"><%= viewModel.Organization %>, <%=viewModel.AreaName %></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <%  }
                                   } %>
                                <% if (viewModel != null && viewModel.DealerCampaignV2.PrimaryDealer.DealerDetails != null && !viewModel.IsPremiumDealer)
                                   { %>
                                <div class="border-solid-top margin-top15 margin-bottom20 padding-top20">
                                    <div class="inline-block margin-right10">
                                        <span class="model-sprite partner-dealer"></span>
                                    </div>
                                    <div class="inline-block margin-right25">
                                        <p class="font14 text-bold text-black margin-right10">One partner dealer near you</p>
                                        <% if (viewModel.DealerCampaignV2.PrimaryDealer != null && !string.IsNullOrEmpty(viewModel.primaryDealerDistance))
                                           { %>
                                        <p class="font12 text-x-light">Approx. <%=viewModel.primaryDealerDistance %> kms away</p>
                                        <% } %>
                                    </div>
                                    <a href="javascript:void(0)" id="getdealerdetails" class="btn btn-orange partner-dealer-details-btn">View dealer details</a>
                                </div>
                                <% } %>
                                <!-- upcoming start -->
                                <% if (modelPageEntity.ModelDetails.Futuristic && modelPageEntity.UpcomingBike != null)
                                   { %>
                                <div id="upcoming">
                                    <% if (modelPageEntity.UpcomingBike.EstimatedPriceMin != 0 && modelPageEntity.UpcomingBike.EstimatedPriceMax != 0)
                                       { %>
                                    <div id="expectedPriceContainer" class="padding-top15">
                                        <div itemscope itemtype="http://schema.org/Offer">
                                            <p class="font14 default-showroom-text text-light-grey">Expected Price</p>
                                            <div class="modelExpectedPrice margin-bottom15">
                                                <span class="bwsprite inr-md-lg"></span>
                                                <span id="bike-price" class="font22 text-bold">
                                                    <span itemprop="priceSpecification" itemscope itemtype="http://schema.org/PriceSpecification">
                                                        <span itemprop="price minPrice">
                                                            <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMin)) %>
                                                        </span>
                                                        <span>- </span>
                                                        <span itemprop="priceCurrency" content="INR"><span class="bwsprite inr-md-lg"></span></span>
                                                        <span itemprop="maxPrice"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMax)) %></span>
                                                    </span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <%}
                                       else
                                       { %>
                                    <p class="font26 default-showroom-text text-light-grey margin-bottom5">Price Unavailable</p>
                                    <% } %>
                                    <% if (!string.IsNullOrEmpty(modelPageEntity.UpcomingBike.ExpectedLaunchDate))
                                       { %>
                                    <div id="expectedDateContainer" class="padding-top15 font14">
                                        <p class="default-showroom-text text-light-grey margin-bottom10">Expected launch date</p>
                                        <p class="modelLaunchDate text-bold font18 margin-bottom10"><%= modelPageEntity.UpcomingBike.ExpectedLaunchDate %></p>
                                        <p class="default-showroom-text text-light-grey"><%= bikeName %> is not launched in India yet.</p>
                                        <p class="text-light-grey">Information on this page is tentative.</p>
                                    </div>
                                    <%} %>
                                </div>
                                <% } %>
                                <!-- upcoming end -->
                            </div>
                            <div class="grid-5 alpha margin-bottom20">
                                <div class="position-rel <%= modelPageEntity.ModelDetails.Futuristic ? string.Empty : "hide" %>">
                                    <span class="upcoming-text-label font16 position-abt text-white text-center">Upcoming</span>
                                </div>
                                <div class="position-rel <%= !modelPageEntity.ModelDetails.Futuristic && !modelPageEntity.ModelDetails.New ? string.Empty : "hide" %>">
                                    <span class="discontinued-text-label font16 position-abt text-center">Discontinued</span>
                                </div>
                                <div class="connected-carousels" id="bikeBannerImageCarousel">
                                    <div class="stage">
                                        <div class="carousel carousel-stage">
                                            <ul>
                                                <asp:Repeater ID="rptModelPhotos" runat="server">
                                                    <ItemTemplate>
                                                        <li>
                                                            <div class="carousel-img-container">
                                                                <span>
                                                                    <a href="/<%=modelPageEntity.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPageEntity.ModelDetails.MaskingName %>/images/?modelpage=true&imageindex=<%#Container.ItemIndex %>#modelGallery"><img class='<%# Container.ItemIndex > 2 ? "lazy" : "" %>' data-original='<%# Container.ItemIndex > 2 ? Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) : "" %>' title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src='<%# Container.ItemIndex > 2 ? "" : Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>' border="0" /></a>
                                                                </span>
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </div>
                                        <a href="#" class="prev prev-stage bwsprite" rel="nofollow"></a>
                                        <a href="#" class="next next-stage bwsprite" rel="nofollow"></a>
                                    </div>

                                    <div class="navigation">
                                        <a href="#" class="prev prev-navigation bwsprite" rel="nofollow"></a>
                                        <a href="#" class="next next-navigation bwsprite" rel="nofollow"></a>
                                        <div class="carousel carousel-navigation">
                                            <ul>
                                                <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                                    <ItemTemplate>
                                                        <li>
                                                            <div class="carousel-nav-img-container">
                                                                <span>
                                                                    <img class="<%# Container.ItemIndex > 7 ? "lazy" : "" %>" data-original="<%# Container.ItemIndex > 7 ? Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) : "" %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="<%# Container.ItemIndex <= 7 ? Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) : "https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"%>" border="0" />
                                                                </span>
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <%if (modelPageEntity != null && modelPageEntity.Photos != null && modelPageEntity.Photos.Count > 4)
                                                  { %>
                                                <li class="all-photos-target">
                                                    <a href="/<%= modelPageEntity.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPageEntity.ModelDetails.MaskingName %>/images/" title="<%= bikeName %> Images">All Images</a>
                                                </li>
                                                <%} %>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>

                            <% if (viewModel != null && viewModel.IsPremiumDealer)
                               { %>
                            <div id="dealerDetailsWrapper" class="border-solid-top">

                                <div class="padding-top20">
                                    <div class="inline-block margin-right10 vertical-top">
                                        <span class="model-sprite partner-dealer"></span>
                                    </div>
                                    <div class="inline-block position-rel margin-bottom10">
                                        <div class="vertical-top">
                                        <h3 class="font18 text-black inline-block"><%= viewModel.Organization %></h3>
                                         <% if (!string.IsNullOrEmpty(viewModel.MaskingNumber))
                                            { %>
                                        <div class="partner-dealer-contact position-rel pos-top2 vertical-top margin-left10 inline-block padding-right10">
                                        <span class="bwsprite phone-md margin-right5"></span>
                                        <span class="font16 text-bold"><%=viewModel.MaskingNumber %></span>
                                    </div>
                                    <% } %>
                                        <p class="font12 text-x-light"><%= (!viewModel.IsDSA ? "Authorized Dealer in " : "Multi-brand Dealer in ") %><%= viewModel.AreaName %></p>
                                        </div>
                                    <% if (viewModel.IsDSA)
                                       { %>
                                        <div class="bw-tooltip multi-brand-tooltip tooltip-left">
                                            <p class="bw-tooltip-text position-rel font14">This dealer sells bikes of multiple brands.<br />Above price is not final and may vary at the dealership.</p>
                                            <span class="position-abt pos-top10 pos-right10 bwsprite cross-sm-dark-grey cur-pointer close-bw-tooltip"></span>
                                        </div>
                                    </div>
                                    <%} %>
                                </div>

                                <% if (viewModel.Offers != null && viewModel.OfferCount > 0)
                                   { %>
                                <div class="padding-top20 border-light-top">
                                    <p class="font14 text-bold margin-bottom15">Offers from this dealer</p>
                                    <ul class="dealership-benefit-list">
                                        <asp:Repeater ID="rptOffers" runat="server">
                                            <ItemTemplate>
                                                <li class="inline-block">
                                                    <span class="benefit-list-image model-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>"></span>
                                                    <span class="font14 benefit-list-title inline-block"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText"))%>
                                                        <span class="tnc font9 margin-left5 <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsOfferTerms"))? string.Empty: "hide" %>" id="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferId")) %>"> View terms</span>
                                                    </span>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                                <% } %>

                                <% if (isBookingAvailable && bookingAmt > 0)
                                   { %>
                                <div class="padding-top20 padding-bottom20 border-light-top font14">
                                    <p class="text-bold margin-bottom15">Book your bike from this dealer</p>
                                    <div class="grid-8 alpha">
                                        <p class="margin-bottom20">Pay <span class="bwsprite inr-sm-dark"></span><%= Bikewale.Utility.Format.FormatPrice(bookingAmt.ToString()) %> to book online and balance amount of <span class="bwsprite inr-sm-dark"></span><%= Bikewale.Utility.Format.FormatPrice((price-bookingAmt).ToString()) %> at the dealership</p>
                                        <ul id="booking-benefits-list">
                                            <li>Save on dealer visits</li>
                                            <li>Secure online payments</li>
                                            <li>Complete buyer protection</li>
                                        </ul>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-4 padding-top15">
                                        <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-white book-now-btn">Book now</a>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <% } %>                                

                                <div id="dealerAssistance" class="border-light-top padding-top20">
                                    <div id="buyingAssistance">
                                        <p class="font14 text-bold margin-bottom15">Complete buying assistance from <%= viewModel.Organization %></p>
                                        <p class="font14 text-light-grey margin-bottom25">Get in touch with <%= viewModel.Organization %> for best offers, test rides, EMI options, exchange benefits and much more...</p>
                                        <div>
                                            <div class="input-box assistance-input-box">
                                                <input type="text" id="assistGetName" data-bind="textInput: fullName" />
                                                <label for="assistGetName">Name<sup>*</sup></label>
                                                <span class="boundary"></span>
                                                <span class="error-text"></span>
                                            </div>

                                            <div class="input-box assistance-input-box">
                                                <input type="text" id="assistGetEmail" data-bind="textInput: emailId" />
                                                <label for="assistGetEmail">Email<sup>*</sup></label>
                                                <span class="boundary"></span>
                                                <span class="error-text"></span>
                                            </div>

                                            <div class="input-box input-number-box assistance-input-box">
                                                <input type="tel" maxlength="10" id="assistGetMobile" data-bind="textInput: mobileNo" />
                                                <label for="assistGetMobile">Mobile number<sup>*</sup></label>
                                                <span class="input-number-prefix">+91</span>
                                                <span class="boundary"></span>
                                                <span class="error-text"></span>
                                            </div>

                                            <a class="btn btn-teal assistance-submit-btn" data-leadsourceid="13" data-item-id="<%= dealerId %>" data-item-name="<%= (viewModel!=null) ? viewModel.Organization : string.Empty %>" data-item-area="<%= (viewModel!=null) ? viewModel.AreaName : string.Empty %> " data-isleadpopup="false" id="assistFormSubmit" data-bind="event: { click: HiddenSubmitLead }">Get assistance</a>
                                            <p class="margin-bottom10 text-left">By proceeding ahead, you agree to BikeWale <a title="Visitor agreement" href="/visitoragreement.aspx" target="_blank">visitor agreement</a> and <a title="Privacy policy" href="/privacypolicy.aspx" target="_blank">privacy policy</a>.</p>
                                        </div>
                                    </div>
                                    <div id="dealer-assist-msg" class="hide">
                                        <p class="font14 leftfloat">Thank you for your interest. <%= viewModel.Organization %> - <%= viewModel.AreaName %> will get in touch shortly</p>
                                        <span class="assistance-response-close bwsprite cross-lg-lgt-grey cur-pointer rightfloat"></span>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                
                            </div>
                            <% } %>

                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>

            <!-- Terms and condition Popup start -->
            <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                <div class="fixed-close-btn-wrapper">
                    <div class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
                </div>
                <h3>Terms and conditions</h3>
                <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                    <img class="lazy" data-original="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" src="" />
                </div>
                <div id="terms" class="breakup-text-container padding-bottom10 font14">
                </div>
                <div id='orig-terms' class='hide'>
                </div>
            </div>
            <!-- Terms and condition Popup Ends -->
        </section>

        <% if (viewModel != null && viewModel.SecondaryDealersV2 != null && viewModel.SecondaryDealerCount > 0)
           { %>
        <section>
            <div class="container">
                <div class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div id="partner-dealer-panel" class="content-box-shadow padding-14-20 font18 text-bold text-black position-rel cur-pointer">
                            Prices from <%=viewModel.SecondaryDealerCount %> more partner <%= viewModel.SecondaryDealerCount == 1 ? "dealer" : "dealers"%>  in <%=cityName %><span class="model-sprite plus-icon"></span>
                        </div>
                        <div id="moreDealersList" class="jcarousel-wrapper inner-content-carousel">
                            <div class="jcarousel margin-top20 margin-bottom20">
                                <ul>
                                    <% foreach (var bike in viewModel.SecondaryDealersV2)
                                       { %>
                                            <li style="min-height:191px">
                                                <a href="javascript:void(0);" onclick="secondarydealer_Click(<%= bike.DealerId %>)" title="<%= bike.Name %>" class="top-block-target">
                                                    <div class="grid-10 alpha margin-bottom5">
                                                        <span class="font14 text-black text-bold text-truncate block"><%= bike.Name %></span>
                                                    </div>
                                                    <div class="grid-2 alpha omega font12 text-light-grey text-right pos-top2"><%= Math.Truncate(bike.Distance) %> kms</div>
                                                    <div class="clear"></div>
                                                    <div class="margin-bottom5">
                                                        <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                        <span class="vertical-top details-column font14 text-light-grey"><%= bike.Area %></span>
                                                    </div>
                                                    <div class="margin-top10">
                                                        <div class="grid-5 alpha omega">
                                                            <p class="font12 text-light-grey margin-bottom5">On-road price</p>
                                                            <span class="bwsprite inr-md"></span>&nbsp;<span class="font16 text-default text-bold"><%=Bikewale.Utility.Format.FormatPrice(bike.SelectedVersionPrice.ToString()) %></span>
                                                        </div>
                                                        <% if (bike.OfferCount > 0)
                                                           { %>
                                                        <div class="grid-7 border-solid-left padding-top10 padding-bottom10 padding-left20 omega ">
                                                            <span class="bwsprite offers-sm-box"></span>
                                                            <span class="font14 text-default text-bold"><%=bike.OfferCount %></span>
                                                            <span class="font12 text-light-grey">Offer<%=(bike.OfferCount)>1?"s":""%> available</span>
                                                        </div>
                                                        <% } %>
								                        <div class="clear"></div>
							                        </div>
                                                </a>
                                                <div class="bottom-block-button margin-top15">
                                                    <a href="javascript:void(0)" onclick="secondarydealer_Click(<%= bike.DealerId %>)" class="btn btn-white partner-dealer-offers-btn" rel="nofollow">View detailed price</a>
                                                </div>
                                            </li>
                                    <% } %>                             
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>

        <% if (pqOnRoad != null && pqOnRoad.BPQOutput != null && viewModel == null && isOnRoadPrice && !string.IsNullOrEmpty(pqOnRoad.BPQOutput.ManufacturerAd))
           {
        %>
        <style type="text/css">
            .offer-benefit-sprite { background: url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/offer-benefit-sprite.png?v1=30Mar2016v1) no-repeat; display: inline-block; }
            #campaign-offer-list li, #campaign-offer-list li span { display: inline-block; vertical-align: middle; }
            #campaign-container .campaign-left-col { width: 78%; padding-right: 10px; }
            #campaign-container .campaign-right-col { width: 21%; }
            .campaign-offer-label { width: 75%; font-size: 14px; font-weight: 700; }
            .btn-large { padding: 8px 56px; }
            #campaign-offer-list li { width: 175px; margin-top: 15px; margin-bottom: 10px; padding-right: 5px; }
            .campaign-offer-1, .campaign-offer-2, .campaign-offer-3, .campaign-offer-4 { width: 34px; height: 28px; margin-right: 5px; }
            .campaign-offer-1 { background-position: 0 -356px; }
            .campaign-offer-2 { background-position: 0 -390px; }
            .campaign-offer-3 { background-position: 0 -425px; }
            .campaign-offer-4 { background-position: 0 -463px; }
            #campaign-container .phone-black-icon { top: 0; }
        </style>
        <%=String.Format(pqOnRoad.BPQOutput.ManufacturerAd) %>

        <%} %>
        <meta itemprop="manufacturer" name="manufacturer" content="<%= modelPageEntity.ModelDetails.MakeBase.MakeName %>">
        <meta itemprop="model" content="<%= TargetedModel %>" />
        <meta itemprop="brand" content="<%= bikeMakeName %>" />
        <style type="text/css">
            .padding-left2 { padding-left: 2px; }
            , #campaign-offer-list li, #campaign-offer-list li span { display: inline-block; vertical-align: middle; }
            #campaign-container .campaign-left-col { width: 78%; padding-right: 10px; }
            #campaign-container .campaign-right-col { width: 21%; }
            .campaign-offer-label { width: 75%; font-size: 14px; font-weight: 700; }
            .btn-large { padding: 8px 56px; }
            #campaign-offer-list li { width: 175px; margin-top: 15px; margin-bottom: 10px; padding-right: 5px; }
            .campaign-offer-1, .campaign-offer-2, .campaign-offer-3, .campaign-offer-4 { width: 34px; height: 28px; margin-right: 5px; }
            .campaign-offer-1 { background-position: 0 -356px; }
            .campaign-offer-2 { background-position: 0 -390px; }
            .campaign-offer-3 { background-position: 0 -425px; }
            .campaign-offer-4 { background-position: 0 -463px; }
        </style>
        <section>
            <div id="modelDetailsFloatingCardContent" class="container">
                <div class="grid-12">
                <div class="model-details-floating-card">
                    <div class="content-box-shadow content-inner-block-1020">
                        <div class="grid-5 alpha omega">
                            <div class="model-card-image-content inline-block-top margin-right20">
                                <img src="<%= modelImage %>"/>
                            </div>
                            <div class="model-card-title-content inline-block-top">
                                <p class="font16 text-bold margin-bottom5"><%= bikeName %></p>
                                <p class="font14 text-light-grey"><%= variantText %></p>
                            </div>
                        </div>
                        <div class="grid-4 floating-card-area-details">
                            <% if (!modelPageEntity.ModelDetails.Futuristic)
                               { %>
                            <%if (isDiscontinued)
                              { %>
                            <p class="font14 text-light-grey leftfloat">Last known Ex-showroom price</p>
                            <%}
                              else
                                  if (!isCitySelected)
                                  {%>
                            <p class="font14 text-light-grey exshowroom-area"><span>Ex-showroom price in</span>&nbsp;<span class="font14 exshowroom-area-name text-truncate"><%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span></p>
                            <% } %>
                            <% else if (!isOnRoadPrice)
                                 {%>
                                <p class="font14 text-light-grey leftfloat exshowroom-area"><span class="leftfloat">Ex-showroom price in </span><span class="leftfloat text-truncate exshowroom-area-name city-area-name margin-right5"><%= String.Format("{0}{1}{2}",areaName,(!string.IsNullOrEmpty(areaName) ? ", " : string.Empty),cityName) %></span></p>
                            <% } %>
                            <% else
                                 {%>
                                <p class="font14 text-light-grey leftfloat"><span class="leftfloat">On-road price in </span><span class="leftfloat text-truncate city-area-name margin-right5"><%= String.Format("{0}{1}{2}",areaName,(!string.IsNullOrEmpty(areaName) ? ", " : string.Empty),cityName) %></span></p>

                            <% } %>
                            <div class="clear"></div>

                            <div>
                                <% if (price == 0)
                                   { %>
                                <span class="font16">Price not available</span>
                                <%  }
                                   else
                                   { %>

                                <span class="bwsprite inr-lg"></span>&nbsp;
                                <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                <%} %>
                            </div>
                            <%}
                               //<!-- upcoming start Floating -->
                               else if (modelPageEntity.UpcomingBike != null)
                               { %>
                            <% if (modelPageEntity.UpcomingBike.EstimatedPriceMin != 0 && modelPageEntity.UpcomingBike.EstimatedPriceMax != 0)
                               { %>
                            <p class="font14 text-light-grey margin-bottom5 leftfloat">Expected Price</p>
                            <div class="clear"></div>
                            <span class="bwsprite inr-lg"></span>
                            <span id="bike-priceFloating" class="font18 text-bold">
                                <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMin)) %></span>
                                <span>- </span>
                                <span class="bwsprite inr-lg"></span>
                                <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMax)) %></span>
                            </span>
                            <%}
                               else
                               { %>
                            <p class="font20">Price Unavailable</p>
                            <% } %>
                            <% } %>
                            <!-- upcoming end Floating-->
                        </div>
                        <div class="grid-3 model-orp-btn alpha omega">
                            <% if (toShowOnRoadPriceButton && !isDiscontinued)
                               { %>
                            <a href="javascript:void(0)" id="btnCheckOnRoadPriceFloating" data-persistent="true" data-reload="true" data-modelid="<%=modelId %>" class="btn btn-orange font14 <%=(viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) ? "margin-top5" : "margin-top20" %> getquotation bw-ga" rel="nofollow" c="Model_Page" a="Floating_Card_Check_On_Road_Price_Button_Clicked" v="myBikeName">Check on-road price</a>
                            <%}
                               else
                                   if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ && !isDiscontinued)
                                   {%>
                            <a href="javascript:void(0)" data-leadsourceid="24" data-item-id="<%= dealerId %>" data-item-name="<%= viewModel.Organization %>" data-item-area="<%= viewModel.AreaName %> " class="btn btn-orange leadcapturebtn font14 bw-ga <%=(viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) ? "margin-top5" : "margin-top20" %>" rel="nofollow" c="Model_Page" a="Floating_Card_Get_Offers_Clicked" v="bikeVersionLocation"" ><%= viewModel.LeadBtnTextLarge %></a>
                            <%} %>

                            <%-- if no 'powered by' text is present remove margin-top5 add margin-top20 in offers button --%>
                            <%if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ)
                              { %>
                            <p class="model-powered-by-text font12 margin-top10 text-truncate" title="<%= viewModel.Organization %>, <%=viewModel.AreaName %>"><span class="text-light-grey">Powered by </span><%= viewModel.Organization %>, <%=viewModel.AreaName %></p>
                            <%} %>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div class="overall-specs-tabs-wrapper content-box-shadow">
                        <% if ((modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)) || modelPageEntity.ModelVersionSpecs != null)
                           { %>
                        <a href="#modelSummaryContent" rel="nofollow">Summary</a>
                        <%} %>
                        <% if (modelPageEntity.ModelVersions != null && modelPageEntity.ModelVersions.Count > 0)
                           { %>
                        <a href="#modelPricesContent" rel="nofollow">Price</a>
                        <% } %>
                        <% if (modelPageEntity.ModelVersionSpecs != null)
                           { %>
                        <a href="#modelSpecsFeaturesContent" rel="nofollow">Specs & Features</a>
                        <% } %>
                        <%if (modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                          { %>
                        <a href="#modelColorsContent" rel="nofollow">Colors</a>
                        <%} %>
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelReviewsContent" rel="nofollow">Reviews</a>
                        <%} %>
                        <% if (ctrlExpertReviews.FetchedRecordsCount == 0 && ctrlUserReviews.FetchedRecordsCount == 0 && ctrlNews.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelReviewsContent" rel="nofollow">News</a>
                        <% } %>
                        <% if (ctrlVideos.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelVideosContent" rel="nofollow">Videos</a>
                        <%} %>
                        <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelSimilarContent" rel="nofollow">Similar Bikes</a>
                        <%} %>
                        <% if ((!isDiscontinued && !modelPageEntity.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)))
                           { %>
                        <a href="#dealerAndServiceContent" rel="nofollow">
                            <% if (ctrlDealerCard.showWidget)
                               {%> Dealers<%} %>
                            <% if (ctrlDealerCard.showServiceCenter || (ctrlServiceCenterCard.showWidget && cityId > 0))
                               { %>
                                <% if (ctrlDealerCard.showWidget)
                                   {%> &<%}%> Service Centers
                            <%} %>
                        </a>
                        <%} %>
                        
                        <% if (ctrlRecentUsedBikes.FetchedRecordsCount > 0)
                           { %>
                        <a href="#makeUsedBikeContent" rel="nofollow">Used</a>
                        <%} %>
                    </div>
                </div>
            </div>
            </div>
        </section>

        <section>
            <div class="container">
                <div id="modelSpecsTabsContentWrapper" class="grid-12 margin-bottom20">
                    <div class="content-box-shadow">
                        <div class="overall-specs-tabs-wrapper">
                            <% if ((modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)) || modelPageEntity.ModelVersionSpecs != null)
                               { %>
                            <a class="active" href="#modelSummaryContent" rel="nofollow">Summary</a>
                            <%} %>
                            <% if (modelPageEntity.ModelVersions != null && modelPageEntity.ModelVersions.Count > 0)
                               { %>
                            <a href="#modelPricesContent" rel="nofollow">Price</a>
                            <% } %>
                            <% if (modelPageEntity.ModelVersionSpecs != null)
                               { %>
                            <a href="#modelSpecsFeaturesContent" rel="nofollow">Specs & Features</a>
                            <% } %>
                            <%if (modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                              { %>
                            <a href="#modelColorsContent" rel="nofollow">Colors</a>
                            <%} %>
                            <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0)
                               { %>
                            <a href="#modelReviewsContent" rel="nofollow">Reviews</a>
                            <%} %>
                            <% if (ctrlExpertReviews.FetchedRecordsCount == 0 && ctrlUserReviews.FetchedRecordsCount == 0 && ctrlNews.FetchedRecordsCount > 0)
                               { %>
                            <a href="#modelReviewsContent" rel="nofollow">News</a>
                            <% } %>
                            <% if (ctrlVideos.FetchedRecordsCount > 0)
                               { %>
                            <a href="#modelVideosContent" rel="nofollow">Videos</a>
                            <%} %>
                            <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                               { %>
                            <a href="#modelSimilarContent" rel="nofollow">Similar Bikes</a>
                            <%} %>
                            <% if ((!isDiscontinued && !modelPageEntity.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)))
                               { %>
                            <a href="#dealerAndServiceContent" rel="nofollow">
                                <% if (ctrlDealerCard.showWidget)
                                   {%> Dealers<%} %>
                                <% if (ctrlDealerCard.showServiceCenter || (ctrlServiceCenterCard.showWidget && cityId > 0))
                                   { %>
                                    <% if (ctrlDealerCard.showWidget)
                                       {%> &<%}%> Service Centers
                                <%} %>
                            </a>
                            <%} %>                          
                            <% if (ctrlRecentUsedBikes.FetchedRecordsCount > 0)
                               { %>
                            <a href="#makeUsedBikeContent" rel="nofollow">Used</a>
                            <%} %>
                        </div>
                        <div class="border-divider"></div>

                        <% if ((modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)) || modelPageEntity.ModelVersionSpecs != null)
                           { %>
                        <div id="modelSummaryContent" class="bw-model-tabs-data margin-right10 margin-left10 content-inner-block-2010 border-solid-bottom">
                            <%if (!modelPageEntity.ModelDetails.Futuristic && modelPageEntity.ModelDetails.MinPrice > 0)
                              { %>
                            <h2 class="margin-bottom0"><%=bikeName %> summary</h2>
                            <%if (!modelPageEntity.ModelDetails.Futuristic && bikeRankObj != null && bikeRankObj.Rank > 0)
                              { %>
                            <div class="grid-8 alpha margin-top15"><%-- add class - grid-12 and omega, if no slug instead of grid-8 --%>
                                <%} %>
                                <%else
                              { %>
                                <div class="grid-12 omega alpha margin-top15">
                                <%} %>
                                <p class="font14 text-light-grey line-height17 margin-bottom15"><%=summaryDescription %></p>
                            </div>
                            <%if (bikeRankObj != null && bikeRankObj.Rank > 0)
                              { %>
                                
                            <div class="grid-4 omega margin-bottom10">
                                <a href="<%= Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName%> in India" class="model-rank-slug">
                                    <div class="inline-block icon-red-bg">
                                        <span class="bwsprite rank-graph"></span>
                                    </div>
                                    <div class="rank-slug-label inline-block text-bold text-default">
                                        <p class="font14"><%=bikeRankObj.Rank>1?rankText:"" %> Most Popular <%=bikeType %></p>
                                        <p class="font11">Check out the complete list.</p>
                                    </div>
                                    <span class="trend-arrow"></span>
                                    <span class="bwsprite right-arrow"></span>
                                </a>
                            </div>
                                
                            <%} %>
                            <div class="clear"></div>

                            <div class="border-solid-bottom margin-bottom20">
                                <div class="grid-8 alpha margin-bottom10">
                                    <table id="model-key-highlights" cellspacing="0" cellpadding="0" width="100%" border="0" class="font14 text-left">
                                        <thead>
                                            <tr>
                                                <th colspan="2"><%= modelPageEntity.ModelDetails.ModelName %> key highlights</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                           <% if (modelPageEntity.ModelDetails.MinPrice > 0)
                                              {%> 
                                            <tr>
                                                <td width="36%">Price</td>
                                                <td width="64%">
                                                    <span class="bwsprite inr-sm-dark"></span>
                                                    <span class="text-bold"><%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPageEntity.ModelDetails.MinPrice))%></span>
                                                    <span class="font12 text-light-grey">(Ex-showroom <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %>)</span>
                                                </td>
                                            </tr>
                                            <%} %>
                                            <%if (modelPageEntity != null && modelPageEntity.ModelVersionSpecs != null && modelPageEntity.ModelVersionSpecs.TopSpeed > 0)
                                              {%>
                                            <tr>
                                                <td>Top speed</td>
                                                <td class="text-bold"> <%=modelPageEntity.ModelVersionSpecs.TopSpeed %> kmph</td>
                                            </tr>
                                            <%} %>
                                            <%if (modelPageEntity != null && modelPageEntity.ModelVersionSpecs != null && modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0)
                                              {%>
                                            <tr>
                                                <td>Mileage</td>
                                                <td class="text-bold"> <%=modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall %> kmpl</td>
                                            </tr>
                                            <%} %>
                                            <%if (colorCount > 0)
                                              {%>
                                            <tr>
                                                <td valign="top">Colors</td>
                                                <td valign="top" class="text-bold">
                                                    <ul class="model-color-list">
                                                          <%foreach (var colorName in modelPageEntity.ModelColors)
                                                            { %>
                                                        <li class="leftfloat"><%=colorName.ColorName %></li>
                                                        <%} %>
                                                    </ul>
                                                </td>
                                            </tr>
                                            <%} %>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="grid-4 text-center alpha omega margin-bottom10">
                                    <!-- #include file="/ads/Ad300x250.aspx" -->
                                </div>
                                <div class="clear"></div>
                            </div>
                            <%} %>
                            <%if (modelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription))
                              { %>
                                <div id="model-overview-content" class="margin-bottom20">
                                    <h3><%= modelPageEntity.ModelDetails.ModelName %> preview</h3>
                                    <p class="font14 text-light-grey line-height17 inline">
                                        <span class="model-preview-main-content">
                                            <%= modelPageEntity.ModelDesc.SmallDescription %>
                                        </span>                                
                                        <span class="model-preview-more-content hide">
                                            <%= modelPageEntity.ModelDesc.FullDescription %>
                                        </span>
                                        <a href="javascript:void(0)" class="font14 read-more-model-preview" rel="nofollow">Read more</a>
                                    </p>
                                </div>
                            <%} %>
                        </div>

                        <%} %>

                        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

                        <% if (modelPageEntity.ModelVersions != null && modelPageEntity.ModelVersions.Count > 0)
                           { %>
                        <div id="modelPricesContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-right10 padding-bottom15 padding-left10 border-solid-bottom">
                            <h2><%=bikeName %> Price List</h2>
                            <div id="prices-by-version-content" class="grid-6 alpha padding-right20">
                                <h3 class="margin-bottom20">Price by versions</h3>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th align="left" width="65%" class="font12 text-unbold text-xt-light-grey padding-bottom5 border-solid-bottom"><%= modelPageEntity.ModelDetails.ModelName %> Version</th>
                                            <th align="left" width="35%" class="font12 text-unbold text-xt-light-grey padding-bottom5 border-solid-bottom">Price</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptVarients" OnItemDataBound="rptVarients_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class="version-prices-tr">
                                                    <td width="70%" class="padding-bottom15 padding-top10 padding-right10 font14" valign="top">
                                                        <p class="margin-bottom5"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionName")) %></p>
                                                        <p class="font12 text-xt-light-grey"><%# FormatVarientMinSpec(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")),Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                                    </td>
                                                    <td width="30%" class="padding-bottom10 padding-top10 font14 divider-bottom" valign="top">
                                                        <p class="font16 text-bold text-default">
                                                            <span class="bwsprite inr-md"></span>
                                                            <span>
                                                                <asp:Label Text='<%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %>' ID="txtComment" runat="server"></asp:Label></span>
                                                        </p>
                                                        <p class="font12 text-xt-dark-grey">
                                                            <asp:Label ID="lblExOn" Text="Ex-showroom" runat="server"></asp:Label>, 
                                                            <% if (cityId != 0 && !string.IsNullOrEmpty(cityName))
                                                               { %>
                                                            <span><%= cityName %></span>
                                                            <% }
                                                               else
                                                               { %>
                                                            <span><%= Bikewale.Common.Configuration.GetDefaultCityName %></span>
                                                            <% } %>
                                                        </p>
                                                    </td>
                                                    <asp:HiddenField ID="hdnVariant" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "VersionId") %>' />
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <BW:TopCityPrice ID="ctrlTopCityPrices" runat="server" />
                            <div class="clear"></div>
                        </div>
                        <% } %>

                        <% if (modelPageEntity.ModelVersionSpecs != null)
                           { %>
                        <div id="modelSpecsFeaturesContent" class="bw-model-tabs-data padding-top20 font14">
                            <h2 class="padding-left20 padding-right20"><%=bikeModelName%> Specifications & Features</h2>
                            <h3 class="padding-left20">Specifications</h3>

                            <ul id="model-specs-list">
                                <li>
                                    <div class="model-accordion-tab active">
                                        <span class="model-sprite engine-sm-icon margin-right10"></span>
                                        <span class="inline-block">Engine & transmission</span>
                                        <span class="bwsprite accordion-angle-icon"></span>
                                    </div>
                                    <div class="specs-features-list">
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>Displacement</p>
                                                <p>Cylinders</p>
                                                <p>Max Power</p>
                                                <p>Maximum Torque</p>
                                                <p>Bore</p>
                                                <p>Stroke</p>
                                                <p>Valves Per Cylinder</p>
                                                <p>Fuel Delivery System</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement) %> <span>cc</span></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Cylinders) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower, "bhp", modelPageEntity.ModelVersionSpecs.MaxPowerRPM, "rpm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaximumTorque, "Nm", modelPageEntity.ModelVersionSpecs.MaximumTorqueRPM,"rpm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Bore,"mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Stroke,"mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ValvesPerCylinder) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelDeliverySystem) %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>Fuel Type</p>
                                                <p>Ignition</p>
                                                <p>Spark Plugs</p>
                                                <p>Cooling System</p>
                                                <p>Gearbox Type</p>
                                                <p>No. of Gears</p>
                                                <p>Transmission Type</p>
                                                <p>Clutch</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelType) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Ignition) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.SparkPlugsPerCylinder, "Per Cylinder") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.CoolingSystem) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.GearboxType) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.NoOfGears) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TransmissionType) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Clutch) %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li>
                                    <div class="model-accordion-tab">
                                        <span class="model-sprite brakes-sm-icon margin-right10"></span>
                                        <span class="inline-block">Brakes, wheels & suspension</span>
                                        <span class="bwsprite accordion-angle-icon"></span>
                                    </div>
                                    <div class="specs-features-list">
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>Brake Type</p>
                                                <p>Front Disc</p>
                                                <p>Front Disc/Drum Size</p>
                                                <p>Rear Disc</p>
                                                <p>Rear Disc/Drum Size</p>
                                                <p>Calliper Type</p>
                                                <p>Wheel Size</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.BrakeType) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontDisc) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontDisc_DrumSize,"mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearDisc) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearDisc_DrumSize,"mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.CalliperType) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.WheelSize,"inches") %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>Front Tyre</p>
                                                <p>Rear Tyre</p>
                                                <p>Tubeless Tyres</p>
                                                <p>Radial Tyres</p>
                                                <p>Alloy Wheels</p>
                                                <p>Front Suspension</p>
                                                <p>Rear Suspension</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontTyre) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearTyre) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TubelessTyres) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RadialTyres) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.AlloyWheels) %></p>
                                                <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontSuspension) %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontSuspension) %></p>
                                                <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearSuspension) %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearSuspension) %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li>
                                    <div class="model-accordion-tab">
                                        <span class="model-sprite dimension-sm-icon margin-right10"></span>
                                        <span class="inline-block">Dimensions & chassis</span>
                                        <span class="bwsprite accordion-angle-icon"></span>
                                    </div>
                                    <div class="specs-features-list">
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>Kerb Weight</p>
                                                <p>Overall Length</p>
                                                <p>Overall Width</p>
                                                <p>Overall Height</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight,"kg") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.OverallLength,"mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.OverallWidth,"mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.OverallHeight,"mm") %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>Wheelbase</p>
                                                <p>Ground Clearance</p>
                                                <p>Seat Height</p>
                                                <p>Chassis Type</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Wheelbase,"mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.GroundClearance, "mm") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.SeatHeight,"mm") %></p>
                                                <p title="<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ChassisType) %>"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ChassisType) %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                                <li>
                                    <div class="model-accordion-tab">
                                        <span class="model-sprite fuel-sm-icon margin-right10"></span>
                                        <span class="inline-block">Fuel efficiency & performance</span>
                                        <span class="bwsprite accordion-angle-icon"></span>
                                    </div>
                                    <div class="specs-features-list">
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>Fuel Tank Capacity</p>
                                                <p>Reserve Fuel Capacity</p>
                                                <p>Fuel Efficiency Overall</p>
                                                <p>Fuel Efficiency Range</p>
                                                <p>Top Speed</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelTankCapacity,"litres") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ReserveFuelCapacity,"litres") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall,"kmpl") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyRange,"km") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TopSpeed,"kmph") %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="grid-6">
                                            <div class="grid-6 text-light-grey">
                                                <p>0 to 60 kmph</p>
                                                <p>0 to 80 kmph</p>
                                                <p>0 to 40 kmph</p>
                                                <p>60 to 0 kmph</p>
                                                <p>80 to 0 kmph</p>
                                            </div>
                                            <div class="grid-6 text-bold">
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_0_60_kmph,"seconds") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_0_80_kmph,"seconds") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_0_40_m,"seconds") %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_60_0_kmph) %></p>
                                                <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Performance_80_0_kmph) %></p>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </li>
                            </ul>

                            <div id="model-features-content" class="margin-top20 margin-bottom20">
                                <h3 class="padding-left20">Features</h3>
                                <div class="specs-features-list model-features-list">
                                    <div class="grid-6 alpha">
                                        <div class="grid-6 padding-left20 text-light-grey">
                                            <p>Speedometer</p>
                                            <p>Fuel Guage</p>
                                            <p>Tachometer Type</p>
                                            <p>Tachometer</p>
                                        </div>
                                        <div class="grid-6 omega text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Speedometer) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelGauge) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TachometerType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Tachometer) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-6">
                                        <div class="grid-6 padding-left20 text-light-grey">
                                            <p>Digital Fuel Guage</p>
                                            <p>Tripmeter</p>
                                            <p>Electric Start</p>
                                            <p>Shift Light</p>
                                        </div>
                                        <div class="grid-6 omega text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.DigitalFuelGauge) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Tripmeter) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ElectricStart) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ShiftLight) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>

                                <div id="model-more-features-list" class="specs-features-list model-features-list">
                                    <div class="grid-6 alpha">
                                        <div class="grid-6 padding-left20 text-light-grey">
                                            <p>Stand Alarm</p>
                                            <p>Stepped Seat</p>
                                            <p>No. of Tripmeters</p>
                                            <p>Tripmeter Type</p>
                                            <p>Low Fuel Indicator</p>
                                            <p>Low Oil Indicator</p>
                                            <p>Low Battery Indicator</p>
                                            <p>Pillion Backrest</p>
                                            <p>Pillion Grabrail</p>
                                            <p>Pillion Seat</p>
                                            <p>Pillion Footrest</p>
                                        </div>
                                        <div class="grid-6 omega text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.StandAlarm) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.SteppedSeat) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.NoOfTripmeters) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TripmeterType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.LowFuelIndicator) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.LowOilIndicator) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.LowBatteryIndicator) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionBackrest) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionGrabrail) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionSeat) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PillionFootrest) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="grid-6">
                                        <div class="grid-6 padding-left20 text-light-grey">
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
                                        <div class="grid-6 omega text-bold">
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.AntilockBrakingSystem) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Killswitch) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Clock) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ElectricSystem) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Battery) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.HeadlightType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.HeadlightBulbType) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Brake_Tail_Light) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TurnSignal) %></p>
                                            <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.PassLight) %></p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>

                                <div class="padding-right20 padding-left20">
                                    <a href="javascript:void(0)" class="view-features-link bw-ga" c="Model_Page" a="View_all_features_link_cliked" v="myBikeName" title="<%=bikeName %> Features" rel="nofollow">View all features</a>
                                </div>
                            </div>

                            <div class="margin-right10 margin-left10 border-solid-top"></div>
                        </div>
                        <% } %>

                            <%if (modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                              { %>
                            <div id="modelColorsContent" class="bw-model-tabs-data padding-top20 font14">
                                <h2 class="padding-left20 padding-right20"><%=bikeName %> Colors</h2>
                                <ul id="modelColorsList">
                                    <% foreach (var modelColor in modelPageEntity.ModelColors)
                                       { %>
                                   <%  if(modelColor.ColorImageId > 0) { %>
                                    <a href="/<%=modelPageEntity.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPageEntity.ModelDetails.MaskingName %>/images/?modelpage=true&colorImageId=<%=modelColor.ColorImageId %>#modelGallery">
                                        <%} %>
                                    <li>
                                        <div title="<%= modelColor.ColorName %>" class="color-box inline-block <%= (((IList)modelColor.HexCodes).Count == 1 )?"color-count-one": (((IList)modelColor.HexCodes).Count >= 3 )?"color-count-three":"color-count-two" %>">
                                            <% if (modelPageEntity.ModelColors != null)
                                               {
                                                   foreach (var HexCode in modelColor.HexCodes)
                                                   { %>
                                            <span <%= String.Format("style='background-color: #{0}'",Convert.ToString(HexCode)) %>></span>
                                            <%}
                                               } %>
                                        </div> 
                                        <p class="font16 inline-block text-truncate"><%= Convert.ToString(modelColor.ColorName) %></p>
                                    </li> 
                                         <%  if(modelColor.ColorImageId > 0) { %>  
                                    </a> 
                                    <%} %>                               
                                    <%} %>
                                </ul>
                                <div class="margin-right10 margin-left10 border-solid-top"></div>
                            </div>
                            <%
                              } %>

                        <%if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 || ctrlNews.FetchedRecordsCount > 0)
                          { %>
                        <div id="modelReviewsContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                              <h2><%=bikeName %> Reviews</h2>
                            <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                               { %>
                            <!-- expert review starts-->
                            <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                            <!-- expert review ends-->
                            <% if (ctrlExpertReviews.FetchedRecordsCount > 0 && ctrlUserReviews.FetchedRecordsCount > 0)
                               { %>
                            <div class="margin-bottom20"></div>
                            <% } %>
                            <% } %>

                            <% if (ctrlUserReviews.FetchedRecordsCount > 0)
                               { %>
                            <!-- user reviews -->
                            <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                            <!-- user reviews ends -->
                            <% } %>
                            <% if (ctrlNews.FetchedRecordsCount > 0)
                               { %>
                            <!-- News widget starts -->
                                <BW:LatestNews runat="server" ID="ctrlNews" />
                            <!-- News widget ends -->
                            <% } %>
                        </div>
                        <%} %>

                        <% if (ctrlVideos.FetchedRecordsCount > 0)
                           { %>
                        <div id="modelVideosContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                            <!-- Video reviews -->
                            <BW:Videos runat="server" ID="ctrlVideos" />
                            <!-- Video reviews ends -->
                        </div>
                        <% } %>
                        <!-- model comparison -->
                        <!-- Popular Comparision -->
                        
                        <div id="modelSimilarContent" class="bw-model-tabs-data padding-top20 font14">
                            <% if (ctrlPopularCompare.fetchedCount > 0 || ctrlAlternativeBikes.FetchedRecordsCount > 0)
                               { %>
                            <h2 class="padding-left20 padding-right20 margin-bottom15">Bikes Similar to <%=modelPageEntity.ModelDetails.ModelName%> </h2>
                            <% if (ctrlPopularCompare.fetchedCount > 0)
                               { %>
                            <h3 class="padding-left20 padding-right20 margin-bottom15">Most compared alternatives</h3>
                            <BW:PopularCompare ID="ctrlPopularCompare" runat="server" />

                            <div class="margin-right10 margin-left10 border-solid-bottom padding-bottom20"></div>
                            <%} %>
                            <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0)
                               { %>
                            <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                            <% } %>
                            <% } %>
                            <%if (!modelPageEntity.ModelDetails.Futuristic && bikeRankObj != null)
                              { %>
                            <%if (bikeRankObj.Rank > 0)
                              { %>
                            <div class="margin-left20 margin-right20 padding-bottom20">
                                <div class="content-inner-block-15 border-solid font14">
                                    <div class="grid-9 alpha">
                                        <div class="inline-block">
                                            <span class="item-rank">#<%=bikeRankObj.Rank%></span>
                                        </div>
                                        <p class="inline-block checkout-list-slug-label"><%=bikeModelName%> is the <%=bikeRankObj.Rank>1?rankText:"" %> most popular <%=bikeType.ToLower() %>. Check out other <%=styleName.ToLower() %> which made it to Top 10 list.</p>
                                    </div>
                                    <div class="grid-3 text-right position-rel pos-top5">
                                        <a href="<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName %> in India">Check out the list now<span class="bwsprite blue-right-arrow-icon"></span></a>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <%} %>
                            <%else
                              { %>
                                <div class="margin-left20 margin-right20 padding-bottom20">
                                <div class="content-inner-block-15 border-solid font14">
                                    <div class="grid-9 alpha">
                                        <div class="inline-block icon-red-bg">
                                            <span class="bwsprite rank-graph"></span>
                                        </div>
                                        <p class="inline-block checkout-list-slug-label">Not sure what to buy? List of Top 10 <%=styleName.ToLower() %> can come in handy.</p>
                                    </div>
                                    <div class="grid-3 text-right position-rel pos-top5">
                                        <a href="<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(bikeRankObj.BodyStyle) %>" title="Best <%=styleName %> in India">Check out the list now<span class="bwsprite blue-right-arrow-icon"></span></a>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <%} %>
                            <%} %>
                          </div>
                            <%if (!modelPageEntity.ModelDetails.Futuristic)
                              { %>
                               <div class="margin-right10 margin-left10 border-solid-bottom"></div>
                            <%} %>
                        

                        <%if ((!isDiscontinued && !modelPageEntity.ModelDetails.Futuristic) && (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)))
                          { %>
                        <div id="dealerAndServiceContent" class="bw-model-tabs-data">
                            <% if (ctrlDealerCard.showWidget)
                               { %>
                            <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                            <% } %>
                            <% if (ctrlServiceCenterCard.showWidget && cityId > 0)
                               { %>
                            <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                            <% } %>
                        </div>
                        <%} %>
                        <!-- Used bikes widget -->
                        <% if (ctrlRecentUsedBikes.FetchedRecordsCount > 0)
                           { %>
                        <BW:UsedBikes runat="server" ID="ctrlRecentUsedBikes" />
                        <%} %>
                        
                        <div id="overallSpecsDetailsFooter"></div>
                    </div>
                </div>
                <div class="clear"></div>
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

        <!-- Check On-Road Price popup -->
        <div id="onRoadPricePopup" class="rounded-corner2 content-inner-block-20 text-center hide">
            <div class="onroadPriceCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="form-control-box padding-top30">
                <select id="ddlCity" data-bind="options: cities, optionsText: 'cityName', optionsValue: 'cityId', value: selectedCity, optionsCaption: 'Select City', chosen: { width: '190px' }"></select>
                <%--<input type="text" class="form-control" placeholder="Type to select city" id="orpCity">--%>
            </div>
            <div class="form-control-box padding-top30">
                <select id="ddlArea" data-bind="options: areas, optionsText: 'areaName', optionsValue: 'areaId', value: selectedArea, optionsCaption: 'Select Area', chosen: { width: '190px' }"></select>
                <%-- <input type="text" class="form-control" placeholder="Type to select area" id="orpArea">--%>
            </div>
            <input type="button" value="Confirm" class="btn btn-orange margin-top40" id="onroadPriceConfirmBtn">
        </div>

        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <!-- #include file="/includes/footerBW.aspx" -->
        
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= !string.IsNullOrEmpty(staticUrl) ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/model-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/model.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript">
            ga_pg_id = '2';

            // Cache selectors outside callback for performance.
            var leadSourceId;

            var getCityArea = GetGlobalCityArea();
            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
                if ($('#getOffersPrimary').length>0)
                    $('#getOffersPrimary').attr('v',bikeVersionLocation) ;
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
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
                        "sourceType": 1,
                        "pQLeadId": pqSourceId,
                        "deviceId": getCookie('BWC')
                    };

                    isSuccess = dleadvm.registerPQ(objData);

                    if (isSuccess) {
                        var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + dleadvm.pqId() + "&VersionId=" + versionId + "&DealerId=" + dealerID;
                        window.location.href = "/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
                    }
                } catch (e) {
                    console.warn("Unable to create pricequote : " + e.message);
                }
            }
            function openLeadCaptureForm(dealerID) {
                triggerGA('Dealer_PQ', 'Secondary_Dealer_Get_Offers_Clicked', bikeVersionLocation);
                event.stopPropagation();
            }
            $(function () {
                if ($('.dealership-benefit-list li').length <= 2) {
                    $('.dealership-benefit-list').addClass("dealer-two-offers");
                }
            });
           
            $('#getEmailID').on("focus", function () {
                $('#assistGetEmail').parent().addClass('not-empty');
            });

            $('#getFullName').on("focus", function () {
                $('#assistGetName').parent().addClass('not-empty');
            });
            
            $('#getMobile').on("focus", function () {
                $('#assistGetMobile').parent().addClass('not-empty');
            });
            $('#getEmailID').on("blur", function () {
                if ($('#assistGetEmail').val()=="")
                    $('#assistGetEmail').parent().removeClass('not-empty');
            });

            $('#getFullName').on("blur", function () {
                if ($('#assistGetName').val() == "")
                    $('#assistGetName').parent().removeClass('not-empty');
            });

            $('#getMobile').on("blur", function () {
                if ($('#assistGetMobile').val() == "")
                    $('#assistGetMobile').parent().removeClass('not-empty');
            });

            $(".leadcapturebtn").click(function (e) {
                ele = $(this);
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
                    "gaobject": {
                        cat: ele.attr("c"),
                        act: ele.attr("a"),
                        lab: bikeVersionLocation
                    }

                };
                dleadvm.setOptions(leadOptions);
            });
        </script>

        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="versions.aspx.cs" Inherits="Bikewale.New.bikeModel" EnableViewState="false" Trace="false" %>
<%@ Register Src="~/controls/NewAlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%--<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>--%>
<%@ Register Src="~/controls/News.ascx" TagName="LatestNews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewUserReviewsList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register Src="~/controls/PriceInTopCities.ascx" TagPrefix="BW" TagName="TopCityPrice" %>
<!doctype html>
<html>
<head>
    <%
		var modDetails = modelPageEntity.ModelDetails;
        title = String.Format("{0} Price in India, Review, Mileage & Photos - Bikewale", bikeName);
		description = String.Format("{0} Price in India - Rs. {1}. Check out {0} on road price, reviews, mileage, versions, news & photos at Bikewale.com", bikeName, Bikewale.Utility.Format.FormatPrice(price.ToString()));
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/{1}/", modelPageEntity.ModelDetails.MakeBase.MaskingName, modelPageEntity.ModelDetails.MaskingName);
		AdId = "1017752";
		AdPath = "/1017752/Bikewale_NewBike_";
		TargetedModel = modDetails.ModelName;
		fbTitle = title;
		alternate = "http://www.bikewale.com/m/" + modDetails.MakeBase.MaskingName + "-bikes/" + modDetails.MaskingName + "/";
		isAd970x90Shown = true;
		TargetedCity = cityName;
        keywords = string.Format("{0}, {0} Price, {0} Reviews, {0} Photos, {0} Mileage", bikeName);
        ogImage = modelImage; 
        isAd970x90BTFShown = true; %>
    <!-- #include file="/includes/headscript.aspx" -->
    <% isHeaderFix = false; %>
    <script type="text/javascript">
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
    <link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/model.css?<%= staticFileVersion %>12345" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="fa fa-angle-right margin-right10"></span>
                                <a href="/<%= modelPageEntity.ModelDetails.MakeBase.MaskingName %>-bikes/" itemprop="url">
                                    <span itemprop="title"><%= modelPageEntity.ModelDetails.MakeBase.MakeName %></span>
                                </a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>
                                <span><%= modelPageEntity.ModelDetails.ModelName %></span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div itemscope="" itemtype="http://auto.schema.org/Motorcycle" class="container" id="modelDetailsContainer">
                <span itemprop="name" class="hide"><%= bikeName %></span>
                <div class="grid-12 margin-bottom20">
                    <div class="content-inner-block-20 content-box-shadow">
                        <div class="grid-5 alpha">
                            <div class="position-rel <%= modelPageEntity.ModelDetails.Futuristic ? string.Empty : "hide" %>">
                                <%--<span class="model-sprite bw-upcoming-bike-ico bike-upcoming-tag position-abt"></span>--%>
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
                                                                <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._476x268) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="" border="0" />
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
                                                                <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" border="0" />
                                                            </span>
                                                        </div>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="grid-7 model-details-wrapper omega">
                            <div class="model-name-review-container">
                                <h1 class="font18 text-black text-bold"><%= bikeName %></h1>
                                <% if (!modelPageEntity.ModelDetails.Futuristic || modelPageEntity.ModelDetails.New)
								   { %>
                                <!-- Review & ratings -->
                                <div id="modelRatingsContainer" class="margin-top5 margin-bottom20 <%= modelPageEntity.ModelDetails.Futuristic ? "hide " : string.Empty %>">
                                    <% if (Convert.ToDouble(modelPageEntity.ModelDetails.ReviewRate) > 0)
									   { %>
                                    <p class="bikeModel-user-ratings leftfloat margin-right10">
                                        <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPageEntity.ModelDetails.ReviewRate)) %>
                                    </p>

                                    <span itemprop="aggregateRating" itemscope="" itemtype="http://schema.org/AggregateRating">
                                        <meta itemprop="ratingValue" content="<%=modelPageEntity.ModelDetails.ReviewRate %>">
                                        <meta itemprop="worstRating" content="1">
                                        <meta itemprop="bestRating" content="5">
                                        <a href="<%= FormatShowReview(modelPageEntity.ModelDetails.MakeBase.MaskingName,modelPageEntity.ModelDetails.MaskingName) %>" class="review-count-box font14 border-solid-left leftfloat margin-right20 padding-left10 ">
                                            <span itemprop="reviewCount">
                                                <%= modelPageEntity.ModelDetails.ReviewCount %>
                                            </span>Reviews
                                        </a>
                                    </span>
                                    <% }
									   else
									   { %>
                                    <p class="leftfloat margin-right20 font14">Not rated yet</p>
                                    <% } %>
                                    <a href="<%= FormatWriteReviewLink() %>" class="hide border-solid-left leftfloat margin-right10 padding-left10 font14 write-review-text">Write a review</a>
                                    <div class="clear"></div>
                                </div>
                                <!-- Review & ratings -->
                                <% } %>
                            </div>
                            <!-- Variants -->
                            <div id="variantDetailsContainer" class="variants-dropDown margin-top20 <%= modelPageEntity.ModelDetails.Futuristic ? "hide": string.Empty%>">
                                <div>
                                    <p class="variantText text-light-grey margin-right10">Version: </p>

                                    <% if (modelPageEntity.ModelVersions != null && modelPageEntity.ModelVersions.Count > 1)
									   { %>
                                    <div class="form-control-box variantDropDown">
                                        <div class="sort-div rounded-corner2">
                                            <div class="sort-by-title" id="sort-by-container">
                                                <span class="leftfloat sort-select-btn">
                                                    <asp:Label runat="server" ID="defaultVariant"></asp:Label>
                                                </span>
                                                <span class="clear"></span>
                                            </div>
                                            <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top5 pos-right10"></span>
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
									   { %>
                                    <p id='versText' class="variantText margin-right20"><%= variantText %></p>
                                    <% } %>
                                    <div class="clear"></div>
                                </div>


                                <%if (modelPageEntity.ModelVersionSpecs != null)
								  { %>
                                <ul class="variantList margin-top10 text-xt-light-grey">
                                    <%if (modelPageEntity.ModelVersionSpecs.Displacement != 0)
									  { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement) %></span>
                                        <span>cc</span>
                                    </li>
                                    <% } %>
                                    <%if (modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall != 0)
									  { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall) %></span>
                                        <span>kmpl</span>
                                    </li>
                                    <% } %>
                                    <%if (modelPageEntity.ModelVersionSpecs.MaxPower != 0)
									  { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower) %></span>
                                        <span>bhp</span>
                                    </li>
                                    <%} %>
                                    <%if (modelPageEntity.ModelVersionSpecs.KerbWeight != 0)
									  { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight) %></span>
                                        <span>kg</span>
                                    </li>
                                    <%} %>
                                </ul>
                                <div class="clear"></div>
                                <%} %>
                            </div>
                            <!-- Variant div ends -->
                            <% if (!modelPageEntity.ModelDetails.Futuristic)
							   { %>
                            <div id="modelPriceContainer" class="padding-top15">
                                <% if (isDiscontinued)
								   { %>
                                <p class="font14 text-light-grey">Last known Ex-showroom price</p>
                                <% } %>
                                <% else if (!isCitySelected)
								   {%>
                                <p class="font14 text-light-grey">Ex-showroom price in <span class="font14 text-default"><%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>
                                <% } %>
                                <% else if (!isOnRoadPrice)
								   {%>
                                <p class="font14 text-light-grey">Ex-showroom price in <span><span class="font14 text-default city-area-name"><%= areaName %> <%= cityName %></span></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>
                                <% } %>
                                <% else
								   {%>
                                <p class="font14 text-light-grey">On-road price in<span><span class="city-area-name"><%= areaName %> <%= cityName %></span></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData changeCity"><span class="bwsprite loc-change-blue-icon"></span></a></p>

                                <% } %>
                                <%  if (price == 0)
									{ %>
                                <span class="font32">Price not available</span>
                                <%  }
									else
									{ %>
                                <div class="leftfloat margin-top5 margin-right15 <%= (isBookingAvailable && isDealerAssitance) ? "model-price-book-now-wrapper" : string.Empty %> " itemprop="offers" itemscope itemtype="http://schema.org/Offer">
                                    <span itemprop="priceCurrency" content="INR">
                                        <span class="font20"><span class="fa fa-rupee"></span></span>
                                    </span>
                                    <span id="new-bike-price" class="font22" itemprop="price" content="<%=price %>"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                    <%if (isOnRoadPrice)
									  {%>
                                    <span id="viewBreakupText" class="font14 text-bold viewBreakupText">View detailed price</span>
                                    <br>
                                    <% } %>
                                </div>
                                <%  } %>
                                <%if (isBookingAvailable && isDealerAssitance) { %>
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
                                <a id="btnGetOnRoadPrice" href="javascript:void(0)" ismodel="true" modelid="<%=modelId %>" class="btn btn-orange margin-top10 fillPopupData">Check On-Road Price</a>
                                <div class="clear"></div>
                                
                                <% } %>
                            </div>

                            <% if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ)
                               { %>
                            <a href="javascript:void(0)" id="getassistance" leadSourceId="12" class="btn btn-orange margin-top10 margin-right10 leftfloat">Get offers from dealer</a>
                            <div class="leftfloat margin-top10">
                                <span class="font12 text-light-grey">Powered by</span><br />
                                <span class="font14"><%= viewModel.Organization %></span>
                            </div>
                            <div class="clear"></div>
                             <%  }
                                } %>
                            <% if (!toShowOnRoadPriceButton && isBikeWalePQ)
							   { %>
                            <div class="insurance-breakup-text text-bold padding-top10" >
                                <a target="_blank" id="insuranceLink" href="/insurance/">Save up to 60% on insurance - PolicyBoss</a>
                            </div>
                            <% } %>
                            <!-- upcoming start -->
                            <% if (modelPageEntity.ModelDetails.Futuristic && modelPageEntity.UpcomingBike != null)
							   { %>
                            <div id="upcoming">
                                <% if (modelPageEntity.UpcomingBike.EstimatedPriceMin != 0 && modelPageEntity.UpcomingBike.EstimatedPriceMax != 0)
								   { %>
                                <div id="expectedPriceContainer" class="padding-top15">
                                    <p class="font14 default-showroom-text text-light-grey">Expected Price</p>
                                    <div class="modelExpectedPrice margin-bottom15">
                                        <span class="font28"><span class="fa fa-rupee"></span></span>
                                        <span id="bike-price" class="font32">
                                            <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMin)) %></span>
                                            <span>- </span>
                                            <span class="font28"><span class="fa fa-rupee"></span></span>
                                            <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMax)) %></span>
                                        </span>
                                    </div>
                                </div>
                                <%}
								   else
								   { %>
                                <p class="font30 default-showroom-text text-light-grey margin-bottom5">Price Unavailable</p>
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
                        <div class="clear"></div>
                        <% if (viewModel!= null && viewModel.IsPremiumDealer && !isBikeWalePQ)
                           { %>
                        <div id="dealerDetailsWrapper" class="border-light margin-top20">
                            <div class="padding-top20 padding-right20 padding-left20">
                                <div class="border-light-bottom padding-bottom20">
                                    <h3 class="font18 text-darker-black leftfloat margin-right20"><%= viewModel.Organization %>, <%=viewModel.AreaName %></h3>
                                    <p class="leftfloat text-bold font16 position-rel pos-top2"><span class="fa fa-phone"></span> <%=viewModel.MaskingNumber %></p>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <% if (viewModel.Offers != null && viewModel.OfferCount > 0)
                               { %>
                            <div class="font14 content-inner-block-20">
                                <p class="text-bold margin-bottom10">Exclusive offers on this bike from <%=viewModel.Organization %>, <%=viewModel.AreaName %>:</p>
                                <ul class="dealership-benefit-list">
                                    <asp:Repeater ID="rptOffers" runat="server">
                                        <ItemTemplate>
		                                    <li>
                                                <span class="benefit-list-image offer-benefit-sprite offerIcon_<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "OfferCategoryId"))%>"></span>
                                                <span class="benefit-list-title"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText"))%></span>
		                                    </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div class="clear"></div>
                            </div>
                            <% } %>
                            <div id="dealerAssistance">
                            <div id="buyingAssistance" class="bg-light-grey font14 content-inner-block-20">
                                <p class="text-bold margin-bottom20">Get assistance on buying this bike:</p>
                                <div>
                                    <div class="form-control-box form-control-username leftfloat margin-right20">
                                        <input type="text" class="form-control" placeholder="Name" id="assistGetName" data-bind="textInput: fullName" />
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <div class="form-control-box form-control-email-mobile leftfloat margin-right20">
                                        <input type="text" class="form-control" placeholder="Email id" id="assistGetEmail" data-bind="textInput: emailId" />
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <div class="form-control-box form-control-email-mobile leftfloat margin-right20">
                                        <p class="mobile-prefix">+91</p>
                                        <input type="text" class="form-control padding-left40" maxlength="10" placeholder="Number" id="assistGetMobile" data-bind="textInput: mobileNo" />
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <a class="btn btn-inv-grey leftfloat" leadSourceId="13" id="assistFormSubmit" data-bind="event: { click: submitLead }">Submit</a>
                                    <div class="clear"></div>
                                </div>
                            </div>
                                        <!-- lead capture popup start-->
                                <div id="leadCapturePopup" class="text-center rounded-corner2">
                                    <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                    <!-- contact details starts here -->
                                    <div id="contactDetailsPopup">
                                        <div class="icon-outer-container rounded-corner50">
                                            <div class="icon-inner-container rounded-corner50">
                                                <span class="bwsprite user-contact-details-icon margin-top25"></span>
                                            </div>
                                        </div>
                                        <p class="font20 margin-top25 margin-bottom10">Provide contact details</p>
                                        <p class="text-light-grey margin-bottom20">Dealership will get back to you with offers, EMI quotes, exchange benefits and much more!</p>
                                        <div class="personal-info-form-container">
                                            <div class="form-control-box personal-info-list">
                                                <input type="text" class="form-control get-first-name" placeholder="Name (mandatory)"
                                                    id="getFullName" data-bind="textInput: fullName">
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <div class="form-control-box personal-info-list">
                                                <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                                                    id="getEmailID" data-bind="textInput: emailId">
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <div class="form-control-box personal-info-list">
                                                <p class="mobile-prefix">+91</p>
                                                <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                                                    id="getMobile" maxlength="10" data-bind="textInput: mobileNo">
                                                <span class="bwsprite error-icon errorIcon"></span>
                                                <div class="bw-blackbg-tooltip errorText"></div>
                                            </div>
                                            <div class="clear"></div>
                                            <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                                        </div>                   
                                    </div>
                                    <!-- contact details ends here -->
                                    <!-- thank you message starts here -->
                                    <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                                        <div class="icon-outer-container rounded-corner50">
                                            <div class="icon-inner-container rounded-corner50">
                                                <span class="bwsprite user-contact-details-icon margin-top25"></span>
                                            </div>
                                        </div>
                                        <p class="font18 text-bold margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                                        <% if(viewModel!=null){ %>
                                        <p class="font16 margin-bottom40"><%=viewModel.Organization %>, <%=viewModel.AreaName %> will get in touch with you soon</p>
                                        <% } %>
                                        <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
                                    </div>
                                    <!-- thank you message ends here -->

                                    <!-- otp starts here -->
                                    <div id="otpPopup">
                                        <div class="icon-outer-container rounded-corner50">
                                            <div class="icon-inner-container rounded-corner50">
                                                <span class="bwsprite otp-icon margin-top25"></span>
                                            </div>
                                        </div>
                                        <p class="font18 margin-top25 margin-bottom20">Verify your mobile number</p>
                                        <p class="font14 text-light-grey margin-bottom20">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                                        <div>
                                            <div class="lead-mobile-box lead-otp-box-container font22">
                                                <span class="fa fa-phone"></span>
                                                <span class="text-light-grey font24">+91</span>
                                                <span class="lead-mobile font24"></span>
                                                <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                                            </div>
                                            <div class="otp-box lead-otp-box-container">
                                                <div class="form-control-box margin-bottom10">
                                                    <input type="text" class="form-control" maxlength="5" placeholder="Enter your OTP" id="getOTP" data-bind="value: otpCode">
                                                    <span class="bwsprite error-icon errorIcon"></span>
                                                    <div class="bw-blackbg-tooltip errorText"></div>
                                                </div>
                                                <a class="resend-otp-btn margin-left10 blue rightfloat resend-otp-btn" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP
                                                </a>
                                                <p class="otp-alert-text margin-left10 otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                                                    OTP has been already sent to your mobile
                                                </p>
                                                <div class="clear"></div>
                                                <%--<p class="resend-otp-btn margin-bottom20" id="resendCwiCode">Resend OTP</p>--%>
                                                <input type="button" class="btn btn-orange margin-top20" value="Confirm OTP" id="otp-submit-btn">
                                            </div>
                                            <div class="update-mobile-box">
                                                <div class="form-control-box text-left">
                                                    <p class="mobile-prefix">+91</p>
                                                    <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                                                    <span class="bwsprite error-icon errorIcon"></span>
                                                    <div class="bw-blackbg-tooltip errorText"></div>
                                                </div>
                                                <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                                            </div>
                                        </div>
                                    </div>
                                    <!-- otp ends here -->
                                </div>
                                <!-- lead capture popup End-->
                                </div>
                            <% if(isBookingAvailable && bookingAmt > 0){ %>
                            <div class="font14 text-light-grey content-inner-block-20">
                                <p>The booking amount of <span class="fa fa-rupee"></span> <%=bookingAmt %> has to be paid online and balance amount of <span class="fa fa-rupee"></span> <%= price-bookingAmt  %> has to be paid at the dealership. <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>">Book now</a></p>
                            </div>
                            <% } %>
                        </div>
                        <% } %>
                        <% if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ && viewModel.SecondaryDealerCount > 0)
                           { %>
                        <ul id="moreDealersList">
                            <asp:Repeater ID="rptSecondaryDealers" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <a href="javascript:void(0);" onclick="secondarydealer_Click(
                                            <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "DealerId")) %>)" class="font18 text-bold text-darker-black margin-right20 secondary"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Name")) %>, <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Area")) %></a>
                                        <span class="font16 text-bold"><span class="fa fa-phone"></span> <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MaskingNumber")) %></span>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        <% } %>
                        </ul>
                        <% if (viewModel!=null && !isBikeWalePQ && viewModel.SecondaryDealerCount > 0)
                           { %>
                        <div class="text-center margin-top20">
                            <a href="javascript:void(0)" class="font14 more-dealers-link">Check price from <%=viewModel.SecondaryDealerCount %> more dealers <span class="font12"><span class="fa fa-chevron-down"></span></span></a>
                            <a href="javascript:void(0)" class="font14 less-dealers-link">Show less dealers <span class="font12"><span class="fa fa-chevron-up"></span></span></a>
                        </div>
                        <%} %>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
           
            <!-- Terms and condition Popup start -->
            <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                <div class="fixed-close-btn-wrapper">
                    <div class="termsPopUpCloseBtn fixed-close-btn bwsprite cross-lg-lgt-grey cur-pointer"></div>
                </div>
                <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                    <%--<span class="fa fa-spinner fa-spin position-abt text-black bg-white" style="font-size: 50px"></span>--%>
                    <img class="lazy" data-original="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  src="" />

                </div>
                <div id="terms" class="breakup-text-container padding-bottom10 font14">
                </div>
                <div id='orig-terms' class='hide'>
                </div>
            </div>
            <!-- Terms and condition Popup Ends -->
        </section>       
        <section id="modelDetailsFloatingCardContent" class="container">
            <div class="grid-12">
                <div class="model-details-floating-card">
                    <div class="content-box-shadow content-inner-block-1020">
                        <div class="grid-5 alpha omega">
                            <div class="model-card-image-content inline-block-top margin-right20">
                                <img src="<%= modelImage %>" />
                            </div>
                            <div class="model-card-title-content inline-block-top">
                                <h2 class="font18 text-bold margin-bottom10"><%= bikeName %></h2>
                                <p class="font14 text-light-grey"><%= variantText %></p>
                            </div>
                        </div>
                        <div class="grid-4 padding-left30">
                                <%if(isDiscontinued){ %>
                                <p class="font14 text-light-grey margin-bottom5">Last known Ex-showroom price</p>
                                <%} else
                                 if (!isCitySelected)
								   {%>
                                <p class="font14 text-light-grey margin-bottom5"><span>Ex-showroom price in</span>&nbsp;<span class="font14 text-default text-truncate"><%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span></p>
                                <% } %>
                                <% else if (!isOnRoadPrice)
								   {%>
                                <p class="font14 text-light-grey margin-bottom5 leftfloat"><span class="leftfloat">Ex-showroom price in</span><span class="leftfloat text-default text-truncate city-area-name margin-right5"><%= areaName %> <%= cityName %></span></p>
                                <% } %>
                                <% else
								   {%>
                                <p class="font14 text-light-grey margin-bottom5 leftfloat"><span class="leftfloat">On-road price in</span><span class="leftfloat text-truncate city-area-name margin-right5"><%= areaName %> <%= cityName %></span></p>

                                <% } %>                          
                            <div class="clear"></div>
                            <div class="font16">
                                <span class="fa fa-rupee"></span> <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                            </div>
                        </div>
                        <div class="grid-3 model-orp-btn alpha omega">
                             <% if (toShowOnRoadPriceButton && !isDiscontinued)
                               { %>                            
                             <a href="javascript:void(0)" id="btnCheckOnRoadPriceFloating" ismodel="true" modelid="<%=modelId %>" class="btn btn-orange font14 <%=(viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) ? "margin-top5" : "margin-top20" %> fillPopupData bw-ga" rel="nofollow" c="Model_Page" a="Floating_Card_Check_On_Road_Price_Button_Clicked" v="myBikeName">Check On-Road Price</a>
                            <%} else
                                    if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ && !isDiscontinued)
                                    {%>									 
                                     <a href="javascript:void(0)" id="getOffersFromDealerFloating" leadSourceId="24" class="btn btn-orange font14 <%=(viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ) ? "margin-top5" : "margin-top20" %> bw-ga" rel="nofollow" c="Model_Page" a="Floating_Card_Get_Offers_Clicked" v="myBikeName">Get offers from dealer</a>
                                    <%} %>
                            
                            <!-- if no 'powered by' text is present remove margin-top5 add margin-top20 in offers button -->
                            <%if (viewModel != null && viewModel.IsPremiumDealer && !isBikeWalePQ)
                              { %>
                            <p class="model-powered-by-text font12 margin-top10 text-truncate"><span class="text-light-grey">Powered by </span><%= viewModel.Organization %></p>
                            <%} %>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div class="overall-specs-tabs-wrapper content-box-shadow">
                        <a class="active" href="#modelSummaryContent" rel="nofollow">Summary</a>
                        <a href="#modelPricesContent" rel="nofollow">Prices</a>
                        <a href="#modelSpecsFeaturesContent" rel="nofollow">Specs & Features</a>
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 || ctrlVideos.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelReviewsContent" rel="nofollow">Reviews</a>
                        <%} %>

                         <% if (ctrlNews.FetchedRecordsCount > 0)
                             { %>
                        <a href="#modelNewsContent" rel="nofollow">News</a><%} %>
                          <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0) { %>
                        <a href="#modelAlternateBikeContent" rel="nofollow">Alternatives</a>  
                        <%} %>                   
                    </div>
                </div>
            </div>
        </section>

        <section class="container">
            <div id="modelSpecsTabsContentWrapper" class="grid-12 margin-bottom20">
                <div class="content-box-shadow">
                    <div class="overall-specs-tabs-wrapper">
                        <a class="active" href="#modelSummaryContent" rel="nofollow">Summary</a>
                        <a href="#modelPricesContent" rel="nofollow">Prices</a>
                        <a href="#modelSpecsFeaturesContent" rel="nofollow">Specs & Features</a>
                        <% if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 || ctrlVideos.FetchedRecordsCount > 0)
                           { %>
                        <a href="#modelReviewsContent" rel="nofollow">Reviews</a>
                        <%} %>
                          <% if (ctrlNews.FetchedRecordsCount > 0)
                             { %>
                        <a href="#modelNewsContent" rel="nofollow">News</a>
                        <%} %>

                        <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0) { %>
                        <a href="#modelAlternateBikeContent" rel="nofollow">Alternatives</a> 
                        <%} %>                      
                    </div>
                    <div class="border-divider"></div>

                    <div id="modelSummaryContent" class="bw-model-tabs-data content-inner-block-20">
                        <%if(modelPageEntity.ModelDesc == null || string.IsNullOrEmpty(modelPageEntity.ModelDesc.SmallDescription)){ %>
                        <div class="grid-8 alpha margin-bottom20 container">
                            <h2><%=bikeName %></h2>
                            <h3>Preview</h3>
                            <p class="font14 text-light-grey line-height17">
                                <span class="model-preview-main-content">
                                    <%= modelPageEntity.ModelDesc.SmallDescription %>
                                </span>
                                <span class="model-preview-more-content hide" style="display: none;">
                                    <%= modelPageEntity.ModelDesc.FullDescription %>
                                </span>
                                <a href="javascript:void(0)" class="read-more-model-preview" rel="nofollow">Read more</a>
                            </p>
                        </div>
                        <div class="grid-4 text-center alpha omega margin-bottom20">
                            <!-- #include file="/ads/Ad300x250.aspx" -->
                        </div>
                        <%} %>
                        <div class="clear"></div>

                        <h3>Specification summary</h3>
                        <div class="grid-3 border-light-right omega">
                            <span class="inline-block model-sprite specs-capacity-icon margin-right30"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement) %><span> cc</span></p>
                                <p class="font16 text-light-grey">Capacity</p>
                            </div>
                        </div>
                        <div class="grid-3 padding-left40 border-light-right omega">
                            <span class="inline-block model-sprite specs-mileage-icon margin-right30"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall) %><span> kmpl</span></p>
                                <p class="font16 text-light-grey">Mileage</p>
                            </div>
                        </div>
                        <div class="grid-3 padding-left60 border-light-right omega">
                            <span class="inline-block model-sprite specs-maxpower-icon margin-right30"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower) %><span class="text-uppercase"> bhp</span></p>
                                <p class="font16 text-light-grey">Max power</p>
                            </div>
                        </div>
                        <div class="grid-3 padding-left50 omega">
                            <span class="inline-block model-sprite specs-weight-icon margin-right30"></span>
                            <div class="inline-block">
                                <p class="font22 text-bold margin-bottom5"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight) %><span> kgs</span></p>
                                <p class="font16 text-light-grey">Weight</p>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div class="margin-right10 margin-left10 border-solid-top"></div> <!-- divider -->

                    <div id="modelPricesContent" class="bw-model-tabs-data content-inner-block-21522">
                        <h2><%=bikeName %> Prices</h2>
                        <div class="grid-8 alpha">
                            <h3 class="margin-bottom20">Prices by versions</h3>
                            <div class="jcarousel-wrapper">
                                <div class="jcarousel">
                                    <ul>
                                       <asp:Repeater runat="server" ID="rptVarients" OnItemDataBound="rptVarients_ItemDataBound">
	                                        <ItemTemplate>
                                                <li class="rounded-corner2">
                                                    <p class="text-bold text-truncate margin-bottom15"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionName")) %></p>
                                                    <p class="text-truncate text-xt-light-grey margin-bottom15"><%# FormatVarientMinSpec(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")),Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                                    <p class="text-truncate text-light-grey margin-bottom10">
                                                        <asp:Label ID="lblExOn" Text="Ex-showroom price" runat="server"></asp:Label>, 
                                                        <% if (cityId != 0)
												           { %>
                                                       <span><%= areaName %> <%= cityName %></span>
                                                        <% }
												           else
												           { %>
                                                        <span><%= Bikewale.Common.Configuration.GetDefaultCityName %></span>
                                                        <% } %>                                                   
                                                    </p>
                                                    <p class="font18 text-bold text-black">
                                                        <span class="fa fa-rupee"></span>
                                                        <span><asp:Label Text='<%#Eval("Price") %>' ID="txtComment" runat="server"></asp:Label></span>
                                                    </p>
                                                    <asp:HiddenField ID="hdnVariant" runat="server" Value='<%#Eval("VersionId") %>' />
                                                </li>
	                                        </ItemTemplate>
                                        </asp:Repeater>                                  
                                    </ul>                                      
                                </div>
                                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                                <p class="jcarousel-pagination text-center"></p>
                            </div>
                        </div>
                       <BW:TopCityPrice ID="ctrlTopCityPrices" runat="server" />
                        <div class="clear"></div>
                    </div>

                    <div class="margin-right10 margin-left10 border-solid-top"></div> <!-- divider -->

                    <div id="modelSpecsFeaturesContent" class="bw-model-tabs-data padding-top20 font14">
                        <h2 class="padding-left20 padding-right20"><%=bikeName %> Specifications & Features</h2>
                        <h3 class="padding-left20">Specifications</h3>
                        <div class="grid-12 alpha omega">
                            <div class="grid-4 alpha">
                                <div class="grid-6 padding-left20 text-light-grey">
                                    <p>Displacement</p>
                                    <p>Max Power</p>
                                    <p>Maximum Torque</p>
                                    <p>No. of gears</p>
                                </div>
                                <div class="grid-6 omega text-bold">
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Displacement,"cc") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.MaxPower, "bhp", modelPageEntity.ModelVersionSpecs.MaxPowerRPM, "rpm") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable( modelPageEntity.ModelVersionSpecs.MaximumTorque, "Nm", modelPageEntity.ModelVersionSpecs.MaximumTorqueRPM,"rpm") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.NoOfGears) %></p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-4">
                                <div class="grid-6 padding-left20 text-light-grey">
                                    <p>Mileage</p>
                                    <p>Brake Type</p>
                                    <p>Front Disc</p>
                                    <p>Rear Disc</p>
                                </div>
                                <div class="grid-6 omega text-bold">
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall,"kmpl") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.BrakeType) %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FrontDisc) %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.RearDisc) %></p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-4 omega">
                                <div class="grid-6 padding-left20 text-light-grey">
                                    <p>Alloy Wheels</p>
                                    <p>Kerb Weight</p>
                                    <p>Top Speed</p>
                                    <p>Fuel Tank Capacity</p>
                                </div>
                                <div class="grid-6 omega text-bold">
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.AlloyWheels) %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.KerbWeight,"kg") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.TopSpeed, "kmph") %></p>
                                    <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelTankCapacity, "litres") %></p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                            <div class="margin-top25 padding-left20">
                                <a href="<%# Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(modelPageEntity.ModelDetails.MakeBase.MaskingName, modelPageEntity.ModelDetails.MaskingName, "specs") %>" class="bw-ga" c="Model_Page" a="View_full_specifications_link_cliked" v="myBikeName">View full specifications<span class="bwsprite blue-right-arrow-icon" ></span></a>
                            </div>
                        </div>
                        <div class="clear"></div>
                        
                        <div class="grid-8 alpha margin-top25 margin-bottom25">
                            <h3 class="padding-left20">Features</h3>
                            <div class="grid-12 alpha omega">
                                <div class="grid-6 alpha">
                                    <div class="grid-6 padding-left20 text-light-grey">
                                        <p>Speedometer</p>
                                        <p>Fuel Guage</p>
                                        <p>Tachometer Type</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Speedometer) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.FuelGauge) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.DigitalFuelGauge) %></p>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="margin-top25 padding-left20">
                                        <a href="<%# Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(modelPageEntity.ModelDetails.MakeBase.MaskingName, modelPageEntity.ModelDetails.MaskingName, "features") %>" class="bw-ga" c="Model_Page" a="View_all_features_link_cliked" v="myBikeName">View all features<span class="bwsprite blue-right-arrow-icon"></span></a>
                                    </div>
                                </div>
                                <div class="grid-6">
                                    <div class="grid-6 padding-left20 text-light-grey">
                                        <p>Digital Fuel Guage</p>
                                        <p>Tripmeter</p>
                                        <p>Electric Start</p>
                                    </div>
                                    <div class="grid-6 omega text-bold">
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.DigitalFuelGauge) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.Tripmeter) %></p>
                                        <p><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPageEntity.ModelVersionSpecs.ElectricStart) %></p>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div id="modelFeaturesAd" class="grid-4 omega text-center">                           
                            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                        </div>
                        <div class="clear"></div>                      
                         
                        <%if (modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                          { %>
                         <%--<div class="grid-12 alpha omega">--%>
                            <h3 class="padding-left20">Colours</h3>
                            <ul id="modelColorsList">
                                <asp:Repeater ID="rptColor" runat="server">
                                    <ItemTemplate>
                                        <li>                                                                                                                                     
                                            <div class="color-box inline-block <%# (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count == 1 )?"color-count-one": (((IList)(DataBinder.Eval(Container.DataItem, "HexCodes"))).Count >= 3 )?"color-count-three":"color-count-two" %>">
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
                         <%--</div>--%>
                        <%} %>
                        <%--<div class="clear"></div>--%>
                    </div>
                      <%if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 || ctrlVideos.FetchedRecordsCount > 0)
                       { %>
                        <div class="margin-right10 margin-left10 border-solid-top"></div>
                      <%} %>
                    <div id="modelReviewsContent" class="bw-model-tabs-data font14">
                        <%if (ctrlExpertReviews.FetchedRecordsCount > 0 || ctrlUserReviews.FetchedRecordsCount > 0 || ctrlVideos.FetchedRecordsCount > 0)
                          { %>
                        <h2 class="padding-top20 padding-right20 padding-left20"><%= bikeName %> Reviews</h2>
                        <%} %>
                        <% if(ctrlExpertReviews.FetchedRecordsCount > 0){ %>
                        <!-- expert review starts-->
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <!-- expert review ends-->
                        <% } %>

                        <% if (ctrlUserReviews.FetchedRecordsCount > 0){ %>
                        <!-- user reviews -->
                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                        <!-- user reviews ends -->
                         <% } %>

                        <% if (ctrlVideos.FetchedRecordsCount > 0)
                           { %>
                        <!-- Video reviews -->
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <!-- Video reviews ends -->
                        <% } %>
                    </div>
                    <% if (ctrlNews.FetchedRecordsCount > 0)
                       { %>
                    <!-- News widget starts -->                    
                    <BW:LatestNews runat="server" ID="ctrlNews" />
                    <!-- News widget ends --> 
                    <% } %>  

                    <% if (ctrlAlternativeBikes.FetchedRecordsCount > 0) { %>
                    <!-- Alternative reviews ends -->
                    <div class="margin-top20 margin-right10 margin-left10 border-solid-top"></div>
                    <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                    <!-- Alternative reviews ends -->
                    <% } %>   
                    <div class="margin-top20 margin-right10 margin-left10 border-solid-top"></div>                
                    <div id="overallSpecsDetailsFooter"></div>
                </div>
            </div>
            <div class="clear"></div>
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

        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/model.js?<%= staticFileVersion %>">"></script>
        <%--<link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/brand.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">--%>

        <script type="text/javascript">
            ga_pg_id = '2';
            var modelPriceByVersionSlider = 2;
            // Cache selectors outside callback for performance.
            var leadSourceId;
            var getCityArea = GetGlobalCityArea();
            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
            function secondarydealer_Click(dealerID) {
                var rediurl = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerID;
                window.location.href = "/pricequote/dealerpricequote.aspx?MPQ=" + Base64.encode(rediurl);
            }
            $(function () {
                if ($('.dealership-benefit-list li').length % 2 == 0) {
                    $('.dealership-benefit-list').addClass("dealer-two-offers");
                }
            });
        </script>
    </form>
</body>
</html>

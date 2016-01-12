﻿<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="versions.aspx.cs" Inherits="Bikewale.New.bikeModel" Trace="false" %>

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
        var modDetails = modelPage.ModelDetails;
        title = modDetails.MakeBase.MakeName + " " + modDetails.ModelName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = modDetails.MakeBase.MakeName + " " + modDetails.ModelName + " Price in India - Rs."
                    + Bikewale.Utility.Format.FormatPrice(modDetails.MinPrice.ToString()) + " - " + Bikewale.Utility.Format.FormatPrice(modDetails.MaxPrice.ToString())
                    + ". Check out " + modDetails.MakeBase.MakeName + " " + modDetails.ModelName + " on road price, reviews, mileage, versions, news & photos at Bikewale.";

        canonical = "http://www.bikewale.com/" + modDetails.MakeBase.MaskingName + "-bikes/" + modDetails.MaskingName + "/";
        AdId = "1017752";
        AdPath = "/1017752/Bikewale_NewBike_";
        TargetedModel = modDetails.ModelName;
        fbTitle = title;
        alternate = "http://www.bikewale.com/m/" + modDetails.MakeBase.MaskingName + "-bikes/" + modDetails.MaskingName + "/";
        isAd970x90Shown = true;
    %>

    <% isAd970x90BTFShown = true; %>
    <!-- #include file="/includes/headscript.aspx" -->
    <% isHeaderFix = false; %>
    <script type="text/javascript">
        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var versionId = '<%= variantId%>';
        var cityId = '<%= cityId%>';
        var clientIP = "<%= clientIP%>";
        var pageUrl = "www.bikewale.com/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;
        var bikeVersionLocation = '';
        var bikeVersion = '';
        var isBikeWalePq = "<%= isBikeWalePQ%>"; 
        var areaId = "<%= areaId %>";
</script>
    <link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/model.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <style>
        .chosen-results::-webkit-scrollbar {
            width: 10px;
            border-radius: 5px;
        }

        .chosen-results::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 2px rgba(0,0,0,0.2);
        }

        .chosen-results::-webkit-scrollbar-thumb {
            background-color: rgba(204, 204, 204,0.7);
        }
    </style>
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
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/<%= modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/"><%= modelPage.ModelDetails.MakeBase.MakeName %></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= modelPage.ModelDetails.ModelName %></li>
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
                    <div class="content-inner-block-20 content-box-shadow">
                        <div class="grid-5 alpha">
                            <div class="position-rel <%= modelPage.ModelDetails.Futuristic ? string.Empty : "hide" %>">
                                <span class="model-sprite bw-upcoming-bike-ico bike-upcoming-tag position-abt"></span>
                            </div>
                            <div class="position-rel <%= !modelPage.ModelDetails.Futuristic && !modelPage.ModelDetails.New ? string.Empty : "hide" %>">
                                <span class="model-sprite bw-discontinued-bike-ico bike-discontinued-tag position-abt"></span>
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
                                    <a href="#" class="prev prev-stage bwsprite"></a>
                                    <a href="#" class="next next-stage bwsprite"></a>
                                </div>

                                <div class="navigation">
                                    <a href="#" class="prev prev-navigation bwsprite"></a>
                                    <a href="#" class="next next-navigation bwsprite"></a>
                                    <div class="carousel carousel-navigation">
                                        <ul>
                                            <asp:Repeater ID="rptNavigationPhoto" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <div class="carousel-nav-img-container">
                                                            <span>
                                                                <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>" title="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" alt="<%# bikeName + ' ' + DataBinder.Eval(Container.DataItem, "ImageCategory").ToString() %>" src="http://img.aeplcdn.com/bikewaleimg/images/loader.gif" border="0" />
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
                                <p class="font24 text-black text-bold"><%= bikeName %></p>
                                <% if (!modelPage.ModelDetails.Futuristic || modelPage.ModelDetails.New)
                                   { %>
                                <!-- Review & ratings -->
                                <div id="modelRatingsContainer" class="margin-top5 padding-bottom10 <%= modelPage.ModelDetails.Futuristic ? "hide " : string.Empty %>">
                                    <% if (Convert.ToDouble(modelPage.ModelDetails.ReviewRate) > 0)
                                       { %>
                                    <p class="bikeModel-user-ratings leftfloat margin-right10">
                                        <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(modelPage.ModelDetails.ReviewRate)) %>
                                    </p>
                                    <a href="<%= FormatShowReview(modelPage.ModelDetails.MakeBase.MaskingName,modelPage.ModelDetails.MaskingName) %>" class="review-count-box font14 border-solid-left leftfloat margin-right20 padding-left10 "><%= modelPage.ModelDetails.ReviewCount %> Reviews
                                    </a>
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
                            <div id="variantDetailsContainer" class="variants-dropDown margin-top15 padding-bottom15 <%= modelPage.ModelDetails.Futuristic ? "hide": string.Empty%>">
                                <div>
                                    <p class="variantText text-light-grey margin-right10">Version: </p>

                                    <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 1)
                                       { %>
                                    <div class="form-control-box variantDropDown">
                                    <div class="sort-div rounded-corner2">
                                        <div class="sort-by-title" id="sort-by-container">
                                            <span class="leftfloat sort-select-btn">
                                                <asp:Label runat="server" ID="defaultVariant"></asp:Label>
                                            </span>
                                            <span class="clear"></span>
                                        </div>
                                        <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                    </div>
                                    <div class="sort-selection-div sort-list-items hide">
                                        <ul id="sortbike">
                                            <asp:Repeater ID="rptVariants" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <asp:Button Style="width: 100%; text-align: left" ID="btnVariant" ToolTip='<%#Eval("VersionId") %>'  OnCommand="btnVariant_Command" versionid='<%#Eval("VersionId") %>' CommandName='<%#Eval("VersionId") %>' CommandArgument='<%#Eval("VersionName") %>' runat="server" Text='<%#Eval("VersionName") %>'></asp:Button></li>
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
                                    <p id='versText' class="variantText text-light-grey margin-right20 text-bold"><%= variantText %></p>
                                    <% } %>
                                    <div class="clear"></div>
                                </div>
                                <%--<div class="sort-div rounded-corner2">
                                    <div class="sort-by-title" id="sort-by-container">
                                        <span class="leftfloat sort-select-btn">Popular</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
                                </div>
                                <div class="sort-selection-div sort-list-items">
                                    <ul id="sortbike1">
                                        <li id="0" class="selected">Popular</li>
                                        <li id="1">Price: Low to High</li>
                                        <li id="2">Price: High to Low</li>
                                        <li id="3">Mileage: High to Low</li>
                                    </ul>
                                </div>--%>


                                <%if (modelPage.ModelVersionSpecs != null)
                                  { %>
                                <ul class="variantList margin-top15">
                                    <%if (modelPage.ModelVersionSpecs.Displacement != 0)
                                    { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %></span>
                                        <span>cc</span>
                                    </li>
                                    <% } %>
                                    <%if (modelPage.ModelVersionSpecs.FuelEfficiencyOverall != 0)
                                    { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelEfficiencyOverall) %></span>
                                        <span>kmpl</span>
                                    </li>
                                    <% } %>
                                    <%if (modelPage.ModelVersionSpecs.MaxPower != 0)
                                    { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.MaxPower) %></span>
                                        <span>bhp</span>
                                    </li>
                                    <%} %>
                                    <%if (modelPage.ModelVersionSpecs.KerbWeight != 0)
                                    { %>
                                    <li>
                                        <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.KerbWeight) %></span>
                                        <span>kg</span>
                                    </li>
                                     <%} %>
                                </ul>
                                <div class="clear"></div>
                                <%} %>
                            </div>
                            <!-- Variant div ends -->
                            <% if (!modelPage.ModelDetails.Futuristic)
                               { %>
                            <div id="modelPriceContainer" class="padding-top15">
                                <% if (isDiscontinued)
                                   { %>
                                        <p class="font14 text-light-grey">Last known Ex-showroom price</p>
                                <% } %>
                                 <% else if( !isCitySelected) {%>
                                        <p class="font14">Ex-showroom price in <span class="font14 text-grey"><%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData"><span class="bwsprite edit-blue-icon"></span></a></p>
                                <% } %>
                                <% else if( !isOnRoadPrice) {%>
                                        <p class="font14">Ex-showroom price in <span><span class="font16 text-grey city-area-name"><%= areaName %> <%= cityName %></span></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData"><span class="bwsprite edit-blue-icon"></span></a></p>
                                <% } %>
                                <% else {%>
                                        <p class="font14">On-road price in <span><span class="font16 text-grey city-area-name"><%= areaName %> <%= cityName %></span></span><a ismodel="true" modelid="<%=modelId %>" class="margin-left5 fillPopupData"><span class="bwsprite edit-blue-icon"></span></a></p>
                                <% } %>
                                <%--<% else if (pqOnRoad !=null && pqOnRoad.IsDealerPriceAvailable)
                                   { %>
                                <p class="font14">On-road Price in <span class="viewBreakupText"><span class="font16 text-grey text-bold"><%= areaName %> <%= cityName %></span></span><a ismodel="true" modelid="<%=modelId %>" class="fa fa-edit viewBreakupText fillPopupData"></a></p>
                                <% } %>

                                <% else if (!isCitySelected)
                                   { %>
                                        <p class="font14">Ex-showroom price in <span href="javascript:void(0)" class="font14 text-grey"><%= ConfigurationManager.AppSettings["defaultName"] %></span><a ismodel="true" modelid="<%=modelId %>" class="fa fa-edit viewBreakupText fillPopupData"></a></p>
                                <% }
                                   else if (!isAreaAvailable)
                                   { %>
                                        <p class="font14">On road Price in<span href="javascript:void(0)" class="font14 text-grey"> <%= cityName %></span><a ismodel="true" modelid="<%=modelId %>" class="fa fa-edit viewBreakupText fillPopupData"></a></p>
                                <% }
                                   else if (isAreaAvailable && !isAreaSelected)
                                   { %>
                                        <p class="font14">Ex-showroom Price in<span href="javascript:void(0)" class="font14 text-grey"> <%= cityName %></span><a ismodel="true" modelid="<%=modelId %>" class="fa fa-edit viewBreakupText fillPopupData"></a></p>
                                <% }
                                   else
                                   { %>
                                        <p class="font14">On road Price in<span href="javascript:void(0)" class="font14 text-grey"> <%= cityName %></span><a ismodel="true" modelid="<%=modelId %>" class="fa fa-edit viewBreakupText fillPopupData"></a></p>
                                <% } %>--%>
                                <div class="modelPriceContainer">
                                    <%  if (price == "" || price == "0")
                                        { %>
                                    <span class="font32">Price not available</span>
                                    <%  }
                                        else
                                        { %>
                                    <span class="font28"><span class="fa fa-rupee"></span></span>
                                    <span id="new-bike-price" class="font32"><%= Bikewale.Utility.Format.FormatPrice(price) %></span>
                                    <%  } %>
                                    <%if (isOnRoadPrice)
                                      {%>
                                    <span id="viewBreakupText" class="font14 text-light-grey viewBreakupText">View Breakup</span>
                                    <br>
                                    <%if (isBikeWalePQ && price != "")
                                      {%>
                                        <span class="font12 text-xt-light-grey">(Ex-showroom + Insurance (comprehensive) + RTO)</span>
                                    <%}
                                      else
                                      { %>
                                         <span class="font12 text-xt-light-grey"><%=viewbreakUpText %></span>
                                    <%} %>
                                 <% } %>
                                </div>
                                <% if (isDiscontinued)
                                   { %>
                                <p class="default-showroom-text font14 text-light-grey margin-top5"><%= bikeName %> is now discontinued in India.</p>
                                <% } %>
                                <% 
                                else 
                                if(toShowOnRoadPriceButton)
                                   { %>
                                <a id="btnGetOnRoadPrice" href="javascript:void(0)" ismodel="true" modelid="<%=modelId %>" class="btn btn-orange margin-top10 fillPopupData">Get on road price</a>
                                <% } %>
                            </div>

                            <% } %>
                            <% if (!toShowOnRoadPriceButton && isBikeWalePQ)
                                   { %>
                             <div class="insurance-breakup-text text-bold padding-top10" style="position: relative; color: rgb(153, 153, 153); font-size: 14px; margin-top: 1px; text-decoration:solid">
                                <a target="_blank" id="insuranceLink" href="/insurance/">Save up to 60% on insurance - PolicyBoss</a>
                            </div>
                               <% } %>
                            <!-- upcoming -->
                            <% if (modelPage.ModelDetails.Futuristic && modelPage.UpcomingBike != null)
                               { %>
                            <div id="upcoming">
                                <% if (modelPage.UpcomingBike.EstimatedPriceMin != 0 && modelPage.UpcomingBike.EstimatedPriceMax != 0)
                                   { %>
                                <div id="expectedPriceContainer" class="padding-top15">
                                    <p class="font14 default-showroom-text text-light-grey">Expected Price</p>
                                    <div class="modelExpectedPrice margin-bottom15">
                                        <span class="font28"><span class="fa fa-rupee"></span></span>
                                        <span id="bike-price" class="font32">
                                            <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %></span>
                                            <span>- </span>
                                            <span class="font28"><span class="fa fa-rupee"></span></span>
                                            <span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                                        </span>
                                    </div>
                                </div>
                                <%}
                                   else
                                   { %>
                                <p class="font30 default-showroom-text text-light-grey margin-bottom5">Price Unavailable</p>
                                <% } %>
                                <% if (!string.IsNullOrEmpty(modelPage.UpcomingBike.ExpectedLaunchDate))
                                   { %>
                                <div id="expectedDateContainer" class="padding-top15 font14">
                                    <p class="default-showroom-text text-light-grey margin-bottom10">Expected launch date</p>
                                    <p class="modelLaunchDate text-bold font18 margin-bottom10"><%= modelPage.UpcomingBike.ExpectedLaunchDate %></p>
                                    <p class="default-showroom-text text-light-grey"><%= bikeName %> is not launched in India yet.</p>
                                    <p class="text-light-grey">Information on this page is tentative.</p>
                                </div>
                                <%} %>
                            </div>
                            <% } %>
                        </div>
                        <div class="clear"></div>
                        <%if (pqOnRoad != null && pqOnRoad.IsDealerPriceAvailable)
                          { %>
                        <div id="modelDetailsOffersContainer" class="grid-12 margin-top20">
                            <div class="grid-<%=grid1_size %> modelGetDetails padding-right20">
                                <h3 class="padding-bottom10"><span class="bwsprite disclaimer-icon margin-right5"></span>Get following details on this bike:</h3>
                                <ul>
                                    <li>Offers from the nearest dealers</li>
                                    <li>Waiting period on this bike at the dealership</li>
                                    <li>Nearest dealership from your place</li>
                                    <li>Finance options on this bike</li>
                                </ul>
                            </div>

                            <div class="grid-7 modelGetDetails offersList <%= offerDivHide %>">
                                <%if (isBookingAvailable)
                                  { %>
                                <h3 class="padding-bottom10"><span class="bwsprite offers-icon margin-left5 margin-right5"></span>Pay <span class="fa fa-rupee"></span> <%=bookingAmt %> to book your bike and get:</h3>
                                <%}
                                  else
                                  { %>
                                <h3 class="padding-bottom10"><span class="bwsprite offers-icon margin-left5 margin-right5"></span>Avail Offers</h3>
                                <%} %>
                                <ul>
                                    <asp:Repeater ID="rptOffers" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %>
                                               <% if(pqOnRoad.DPQOutput.objOffers.Count > 2)
                                                  { %>
                                               <%# Container.ItemIndex >  0 ? "<a class='viewMoreOffersBtn'>(view more)</a>" : "" %>
                                               <%} %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <ul class="moreOffersList hide">
                                    <asp:Repeater ID="rptMoreOffers" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="grid-<%= grid2_size %> rightfloat moreDetailsBookBtns <%=cssOffers %> margin-top20">
                                <input type="button" value="Get more details" class="btn btn-orange margin-right20" id="getMoreDetailsBtn">
                                <%if (isBookingAvailable && isOfferAvailable)
                                  { %>
                                <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-grey" id="bookNowBtn"> Book now </a>
                                <%} %>
                            </div>
                            <div class="clear"></div>
                            <% if (isBookingAvailable && !isOfferAvailable)
                               {%>
                            <div id="noOfferBookBtn" class="grid-12 padding-top10 alpha">
                                <div class="grid-9 omega">
                                    <h3 class="padding-bottom10"><span class="bwsprite offers-icon margin-left5 margin-right5"></span>Pay <span class="fa fa-rupee"></span> <%=bookingAmt %> to book your bike and get:</h3>
                                </div>
                                <div class="grid-3 alpha no-offer-book-btn">
                                    <a href="/pricequote/bookingsummary_new.aspx?MPQ=<%= mpqQueryString %>" class="btn btn-grey" id="bookNowBtn"> Book now </a>
                                </div>
                            </div>
                            <% } %>
                        </div>
                        <div class="clear"></div>
                        <% } %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <!-- View BreakUp Popup Starts here-->
            <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                <div class="breakupCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                <div class="breakup-text-container padding-bottom10">
                    <h3 class="breakup-header font26 margin-bottom20"><%= bikeName %> <span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                    <% if (isBikeWalePQ)
                       { %>
                    <table class="font16">
                        <tbody>
                            <tr>
                                <td width="350" class="padding-bottom10">Ex-showroom (Mumbai)</td>
                                <td width="150" align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.Price)) %> </td>
                            </tr>
                            <tr>
                                <td class="padding-bottom10">RTO</td>
                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.RTO)) %>  </td>
                            </tr>
                            <tr>
                                <td class="padding-bottom10">Insurance (comprehensive)
                                        <div class="insurance-breakup-text" style="position: relative; color: #999; font-size: 11px; margin-top: 1px;"><a target="_blank" href="/insurance/" >Save up to 60% on insurance - PolicyBoss</a> <span style="margin-left: 8px; vertical-align: super; font-size: 9px;">Ad</span></div>
                                </td>
                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.Insurance)) %>  </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="border-solid-top padding-bottom10"></div>
                                </td>
                            </tr>
                            <tr>
                                <!--  if :BWPriceList -->
                                <td class="padding-bottom10 text-bold">Total on road price
                                </td>
                                <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.Price + objSelectedVariant.RTO + objSelectedVariant.Insurance)) %></td>
                                <!-- / -->
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="border-solid-top padding-bottom10"></div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <% }
                       else if (pqOnRoad != null && pqOnRoad.IsDealerPriceAvailable)
                       {%>
                    <table class="font16">
                        <tbody>
                            <asp:Repeater ID="rptCategory" runat="server">
                                <ItemTemplate>
                                    <tr class="carwale">
                                        <td width="350" class="padding-bottom10"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryName")) %>
                                           <% if(!pqOnRoad.IsInsuranceFree) { %>
                                             <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryName")).ToLower().StartsWith("insurance") ? "<a style='position: relative; font-size: 11px; margin-top: 1px;' target='_blank' href='/insurance/' >Up to 60% off - PolicyBoss </a>" : ""  %>
                                            <% } %>
                                        </td>
                                        <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %></span></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <% if (pqOnRoad.IsInsuranceFree && pqOnRoad.InsuranceAmount > 0)
                               {%>
                            <tr>
                                <td colspan="2">
                                    <div class="border-solid-top padding-bottom10"></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="padding-bottom10">Total on road price</td>
                                <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(onRoadPrice)) %></td>
                            </tr>

                            <tr>
                                <td class="padding-bottom10">Minus insurance</td>
                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(pqOnRoad.InsuranceAmount)) %></td>
                            </tr>
                            <% } %>
                            <tr>
                                <td colspan="2">
                                    <div class="border-solid-top padding-bottom10"></div>
                                </td>
                            </tr>
                            <tr>
                                <!-- ko if :DealerPriceList() -->
                                <% if (pqOnRoad.DPQOutput.PriceList.Count > 0)
                                   {%>
                                <td class="padding-bottom10 text-bold">Total on road price</td>
                                <% if (pqOnRoad.InsuranceAmount > 0)
                                   {
                                %>
                                <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(onRoadPrice - pqOnRoad.InsuranceAmount)) %></td>
                                <% }
                                   else
                                   { %>
                                <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(price) %></td>
                                <%} %>
                                <%} %>
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
                    <% } %>
                </div>
            </div>
            <!--View Breakup popup ends here-->

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
                    <p class="font20 margin-top25 margin-bottom10">Get more details on this bike</p>
                    <p class="text-light-grey margin-bottom20">Please provide contact info to see more details</p>
                    <div class="personal-info-form-container">
                        <div class="form-control-box personal-info-list">
                            <input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
                                id="getFullName" data-bind="value: fullName">
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                        </div>
                        <div class="form-control-box personal-info-list">
                            <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                                id="getEmailID" data-bind="value: emailId">
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                        </div>
                        <div class="form-control-box personal-info-list">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                                id="getMobile" maxlength="10" data-bind="value: mobileNo">
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                    </div>
                    <!--
                    <div class="mobile-verification-container hide">
                        <div class="input-border-bottom"></div>
                        <div class="margin-top20">
                            <p class="font14 confirm-otp-text leftfloat">Please confirm your contact details and enter the OTP for mobile verfication</p>
                            <div class="form-control-box">
                                <input type="text" class="form-control get-otp-code rightfloat" maxlength="5" placeholder="Enter OTP" id="getOTP" data-bind="value: otpCode">
                                <span class="bwsprite error-icon errorIcon hide"></span>
                                <div class="bw-blackbg-tooltip errorText hide"></div>
                            </div>

                            <div class="clear"></div>
                        </div>
                        <a class="margin-left10 blue rightfloat resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                        <p class="otp-alert-text margin-left10 rightfloat otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                            OTP has been already sent to your mobile
                        </p>
                        <div class="clear"></div>
                        <br />
                        <a class="btn btn-orange" id="otp-submit-btn">Confirm OTP</a>
                        <div style="margin-right: 70px;" id="processing" class="hide"><b>Processing Please wait...</b></div>
                    </div>
                    -->
                </div>
                <!-- contact details ends here -->
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
        </section>
        <section>
            <!-- #include file="/ads/Ad970x90_BTF.aspx" -->
        </section>

        <section class="container <%= (modelPage.ModelDesc == null || string.IsNullOrEmpty(modelPage.ModelDesc.SmallDescription)) ? "hide" : string.Empty %>">
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
                        <span><a href="javascript:void(0)" class="read-more-btn">Read <span>full story</span></a></span>
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
                        <a href="#variants" style="<%= (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0) ? string.Empty: "display:none;" %>">Versions</a>
                        <a href="#colours" style="<%= (modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0) ? string.Empty: "display:none;" %>">Colours</a>
                    </div>
                    <!-- Overview code starts here -->
                    <div class="bw-tabs-data margin-bottom20 active" id="overview">
                        <h2 class="font24 margin-top10 margin-bottom20 text-center">Overview</h2>
                        <div class="grid-3 border-solid-right">
                            <div class="font22 text-center padding-top20 padding-bottom20">
                                <%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement) %>
                                <small class='<%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.Displacement).Equals("--") ? "font16 text-medium-grey hide":"font16 text-medium-grey" %>'>CC</small>
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
                                        <li data-tabs="fuelEffiency"><span class="model-sprite bw-performance-ico"></span>Fuel efficiency and Performance</li>
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
                                </li>
                                <li>
                                    <div class="text-light-grey">Fuel Guage</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.FuelGauge) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Tachometer Type</div>
                                    <div class="text-bold"><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(modelPage.ModelVersionSpecs.TachometerType) %></div>
                                </li>
                                <li>
                                    <div class="text-light-grey">Digital Fuel Guage</div>
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
                            <div class="more-features-btn"><a href="javascript:void(0)">+</a></div>
                        </div>
                    </div>
                    <!-- variant code starts here -->
                    <div class="bw-tabs-data margin-bottom20 <%= modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0 ? string.Empty : "hide" %>" id="variants">
                        <h2 class="font24 margin-bottom20 text-center">Versions</h2>
                        <asp:Repeater runat="server" ID="rptVarients" OnItemDataBound="rptVarients_ItemDataBound">
                            <ItemTemplate>
                                <div class="grid-6">
                                    <div class="border-solid content-inner-block-10 margin-bottom20">
                                        <div class="grid-8 varient-desc-container alpha">
                                            <h3 class="font16 margin-bottom10"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionName")) %></h3>
                                            <p class="font14"><%# FormatVarientMinSpec(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AlloyWheels")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "ElectricStart")),Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "AntilockBrakingSystem")),Convert.ToString(DataBinder.Eval(Container.DataItem, "BrakeType"))) %></p>
                                        </div>
                                        <div class="grid-4 omega">
                                            <p class="font18 margin-bottom10">
                                                <span class="fa fa-rupee margin-right5"></span>
                                                <span id="<%# "price_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>">
                                                    <asp:Label Text='<%#Eval("Price") %>' ID="txtComment" runat="server"></asp:Label>
                                                </span>
                                            </p>
                                            <p class="font12 text-light-grey" id="<%# "locprice_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>">
                                               <asp:Label ID="lblExOn" Text="Ex-showroom price" runat="server"></asp:Label>, 
                                                <% if(cityId!= 0)
                                                   { %>
                                                    <%= cityName %>
                                                <% } else 
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
                    <!-- colours code starts here -->
                    <div class="bw-tabs-data margin-bottom20 <%= modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0 ? string.Empty : "hide" %>" id="colours">
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
        <section class="container <%= reviewTabsCnt == 0 ? "hide" : string.Empty %>">
            <!--  News Bikes latest updates code starts here -->
            <div class="newBikes-latest-updates-container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest updates on <%= bikeName %></h2>
                    <div class="bw-tabs-panel content-box-shadow">
                        <div class="text-center <%= reviewTabsCnt > 2 ? string.Empty : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                <ul>
                                    <li class="<%= isUserReviewActive ? "active" : String.Empty %>" style="<%= (Convert.ToInt32(ctrlUserReviews.FetchedRecordsCount) > 0) ? string.Empty: "display:none;" %>" data-tabs="ctrlUserReviews">User Reviews</li>
                                    <li class="<%= isExpertReviewActive ? "active" : String.Empty %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? string.Empty: "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>
                                    <li class="<%= isNewsActive ? "active" : String.Empty %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? string.Empty: "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li class="<%= isVideoActive ? "active" : String.Empty  %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? string.Empty: "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                </ul>
                            </div>
                        </div>
                        <%if (!isUserReviewZero)
                          { %>
                        <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                        <% } %>
                        <%if (!isExpertReviewZero)
                          { %>
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <% } %>
                        <%if (!isNewsZero)
                          { %>
                        <BW:News runat="server" ID="ctrlNews" />
                        <% } %>
                        <%if (!isVideoZero)
                          { %>
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <% } %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <section class="margin-bottom30 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
            <div class="container">
                <div class="grid-12 alternative-section" id="alternative-bikes-section">
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

        <section>
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <div class="inline-block text-center margin-right30">
                            <div class="icon-outer-container rounded-corner50">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite question-mark-icon margin-top25"></span>
                                </div>
                            </div>
                        </div>
                        <div class="inline-block">
                            <h3 class="margin-bottom10">Questions?</h3>
                            <p class="text-light-grey font14">We’re here to help. Read our <a href="">FAQs</a>, <a href="mailto:contact@bikewale.com">email</a> or call us on <span class="text-dark-grey">1800 120 8300</span></p>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <!-- get on road price popup -->
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

        <BW:PriceQuotePopup ID="ctrlPriceQuotePopup" runat="server" />
        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/model.js?<%= staticFileVersion %>">"></script>
        <%--<link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/brand.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">--%>
        <script type="text/javascript">

            function bindInsuranceText() {
                icityArea = GetGlobalCityArea();
                if (!viewModel.isDealerPQAvailable()) {
                    var d = $("#bw-insurance-text");
                    d.find("div.insurance-breakup-text").remove();
                    d.append(" <div class='insurance-breakup-text' style='position: relative; color: #999; font-size: 11px; margin-top: 1px;'>Save up to 60% on insurance - <a target='_blank' href='/insurance/' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Model_Page', act: 'Insurance_Clicked',lab: '" + myBikeName + "_" + icityArea + "' });\">PolicyBoss</a> <span style='margin-left: 8px; vertical-align: super; font-size: 9px;'>Ad</span></div>");
                }
                else if (viewModel.isDealerPQAvailable() && !(viewModel.priceQuote().isInsuranceFree && viewModel.priceQuote().insuranceAmount > 0)) {
                    var e = $("table#model-view-breakup tr td:contains('Insurance')");
                    e.find("div.insurance-breakup-text").remove();
                    e.append("<div class='insurance-breakup-text' style='position: relative; color: #999; font-size: 11px; margin-top: 1px;'>Save up to 60% on insurance - <a target='_blank' href='/insurance/' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Model_Page', act: 'Insurance_Clicked',lab: '" + myBikeName + "_" + icityArea + "' });\">PolicyBoss</a> <span style='margin-left: 8px; vertical-align: super; font-size: 9px;'>Ad</span></div>");
                }
            }

            var myBikeName = "<%= this.bikeName %>";
            var clientIP = "<%= clientIP%>";
            var pageUrl = "<%= canonical %>"
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
                document.location.href.split('?')[0];
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
                    $('html, body').animate({ scrollTop: target.offset().top - 50 - $(".header-fixed").height() }, 1000);
                    return false;

                });
                // ends                                

            <%--<% if (modelPage.ModelDetails.New)
               { %>
                var cityId = '<%= cityId%>';
                InitVM(cityId);
                <% } %>--%>

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
                $(".more-features").css("display", "block");
                sectionContainer_height = section_height.outerHeight() + menuTop - 250;
                $(".more-features").css("display", "none");
            });

            section_height.bind('heightChangeNone', function () {
                $(".more-features").css("display", "none");
                sectionContainer_height = section_height.outerHeight() + menuTop - 250;
                $(".more-features").css("display", "block");
            });

            $(".more-features-btn").click(function () {
                if ($(".more-features").css("display") == "none")
                    section_height.trigger('heightChangeBlock');
                else if ($(".more-features").css("display") == "block")
                    section_height.trigger('heightChangeNone');
            });

            $window.scroll(function () {
                $menu.toggleClass('affix', sectionContainer_height >= $window.scrollTop() && $window.scrollTop() > sectionStart);
                var cur_pos = $(this).scrollTop();

                sections.each(function () {
                    var top = $(this).offset().top - 10 - nav_height,
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
           <%-- var viewModel = null;
            function InitVM(cityId) {
                debugger;
                viewModel = new pqViewModel('<%= modelId%>', cityId);
                modelViewModel = viewModel;
                ko.applyBindings(viewModel, $('#dvBikePrice')[0]);
                viewModel.LoadCity();
            }--%>

            if ('<%=isUserReviewActive%>' == 'False') $("#ctrlUserReviews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");
            var getCityArea = GetGlobalCityArea();
            if(bikeVersionLocation == ''){
                bikeVersionLocation = getBikeVersionLocation();
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
            if(isBikeWalePq == 'True' ){
                if(getCityArea!= null) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_page', 'act': 'Page_Load', 'lab': 'BWPQ_' + getCityArea + '_'+  myBikeName });
                  }       
            }else{
                if(getCityArea!= null)  {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_page', 'act': 'Page_Load', 'lab': 'DealerPQ_' + getCityArea + '_' + myBikeName });
                }
            }
        </script>
    </form>
</body>
</html>

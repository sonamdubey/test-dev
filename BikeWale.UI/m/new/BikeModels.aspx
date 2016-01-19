<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeModels" %>

<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="/m/controls/UserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register Src="~/m/controls/UsersTestimonials.ascx" TagPrefix="BW" TagName="UsersTestimonials" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <%
        title = modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " Price in India - Rs."
                    + Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MinPrice.ToString()) + " - " + Bikewale.Utility.Format.FormatPrice(modelPage.ModelDetails.MaxPrice.ToString())
                    + ". Check out " + modelPage.ModelDetails.MakeBase.MakeName + " " + modelPage.ModelDetails.ModelName + " on road price, reviews, mileage, versions, news & photos at Bikewale.";

        canonical = "http://www.bikewale.com/" + modelPage.ModelDetails.MakeBase.MaskingName + "-bikes/" + modelPage.ModelDetails.MaskingName + "/";
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1017752";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
        Ad_300x250 = true;
        TargetedModel = modelPage.ModelDetails.ModelName;        
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <script type="text/javascript">
        var dealerId = '<%= dealerId%>';
        var pqId = '<%= pqId%>';
        var versionId = '<%= variantId%>';
        var cityId = '<%= cityId%>';
        var clientIP = "<%= clientIP%>";
        var pageUrl = "www.bikewale.com/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;
        var areaId = '<%= areaId%>';
        var bikeVersionLocation = '';
        var bikeVersion = '';
        var isBikeWalePq = "<%= isBikeWalePQ%>"; 
    </script>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-model.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container bg-white clearfix">
                <div class="<%= !modelPage.ModelDetails.New ? "padding-top20 position-rel" : ""%>">
                    <% if (modelPage.ModelDetails.New)
                       { %><h1 class="padding-top15 padding-left20 padding-right20"><%= bikeName %></h1>
                    <% } %>
                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Upcoming</div>
                    <div class="bikeTitle">
                        <h1 class="padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>
                    <% if (!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="upcoming-text-label font16 position-abt pos-top10 text-white text-center">Discontinued</div>
                    <div class="bikeTitle">
                        <h1 class="padding-top30 padding-left20 padding-right20"><%= bikeName %></h1>
                    </div>
                    <% } %>

                    <% if (modelPage.ModelDetails.New || !modelPage.ModelDetails.New)
                       { %>
                    <div class="padding-left20 padding-right10 margin-top5 margin-bottom15">
                        <p class=" <%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> leftfloat margin-right10 rating-wrap">
                            <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble((modelPage.ModelDetails == null || modelPage.ModelDetails.ReviewRate == null) ? 0 : modelPage.ModelDetails.ReviewRate )) %>
                        </p>
                        <p class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "hide" : ""  %> leftfloat margin-right10 rating-wrap">
                            Not rated yet
                        </p>
                        <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> border-solid-left leftfloat margin-right10 padding-left10 line-Ht22">
                            <%= modelPage.ModelDetails.ReviewCount %> Reviews
                        </a>
                        <div class="clear"></div>
                    </div>
                    <% } %>

                    <div class="swiper-container padding-bottom20 model" id="bikeBannerImageCarousel">
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
                        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <p class="pagination-number margin margin-bottom10 text-center font16 text-light-grey"><span class="bike-model-gallery-count">1/<%= modelPage.Photos.Count %></span></p>
                        <% } %>
                    </div>

                    <% if (modelPage.ModelDetails.Futuristic)
                       { %>
                    <div class="bikeDescWrapper">
                        <%-- <div class="bikeTitle">
                            <h1 class="padding-bottom10"><%= bikeName %></h1>
                        </div>--%>
                        <div class="margin-bottom10 font14 text-light-grey">Expected price</div>
                        <div class="font22 text-grey">

                            <span class="font24 text-bold"><span class="fa fa-rupee"></span> <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
                        </div>
                        <div class="border-light-bottom margin-top15 margin-bottom15"></div>
                        <div class="margin-bottom10 font14 text-light-grey">Expected launch date</div>
                        <div class="font18 text-grey margin-bottom5">
                            <span class="text-bold"><%= modelPage.UpcomingBike.ExpectedLaunchDate %></span>
                        </div>

                        <p class="font14 text-grey"><%= bikeName %> is not launched in India yet. Information on this page is tentative.</p>
                    </div>
                    <% } %>
                </div>
                <% if (modelPage.ModelDetails.New)
                   { %>
                <div class="grid-12 bg-white box-shadow" id="dvBikePrice">

                    <div class="clearfix">
                        <div class="font14 text-light-grey alpha omega grid-2 margin-top10">Version:</div>
                        <% if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 1)
                           { %>
                        <div class="form-control-box variantDropDown leftfloat grid-10 omega">
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
                                                <asp:Button Style="width: 100%; text-align: left" ID="btnVariant" ToolTip='<%#Eval("VersionId") %>' OnCommand="btnVariant_Command" versionid='<%#Eval("VersionId") %>' CommandName='<%#Eval("VersionId") %>' CommandArgument='<%#Eval("VersionName") %>' runat="server" Text='<%#Eval("VersionName") %>'></asp:Button>
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
                        <p id='versText' class="variantText text-medium-grey grid-10 text-bold font14 margin-top10"><%= variantText %></p>
                        <% } %>
                        <%--<div class="leftfloat grid-10 omega">
                        	<select class="form-control">
	                            <option>Alloy, Self</option>
                            	<option>Alloy, Double Disc, Self</option>
                                <option>Double Disc, Self</option>
                            </select>
                        </div>--%>
                    </div>
                    <div>


                        <% if (isDiscontinued){ %>
                            <p class="margin-top20 margin-left10 font14 text-light-grey clear fillPopupData">Last known Ex-showroom price</p>
                        <% } %>
                        <% else 
                          if( !isCitySelected) {%>
                        <p class="font14 fillPopupData">
                            Ex-showroom price in <span href="javascript:void(0)" class="text-light-grey clear">
                                <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span>
                            <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5">
                                <span class="bwmsprite edit-blue-icon"></span>
                            </a>
                        <% } %>
                        <% else
                           if( !isOnRoadPrice) {%>
                            <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">Ex-showroom price in <span class="font14 text-grey"><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5"><span class="bwmsprite edit-blue-icon"></span></a>
                            </p>
                        <% } %>
                        <% else {%>
                            <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">On-road price in <span class="font14 text-grey"><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5"><span class="bwmsprite edit-blue-icon"></span></a>
                            </p>
                        <% } %>

                        <%--<% else if (pqOnRoad !=null && pqOnRoad.IsDealerPriceAvailable){ %>
                            <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">On-road price in <span class="font14 text-grey text-bold"><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="viewBreakupText fillPopupData"><span class="fa fa-edit"></span></a>
                            </p>
                        <% } %>
                        <% else if (!isCitySelected){ %>
                            <p class="font14 fillPopupData">
                            Ex-showroom price in <span href="javascript:void(0)" class="text-light-grey clear"><%= Bikewale.Common.Configuration.GetDefaultCityName %></span><a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="viewBreakupText fillPopupData">
                            <span class="fa fa-edit"></span></a>
                        </p>
                        <% } %>
                        <% else if (!isAreaAvailable)
                           { %>
                                <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">
                            On-road price in <span class="font14 text-grey text-bold">
                                <%= cityName %></span>
                            <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="viewBreakupText fillPopupData">
                                <span class="fa fa-edit"></span></a>
                            </p>
                        <% } %>
                        <% else if (isAreaAvailable && !isAreaSelected)
                           { %>
                            <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">
                            Ex-showroom price in <span class="font14 text-grey text-bold">
                                <%= cityName %></span>
                            <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="viewBreakupText fillPopupData">
                                <span class="fa fa-edit"></span></a>
                            </p>
                        <% } %>
                        <% else 
                           { %>
                            <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">
                            On-road price in <span class="font14 text-grey text-bold">
                                <%= cityName %></span>
                            <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="viewBreakupText fillPopupData">
                                <span class="fa fa-edit"></span></a>
                            </p>
                        <% } %>--%>
                        <p class="leftfloat">

                            <%if (price != "0" && price != string.Empty)
                              { %>
                            <span class="font24 text-bold"><span class="fa fa-rupee"></span> <%= Bikewale.Utility.Format.FormatPrice(price) %></span>
                            <% }
                              else
                              { %>
                            <span class="font20 text-bold">Price unavailable</span>
                            <%  } %>
                        </p>
                        <%if (isOnRoadPrice)  {%>
                                <p id="viewBreakupText" class="font14 text-light-grey leftfloat viewBreakupText">View Breakup</p>
                        <%if (isBikeWalePQ && price != "0" && price != string.Empty) {%>
                                <p class="font12 text-light-grey clear ">Ex-showroom + RTO + Insurance(Comprehensive)</p>
                        <%}
                          else
                          { %>
                                <p class="font12 text-xt-light-grey clear"><%=viewbreakUpText %></p>
                        <%} %>
                        <% } %>
                        <% if (!toShowOnRoadPriceButton && isBikeWalePQ)
                           { %>
                            <p class="margin-top10 margin-bottom20 clear">
                                <a class='padding-top10 text-bold' style="position: relative; font-size: 14px; margin-top: 1px;" target="_blank" href="/m/insurance/" id="insuranceLink">Save up to 60% on insurance - PolicyBoss
                                </a>
                            </p>
                        <% } %>
                    </div>

                    <% if (pqOnRoad != null && pqOnRoad.IsDealerPriceAvailable)
                       {%>
                    <div id="offersBlock" class="city-unveil-offer-container position-rel margin-top20 margin-bottom20">

                        <div class="available-offers-container content-inner-block-10">
                            <div class="offer-list-container" id="dvAvailableOffer">
                                <%if (isBookingAvailable && bookingAmt > 0)
                                  { %>
                                <h4 class="border-solid-bottom padding-bottom5 margin-bottom10"><span class="bwmsprite offers-icon"></span>
                                    Pay <span class="fa fa-rupee"></span> <%=bookingAmt %> to book your bike and get:
                                </h4>
                                <%    } %>
                                <% if (isOfferAvailable)
                                   { %>
                                <ul class="offersList" style="list-style:none">
                                    <asp:Repeater ID="rptOffers" runat="server">
                                        <ItemTemplate>
                                            <li class="offertxt float-left">
                                                <span style='display:inline;' class="show"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %></span>
                                                <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span  class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>
                                                <% if (pqOnRoad.DPQOutput.objOffers.Count > 2)
                                                   { %>
                                                <%# Container.ItemIndex >  0 ? "<a class='viewMoreOffersBtn'>(view more)</a>" : "" %>
                                                <% } %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <ul class="moreOffersList hide" style="list-style:none">
                                    <asp:Repeater ID="rptMoreOffers" runat="server">
                                        <ItemTemplate>
                                             <li class="offertxt float-left">
                                                <span style="display:inline;" class="show"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %></span>
                                                <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span  class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <% } %>

                                <%= (isOfferAvailable)?"<div class=\"border-top1 margin-top10 margin-bottom10\"></div>":string.Empty %>
                                <h4 class="border-solid-bottom padding-bottom5 margin-bottom10"><span class="bwmsprite disclaimer-icon margin-right5"></span> Get following details on the bike</h4>
                                <ul class="bike-details-list-ul">

                                    <li>
                                        
                                        <span class="show">Offers from the nearest dealers</span>
                                    </li>
                                    <li>
                                        
                                        <span class="show">Waiting period on this bike at the dealership</span>
                                    </li>
                                    <li>
                                       
                                        <span class="show">Nearest dealership from your place</span>
                                    </li>
                                    <li>
                                       
                                        <span class="show">Finance options on this bike</span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
                <% } %>
                <% if (!modelPage.ModelDetails.New && !modelPage.ModelDetails.Futuristic)
                   { %>
                <div class="container clearfix box-shadow">
                    <%--                    <div class="bikeTitle">
                        <h1 class="padding-bottom15 padding-left15"><%= bikeName %></h1>
                    </div>
                    <div class="leftfloat">
                        <div class="padding-left5 padding-right5 ">
                            <div>
                                <span class="margin-bottom10  <%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %>">
                                    <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble((modelPage.ModelDetails == null || modelPage.ModelDetails.ReviewRate == null) ? 0 : modelPage.ModelDetails.ReviewRate )) %>
                                </span>
                                <span class="margin-bottom10 <%= modelPage.ModelDetails.ReviewCount > 0 ? "hide" : ""  %>">Not rated yet
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="leftfloat border-left1">
                        <div class="padding-left5 padding-right5 ">
                            <span class="font16 text-light-grey">
                                <a href="/m/<%=modelPage.ModelDetails.MakeBase.MaskingName %>-bikes/<%= modelPage.ModelDetails.MaskingName %>/user-reviews/" class="<%= modelPage.ModelDetails.ReviewCount > 0 ? "" : "hide"  %> margin-right10 padding-left10 line-Ht22">
                                    <%= modelPage.ModelDetails.ReviewCount %> Reviews</a>
                            </span>
                        </div>
                    </div>
                    <div class="clear"></div>--%>
                    <div class="bike-price-container margin-bottom15">
                        <span class="font14 text-grey padding-left10">Last known Ex-showroom Price</span>
                    </div>
                    <div class="bike-price-container margin-top10 font22 margin-bottom10 padding-left10">
                        <div class="bike-price-container font22 margin-bottom15">
                            <span class="fa fa-rupee"></span>
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
            </div>

            <!-- floating buttons -->
            <%-- <div class="grid-12 float-button float-fixed clearfix">--%>
            <% if(!isDiscontinued)
               {
                   if (toShowOnRoadPriceButton)
                   {   %>
                        <div class="grid-12 float-button float-fixed clearfix">
                       
                    <a id="btnGetOnRoadPrice" href="javascript:void(0)" ismodel="true" modelid="<%=modelId %>" style="width: 100%" class="btn btn-orange margin-top10 fillPopupData">Get on road price</a>
                <% }
               else
                    {   %>
                <div class="grid-12 float-button float-fixed clearfix">
                    <div class="show padding-top10">
                        <% if (modelPage.ModelDetails.New)
                           {   %>
                        <% if (pqOnRoad != null && pqOnRoad.IsDealerPriceAvailable)
                           { %>
                        <div class="grid-<%=btMoreDtlsSize %>">
                            <input type="button" value="Get more details" class="btn btn-orange btn-full-width btn-sm margin-right10 leftfloat" id="getMoreDetailsBtn" />
                        </div>
                        <%} %>
                        <% if (bookingAmt > 0)
                           { %>
                        <div class="grid-5 omega">
                            <input type="button" value="Book now" class="btn btn-grey btn-full-width btn-sm rightfloat" id="bookNowBtn" />
                        </div>
                        <%} %>

                        <%} %>
                    </div>
                </div>
                <%  } 
              }%>
            </div>

        </section>
        <% if (Ad_300x250)
           { %>
        <section>
            <!-- #include file="/ads/Ad300x250_mobile.aspx" -->
        </section>
        <% } %>

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
                                <li class="active" data-tabs="summary">Summary</li>
                                <li data-tabs="engineTransmission">Engine &amp; Transmission </li>
                                <li data-tabs="brakeWheels">Brakes, Wheels and Suspension</li>
                                <li data-tabs="dimensions">Dimensions and Chassis</li>
                                <li data-tabs="fuelEffiency">Fuel efficiency and Performance</li>
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
                                                    <span class="fa fa-rupee margin-right5"></span>
                                                    <span id="<%# "priced_" + Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId")) %>">
                                                        <asp:Label Text='<%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %>' ID="txtComment" runat="server"></asp:Label>
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
                    </div>
                    <!-- colours code starts here -->
                    <div class="grid-12">
                        <div class="colours-wrap margin-bottom20 <%= modelPage.ModelColors != null && modelPage.ModelColors.ToList().Count > 0 ? "" : "hide" %>">
                            <h2 class="margin-top30 margin-bottom20 text-center">Colours</h2>
                           <div class="swiper-container padding-bottom60">
                                <div class="swiper-wrapper text-center">
                                        <asp:Repeater ID="rptColors" runat="server">
                                            <ItemTemplate>
                                                <div class="swiper-slide available-colors">
                                                    <div class="color-box" style="background-color: #<%# DataBinder.Eval(Container.DataItem, "HexCode")%>;"></div>
                                                    <p class="font16 text-medium-grey"><%# DataBinder.Eval(Container.DataItem, "ColorName") %></p>
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

                                <li style="<%= (Convert.ToInt32(ctrlUserReviews.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%=isUserReviewActive ? "active" : "hide" %>" data-tabs="ctrlUserReviews">User Reviews</li>
                                <li style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%=isExpertReviewActive ? "active" : "hide" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>
                                <li style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%= isNewsActive ? "active" : "hide" %>" data-tabs="ctrlNews">News</li>
                                <li style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount)  > 0) ? "": "display:none;" %>" class="<%= isVideoActive ? "active" : "hide" %>" data-tabs="ctrlVideos">Videos</li>

                            </ul>
                        </div>
                        <div class="grid-12">
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
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom10">
                <div class="grid-12">
                    <!-- Most Popular Bikes Starts here-->
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= bikeName %> alternatives</h2>

                     <div class="swiper-container discover-bike-carousel alternatives-carousel padding-bottom60">
                        <div class="swiper-wrapper">
                            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
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

        <% if (ctrlUsersTestimonials.FetchedCount > 0 && bookingAmt > 0)
           { %>
        <section>
            <div id="testimonialWrapper" class="container margin-bottom10">
                <h2 class="text-bold text-center margin-top30 margin-bottom20 font24">Testimonials</h2>
                <div class="swiper-container text-center">
                    <div class="swiper-wrapper margin-bottom10">
                        <BW:UsersTestimonials ID="ctrlUsersTestimonials" runat="server"></BW:UsersTestimonials>                        
                    </div>
                </div>
            </div>
        </section>
            <%
           }        
        %>
        

        <% if (bookingAmt > 0){ %>
        <section>
            <div class="container margin-top20 margin-bottom30">
                <div id="faqSlug" class="grid-12">
                    <div class="faq-slug-container content-box-shadow content-inner-block-20">
                        <div class="question-icon-container text-center leftfloat">
                            <div class="icon-outer-container rounded-corner50percent">
                                <div class="icon-inner-container rounded-corner50percent">
                                    <span class="bwmsprite question-mark-icon margin-top20"></span>
                                </div>
                            </div>
                        </div>
                        <div class="question-text-container leftfloat padding-left15">
                            <p class="question-title font16 text-bold text-black">Questions?</p>
                            <p class="question-subtitle text-light-grey font14">We’re here to help.<br />Read our <a href="/m/faq.aspx" target="_blank" >FAQs</a>, <a href="mailto:contact@bikewale.com">email</a> or call us on <a href="tel:18001208300" class="text-dark-grey">1800 120 8300</a></p>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
        <!-- Terms and condition Popup Ends -->
        <!-- View BreakUp Popup Starts here-->
        <div class="breakupPopUpContainer bwm-fullscreen-popup hide" id="breakupPopUpContainer">
            <div class="breakupCloseBtn position-abt pos-top10 pos-right10 bwmsprite  cross-lg-lgt-grey cur-pointer"></div>
            <div class="breakup-text-container padding-bottom10">
                <h3 class="breakup-header margin-bottom5"><%= bikeName %> <span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                <% if (isBikeWalePQ)
                    { %>
                <table class="font14" width="100%">
                    <tbody>
                        <tr>
                            <td width="60%" class="padding-bottom10">Ex-showroom (Mumbai)</td>
                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.Price)) %></td>
                        </tr>
                        <tr>
                            <td class="padding-bottom10">RTO</td>
                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.RTO)) %></td>
                        </tr>
                        <tr>
                            <td class="padding-bottom10">Insurance<a style="position: relative; font-size: 11px; margin-top: 1px;" target="_blank" href="/m/insurance/"> Up to 60% off - PolicyBoss </a></td>
                            <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.Insurance)) %></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="border-solid-top padding-bottom10"></div>
                            </td>
                        </tr>
                        <tr>
                            <!-- ko if :BWPriceList -->
                            <td class="padding-bottom10 text-bold">Total on road price</td>
                            <td align="right" class="padding-bottom10 font20 text-bold text-right"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(objSelectedVariant.Price + objSelectedVariant.RTO + objSelectedVariant.Insurance)) %></td>
                            <!-- /ko -->
                        </tr>
                    </tbody>
                </table>
                <% }
                    else if (pqOnRoad != null && pqOnRoad.IsDealerPriceAvailable)
                    {    %>
                <table id="dp-insurance-text" class="font14" width="100%">
                    <tbody>

                        <asp:Repeater ID="rptCategory" runat="server">
                            <ItemTemplate>
                                <tr class="carwale">
                                    <td width="60%" class="padding-bottom10"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryName")) %>
                                        <% if(!pqOnRoad.IsInsuranceFree) { %>
                                            <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryName")).ToLower().StartsWith("insurance") ? "<a style='position: relative; font-size: 11px; margin-top: 1px;' target='_blank' href='/insurance/' >Up to 60% off - PolicyBoss </a>" : ""  %>
                                        <% } %>
                                    </td>
                                    <td align="right" class="padding-bottom10 text-bold text-right"><span class="fa fa-rupee margin-right5"></span>
                                        <span><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "Price"))) %></span></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                        <%if (pqOnRoad != null && pqOnRoad.IsDiscount)
                          { %>
                            <tr>
                                <td colspan="2">
                                    <div class="border-solid-top padding-bottom10"></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="padding-bottom10">Total on road price</td>
                                <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(onRoadPrice)) %></td>
                            </tr>
                            <asp:Repeater ID="rptDiscount" runat="server">
                                <ItemTemplate>
                                    <tr class="carwale">
                                        <td width="350" class="padding-bottom10">Minus <%# Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryName")) %></td>
                                        <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span>
                                            <span><%# Bikewale.Utility.Format.FormatPrice(
                                                  Convert.ToString(DataBinder.Eval(Container.DataItem, "Price")))  %></span></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%--<% if (pqOnRoad.IsInsuranceFree && pqOnRoad.InsuranceAmount > 0)
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
                            <% } %>--%>
                            <%} %>
                        <tr>
                            <td colspan="2">
                                <div class="border-solid-top padding-bottom10"></div>
                            </td>
                        </tr>
                        <tr>
                            <% if (pqOnRoad.DPQOutput.PriceList.Count > 0)
                                {%>
                            <td class="padding-bottom10 text-bold">Total on road price</td>
                            <% if (pqOnRoad.InsuranceAmount > 0)
                                {
                            %>
                            <td align="right" class="padding-bottom10 font20 text-bold text-right"><span class="fa fa-rupee margin-right5"></span>
                                <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(onRoadPrice - TotalDiscountedPrice())) %>

                            </td>
                            <% }
                                else
                                { %>
                            <td align="right" class="padding-bottom10 font20 text-bold text-right"><span class="fa fa-rupee margin-right5"></span>
                                <%= Bikewale.Utility.Format.FormatPrice(price) %>
                                <%} %>
                                <%} %>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="border-solid-top padding-bottom10"></div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <% } %>
            </div>
        </div>
        <!--View Breakup popup ends here-->

        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <!-- Contact details Popup starts here -->
                    <h2 class="margin-bottom5">Get more details on this bike</h2>
                    <p class="text-light-grey margin-bottom5">Please provide contact info to see more details</p>

                    <div class="personal-info-form-container margin-top10">
                        <div class="form-control-box">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="value: fullName" />
                            <span class="bwmsprite error-icon "></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your name</div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="value: emailId" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your email adress</div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="value: mobileNo" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                    </div>
                    <input type="button" class="btn btn-full-width btn-orange hide" value="Submit" onclick="validateDetails();" class="rounded-corner5" data-role="none" id="btnSubmit" />
                </div>
                <!-- Contact details Popup ends here -->
                <div id="otpPopup">
                    <p class="font18 margin-bottom5">Verify your mobile number</p>
                    <p class="text-light-grey margin-bottom5">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                            <span class="fa fa-phone"></span>
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
                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" />
                        </div>
                    </div>

                </div>
                <!-- OTP Popup ends here -->
            </div>
        </div>
        <!-- Lead Capture pop up end  -->   
                                <!-- Terms and condition Popup start -->
                        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                            <h3>Terms and Conditions</h3>
                            <div style="vertical-align: middle; text-align: center;" id="termspinner">
                                <%--<span class="fa fa-spinner fa-spin position-abt text-black bg-white" style="font-size: 50px"></span>--%>
                                <img src="/images/search-loading.gif" />
                            </div>
                            <div class="termsPopUpCloseBtn position-abt pos-top10 pos-right10 bwmsprite  cross-lg-lgt-grey cur-pointer"></div>
                            <div id="terms" class="breakup-text-container padding-bottom10 font14">
                            </div>
                        </div>
                        <!-- Terms and condition Popup end -->
                      
        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <BW:MPopupWidget runat="server" ID="MPopupWidget1" />

        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-model.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            vmModelId = '<%= modelId%>';
            clientIP = '<%= clientIP%>';
            cityId = '<%= cityId%>';
            isUsed = '<%= !modelPage.ModelDetails.New %>';
            var pageUrl = "<%= canonical %>";
            var myBikeName = "<%= this.bikeName %>";
            ga_pg_id = '2';
            if ('<%=isUserReviewActive%>' == "False") $("#ctrlUserReviews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide"); 
            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
            }
            if (bikeVersion == '') {
                bikeVersion = getBikeVersion();
            }
            var getCityArea = GetGlobalCityArea();
            if (isBikeWalePq == 'True') {
                if (getCityArea != null) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_page', 'act': 'Page_Load', 'lab': 'BWPQ_' + getCityArea + myBikeName });
                }
            } else {
                if (getCityArea != null) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_page', 'act': 'Page_Load', 'lab': 'DealerPQ_' + getCityArea + myBikeName });
                }
            }
        </script>
    </form>
</body>
</html>

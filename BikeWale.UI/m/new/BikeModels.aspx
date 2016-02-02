<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeModels" %>

<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="/m/controls/UserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
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
        TargetedCity = cityName;
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
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div itemscope="" itemtype="http://auto.schema.org/Motorcycle" class="container bg-white clearfix">
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

                            <span class="font24 text-bold"><span class="fa fa-rupee"></span><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)) %></span>
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


                        <% if (isDiscontinued)
                           { %>
                        <p class="margin-top20 margin-left10 font14 text-light-grey clear fillPopupData">Last known Ex-showroom price</p>
                        <% } %>
                        <% else
                               if (!isCitySelected)
                               {%>
                        <p class="font14 fillPopupData margin-top10">
                            Ex-showroom price in <span href="javascript:void(0)" class="text-light-grey clear">
                                <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></span>
                            <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity">
                                <span class="bwmsprite loc-change-blue-icon"></span>
                            </a>
                            <% } %>
                            <% else
                                   if (!isOnRoadPrice)
                                   {%>
                            <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">
                                Ex-showroom price in <span class="font14 text-grey"><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity"><span class="bwmsprite loc-change-blue-icon"></span></a>
                            </p>
                            <% } %>
                            <% else
                                   {%>
                            <p class="margin-top20 margin-bottom10 font14 text-light-grey clear">
                                On-road price in <span class="font14 text-grey "><%= areaName %> <%= cityName %></span>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData margin-left5 changeCity"><span class="bwmsprite loc-change-blue-icon"></span></a>
                            </p>
                            <% } %>
                            <% if (totalDiscountedPrice != 0)
                               { %>
                            <p>
                                <span class="offertxt strike padding-right10 font14"><span class="fa fa-rupee margin-right5"></span>
                                    <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(onRoadPrice)) %>
                                </span>
                                (<span class="offertxt red-font text-bold font14"><span class="fa fa-rupee"></span>
                                    <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(totalDiscountedPrice)) %>
                                     Off</span>)
                            </p>
                             <% } %>
                            <span itemprop="name" class="hide"><%= bikeName %></span>
                            <div itemprop="offers" itemscope itemtype="http://schema.org/Offer">
                                <p class="leftfloat">

                                    <%if (price != "0" && price != string.Empty)
                                      { %>

                                    <span class="font24 text-bold">
                                        <span itemprop="priceCurrency" content="INR"><span class="fa fa-rupee"></span></span>
                                        <span itemprop="price" content="<%=price %>">
                                            <%= Bikewale.Utility.Format.FormatPrice(price) %>
                                        </span>
                                    </span>

                                    <% }
                                      else
                                      { %>
                                    <span class="font20 text-bold">Price unavailable</span>
                                    <%  } %>
                                </p>
                            </div>
                            <%if (isOnRoadPrice)
                              {%>
                            <p id="viewBreakupText" class="font14 text-light-grey leftfloat viewBreakupText">View Breakup</p>
                            <%if (isBikeWalePQ && price != "0" && price != string.Empty)
                              {%>
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

                    <% if (toShowOnRoadPriceButton)
                       {%>
                        <div class="clear"></div>
                        <div id="benefitsOfBookingContainer" class="padding-top10 padding-bottom10">
                            <div class="padding-bottom20 border-light-bottom">
                                <p class="font18 text-bold">Benefits of booking online</p>
                                <a href="javascript:void(0)" ismodel="true" modelid='<%= modelId %>' class="fillPopupData font14">Available in Mumbai, Pune & Bangalore</a>
                            </div>
                            <ul>
                                <li>
                                    <div class="benefits-item">
                                        <span class="model-sprite benefit-offers-ico margin-right15"></span>
                                    </div>
                                    <div class="benefits-item text-uppercase">
                                        <h2>Exclusive</h2>
                                        <span>Offers</span>
                                    </div>
                                </li>
                                <li class="benefits-dealer-visits">
                                    <div class="benefits-item">
                                        <span class="model-sprite benefit-dealer-visits-ico margin-right10"></span>
                                    </div>
                                    <div class="benefits-item text-uppercase">
                                        <h2>Save on</h2>
                                        <span>Dealer visits</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="benefits-item">
                                        <span class="model-sprite benefit-assistance-ico margin-right15"></span>
                                    </div>
                                    <div class="benefits-item text-uppercase">
                                        <h2>Complete</h2>
                                        <span>Buying assistance</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="benefits-item">
                                        <span class="bwmsprite cancel-policy-lg-icon margin-right15"></span>
                                    </div>
                                    <div class="benefits-item text-uppercase">
                                        <h2>Easy</h2>
                                        <span>Cancellation</span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    <%} %>
                    
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
                                <ul class="offersList" style="list-style: none">
                                    <asp:Repeater ID="rptOffers" runat="server">
                                        <ItemTemplate>
                                            <li class="offertxt float-left">
                                                <span style='display: inline;' class="show"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %></span>
                                                <%# "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>"  %>
                                                <%--<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span  class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>--%>
                                                <%--<% if (pqOnRoad.DPQOutput.objOffers.Count > 2)
                                                   { %>
                                                <%# Container.ItemIndex >  0 ? "<a class='viewMoreOffersBtn'>(view more)</a>" : "" %>
                                                <% } %>--%>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <%--<ul class="moreOffersList hide" style="list-style: none">
                                    <asp:Repeater ID="rptMoreOffers" runat="server">
                                        <ItemTemplate>
                                            <li class="offertxt float-left">
                                                <span style="display: inline;" class="show"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "offerText")) %></span>
                                                <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span  class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>--%>
                                <% } %>

                                <%= (isOfferAvailable)?"<div class=\"border-top1 margin-top10 margin-bottom10\"></div>":string.Empty %>
                                <h4 class="border-solid-bottom padding-bottom5 margin-bottom10"><span class="bwmsprite disclaimer-icon margin-right5"></span>Get following details on the bike</h4>
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
                    <% if (bookingAmt > 0 && isDealerAssitance)
                           { %>
                            <div class="grid-12 alpha omega margin-bottom20">
                                <input type="button" value="Book now" class="btn btn-grey btn-full-width btn-sm rightfloat" id="bookNowBtn" />
                            </div>
                        <%} %>
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
            <% if (!isDiscontinued)
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
                        <% if (bookingAmt > 0 && !isDealerAssitance)
                           { %>
                            <div class="grid-5 omega">
                                <input type="button" value="Book now" class="btn btn-grey btn-full-width btn-sm rightfloat" id="bookNowBtn" />
                            </div>
                        <%} %>

                        <% if (pqOnRoad != null && pqOnRoad.IsDealerPriceAvailable)
                           { %>
                        <div class="grid-<%=btMoreDtlsSize %> ">
                            <input type="button" value="Get more details" class="btn btn-full-width btn-sm margin-right10 leftfloat <%= (isDealerAssitance && bookingAmt > 0) ? "btn-grey" : "btn-orange"   %>" id="getMoreDetailsBtn" />
                        </div>
                        <%} %>

                        <% if (bookingAmt > 0 && isDealerAssitance)
                           { %>
                        <div class="grid-5 alpha omega">
                            <a class="btn btn-orange btn-full-width btn-sm rightfloat" href="tel:+919167969266"><span class="fa fa-phone margin-right5"></span>Call dealer</a>
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
                                                     <% if (cityId != 0)
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
                <h2 class="text-bold text-center margin-top30 margin-bottom20 font24">What do our customers say</h2>
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


        <% if (bookingAmt > 0)
           { %>
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
                            <p class="question-subtitle text-light-grey font14">
                                We’re here to help.<br />
                                Read our <a href="/m/faq.aspx" target="_blank">FAQs</a>, <a href="mailto:contact@bikewale.com">email</a> or call us on <a href="tel:18001208300" class="text-dark-grey">1800 120 8300</a>
                            </p>
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
                                        <% if (!pqOnRoad.IsInsuranceFree)
                                           { %>
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
                                <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(onRoadPrice -totalDiscountedPrice)) %>

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
            <div class="fixed-close-btn-wrapper">
                <div class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <h3>Terms and Conditions</h3>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="/images/search-loading.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class="hide">
                <h1>Offers and Gifts Promotion Terms and Conditions</h1>
                <p><strong>Definitions:</strong></p>
                <p>"BikeWale" refers to Automotive Exchange Private Limited, a private limited company having its head office at 12<sup>th</sup> Floor, Vishwaroop IT Park, Sector 30A, Vashi, Navi Mumbai 400705, India, who owns and operates www.bikewale.com, one of India's leading automotive web portals.</p>
                <p>"Bike Manufacturer" or "manufacturer" refers to the company that manufactures and / or markets and sells bikes in India through authorised dealers.</p>
                <p>"Dealership" or "dealer" refers to companies authorised by a Bike Manufacturer to sell their bikes. Each Bike Manufacturer many have more than one Dealership and / or Dealer.</p>
                <p>"Offer" refers to the promotions, discounts and gifts that are available as displayed on BikeWale.</p>
                <p>"Buyer" or "user" or "participant" refers to the individual who purchases a Bike and / or avails any of the offers.</p>
                <p><strong>Offers from Bike Manufacturers and Dealers</strong></p>
                <p>1. All offers are from Bike manufacturers and / or their dealers, and BikeWale makes no representation or warranty regarding the accuracy, truth, quality, suitability or reliability of such information.</p>
                <p>2. These terms and conditions are to be read in conjunction with the terms and conditions of the manufacturers / dealers. Please refer to the manufacturers and / or their dealers' websites for a detailed list of terms and conditions that apply to these offers.</p>
                <p>3. In the event of any discrepancy between the manufacturers / dealers' offer terms and conditions, and the terms and conditions mentioned herewith, the manufacturers / dealers' terms and conditions will apply.</p>
                <p>4. All questions, clarifications, complaints and any other communication pertaining to these offers should be addressed directly to the manufacturer and / or their dealers. BikeWale will not be able to entertain any communication in this regard.</p>
                <p>5. The offers may be modified and / or withdrawn by manufacturers and / or their dealers without notice, and buyers are strongly advised to check the availability and detailed terms and conditions of the offer before making a booking.</p>
                <p>6. Buyers are strongly advised to verify the offer details with the manufacturer and / or the nearest dealer before booking the bike.</p>
                <p>7. Any payments made towards purchase of the Bike are governed by the terms and conditions agreed between the buyer and the manufacturer and / or the dealer. BikeWale is in no way related to the purchase transaction and cannot be held liable for any refunds, financial loss or any other liability that may arise directly or indirectly out of participating in this promotion.</p>
                <p><strong>Gifts from BikeWale</strong></p>
                <p>8. In select cases, BikeWale may offer a limited number of free gifts to buyers, for a limited period only, over and above the offers from Bike manufacturers and / or their dealers. The quantity and availability period (also referred to as 'promotion period' hereafter) will be displayed prominently along with the offer and gift information on www.bikewale.com.</p>
                <p>9. These free gifts are being offered solely by BikeWale, and entirely at BikeWale's own discretion, without any additional charges or fees to the buyer.</p>
                <p>10. In order to qualify for the free gift, the buyer must fulfil the following:</p>
                <div class="margin-left20 margin-top10">
                    <p>a. Be a legally recognised adult Indian resident, age eighteen (18) years or above as on 01 Dec 2014, and be purchasing the Bike in their individual capacity</p>
                    <p>b. Visit www.bikewale.com and pay the booking amount online against purchase of selected vehicle from BikeWale’s assigned dealer.</p>
                    <p>c. Complete all payment formalities and take delivery of the bike from the same dealership. </p>
                    <p>d. Inform BikeWale through any of the means provided about the completion of the delivery of the bike.</p>

                </div>
                <p>11. By virtue of generating an offer code and / or providing BikeWale with Bike booking and / or delivery details, the buyer agrees that s/he is:</p>
                <div class="margin-left20 margin-top10">
                    <p>a. Confirming his/her participation in this promotion; and</p>
                    <p>b. Actively soliciting contact from BikeWale and / or Bike manufacturers and / or dealers; and</p>
                    <p>c. Expressly consenting for BikeWale to share the information they have provided, in part or in entirety, with Bike manufacturers and / or dealers, for the purpose of being contacted by them to further assist in the Bike buying process; and</p>
                    <p>d. Expressly consenting to receive promotional phone calls, emails and SMS messages from BikeWale, Bike manufacturers and / or dealers; and</p>
                    <p>e. Expressly consenting for BikeWale to take photographs and record videos of the buyer and use their name, photographs, likeness, voice and comments for advertising, promotional or any other purposes on any media worldwide and in any way as per BikeWale's discretion throughout the world in perpetuity without any compensation to the buyer whatsoever; and</p>
                    <p>f. Confirming that, on the request of BikeWale, s/he shall also make arrangements for BikeWale to have access to his / her residence, work place, favourite hangouts, pets etc. and obtain necessary permissions from his / her parents, siblings, friends, colleagues to be photographed, interviewed and to record or take their photographs, videos etc. and use this content in the same manner as described above; and</p>
                    <p>g. Hereby agreeing to fully indemnify BikeWale against any claims for expenses, damages or any other payments of any kind, including but not limited to that arising from his / her actions or omissions or arising from any representations, misrepresentations or concealment of material facts; and</p>
                    <p>h. Expressly consenting that BikeWale may contact the Bike manufacturer and / or dealer to verify the booking and / or delivery details provided by the buyer; and</p>
                    <p>i. Waiving any right to raise disputes and question the process of allocation of gifts</p>
                </div>
                <p>12. Upon receiving complete booking and delivery details from the buyer, BikeWale may at its own sole discretion verify the details provided with the Bike manufacturer and / or dealer. The buyer will be eligible for the free gift only if the details can be verified as matching the records of the manufacturer and / or dealer.</p>
                <p>13. The gifts will be allocated in sequential order at the time of receiving confirmed booking details. Allocation of a gift merely indicates availability of that specific gift for the selected Bike at that specific time, and does not guarantee, assure or otherwise entitle the buyer in any way whatsoever to receive the gift. Allocation of gifts will be done entirely at BikeWale's own sole discretion. BikeWale may change the allocation of gifts at their own sole discretion without notice and without assigning a reason.</p>
                <p>14. The quantity of gifts available, along with the gift itself, varies by Bike and city. The availability of gifts displayed on www.bikewale.com is indicative in nature. Buyers are strongly advised to check availability of gifts by contacting BikeWale via phone before booking the bike.</p>
                <p>15. The gift will be despatched to buyers only after the dealer has confirmed delivery of the bike.</p>
                <p>16. Gifts will be delivered to addresses in India only. In the event that delivery is not possible at certain locations, BikeWale may at its own sole discretion, accept an alternate address for delivery, or arrange for the gift to be made at the nearest convenient location for the buyer to collect.</p>
                <p>17. Ensuring that the booking and / or delivery information reaches BikeWale in a complete and timely manner is entirely the responsibility of the buyer, and BikeWale, Bike manufacturers, dealers and their employees and contracted staff cannot be held liable for incompleteness of information and / or delays of any nature under any circumstances whatsoever.</p>
                <p>18. The buyer must retain the offer code, booking confirmation form, invoice of the bike, and delivery papers provided by the dealer, and provide any or all of the same on demand along with necessary identity documents and proof of age. BikeWale may at its own sole discretion declare a buyer ineligible for the free gift in the event the buyer is not able to provide / produce any or all of the documents as required.</p>
                <p>19. In the event of cancellation of a booking, or if the buyer fails to take delivery of the Bike for any reason, the buyer becomes ineligible for the gift.</p>
                <p>20. BikeWale's sole decision in all matters pertaining to the free gift, including the choice and value of product, is binding and non-contestable in all respects.</p>
                <p>21. The buyer accepts and agrees that BikeWale, Bike manufacturers, dealers and other associates of BikeWale, including agencies and third parties contracted by BikeWale, and / or their directors, employees, officers, affiliates or subsidiaries, cannot be held liable for any damage or loss, including but not limited to lost opportunity, lost profit, financial loss, bodily harm, injuries or even death, directly or indirectly, arising out of the use or misuse of the gift, or a defect of any nature in the gift, or out of participating in this promotion in any way whatsoever.</p>
                <p>22. The buyer specifically agrees not to file in person / through any family member and / or any third party any applications, criminal and/or civil proceedings in any courts or forum in India against BikeWale, Bike manufacturers, dealers and other associates of BikeWale, including agencies and third parties contracted by BikeWale, and/or their directors, employees, officers, affiliates or subsidiaries, and / or their directors, employees, officers, affiliates or subsidiaries to claim any damages or relief in connection with this promotion.</p>
                <p>23. All gifts mentioned, including the quantity available, are indicative only. Pictures are used for representation purposes only and may not accurately depict the actual gift.</p>
                <p>24. BikeWale reserves the right to substitute any gift with a suitable alternative or provide gift vouchers of an equivalent value to the buyer, without assigning a reason for the same. Equivalent value of the gift shall be determined solely by BikeWale, irrespective of the market / retail / advertised prices or Maximum Retail Price (MRP) of the product at the time of despatch of the gift. An indicative “gift value” table is provided below.</p>
                <p>25. Delivery of the product shall be arranged through a third party logistics partner and BikeWale is in no way or manner liable for any damage to the product during delivery.</p>
                <p>26. Warranty on the gift, if any, will be provided as per the gift manufacturer's terms and directly by the gift manufacturer.</p>
                <p>27. Gifts cannot be transferred or redeemed / exchanged for cash.</p>
                <p>28. Income tax, gift tax and / or any other statutory taxes, duties or levies as may be applicable from time to time, arising out of the free gifts, shall be payable entirely by the buyer on his/her own account.</p>
                <p>29. BikeWale makes no representation or warranties as to the quality, suitability or merchantability of any of the gifts whatsoever, and no claim or request, whatsoever, in this respect shall be entertained.</p>
                <p>30. Certain gifts may require the buyer to incur additional expenses such as installation expenses or subscription fees or purchasing additional services, etc. The buyer agrees to bear such expenses entirely on their own account.</p>
                <p>31. Availing of the free gift and offer is purely voluntary. The buyer may also purchase the Bike without availing the free gift and / or the offer.</p>
                <p>32. For the sake of clarity it is stated that the Bike manufacturer and / or dealer shall not be paid any consideration by BikeWale to display their offers and / or offer free gifts for purchasing bikes from them. Their only consideration will be the opportunity to sell a Bike to potential Bike buyers who may discover their offer on www.bikewale.com.</p>
                <p>33. Each buyer is eligible for only one free gift under this promotion, irrespective of the number of bikes they purchase.</p>
                <p>34. This promotion cannot be used in conjunction with any other offer, promotion, gift or discount scheme.</p>
                <p>35. In case of any dispute, BikeWale's decision will be final and binding and non-contestable. The existence of a dispute, if any, does not constitute a claim against BikeWale.</p>
                <p>36. This promotion shall be subject to jurisdiction of competent court/s at Mumbai alone.</p>
                <p>37. Employees of BikeWale and their associate / affiliate companies, and their immediate family members, are not eligible for any free gifts under this promotion.</p>
                <p>38. This promotion is subject to force majeure circumstances i.e. Act of God or any circumstances beyond the reasonable control of BikeWale.</p>
                <p>39. Any and all information of the buyers or available with BikeWale may be shared with the government if any authority calls upon BikeWale / manufacturers / dealers to do so, or as may be prescribed under applicable law.</p>
                <p>40. In any case of any dispute, inconvenience or loss, the buyer agrees to indemnify BikeWale, its representing agencies and contracted third parties without any limitation whatsoever.</p>
                <p>41. The total joint or individual liability of BikeWale, its representing agencies and contracted third parties, along with Bike manufacturers and dealers, will under no circumstances exceed the value of the free gift the buyer may be eligible for.</p>
                <p>42. BikeWale reserves the right to modify any and all of the terms and conditions mentioned herein at its own sole discretion, including terminating this promotion, without any notice and without assigning any reason whatsoever, and the buyers agree not to raise any claim due to such modifications and / or termination.</p>
                <p>By participating in this promotion, the buyer / user agrees to the terms and conditions above in toto.</p>
            </div>
        </div>
        <!-- Terms and condition Popup end -->

        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />

        <!-- all other js plugins -->
        <script>


            var imgTitle, imgTotalCount;
            var leadBtnBookNow = $("#leadBtnBookNow"), leadCapturePopup = $("#leadCapturePopup");
            var fullname = $("#getFullName");
            var emailid = $("#getEmailID");
            var mobile = $("#getMobile");
            var otpContainer = $(".mobile-verification-container");

            var detailsSubmitBtn = $("#user-details-submit-btn");
            var otpText = $("#getOTP");
            var otpBtn = $("#otp-submit-btn");


            $("#leadBtnBookNow").on('click', function () {
                leadCapturePopup.show();
                $('body').addClass('lock-browser-scroll');
                $(".blackOut-window").show();
                /*
                $(document).on('keydown', function (e) {
                    if (e.keyCode === 27) {
                        $("#leadCapturePopup .leadCapture-close-btn").click();
                    }
                });
                */
            });

            var leadPopupClose = function () {
                leadCapturePopup.hide();
                $("#contactDetailsPopup").show();
                $("#otpPopup").hide();
                $('body').removeClass('lock-browser-scroll');
                $(".blackOut-window").hide();
            };

            $(".leadCapture-close-btn, .blackOut-window").on("click", function () {
                $("#leadCapturePopup").hide();
                $('body').removeClass('lock-browser-scroll');
                $(".blackOut-window").hide();
                window.history.back();
            });


            $('#getMoreDetailsBtn').on('click', function (e) {
                $("#leadCapturePopup").show();
                $(".blackOut-window").show();
                appendHash("contactDetails");
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Clicked', 'lab': bikeVersionLocation });
            });


            $("#viewBreakupText").on('click', function (e) {
                $("div#breakupPopUpContainer").show();
                $(".blackOut-window").show();
                appendHash("viewBreakup");
            });
            $(".breakupCloseBtn,.blackOut-window").on('click', function (e) {
                viewBreakUpClosePopup();
                window.history.back();
            });

            var viewBreakUpClosePopup = function () {
                $("div#breakupPopUpContainer").hide();
                $(".blackOut-window").hide();
                $("#contactDetailsPopup").show();
                $("#otpPopup").hide();
                leadPopupClose();
            };

            $(".termsPopUpCloseBtn, .blackOut-window").on('mouseup click', function (e) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            });

            $(document).on('keydown', function (e) {
                if (e.keyCode === 27) {
                    $("div.breakupCloseBtn").click();
                    $("div.termsPopUpCloseBtn").click();
                    $("div.leadCapture-close-btn").click();
                    leadPopupClose();
                    $("div#termsPopUpContainer").hide();
                    $(".blackOut-window").hide();
                }
            });



            $(".more-features-btn").click(function () {
                $(".more-features").slideToggle();
                var a = $(this).find("a");
                a.text(a.text() === "+" ? "-" : "+");
                if (a.text() === "+")
                    a.attr("href", "#features");
                else a.attr("href", "javascript:void(0)");
            });

            $("a.read-more-btn").click(function () {
                if (!$(this).hasClass("open")) {
                    $(".model-about-main").hide();
                    $(".model-about-more-desc").show();
                    var a = $(this).find("span");
                    a.text(a.text() === "full story" ? "less" : "full story");
                    $(this).addClass("open");
                }
                else if ($(this).hasClass("open")) {
                    $(".model-about-main").show();
                    $(".model-about-more-desc").hide();
                    var a = $(this).find("span");
                    a.text(a.text() === "full story" ? "less" : "full story");
                    $(this).removeClass("open");
                }

            });


            $('#bookNowBtn').on('click', function (e) {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Book_Now_Clicked', 'lab': bikeVersionLocation });
                var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                window.location.href = "/m/pricequote/bookingSummary_new.aspx?MPQ=" + Base64.encode(cookieValue);;
            });

        </script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
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
            $(document).ready(function (e) {
                if ($('#getMoreDetailsBtn').length > 0) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Shown', 'lab': myBikeName + '_' + getBikeVersion() + '_' + getCityArea });
                }
                if ($('#btnGetOnRoadPrice').length > 0) {
                    dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_On_Road_Price_Button_Shown', 'lab': myBikeName + '_' + getBikeVersion() });
                }
            });

        </script>
    </form>
</body>
</html>

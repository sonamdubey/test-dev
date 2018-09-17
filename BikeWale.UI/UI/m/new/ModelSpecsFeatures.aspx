<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.ModelSpecsFeatures" EnableViewState="false" %>

<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/UI/m/controls/GenericBikeInfoControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%
        description = String.Format("Know more about {0} Specifications and Features. See details about mileage, engine displacement, power, kerb weight and other specifications.", bikeName);
        title = pgTitle;
        canonical = String.Format("https://www.bikewale.com/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        keywords = string.Format("{0} specifications, {0} specs, {0} features, {0} mileage, {0} fuel efficiency", bikeName);
        EnableOG = true;
        OGImage = modelImage;
    %>
    <!-- #include file="/UI/includes/headscript_mobile.aspx" -->
    <style type="text/css">

        .content-inner-block-1420{padding:14px 20px}.content-inner-block-120{padding:10px 20px 0}.text-dark-black{color:#1a1a1a}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.model-price-content{width:110px}.model-area-content{width:60%;position:relative;top:6px}.specs-features-wrapper{height:44px}#specsFeaturesTabsWrapper{width:100%;background:#fff;z-index:3;display:block;border-bottom:1px solid #e2e2e2;overflow-x:auto}.model-specs-features-tabs-wrapper{display:table;background:#fff}.model-specs-features-tabs-wrapper li{padding:10px 20px;display:table-cell;text-align:center;white-space:nowrap;font-size:14px;color:#82888b}.model-specs-features-tabs-wrapper li.active{border-bottom:3px solid #ef3f30;font-weight:bold;color:#4d5057}.border-divider{border-top:1px solid #e2e2e2}.specs-features-list{overflow:hidden}.specs-features-list li{margin-bottom:20px}.specs-features-list p{width:50%;float:left;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.specs-features-label{color:#82888b}.specs-features-value{padding-left:20px;font-weight:bold}.fixed-topNav{position:fixed;top:0;left:0}.float-button{background-color:#fff;padding:10px}.float-button.float-fixed{position:fixed;bottom:0;z-index:8;left:0;right:0;background:#f5f5f5}.star-span{position:relative;top:-2px;transform:scale(.9)}.border-light-left{border-left:1px solid #ededed}.star-icon{width:26px;height:25px;vertical-align:middle;-webkit-transform:scale(.8);-moz-transform:scale(.8);-o-transform:scale(.8);transform:scale(.8)}.star-icon.star-size-16{-webkit-transform:scale(.6);-moz-transform:scale(.6);-o-transform:scale(.6);transform:scale(.6)}.rate-count-1 .star-icon{background-position:0 -667px}.rate-count-2 .star-icon{background-position:-38px -667px}.rate-count-3 .star-icon{background-position:0 -278px}.rate-count-4 .star-icon{background-position:-74px -667px}.rate-count-5 .star-icon{background-position:-110px -667px}.rate-count-5{color:#198D55}.rate-count-4{color:#13B65D}.rate-count-3{color:#EFD700}.rate-count-2{color:#FD971C}.rate-count-1{color:#EF3F30}.review-left-divider{position:relative;margin-left:15px;padding-left:15px}.review-left-divider:before{content:"";position:absolute;top:4px;left:0;width:1px;height:12px;border-left:1px solid #e2e2e2}.breadcrumb{margin:10px;padding:10px;background:#eee}.breadcrumb-title{display:block;font-size:12px;color:#82888B;padding-bottom:5px}.breadcrumb li{position:relative;display:inline-block;vertical-align:middle;margin-right:5px;margin-bottom:10px}.breadcrumb-link::after{content:'';height:5px;width:5px;border:solid grey;border-width:0px 1px 1px 0px;-webkit-transform:rotate(-45deg);-moz-transform:rotate(-45deg);-o-transform:rotate(-45deg);transform:rotate(-45deg);display:inline-block;position:absolute;top:7px;right:0px}.breadcrumb-link__label{padding-right:12px}.swiper-type-similar .swiper-btn-block{padding-bottom:15px}.swiper-type-similar .right-arrow{width:8px;height:12px;background-position:-182px -394px;position:absolute;right:18px;top:13px;transform:scale(0.8)}.compare-with-target{display:block;padding:10px 35px 10px 15px;position:relative;font-size:12px;color:#4d5057}.compare-with-target:before{content:'';position:absolute;left:8%;top:0;width:168px;height:1px;border-top:1px solid #e2e2e2}.compare-sm{width:17px;height:12px;background-position:-173px -596px;position:relative;top:1px;margin-right:5px}.exploremore__imagebackground { padding-top: 15px; padding-bottom: 10px; background: rgba(65,180,196,0.3); }.exploremore__icon-background { height: 64px; width: 64px; background: white url(https://imgd.aeplcdn.com/0x0/bw/static/icons/41b4c4/bike-36x22.png) no-repeat center; margin: 0 auto; -moz-border-radius: 50%; -webkit-border-radius: 50%; border-radius: 50%; }.exploremore-detailblock { margin: 0 auto; padding-top: 10px; min-height: 115px; text-align: center; }.detailblock__title { font-size: 14px; color: #4D5057; font-weight: 600; line-height: 25px; margin-bottom: 10px; }.detailblock__description { font-size: 12px; color: #A2A2A2; }

    </style>

</head>
<body>
    <form runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <section class="bg-white box-shadow margin-bottom10">
            <div id="modelPriceDetails" class="content-inner-block-120">

                  <% if (Bikewale.Utility.BWConfiguration.Instance.MetasMakeId.Split(',').Contains(_makeId.ToString()))
                     {%> 
                
                 <h1 class="margin-bottom5">Specifications & Feature of <%= bikeName%></h1>
                
                <%}else{ %>
                 <h1 class="margin-bottom5"><%= bikeName %> Specifications and Features</h1>
                <%} %>

               

                <% if (price > 0)
                    { %>
                <p class="font20 text-bold model-price-content leftfloat">
                    <span class="bwmsprite inr-sm-icon"></span>&nbsp;<%= Bikewale.Utility.Format.FormatPrice(price.ToString())%>
                </p>
                <% }
                    else
                    { %>
                <p>Price not available</p>
                <% } %>
                <% if (price > 0)
                    { %>
                <%if (isDiscontinued)
                    { %>
                <p class="font14 text-truncate text-light-grey model-area-content leftfloat">Last known Ex-showroom Price</p>
                <% }
                    else if (!IsExShowroomPrice)
                    { %>
                <p class="font14 text-truncate text-light-grey model-area-content leftfloat">On-road price in <%= string.IsNullOrEmpty(areaName) ? cityName : string.Format("{0}, {1}", areaName, cityName)%></p>
                <% }
                    else
                    { %>
                <p class="font14 text-truncate text-light-grey model-area-content leftfloat">Ex-showroom price in <%= string.IsNullOrEmpty(areaName) ? cityName : string.Format("{0}, {1}", areaName, cityName)%></p>
                <%} %>
                <%} %>
                <div class="clear"></div>
            </div>

            <% if (versionSpecsFeatures != null)
                { %>
            <div class="specs-features-wrapper">
                <div id="specsFeaturesTabsWrapper">
                    <ul class="model-specs-features-tabs-wrapper">
                        <li class="active" data-tabs="#specs">Specifications</li>
                        <li data-tabs="#features">Features</li>
                    </ul>
                </div>
            </div>

            <div id="specsFeaturesDetailsWrapper" class="padding-right20 padding-left20 font14">
                <div id="specs" class="bw-model-tabs-data padding-top15">
                    <% string itemValue, featureValue; %>
                    <% if (versionSpecsFeatures.Specs != null && versionSpecsFeatures.Specs.Any())
                        { %>
                    <h2 class="margin-bottom15">Specifications</h2>
                    <% foreach (var specCat in versionSpecsFeatures.Specs)
                        { %>
						<h3 class="margin-bottom20"><%=specCat.DisplayText %></h3>
						<ul class="specs-features-list margin-bottom5">
                            <% foreach (var specItem in specCat.SpecsItemList)
                                    {
                                        itemValue = specItem.ItemValues.FirstOrDefault();
                                    %>
                                      <li>
                                           <p class="specs-features-label"><%=specItem.DisplayText%></p>
                                           <p class="specs-features-value"><span><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(itemValue, specItem.UnitTypeText, specItem.DataType, specItem.Id) %></span></p>
										  <div class="clear"></div>
                                        </li>
                                <% } %>
                        </ul>
                    <% } %>
                    <% } %>
                </div>
                <div class="border-divider"></div>
               
                <div id="features" class="padding-top15 bw-model-tabs-data">
                    <% if (versionSpecsFeatures.Features != null && versionSpecsFeatures.Features.Any())
                        { %>
                    <h2 class="margin-bottom20">Features</h2>
                    <% foreach (var feature in versionSpecsFeatures.Features)
                        { 
                            featureValue = feature.ItemValues.FirstOrDefault();
                            %>
                    <ul class="specs-features-list">
                        <li>
                                     <p class="specs-features-label"><%=feature.DisplayText %></p>
                                     <p class="specs-features-value"><span><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(featureValue, feature.UnitTypeText, feature.DataType, feature.Id) %></span></p>
                                     <div class="clear"></div>
                        </li>
                    </ul>
                    <% } %>
                    <% } %>
                  </div>
            </div>
            <div id="specsFeaturesFooter"></div>
            <% } %>
        </section>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin margin-bottom10">
                <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />
            </div>
        </section>

        <section>
            <div class="container">
                <% if (similarBikes != null && similarBikes.Bikes != null && similarBikes.Bikes.Any())
                    { %>
                <div id="similarContent" class="bw-model-tabs-data padding-top15 padding-bottom15 content-box-shadow card-bottom-margin content-details-wrapper">
                    <h2 class="padding-left20 padding-right20 font18"><%= bodyStyleText %> similar to <%= modelDetail.ModelDetails.ModelName %></h2>
                    <div class="swiper-container card-container swiper-type-similar">
                        <div class="swiper-wrapper">
                            <% if (similarBikes != null && similarBikes.Bikes != null && similarBikes.Bikes.Count() > 0)
                                {

                                    foreach (var bike in similarBikes.Bikes)
                                    {
                                        var bikeUrl = String.Format("/m{0}", Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName, bike.ModelBase.MaskingName)); %>
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="position-rel">
                                        <a href="<%= bikeUrl %>" title="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>">
                                            <div class="swiper-image-preview">
                                                <img class="swiper-lazy" data-src="<%=  Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl, Bikewale.Utility.ImageSize._210x118,Bikewale.Utility.QualityFactor._70) %>" title="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" alt="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <% if (bike.VersionPrice == 0 && bike.AvgExShowroomPrice == 0)
                                                { %>
                                            <p class="text-bold text-default">
                                                <span class="font14">Price not available</span>
                                            </p>
                                            <% }
                                                else
                                                { %>
                                            <div class="swiper-details-block">
                                                <p class="target-link font12 text-truncate margin-bottom5"><%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %></p>
                                                <p class="text-truncate text-light-grey font11"><%= (bike.VersionPrice > 0 ? String.Format("Ex-showroom, {0}", (!String.IsNullOrEmpty(bike.CityName) ? bike.CityName : Bikewale.Utility.BWConfiguration.Instance.GetDefaultCityName)) : "Avg. Ex-showroom price")%></p>
                                                <p class="text-default font16">
                                                    <span>&#x20B9;</span>&nbsp;<span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.VersionPrice > 0 ? bike.VersionPrice.ToString() : bike.AvgExShowroomPrice.ToString()) %></span>
                                                </p>
                                            </div>
                                            <% } %>
                                        </a>
                                        <% if (bike.VersionPrice == 0)
                                            { %>
                                        <span class="info-icon tooltip-icon-target tooltip-top tooltip--right">
                                            <span class="bw-tooltip info-tooltip">
                                                <span class="bw-tooltip-text"><%= string.Format("Price is not available in {0}", bike.CityName) %></span>
                                            </span>
                                        </span>
                                        <% } %>
                                    </div>
                                    <% if (similarBikes.ShowCheckOnRoadCTA)
                                        { %>
                                    <div class="swiper-btn-block">
                                        <a href="javascript:void(0)" data-pqsourceid="<%= ((int)similarBikes.PQSourceId) %>" data-modelid="<%= bike.ModelBase.ModelId %>" class="btn btn-card btn-full-width btn-white getquotation" rel="nofollow">Check on-road price</a>
                                    </div>
                                    <% }
                                        if (similarBikes.ShowPriceInCityCTA)
                                        { %>
                                    <div class="swiper-btn-block">
                                        <a href="<%= String.Format("/m{0}", Bikewale.Utility.UrlFormatter.PriceInCityUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName,bike.CityMaskingName)) %>" class="btn btn-card btn-full-width btn-white text-truncate getquotation font12" rel="nofollow" title="<%= String.Format("{0} {1} On-road price in {2}",makeName,modelName,bike.CityName)%>">On-road price in <%= bike.CityName %></a>
                                    </div>
                                    <% }
                                        if (similarBikes.Make != null && similarBikes.Model != null && similarBikes.IsNew)
                                        {
                                            string fullUrl = string.Format("/m/{0}", Bikewale.Utility.UrlFormatter.CreateCompareUrl(similarBikes.Make.MaskingName, similarBikes.Model.MaskingName, bike.MakeBase.MaskingName, bike.ModelBase.MaskingName, Convert.ToString(similarBikes.VersionId),  Convert.ToString(bike.VersionBase.VersionId), (uint)similarBikes.Model.ModelId, (uint)bike.ModelBase.ModelId, Bikewale.Entities.Compare.CompareSources.Mobile_Model_MostPopular_Compare_Widget));
                                             %>
                                    <a class="compare-with-target text-truncate redirect-url" href="<%= Bikewale.Utility.UrlFormatter.RemoveQueryString(fullUrl) %>" data-url="<%= fullUrl  %>" title="<%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.ModelBase.ModelName, similarBikes.Model.ModelName) %>">
                                        <span class="bwmsprite compare-sm"></span>Compare with <%= similarBikes.Model.ModelName %><span class="bwmsprite right-arrow"></span>
                                    </a>
                                    <% } %>
                                </div>
                            </div>
                            <% } %>
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="<%= string.Format("/m{0}",(similarBikes.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter))? "/scooters/" : "/new-bikes-in-india/") %>" title="<%= (string.Format("Explore more {0}", (similarBikes.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter))? "scooters" : "bikes"))%>" class="bw-ga" c="<%= similarBikes.Page %>" a="Clicked_ExploreMore_Card" l="<%= similarBikes.Model.ModelName %>">
                                        <div class="swiper-image-preview">
                                            <div class="exploremore__imagebackground">
                                                <div class="exploremore__icon-background">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="swiper-details-block">
                                            <div class="exploremore-detailblock">
                                                <p class="detailblock__title">Couldn’t find what you were looking for?</p>
                                                <p class="detailblock__description"><%= (similarBikes.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter) ? "View 60+ scooters from over 10 brands" : " View 200+ bikes from over 30 brands") %></p>
                                            </div>
                                        </div>
                                        <% if (similarBikes.IsNew)
                                            { %>
                                        <div class="compare-with-target text-truncate">
                                            <%= (string.Format("Explore more {0}", (similarBikes.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)) ? "scooters" : "bikes"))%><span class="bwmsprite right-arrow"></span>
                                        </div>
                                        <% } %>
                                    </a>
                                </div>
                            </div>

                            <% } %>
                        </div>
                    </div>
                </div>
                <% }
                    else if ((popularBodyStyle != null && popularBodyStyle.PopularBikes != null && popularBodyStyle.PopularBikes.Any()) && (similarBikes.IsNew || similarBikes.IsUpcoming))

                    { %>
                <div id="popularContent" class="bw-model-tabs-data container bg-white box-shadow padding-bottom20 card-bottom-margin">
                    <div class="carousel-heading-content padding-top15">
                        <div class="swiper-heading-left-grid inline-block">
                            <h2>Other popular <%= popularBodyStyle.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)? "scooters" : "bikes" %></h2>
                        </div>
                        <div class="swiper-heading-right-grid inline-block text-right">
                            <a href="<%= String.Format("/m{0}", Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(popularBodyStyle.BodyStyle)) %>" title="Best <%= popularBodyStyle.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)? "scooters" : "bikes" %> in India" class="btn view-all-target-btn">View all</a>
                        </div>
                    </div>
                    <div class="swiper-container card-container swiper-small other-popular-bikes-swiper">
                        <div class="swiper-wrapper">
                            <% foreach (var bike in popularBodyStyle.PopularBikes)
                                { %>
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="position-rel">
                                        <a href="<%= string.Format("/m{0}",Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName)) %>" title="<%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %>">
                                            <div class="swiper-image-preview position-rel">
                                                <img class="swiper-lazy" alt="<%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %>" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath, bike.HostURL, Bikewale.Utility.ImageSize._174x98, Bikewale.Utility.QualityFactor._70) %>" title="<%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName)%>">
                                            </div>
                                            <div class="swiper-details-block">
                                                <h3 class="target-link font12 text-truncate margin-bottom5"><%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %></h3>
                                                <% if (bike.VersionPrice == 0 && bike.AvgPrice == 0)
                                                    { %>
                                                <p class="text-default">
                                                    <span class="font14 text-light-grey">Price not available</span>
                                                </p>
                                                <% }
                                                    else
                                                    { %>
                                                <p class="text-truncate text-light-grey font11"><%= (bike.VersionPrice > 0 ? string.Format("Ex-showroom, {0}", (!string.IsNullOrEmpty(bike.CityName) ? bike.CityName : Bikewale.Utility.BWConfiguration.Instance.GetDefaultCityName)) : "Avg. Ex-showroom price") %></p>
                                                <p class="text-default font16">
                                                    <span>&#x20B9;</span>&nbsp;<span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice > 0 ? bike.VersionPrice : bike.AvgPrice)) %></span>
                                                </p>
                                                <% } %>
                                            </div>
                                        </a>
                                        <% if (bike.VersionPrice == 0)
                                            { %>
                                        <span class="info-icon tooltip-icon-target tooltip-top tooltip--right">
                                            <span class="bw-tooltip info-tooltip">
                                                <span class="bw-tooltip-text"><%= string.Format("Price is not available in {0}", bike.CityName) %></span>
                                            </span>
                                        </span>
                                        <% } %>
                                    </div>
                                    <% if (popularBodyStyle.ShowCheckOnRoadCTA)
                                        { %>
                                    <div class="swiper-btn-block">
                                        <a href="javascript:void(0)" data-pqsourceid="<%= ((int)popularBodyStyle.PQSourceId) %>" data-modelid="<%= bike.objModel.ModelId %>" class="btn btn-card btn-full-width btn-white getquotation" rel="nofollow">Check on-road price</a>
                                    </div>
                                    <% } %>
                                </div>
                            </div>
                            <% } %>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <% } %>
            </div>
        </section>

        <section>
            <div class="breadcrumb">
                <span class="breadcrumb-title">You are here:</span>
                <ul>
                    <% var url = string.Format("{0}/m/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl); %>

                    <li>
                        <a class="breadcrumb-link" href="/m/" title="Home">
                            <span class="breadcrumb-link__label" itemprop="name">Home</span>
                        </a>
                    </li>
                     <li>
                        <a class="breadcrumb-link" href="/m/new-bikes-in-india/" title="New Bikes">
                            <span class="breadcrumb-link__label" itemprop="name">New Bikes</span>
                        </a>
                    </li>
                    <li>
                        <a class="breadcrumb-link" href="/m/<%= makeMaskingName %>-bikes/" title="<%= makeName %> Bikes">
                            <span class="breadcrumb-link__label" itemprop="name"><%= makeName %> Bikes</span>
                        </a>
                    </li>
                    <% if (IsScooter && !IsScooterOnly)
                        { %>
                    <li>
                        <a class="breadcrumb-link" href="/m/<%= makeMaskingName %>-scooters/" title="<%= String.Format("{0} {1}", makeName, modelName) %>">
                            <span class="breadcrumb-link__label" itemprop="name"><%= makeName %> Scooters</span>
                        </a>
                    </li>
                    <%  }
                    %>

                    <% if (!string.IsNullOrEmpty(seriesUrl))
                        { %>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a class="breadcrumb-link" href="/m/<%= seriesUrl %>" title="<%= Series.SeriesName%>">
                                <span class="breadcrumb-link__label" itemprop="name"><%=Series.SeriesName %></span>
                            </a>
                        </li>
                    <% } %>

                    <li>
                        <a class="breadcrumb-link" href="/m/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" title="<%= String.Format("{0} {1}", makeName, modelName) %>">
                            <span class="breadcrumb-link__label" itemprop="name"><%= String.Format("{0} {1}", makeName, modelName)%></span>
                        </a>
                    </li>
                    <li>
                        <span class="breadcrumb-link__label">Specifications & Features</span>
                    </li>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/UI/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            ga_pg_id = "15";
            $(document).ready(function () {
                var $window = $(window),
                topNavBarWrapper = $('.specs-features-wrapper'),
                topNavBar = $('#specsFeaturesTabsWrapper'),
                specsFeaturesFooter = $('#specsFeaturesFooter'),
                floatButton = $('.float-button'),
                footer = $('footer'),
                floating = false;

                if (floatButton.length != 0) {
                    floating = true;
                }

                $window.scroll(function () {
                    var windowScrollTop = $window.scrollTop(),
                        topNavBarWrapperOffset = topNavBarWrapper.offset(),
                        topNavBarOffset = topNavBar.offset(),
                        specsFeaturesFooterOffset = specsFeaturesFooter.offset();

                    if (windowScrollTop > topNavBarOffset.top) {
                        topNavBar.addClass('fixed-topNav');
                    }

                    else if (windowScrollTop < topNavBarWrapperOffset.top) {
                        topNavBar.removeClass('fixed-topNav');
                    }

                    if (topNavBar.hasClass('fixed-topNav')) {
                        if (windowScrollTop > specsFeaturesFooterOffset.top - topNavBar.height())
                            topNavBar.removeClass('fixed-topNav');
                    }

                    if (floating) {
                        if (floatButton.offset().top < footer.offset().top - 50)
                            floatButton.addClass('float-fixed');
                        if (floatButton.offset().top > footer.offset().top - 50)
                            floatButton.removeClass('float-fixed');
                    }

                    $('#specsFeaturesDetailsWrapper .bw-model-tabs-data').each(function () {
                        var top = $(this).offset().top - topNavBar.height(),
                            bottom = top + $(this).outerHeight();
                        if (windowScrollTop >= top && windowScrollTop <= bottom) {
                            topNavBar.find('li').removeClass('active');
                            $('#specsFeaturesDetailsWrapper .bw-mode-tabs-data').removeClass('active');

                            $(this).addClass('active');

                            var currentActiveTab = topNavBar.find('li[data-tabs="#' + $(this).attr('id') + '"]');
                            topNavBar.find(currentActiveTab).addClass('active');
                        }
                    });
                });

                $('.model-specs-features-tabs-wrapper li').click(function () {
                    var target = $(this).attr('data-tabs');
                    $('html, body').animate({ scrollTop: $(target).offset().top - topNavBar.height() }, 1000);
                    return false;
                });
            });
        </script>
    </form>
</body>
</html>

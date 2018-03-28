<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.ModelSpecsFeatures" EnableViewState="false" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/controls/GenericBikeInfoControl.ascx" %>
<!DOCTYPE html>

<html>
<head>
    <%
        isHeaderFix = false;

        title = pgTitle;
        description = string.Format("Know more about {0} Specifications and Features. See details about mileage, engine displacement, power, kerb weight and other specifications.", bikeName);
        keywords = string.Format("{0} specifications, {0} specs, {0} features, {0} mileage, {0} fuel efficiency", bikeName);
        alternate = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        canonical = string.Format("https://www.bikewale.com/{0}-bikes/{1}/specifications-features/", makeMaskingName, modelMaskingName);
        ogImage = modelImage;
        isAd970x90Shown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        AdId = "1442913773076";
        AdPath = "/1017752/Bikewale_NewBike_";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%=  staticUrl  %>/css/specsandfeature.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a href="/" itemprop="url"><span itemprop="title">Home</span></a>
                            </li>
                             <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/new-bikes-in-india/" itemprop="url"><span itemprop="title">New Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/" itemprop="url"><span itemprop="title"><%= makeName %> Bikes</span></a>
                            </li>

                              <% if (IsScooter && !IsScooterOnly)
                                  { %>
                              <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-scooters/" itemprop="url"><span itemprop="title"><%= String.Format("{0} Scooters", makeName) %></span></a>
                            </li>
                              <%  }
                                 %>

                            <% if (!string.IsNullOrEmpty(seriesUrl))
                                { %>
                                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                    <span class="bwsprite fa-angle-right margin-right10"></span>
                                    <a href="/<%= seriesUrl %>" itemprop="url"><span itemprop="title"><%= Series.SeriesName %></span></a>
                                </li>
                            <% } %>

                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/" itemprop="url"><span itemprop="title"><%= String.Format("{0} {1}", makeName, modelName) %></span></a>
                            </li>
                            <li>
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <span>Specifications & Features</span>
                            </li>
                          
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div>
    </div>

        <section id="bikeModelHeading" class="container">
            <div class="grid-12">
                <% if (Bikewale.Utility.BWConfiguration.Instance.MetasMakeId.Split(',').Contains(_makeId.ToString())) {%> 
                <h1 class="content-box-shadow content-inner-block-1420 box-shadow">Specifications & Feature of <%= bikeName%></h1>
                
                <%} else { %>
                <h1 class="content-box-shadow content-inner-block-1420 box-shadow"><%= bikeName %> Specifications and Features</h1>
                <%} %>
            </div>
            <div class="clear"></div>
        </section>
        
        <section id="modelCardAndDetailsWrapper" class="container margin-bottom20 font14">
            <div class="grid-12">
                <div id="modelFloatingCardContent">
                    <div class="model-details-floating-card content-box-shadow">
                        <div class="content-inner-block-1020">
                            <div class="grid-5 alpha omega">
                                <div class="model-card-image-content inline-block-top margin-right20">
                                    <img src="<%= modelImage %>" 
                                        title="<%= String.Format("{0} {1}", bikeName, versionName) %> Images" alt="<%= String.Format("{0} {1}", bikeName, versionName) %> Photos"  />
                                </div>
                                <div class="model-card-title-content inline-block-top">
                                    <p class="font16 text-bold margin-bottom5"><%= bikeName %></p>
                                    <p class="font14 text-light-grey"><%= versionName %></p>
                                </div>
                            </div>
                            
                            <% if (isDiscontinued)
                                { %>
                                <div class="grid-7 padding-left30">
                                    <p class="font14 text-light-grey text-truncate">Last known Ex-showroom price</p>
                                    <div>
                                        <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %></span>
                                    </div>
                                    <p class="font14 text-light-grey"><%= bikeName %> is now discontinued in India.</p>
                                </div>
                                <div class="clear"></div>
                            <%  }
                                else
                                { %>
                                <div class="grid-4 padding-left30">
                                    <p class="font14 text-light-grey text-truncate"><%=IsExShowroomPrice ? "Ex-showroom price in Mumbai" : string.Format("On-road price in {0} {1}", areaName, cityName) %></p>
                                    <span class="bwsprite inr-lg"></span>
                                    <span class="font18 text-bold">
                                        <% if (price > 0)
                                            { %>
                                        <%= Bikewale.Utility.Format.FormatPrice(price.ToString()) %>
                                        <% }
                                            else
                                            { %>
                                        Price not available
                                        <% } %>
                                    </span>
                                </div>
                            <%} %>
                             <div class="clear"></div>
              
                        </div>
                        <div class="overall-specs-tabs-wrapper">
                            <a class="active" href="#specs">Specifications</a>
                            <a href="#features">Features</a>
                        </div>
                    </div>
                 </div>
                <div id="modelSpecsAndFeaturesWrapper" class="content-box-shadow">
                    <div class="border-divider"></div>
                    <% if(versionSpecsFeatures != null) { %>
                    <% string itemValue, featureValue; %>
                    <% if (versionSpecsFeatures.Specs != null && versionSpecsFeatures.Specs.Any()) { %>
                    <div id="specs" class="bw-model-tabs-data padding-top20">
                    <h2 class="padding-left20 padding-right20">Specifications</h2>
                     <% foreach (var specCat in versionSpecsFeatures.Specs)
                         { %>
                            <% var firstGridSpecsCount = (specCat.SpecsItemList.Count() + 1) / 2;
                            %>
                            <h3 class="specs-feature__category-name"><%=specCat.DisplayText %></h3>
                            <div class="grid-6">
                                <% foreach (var specItem in specCat.SpecsItemList.Take(firstGridSpecsCount))
                                    {
                                        itemValue = specItem.ItemValues.FirstOrDefault();
                                          %>
                                           <p>
                                                <span class="specs-features-item__content text-light-grey"><%=specItem.DisplayText%></span>
                                                <span class="specs-features-item__content text-bold"><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(itemValue,specItem.UnitTypeText)%></span>
                                            </p>
                                <% } %>
                            </div>
                       
                            <div class="grid-6">
                                <% foreach (var specItem in specCat.SpecsItemList.Skip(firstGridSpecsCount))
                                    {
                                        itemValue = specItem.ItemValues.FirstOrDefault();
                                        %>
                                         <p>
                                            <span class="specs-features-item__content text-light-grey"><%=specItem.DisplayText %></span>
                                            <span class="specs-features-item__content text-bold"><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(itemValue,specItem.UnitTypeText)%></span>
                                         </p>
                                <% } %>
                            </div>
                            <div class="clear"></div>
                         <% } %>
                        <div class="margin-top30 margin-right10 margin-left10 border-divider"></div>
                        <% } %>
                    </div>

                    <div id="features" class="bw-model-tabs-data padding-top20 padding-bottom40">
                        <% if (versionSpecsFeatures.Features != null && versionSpecsFeatures.Features.Any()) { %>
                        <% var firstGridFeaturesCount = (versionSpecsFeatures.Features.Count() + 1) / 2;
                          %>
                        <h2 class="padding-left20 padding-right20">Features</h2>
                        <% foreach (var feature in versionSpecsFeatures.Features.Take(firstGridFeaturesCount))
                            {
                                featureValue = feature.ItemValues.FirstOrDefault();
                                %>
                            <div class="grid-6">
                                        <p>
                                            <span class="specs-features-item__content text-light-grey"><%=feature.DisplayText %></span>
                                            <span class="specs-features-item__content text-bold"><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(featureValue,feature.UnitTypeText) %></span>
                                         </p>
                            </div>
                        <% } %>
                        <% foreach (var feature in versionSpecsFeatures.Features.Skip(firstGridFeaturesCount))
                            {
                                featureValue = feature.ItemValues.FirstOrDefault();
                                %>
                            <div class="grid-6">
                                        <p>
                                            <span class="specs-features-item__content text-light-grey"><%=feature.DisplayText %></span>
                                            <span class="specs-features-item__content text-bold"><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(featureValue,feature.UnitTypeText) %></span>
                                         </p>
                            </div>
                        <% } %>
                        <div class="clear"></div>
                        <% } %>
                    </div>
                    <div id="modelSpecsFeaturesFooter"></div>
                    <% } %>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <BW:GenericBikeInfo runat="server" ID="ctrlGenericBikeInfo" />
        <section class="container">
            <div class="grid-12">
                <% if (similarBikes != null && similarBikes.Bikes != null && similarBikes.Bikes.Any())
                    { %>
                <div id="modelSimilarContent" class="bw-model-tabs-data content-box-shadow padding-bottom20 card-bottom-margin font14">
                    <h2 class="h2-heading padding-top20 padding-right20 padding-left20 margin-bottom15 font18"><%= bodyStyleText %> similar to <%= modelPg.ModelDetails.ModelName %></h2>
                    <div class="jcarousel-wrapper inner-content-carousel">
                        <div class="jcarousel">
                            <ul>
                                <% foreach (var bike in similarBikes.Bikes)
                                    {  %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName) %>" title="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" class="jcarousel-card">
                                        <div class="model-jcarousel-image-preview">
                                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._310x174,Bikewale.Utility.QualityFactor._75) %>" alt="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" title="<%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %>" src="" border="0" />
                                        </div>
                                        <div class="card-desc-block">
                                            <p class="bikeTitle"><%= string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName) %></p>
                                            <p class="text-xt-light-grey margin-bottom10">
                                                <%= Bikewale.Utility.FormatMinSpecs.GetMinSpecsSpanElements(bike.MinSpecsList) %>
                                            </p>

                                            <% if (bike.VersionPrice == 0 && bike.AvgExShowroomPrice == 0)
                                                { %>
                                            <p class="text-bold text-default">
                                                <span class="font14">Price not available</span>
                                            </p>
                                            <% }
                                                else
                                                {
                                                    if (bike.VersionPrice > 0)
                                                    { %>
                                                        <p class="text-light-grey margin-bottom5"><%= string.Format("Ex-showroom, {0}", bike.CityName) %></p>
                                                     <% }
                                                    else
                                                    { %>
                                                    <p>
                                                    <span class="text-light-grey margin-bottom5 margin-right5">Avg. Ex-showroom price</span><span class="bwsprite info-icon tooltip-icon-target tooltip-top">
                                                    <span class="bw-tooltip info-tooltip">
                                                        <span class="bw-tooltip-text"><%= string.Format("Price is not available in {0}", bike.CityName) %></span>
                                                    </span>
                                                </span>
                                            </p>
                                            <% } %>

                                            <span class='font18 text-default'>&#x20B9;</span>
                                            <span class="font18 text-default text-bold">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(bike.VersionPrice > 0 ? bike.VersionPrice.ToString() : bike.AvgExShowroomPrice.ToString())%></span>
                                            <% } %>
                                        </div>
                                    </a>
                                    <% if (similarBikes.ShowCheckOnRoadCTA)
                                        { %>
                                    <div class="margin-left20 margin-bottom20">
                                        <a href="javascript:void(0);" data-pqsourceid="<%= ((int)similarBikes.PQSourceId) %>" data-makename="<%= makeName %>" data-modelname="<%= modelName %>" data-modelid="<%= bike.ModelBase.ModelId %>" class="btn btn-grey btn-sm font14  <%= (bike.AvgExShowroomPrice!=0 ?"":"hide") %> getquotation" rel="nofollow">Check on-road price</a>
                                    </div>
                                    <% } %>
                                    <% if (similarBikes.ShowPriceInCityCTA)
                                        { %>
                                    <div class="margin-left20 margin-bottom20">
                                        <a href="<%= Bikewale.Utility.UrlFormatter.PriceInCityUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName,bike.CityMaskingName) %>" class="btn btn-white btn-truncate font14 btn-size-2" title="<%= String.Format("{0} {1} On-road price in {2}",makeName,modelName,bike.CityName) %>"><%= String.Format("On-road price in {0}", bike.CityName) %></a>
                                    </div>
                                    <% } %>
                                    <% if (similarBikes.Make != null && similarBikes.Model != null && similarBikes.IsNew)
                                        {
                                           string fullUrl = string.Format("/{0}",Bikewale.Utility.UrlFormatter.CreateCompareUrl(similarBikes.Make.MaskingName, similarBikes.Model.MaskingName, bike.MakeBase.MaskingName, bike.ModelBase.MaskingName, Convert.ToString(similarBikes.VersionId),  Convert.ToString(bike.VersionBase.VersionId), (uint)similarBikes.Model.ModelId, (uint)bike.ModelBase.ModelId, Bikewale.Entities.Compare.CompareSources.Desktop_Model_MostPopular_Compare_Widget));
                                            %>
                                    <a title="<%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.ModelBase.ModelName, similarBikes.Model.ModelName) %>" href="<%=Bikewale.Utility.UrlFormatter.RemoveQueryString(fullUrl) %>" data-url="<%=fullUrl  %>" class="compare-with-target text-truncate redirect-url">
                                        <span class="bwsprite compare-sm"></span>Compare with <%= similarBikes.Model.ModelName %><span class="bwsprite next-grey-icon"></span>
                                    </a>
                                    <% } %>
                                </li>
                                <% } %>
                                <li>
                                    <a href="<%= ((bodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter))? "/scooters/" : "/new-bikes-in-india/") %>" title="<%= (string.Format("Explore more {0}", (similarBikes.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter))? "scooters" : "bikes")) %>" class="jcarousel-card bw-ga" c="<%=similarBikes.Page %>" a="Clicked_ExploreMore_Card" l="<%= similarBikes.Model.ModelName %>">
                                        <div class="model-jcarousel-image-preview">
                                            <div class="exploremore__imagebackground">
                                                <div class="exploremore__icon-background">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-desc-block">
                                            <div class="exploremore-detailblock">
                                                <p class="detailblock__title">Couldn’t find what you were looking for?</p>
                                                <p class="detailblock__description"><%= (bodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter) ? "View 60+ scooters from over 10 brands" : " View 200+ bikes from over 30 brands") %></p>
                                            </div>
                                        </div>
                                        <% if (similarBikes.IsNew)
                                            { %>
                                        <div class="compare-with-target text-truncate">
                                            <%= (string.Format("Explore more {0}", (bodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)) ? "scooters" : "bikes")) %><span class="bwsprite next-grey-icon"></span>
                                        </div>
                                        <% } %>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                    </div>
                </div>
                <% }
                    else if ((popularBodyStyle != null && popularBodyStyle.PopularBikes != null && popularBodyStyle.PopularBikes.Any()) && (similarBikes.IsNew || similarBikes.IsUpcoming))
                    { %>
                <div id="modelPopularContent" class="bw-model-tabs-data content-box-shadow card-bottom-margin padding-bottom15">
                    <div class="carousel-heading-content padding-top20">
                        <div class="swiper-heading-left-grid inline-block">
                            <h2 class="h2-heading">Other popular <%= popularBodyStyle.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)? "scooters" : "bikes" %></h2>
                        </div>
                        <div class="swiper-heading-right-grid inline-block text-right">
                            <a href="<%= Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(popularBodyStyle.BodyStyle) %>" title="Best <%= popularBodyStyle.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter)? "scooters" : "bikes" %> in India" class="btn view-all-target-btn">View all</a>
                        </div>
                    </div>
                    <% if (popularBodyStyle != null && popularBodyStyle.PopularBikes != null && popularBodyStyle.PopularBikes.Count() > 0)
                        { %>
                    <div class="jcarousel-wrapper inner-content-carousel padding-bottom20">
                        <div class="jcarousel">
                            <ul>
                                <% foreach (var bike in popularBodyStyle.PopularBikes)
                                    { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName) %>" title="<%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %>" class="jcarousel-card">
                                        <div class="model-jcarousel-image-preview">
                                            <span class="card-image-block">
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath, bike.HostURL, Bikewale.Utility.ImageSize._310x174, Bikewale.Utility.QualityFactor._75) %>" alt="<%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %>" src="" border="0">
                                            </span>
                                        </div>
                                        <div class="card-desc-block">
                                            <h3 class="bikeTitle"><%= string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName) %></h3>
                                            <% if (bike.VersionPrice == 0 && bike.AvgPrice == 0)
                                                { %>
                                            <span class="font16 text-default text-light-grey">Price not available</span>
                                            <% }
                                                else
                                                {
                                                    if (bike.VersionPrice > 0)
                                                    { %>
                                            <p class="font14 text-light-grey margin-bottom5"><%= string.Format("Ex-showroom, {0}", (!string.IsNullOrEmpty(bike.CityName) ? bike.CityName : Bikewale.Utility.BWConfiguration.Instance.GetDefaultCityName)) %></p>
                                            <span class='font16 text-default'>&#x20B9;</span>
                                            <span class="font16 text-bold text-default">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                                            <% }
                                                else
                                                { %>
                                            <p class="font14 text-light-grey margin-bottom5">
                                                <span class="margin-right5">Avg. Ex-showroom price</span>
                                                <span class="bwsprite info-icon tooltip-icon-target tooltip-top">
                                                    <span class="bw-tooltip info-tooltip">
                                                        <span class="bw-tooltip-text"><%= string.Format("Price is not available in {0}", bike.CityName) %></span>
                                                    </span>
                                                </span>
                                            </p>

                                            <span class='font16 text-default'>&#x20B9;</span>
                                            <span class="font16 text-bold text-default">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.AvgPrice))%></span>
                                            <% }
                                                }%>
                                        </div>
                                    </a>
                                    <% if (popularBodyStyle.ShowCheckOnRoadCTA && (bike.AvgPrice > 0 || bike.VersionPrice > 0))
                                        {%>
                                    <div class="margin-left20 margin-bottom20">
                                        <a href="javascript:void(0);" data-pqsourceid="<%= ((int)popularBodyStyle.PQSourceId) %>" data-makename="<%= bike.MakeName %>" data-modelname="<%= bike.objModel.ModelName %>" data-modelid="<%= bike.objModel.ModelId %>" class="btn btn-grey btn-sm font14 getquotation" rel="nofollow">Check on-road price</a>
                                    </div>
                                    <% } %>
                                </li>
                                <% } %>
                            </ul>

                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                    </div>
                    <% } %>
            </div>
            <% } %>
            </div>
            <div class="clear"></div>
        </section>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript">
            var pageUrl = window.location.href;
            var clientIP = '<%= clientIP %>';
            var bikename = '<%= bikeName %>';
            var bikeVersionName = bikename + '_' + '<%= versionName %>'; 
            var cityArea = '<%=cityName%>' + '_' + '<%=areaName%>';
            var BkCityArea=bikename+'_'+cityArea;
            ga_pg_id=15;
            $(document).ready(function () {

                var hashValue = window.location.hash.substr(1);
                if (hashValue.length > 0) {
                    $("body, html").animate({
                        scrollTop: $("#" + hashValue).offset().top - $('.model-details-floating-card').height() + 110
                    }, 500);
                }

                var $window = $(window),
                    modelCardAndDetailsWrapper = $('#modelCardAndDetailsWrapper'),
                    modelDetailsFloatingCard = $('.model-details-floating-card'),
                    modelSpecsFeaturesFooter = $('#modelSpecsFeaturesFooter');

                $('#modelFloatingCardContent').css({ 'height': modelDetailsFloatingCard.height() });

                $(window).scroll(function () {
                    var windowScrollTop = $window.scrollTop(),
                        modelCardAndDetailsOffsetTop = modelCardAndDetailsWrapper.offset().top,
                        modelSpecsFeaturesFooterOffsetTop = modelSpecsFeaturesFooter.offset().top;

                    if (windowScrollTop > modelCardAndDetailsOffsetTop)
                        modelDetailsFloatingCard.addClass('fixed-card');

                    else if (windowScrollTop < modelCardAndDetailsOffsetTop)
                        modelDetailsFloatingCard.removeClass('fixed-card');

                    if (modelDetailsFloatingCard.hasClass('fixed-card')) {
                        if (windowScrollTop > modelSpecsFeaturesFooterOffsetTop - modelDetailsFloatingCard.height())
                            modelDetailsFloatingCard.removeClass('fixed-card');
                    }

                    $('#modelSpecsAndFeaturesWrapper .bw-model-tabs-data').each(function () {
                        var top = $(this).offset().top - modelDetailsFloatingCard.height(),
                            bottom = top + $(this).outerHeight();
                        if (windowScrollTop >= top && windowScrollTop <= bottom) {
                            modelDetailsFloatingCard.find('a').removeClass('active');
                            $('#modelSpecsAndFeaturesWrapper .bw-mode-tabs-data').removeClass('active');

                            $(this).addClass('active');
                            modelDetailsFloatingCard.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
                        }
                    });

                });

                $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
                    var target = $(this.hash);
                    if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
                    if (target.length == 0) target = $('html');
                    $('html, body').animate({ scrollTop: target.offset().top - modelDetailsFloatingCard.height() }, 1000);
                    return false;
                });
           });
        </script>

    </form>
</body>
</html>

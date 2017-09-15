<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.New.Photos.Default" %>

<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/m/controls/GenericBikeInfoControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="SimilarBikeWithPhotos" Src="~/m/controls/SimilarBikeWithPhotos.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%  if (vmModelPhotos != null && vmModelPhotos.pageMetas != null)
        {
            title = vmModelPhotos.pageMetas.Title;
            keywords = vmModelPhotos.pageMetas.Keywords;
            description = vmModelPhotos.pageMetas.Description;
            canonical = vmModelPhotos.pageMetas.CanonicalUrl;
            EnableOG = true;
            OGImage = vmModelPhotos.modelImage;
        }
       
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/photos.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
     <% if (vmModelPhotos != null && !String.IsNullOrEmpty(vmModelPhotos.SchemaJSON))
        { %>
    <script type="application/ld+json">
        <%= vmModelPhotos.SchemaJSON %>
    </script>
    <% }%>
</head>
<body>
    <form id="form1" runat="server">
        <% if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Length > 0)
            { %>
        <div class="gallery-loader-placeholder gallery-bg-overlay text-center"><span class="spin-loader fixed-loader"></span></div>
        <%}
            else
            {%>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% if (vmModelPhotos != null)
            {
                var objImages = vmModelPhotos.objImageList; %>
        <section>
            <div class="container box-shadow section-bottom-margin padding-bottom2">
                <h1 class="section-header bg-white"><%= vmModelPhotos.bikeName %> Images</h1>
                <% int i = 0; if (vmModelPhotos.totalPhotosCount > 0)
                    { %>
                <ul class="photos-grid-list">
                    <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < 6)
                        { %>
                    <li>
                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%= string.Format("{0} {1}",bikeName,objImages[i].ImageCategory) %> Image" title="<%= string.Format("{0} {1}",bikeName,objImages[i++].ImageCategory)%>" />
                    </li>
                    <% } %>
                    <% while (i < vmModelPhotos.gridPhotosCount && i < vmModelPhotos.GridSize)
                        { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= string.Format("{0} {1}",bikeName,objImages[i].ImageCategory) %> Image" title="<%=string.Format("{0} {1}",bikeName, objImages[i++].ImageCategory) %>" />
                    </li>
                    <% }  %>
                </ul>
                <% if (vmModelPhotos.totalPhotosCount < vmModelPhotos.GridSize && vmModelPhotos.nongridPhotosCount > 0)
                    { %>
                <ul class="photos-grid-list photos-remainder-<%= vmModelPhotos.nongridPhotosCount %> remainder-grid-list">
                    <% while (i < vmModelPhotos.totalPhotosCount && i < vmModelPhotos.GridSize)
                        { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= string.Format("{0} {1}",bikeName,objImages[i].ImageCategory) %> Image" title="<%= string.Format("{0} {1}",bikeName,objImages[i++].ImageCategory) %>" />
                    </li>
                    <% } %>
                </ul>
                <% }
                    } %>
                <div class="clear"></div>
            </div>
        </section>
        <%}
            } %>

        <%if (ctrlVideos.FetchedRecordsCount > 0)
            { %>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <h2 class="margin-bottom15"><%= vmModelPhotos.bikeName %> Videos</h2>
                <BW:Videos runat="server" ID="ctrlVideos" />
            </div>
        </section>
        <% } %>

        <%if (bikeInfo != null)
            { %><section>
              <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                  <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />
              </div>
          </section>
        <%} %>
        <BW:SimilarBikeWithPhotos ID="ctrlSimilarBikesWithPhotos" runat="server" />

        <div id="gallery-root">
            <div class="gallery-loader-placeholder gallery-bg-overlay text-center hide"><span class="spin-loader fixed-loader"></span></div>

            <div class="gallery-container gallery-bg-overlay" style="display: none;" data-bind="visible: isGalleryActive()">
                <div class="gallery-header" data-bind="visible: galleryTabsActive()">
                    <h2 class="text-white gallery-title"><%=bikeName %> Images</h2>
                    <span id="gallery-close-btn" class="position-abt bwmsprite cross-md-white cur-pointer"></span>
                    <ul class="horizontal-tabs-wrapper">
                        <%if (vmModelPhotos != null && vmModelPhotos.totalPhotosCount > 0)
                            {%>
                        <li data-bind="click: togglePhotoTab, css: photosTabActive() ? 'active' : ''">Images</li>
                        <%} %>
                        <%if (VideoCount > 0)
                            { %>
                        <li data-bind="click: togglePhotoTab, css: !photosTabActive() ? 'active' : ''">Videos</li>
                        <%} %>
                    </ul>
                </div>

                <div class="gallery-body">
                    <div id="main-photo-swiper" class="swiper-container gallery-swiper noSwiper" data-bind="visible: photosTabActive() && photoSwiperActive()">
                        <div class="swiper-heading-details" data-bind="visible: photoHeadingActive()">
                            <p class="grid-9 text-truncate font14 text-white text-left" data-bind="text: activePhotoTitle()"></p>
                            <div class="grid-3 alpha font12 text-xx-light text-right position-rel pos-top2">
                                <span data-bind="text: activePhotoIndex()"></span>&nbsp;/&nbsp;<span data-bind="    text: photoList().length"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="swiper-wrapper" data-bind="foreach: photoList">
                            <div class="swiper-slide">
                                <img class="swiper-lazy gallery-swiper-image" data-bind="attr: { 'data-index': $index, alt: modelName + ' ' + ImageTitle, title: modelName + ' ' + ImageTitle, 'data-src': HostUrl + '/642x361/' + OriginalImgPath }" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" alt="" title="" border="0" />
                            </div>
                        </div>
                        <div class="bwmsprite swiper-button-next gallery-type-next"></div>
                        <div class="bwmsprite swiper-button-prev gallery-type-prev"></div>
                    </div>

                    <div id="main-color-swiper" style="display: none" class="swiper-container gallery-swiper noSwiper" data-bind="visible: photosTabActive() && !photoSwiperActive()">
                        <div class="swiper-heading-details" data-bind="visible: photoHeadingActive()">
                            <p class="grid-9 text-truncate font14 text-white text-left" data-bind="text: activeColorTitle()"></p>
                            <div class="grid-3 alpha font12 text-xx-light text-right position-rel pos-top2">
                                <span data-bind="text: activeColorIndex()"></span>&nbsp<span>of</span>
                                <span data-bind="text: colorPhotoList().length"></span>&nbsp<span data-bind="    text: colorPhotoList().length > 1 ? 'colours' : 'colour'"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="swiper-wrapper" data-bind="foreach: colorPhotoList">
                            <div class="swiper-slide">
                                <img class="swiper-lazy gallery-swiper-image" data-bind="attr: { alt: modelName + ' ' + ImageTitle, title: modelName + ' ' + ImageTitle, 'data-src': HostUrl + '/642x361/' + OriginalImgPath }" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" alt="" title="" border="0" />
                            </div>
                        </div>
                        <div class="bwmsprite swiper-button-next color-type-next"></div>
                        <div class="bwmsprite swiper-button-prev color-type-prev"></div>
                    </div>

                    <div id="main-video-content" data-bind="visible: !photosTabActive()">
                        <div class="swiper-heading-details" data-bind="visible: photoHeadingActive()">
                            <p class="grid-12 text-truncate font14 text-white" data-bind="text: activeVideoTitle()"></p>
                            <div class="clear"></div>
                        </div>
                        <div class="main-video-wrapper">
                            <div class="main-video-iframe-content">
                                <iframe id="iframe-video" width="360" height="203" src="" frameborder="0" allowfullscreen></iframe>
                            </div>
                        </div>
                    </div>

                </div>

                <div id="gallery-footer" class="gallery-footer" data-bind="visible: galleryFooterActive()">
                    <div class="footer-tabs-wrapper">
                        <div data-bind="click: togglePhotoThumbnailScreen, visible: photosTabActive(), css: photoThumbnailScreen() ? 'tab-active' : ''" class="footer-tab all-option-tab position-rel tab-separator">
                            <span class="bwmsprite grid-icon margin-right10"></span>
                            <span class="inline-block font14">All photos</span>
                        </div>

                        <%if (VideoCount > 1)
                            { %>
                        <div style="display: none" data-bind="click: toggleVideoListScreen, visible: !photosTabActive(), css: videoListScreen() ? 'tab-active' : ''" class="footer-tab all-option-tab position-rel tab-separator">
                            <span class="bwmsprite grid-icon margin-right10"></span>
                            <span class="inline-block font14">All videos</span>
                        </div>
                        <%} %>

                        <div data-bind="click: toggleFullScreen, visible: photosTabActive() && fullscreenSupport(), css: fullScreenModeActive() ? 'fullscreen-active' : ''" class="footer-tab grid-3-tab">
                            <span class="bwmsprite fullscreen-icon"></span>
                        </div>

                        <div data-bind="click: toggleModelInfoScreen, css: modelInfoScreen() ? 'tab-active' : ''" class="footer-tab grid-3-tab">
                            <span class="bwmsprite info-icon"></span>
                        </div>

                        <div data-bind="click: toggleColorThumbnailScreen, visible: photosTabActive() && colorTabActive(), css: colorsThumbnailScreen() ? 'tab-active' : ''" class="footer-tab grid-3-tab">
                            <span class="bwmsprite color-palette"></span>
                        </div>

                        <div class="clear"></div>
                    </div>

                    <div id="thumbnail-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" data-bind="css: photoThumbnailScreen() ? 'position-fixed' : ''">
                        <div id="thumbnail-photo-swiper" class="swiper-container thumbnail-swiper noSwiper">
                            <div class="swiper-wrapper" data-bind="foreach: photoList">
                                <div class="swiper-slide">
                                    <div class="thumbnail-image-placeholder">
                                        <img class="swiper-lazy" data-bind="attr: { alt: modelName + ' ' + ImageTitle, title: modelName + ' ' + ImageTitle, 'data-src': HostUrl + '/110x61/' + OriginalImgPath }" src="" alt="" title="" border="0" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="color-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" data-bind="css: colorsThumbnailScreen() ? 'position-fixed' : ''">
                        <div id="thumbnail-colors-swiper" class="swiper-container color-thumbnail-swiper noSwiper">
                            <div class="swiper-wrapper" data-bind="foreach: colorPhotoList">
                                <div class="swiper-slide">
                                    <div class="color-box inline-block" data-bind="foreach: Colors, css: (Colors.length == 3) ? 'color-count-three' : (Colors.length == 2) ? 'color-count-two' : 'color-count-one'">
                                        <span data-bind="style: { 'background-color': '#' + $data }"></span>
                                    </div>
                                    <p class="color-box-label inline-block" data-bind="text: (ImageTitle.length > 12 ? ImageTitle.substring(0, 12) + '...' : ImageTitle)"></p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%if (bikeInfo != null)
                        { %>
                    <div id="info-tab-screen" class="footer-tab-card" data-bind="css: modelInfoScreen() ? 'position-fixed' : ''">
                        <div class="model-more-info-section padding-15-20">
                            <%if (IsUpcoming)
                                { %><p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
                            <%} %>
                            <%if (IsDiscontinued)
                                { %>
                            <p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
                            <%} %>
                            <div class="margin-bottom10">
                                <a href="<%=bikeUrl %>" class="item-image-content vertical-top" title="<%=bikeName %>">
                                    <img src="<%=Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._110x61)%>" alt="<%=bikeName %>">
                                </a>
                                <%int count = 0; %>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="<%=bikeUrl %>" class="block text-bold text-default text-truncate" title="<%=bikeName %>"><%=bikeName%></a></h3>
                                    <ul class="item-more-details-list">
                                        <%if (bikeInfo.IsSpecsAvailable && count < 3)
                                            {
                                                count++;%>
                                        <li>
                                            <a href="/m<%=  Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%=bikeName %> Reviews">
                                                <span class="bwmsprite specs-sm"></span>
                                                <span class="icon-label">Specs</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if (bikeInfo.UserReview > 0 && count < 3)
                                            {
                                                count++;
                                        %>
                                        <li>
                                            <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatUserReviewUrl(bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName) %>" title="<%=bikeName %> News">
                                                <span class="bwmsprite user-reviews-sm"></span>
                                                <span class="icon-label">Reviews</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if (bikeInfo.VideosCount > 0 && count < 3)
                                            {
                                                count++; %>
                                        <li>
                                            <a href="/m<%=  Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName) %>" title="<%=bikeName %> Specification">
                                                <span class="bwmsprite videos-sm"></span>
                                                <span class="icon-label">Videos</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if (bikeInfo.DealersCount > 0 && count < 3)
                                            {
                                                count++; %>
                                        <li>
                                            <a href="/m<%=Bikewale.Utility.UrlFormatter.DealerLocatorUrl(bikeInfo.Make.MaskingName, CityDetails != null ? CityDetails.CityMaskingName : "india") %>" title="<%= bikeName %> Specification">
                                                <span class="bwmsprite dealers-sm"></span>
                                                <span class="icon-label">Dealers</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if (bikeInfo.ExpertReviewsCount > 0 && count < 3)
                                            {
                                                count++; %>
                                        <li>
                                            <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Expert reviews">
                                                <span class="bwmsprite reviews-sm"></span>
                                                <span class="icon-label">Expert reviews</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if (bikeInfo.NewsCount > 0 && count < 3)
                                            {
                                                count++;  %>
                                        <li>
                                            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatNewsUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> News">
                                                <span class="bwmsprite news-sm"></span>
                                                <span class="icon-label">News</span>
                                            </a>
                                        </li>
                                        <%} %>
                                    </ul>
                                </div>
                            </div>
                            <%if (!IsUpcoming && !IsDiscontinued)
                                { %>

                            <div class="grid-7 alpha omega">
                                <p class="font11 text-light-grey text-truncate">Ex-showroom, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                                <div>
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                                </div>
                            </div>
                            <%}
                                else if (IsUpcoming)
                                { %>
                            <div class="grid-7 alpha omega">
                                <p class="font11 text-light-grey text-truncate">Expected price</p>
                                <div>
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMin)) %> onwards</span>
                                </div>
                            </div>
                            <%}
                                else if (IsDiscontinued)
                                { %>
                            <div class="grid-7 alpha omega">
                                <p class="font11 text-light-grey text-truncate">Last know price</p>
                                <div>
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                                </div>
                            </div>
                            <%} %>
                            <div class="grid-5 omega">
                                <a href="<%=bikeUrl %>" title="<%=bikeName%>" class="btn btn-white btn-size-120">View details<span class="bwmsprite btn-red-arrow"></span></a>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <%} %>

                    <div id="video-tab-screen" class="footer-tab-card font14" style="display: none" data-bind="visible: videoListScreen()">
                        <ul class="video-tab-list" data-bind="foreach: videoList">
                            <li>
                                <div class="video-image-block inline-block">
                                    <img data-bind="attr: { alt: VideoTitle, src: 'https://img.youtube.com/vi/' + VideoId + '/default.jpg' }" border="0" />
                                    <span class="black-overlay">
                                        <span class="bwmsprite video-play-icon"></span>
                                    </span>
                                </div>
                                <p class="video-title-block padding-left15 inline-block" data-bind="text: VideoTitle"></p>
                            </li>
                        </ul>
                    </div>

                </div>

            </div>
        </div>


        <% if (vmModelPhotos.Breadcrumb != null && vmModelPhotos.Breadcrumb.BreadcrumListItem != null && vmModelPhotos.Breadcrumb.BreadcrumListItem.Any())
            {%>
        <section>
            <div class="breadcrumb">
                <span class="breadcrumb-title">You are here:</span>
                <ul>
                    <%foreach (var item in vmModelPhotos.Breadcrumb.BreadcrumListItem)
                        {%>
                    <%if (!string.IsNullOrEmpty(item.Item.Url))
                        {%>
                    <li>
                        <a class="breadcrumb-link" href="<%= item.Item.Url %>" title="<%= item.Item.Name %>">
                            <span class="breadcrumb-link__label" itemprop="name"><%= item.Item.Name %></span>
                        </a>
                    </li>
                    <%}
                        else
                        {%>
                    <li>
                        <span class="breadcrumb-link__label"><%= item.Item.Name %></span>
                    </li>
                    <%}%>
                    <%}%>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>
        <% } %>

        <script type="text/javascript" src="<%= staticUrl %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            try {
                var photoCount = "<%= vmModelPhotos!=null ?  vmModelPhotos.totalPhotosCount : 0 %>";
                var modelId = "<%= modelId%>";
                var ModelId = "<%=vmModelPhotos.objModel.ModelId%>";
                var videoCount = "<%=VideoCount%>";
                var colorImageId = "<%= colorImageId%>";
                var modelName = "<%= vmModelPhotos.bikeName %>";
                var encodedVideoList = "<%= JSONVideoList%>";
                var encodedImageList = "<%= JSONImageList %>";
                var imageIndex = "<%= imageIndex %>";
                var returnUrl = "<%= returnUrl%>";
            } catch (e) {
                console.warn(e);
            }
        </script>

        <script type="text/javascript" src="<%= staticUrl %>/m/src/photos.js?<%= staticFileVersion%>"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                if (returnUrl.length > 0) {
                    popupGallery.bindGallery(imageIndex);
                }
            });
        </script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>


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
</head>
<body>
    <form id="form1" runat="server">
         <% if (isModelPage)
            {
            var objImages = vmModelPhotos.objImageList;%>
            <div class="blackOut-window text-center" style="background: #2a2a2a; display: block; opacity:1;"><span class="spin-loader fixed-loader"></span></div>
        <section>
            <div class="container box-shadow section-bottom-margin">
                <h1 class="section-header bg-white"><%= vmModelPhotos.bikeName %> Images</h1>
         
                <ul class="photos-grid-list">                
                    <li>
                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[0].OriginalImgPath,objImages[0].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%= objImages[0].ImageCategory %> Image" title="<%= objImages[0].ImageCategory %>" />
                    </li>                
                </ul>
             
                <div class="clear"></div>
            </div>
        </section>
          <%}else {%>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% if (vmModelPhotos != null)
        {
            var objImages = vmModelPhotos.objImageList; %>
        <section>
            <div class="container box-shadow section-bottom-margin">
                <h1 class="section-header bg-white"><%= vmModelPhotos.bikeName %> Images</h1>
                <% int i = 0; if (vmModelPhotos.totalPhotosCount > 0)
                   { %>
                <ul class="photos-grid-list">
                    <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < 6) //to handle lazy load for initial images (6 images can vary) 
                       { %>
                    <li>
                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                    </li>
                    <% } %>
                    <% while (i < vmModelPhotos.gridPhotosCount && i < vmModelPhotos.GridSize)
                       { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                    </li>
                    <% }  %>
                </ul>
                <% if (vmModelPhotos.totalPhotosCount < vmModelPhotos.GridSize && vmModelPhotos.nongridPhotosCount > 0)
                   { %>
                <ul class="photos-grid-list photos-remainder-<%= vmModelPhotos.nongridPhotosCount %> remainder-grid-list">
                    <% while (i < vmModelPhotos.totalPhotosCount && i < vmModelPhotos.GridSize)
                       { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                    </li>
                    <% } %>
                </ul>
                <% }
                   } %>
                <div class="clear"></div>
            </div>
        </section>
        <%} } %>
        
    

        <%if (ctrlVideos.FetchedRecordsCount > 0)
          { %>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <h2 class="margin-bottom15"><%= vmModelPhotos.bikeName %> Videos</h2>
                <BW:Videos runat="server" ID="ctrlVideos" />
            </div>
        </section>
        <% } %>
           
        <%if(bikeInfo!=null){ %><section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />
            </div>
        </section>
       <%} %>
        <BW:SimilarBikeWithPhotos ID="ctrlSimilarBikesWithPhotos" runat="server" />
        
        <div id="gallery-root">
            <!-- ko component: "gallery-component" -->
            <!-- /ko -->
            <script type="text/html" id="gallery-template-wrapper">
                <!-- ko if: vmPhotosPage.photoGalleryContainerActive() -->
                <div class="gallery-container text-center"><span class="spin-loader fixed-loader"></span></div>
                <!-- /ko -->
                <!-- ko if: vmPhotosPage.activateGallery() -->
                    <div id="gallery-container" class="gallery-container" data-bind="template: { name: 'gallery-template', afterRender: afterRender }"></div>
                <!-- /ko -->
            </script>

            <script type="text/html" id="gallery-template">
                <!-- gallery header -->
                <div class="gallery-header" data-bind="visible: galleryTabsActive()">
                    <h2 class="text-white gallery-title"><%=bikeName %> Images</h2>
                    <span id="gallery-close-btn" class="position-abt pos-top10 pos-right10 bwmsprite cross-md-white cur-pointer"></span>
                    <ul class="horizontal-tabs-wrapper">
                       <%if(vmModelPhotos!=null && vmModelPhotos.totalPhotosCount>0) {%> <li data-bind="click: togglePhotoTab, css: photosTabActive() ? 'active': ''">Images</li><%} %>
                        <%if (VideoCount > 0)
                          { %> <li data-bind="click: togglePhotoTab, css: !photosTabActive() ? 'active': ''">Videos</li><%} %>
                    </ul>
                </div>

                <!-- gallery body -->
                <div class="gallery-body">
                    <div id="main-photo-swiper" class="swiper-container gallery-swiper" data-bind="visible: photosTabActive() && photoSwiperActive()">
                        <div class="swiper-heading-details" data-bind="visible: photoHeadingActive()">
                            <p class="grid-9 text-truncate font14 text-white text-left" data-bind="text: activePhotoTitle()"></p>
                            <div class="grid-3 alpha font12 text-xx-light text-right position-rel pos-top2">
                                <span data-bind="text: activePhotoIndex()"></span> / <span data-bind="text: photoList().length"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="swiper-wrapper" data-bind="foreach: photoList">
                            <div class="swiper-slide">
                                <img class="swiper-lazy gallery-swiper-image" data-bind="attr: { 'data-index': $index, alt: ImageTitle, title: ImageTitle, 'data-src': HostUrl + '/642x361/' + OriginalImgPath }" src="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" alt="" title="" border="0" />
                            </div>
                        </div>
                        <div class="bwmsprite swiper-button-next"></div>
                        <div class="bwmsprite swiper-button-prev"></div>
                    </div>

                    <div id="main-color-swiper" class="swiper-container gallery-swiper" data-bind="visible: photosTabActive() && !photoSwiperActive()">
                        <div class="swiper-heading-details" data-bind="visible: photoHeadingActive()">
                            <p class="grid-9 text-truncate font14 text-white text-left" data-bind="text: activeColorTitle()"></p>
                            <div class="grid-3 alpha font12 text-xx-light text-right position-rel pos-top2">
                                <span data-bind="text: activeColorIndex()"></span> of <span data-bind="text: colorPhotoList().length"></span> <span data-bind="text: colorPhotoList().length > 1 ? 'colors' : 'color'"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="swiper-wrapper" data-bind="foreach: colorPhotoList">
                            <div class="swiper-slide top10">
                                <img class="swiper-lazy gallery-swiper-image" data-bind="attr: { alt: ImageTitle, title: ImageTitle, 'data-src': HostUrl + '/642x361/' + OriginalImgPath }" src="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" alt="" title="" border="0" />
                            </div>
                        </div>
                        <div class="bwmsprite swiper-button-next"></div>
                        <div class="bwmsprite swiper-button-prev"></div>
                    </div>

                    <div id="main-video-content" data-bind="visible: !photosTabActive()">
                        <div class="swiper-heading-details" data-bind="visible: photoHeadingActive()">
                            <p class="grid-12 text-truncate font14 text-white text-left" data-bind="text: activeVideoTitle()"></p>
                            <div class="clear"></div>
                        </div>
                        <div class="main-video-wrapper">
                            <div class="main-video-iframe-content">
                                <iframe width="320" height="180" data-bind="attr: { src: 'https://www.youtube.com/embed/' + activeVideoId() + '?showinfo=0' }" src="" frameborder="0" allowfullscreen></iframe>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- gallery footer -->
                <div class="gallery-footer" data-bind="visible: galleryFooterActive()">
                    <div class="footer-tabs-wrapper">
                        <div data-bind="click: togglePhotoThumbnailScreen, visible: photosTabActive(), css: photoThumbnailScreen() ? 'tab-active': ''" class="footer-tab all-option-tab position-rel tab-separator">
                            <span class="bwmsprite grid-icon margin-right10"></span>
                            <span class="inline-block font14">All photos</span>
                        </div>

                         <%if(VideoCount>1){ %>
                        <div data-bind="click: toggleVideoListScreen, visible: !photosTabActive(), css: videoListScreen() ? 'tab-active': ''" class="footer-tab all-option-tab position-rel tab-separator">
                            <span class="bwmsprite grid-icon margin-right10"></span>
                           <span class="inline-block font14">All videos</span>
                        </div>
                        <%} %>

                        <div data-bind="click: toggleFullScreen, visible: photosTabActive(), css: fullScreenModeActive() ? 'fullscreen-active' : ''" class="footer-tab grid-3-tab">
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
                        <div id="thumbnail-photo-swiper" class="swiper-container thumbnail-swiper">
                            <div class="swiper-wrapper" data-bind="foreach: photoList">
                                <div class="swiper-slide">
                                    <img data-bind="attr: { alt: ImageTitle, title: ImageTitle, src: HostUrl + '/110x61/' + OriginalImgPath }" src="" alt="" title="" border="0" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="color-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" data-bind="css: colorsThumbnailScreen() ? 'position-fixed' : ''">
                        <div id="thumbnail-colors-swiper" class="swiper-container color-thumbnail-swiper">
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
                    
                  <%if(bikeInfo!=null){ %>  <div id="info-tab-screen" class="footer-tab-card" data-bind="css: modelInfoScreen() ? 'position-fixed' : ''">
                        <div class="model-more-info-section padding-15-20 ribbon-present"><!-- add class 'ribbon-present' for upcoming and discontinued bike -->
                           <%if(IsUpcoming){ %><p class="model-ribbon-tag upcoming-ribbon">Upcoming</p><%} %>
                            <%if(IsDiscontinued){ %>
                            <p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
                            <%} %>
                            <div class="margin-bottom10">
                                <a href="<%=bikeUrl %>" class="item-image-content vertical-top" title="<%=bikeName %>">
                                    <img src="<%=Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._110x61)%>" alt="<%=bikeName %>">
                                </a>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="<%=bikeUrl %>" class="block text-bold text-default text-truncate" title="<%=bikeName %>"><%=bikeName%></a></h3>
                                    <ul class="item-more-details-list">
                                        <%if(bikeInfo.ExpertReviewsCount>0) {%>
                                        <li>
                                            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%=bikeName %> Reviews">
                                                <span class="bwmsprite reviews-sm"></span>
                                                <span class="icon-label">Reviews</span>
                                            </a>
                                        </li>        
                                        <%} %>
                                        <%if(bikeInfo.NewsCount>0){ %>
                                        <li>
                                            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatNewsUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%=bikeName %> News">
                                                <span class="bwmsprite news-sm"></span>
                                                <span class="icon-label">News</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if(bikeInfo.IsSpecsAvailable) {%>
                                        <li>
                                            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%=bikeName %> Specification">
                                                <span class="bwmsprite specs-sm"></span>
                                                <span class="icon-label">Specs</span>
                                            </a>
                                        </li>         
                                        <%} %>
                                    </ul>
                                </div>
                            </div>
                            <%if(!IsUpcoming&&!IsDiscontinued){ %>
                            
                            <div class="grid-7 alpha omega">
                                <p class="font11 text-light-grey text-truncate">Ex-showroom, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                                <div>
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                                </div>
                            </div>
                            <%}else if (IsUpcoming){ %>
                               <div class="grid-7 alpha omega">
                                <p class="font11 text-light-grey text-truncate">Expected price</p>
                                <div>
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMin)) %> onwards</span>
                                </div>
                            </div>
                            <%}else if (IsDiscontinued){ %>
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
                    </div>  <%} %>         
                
                    <div id="video-tab-screen" class="footer-tab-card font14" data-bind="visible: videoListScreen()">
                        <ul class="video-tab-list" data-bind="foreach: videoList">
                            <li data-bind="click: $parent.videoSelection, attr: { 'data-video-id': VideoId }">
                                <div class="video-image-block inline-block">
                                    <img data-bind="attr: { alt: VideoTitle, src: 'https://img.youtube.com/vi/'+VideoId+'/sddefault.jpg' }" border="0" />
                                    <span class="play-icon-wrapper">
                                        <span class="bwmsprite video-play-icon"></span>
                                    </span>
                                </div>
                                <p class="video-title-block padding-left15 inline-block" data-bind="text: VideoTitle"></p>
                            </li>
                        </ul>
                    </div>         
                </div>
            </script>
        </div>        
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            var photoCount = "<%= vmModelPhotos!=null ?  vmModelPhotos.totalPhotosCount : 0 %>";
            var modelId = "<%= modelId%>";
            var isModelPage = "<%= isModelPage.ToString().ToLower() %>";
            var ModelId="<%=vmModelPhotos.objModel.ModelId%>";
            var videoCount = "<%=VideoCount%>";
            var modelName = "<%= vmModelPhotos.bikeName %>";            
            var encodedVideoList = "<%= JSONVideoList%>"
            var encodedImageList = "<%= JSONImageList %>"
        </script> 
        
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/photos.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            $(".gallery-close-btn").on('click', function () {
                if(isModelPage) {
                    gallery.gotoModelPage();
                }
                else if(!isModelPage) {
                    gallery.close();
                }
            });
            $(document).ready(function () {
                if(isModelPage == 'true')
                {   
                    bindGallery($(this));
                }
            });
        </script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>

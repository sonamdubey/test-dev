<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.New.Photos.Default" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/m/controls/GenericBikeInfoControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="SimilarBikeWithPhotos" Src="~/m/controls/SimilarBikeWithPhotos.ascx" %>
<!DOCTYPE html>
<html>
<head>
   <%  if (vmModelPhotos != null && vmModelPhotos.pageMetas != null) {
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
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% if (vmModelPhotos != null)
        {
            var objImages = vmModelPhotos.objImageList; %>
        <section>
            <div class="container box-shadow section-bottom-margin">
                <h1 class="section-header bg-white"><%= vmModelPhotos.bikeName %> Photos</h1>
                 <% int i = 0; if (vmModelPhotos.totalPhotosCount > 0)
                   { %>
                <ul class="photos-grid-list">
                     <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < 6) //to handle lazy load for initial images (6 images can vary) 
                       { %>
                    <li>
                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>"  alt="<%= objImages[i].ImageCategory %> Image"  title="<%= objImages[i++].ImageCategory %>"/>
                    </li>
                    <% } %>
                     <% while (i < vmModelPhotos.gridPhotosCount && i < vmModelPhotos.gridSize)
                       { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                    </li>
                <% }  %>

                </ul>
                 <% if (vmModelPhotos.totalPhotosCount < vmModelPhotos.gridSize && vmModelPhotos.nongridPhotosCount > 0) { %>
                <ul class="photos-grid-list photos-remainder-<%= vmModelPhotos.nongridPhotosCount %> remainder-grid-list">
                    <% while (i < vmModelPhotos.totalPhotosCount && i < vmModelPhotos.gridSize)
                         { %>
                    <li>
                         <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                    </li>
                     <% } %>
                </ul>
                <% } } %>
                <div class="clear"></div>
            </div>
        </section>
        <% if(!isUpcoming) { %>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <h2 class="margin-bottom15">Know more about this bike</h2>
                <BW:GenericBikeInfo  ID="ctrlGenericBikeInfo" runat="server" />
            </div>
        </section>
         <% } %>
        <% } %>

        <%if (ctrlVideos.FetchedRecordsCount > 0)
        { %>
        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <h2 class="margin-bottom15"><%= vmModelPhotos.bikeName %> Videos</h2>
                <BW:Videos runat="server" ID="ctrlVideos" />
            </div>
        </section>
        <% } %>

        <BW:SimilarBikeWithPhotos  ID="ctrlSimilarBikesWithPhotos" runat="server" />
        <%--<BW:ModelGallery ID="ctrlModelGallery" runat="server" />--%>
        <!-- model-gallery-container ends here -->

        <%--<div id="gallery-container" class="gallery-container">
            

            <div class="gallery-body">
                

                <div data-bind="visible: !galleryHeader().photosTabActive()">Videos</div>
            </div>

            <div class="gallery-footer" data-bind="css: $root.galleryHeader().photosTabActive() ? '' : 'grid-2-tab', with: galleryFooter">
                

                <!-- color -->
                <div id="color-tab-screen" class="footer-tab-card">
                    <div class="swiper-container card-container color-palette-swiper">
                        <ul class="swiper-wrapper">
                            <li class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="color-box color-count-one inline-block">
                                        <span style="background-color: #F3CF1C"></span>    
                                    </div>
                                    <p class="font16 inline-block">Yellow</p>
                                </div>
                            </li>
                            <li class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="color-box color-count-one inline-block">
                                        <span style="background-color: #F3CF1C"></span>    
                                    </div>
                                    <p class="font16 inline-block">Yellow</p>
                                </div>
                            </li>
                            <li class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="color-box color-count-one inline-block">
                                        <span style="background-color: #F3CF1C"></span>    
                                    </div>
                                    <p class="font16 inline-block">Yellow</p>
                                </div>
                            </li>
                            <li class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="color-box color-count-one inline-block">
                                        <span style="background-color: #F3CF1C"></span>    
                                    </div>
                                    <p class="font16 inline-block">Yellow</p>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>

                <!-- share -->
                <div id="share-tab-screen" class="footer-tab-card" data-bind="visible: shareScreen()">
                    <ul class="social-share-list">
                        <li>Whatsapp</li>
                        <li>Facebook</li>
                        <li>Twitter</li>
                    </ul>
                </div>

                <!-- model info -->
                <div id="info-tab-screen" class="footer-tab-card" data-bind="visible: modelScreen()">
                    <div class="model-more-info-section padding-15-20"><!-- add class 'ribbon-present' for upcoming and discontinued bike -->
                        <p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
                        <p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
                        <div class="margin-bottom10">
                            <a href="" class="item-image-content vertical-top" title="Bajaj Pulsar AS200">
                                <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" src="" alt="Bajaj Pulsar AS200">
                            </a>
                            <div class="bike-details-block vertical-top">
                                <h3 class="margin-bottom5"><a href="" class="block text-bold text-default text-truncate" title="Bajaj Pulsar AS200">Bajaj Pulsar AS200</a></h3>
                                <ul class="item-more-details-list">
                                    <li>
                                        <a href="" title="Bajaj Pulsar AS200 Reviews">
                                            <span class="bwmsprite reviews-sm"></span>
                                            <span class="icon-label">Reviews</span>
                                        </a>
                                    </li>        
                                    <li>
                                        <a href="" title="Bajaj Pulsar AS200 News">
                                            <span class="bwmsprite news-sm"></span>
                                            <span class="icon-label">News</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="" title="Bajaj Pulsar AS200 Specs">
                                            <span class="bwmsprite specs-sm"></span>
                                            <span class="icon-label">Specs</span>
                                        </a>
                                    </li>         
                                </ul>
                            </div>
                        </div>
                        <div class="grid-5 alpha omega">
                            <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                            <div>
                                <span class="bwmsprite inr-xsm-icon"></span>
                                <span class="font16 text-bold">50,615</span>
                            </div>
                        </div>
                        <div class="grid-7 omega">
                            <a href="" class="btn btn-white btn-size-180">View model details<span class="bwmsprite btn-red-arrow"></span></a>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>

            </div>
        </div>--%>

        <div id="gallery-root">
            <!-- ko component: "gallery-component" -->
            <!-- /ko -->
            <script type="text/html" id="gallery-template-wrapper">
                <!-- ko if: vmPhotosPage.activateGallery() -->
                    <div id="gallery-container" class="gallery-container" data-bind="template: { name: 'gallery-template', afterRender: afterRender }"></div>
                <!-- /ko -->
            </script>

            <script type="text/html" id="gallery-template">
                <!-- gallery header -->
                <div class="gallery-header" data-bind="visible: galleryTabsActive()">
                    <h2 class="text-white gallery-title">Bajaj Pulsar AS200 Photos</h2>
                    <span id="gallery-close-btn" class="position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer" data-bind="event: { click: function() { postAction(ACTIONS.HIDE_GALLERY) } }"></span>
                    <ul class="horizontal-tabs-wrapper">
                        <li data-bind="event: { click: function(){ postAction(ACTIONS.ACTIVATE_PHOTO_TAB) } }, css: photosTabActive() ? 'active': ''">Photos</li>
                        <li data-bind="event: { click: function(){ postAction(ACTIONS.DEACTIVATE_PHOTO_TAB) } }, css: !photosTabActive() ? 'active': ''">Videos</li>
                    </ul>
                </div>

                <!-- gallery body -->
                <div class="gallery-body">
                    <div id="main-photo-swiper" class="swiper-container gallery-swiper" data-bind="visible: photosTabActive()">
                        <div class="main-photo-swiper-details padding-bottom10 padding-right5 padding-left5" data-bind="visible: photoHeadingActive()">
                            <p class="grid-9 font14 text-white text-left" data-bind="text: activePhotoTitle()"></p>
                            <div class="grid-3 alpha font12 text-xx-light text-right position-rel pos-top3">
                                <span data-bind="text: activePhotoIndex()"></span> / <span data-bind="text: photoList.length"></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="swiper-wrapper" data-bind="foreach: photoList">
                            <div class="swiper-slide">
                                <img data-bind="attr: { alt: imageTitle, title: imageTitle, src: hostUrl + '/642x361/' + imagePathLarge }" src="" alt="" title="" border="0" />
                            </div>
                        </div>
                    </div>

                    <div data-bind="visible: !photosTabActive()">Videos</div>
                </div>

                <!-- gallery footer -->
                <div class="gallery-footer" data-bind="visible: galleryFooterActive(), css: photosTabActive() ? '' : 'grid-2-tab'">
                    <div class="footer-tabs-wrapper">
                        <div data-bind="event: { click: function() { postAction(ACTIONS.TOGGLE_PHOTO_THUMBNAIL_SCREEN) } }, visible: photosTabActive(), css: photoThumbnailScreen() ? 'tab-active': ''" class="footer-tab all-option-tab position-rel tab-separator">
                            <span class="bwmsprite grid-icon margin-right10"></span>
                            <span class="inline-block font14">All photos</span>
                        </div>
                        <div data-bind="visible: !photosTabActive()" class="footer-tab all-option-tab position-rel tab-separator">
                            <span class="bwmsprite grid-icon margin-right10"></span>
                            <span class="inline-block font14">All videos</span>
                        </div>

                        <div data-bind="event: { click: function() { postAction(ACTIONS.TOGGLE_COLORS_THUMBNAIL_SCREEN) } }, visible: photosTabActive(), css: colorsThumbnailScreen() ? 'tab-active' : ''" class="footer-tab grid-3-tab">
                            <span class="bwmsprite color-palette"></span>
                        </div>

                        <div class="footer-tab grid-3-tab">
                            <span class="bwmsprite share-icon"></span>
                        </div>

                        <div class="footer-tab grid-3-tab" data-bind="event: { click: function() { postAction(ACTIONS.TOGGLE_MODEL_INFO_SCREEN) } }, css: modelInfoScreen() ? 'tab-active' : ''">
                            <span class="bwmsprite info-icon"></span>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div id="thumbnail-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" data-bind="visible: photoThumbnailScreen()">
                        <div id="thumbnail-photo-swiper" class="swiper-container thumbnail-swiper">
                            <div class="swiper-wrapper" data-bind="foreach: photoList">
                                <div class="swiper-slide">
                                    <img data-bind="attr: { alt: imageTitle, title: imageTitle, src: hostUrl + '/110x61/' + imagePathThumbnail }" src="" alt="" title="" border="0" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="color-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" data-bind="visible: colorsThumbnailScreen()">
                        <div id="thumbnail-colors-swiper" class="swiper-container color-thumbnail-swiper">
                            <div class="swiper-wrapper">
                                <div class="swiper-slide">
                                    <div class="color-box inline-block color-count-two">
                                        <span style="background-color: #00A8DD"></span>
                                        <span style="background-color: #040004"></span>
                                    </div>
                                    <p class="color-box-label inline-block">Dual Tone Blue</p>
                                </div>
                                <div class="swiper-slide">
                                    <div class="color-box inline-block color-count-two">
                                        <span style="background-color: #DA251F"></span>
                                        <span style="background-color: #040004"></span>
                                    </div>
                                    <p class="color-box-label inline-block">Dual Tone Red</p>
                                </div>
                                <div class="swiper-slide">
                                    <div class="color-box inline-block color-count-two">
                                        <span style="background-color: #EFEFEF"></span>
                                        <span style="background-color: #040004"></span>
                                    </div>
                                    <p class="color-box-label inline-block">Dual Tone White</p>
                                </div>
                                <div class="swiper-slide">
                                    <div class="color-box inline-block color-count-one">
                                        <span style="background-color: #00AEE8"></span>
                                    </div>
                                    <p class="color-box-label inline-block">Deep Sky Blue</p>
                                </div>
                                <div class="swiper-slide">
                                    <div class="color-box inline-block color-count-one">
                                        <span style="background-color: #CE1417"></span>
                                    </div>
                                    <p class="color-box-label inline-block">Volcano Red</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="info-tab-screen" class="footer-tab-card" data-bind="visible: modelInfoScreen()">
                        <div class="model-more-info-section padding-15-20"><!-- add class 'ribbon-present' for upcoming and discontinued bike -->
                            <%--<p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>--%>
                            <%--<p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>--%>
                            <div class="margin-bottom10">
                                <a href="" class="item-image-content vertical-top" title="Bajaj Pulsar AS200">
                                    <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg" src="" alt="Bajaj Pulsar AS200">
                                </a>
                                <div class="bike-details-block vertical-top">
                                    <h3 class="margin-bottom5"><a href="" class="block text-bold text-default text-truncate" title="Bajaj Pulsar AS200">Bajaj Pulsar AS200</a></h3>
                                    <ul class="item-more-details-list">
                                        <li>
                                            <a href="" title="Bajaj Pulsar AS200 Reviews">
                                                <span class="bwmsprite reviews-sm"></span>
                                                <span class="icon-label">Reviews</span>
                                            </a>
                                        </li>        
                                        <li>
                                            <a href="" title="Bajaj Pulsar AS200 News">
                                                <span class="bwmsprite news-sm"></span>
                                                <span class="icon-label">News</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="" title="Bajaj Pulsar AS200 Specs">
                                                <span class="bwmsprite specs-sm"></span>
                                                <span class="icon-label">Specs</span>
                                            </a>
                                        </li>         
                                    </ul>
                                </div>
                            </div>
                            <div class="grid-5 alpha omega">
                                <p class="font11 text-light-grey text-truncate">Ex-showroom, Mumbai</p>
                                <div>
                                    <span class="bwmsprite inr-xsm-icon"></span>
                                    <span class="font16 text-bold">50,615</span>
                                </div>
                            </div>
                            <div class="grid-7 omega">
                                <a href="" class="btn btn-white btn-size-180">View model details<span class="bwmsprite btn-red-arrow"></span></a>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    
                </div>
            </script>
        </div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript">
            var photoCount = <%= vmModelPhotos!=null ?  vmModelPhotos.totalPhotosCount : 0 %>;
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/photos.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>

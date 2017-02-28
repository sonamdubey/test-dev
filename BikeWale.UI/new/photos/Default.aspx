﻿<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.New.Photos.Default" %>

<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/controls/GenericBikeInfoControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="SimilarBikeWithPhotos" Src="~/controls/SimilarBikeWithPhotos.ascx" %>
<%@ Register TagPrefix="BW" TagName="Videos" Src="~/controls/NewVideosControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%  if (vmModelPhotos != null && vmModelPhotos.pageMetas != null)
        {
            title = vmModelPhotos.pageMetas.Title;
            keywords = vmModelPhotos.pageMetas.Keywords;
            description = vmModelPhotos.pageMetas.Description;
            canonical = vmModelPhotos.pageMetas.CanonicalUrl;
            alternate = vmModelPhotos.pageMetas.AlternateUrl;
            enableOG = true;
            ogImage = vmModelPhotos.modelImage;
        }       
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/photos.css" />
    <!--[if lt IE 9]>
	    <style type="text/css">
            .photos-grid-list li { height: auto; min-height: 100px; }
        </style>
    <![endif]-->
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <%--<div class="gallery-loader-placeholder gallery-bg-overlay text-center">
            <span class="spin-loader fixed-loader"></span>
        </div>--%>

        <section class="container padding-top10">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url" title="Home">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/new-bikes-in-india/" itemprop="url" title="New Bikes">
                                <span itemprop="title">New Bikes</span>
                            </a>
                        </li>
                        <% if (vmModelPhotos != null && vmModelPhotos.objMake != null && vmModelPhotos.objModel != null)
                           { %>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%=vmModelPhotos.objMake.MaskingName%>-bikes/" itemprop="url" title="<%=vmModelPhotos.objMake.MakeName%> Bikes">
                                <span itemprop="title"><%=vmModelPhotos.objMake.MakeName%> Bikes</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%=vmModelPhotos.objMake.MaskingName%>-bikes/<%=vmModelPhotos.objModel.MaskingName%>/" title="<%=bikeName%>">
                                <span itemprop="title"><%=bikeName%></span>
                            </a>
                        </li>
                        <%} %>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span>Images</li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow bg-listing">
                        <h1 class="content-box-shadow padding-14-20"><%=bikeName%> Images</h1>
                        <% if (vmModelPhotos != null)
                           {
                               var objImages = vmModelPhotos.objImageList;                                
                        %>
                        <ul class="photos-grid-list model-main-image">
                            <li>
                                <%if (vmModelPhotos.firstImage != null)
                                  { %>
                                <div class="main-image-container">
                                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(vmModelPhotos.firstImage.OriginalImgPath,vmModelPhotos.firstImage.HostUrl,Bikewale.Utility.ImageSize._640x348)%>" alt="<%= vmModelPhotos.firstImage.ImageCategory %> Image" title="<%= vmModelPhotos.firstImage.ImageCategory %>" />
                                </div>
                                <% } %>
                            </li>
                        </ul>
                        <% if(!isModelPage) { %>
                        <% int i = 0; if (vmModelPhotos.totalPhotosCount > 0)
                           { %>
                        <ul class="photos-grid-list model-grid-images">
                            <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < 13) //to handle lazy load for initial images (12 images can vary) 
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
                        <% if (vmModelPhotos.totalPhotosCount < vmModelPhotos.GridSize && vmModelPhotos.nongridPhotosCount > 1)
                           { %>
                        <ul class="photos-grid-list photos-remainder-<%= vmModelPhotos.nongridPhotosCount %> remainder-grid-list model-grid-images">
                            <% while (i < vmModelPhotos.totalPhotosCount && i < vmModelPhotos.GridSize)
                               { %>
                            <li>
                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="<%= objImages[i].ImageCategory %> Image" title="<%= objImages[i++].ImageCategory %>" />
                            </li>
                            <% }
                           }
                           else if (vmModelPhotos.totalPhotosCount == 1)
                           { %>
                            <li>
                                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[0].OriginalImgPath,objImages[0].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" alt="<%= objImages[0].ImageCategory %> Image" title="<%= objImages[0].ImageCategory %>" />
                            </li>
                            <% } %>
                        </ul>
                        <% }
                           }
                           }%>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <BW:GenericBikeInfo runat="server" ID="ctrlGenericBikeInfo" />

        <% if (ctrlVideos.FetchedRecordsCount > 0)
           { %>
        <div class="container margin-bottom20">
            <div class="grid-12">
                <div id="modelVideosContent" class="content-box-shadow font14 padding-top20 padding-right10 padding-bottom20 padding-left10 margin-bottom20 border-solid-bottom">
                    <BW:Videos runat="server" ID="ctrlVideos" />
                </div>
            </div>
        </div>
        <% } %>

        <BW:SimilarBikeWithPhotos ID="ctrlSimilarBikesWithPhotos" runat="server" />


        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <div id="gallery-root">
            <div class="gallery-container gallery-bg-overlay" style="display: none;" data-bind="visible: isGalleryActive()">
                <div class="gallery-header">
                    <h2 class="text-white gallery-title"><%= bikeName %> Images</h2>
                    <span id="gallery-close-btn" class="bwsprite cross-md-white cur-pointer"></span>
                    <ul class="horizontal-tabs-wrapper">
                        <li data-bind="click: togglePhotoTab, css: photosTabActive() ? 'active' : ''">Images</li>
                        <%if (VideoCount > 0)
                          { %>
                        <li data-bind="click: togglePhotoTab, css: !photosTabActive() ? 'active' : ''">Videos</li>
                        <%} %>
                    </ul>
                    <div class="clear"></div>
                </div>

                <div class="gallery-body">
                    <div id="main-photo-swiper" class="gallery-swiper" data-bind="visible: photosTabActive() && photoSwiperActive()">
                        <div class="swiper-container gallery-type-swiper">
                            <div class="swiper-wrapper" data-bind="foreach: photoList">
                                <div class="swiper-slide">
                                    <div class="gallery-image-placeholder">
                                        <img class="swiper-lazy gallery-swiper-image" data-bind="attr: { alt: ImageTitle, 'data-src': HostUrl + '/1056x594/' + OriginalImgPath }" src="" alt="" border="0" />
                                    </div>
                                </div>
                            </div>
                            <div class="swiper-button-next bwsprite gallery-next-icon gallery-type-next"></div>
                            <div class="swiper-button-prev bwsprite gallery-prev-icon gallery-type-prev"></div>
                        </div>
                    </div>

                    <div id="main-color-swiper" class="gallery-swiper" data-bind="visible: photosTabActive() && !photoSwiperActive()">
                        <div class="swiper-container gallery-color-type-swiper">
                            <div class="swiper-wrapper" data-bind="foreach: colorPhotoList">
                                <div class="swiper-slide">
                                    <div class="gallery-image-placeholder">
                                        <img class="swiper-lazy gallery-swiper-image" data-bind="attr: { alt: ImageTitle, 'data-src': HostUrl + '/1056x594/' + OriginalImgPath }" src="" alt="" border="0" />
                                    </div>
                                </div>
                            </div>
                            <div class="swiper-button-next bwsprite gallery-next-icon color-type-next"></div>
                            <div class="swiper-button-prev bwsprite gallery-prev-icon color-type-prev"></div>
                        </div>
                    </div>

                    <div id="main-video-content" class="main-video-content" data-bind="visible: !photosTabActive()">
                        <div class="main-video-wrapper">
                            <div class="main-video-iframe-content">
                                <iframe width="976" height="549" data-bind="attr: { src: 'https://www.youtube.com/embed/' + activeVideoId() + '?showinfo=0' }" src="" frameborder="0" allowfullscreen></iframe>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="gallery-footer">
                    <div class="footer-tabs-wrapper" data-bind="css: !photosTabActive() && totalVideoCount < 2 ? 'video-tab-absent' : ''">
                        <div data-bind="click: toggleModelInfoScreen, css: modelInfoScreen() ? 'tab-active' : ''" class="footer-tab">
                            <span class="bwsprite info-icon"></span>
                            <span class="inline-block font14">Know more about the bike</span>
                        </div>

                        <div style="display: none" data-bind="click: toggleColorThumbnailScreen, visible: photosTabActive() && colorTabActive(), css: colorsThumbnailScreen() ? 'tab-active' : ''" class="footer-tab">
                            <span class="bwsprite color-palette"></span>
                            <span class="inline-block font14">Colours</span>
                        </div>

                        <div data-bind="click: togglePhotoThumbnailScreen, visible: photosTabActive(), css: photoThumbnailScreen() ? 'tab-active': ''" class="footer-tab">
                            <span class="bwsprite grid-icon"></span>
                            <span class="inline-block font14">All photos</span>
                        </div>

                        <div style="display: none" data-bind="click: toggleVideoListScreen, visible: !photosTabActive() && totalVideoCount > 1, css: videoListScreen() ? 'tab-active': ''" class="footer-tab">
                            <span class="bwsprite grid-icon"></span>
                            <span class="inline-block font14">All videos</span>
                        </div>

                        <div class="gallery-details-tab" style="display: none;" data-bind="visible: photosTabActive() || !photosTabActive(), css: !colorTabActive() || !photosTabActive() ? 'color-tab-absent' : ''">
                            <!-- ko if: photosTabActive() && photoSwiperActive() -->
                            <span class="swiper-heading text-truncate" data-bind="text: activePhotoTitle()"></span>
                            <span class="rightfloat text-light-grey">
                                <span data-bind="text: activePhotoIndex()"></span>/
                                <span data-bind="text: photoList().length"></span>
                            </span>
                            <!-- /ko -->
                            <!-- ko if: photosTabActive() && !photoSwiperActive() -->
                            <span class="swiper-heading text-truncate" data-bind="text: activeColorTitle()"></span>
                            <span class="rightfloat text-light-grey">
                                <span data-bind="text: activeColorIndex()"></span>/
                                <span data-bind="text: colorPhotoList().length"></span>
                            </span>
                            <!-- /ko -->
                            <!-- ko if: !photosTabActive() -->
                            <span class="swiper-heading text-truncate" data-bind="text: activeVideoTitle()"></span>
                            <span class="rightfloat text-light-grey" data-bind="visible: totalVideoCount > 1">
                                <span data-bind="text: activeVideoIndex()"></span>/
                                <span data-bind="text: totalVideoCount"></span>
                            </span>
                            <!-- /ko -->
                        </div>

                        <div class="clear"></div>
                    </div>


                    <div id="thumbnail-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" style="display: none" data-bind="visible: photoThumbnailScreen()">
                        <div class="swiper-container thumbnail-swiper">
                            <div class="swiper-wrapper" data-bind="foreach: photoList">
                                <div class="swiper-slide">
                                    <div class="thumbnail-image-placeholder">
                                        <img class="swiper-lazy" data-bind="attr: { alt: ImageTitle, title: ImageTitle, 'data-src': HostUrl + '/110x61/' + OriginalImgPath }" src="" alt="" title="" border="0" />
                                    </div>
                                </div>
                            </div>
                            <div class="swiper-button-next thumbnail-type-next">
                                <span class="bwsprite thumbnail-next-icon"></span>
                            </div>
                            <div class="swiper-button-prev thumbnail-type-prev">
                                <span class="bwsprite thumbnail-prev-icon"></span>
                            </div>
                        </div>
                    </div>

                    <div id="color-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" style="display: none" data-bind="visible: colorsThumbnailScreen()">
                        <div class="swiper-container color-thumbnail-swiper">
                            <div class="swiper-wrapper" data-bind="foreach: colorPhotoList">
                                <div class="swiper-slide">
                                    <div class="color-box inline-block" data-bind="foreach: Colors, css: (Colors.length == 3) ? 'color-count-three' : (Colors.length == 2) ? 'color-count-two' : 'color-count-one'">
                                        <span data-bind="style: { 'background-color': '#' + $data }"></span>
                                    </div>
                                    <p class="color-box-label inline-block" data-bind="text: (ImageTitle.length > 20 ? ImageTitle.substring(0, 20) + '...' : ImageTitle)"></p>
                                </div>
                            </div>
                            <div class="swiper-button-next color-thumbnail-type-next">
                                <span class="bwsprite thumbnail-next-icon"></span>
                            </div>
                            <div class="swiper-button-prev color-thumbnail-type-prev">
                                <span class="bwsprite thumbnail-prev-icon"></span>
                            </div>
                        </div>
                    </div>

                    <div id="info-tab-screen" class="footer-tab-card" style="display: none" data-bind="visible: modelInfoScreen()">
                        <div class="model-more-info-section">
                            <%if (IsUpcoming)
                              { %><p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
                            <%} %>
                            <%if (IsDiscontinued)
                              { %><p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
                            <%} %>
                            <div class="clear"></div>
                            <div class="info-grid-9 inline-block">
                                <a href="<%=bikeUrl %>" class="item-image-content inline-block" title="<%=bikeName %>">
                                    <img src="<%=Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._110x61)%>" alt="<%= bikeName %>" title="<%= bikeName %>">
                                </a>
                                <div class="bike-details-block inline-block">
                                    <h3><a href="<%=bikeUrl %>" class="text-default"><%= bikeName %></a></h3>
                                    <ul class="item-more-details-list inline-block">
                                        <%if (bikeInfo.ExpertReviewsCount > 0)
                                          {%>
                                        <li>
                                            <a href="<%= Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Expert reviews">
                                                <span class="bwsprite reviews-sm"></span>
                                                <span class="icon-label">Expert reviews</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if (bikeInfo.NewsCount > 0)
                                          { %>
                                        <li>
                                            <a href="<%= Bikewale.Utility.UrlFormatter.FormatNewsUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> News">
                                                <span class="bwsprite news-sm"></span>
                                                <span class="icon-label">News</span>
                                            </a>
                                        </li>
                                        <%} %>
                                        <%if (bikeInfo.IsSpecsAvailable)
                                          {%>
                                        <li>
                                            <a href="<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Specification">
                                                <span class="bwsprite specs-sm"></span>
                                                <span class="icon-label">Specs</span>
                                            </a>
                                        </li>
                                        <%} %>
                                    </ul>
                                </div>
                                <a href="<%=bikeUrl %>" title="<%= bikeName %>" class="btn btn-white btn-162-34">View details<span class="bwsprite btn-red-arrow"></span></a>
                            </div>

                            <%if (!IsUpcoming && !IsDiscontinued)
                              { %>
                            <div class="info-grid-3 inline-block">
                                <p class="font12 text-light-grey text-truncate margin-bottom5">Ex-showroom price, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>

                                <span class="bwsprite inr-md"></span>
                                <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                            </div>
                            <%}
                              else if (IsUpcoming)
                              { %>
                            <div class="info-grid-3 inline-block">
                                <p class="font12 text-light-grey text-truncate margin-bottom5">Expected price</p>

                                <span class="bwsprite inr-md"></span>
                                <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMin)) %> onwards</span>
                            </div>
                            <%}
                              else if (IsDiscontinued)
                              { %>
                            <div class="info-grid-3 inline-block">
                                <p class="font12 text-light-grey text-truncate margin-bottom5">Last know price</p>

                                <span class="bwsprite inr-md"></span>
                                <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                            </div>
                            <% } %>
                            <div class="clear"></div>
                        </div>
                    </div>

                    <div id="video-tab-screen" class="footer-tab-card padding-top20 padding-bottom20" style="display: none" data-bind="visible: videoListScreen()">
                        <div class="swiper-container video-thumbnail-swiper">
                            <div class="swiper-wrapper" data-bind="foreach: videoList">
                                <div class="swiper-slide">
                                    <div class="video-image-block inline-block">
                                        <img class="swiper-lazy" data-bind="attr: { alt: VideoTitle, title: VideoTitle, src: 'https://img.youtube.com/vi/'+VideoId+'/default.jpg' }" border="0" />
                                        <span class="play-icon-wrapper">
                                            <span class="bwsprite video-play-icon"></span>
                                        </span>
                                    </div>
                                    <p class="video-title-block padding-left20 inline-block" data-bind="text: VideoTitle"></p>
                                </div>
                            </div>
                            <div class="swiper-button-next video-type-next">
                                <span class="bwsprite thumbnail-next-icon"></span>
                            </div>
                            <div class="swiper-button-prev video-type-prev">
                                <span class="bwsprite thumbnail-prev-icon"></span>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="fallback-model-gallery">
            <div class="blackOut-window-model"></div>
            <div class="modelgallery-close-btn position-abt pos-top20 pos-right20 bwsprite cross-lg-white cur-pointer hide"></div>

            <div class="bike-gallery-popup bw-tabs-panel">
                <% if (VideoCount > 0)
                   { %>
                <div class="text-center photos-videos-tabs margin-bottom20">
                    <div class="bw-tabs home-tabs">
                        <ul>
                            <li class="active" data-tabs="Photos" id="photos-tab">Images</li>
                            <li data-tabs="Videos" id="videos-tab">Videos</li>
                        </ul>
                    </div>
                </div>
                <% } %>
                <div class="bike-gallery-heading margin-bottom20 margin-left30 <%= VideoCount == 0 ? "margin-top90" : "" %>">
                    <p class="font18 text-bold text-white"><%= bikeName %></p>
                </div>
                <div class="bw-tabs-data" id="Photos">
                    <div class="connected-carousels-photos">
                        <div class="stage-photos stage-media">
                            <div class="carousel-photos carousel-stage-photos carousel-stage-media">
                                <ul data-bind="foreach: photoList">
                                    <li>
                                        <div class="gallery-photo-img-container">
                                            <span>
                                                <img class="lazy" data-bind="attr: { alt: ImageTitle, 'data-original': HostUrl + '/1056x594/' + OriginalImgPath }" src="https://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" alt="" border="0" />
                                            </span>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="bike-gallery-details">
                                <span class="leftfloat bike-gallery-title"></span>
                                <span class="rightfloat bike-gallery-count"></span>
                            </div>
                            <a href="#" class="prev photos-prev-stage bwsprite media-prev-next-stage" rel="nofollow"></a>
                            <a href="#" class="next photos-next-stage bwsprite media-prev-next-stage" rel="nofollow"></a>
                        </div>
                        <div class="navigation-photos navigation-media">
                            <a href="#" class="prev photos-prev-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                            <a href="#" class="next photos-next-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                            <div class="carousel-photos carousel-navigation-photos carousel-navigation-media">
                                <ul data-bind="foreach: photoList">
                                    <li>
                                        <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-bind="attr: { alt: ImageTitle, 'data-original': HostUrl + '/144x81/' + OriginalImgPath }" src="" alt="" border="0" />
                                            </span>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <% if (VideoCount > 0)
                   { %>
                <div class="bw-tabs-data hide" id="Videos">
                    <div class="connected-carousels-videos">
                        <div class="stage-videos stage-media">
                            <div class="carousel-videos carousel-stage-videos carousel-stage-media">
                                <div class="yt-iframe-container">
                                    <iframe id="video-iframe" src="" frameborder="0" allowfullscreen></iframe>
                                </div>
                            </div>
                        </div>
                        <div class="navigation-videos navigation-media">
                            <a href="#" class="prev videos-prev-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                            <a href="#" class="next videos-next-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                            <div class="carousel-videos carousel-navigation-videos carousel-navigation-media">
                                <ul data-bind="foreach: videoList">
                                    <li>
                                        <div class="yt-iframe-container">
                                            <img data-bind="attr: { alt: VideoTitle, title: VideoTitle, src: 'https://img.youtube.com/vi/'+VideoId+'/default.jpg', 'iframe-data': VideoUrl }" border="0" />
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <% } %>
            </div>
        </div>

        <!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            var photoCount = <%= vmModelPhotos.totalPhotosCount + 1%>;
            var ModelId = <%=vmModelPhotos.objModel.ModelId%>;
            var videoCount = <%=VideoCount%>;
            var modelName = "<%= vmModelPhotos.bikeName %>";  
            var imageIndex = "<%=imageIndex%>";
            var isModelPage = "<%= isModelPage.ToString().ToLower() %>";
            var encodedVideoList = "<%= JSONVideoList%>"
            var encodedImageList = "<%= JSONImageList %>"
            var encodedFirstImage = "<%= JSONFirstImage%>"                        
        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/swiper-3.1.7.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/photos.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                if(isModelPage == 'true')
                {                     
                    popupGallery.bindGallery(imageIndex);
                }
            });
        </script>
        <!--[if lt IE 10]>
            <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/fallback-gallery.js?<%=staticFileVersion%>"></script>
        <![endif]-->

        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>

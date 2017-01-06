<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.New.Photos.Default" %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head runat="server">
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
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
         <% if (vmModelPhotos != null)
           {
               var objImages = vmModelPhotos.objImageList; %>
        <section>
            <div class="container box-shadow section-bottom-margin">
                <h1 class="section-header"><%= vmModelPhotos.bikeName %> Photos</h1>
                 <% int i = 0; if (vmModelPhotos.totalPhotosCount > 0)
                   { %>
                <ul class="photos-grid-list">
                     <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < 6) //to handle lazy load for initial images (6 images can vary) 
                       { %>
                    <li>
                        <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>"  alt="Model Image" />
                    </li>
                    <% } %>
                     <% while (i < vmModelPhotos.gridPhotosCount - 1 && i < vmModelPhotos.gridSize)
                       { %>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                    <li>
                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                
                <% }  %>

                </ul>
                 <% if (vmModelPhotos.totalPhotosCount < vmModelPhotos.gridSize && vmModelPhotos.nongridPhotosCount > 0) { %>
                <ul class="photos-grid-list photos-remainder-<%= vmModelPhotos.nongridPhotosCount %> remainder-grid-list">
                    <% while (i < vmModelPhotos.totalPhotosCount && i < vmModelPhotos.gridSize)
                         { %>
                    <li>
                         <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objImages[i].OriginalImgPath,objImages[i++].HostUrl,Bikewale.Utility.ImageSize._476x268) %>" src="" alt="Model Image" />
                    </li>
                     <% } %>
                </ul>
                <% } } %>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <h2 class="margin-bottom15">Know more about this bike</h2>
                <div class="model-more-info-section">
                    <div class="margin-bottom10">
                        <a href="" class="item-image-content inline-block">
                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg?20151209184344" src="" alt="Bajaj Pulsar RS200" />
                        </a>
                        <div class="bike-details-block inline-block">
                            <h3 class="margin-bottom5"><a href="" class="block text-bold text-default text-truncate">Bajaj Pulsar RS200</a></h3>
                            <ul class="key-specs-list font12 text-xx-light">
                                <li>
                                    <span>124 cc</span>
                                </li>
                                <li>
                                    <span>54 kmpl</span>
                                </li>
                                <li>
                                    <span>8.6 bhp</span>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <ul class="item-more-details-list">
                        <li>
                            <a href="" title="Honda CB Shine Expert Reviews">
                                <span class="generic-sprite reviews-sm"></span>
                                <span class="icon-label">Reviews</span>
                            </a>
                        </li>
                        <li>
                            <a href="" title="Honda CB Shine News">
                                <span class="generic-sprite news-sm"></span>
                                <span class="icon-label">News</span>
                            </a>
                        </li>
                        <li>
                            <a href="" title="Honda CB Shine Specification">
                                <span class="generic-sprite specs-sm"></span>
                                <span class="icon-label">Specs</span>
                            </a>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <div class="margin-top5 margin-bottom5">
                        <p class="font13 text-grey">Ex-showroom, Mumbai</p>
                        <div class="margin-bottom10">
                            <span class="bwmsprite inr-xsm-icon"></span>
                            <span class="font16 text-bold">57,931</span>
                        </div>
                        <button type="button" class="btn btn-white font14 btn-size-180">Check on-road price</button>
                    </div>
                </div>
            </div>
        </section>

        <% } %>

        <section>
            <div class="container bg-white box-shadow padding-15-20 section-bottom-margin">
                <%if (ctrlVideos.FetchedRecordsCount > 0)
                    { %>
                    <h2 class="margin-bottom15"><%= vmModelPhotos.bikeName %> Videos</h2>
                    <BW:Videos runat="server" ID="ctrlVideos" />
                <% } %>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow section-bottom-margin">
                <h2 class="padding-15-20">Photos for alternative bikes</h2>
                <div class="swiper-container card-container alternate-bikes-photo-swiper">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="Bajaj Pulsar RS200 Photos" class="block swiper-card-target">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/160x89/bw/models/bajaj-pulsar-rs200.jpg" alt="Bajaj Pulsar RS200 Photos" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                        <span class="black-overlay">
                                            <span class="bwmsprite photos-white-icon"></span>
                                        </span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="margin-bottom5 text-truncate">Bajaj Pulsar RS200</h3>
                                        <p class="font14 text-default text-truncate">128 photos</p>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="Bajaj Pulsar RS200 Photos" class="block swiper-card-target">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/160x89/bw/models/bajaj-pulsar-rs200.jpg" alt="Bajaj Pulsar RS200 Photos" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                        <span class="black-overlay">
                                            <span class="bwmsprite photos-white-icon"></span>
                                        </span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="margin-bottom5 text-truncate">Bajaj Pulsar RS200</h3>
                                        <p class="font14 text-default text-truncate">128 photos</p>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="" title="Bajaj Pulsar RS200 Photos" class="block swiper-card-target">
                                    <div class="swiper-image-preview">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/160x89/bw/models/bajaj-pulsar-rs200.jpg" alt="Bajaj Pulsar RS200 Photos" src="" />
                                        <span class="swiper-lazy-preloader"></span>
                                        <span class="black-overlay">
                                            <span class="bwmsprite photos-white-icon"></span>
                                        </span>
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="margin-bottom5 text-truncate">Bajaj Pulsar RS200</h3>
                                        <p class="font14 text-default text-truncate">128 photos</p>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="model-gallery-container relative-gallery-container">
                <h3 class="font16 text-white">Bajaj Pular RS200 Photos</h3>

                <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

                <div class="bw-tabs-panel">

                    <ul class="bw-tabs horizontal-tabs-wrapper">
                        <li class="active" data-tabs="photos" id="photos-tab">Photos</li>
                        <li data-tabs="videos" id="videos-tab">Videos</li>
                    </ul>

                    <div id="bike-gallery-popup">
                        <div class="bw-tabs-data" id="photos">
                            <div class="font14 text-white margin-bottom15">
                                <span class="leftfloat media-title"></span>
                                <span class="rightfloat gallery-count"></span>
                                <div class="clear"></div>
                            </div>
                            <div class="connected-carousels-photos">
                                <div class="stage-photos">
                                    <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                                        <div class="swiper-wrapper">
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd7.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Side-50239.jpg?20151004164119&t=164119207&t=164119207" src="" alt="" title="Model Image" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd8.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Side-50240.jpg?20151004164217&t=164217690&t=164217690" src="" alt="" title="Model Image" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd5.aeplcdn.com//310x174//n/bw/olwlr33_2812.jpg" src="" alt="" title="Model Image" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd6.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Exterior-50242.jpg?20151004165351&t=165351703&t=165351703" src="" alt="" title="Model Image" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd7.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Exterior-50243.jpg?20151004165357&t=165357333&t=165357333" src="" alt="" title="Model Image" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                        </div>
                                        <div class="bwmsprite swiper-button-next"></div>
                                        <div class="bwmsprite swiper-button-prev"></div>
                                    </div>
                                </div>

                                <div class="navigation-photos">
                                    <div class="swiper-container noSwiper carousel-navigation-photos">
                                        <div class="swiper-wrapper">
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd7.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Side-50239.jpg?20151004164119&t=164119207&t=164119207" src="" alt="" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd8.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Side-50240.jpg?20151004164217&t=164217690&t=164217690" src="" alt="" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd5.aeplcdn.com//310x174//n/bw/olwlr33_2812.jpg" src="" alt="" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd6.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Exterior-50242.jpg?20151004165351&t=165351703&t=165351703" src="" alt="" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="swiper-slide">
                                                <img class="swiper-lazy" data-src="https://imgd7.aeplcdn.com//476x268//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Exterior-50243.jpg?20151004165357&t=165357333&t=165357333" src="" alt="" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="bw-tabs-data" id="videos">
                            <div class="connected-carousels-videos">
                                <div class="stage-videos">
                                    <div class="carousel-videos carousel-stage-videos">
                                        <div class="yt-iframe-preview">
                                            <iframe id="video-iframe" src="" frameborder="0" allowfullscreen></iframe>
                                        </div>
                                    </div>
                                </div>
                                <div class="navigation-videos">
                                    <div class="swiper-container noSwiper carousel-navigation-videos">
                                        <div class="swiper-wrapper">
                                            <div class="swiper-slide">
                                                <img iframe-data="https://www.youtube.com/embed/2olrXRfebos?rel=0&showinfo=0" src="http://img.youtube.com/vi/2olrXRfebos/1.jpg" width="83" height="47" />
                                            </div>
                                            <div class="swiper-slide">
                                                <img iframe-data="https://www.youtube.com/embed/8Pvu-DqbsCc?rel=0&showinfo=0" src="http://img.youtube.com/vi/8Pvu-DqbsCc/1.jpg" width="83" height="47" />
                                            </div>
                                            <div class="swiper-slide">
                                                <img iframe-data="https://www.youtube.com/embed/h399XRm-OcA?rel=0&showinfo=0" src="http://img.youtube.com/vi/h399XRm-OcA/1.jpg" width="83" height="47" />
                                            </div>
                                            <div class="swiper-slide">
                                                <img iframe-data="https://www.youtube.com/embed/SqqTPir9v9g?rel=0&showinfo=0" src="http://img.youtube.com/vi/SqqTPir9v9g/1.jpg" width="83" height="47" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </section>
        <!-- model-gallery-container ends here -->


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

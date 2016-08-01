<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.New.ModelGallery" EnableViewState="false" Trace="false" %>

<!DOCTYPE html>
<html>
<head>
    <title>Model Gallery</title>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/model-gallery.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <section class="model-gallery-container">
        <h1 class="font16 text-white">Bajaj Pulsar AS200 Photos</h1>
        <div class="gallery-close-btn position-abt pos-top15 pos-right15 bwmsprite cross-md-white cur-pointer"></div>

        <div class="bw-tabs-panel">
            <ul class="bw-tabs horizontal-tabs-wrapper">
                <li class="active" data-tabs="photos" id="photos-tab">Photos</li>
                <li data-tabs="videos" id="videos-tab">Videos</li>
            </ul>

            <div id="bike-gallery-popup">
                <div class="bw-tabs-data" id="photos">
                    <div class="font14 text-white margin-bottom15">
                        <span class="leftfloat media-title">Rigth-side Front Three Quarter</span>
                        <span class="rightfloat gallery-count"></span>
                        <div class="clear"></div>
                    </div>

                    <div class="connected-carousels-photos">
                        <div class="stage-photos">
                            <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                                <div class="swiper-wrapper">
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="http://imgd7.aeplcdn.com//476x268//bikewaleimg/ec/15246/img/l/TVS-Wego-Instrument-cluster-47839.jpg?20151702124531&t=124531253&t=124531253" src="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="http://imgd6.aeplcdn.com//476x268//bikewaleimg/ec/15246/img/l/TVS-Wego-Exterior-47838.jpg?20151702124521&t=124521803&t=124521803" src="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="http://imgd5.aeplcdn.com//476x268//bikewaleimg/ec/15246/img/l/TVS-Wego-Exterior-47837.jpg?20151702124512&t=124512167&t=124512167" src="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="http://imgd5.aeplcdn.com//476x268//bikewaleimg/ec/15246/img/l/TVS-Wego-Exterior-47833.jpg?20151702124435&t=124435577&t=124435577" src="" alt="" />
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
                                        <img class="swiper-lazy" data-src="http://imgd8.aeplcdn.com//110x61//bikewaleimg/ec/15246/img/l/TVS-Wego-Side-47828.jpg?20151702124340&t=124340420&t=124340420" src="" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="http://imgd7.aeplcdn.com//110x61//bikewaleimg/ec/15246/img/l/TVS-Wego-Side-47827.jpg?20151702124329&t=124329740&t=124329740" src="" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="http://imgd6.aeplcdn.com//110x61//bikewaleimg/ec/15246/img/l/TVS-Wego-Side-47826.jpg?20151702124319&t=124319267&t=124319267" src="" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="http://imgd5.aeplcdn.com//110x61//bikewaleimg/ec/15246/img/l/TVS-Wego-Exterior-47825.jpg?20151702124307&t=124307153&t=124307153" src="" title="" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                </div>
                                <div class="bwmsprite swiper-button-next hide"></div>
                                <div class="bwmsprite swiper-button-prev hide"></div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="bw-tabs-data" id="videos">
                    <div class="connected-carousels-videos">
                        <div class="stage-videos">
                            <div class="carousel-videos carousel-stage-videos">
                                <div class="yt-iframe-preview">
                                    <iframe id="video-iframe" src="https://www.youtube.com/embed/yJr0xNsMIyM?rel=0&showinfo=0" frameborder="0" allowfullscreen></iframe>
                                </div>
                            </div>
                        </div>
                        <div class="navigation-videos">
                            <div class="swiper-container noSwiper carousel-navigation-videos">
                                <div class="swiper-wrapper">
                                    <div class="swiper-slide">
                                        <img iframe-data="https://www.youtube.com/embed/yJr0xNsMIyM?rel=0&showinfo=0" src="http://img.youtube.com/vi/yJr0xNsMIyM/1.jpg" width="90" height="50" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img iframe-data="https://www.youtube.com/embed/yZLFEqJ-8ck?rel=0&showinfo=0" src="http://img.youtube.com/vi/yZLFEqJ-8ck/1.jpg" width="90" height="50" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img iframe-data="https://www.youtube.com/embed/ilVJpoSXroA?rel=0&showinfo=0" src="http://img.youtube.com/vi/ilVJpoSXroA/1.jpg" width="90" height="50" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img iframe-data="https://www.youtube.com/embed/5rdeEL1cU_c?rel=0&showinfo=0" src="http://img.youtube.com/vi/5rdeEL1cU_c/1.jpg" width="90" height="50" />
                                    </div>
                                    <div class="swiper-slide">
                                        <img iframe-data="https://www.youtube.com/embed/yZLFEqJ-8ck?rel=0&showinfo=0" src="http://img.youtube.com/vi/lGWVF35kHBQ/1.jpg" width="90" height="50" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </section>    
    <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/model-gallery.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>

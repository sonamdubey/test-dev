<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.TrackDay.Default" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        //title       = "Bike News - Latest Indian Bike News & Views - BikeWale";
        //description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
        //keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
        //canonical   = "https://www.bikewale.com/news/";
        //AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        //AdId = "1398766302464";
        //Ad_320x50 = true;
        //Ad_Bot_320x50 = true;
    %>
   
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday/css/track-day.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <% if(!androidApp) { %>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% } %>

        <section>
            <div class="container trackday-banner text-center section-container">
                <h1 class="font24 text-uppercase text-white padding-bottom10">Track Day 2016</h1>
                <h2 class="font14 text-unbold text-white">Lorem ipsum dolor sit amet, consecte</h2>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Articles</h2>
                <div class="content-box-shadow padding-right20 padding-left20 font14">
                    <ul class="article-list">
                        <li>
                            <div class="review-image-full-width-wrapper">
                                <a href="" title="Honda Brazil launches fuel injected CG125i motorcycle" class="block">
                                    <div class="bg-loader-placeholder">
                                        <img class="lazy" data-original="https://imgd2.aeplcdn.com//370x208//bw/ec/21691/Honda-Exterior-65001.jpg" alt="Honda Brazil launches fuel injected CG125i motorcycle" src="">
                                    </div>
                                </a>
                            </div>
                            <div class="review-heading-full-width-wrapper">
                                <h3>
                                    <a href="/m/news/21691-honda-brazil-launches-fuel-injected-cg125i-motorcycle.html" class="target-link">TVS Apache RTR 200 4V vs Bajaj Pulsar RS200: Comparsion Test</a>
                                </h3>
                                <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Nov 15, 2016</span>
                                </div>
                                <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Santosh Nair</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">Honda motorcycles Brazil has launched the CG125i for the Brazilian market at 6,790 Real (Rs 1.12 lakh). Honda’s Brazilian arm introduced the upgraded version in a bid...</p>
                        </li>
                        <li>
                            <div class="review-image-wrapper">
                                <a href="" title="Honda Brazil launches fuel injected CG125i motorcycle" class="block">
                                    <div class="bg-loader-placeholder">
                                        <img class="lazy" data-original="https://imgd2.aeplcdn.com//370x208//bw/ec/21691/Honda-Exterior-65001.jpg" alt="Honda Brazil launches fuel injected CG125i motorcycle" src="">
                                    </div>
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <h3>
                                    <a href="/m/news/21691-honda-brazil-launches-fuel-injected-cg125i-motorcycle.html" class="target-link">Honda Brazil launches fuel injected...</a>
                                </h3>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Nov 15, 2016</span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Santosh Nair</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">Honda motorcycles Brazil has launched the CG125i for the Brazilian market at 6,790 Real (Rs 1.12 lakh). Honda’s Brazilian arm introduced the upgraded version in a bid...</p>
                        </li>
                        <li>
                            <div class="review-image-full-width-wrapper">
                                <a href="" title="Honda Brazil launches fuel injected CG125i motorcycle" class="block">
                                    <div class="bg-loader-placeholder">
                                        <img class="lazy" data-original="https://imgd2.aeplcdn.com//370x208//bw/ec/21691/Honda-Exterior-65001.jpg" alt="Honda Brazil launches fuel injected CG125i motorcycle" src="">
                                    </div>
                                </a>
                            </div>
                            <div class="review-heading-full-width-wrapper">
                                <h3>
                                    <a href="/m/news/21691-honda-brazil-launches-fuel-injected-cg125i-motorcycle.html" class="target-link">TVS Apache RTR 200 4V vs Bajaj Pulsar RS200: Comparsion Test</a>
                                </h3>
                                <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Nov 15, 2016</span>
                                </div>
                                <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Santosh Nair</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">Honda motorcycles Brazil has launched the CG125i for the Brazilian market at 6,790 Real (Rs 1.12 lakh). Honda’s Brazilian arm introduced the upgraded version in a bid...</p>
                        </li>
                        <li>
                            <div class="review-image-wrapper">
                                <a href="" title="Honda Brazil launches fuel injected CG125i motorcycle" class="block">
                                    <div class="bg-loader-placeholder">
                                        <img class="lazy" data-original="https://imgd2.aeplcdn.com//370x208//bw/ec/21691/Honda-Exterior-65001.jpg" alt="Honda Brazil launches fuel injected CG125i motorcycle" src="">
                                    </div>
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <h3>
                                    <a href="/m/news/21691-honda-brazil-launches-fuel-injected-cg125i-motorcycle.html" class="target-link">Honda Brazil launches fuel injected...</a>
                                </h3>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Nov 15, 2016</span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Santosh Nair</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">Honda motorcycles Brazil has launched the CG125i for the Brazilian market at 6,790 Real (Rs 1.12 lakh). Honda’s Brazilian arm introduced the upgraded version in a bid...</p>
                        </li>
                        <li>
                            <div class="review-image-full-width-wrapper">
                                <a href="" title="Honda Brazil launches fuel injected CG125i motorcycle" class="block">
                                    <div class="bg-loader-placeholder">
                                        <img class="lazy" data-original="https://imgd2.aeplcdn.com//370x208//bw/ec/21691/Honda-Exterior-65001.jpg" alt="Honda Brazil launches fuel injected CG125i motorcycle" src="">
                                    </div>
                                </a>
                            </div>
                            <div class="review-heading-full-width-wrapper">
                                <h3>
                                    <a href="/m/news/21691-honda-brazil-launches-fuel-injected-cg125i-motorcycle.html" class="target-link">Honda Brazil launches fuel injected...</a>
                                </h3>
                                <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content">Nov 15, 2016</span>
                                </div>
                                <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content">Santosh Nair</span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10">Honda motorcycles Brazil has launched the CG125i for the Brazilian market at 6,790 Real (Rs 1.12 lakh). Honda’s Brazilian arm introduced the upgraded version in a bid...</p>
                        </li>
                    </ul>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Videos</h2>
                <div class="video-content">
                    <iframe src="https://www.youtube.com/embed/Aod9AcExx1Q" frameborder="0" width="320" height="180"></iframe>
                    <div class="bg-loader-placeholder"></div>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Gallery</h2>
                <div>
                    <div class="connected-carousels-photos">
                        <div class="stage-photos">
                            <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                                <div class="swiper-wrapper">
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//762x429//bw/ec/24236/Action-77224.jpg?v=20162707184052" alt="" />
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
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com//110x61//bw/ec/24236/Action-77224.jpg?v=20162707184052" alt="" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>                                        
                                </div>
                                <div class="bwmsprite swiper-button-next hide"></div>
                                <div class="bwmsprite swiper-button-prev hide"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Expert reviews</h2>
                <div class="container bg-white box-shadow padding-top15 padding-bottom15 font14">
                    <div class="swiper-container card-container">
                        <div class="swiper-wrapper">
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="expert-review-image-preview position-rel">
                                        <img class="swiper-lazy" alt="Vikrant Singh" data-src="https://imgd1.aeplcdn.com//640x348//bw/ec/20754/Honda-CB-Shine-SP-Exterior-60338.jpg?wm=0&t=123349767&t=123349767">
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="expert-review-details-block">
                                        <p class="text-bold text-black margin-bottom10">Vikrant Singh</p>
                                        <p class="text-light-grey">Two bikes. Two different doing a comparison test and also the same...</p>
                                    </div>
                                </div>
                            </div>

                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <div class="expert-review-image-preview position-rel">
                                        <img class="swiper-lazy" alt="Ranjan Bhat" data-src="https://imgd1.aeplcdn.com//640x348//bw/ec/20754/Honda-CB-Shine-SP-Exterior-60338.jpg?wm=0&t=123349767&t=123349767">
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="expert-review-details-block">
                                        <p class="text-bold text-black margin-bottom10">Ranjan Bhat</p>
                                        <p class="text-light-grey">Two bikes. Two different doing a comparison test and also the same...</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Making of Track Day</h2>
                <div class="container bg-white box-shadow">
                    <div class="padding-15-20 font14">
                        <p class="text-bold text-black margin-bottom10">Lorem imsup dolor sit amet</p>
                        <p class="text-light-grey">Lorem ipsum dolor sit amet, consectetuer adipis cing elit. Aenean commodo ligula eget dolor. Aenean massa.</p>
                    </div>
                    <ul class="track-day-collage-list">
                        <li>
                            <div class="bg-loader-placeholder">
                                <img class="lazy" data-original="https://imgd1.aeplcdn.com/600x337/bw/ec/20754/Honda-CB-Shine-SP-Exterior-60339.jpg?wm=0" src="" border="0" />
                            </div>
                        </li>
                        <li>
                            <div class="bg-loader-placeholder">
                                <img class="lazy" data-original="https://imgd1.aeplcdn.com/600x337/bw/ec/20754/Honda-CB-Shine-SP-Exterior-60339.jpg?wm=0" src="" border="0" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <% if(!androidApp) { %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <% } %>

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>
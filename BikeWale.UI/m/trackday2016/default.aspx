<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.TrackDay.Default" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        title = "BikeWale Track Day 2016";
        description = "BikeWale brings to you the celebration of motoring in its finest and purest form! The idea is to bring together bikes on a racetrack that have impressed us by their sheer dynamic ability and their prowess.";
        //keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
        //canonical   = "https://www.bikewale.com/news/";
        //AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        //AdId = "1398766302464";
        //Ad_320x50 = true;
        //Ad_Bot_320x50 = true;
    %>
   
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday2016/css/track-day.css?<%= staticFileVersion %>" />
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
            <div class="container text-center section-container">
                <div class="video-content">
                    <iframe width="320" height="180" src="https://www.youtube.com/embed/94hLbNXL3JE" frameborder="0" allowfullscreen></iframe>
                    <div class="bg-loader-placeholder"></div>
                </div>
            </div>
        </section>

        <%  if (objTrackDayArticles!=null && objTrackDayArticles.RecordCount > 0) { %>
               
        <section>
            <div class="container section-container">
                <h2 class="section-heading">Track Day updates</h2>
                <div class="content-box-shadow padding-right20 padding-left20 font14">
                    <ul class="article-list">
                        <% foreach (var article in objTrackDayArticles.Articles)
                        { %>
                        <li>
                            <div class="review-image-wrapper">
                                <a class="block" href="<%= string.Format("/m{0}{1}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString()),androidApp?"?isapp=true":string.Empty) %>" title="<%=article.Title %>">
                                    <div class="bg-loader-placeholder">
                                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._360x202)  %>" alt="<%= article.Title %>" src="">
                                    </div>
                                </a>
                            </div>
                            <div class="review-heading-wrapper">
                                <h3>
                                    <a href="<%= string.Format("/m{0}{1}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString()),androidApp?"?isapp=1":string.Empty) %>" title="<%= article.Title %>" class="target-link"><%= article.Title %></a>
                                </h3>
                                <div class="grid-7 alpha padding-right5">
                                    <span class="bwmsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content"> <%= Bikewale.Utility.FormatDate.GetFormatDate(article.DisplayDate.ToString(),"MMM dd, yyyy") %></span>
                                </div>
                                <div class="grid-5 alpha omega">
                                    <span class="bwmsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content"> <%= article.AuthorName %></span>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <p class="margin-top10"><%= article.Description %></p>
                        </li>
                        <%} %>
                    </ul>
                </div>
            </div>
        </section>
        <% } %>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Gallery</h2>
                <div class="content-box-shadow padding-bottom1">
                    <div class="connected-carousels-photos">
                        <div class="stage-photos">
                            <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                                <div class="swiper-wrapper">
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/01.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd2.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/02.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/03.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/04.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd2.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/05.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/06.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/07.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd2.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/08.jpg?wm=2" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/762x429/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/09.jpg?wm=2" alt="Track Day Gallery" />
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
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/01.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd2.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/02.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/03.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/04.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd2.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/05.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/06.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd1.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/07.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd2.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/08.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="https://imgd3.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/09.jpg" alt="Track Day Gallery" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                </div>
                            </div>
                        </div>                        
                    </div>
                </div>
            </div>
        </section>

        <div id="gallery-close-btn" class="bwmsprite cross-lg-white"></div>
        <div id="gallery-blackOut-window"></div>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Buzz on social media</h2>
                <div class="container bg-white box-shadow">
                    <a class="twitter-timeline"  href="https://twitter.com/hashtag/bikewaletrackday" data-widget-id="808683033557549056" data-width="100%" data-height="440">#bikewaletrackday Tweets</a>
                    <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + "://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Making of Track Day</h2>
                <div class="container bg-white box-shadow track-day-collage-content">
                    <div class="bg-loader-placeholder">
                        <img class="lazy" data-original="https://imgd1.aeplcdn.com/0x0/cw/es/trackday/2016/bwm-trackday/landing-images/making-of-the-event.jpg" alt="Making of Track Day" src="" border="0" />
                    </div>
                </div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <% if(!androidApp) { %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <% } %>

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday2016/src/track-day.js?<%= staticFileVersion %>"></script>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>
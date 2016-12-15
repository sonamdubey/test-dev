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
                <h2 class="section-heading">Track day updates</h2>
                <div class="content-box-shadow padding-right20 padding-left20 font14">
                    <ul class="article-list">
                        <% foreach (var article in objTrackDayArticles.Articles)
                        { %>
                        <li>
                            <div class="review-image-wrapper">
                                <a class="block" href="<%= string.Format("/m{0}{1}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString()),androidApp?"?isapp=true":string.Empty) %>" title="<%=article.Title %>">
                                    <div class="bg-loader-placeholder">
                                        <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._110x61)  %>" alt="<%= article.Title %>" src="">
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
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/01.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/02.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/03.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/04.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/05.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/06.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/07.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/08.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/09.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/10.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/11.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/12.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/13.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/14.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/15.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/16.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/17.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/18.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/19.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/20.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/768x432_l/21.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>

                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/01.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/02.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/03.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/04.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/05.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/06.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/07.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/08.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/09.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/10.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/11.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/12.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/13.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/14.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/15.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/16.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/17.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/18.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/19.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/20.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/21.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/768x432_l/22.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>

                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/01.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/02.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/03.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/04.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/05.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/06.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/07.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/08.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/09.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/10.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/11.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/12.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/13.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/14.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/15.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/16.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/17.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/18.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/19.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/768x432_l/20.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>

                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/02.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/03.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/04.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/05.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/06.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/07.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/08.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/09.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/10.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/11.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/12.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/13.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/14.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/15.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/16.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/768x432_l/17.jpg" alt="Ducati 959 Panigale" />
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
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/01.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/02.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/03.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/04.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/05.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/06.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/07.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/08.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/09.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/10.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/11.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/12.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/13.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/14.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/15.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/16.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/17.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/18.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/19.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/20.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/tvs-apache-images/gallery/170x100_t/21.jpg" alt="TVS Apache RTR 200 4V" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>

                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/01.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/02.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/03.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/04.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/05.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/06.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/07.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/08.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/09.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/10.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/11.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/12.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/13.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/14.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/15.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/16.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/17.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/18.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/19.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/20.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/21.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/yamaha-yzf-images/gallery/170x100_t/22.jpg" alt="Yamaha YZF-R3" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>

                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/01.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/02.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/03.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/04.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/05.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/06.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/07.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/08.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/09.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/10.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/11.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/12.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/13.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/14.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/15.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/16.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/17.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/18.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/19.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/benelli-tnt-images/gallery/170x100_t/20.jpg" alt="Benelli TNT 600i ABS" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>

                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/02.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/03.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/04.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/05.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/06.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/07.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/08.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/09.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/10.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/11.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/12.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/13.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/14.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/15.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/16.jpg" alt="Ducati 959 Panigale" />
                                        <span class="swiper-lazy-preloader"></span>
                                    </div>
                                    <div class="swiper-slide">
                                        <img class="swiper-lazy" data-src="/m/images/trackday/ducati-959-images/gallery/170x100_t/17.jpg" alt="Ducati 959 Panigale" />
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
                <div class="container bg-white box-shadow">
                    <div class="padding-15-20 font14">
                        <p class="text-bold text-black margin-bottom10">Lorem imsup dolor sit amet</p>
                        <p class="text-light-grey">Lorem ipsum dolor sit amet, consectetuer adipis cing elit. Aenean commodo ligula eget dolor. Aenean massa.</p>
                    </div>
                    <ul class="track-day-collage-list">
                        <li>
                            <div class="bg-loader-placeholder">
                                <img class="lazy" data-original="/m/images/trackday/landing-images/making-of-the-event.jpg" alt="Making of Track Day" src="" border="0" />
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

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday2016/src/track-day.js?<%= staticFileVersion %>"></script>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>
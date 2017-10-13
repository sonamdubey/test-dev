<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.TrackDay.Default" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = "BikeWale Track Day 2016";
        description = "BikeWale brings to you the celebration of motoring in its finest and purest form! The idea is to bring together bikes on a racetrack that have impressed us by their sheer dynamic ability and their prowess.";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        alternate = String.Format("{0}/m/trackday2016/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl);
        canonical = String.Format("{0}/trackday2016/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl);
       
    %>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
     <link rel="stylesheet" type="text/css" href="<%= staticUrl  %>/trackday2016/css/track-day.css?<%= staticFileVersion %>" />
  
      <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header>
            <div class="trackday-banner">
                <div class="container">
                    <div class="welcome-box">
                        <h1 class="font30 text-uppercase margin-bottom30">Track Day 2016</h1>
                    </div>
                </div>
            </div>
        </header>
       
        <%  if (objTrackDayArticles!=null && objTrackDayArticles.RecordCount > 0) { %>
        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="margin-top25 section-header">Track Day updates</h2>
                    <div class="content-box-shadow padding-right20 padding-left20 padding-bottom5">
                        <ul class="article-list">
                        <% foreach (var article in objTrackDayArticles.Articles)
                        { %>
                            <li>
                                <div class="review-image-wrapper">
                                    <a href="<%= string.Format("{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString())) %>" title="<%=article.Title %>" data-cat="bw_trackday" data-act="<%=article.Title %>-article-<%= article.BasicId.ToString() %>-image" data-lab="bw_trackday" class="bw-ga block">
                                        <div class="bg-loader-placeholder">
                                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._642x361)  %>" alt="<%= article.Title %>" src="">
                                        </div>
                                    </a>
                                </div>
                                <div class="review-heading-wrapper">
                                    <h3>
                                        <a href="<%= string.Format("{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString())) %>" class="article-target-link bw-ga" data-cat="bw_trackday" data-act="<%=article.Title %>-article-<%= article.BasicId.ToString() %>-title" data-lab="bw_trackday"><%= article.Title %></a>
                                    </h3>
                                    <div class="article-stats-left-grid">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content text-truncate"><%= Bikewale.Utility.FormatDate.GetFormatDate(article.DisplayDate.ToString(),"dd MMMM yyyy") %></span>
                                    </div>
                                    <div class="article-stats-right-grid" title="<%= article.AuthorName %>">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content text-truncate"><%= article.AuthorName %></span>
                                    </div>
                                </div>
                                <div class="article-desc-wrapper margin-top15">
                                    <p class="font14 text-light-grey"><%= Bikewale.Utility.FormatDescription.TruncateDescription(article.Description,200) %></p>
                                
                                    <a href="<%= string.Format("{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString())) %>" data-cat="bw_trackday" data-act="<%=article.Title %>-article-<%= article.BasicId.ToString() %>-link" data-lab="bw_trackday" class="font14 bw-ga">Read full story</a>
                                </div>
                            </li>
                        <%} %>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Video</h2>
                    <div class="content-box-shadow">
                        <div id="track-day-video" class="video-content">
                            <iframe id="td-iframe-video" width="976" height="549" src="https://www.youtube.com/embed/XuEp3JO6zCw" frameborder="0" allowfullscreen></iframe>
                            <div class="bg-loader-placeholder"></div>
                            <div data-cat="bw_trackday" data-act="Track day video" data-lab="bw_trackday" class="video-overlay cur-pointer bw-ga"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div id="trackdayGallery" class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Gallery</h2>
                    <div class="content-box-shadow padding-bottom1">
                        <div class="connected-carousels-photos">
                            <div class="stage-photos stage-media">
                                <div class="carousel-photos carousel-stage-photos carousel-stage-media">
                                    <ul>                            
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/01.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/02.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/03.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/04.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/05.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/06.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/07.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/08.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-img-container">
                                                <span>
                                                    <img class="lazy" data-original="https://imgd.aeplcdn.com/1280x720/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/09.jpg?wm=2"  src="" alt="Track Day Gallery">
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <a href="#" class="prev photos-prev-stage bwsprite media-prev-next-stage" rel="nofollow"></a>
                                <a href="#" class="next photos-next-stage bwsprite media-prev-next-stage" rel="nofollow"></a>
                            </div>

                            <div class="navigation-photos navigation-media">
                                <a href="#" class="prev photos-prev-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                                <a href="#" class="next photos-next-navigation bwsprite media-prev-next-nav" rel="nofollow"></a>
                                <div class="carousel-photos carousel-navigation-photos carousel-navigation-media">
                                    <ul>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/01.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/02.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/03.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/04.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/05.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/06.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/07.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/08.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="https://imgd.aeplcdn.com/110x61/cw/es/trackday/2016/bwm-trackday/landing-images/gallery/09.jpg" src="" alt="Track Day Gallery"/>
                                            </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <div id="gallery-close-btn" class="bwsprite cross-lg-white cur-pointer"></div>
        <div id="gallery-blackOut-window"></div>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Buzz on social media</h2>
                    <div class="content-box-shadow">
                        <a class="twitter-timeline"  href="https://twitter.com/hashtag/bikewaletrackday" data-widget-id="808683033557549056" data-width="100%" data-height="440">#bikewaletrackday Tweets</a>
                        <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + "://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Making of Track Day</h2>
                    <div class="content-box-shadow track-day-collage-content">
                        <div class="bg-loader-placeholder">
                            <img class="lazy" data-original="https://imgd.aeplcdn.com/0x0/cw/es/trackday/2016/bw-trackday/landing-images/making-of-the-event.jpg" alt="Making of Track Day" src="" border="0" />
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl  %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl  %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/trackday2016/src/track-day.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/fontBW.aspx" -->

        <script type="text/javascript">
            $(document).ready(function () {
                if ($(window).scrollTop() > 40) {
                    $('#header').removeClass("header-landing").addClass("header-fixed");
                }
            });

            $(window).on("scroll", function () {
                if ($(window).scrollTop() > 40)
                    $('#header').removeClass("header-landing").addClass("header-fixed");
                else
                    $('#header').removeClass("header-fixed").addClass("header-landing");
            });
        </script>

    </form>
</body>
</html>

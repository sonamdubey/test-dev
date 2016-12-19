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
    %>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/trackday2016/css/track-day.css?<%= staticFileVersion %>" />

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
                                    <a href="<%= string.Format("{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString())) %>" title="<%=article.Title %>" class="block">
                                        <div class="bg-loader-placeholder">
                                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._642x361)  %>" data-label="<%= article.Title %>-image" alt="<%= article.Title %>" src="">
                                        </div>
                                    </a>
                                </div>
                                <div class="review-heading-wrapper">
                                    <h3>
                                        <a href="<%= string.Format("{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString())) %>" data-label="<%= article.Title %>-title" class="article-target-link"><%= article.Title %></a>
                                    </h3>
                                    <div class="article-stats-left-grid">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content text-truncate"><%= Bikewale.Utility.FormatDate.GetFormatDate(article.DisplayDate.ToString(),"MMM dd, yyyy") %></span>
                                    </div>
                                    <div class="article-stats-right-grid" title="<%= article.AuthorName %>">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content text-truncate"><%= article.AuthorName %></span>
                                    </div>
                                </div>
                                <div class="article-desc-wrapper margin-top15">
                                    <p class="font14 text-light-grey"><%= Bikewale.Utility.FormatDescription.TruncateDescription(article.Description,200) %></p>
                                
                                    <a href="<%= string.Format("{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString())) %>" class="font14">Read full story</a>
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
                        <div class="video-content">
                            <iframe width="976" height="549" src="https://www.youtube.com/embed/94hLbNXL3JE" frameborder="0" allowfullscreen></iframe>
                            <div class="bg-loader-placeholder"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <%--<section>
            <div class="connected-carousels-photos">
                <div class="stage-photos stage-media">
                    <div class="carousel-photos carousel-stage-photos carousel-stage-media">
                        <ul>                            
                            <li>
                                <div class="gallery-photo-img-container">
                                    <span>
                                        <img class="lazy" data-original="https://imgd7.aeplcdn.com//762x429//bw/ec/26202/Action-85705.jpg?wm=2"  src="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  title="">
                                    </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-img-container">
                                    <span>
                                        <img class="lazy" data-original="https://imgd6.aeplcdn.com//762x429//bw/ec/26202/Action-85706.jpg?wm=2"  src="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  title="">
                                    </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-img-container">
                                    <span>
                                        <img class="lazy" data-original="https://imgd8.aeplcdn.com//762x429//bw/ec/26202/Front-threequarter-85707.jpg?wm=2"  src="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  title="">
                                    </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-img-container">
                                    <span>
                                        <img class="lazy" data-original="https://imgd8.aeplcdn.com//762x429//bw/ec/26202/Exterior-85390.jpg?wm=2"  src="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  title="">
                                    </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-img-container">
                                    <span>
                                        <img class="lazy" data-original="https://imgd8.aeplcdn.com//762x429//bw/ec/26202/Exhaust-85709.jpg?wm=2"  src="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif"  title="">
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
                                    <img class="lazy" data-original="https://imgd7.aeplcdn.com//110x61//bw/ec/26202/Exterior-85327.jpg?wm=2" src="" title=""/>
                                </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-nav-img-container">
                                <span>
                                    <img class="lazy" data-original="https://imgd7.aeplcdn.com//110x61//bw/ec/26202/Action-85706.jpg?wm=2" src="" title=""/>
                                </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-nav-img-container">
                                <span>
                                    <img class="lazy" data-original="https://imgd7.aeplcdn.com//110x61//bw/ec/26202/Exterior-85327.jpg?wm=2" src="" title=""/>
                                </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-nav-img-container">
                                <span>
                                    <img class="lazy" data-original="https://imgd7.aeplcdn.com//110x61//bw/ec/26202/Exterior-85327.jpg?wm=2" src="" title=""/>
                                </span>
                                </div>
                            </li>
                            <li>
                                <div class="gallery-photo-nav-img-container">
                                <span>
                                    <img class="lazy" data-original="https://imgd7.aeplcdn.com//110x61//bw/ec/26202/Exterior-85327.jpg?wm=2" src="" title=""/>
                                </span>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </section>--%>

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
                    <h2 class="section-header">Making of the event</h2>
                    <div class="content-box-shadow">

                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/trackday2016/src/track-day.js?<%= staticFileVersion %>"></script>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

        <script type="text/javascript">
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

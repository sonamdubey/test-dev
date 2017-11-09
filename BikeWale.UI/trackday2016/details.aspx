<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.TrackDay.Details" %>

<!DOCTYPE html>
<html>
<head>
    <%        
        if (objTrackDay != null)
        {
            title = objTrackDay.Title;
            description = objTrackDay.Description;
        }

        isHeaderFix = true;
        isTransparentHeader = false;
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        
		//AdId="1395995626568";
		//AdPath="/1017752/BikeWale_News_";
        
	%>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="<%= staticUrl  %>/trackday2016/css/track-day.css?<%= staticFileVersion %>" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="container padding-top10">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
						<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
							<span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/trackday2016/" itemprop="url">
                                <span itemprop="title">Track Day 2016</span>
                            </a>
                        </li>
                        <% if (objTrackDay != null)
                           { %>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span><%= objTrackDay.Title  %></li>
                         <% } %>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <% if (objTrackDay != null)
               { %>
            <div id="track-day-article" class="container section-container">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="padding-top20">
                    <div class="text-center">
                        <div class="trackday-logo"></div>
                    </div>

                    <div class="padding-right20 padding-left20">
                        <% if (objTrackDay.BasicId != 26199)
                           {%>
                        <div class="bg-loader-placeholder">
                            <img class="article-image lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objTrackDay.OriginalImgUrl, objTrackDay.HostUrl, Bikewale.Utility.ImageSize._958x539) %>" alt="<%= objTrackDay.Title %>" src="" border="0" />
                        </div>
                        <%} %>
                        <div class="text-center margin-bottom40">
                            <h1 class="article-heading text-unbold"><%= objTrackDay.Title %></h1>
                            <p class="article-author margin-bottom10"><i><%= objTrackDay.AuthorName %></i></p>
                            <p class="font12 text-light-grey"><%= Bikewale.Utility.FormatDate.GetFormatDate(objTrackDay.DisplayDate.ToString(),"dd MMMM yyyy") %></p>
                        </div>

                        <%= String.IsNullOrEmpty(objTrackDay.Content) ? "" : objTrackDay.Content %>
                    </div>

                    </div>

                        <% if (objImages != null)
                           { %>
                        <p class="margin-left20 photography-heading">Images</p>
                        <div class="connected-carousels-photos">
                        <div class="stage-photos stage-media">
                            <div class="carousel-photos carousel-stage-photos carousel-stage-media">
                                <ul>
                                    <% foreach (var image in objImages)
                                       { %>
                                    <li>
                                        <div class="gallery-photo-img-container">
                                            <span>
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(image.OriginalImgPath,image.HostUrl,Bikewale.Utility.ImageSize._1280x720)%>" alt="<%= image.ImageName %>" src="" />
                                            </span>
                                        </div>
                                    </li>
                                    <% } %>
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
                                    <% foreach (var image in objImages)
                                       { %>
                                    <li>
                                        <div class="gallery-photo-nav-img-container">
                                            <span>
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(image.OriginalImgPath,image.HostUrl,Bikewale.Utility.ImageSize._110x61)%>" alt="<%= image.ImageName %>" src="" />
                                            </span>
                                        </div>
                                    </li>
                                    <% } %>
                                </ul>
                            </div>
                        </div>
                    </div>
                        <% } %>

                        <% if (objTrackDayArticles != null && objTrackDayArticles.RecordCount > 0)
                           { %>
                            <div class="padding-right20 padding-left20">
                                <p class="related-articles-heading">More BikeWale Track Day Articles</p>
                                <ul class="related-articles-list">
                                    <% foreach (var article in objTrackDayArticles.Articles)
                                       { %>
                                    <li>
                                        <a href="<%= string.Format("{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString())) %>" data-cat="bw_trackday" data-act="<%= article.Title %>-related-article" data-lab="bw_trackday" title="<%= article.Title %>" class="bw-ga"><%= article.Title %></a>
                                    </li>
                                    <% } %>
                                </ul>
                            </div>
                        <% } %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <% } %>
        </section>

        <div id="gallery-close-btn" class="bwsprite cross-lg-white cur-pointer"></div>
        <div id="gallery-blackOut-window"></div>

        <script type="text/javascript" src="<%= staticUrl %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl %>/trackday2016/src/track-day.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/fontBW.aspx" -->

        <script type="text/javascript">
            var articleImages = $('.article-image-margin img');

            // call higher dimension image for desktop
            articleImages.each(function () {
                var currentImage = $(this),
                    imageUrl = currentImage.attr('src'),
                    newImageUrl = imageUrl.replace('600x337', '958x539');

                currentImage.attr('src', newImageUrl);
            });
        </script>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.TrackDay.Details" %>

<!DOCTYPE html>
<html>
<head>

    <% 
        if (objTrackDay != null)
        {
            title = objTrackDay.Title;
            description = objTrackDay.Description;
        }
        Ad_320x50 = false;
        Ad_Bot_320x50 = false;
        Ad_300x250 = false;
        
    %>

    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl  %>/m/trackday2016/css/track-day.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <% if (!androidApp)
           { %>
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <% } %>

        <section>
            <% if (objTrackDay != null)
               { %>
            <div id="track-day-article" class="container box-shadow bg-white section-container padding-bottom1">
                <div class="padding-top20">
                    <div class="text-center">
                        <div class="trackday-logo"></div>
                    </div>

                    <div class="padding-right20 padding-left20">
                        <% if (objTrackDay.BasicId != 26199) 
                           {%>
                        <div class="bg-loader-placeholder">
                            <img class="article-image lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objTrackDay.OriginalImgUrl, objTrackDay.HostUrl, Bikewale.Utility.ImageSize._762x429) %>" alt="<%= objTrackDay.Title %>" src="" border="0" />
                        </div>
                        <%} %>
                        <div class="text-center margin-bottom30">
                            <h1 class="article-heading text-unbold"><%= objTrackDay.Title %></h1>
                            <p class="article-author margin-bottom5"><i><%= objTrackDay.AuthorName %></i></p>
                            <p class="font12 text-light-grey"><%= Bikewale.Utility.FormatDate.GetFormatDate(objTrackDay.DisplayDate.ToString(),"dd MMMM yyyy") %></p>
                        </div>

                        <%= String.IsNullOrEmpty(objTrackDay.Content) ? "" : objTrackDay.Content %>
                    </div>

                </div>

                <% if (objImages != null)
                   { %>
                <p class="margin-left20 photography-heading">Images</p>
                <div class="connected-carousels-photos">
                    <div class="stage-photos">
                        <div class="swiper-container noSwiper carousel-photos carousel-stage-photos">
                            <div class="swiper-wrapper">
                                <% foreach (var image in objImages)
                                   { %>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(image.OriginalImgPath,image.HostUrl,Bikewale.Utility.ImageSize._762x429)  %>" alt="<%= image.ImageName %>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <% } %>
                            </div>
                            <div class="bwmsprite swiper-button-next"></div>
                            <div class="bwmsprite swiper-button-prev"></div>
                        </div>
                    </div>

                    <div class="navigation-photos">
                        <div class="swiper-container noSwiper carousel-navigation-photos">
                            <div class="swiper-wrapper">
                                <% foreach (var image in objImages)
                                   { %>
                                <div class="swiper-slide">
                                    <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(image.OriginalImgPath,image.HostUrl,Bikewale.Utility.ImageSize._110x61)  %>" alt="<%= image.ImageName %>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </div>
                                <% } %>
                            </div>
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
                                <a href="<%= string.Format("/m{0}{1}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString()),androidApp?"?isapp=true":string.Empty) %>" title="<%= article.Title %>"><%= article.Title %></a>
                            </li>
                            <% } %>
                        </ul>
                    </div>
                <% } %>
            </div>
            <% } %>
        </section>

        <div id="gallery-close-btn" class="bwmsprite cross-lg-white"></div>
        <div id="gallery-blackOut-window"></div>

        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <% if (!androidApp)
           { %>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <% } %>

        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl %>/m/trackday2016/src/track-day.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->

    </form>
</body>
</html>

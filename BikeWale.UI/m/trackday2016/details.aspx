<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.TrackDay.Details" %>

<!DOCTYPE html>
<html>
<head>

    <% 
        if(objTrackDay!=null)
        {
            title = objTrackDay.Title;
            description = objTrackDay.Description;  
        }
         
        //keywords    = "news, bike news, auto news, latest bike news, indian bike news, bike news of india"; 
        //canonical   = "https://www.bikewale.com/news/";
        //AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        //AdId = "1398766302464";
        Ad_320x50 = false;
        Ad_Bot_320x50 = false;
        Ad_300x250 = false;
        
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday/css/track-day.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <% if (!androidApp)
           { %>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% } %>

        <section>
            <% if (objTrackDay != null)
               { %>
            <div id="track-day-article" class="container box-shadow bg-white section-container padding-bottom1">
                <div class="padding-top20">
                    <div class="text-center">
                        <div class="trackday-logo"></div>
                    </div>
                    <%-- <% switch (articleid)
                       {
                        case 1: %>
                            <!-- #include file="/m/trackday/articles/Introduction.aspx" -->
                            <% break;
                        case 2: 
                        %>
                            <!-- #include file="/m/trackday/articles/TVSApache.aspx" -->
                            <% break;
                        case 3: 
                        %>
                            <!-- #include file="/m/trackday/articles/YamahaYZF.aspx" -->
                            <% break;
                        case 4: 
                        %>
                            <!-- #include file="/m/trackday/articles/BenelliTNT.aspx" -->
                            <% break;
                        case 5: 
                        %>
                            <!-- #include file="/m/trackday/articles/DucatiPanigale.aspx" -->
                            <% break;
                        case 6: 
                        %>
                            <!-- #include file="/m/trackday/articles/TopBikes.aspx" -->
                            <% break;
                        default: 
                        %>
                            <!-- #include file="/m/trackday/articles/Introduction.aspx" -->
                            <% break;
                       } %>--%>

                    <div class="padding-right20 padding-left20">
                        <div class="bg-loader-placeholder">
                            <img class="article-image lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(objTrackDay.OriginalImgUrl, objTrackDay.HostUrl, Bikewale.Utility.ImageSize._640x348) %>" alt="<%= objTrackDay.Title %>" src="" border="0" />
                        </div>

                        <div class="text-center margin-bottom30">
                            <h1 class="article-heading text-unbold"><%= objTrackDay.Title %></h1>
                            <p class="article-author margin-bottom5"><i><%= objTrackDay.AuthorName %></i></p>
                            <p class="font12 text-light-grey"><%= Bikewale.Utility.FormatDate.GetFormatDate(objTrackDay.DisplayDate.ToString(),"MMM dd, yyyy") %></p>
                        </div>

                         <%= String.IsNullOrEmpty(objTrackDay.Content) ? "" : objTrackDay.Content %>

                        <% if(objTrackDayArticles!=null && objTrackDayArticles.RecordCount > 0) { %>

                         <p class="related-articles-heading">More BikeWale Track Day Articles</p>
                            <ul class="related-articles-list">
                                <% foreach(var article in objTrackDayArticles.Articles) { %>
                            <li>
                                <a href="<%= string.Format("/m{0}{1}", Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(),article.ArticleUrl,article.CategoryId.ToString()),androidApp?"?isapp=1":string.Empty) %>" title="<%= article.Title %>"><%= article.Title %></a>
                            </li>
                                <% } %>
                        </ul>

                        <% } %>
                        
                    </div>

                </div>
            </div>
            <% } %>
        </section>

        <div id="gallery-close-btn" class="bwmsprite cross-lg-white"></div>
        <div id="gallery-blackOut-window"></div>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <% if (!androidApp)
           { %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <% } %>

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/trackday/src/track-day.js?<%= staticFileVersion %>"></script>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>

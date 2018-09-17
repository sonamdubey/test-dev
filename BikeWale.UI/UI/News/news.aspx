<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.news" %>

<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/UI/controls/MostPopularBikesMin.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/UI/controls/UpcomingBikesMinNew.ascx" %>
<%@ Register TagPrefix="BW" TagName="MinGenericBikeInfo" Src="~/UI/controls/MinGenericBikeInfoControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="PopularBikesByBodyStyle" Src="~/UI/controls/PopularBikesByBodyStyle.ascx" %>

<!Doctype html>
<html>
<head>
    <%
        if (metas != null)
        {
            title = metas.Title;
            description = metas.Description;
            canonical = metas.CanonicalUrl;
            fbTitle = metas.Title;
            fbImage = metas.ShareImage;
            alternate = metas.AlternateUrl;
        }
        AdId = "1395995626568";
        AdPath = "/1017752/BikeWale_News_";
        isAd300x250Shown = true;
        isAd300x250BTFShown = false;

    %>

    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->
    <% if(metas!= null){ %>
    <link rel="amphtml" href="<%= metas.AmpUrl %>" />
    <% } %>
    <link href="<%= staticUrl  %>/UI/css/content/details.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->

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
                            <a href="/news/" itemprop="url">
                                <span itemprop="title">Bike News</span>
                            </a>
                        </li>
                        <% if (objArticle != null)
                           { %>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span><%= objArticle.Title  %></li>
                        <% } %>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div id="content" class="grid-8 alpha">
                        <div class="bg-white">
                            <% if (objArticle != null)
                               { %>
                            <div class="section-header">
                                <h1 class="margin-bottom5"><%= objArticle.Title %></h1>
                                <div>
                                    <span class="bwsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(objArticle.DisplayDate), "dd MMMM yyyy, hh:mm tt") %></span>
                                    <span class="bwsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content"><%= objArticle.AuthorName %></span>
                                </div>
                            </div>
                            <div class="section-inner-padding">
                                <div id="post-<%= objArticle.BasicId %>">
                                    <div class="article-content padding-top5">
                                        <%if (!String.IsNullOrEmpty(metas.ShareImage)) %>
                                        <div style="text-align: center;">
                                            <img alt='<%= objArticle.Title %>' title='<%= objArticle.Title %>' src='<%= metas.ShareImage %>'></div>
                                        <%= objArticle.Content %>
                                        <div class="clear"></div>
                                    </div>
                                </div>

                                <BW:MinGenericBikeInfo ID="ctrlMinGenericBikeInfo" runat="server" />

                                <div class="border-solid-top padding-top10">
                                    <% if (!String.IsNullOrEmpty(objArticle.PrevArticle.ArticleUrl))
                                       { %>
                                    <div class="grid-6 alpha border-solid-right">
                                        <a href="<%= "/news/" + objArticle.PrevArticle.BasicId + "-" + objArticle.PrevArticle.ArticleUrl + ".html"%>" title="<%=objArticle.PrevArticle.Title %>" class="text-default next-prev-article-target">
                                            <span class="bwsprite prev-arrow"></span>
                                            <div class="next-prev-article-box inline-block padding-left5">
                                                <span class="font12 text-light">Previous</span><br />
                                                <span class="next-prev-article-title text-truncate"><%=objArticle.PrevArticle.Title %></span>
                                            </div>
                                        </a>
                                    </div>
                                    <%} %>
                                    <% if (!String.IsNullOrEmpty(objArticle.NextArticle.ArticleUrl))
                                       { %>
                                    <div class="grid-6 omega rightfloat">
                                        <a href="<%=  "/news/" + objArticle.NextArticle.BasicId + "-" + objArticle.NextArticle.ArticleUrl + ".html"%>" title="<%=objArticle.NextArticle.Title %>" class="text-default next-prev-article-target">
                                            <div class="next-prev-article-box inline-block padding-right5">
                                                <span class="font12 text-light">Next</span>
                                                <span class="next-prev-article-title text-truncate"><%=objArticle.NextArticle.Title %></span>
                                            </div>
                                            <span class="bwsprite next-arrow"></span>
                                        </a>
                                    </div>
                                    <% } %>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <% } %>
                        </div>
                    </div>

                    <script type="text/javascript" src="<%= staticUrl  %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>

                    <div class="grid-4 omega">
                        <BW:MostPopularBikesMin runat="server" ID="ctrlPopularBikes" />
                       <div class="margin-bottom20">
                            <!-- #include file="/UI/ads/ad300x250.aspx" -->
                        </div>
                        <%if(isModelTagged){ %>
                        <%if (ctrlBikesByBodyStyle.FetchedRecordsCount > 0){ %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                       <h2>Popular <%=ctrlBikesByBodyStyle.BodyStyleText%></h2>
                        <BW:PopularBikesByBodyStyle ID="ctrlBikesByBodyStyle" runat="server"/>
                        </div>
                        <%} %>
                            <%} else{%>
                        <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />
                        <%} %>
                       
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>



        <!-- #include file="/UI/includes/footerBW.aspx" -->

        <link href="<%= staticUrl  %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%=  staticUrl  %>/UI/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $("body").floatingSocialShare();
            });

        </script>
        <!-- #include file="/UI/includes/fontBW.aspx" -->
    </form>
</body>
</html>

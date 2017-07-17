<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.Default" EnableViewState="false" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/m/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMinNew.ascx" %>
<%@ Register TagPrefix="BW" TagName="PopularBikesByBodyStyle" Src="~/controls/PopularBikesByBodyStyle.ascx" %>
<!Doctype html>
<html>
<head>
    <%
        title = objNews.PageTitle;
        description = objNews.Description;
        keywords = objNews.Keywords;
        canonical = objNews.Canonical;
        relPrevPageUrl = objNews.prevUrl;
        relNextPageUrl = objNews.nextUrl;
        alternate = objNews.Alternate;
        AdId = "1395995626568";
        AdPath = "/1017752/BikeWale_News_";
        isAd300x250Shown=true;
        isAd300x250BTFShown = false;

    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl  %>/css/content/listing.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
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
                        <% if(objNews.MakeId > 0){ %>
                         <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                             <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="<%= String.Format("/{0}-bikes/",objNews.objMake.MaskingName) %>" itemprop="url">
                                <span itemprop="title"><%= objNews.objMake.MakeName %> Bikes</span>
                            </a>
                        </li>
                        <%} %>
                        <% if(objNews.ModelId > 0){ %>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="<%= String.Format("/{0}-bikes/{1}/",objNews.objMake.MaskingName,objNews.objModel.MaskingName) %>" itemprop="url">
                                <span itemprop="title"><%= String.Format("{0} {1}", objNews.objMake.MakeName, objNews.objModel.ModelName) %> Bikes</span>
                            </a>
                        </li>
                        <%} %>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span>News</li>
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
                            <div id="news-header" class="section-header padding-15-20">
                                <h1 class="margin-right5"><%= objNews.PageH1 %></h1>
                                <h2 class="font14 text-unbold text-light-grey"><%= objNews.PageH2 %></h2>
                            </div>
                             <%if(newsArticles!=null) { %>
                            <div class="section-inner-padding">
                                <ul class="article-list">
                                <% foreach (var article in newsArticles) {
                                       string articleUrl = Bikewale.Utility.UrlFormatter.GetArticleUrl(article.BasicId.ToString(), article.ArticleUrl, article.CategoryId.ToString());
                                       %>
                                        <li id='post-<%= article.BasicId %>' class="<%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "sponsored-content" : "post-content" %> article-content">
                                            <div class="article-item-content">
                                                <%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : string.Empty %>

                                                <div class="article-image-wrapper">
                                                    <a href='<%= articleUrl %>' title='<%= article.Title %>'>
                                                        <img class="lazy" src="" data-original='<%= Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._210x118) %>' alt='<%= article.Title %>' title='<%= article.Title %>' width='100%' border='0' />
                                                    </a>
                                                </div>
                                                <div class="article-desc-wrapper">
                                                    <div class="article-category">
                                                        <span class="text-uppercase font12 text-bold"><%= GetContentCategory(article.CategoryId.ToString()) %></span>
                                                    </div>
                                                    <h3 class="font14 margin-bottom10">
                                                        <a href="<%= articleUrl %>" title='<%= article.Title %>' rel="bookmark" class="text-black text-bold"><%= article.Title %></a>
                                                    </h3>
                                                    <div class="font12 text-light-grey margin-bottom20">
                                                        <div class="article-date">
                                                            <span class="bwsprite calender-grey-icon inline-block"></span>
                                                            <span class="inline-block">
                                                                <%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(article.DisplayDate),"dd MMMM yyyy") %>
                                                            </span>
                                                        </div>
                                                        <div class="article-author">
                                                            <span class="bwsprite author-grey-icon inline-block"></span>
                                                            <span class="inline-block">
                                                                <%= article.AuthorName %>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="font14 line-height"><%= article.Description %><a href="<%= articleUrl %>">Read full story</a></div>
                                                </div>
                                                <div class="clear"></div>
                                             </div>
                                        </li>
                                <% } %>
                                </ul>
                                <div id="footer-pagination" class="font14 pagination-padding">
                                    <div class="grid-5 alpha omega text-light-grey">
                                        <p>Showing <span class="text-default text-bold"><%= startIndex %>-<%= endIndex %></span> of <span class="text-default text-bold"><%= totalArticles %></span> articles</p>
                                    </div>
                                    <BikeWale:RepeaterPager ID="ctrlPager" runat="server" />
                                     <div class="clear"></div>
                                </div>
                            </div>
                            <% }  %>
                        </div>
                    </div>

                    <script type="text/javascript" src="<%= staticUrl  %>/src/frameworks.js?<%=staticFileVersion %>"></script>

                    <div class="grid-4 omega">
                        <BW:MostPopularBikesMin runat="server" ID="ctrlPopularBikes" />
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
                          <div class="margin-bottom20">
                            <!-- #include file="/ads/Ad300x250.aspx" -->
                        </div>

                        <a href="/pricequote/" id="on-road-price-widget" class="content-box-shadow content-inner-block-20">
                            <span class="inline-block">
                                <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/on-road-price.png" alt="Rupee" border="0" />
                            </span><h2 class="text-default inline-block">Get accurate on-road price for bikes</h2>
                            <span class="bwsprite right-arrow"></span>
                        </a>
                    </div>
                    <div class="clear"></div>

                </div>
                <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl  %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>

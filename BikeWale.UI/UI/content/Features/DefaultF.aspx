<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultF"  %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/UI/m/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/UI/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/UI/controls/UpcomingBikesMinNew.ascx" %>
<!Doctype html>
<html>
<head>
    <%
	    title = "Features - Stories, Specials & Travelogues | BikeWale";
	    description = "Features section of BikeWale brings specials, stories, travelogues and much more.";
	    keywords = "features, stories, travelogues, specials, drives.";
	    canonical = "https://www.bikewale.com/features/";
	    alternate = "https://www.bikewale.com/m/features/";
        relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "https://www.bikewale.com" + prevPageUrl;
        relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "https://www.bikewale.com" + nextPageUrl;
	    AdId = "1395986297721";
	    AdPath = "/1017752/BikeWale_New_";
        isAd300x250BTFShown = false;
    %>
    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl  %>/UI/css/content/listing.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

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
                        <li><span class="bwsprite fa-angle-right margin-right10"></span>Features</li>
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
                             <% if(articlesList != null && articlesList.Count > 0) { %>
                            <h1 class="section-header">Features</h1>
                            <div class="section-inner-padding">
                                <% foreach(var article in articlesList){ %> 
						                <div id='post-<%= article.BasicId%>' class="<%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "sponsored-content" : "post-content" %> article-content">
								                <%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
								            <div class="article-image-wrapper">
									            <%= string.Format("<a href='{0}'><img src='{1}' alt='{2}' title='{2}' width='100%' border='0' /></a>",Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId), article.ArticleUrl, Bikewale.Entities.CMS.EnumCMSContentType.Features.ToString()), Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl, article.HostUrl, Bikewale.Utility.ImageSize._210x118), article.Title) %>
								            </div>
								            <div class="article-desc-wrapper">
									            <h2 class="font14 margin-bottom10">
										            <a href="<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId), article.ArticleUrl, Bikewale.Entities.CMS.EnumCMSContentType.Features.ToString()) %>" rel="bookmark" class="text-black text-bold">
											            <%= article.Title %>
										            </a>
									            </h2>
									            <div class="font12 text-light-grey margin-bottom25">
										            <div class="article-date">
											            <span class="bwsprite calender-grey-icon inline-block"></span>
											            <span class="inline-block">
												            <%= Bikewale.Utility.FormatDate.GetFormatDate(article.DisplayDate.ToString(),"dd MMMM yyyy") %>
											            </span>
										            </div>
										            <div class="article-author">
											            <span class="bwsprite author-grey-icon inline-block"></span>
											            <span class="inline-block">
												            <%= article.AuthorName %>
											            </span>
										            </div>
									            </div>
									            <div class="font14 line-height"><%= article.Description %><a href="<%= Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId), article.ArticleUrl, Bikewale.Entities.CMS.EnumCMSContentType.Features.ToString()) %>">Read full story</a></div>
								            </div>
                                            <div class="clear"></div>
						                </div>
                                <%} %>
                                <div id="footer-pagination" class="font14 padding-top10">
                                    <div class="grid-5 alpha omega text-light-grey">
                                                                              <p>Showing <span class="text-default text-bold"><%= startIndex %>-<%= endIndex %></span> of <span class="text-default text-bold"><%= totalArticles %></span> articles</p>
                                    </div>
				                    <BikeWale:RepeaterPager ID="ctrlPager" runat="server" />
                                   <div class="clear"></div>
                                </div>
                                <%} %>
                            </div>
                        </div>
                    </div>

                    <script type="text/javascript" src="<%= staticUrl  %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>

                    <div class="grid-4 omega">

                        <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />

                         <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />

                         <div class="margin-bottom20">
                           <!-- #include file="/UI/ads/Ad300x250.aspx" -->
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
            </div>
        </section>

        <!-- #include file="/UI/includes/footerBW.aspx" -->

        <link href="<%= staticUrl  %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <!-- #include file="/UI/includes/fontBW.aspx" -->
    </form>
</body>
</html>

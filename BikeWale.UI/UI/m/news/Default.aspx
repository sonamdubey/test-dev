<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.News.Default" Trace="false" Async="true" %>

<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/UI/m/controls/LinkPagerControl.ascx" %>
<%@ Register Src="~/UI/m/controls/UpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin" %>
<%@ Register Src="~/UI/m/controls/PopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin" %>
<%@ Register Src="~/UI/m/controls/PopularBikesByBodyStyle.ascx" TagPrefix="BW" TagName="MBikesByBodyStyle"  %>
<!DOCTYPE html>
<html>
<head>
    <% 
	    title       = objNews.PageTitle;
	    description = objNews.Description;
	    keywords    = objNews.Keywords; 
	    canonical   = objNews.Canonical;
	    relPrevPageUrl = objNews.prevUrl;
	    relNextPageUrl = objNews.nextUrl;
	    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
	    AdId = "1398766302464";
	    Ad_320x50 = true;
        Ad_Mid_320x50=true;
    %>

    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl  %>/m/css/content/listing.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <h1 class="box-shadow padding-15-20"><%= objNews.PageH1 %></h1>

                <div id="divListing" class="article-list">
                    <% foreach (var news in newsArticles)
	{ %>
                    <a href="<%= string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString( news.BasicId),news.ArticleUrl,Convert.ToString(news.CategoryId))) %>" title="<%=  news.Title %>">
                        <div class="article-item-content <%= news.AuthorName.ToLower().Contains("sponsored") ? "sponsored-content" : ""%>">

                            <%= Regex.Match(news.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>

                            <div class="article-wrapper">
                                <div class="article-image-wrapper">
                                    <img class="lazy" alt='<%= news.Title %>' title="<%= news.Title %>" data-original='<%= Bikewale.Utility.Image.GetPathToShowImages(news.OriginalImgUrl, news.HostUrl,Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0" src="">
                                </div>
                                <div class="padding-left10 article-desc-wrapper">
                                    <div class="article-category">
                                        <span class="text-uppercase font12 text-bold"><%= GetContentCategory(Convert.ToString(news.CategoryId)) %></span>
                                    </div>
                                    <h2 class="font14"><%= news.Title %></h2>
                                </div>
                            </div>

                            <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%= Bikewale.Utility.FormatDate.GetFormatDate(news.DisplayDate.ToString(),"dd MMMM yyyy") %></span>
                            </div>

                            <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                <span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%= news.AuthorName %></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </a>
                    <%} %>
                </div>
                <div class="margin-right10 margin-left10 padding-top15 padding-bottom15 border-solid-top font14">
                    <div class="grid-5 omega text-light-grey font13">
                        <span class="text-bold text-default"><%=startIndex %>-<%=endIndex %></span> of <span class="text-bold text-default"><%=totalrecords %></span> articles
                    </div>
                    <BikeWale:newPager ID="ctrlPager" runat="server" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <div class="margin-bottom15">
            <!-- #include file="/UI/ads/Ad320x50_Middle_mobile.aspx" -->
        </div>

        <BW:MPopularBikesMin runat="server" ID="ctrlPopularBikes" />
        <%if(modelId>0){%>
        <%if (ctrlBikesByBodyStyle.FetchedRecordsCount > 0){%>
         <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-bottom20">
               <BW:MBikesByBodyStyle runat="server" ID="ctrlBikesByBodyStyle" />
                </div>
             </section>
        <%} %>
        <%}else{ %>
        <BW:MUpcomingBikesMin runat="server" ID="ctrlUpcomingBikes" />
        <%} %>
        <script type="text/javascript" src="<%= staticUrl %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->

        <script type="text/javascript">
            ga_pg_id = "10";
        </script>
    </form>
</body>
</html>

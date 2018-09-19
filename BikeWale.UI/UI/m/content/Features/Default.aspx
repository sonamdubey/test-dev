<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.Features" Async="true" Trace="false" %>

<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/UI/m/controls/LinkPagerControl.ascx" %>
<%@ Register Src="~/UI/m/controls/UpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin" %>
<%@ Register Src="~/UI/m/controls/PopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin" %>
<!DOCTYPE html>
<html>
<head>
    <% 
        title = "Features - Stories, Specials & Travelogues - BikeWale";
        description = "Learn about the trending stories related to bike and bike products. Know more about features, do's and dont's of different bike products exclusively on BikeWale";
        keywords = "features, stories, travelogues, specials, drives.";
        canonical = "https://www.bikewale.com/features/";
        relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "https://www.bikewale.com" + prevPageUrl;
        relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "https://www.bikewale.com" + nextPageUrl;
        AdPath = "/1017752/Bikewale_Mobile_NewBikes_";
        AdId = "1398766302464";
        Ad_320x50 = true;
        Ad_Mid_320x50 = true;
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
                <h1 class="box-shadow padding-15-20">Latest Bike Features</h1>
                <% if(articlesList != null && articlesList.Count > 0) { %>
                <div id="divListing" class="article-list">
                    <% foreach(var article in articlesList){ %>                    
                            <a href='<%= string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),Convert.ToString(article.ArticleUrl),(Bikewale.Entities.CMS.EnumCMSContentType.Features).ToString())) %>' title="<%= article.Title %>">

                                <div class='article-item-content <%= Convert.ToString(article.AuthorName).ToLower().Contains("sponsored") ? "sponsored-content" : ""%>'>

                                    <%= Regex.Match(Convert.ToString(article.AuthorName), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>

                                    <div class="article-wrapper">
                                        <div class="article-image-wrapper">
                                            <img class="lazy" alt='<%= article.Title %>' title="<%= article.Title %>" data-original='<%= Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl, article.HostUrl, Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0" src="">
                                        </div>
                                        <div class="padding-left10 article-desc-wrapper">
                                            <h2 class="font14"><%= article.Title %></h2>
                                        </div>
                                    </div>

                                    <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                        <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%= Bikewale.Utility.FormatDate.GetFormatDate(article.DisplayDate.ToString(), "dd MMMM yyyy") %></span>
                                    </div>
                                    <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                                        <span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%= article.AuthorName %></span>
                                    </div>
                                    <div class="clear"></div>

                                </div>

                            </a>
                    <%} %>
                </div>
                <%} %>
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
        <BW:MUpcomingBikesMin runat="server" ID="ctrlUpcomingBikes" />        
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->

    </form>
</body>
</html>

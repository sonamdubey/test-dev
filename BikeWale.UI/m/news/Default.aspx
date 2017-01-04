﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.News.Default"  Trace="false" Async="true"%>
<%@ Register TagPrefix="BikeWale" TagName="newPager" Src="/m/controls/LinkPagerControl.ascx" %>
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
	    Ad_Bot_320x50 = true;
    %>
   
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/content/listing.css?<%= staticFileVersion %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <h1 class="box-shadow padding-15-20">Latest Bike News</h1>

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
								        <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%= Bikewale.Utility.FormatDate.GetFormatDate(news.DisplayDate.ToString(),"MMM dd, yyyy") %></span>
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
                        <span class="text-bold text-default"><%=startIndex %>-<%=endIndex %></span> of <span class="text-bold text-default"><%=totalrecords %></span> articles</div>
	         <BikeWale:newPager ID="ctrlPager" runat="server" />
            <div class="clear"></div>
                </div>
                     <div class="clear"></div>
             </div>
        </section>
          <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->

        <script type="text/javascript">
            ga_pg_id = "10";
        </script>
    </form>
</body>
</html>
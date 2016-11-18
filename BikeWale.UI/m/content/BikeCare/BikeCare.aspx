<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.BikeCare" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="/m/controls/LinkPagerControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = pgTitle;
        description = pgDescription;
        keywords = pgKeywords;
        relPrevPageUrl = pgPrevUrl;
        relNextPageUrl = pgNextUrl;
   %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/content/listing.css?<%= staticFileVersion %>" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <div class="box-shadow padding-15-20">
                    <h1 class="margin-bottom5">Bike Care</h1>
                    <h2 class="font14 text-unbold">BikeWale brings you maintenance tips from experts to rescue you from common problems</h2>
                </div>

                <% if(objArticleList!=null){%>
	            <div id="divListing" class="article-list">
                     <%foreach(var article in objArticleList.Articles) {%>
                        <a href="/m<%=Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(article.BasicId),article.ArticleUrl,Bikewale.Entities.CMS.EnumCMSContentType.TipsAndAdvices.ToString()) %>" title="<%=article.Title%>">

					        <div class="article-item-content">
						        <div class="article-wrapper">
							        <div class="article-image-wrapper">
                                        <img class="lazy" alt='Bike Care:<%=article.Title%>' title="Bike Care: <%=article.Title%>" data-original="<%=Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(article.OriginalImgUrl),Convert.ToString(article.HostUrl),Bikewale.Utility.ImageSize._110x61) %>" width="100%" border="0" src="">
							        </div>
							        <div class="padding-left10 article-desc-wrapper">
								        <h2 class="font14">Bike Care: <%=article.Title%></h2>
							        </div>
						        </div>
						        <div class="article-stats-wrapper font12 leftfloat text-light-grey">
							        <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%=Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(article.DisplayDate), "MMM dd, yyyy") %></span>
						        </div>
						        <div class="article-stats-wrapper font12 leftfloat text-light-grey">
							        <span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%=article.AuthorName %></span>
						        </div>
						        <div class="clear"></div>
					        </div>
				        </a>
                    <%} %>
                    <%} %>
                </div>

                <div class="margin-right10 margin-left10 padding-top15 padding-bottom15 border-solid-top font14">
                    <div class="grid-5 omega text-light-grey font13">
                        <span class="text-bold text-default"><%=startIndex %>-<%=endIndex %></span> of <span class="text-bold text-default"><%=totalArticles %></span> articles
                    </div>
	                <BikeWale:Pager ID="ctrlPager" runat="server" />
                    <div class="clear"></div>
                </div>

            </div>
        </section>
        
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->

        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />

    </form>
</body>
</html>
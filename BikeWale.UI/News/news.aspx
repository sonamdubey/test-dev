<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.news"  %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMinNew.ascx" %>
<!Doctype html>
<html>
<head>
	<%
        if(metas!=null)
        {
            title = metas.Title;
            description = metas.Description;
            canonical = metas.CanonicalUrl;
            fbTitle = metas.Title;
            fbImage = metas.ShareImage;
            alternate = metas.AlternateUrl;  
        }
		
		AdId="1395995626568";
		AdPath="/1017752/BikeWale_News_";
        isAd300x250Shown = true;
        isAd300x250BTFShown = false;
        
	%>

	<!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/content/details.css" rel="stylesheet" type="text/css" />

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
						<li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
							<span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/news/" itemprop="url">
                                <span itemprop="title">Bike News</span>
                            </a>
                        </li>
                        <% if(objArticle!=null) { %>
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
                            <% if(objArticle!=null) { %>
                            <div class="section-header">
                                <h1 class="margin-bottom5"><%= objArticle.Title %></h1>
								<div>
									<span class="bwsprite calender-grey-sm-icon"></span>
									<span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(objArticle.DisplayDate), "MMMM dd, yyyy hh:mm tt") %></span>
									<span class="bwsprite author-grey-sm-icon"></span>
									<span class="article-stats-content"><%= objArticle.AuthorName %></span>
								</div>
                            </div>
                            <div class="section-inner-padding">
								<div id="post-<%= objArticle.BasicId %>">
									<div class="article-content padding-top5">
										<%if(!String.IsNullOrEmpty(metas.ShareImage)) %>
											<div style="text-align:center;"><img alt='<%= objArticle.Title %>' title='<%= objArticle.Title %>' src='<%= metas.ShareImage %>'></div>
										<%= objArticle.Content %>
										<div class="clear"></div>
									</div>
								</div>
								<div class="grid-6 alpha">
									<% if (!String.IsNullOrEmpty(objArticle.PrevArticle.ArticleUrl))
									   { %>
										<a href="<%= "/news/" + objArticle.PrevArticle.BasicId + "-" + objArticle.PrevArticle.ArticleUrl + ".html"%>" class="text-default next-prev-article-target">
											<span class="bwsprite prev-arrow"></span>
											<span class="text-bold">Previous Article</span><br />
											<span class="next-prev-article-title"><%=objArticle.PrevArticle.Title %></span>
										</a>
									<%} %>
								</div>
								<div class="grid-6 omega text-right">
									<% if (!String.IsNullOrEmpty(objArticle.NextArticle.ArticleUrl))
									   { %>
										<a href="<%=  "/news/" + objArticle.NextArticle.BasicId + "-" + objArticle.NextArticle.ArticleUrl + ".html"%>" class="text-default next-prev-article-target">
											<span class="text-bold">Next Article</span>
											<span class="bwsprite next-arrow"></span><br />
											<span class="next-prev-article-title"><%=objArticle.NextArticle.Title %></span>
										</a>
									<% } %>
								</div>
								<div class="clear"></div>
							</div>
                            <% } %>
						</div>
					</div>

					<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

					<div class="grid-4 omega">
					     <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />
						<div class="margin-bottom20">
                              <!-- #include file="/ads/ad300x250.aspx" -->
                        </div>
                        <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />
					</div>
					<div class="clear"></div>
				</div>
				<div class="clear"></div>
			</div>
		</section>

        

		<!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
		<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
		<script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>">"></script>
		<script type="text/javascript" type="text/javascript">
		    $(document).ready(function () {
		        $("body").floatingSocialShare();
		    });

		</script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultRT" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/UI/m/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/UI/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/UI/controls/UpcomingBikesMinNew.ascx" %>
<%@ Register TagPrefix="BW" TagName="PopularBikesByBodyStyle" Src="~/UI/controls/PopularBikesByBodyStyle.ascx" %>
<!Doctype html>
<html>
<head>
    <%
	    // Listing page
	    if (string.IsNullOrEmpty(modelName) && string.IsNullOrEmpty(makeName))
	    {
		    title = "Expert Bike Reviews India - Bike Comparison & Road Tests - BikeWale";
		    description = "Latest expert reviews on upcoming and new bikes in India. Read bike comparison tests and road tests exclusively on BikeWale";
		    keywords = "Expert bike reviews, bike road tests, bike comparison tests, bike reviews, road tests, expert reviews, bike comparison, comparison tests";
		    canonical = "https://www.bikewale.com" + "/expert-reviews/";
		    alternate = "https://www.bikewale.com" + "/m/expert-reviews/";
	    }
	    // Model Name exists
	    else if (!string.IsNullOrEmpty(modelName))
	    {
		    title = string.Format("{0} {1} Expert Reviews India - Bike Comparison & Road Tests - BikeWale",makeName, modelName);
		    description = string.Format("Latest expert reviews on {0} {1} in India. Read {0} {1} comparison tests and road tests exclusively on BikeWale", makeName, modelName);
		    keywords = string.Format("{0} {1} expert reviews, {0} {1} road tests, {0} {1} comparison tests, {0} {1} reviews, {0}{1} bike comparison", makeName, modelName);
		    canonical = string.Format("https://www.bikewale.com/{0}-bikes/{1}/expert-reviews/", makeMaskingName, modelMaskingName);
		    alternate = string.Format("https://www.bikewale.com/m/{0}-bikes/{1}/expert-reviews/", makeMaskingName, modelMaskingName);
	    }
	    // Make name exists
	    else
	    {
		    title = string.Format("{0} Bikes Expert Reviews India - Bike Comparison & Road Tests - BikeWale", makeName);
		    description = string.Format("Latest expert reviews on upcoming and new {0} bikes in India. Read {0} bike comparison tests and road tests exclusively on BikeWale", makeName);
		    keywords = string.Format("{0} bike expert reviews, {0} bike road tests, {0} bike comparison tests, {0} bike reviews, {0} road tests, {0} expert reviews, {0} bike comparison, {0} comparison tests.",makeName);
		    canonical = string.Format("https://www.bikewale.com/{0}-bikes/expert-reviews/", makeMaskingName);
		    alternate = string.Format("https://www.bikewale.com/m/{0}-bikes/expert-reviews/", makeMaskingName);
	    }
	    fbTitle = title;
	    AdId = "1395986297721";
	    AdPath = "/1017752/Bikewale_Reviews_";
        relPrevPageUrl = prevUrl;
        relNextPageUrl = nextUrl;
	    fbImage = Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo;
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
                        <% if (!string.IsNullOrEmpty(makeName)) 
		                    {%>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%=makeMaskingName %>-bikes/" itemprop="url"><span itemprop="title"><%=makeName%> Bikes</span></a>
                        </li>
                        <% } %>
                        <% if (!string.IsNullOrEmpty(modelName)) 
		                    {%>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%=makeMaskingName %>-bikes/<%=modelMaskingName%>/" itemprop="url"><span itemprop="title"><%= String.Format("{0} {1}", makeName,modelName)%> Bikes</span></a>
                        </li>
                        <% } %>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span>Expert Reviews</li>
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
                            <% if (!string.IsNullOrEmpty(modelName)) 
		                       {%>
		                    <h1 class="section-header"><%= String.Format("{0} {1}", makeName,modelName) %> Expert Reviews</h1>
		                    <% }
		                       else if(!string.IsNullOrEmpty(makeName)) { %>
		                    <h1 class="section-header"><%= makeName  %> Bikes Expert Reviews</h1>
		                    <% } else {
		                     %>
		                    <h1 class="section-header">Expert Reviews</h1>
		                    <% } %>
                            <% if(articlesList != null && articlesList.Count > 0) { %>
                            <div class="section-inner-padding">
                                	<% foreach(var article in articlesList){ %>      			
						                <div id='post-<%= article.BasicId %>' class="<%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "sponsored-content" : "post-content" %> article-content">
							                <%= Regex.Match(article.AuthorName, @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
								            <div class="article-image-wrapper">
									            <%= string.Format("<a href='/expert-reviews/{0}-{1}.html'><img src='{2}' alt='{3}' title='{3}' width='100%' border='0' /></a>", article.ArticleUrl, article.BasicId, Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl, article.HostUrl, Bikewale.Utility.ImageSize._210x118), article.Title) %>
								            </div>
								            <div class="article-desc-wrapper">
									            <h2 class="font14 margin-bottom10">
										            <a href='/expert-reviews/<%= article.ArticleUrl %>-<%= article.BasicId %>.html' rel="bookmark" class="text-black text-bold">
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
												            <%= article.AuthorName%>
											            </span>
										            </div>
									            </div>
									            <div class="font14 line-height"><%= article.Description %><a href="/expert-reviews/<%= article.ArticleUrl %>-<%= article.BasicId %>.html">Read full review</a></div>
								            </div>
								            <div class="clear"></div>
						                </div>
					                <% } %>

                                <div id="footer-pagination" class="font14 padding-top10">
                                    <div class="grid-5 alpha omega text-light-grey">
                                                                       <p>Showing <span class="text-default text-bold"><%= startIndex %>-<%= endIndex %></span> of <span class="text-default text-bold"><%= totalArticles %></span> articles</p>                                        
                                    </div>
				                    <BikeWale:RepeaterPager ID="ctrlPager" runat="server" />
                                     <div class="clear"></div>
                                </div>
                            </div>
                            <%} %>
                        </div>
                    </div>

                    <script type="text/javascript" src="<%= staticUrl  %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>

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
                           <!-- #include file="/UI/ads/Ad300x250.aspx" -->
                        </div>                       
                        <a href="/pricequote/" id="on-road-price-widget" class="content-box-shadow content-inner-block-20">
                            <span class="inline-block">
                                <img src="https://imgd.aeplcdn.com/0x0/bw/static/design15/on-road-price.png" alt="Rupee" border="0" />
                            </span><h2 class="text-default inline-block">Get accurate on-road price for bikes</h2>
                            <span class="bwsprite right-arrow"></span>
                        </a>
                        <%--<div>
				        <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
				        
				        </div>--%>
			        
			            <%--<div>
				            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
				            <!-- #include file="/UI/ads/Ad300x250BTF.aspx" -->
			            </div>--%>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/UI/includes/footerBW.aspx" -->
        
        <link href="<%= staticUrl %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <!-- #include file="/UI/includes/fontBW.aspx" -->
    </form>
</body>
</html>


<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.BikeCare" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/m/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMinNew.ascx" %>
<%
    title = pgTitle;
    description = pgDescription;
    keywords = pgKeywords;
    relPrevPageUrl = pgPrevUrl;
    relNextPageUrl = pgNextUrl;
    canonical = "http://www.bikewale.com/bike-care/";
    alternate = "http://www.bikewale.com/m/bike-care/";
    AdId = "1395995626568";
    AdPath = "/1017752/BikeWale_News_";
    isAd300x250Shown = true;
    isAd300x250BTFShown = false;
  
%>
            <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/content/listing.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
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
                        <li><span class="bwsprite fa-angle-right margin-right10"></span>Bike Care</li>
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
                        <div>
                            <div class="section-header">
		                    <h1 >Bike Care</h1>
                            <h2>BikeWale brings you maintenance tips from experts to rescue you from common problems</h2>
                                </div>
                            <div class="section-inner-padding">
                                <% foreach (var article in objArticleList.Articles) {%>					
						                <div id='post-<%=article.BasicId%>' class="article-content">
							                
								            <div class="article-image-wrapper">
									            <%= string.Format("<a href='/bike-care/{0}-{1}.html'><img src='{2}' alt='{3}' title='{3}' width='100%' border='0' /></a>", article.ArticleUrl,article.BasicId,Bikewale.Utility.Image.GetPathToShowImages(article.OriginalImgUrl,article.HostUrl,Bikewale.Utility.ImageSize._210x118),article.Title) %>
								            </div>
								            <div class="article-desc-wrapper">
									            <h2 class="font14 margin-bottom10">
										            <a href='/bike-care/<%=article.ArticleUrl %>-<%=article.BasicId%>.html' rel="bookmark" class="text-black text-bold" title=" <%=article.Title%>">
											            <%=article.Title%>
										            </a>
									            </h2>
									            <div class="font12 text-light-grey margin-bottom25">
										            <div class="article-date">
											            <span class="bwsprite calender-grey-icon inline-block"></span>
											            <span class="inline-block">
												            <%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(article.DisplayDate),"MMMM dd, yyyy") %>
											            </span>
										            </div>
										            <div class="article-author">
											            <span class="bwsprite author-grey-icon inline-block"></span>
											            <span class="inline-block">
												            <%=article.AuthorName  %>
											            </span>
										            </div>
									            </div>
									            <div class="font14 line-height"><%= Bikewale.Utility.FormatDescription.TruncateDescription(Convert.ToString(article.Description) ,250)%><a href="/bike-care/<%=article.ArticleUrl %>-<%=article.BasicId%>.html" title=" <%=article.Title%>">Read full story</a></div>
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
                            </div>
                        </div>
                    </div>

                    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

                    <div class="grid-4 omega">
                        <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />
                       <!-- #include file="/ads/Ad300x250.aspx" -->
                       
                          <BW:UpcomingBikes ID="ctrlUpcoming" runat="server" />

                         <div class="margin-bottom20">
                          
                        </div>
                        <a href="/pricequote/" id="on-road-price-widget" class="content-box-shadow content-inner-block-20">
                            <span class="inline-block">
                                <img src="https://imgd1.aeplcdn.com/0x0/bw/static/design15/on-road-price.png" alt="Rupee" border="0" />
                            </span><h2 class="text-default inline-block">Get accurate on-road price for bikes</h2>
                            <span class="bwsprite right-arrow"></span>
                        </a>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>

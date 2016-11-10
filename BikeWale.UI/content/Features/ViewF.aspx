<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewF" Trace="false" Debug="false" Async="true" %>
<%@ Register TagPrefix="PG" TagName="PhotoGallery" Src="/controls/ArticlePhotoGallery.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpComingBikesCMS.ascx" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!Doctype html>
<html>
<head>
    <% 
        title = articleTitle + " - Bikewale ";
        keywords = "features, stories, travelogues, specials, drives";
        description = "";
        canonical = "http://www.bikewale.com" + canonicalUrl;
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        alternate = "http://www.bikewale.com/m" + canonicalUrl;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/content/details.css" rel="stylesheet" type="text/css" />
    <link href="/css/components/model-gallery.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/common/jquery.colorbox-min.js?v=1.0"></script>
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
                            <a href="/features/" itemprop="url">
                                <span itemprop="title">Features</span>
                            </a>
                        </li>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span><%= articleTitle%> Features</li>
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
                            <div class="section-header">
                                <h1><%= articleTitle%></h1>
                                <div>
									<span class="bwsprite calender-grey-sm-icon"></span>
									<span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "MMMM dd, yyyy hh:mm tt") %></span>
									<span class="bwsprite author-grey-sm-icon"></span>
									<span class="article-stats-content"><%= authorName%></span>
								</div>
                            </div>
                            <div class="section-inner-padding">
                                <div id="topNav" runat="server" class="margin-bottom10">
                                    <asp:repeater id="rptPages" runat="server">
                                        <headertemplate>
                                        <ul>
                                        </headertemplate>
					                    <itemtemplate>
                                            <li>
                                                <a href="#<%#Eval("pageId") %>"><%#Eval("PageName") %></a>
                                            </li>
					                    </itemtemplate>
                                        <footertemplate>
                                            <li>
                                                <a href="#divPhotos">Photos</a>
                                            </li>
                                        </ul>
                                        </footertemplate>
				                    </asp:repeater>
                                </div>
                                <div class="clear"></div>
                                <asp:repeater id="rptPageContent" runat="server">
					                <itemtemplate>
                                        <div class="margin-top10 margin-bottom10">
                                            <h3 class="article-content-title"><%#Eval("PageName") %></h3>
                                            <div id='<%#Eval("pageId") %>' class="margin-top10 article-content">
                                                <%#Eval("content") %>
                                            </div>
                                        </div>
					                </itemtemplate>             
				                </asp:repeater>
                                <div id="divPhotos">
                                    <PG:PhotoGallery runat="server" ID="ctrPhotoGallery" />
                                </div>
                            </div>
                        </div>
                    </div>
                                                    
                    <div class="grid-4 omega">
                    <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />
						<div class="margin-bottom20">
                         <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
                           { %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                        <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />
                        <div class="margin-top10 margin-bottom10">
                                <a href="<%=upcomingBikeslink%>" class="font14">View all upcoming bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            </div>
                        <% } %>
                                 <!-- #include file="/ads/ad300x250.aspx" -->
                        </div>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                            <h2>Upcoming Royal Enfield bikes</h2>
                            <ul class="sidebar-bike-list">
                                <li>
                                    <a href="" title="Harley Davison Softail" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Harley Davison Softail</h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;87,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Bajaj Pulsar AS200" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Bajaj Pulsar AS200</h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;92,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Honda Unicorn 150" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Honda Unicorn 150</h3>
                                            <p class="font11 text-light-grey">Expected price</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;1,12,000</span>
                                        </div>
                                    </a>
                                </li>
                            </ul>
                            <div class="margin-top10 margin-bottom10">
                                <a href="" class="font14">View all upcoming Royal Enfield bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div id="back-to-top" class="back-to-top"><a><span></span></a></div>        
         <!-- #include file="/includes/footerBW.aspx" -->
          <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
		<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
		<script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>">"></script>
                 <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/content/details.js?<%= staticFileVersion %>"></script>
         <script type="text/javascript">
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
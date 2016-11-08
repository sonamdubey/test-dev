﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.News.Default" Trace="false" EnableViewState="false" Async="true" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="~/m/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpComingBikesCMS.ascx" %>
<!Doctype html>
<html>
<head>
    <%
        title = "Bike News - Latest Indian Bike News & Views | BikeWale";
        description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
        keywords = "news, bike news, auto news, latest bike news, indian bike news, bike news of india";
        canonical = "http://www.bikewale.com/news/";
        relPrevPageUrl = prevUrl;
        relNextPageUrl = nextUrl;
        alternate = "http://www.bikewale.com/m/news/";
        AdId = "1395995626568";
        AdPath = "/1017752/BikeWale_News_";
        isAd300x250Shown=false;
    
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/content/listing.css" rel="stylesheet" type="text/css" />

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
                            <div id="news-header" class="section-header">
                                <h1 class="margin-right5">Bike News</h1>
                                <h2 class="font14 text-unbold text-light-grey">Latest Indian Bikes News and Views</h2>
                            </div>
                            <div class="section-inner-padding">
                                <asp:repeater id="rptNews" runat="server">
                                    <itemtemplate>
                                        <div id='post-<%# Eval("BasicId") %>' class="<%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "sponsored-content" : "post-content" %> article-content">
                                            <%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
                                        
                                            <div class="article-image-wrapper">
                                                <%# string.Format("<a href='{0}'><img src='{1}' alt='{2}' title='{2}' width='100%' border='0' /></a>",Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CategoryId"))),Bikewale.Utility.Image.GetPathToShowImages(Convert.ToString(DataBinder.Eval(Container.DataItem,"OriginalImgUrl")),Convert.ToString(DataBinder.Eval(Container.DataItem,"HostUrl")),Bikewale.Utility.ImageSize._210x118),Convert.ToString(DataBinder.Eval(Container.DataItem,"Title"))) %>
                                            </div>
                                            <div class="article-desc-wrapper">
                                                <div class="article-category">
                                                    <span class="text-uppercase font12 text-bold"><%# GetContentCategory(DataBinder.Eval(Container.DataItem,"CategoryId").ToString()) %></span>
                                                </div>
                                                <h3 class="font14 margin-bottom10">
                                                    <a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CategoryId"))) %>" rel="bookmark" class="text-black text-bold"><%# Eval("Title") %></a>
                                                </h3>
                                                <div class="font12 text-light-grey margin-bottom20">
                                                    <div class="article-date">
                                                        <span class="bwsprite calender-grey-icon inline-block"></span>
                                                        <span class="inline-block">
                                                            <%# Bikewale.Utility.FormatDate.GetFormatDate(Eval("DisplayDate").ToString(),"MMMM dd, yyyy") %>
                                                        </span>
                                                    </div>
                                                    <div class="article-author">
                                                        <span class="bwsprite author-grey-icon inline-block"></span>
                                                        <span class="inline-block">
                                                            <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="font14 line-height"><%# DataBinder.Eval(Container.DataItem,"Description") %><a href="<%# Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),Convert.ToString(DataBinder.Eval(Container.DataItem,"CategoryId"))) %>">Read full story</a></div>
                                            </div>
                                            <div class="clear"></div>
                                        
                                        </div>
                                    </itemtemplate>
                                </asp:repeater>

                                <div id="footer-pagination" class="font14 padding-top10">
                                    <div class="grid-5 alpha omega text-light-grey">
                                        <p>Showing <span class="text-default text-bold">1-10</span> of <span class="text-default text-bold">26,398</span> articles</p>                                        
                                    </div>
                                    <BikeWale:RepeaterPager ID="ctrlPager" runat="server" />
                                </div>
                            </div>                        
                        </div>
                    </div>

                    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

                    <div class="grid-4 omega">
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                            <h2>Popular bikes</h2>
                            <ul class="sidebar-bike-list">
                                <li>
                                    <a href="" title="Harley Davison Softail" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Harley Davison Softail</h3>
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
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
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
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
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;1,12,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="" title="Royal Enfield Thunderbird 350" class="bike-target-link">
                                        <div class="bike-target-image inline-block">
                                            <img src="http://imgd1.aeplcdn.com//110x61//bw/models/tvs-apache-rtr-200-4v.jpg" />
                                        </div>
                                        <div class="bike-target-content inline-block padding-left10">
                                            <h3>Royal Enfield Thunderbird 350</h3>
                                            <p class="font11 text-light-grey">Ex-showroom New Delhi</p>
                                            <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;1,32,000</span>
                                        </div>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
                           { %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                        <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />
                        <div class="margin-top10 margin-bottom10">
                                <a href="/upcoming-bikes/" class="font14">View all upcoming bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            </div>
                        <% } %>
                        
                        <div class="margin-bottom20">
                            <!-- Ad -->
                        </div>
                        <a href="" id="on-road-price-widget" class="content-box-shadow content-inner-block-20">
                            <span class="inline-block">
                                <img src="https://imgd1.aeplcdn.com/0x0/bw/static/design15/on-road-price.png" alt="Rupee" border="0" />
                            </span><h2 class="text-default inline-block">Get accurate on-road price for bikes</h2>
                            <span class="bwsprite right-arrow"></span>
                        </a>
                        <%--<div class="margin-top15">
                            <!-- BikeWale_News/BikeWale_News_300x250 -->
                            <!-- #include file="/ads/Ad300x250.aspx" -->
                        </div>--%>
                        
                        <%--<div>
                            <!-- BikeWale_News/BikeWale_News_300x250 -->
                            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                        </div>--%>
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

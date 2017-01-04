﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewRT" Trace="false" Async="true" Debug="false" %>
<%@ Register TagPrefix="PG" TagName="PhotoGallery" Src="/controls/ArticlePhotoGallery.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMinNew.ascx" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<!Doctype html>
<html>
<head>
    <%
        title = articleTitle;
        description 	= "Learn about the trending stories related to bike and bike products. Know more about features, do's and dont's of different bike products exclusively on BikeWale";
        //keywords		= RoadTestPageKeywords;
        canonical = "https://www.bikewale.com/expert-reviews/" + articleUrl + "-" + basicId + ".html";
        //prevPageUrl     = prevUrl;
        //nextPageUrl     = nextUrl;
        fbTitle = articleTitle;
        //fbImage			= fbLogoUrl;
        alternate = "https://www.bikewale.com/m/expert-reviews/" + articleUrl + "-" + basicId + ".html";
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_Reviews_";
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/content/details.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    
    <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
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
                            <a href="/expert-reviews/" itemprop="url">
                                <span itemprop="title">Expert Reviews</span>
                            </a>
                        </li>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span><%= articleTitle%></li>
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
                                <h1 class="margin-bottom5"><%= articleTitle%></h1>
                                <div>
									<span class="bwsprite calender-grey-sm-icon"></span>
									<span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "MMMM dd, yyyy hh:mm tt") %></span>
									<span class="bwsprite author-grey-sm-icon"></span>
									<span class="article-stats-content margin-right20"><%= authorName %></span>
                                    <span class="font12 inline-block text-light-grey"><%= (_bikeTested!=null && !String.IsNullOrEmpty(_bikeTested.ToString())) ? String.Format("{0}",_bikeTested) : "" %></span>
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
                                 <!-- #include file="/ads/ad300x250.aspx" -->
                        </div>
                           <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />
                        
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <div class="back-to-top" id="back-to-top"></div>

        <!-- #include file="/includes/footerBW.aspx" -->
                <BW:ModelGallery ID="ctrlModelGallery" runat="server" />

        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
		<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <!-- #include file="/includes/footerscript.aspx" -->
		<script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "https://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/content/details.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("body").floatingSocialShare();

                $('#drpPages,#drpPages_footer').change(function () {
                    // Modified By :Lucky Rathore on 12 July 2016.
                    Form.Action = Request.RawUrl;
                    var url = '<%= HttpContext.Current.Request.ServerVariables["HTTP_X_ORIGINAL_URL"] %>';
                    if (url.indexOf(".html") > 0) {
                        url = url.substring(0, url.indexOf('.html')) + "/p" + $(this).val() + "/";
                    } else if (url.indexOf("/p") > 0) {
                        url = url.substring(0, url.indexOf('/p')) + "/p" + $(this).val() + "/";
                    }
                    location.href = url;
                });

            });
        </script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>


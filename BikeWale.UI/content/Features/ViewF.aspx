<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewF" Trace="false" Debug="false" Async="true" %>
<%@ Register TagPrefix="PG" TagName="PhotoGallery" Src="/controls/ArticlePhotoGallery.ascx" %>
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
                                <div id="topNav" runat="server">
                                    <div class="text-bold">
                                        Read Pages
                                    </div>
                                    <div style="padding: 5px 0;">
                                        <asp:repeater id="rptPages" runat="server">
                                                <headertemplate>
                                                    <ul>
                                                     <%--   <li style="border:none;" ><a>Read Pages : </a></li>--%>
                                                </headertemplate>
					                            <itemtemplate>
                                                    <li>
                                                        <a href="#<%#Eval("pageId") %>"><%#Eval("PageName") %></a>
                                                    </li>
						                          <%--  <%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url ) %>--%>
					                            </itemtemplate>
                                                <footertemplate>
                                                    <li>
                                                        <a href="#divPhotos">Photos</a>
                                                    </li>
                                                    </ul>
                                                </footertemplate>
					                         <%--   <footertemplate>
						                            <% if ( ShowGallery )  { %>
						                            <%# CreateNavigationLink( Str, Url ) %>
						                            <% } %>	
					                            </footertemplate>--%>
				                            </asp:repeater>
                                    </div>
                                </div>
                                <div class="margin-top10">
                                    <asp:repeater id="rptPageContent" runat="server">
					                            <itemtemplate>
                                                    <div class="margin-top10 margin-bottom10">
                                                        <h3 class="content-block grey-bg"><%#Eval("PageName") %></h3>
                                                        <div id="<%#Eval("pageId") %>" class="margin-top10 article-content">
                                                            <%#Eval("content") %>
                                                        </div>
                                                    </div>
					                            </itemtemplate>             
				                            </asp:repeater>
                                </div>
                                <div id="divPhotos">
                                    <PG:PhotoGallery runat="server" ID="ctrPhotoGallery" />
                                </div>
                                <%--<div class="margin-top10">
			                            <asp:Label ID="lblDetails" runat="server" />
			                            <asp:DataList ID="dlstPhoto" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="top">
				                            <itemtemplate>
					                            <a rel="slidePhoto" target="_blank" href="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathLarge" ).ToString() %>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ) %></b>" />
						                            <img alt="<%# DataBinder.Eval( Container.DataItem, "CategoryName" ) %>" border="0" style="margin:0px 45px 10px 0px;cursor:pointer;" src="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathThumbNail" ).ToString() %>" title="Click to view larger photo" />
					                            </a>
				                            </itemtemplate>
			                            </asp:DataList>
		                            </div>--%>
                                <%--<div class="margin-top10 content-block grey-bg" id="bottomNav" runat="server">
			                            <div align="right" style="width:245px;float:right;">
				                            <asp:DropDownList ID="drpPages_footer" AutoPostBack="true" CssClass="drpClass" runat="server"></asp:DropDownList>
			                            </div>
			                            <div style="width:380px; padding:5px 0;">
				                            <b>Read Page : </b>
				                            <asp:Repeater ID="rptPages_footer" runat="server">
					                            <itemtemplate>
						                            <%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url) %>
					                            </itemtemplate>
					                            <footertemplate>
						                            <% if ( ShowGallery )  { %>
						                            <%# CreateNavigationLink( Str, Url ) %>
						                            <% } %>	
					                            </footertemplate>
				                            </asp:Repeater>
			                            </div>	
		                            </div>--%>
                            </div>
                        </div>
                    </div>

                    <div class="grid-4 omega">
                        <div class="margin-top15">
                            <!-- bikewale_newbike/bikewale_newbike_hp_300x250 -->
                            <!-- #include file="/ads/ad300x250.aspx" -->
                        </div>
                        <div class="margin-top15">
                            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <div id="back-to-top" class="back-to-top"><a><span></span></a></div>        

        <!-- #include file="/includes/footerBW.aspx" -->

        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
		<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
		<script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>">"></script>

        <script type="text/javascript">
            //$(document).ready(function () {
            //    var speed = 300;
            //    //input parameter : id of element, scroll up speed 
            //    ScrollToTop("back-to-top", speed);
            //});
        </script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
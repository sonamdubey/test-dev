<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewRT" Trace="false" Async="true" Debug="false" %>
<%@ Register TagPrefix="PG" TagName="PhotoGallery" Src="/controls/ArticlePhotoGallery.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpComingBikesCMS.ascx" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!Doctype html>
<html>
<head>
    <%
        title = articleTitle;
        //description 	= RoadTestPageDesc;
        //keywords		= RoadTestPageKeywords;
        canonical = "http://www.bikewale.com/expert-reviews/" + articleUrl + "-" + basicId + ".html";
        //prevPageUrl     = prevUrl;
        //nextPageUrl     = nextUrl;
        fbTitle = articleTitle;
        //fbImage			= fbLogoUrl;
        alternate = "http://www.bikewale.com/m/expert-reviews/" + articleUrl + "-" + basicId + ".html";
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_Reviews_";
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="/css/content/details.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
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
						                    <%--<%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url ) %>--%>
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
                                <div class="clear"></div>
                                <%-- %><div class="format-content"><asp:Label ID="lblDetails" runat="server" /></div>
                                    <div id="divOtherInfo" runat="server"></div>
			                        <asp:DataList ID="dlstPhoto" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="top">
				                        <itemtemplate>
					                        <a rel="slidePhoto" target="_blank" href="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathLarge" ).ToString() %>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ).ToString() %></b>" />
						                        <img border="0" alt="<%# MakeMaskName + " " + ModelMaskName %>" style="margin:0px 45px 10px 0px;cursor:pointer;" src="<%# "http://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathThumbNail" ).ToString() %>" title="Click to view larger photo" />
					                        </a>
				                        </itemtemplate>
			                        </asp:DataList>--%>
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
                            <%--<div class="margin-top10 content-block grey-bg" id="bottomNav" runat="server">
			                        <div align="right" style="width:245px;float:right;">
				                        <asp:DropDownList ID="drpPages_footer" CssClass="drpClass" runat="server"></asp:DropDownList>
			                        </div>
			                        <div style="width:380px; padding:5px 0;">
				                        <b>Read Page : </b>
				                        <asp:Repeater ID="rptPages_footer" runat="server">
					                        <itemtemplate>
						                        <%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString()) %>
					                        </itemtemplate>
					                        <footertemplate>
						                        <% if ( ShowGallery )  { %>
						                        <%# CreateNavigationLink( str ) %>
						                        <% } %>	
					                        </footertemplate>
				                        </asp:Repeater>
			                        </div>	
		                        </div>--%>
                            </div>
                        </div>
                    </div>
                     <div class="grid-4 omega">
                        <!-- Right Container starts here -->
                    <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
                           { %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                        <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />
                        <div class="margin-top10 margin-bottom10">
                                <a href="<%=upcomingBikesLink%>" class="font14">View all upcoming bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            </div>
                        <% } %>
                            <div class="margin-top15">
                            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                            <!-- #include file="/ads/Ad300x250.aspx" -->
                        </div>
                        <div>
                            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <div class="back-to-top" id="back-to-top"></div>

        <!-- #include file="/includes/footerBW.aspx" -->
                <BW:ModelGallery ID="ctrlModelGallery" runat="server" />

        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
		<link href="<%= !string.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
		<script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/content/details.js?<%= staticFileVersion %>"></script>

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
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>


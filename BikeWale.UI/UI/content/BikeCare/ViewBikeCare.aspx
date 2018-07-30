<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Content.ViewBikeCare" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMinNew.ascx" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<%@ Register TagPrefix="PG" TagName="PhotoGallery" Src="/controls/ArticlePhotoGallery.ascx" %>
<%@ Register TagPrefix="BW" TagName="PopularBikesByBodyStyle" Src="~/controls/PopularBikesByBodyStyle.ascx" %>
   <%	
    keywords = pageKeywords;
    title = pageTitle;
    description  = pageDescription;
    canonical = string.Format("https://www.bikewale.com/bike-care/{0}-{1}.html", pageTitle, basicId);
    alternate = string.Format("https://www.bikewale.com/bike-care/m/{0}-{1}.html", pageTitle, basicId);
    AdId = "1395995626568";
    AdPath = "/1017752/BikeWale_News_";
    isAd300x250Shown = true;
    isAd300x250BTFShown = false;
%>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl  %>/css/content/details.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    
    <script type="text/javascript" src="<%= staticUrl %>/src/frameworks.js?<%=staticFileVersion %>"></script>
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
                            <a href="/bike-care/" itemprop="url">
                                <span itemprop="title">Bike Care</span>
                            </a>
                        </li>
                        <li><span class="bwsprite fa-angle-right margin-right10"></span><%=pageTitle%></li>
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
                                <h1 class="margin-bottom5"><%= pageTitle%></h1>
                                <div>
									<span class="bwsprite calender-grey-sm-icon"></span>
									<span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "dd MMMM yyyy, hh:mm tt") %></span>
									<span class="bwsprite author-grey-sm-icon"></span>
									<span class="article-stats-content margin-right20"><%= objTipsAndAdvice.AuthorName %></span>
                                    <span class="font12 inline-block text-light-grey"><%= (bikeTested!=null && !String.IsNullOrEmpty(bikeTested.ToString())) ? String.Format("{0}",bikeTested) : "" %></span>
								</div>
                            </div>
                                      <% if (objTipsAndAdvice != null)
        {%>
              
                            <div class="section-inner-padding">
                                <div id="topNav" runat="server" class="margin-bottom10">
                                    <ul>
                                   <% foreach (var page in objTipsAndAdvice.PageList) {%>
                                            <li><a href="#<%=page.pageId %>"><%=page.PageName %></a></li>				                
                                            
                                    <%} %>
                                        <li><a href="#divPhotos">Images</a></li>
                                        </ul>
                                </div>
                                <div class="clear"></div>
                               <% foreach (var page in objTipsAndAdvice.PageList) {%>
                                        <div class="margin-top10 margin-bottom10">
                                            <h3 class="article-content-title"><%=page.PageName%></h3>
                                            <div id='<%=page.pageId %>' class="margin-top10 article-content"><%=page.Content%>
                                            </div>
                                        </div>
                                <%} %>
                                <div id="divPhotos">
                                    <PG:PhotoGallery runat="server" ID="ctrPhotoGallery" />
                                </div>
					           <%} %>
                            </div>
                        </div>
                    </div>
                  
                       <div class="grid-4 omega">
                        <BW:MostPopularBikesMin runat="server" ID="ctrlPopularBikes" />
                       <div class="margin-bottom20">
                            <!-- #include file="/ads/ad300x250.aspx" -->
                        </div>
                        <%if(isModelTagged){ %>
                        <%if (ctrlBikesByBodyStyle.FetchedRecordsCount > 0){ %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                       <h2>Popular <%=ctrlBikesByBodyStyle.BodyStyleText%></h2>
                        <BW:PopularBikesByBodyStyle ID="ctrlBikesByBodyStyle" runat="server"/>
                        </div>
                        <%} %>
                            <%} else{%>
                        <BW:UpcomingBikes ID="ctrlUpcoming" runat="server" />
                        <%} %>
                         <div class="margin-bottom20">
                         
                        </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>
        <div class="back-to-top" id="back-to-top"></div>

        <!-- #include file="/includes/footerBW.aspx" -->
               <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <link href="<%= staticUrl  %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
		<link href="<%= staticUrl  %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <!-- #include file="/includes/footerscript.aspx" -->
		<script type="text/javascript" src="<%= staticUrl  %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript" src="<%= staticUrl  %>/src/content/details.js?<%= staticFileVersion %>"></script>

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

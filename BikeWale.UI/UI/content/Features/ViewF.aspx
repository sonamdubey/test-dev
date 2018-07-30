<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewF" %>

<%@ Register TagPrefix="PG" TagName="PhotoGallery" Src="/controls/ArticlePhotoGallery.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UpcomingBikes" Src="~/controls/UpcomingBikesMinNew.ascx" %>
<%@ Register TagPrefix="BW" TagName="PopularBikesByBodyStyle" Src="~/controls/PopularBikesByBodyStyle.ascx" %>
<%@ Register Src="~/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery" %>
<!Doctype html>
<html>
<head>
    <% 
        title = articleTitle + " - Bikewale ";
        keywords = "features, stories, travelogues, specials, drives";
        description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", articleTitle);
        canonical = Bikewale.Utility.BWConfiguration.Instance.BwHostUrl + canonicalUrl;
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        alternate = Bikewale.Utility.BWConfiguration.Instance.BwHostUrl + "/m" + canonicalUrl;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl  %>/css/content/details.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
    <link rel="amphtml" href="<%= ampUrl %>" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>

    <script type="text/javascript" src="<%= staticUrl  %>/src/frameworks.js?<%=staticFileVersion %>"></script>

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
                                    <span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "dd MMMM yyyy, hh:mm tt") %></span>
                                    <span class="bwsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content"><%= authorName%></span>
                                </div>
                            </div>
                            <div class="section-inner-padding">
                                <div id="topNav" runat="server" class="margin-bottom10">
                                    <asp:Repeater ID="rptPages" runat="server">
                                        <HeaderTemplate>
                                            <ul>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <a href="#<%#Eval("pageId") %>"><%#Eval("PageName") %></a>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <li>
                                                <a href="#divPhotos">Images</a>
                                            </li>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="clear"></div>
                                <asp:Repeater ID="rptPageContent" runat="server">
                                    <ItemTemplate>
                                        <div class="margin-top10 margin-bottom10">
                                            <h3 class="article-content-title"><%#Eval("PageName") %></h3>
                                            <div id='<%#Eval("pageId") %>' class="margin-top10 article-content">
                                                <%#Eval("content") %>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div id="divPhotos">
                                    <PG:PhotoGallery runat="server" ID="ctrPhotoGallery" />
                                </div>
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
                        <BW:UpcomingBikes ID="ctrlUpcomingBikes" runat="server" />
                        <%} %>                       
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div id="back-to-top" class="back-to-top"><a><span></span></a></div>
        <!-- #include file="/includes/footerBW.aspx" -->
        <BW:ModelGallery ID="ctrlModelGallery" runat="server" />
        <link href="<%= staticUrl  %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%=  staticUrl %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl  %>/src/content/details.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $("body").floatingSocialShare();
            });
        </script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>

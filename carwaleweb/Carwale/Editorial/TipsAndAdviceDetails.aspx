<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Editorial.TipsAndAdvicesDefault" Trace="false" Debug="false" EnableEventValidation="false" ViewStateMode="Disabled" %>

<%@ Import Namespace="Carwale.UI.Common" %>

<%@ Register TagPrefix="Vspl" TagName="RepeaterPager" Src="/Controls/CommonPager.ascx" %>
<%@ Register TagPrefix="Qr" TagName="QuickResearch" Src="/Controls/QuickResearch.ascx" %>
<%@ Register TagPrefix="uc" TagName="TipsAndAdvices" Src="/Controls/TipsAndAdvices.ascx" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <asp:PlaceHolder ID="headMetaTags" runat="server"></asp:PlaceHolder>
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 44;
        Title = "Car Tips, Advice, How-To's and Do It Yourself";
        Description = " Tips, advice, how-to's and DIYs for car driving, ownership and maintenance. Know what to do and what not to around everyday car ownership and driving.";

        Revisit = "15";
        DocumentState = "Static";
        //Trace.Warn("page " + pageNumber);
        canonical = canonicalUrl;
        altUrl = altURL;

        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
        targetKey = "Accessories";
        targetValue = "TyreGuide";
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
        <% if (Ad300x600 == true)
           { %>googletag.defineSlot('<%= AdPath %>300x600', [[120, 240], [120, 600], [160, 600], [300, 250], [300, 600]], 'div-gpt-ad-<%= AdId %>-4').addService(googletag.pubads());<% } %>
            googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
            googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
 
    <script language="c#" runat="server">
        public bool Ad300x600 = true;
    </script>    
    <script   src="/static/src/jquery.colorbox.js"  type="text/javascript"></script>
    <link rel="stylesheet" href="/static/css/colorbox.css" type="text/css" >
    <script type="text/javascript" >
        $(document).ready(function () {
            $("a.cbBox").colorbox({ rel: "nofollow" });
            $("a[rel='slide']").colorbox({ width: "700px", height: "500px" });
        });
    </script>
    <style type="text/css">
.medPadding{padding:5.5px}
#ulUCL{list-style:none}
#ulUCL li{padding:0 0 10px 0}
#ulUCL li a{background:url(https://imgd.aeplcdn.com/0x0/cw-common/ul-arrow.gif) no-repeat center left;margin-left:5px;padding-left:10px}
.entry{width:630px}
    </style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom10 padding-top10 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a title="Car Research" href="/reviews-news/">Reviews & News</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a title="Car Tips & Advice" href="/tipsadvice/">Car Tips & Advice</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= subCategoryName %></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text"><%= subCategoryName %></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-8">
                    <asp:Repeater ID="rptTipsAdvices" runat="server" EnableViewState="false">
                        <ItemTemplate>
                            <div class="content-box-shadow content-inner-block-10 margin-bottom20">
                                <div id="post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>">
                                    <h4>
                                        <a href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>" rel="bookmark" title="Permanent Link to <%# DataBinder.Eval(Container.DataItem,"Title") %>">
                                            <%# DataBinder.Eval(Container.DataItem,"Title") %>
                                        </a>
                                    </h4>
                                    <div class="f-small" style="padding: 4px 0 0 0;">
                                        <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>,
                                            <abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:dd-MMM-yyyy}") %></abbr>
                                    </div>
                                    <div class="margin-top5">
                                        <%# DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString() != "" ? "<a class='cbBox' href='" + Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.DTOs.CMS.Articles.ArticleSummary)Container.DataItem).HostUrl, Carwale.Utility.ImageSizes._160X89, ((Carwale.DTOs.CMS.Articles.ArticleSummary)Container.DataItem).OriginalImgUrl)  + "'><img class='alignright size-thumbnail img-border-news' src='" + Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.DTOs.CMS.Articles.ArticleSummary)Container.DataItem).HostUrl, Carwale.Utility.ImageSizes._227X128, ((Carwale.DTOs.CMS.Articles.ArticleSummary)Container.DataItem).OriginalImgUrl)   +"' align='right' border='0' /></a>" : "" %>
                                        <%# DataBinder.Eval(Container.DataItem,"Description") %>
                                        <div class="float-rt"><a href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>">Read full article &raquo;</a></div>
                                        <div style="clear: both"></div>
                                    </div>
                                </div>
                                <div style="clear: both"></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <% if (totalPages > 1)
                       { %>
                    <div class="footerStrip" id="divStrip" align="right">
                        <Vspl:RepeaterPager ID="pagerDetails" Visible="true" runat="server" align="right"></Vspl:RepeaterPager>
                    </div>
                    <%} %>
                </div>
                <div class="grid-4">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                    <div class="content-box-shadow content-inner-block-10 margin-bottom20">
                        <h2 class="hd2">Quick Research</h2>
                        <Qr:QuickResearch ID="qrQuickResearch" runat="server" />
                    </div>
                    <div class="content-box-shadow content-inner-block-10 margin-bottom20">
                        <uc:TipsAndAdvices ID="ucTipsAndAdvices" runat="server" />
                    </div>
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 600, 10, 10, false, 4) %>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script type="text/javascript">
            Common.showCityPopup = false;
            doNotShowAskTheExpert = false;
        </script>
    </form>
</body>
</html>










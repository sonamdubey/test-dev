<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Editorial.TipsAndAdviceView" Trace="false" Debug="false" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc" TagName="TipsAndAdvices" Src="/Controls/TipsAndAdvices.ascx" %>
<%@ Register TagPrefix="Qr" TagName="QuickResearch" Src="/Controls/QuickResearch.ascx" %>
<%@ Register TagPrefix="uc" TagName="SubNavigation" Src="/Controls/subNavigation.ascx" %>
<%@ Import Namespace="Carwale.Utility" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 44;
        Title = CarComparePageTitle;
        Description = CarComparePageDesc;
        Keywords = "car tips, car advice, car how to";
        Revisit = "15";
        DocumentState = "Static";
        canonical = canonicalUrl;
        altUrl = altURL;
        AdPath = "/1017752/ReviewsNews_";
        targetKey = (BasicId == "8732" ? "TipsAdvice" : "Accessories");
        targetValue = (BasicId == "8732" ? "SaveFuel" : "TyreGuide");
        prevPageUrl = prevUrl;
        nextPageUrl = nextUrl;
    %>
     <!-- #include file="/includes/global/head-script.aspx" -->
    <meta name="twitter:card" content="summary_large_image">
<meta name="twitter:site" content="@CarWale">
<meta name="twitter:title" content="<%=tipsDetail.Title %>">
<meta name="twitter:description" content="<%=tipsDetail.Description%>">
<meta name="twitter:creator" content="@CarWale">
<meta name="twitter:image" content="<%=Carwale.Utility.ImageSizes.CreateImageUrl(tipsDetail.HostUrl,Carwale.Utility.ImageSizes._642X361,tipsDetail.OriginalImgUrl) %>">
<meta property="og:title" content="<%=tipsDetail.Title %>" />
<meta property="og:type" content="article" />
<meta property="og:url" content="<%="https://www.carwale.com" + tipsDetail.ArticleUrl %>" />
<meta property="og:image" content="<%=Carwale.Utility.ImageSizes.CreateImageUrl(tipsDetail.HostUrl,Carwale.Utility.ImageSizes._642X361,tipsDetail.OriginalImgUrl) %>" />
<meta property="og:description" content="<%=tipsDetail.Description%>" />
<meta property="og:site_name" content="CarWale" />
<meta property="article:published_time" content="<%=tipsDetail.DisplayDate.ToString("s") %>" />
<meta property="article:section" content="Car News" />
<meta property="article:tag" content="<%=string.Join(",",tipsDetail.TagsList.ToArray()) %>" />
<meta property="fb:admins" content="154881297559" />
<meta property="fb:pages" content="154881297559" />
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            <% if (Ad300x600 == true)
               { %>googletag.defineSlot('<%= AdPath %>300x600', [[120, 240], [120, 600], [160, 600], [300, 250], [300, 600]], 'div-gpt-ad-<%= AdId%>-4').addService(googletag.pubads());<% } %>
            googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
   
    <script language="c#" runat="server">
        //public string nextUrl = string.Empty, prevUrl = string.Empty, prevPageUrl = "", nextPageUrl = "";
        public bool Ad300x600 = true;
    </script>
    <link rel="stylesheet" href="/static/css/slideshow.css" type="text/css" >
    <link rel="stylesheet" href="/static/css/rt.css" type="text/css" >
    <link rel="stylesheet" href="/static/css/articles.css" type="text/css" >
    <link rel="stylesheet" type="text/css" href="https://st.aeplcdn.com/v2/css/expert-review.css?20161024032055" />
    <style>
        .imgtext {
            background-color: #f2f2f2;
            padding: 5px;
            font-size: 11px;
        }

        .menu a {
            margin: 8px;
        }
        .view-icon {
            background: url(https://imgd.aeplcdn.com/0x0/cw/static/sprites/cw-sprite_v1.3.png?28052015) no-repeat;
            display:inline-block;
            background-position: -166px -821px;
            width: 14px;
            height: 9px;
        }
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a title="Car Research" href="/reviews-news/"">Reviews & News</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a title="Comparison Test" href="/tipsadvice/">Car Tips & Advice</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= ArticleTitle%></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text"><%= ArticleTitle%></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-8 margin-top10">
                    <div class="">
                        <!--Below code for expert Review Subnavigation starts here-->
                                <div class="sub-navExpert">
                                    <uc:SubNavigation ID="SubNavigation" runat="server" IsOverviewPage="true" />
                                </div>
                                <div class="content-box-shadow content-inner-block-10 margin-top10">
                                <div class="leftfloat"><% if (!string.IsNullOrEmpty(authorMaskingName))
                                                          { %><a href="/authors/<%= authorMaskingName %>"><%= authorName %></a><%}
                                                          else
                                                          { %><%= authorName %><%} %>, <%= displayDate.ToString("dd-MMM-yyyy")%>
                                </div>
                                <div class="rightfloat"><span class="view-icon margin-right5"></span><%= views != "" ?views: "0" %> Views</div>
                                <div class="clear"></div>

                                <ul class="social margin-top10">
                                    <li style="float: left; width: 90px; height: 20px;">
                                        <iframe src="https://www.facebook.com/plugins/like.php?layout=button_count&show_faces=false&action=like&font&colorscheme=light&width=100&height=25&href=https://www.carwale.com<%= Url %>" scrolling="no" frameborder="0" style="border: none; overflow: hidden; width: 100px; height: 25px;" allowtransparency="true"></iframe>
                                    </li>
                                    <li style="float: left; width: 90px; height: 20px;">
                                        <iframe allowtransparency="true" frameborder="0" scrolling="no" src='https://platform.twitter.com/widgets/tweet_button.html?text=<%= ArticleTitle  %>&via=CarWale&url=https://www.carwale.com<%= Url %>&counturl=https://www.carwale.com<%= Url %>' style="width: 110px; height: 40px;"></iframe>
                                    </li>
                                    <li style="float: left; width: 90px; height: 20px;">
                                        <div class="g_plus">
                                            <!-- Place this tag where you want the +1 button to render -->
                                            <g:plusone size="medium" href='https://www.carwale.com<%= Url %>' count="true"></g:plusone>
                                            <!-- Place this tag after the last plusone tag -->
                                            <script type="text/javascript">
                                                (function () {
                                                    var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                                    po.src = 'https://apis.google.com/js/plusone.js';
                                                    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                                                })();
                                            </script>
                                        </div>
                                    </li>
                                </ul>

                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                            <div class="content-box-shadow content-inner-block-10 margin-top10">
                                
                                <!--Below code for expert Review Subnavigation ends here-->
                                <div id="expert-review">
                                    <asp:Repeater ID="rptTips" runat="server">
                                        <ItemTemplate>
                                            <div id='<%# Format.RemoveSpecialCharacters( DataBinder.Eval( Container.DataItem, "PageName" ).ToString() )%>' class="btm-border">
                                                <h2><%# DataBinder.Eval( Container.DataItem, "PageName" ).ToString() %> </h2>
                                                <div class="text-area-right">
                                                    <%# DataBinder.Eval( Container.DataItem, "Content" ).ToString() %>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <%--for photos code starts here--%>
                                    <% if (ShowGallery)
                                       {%>
                                    <div id="photos" class="content-box-shadow content-inner-block-10 margin-top10">
                                        <h2>Photos</h2>
                                        <div>
                                            <asp:DataList ID="dlstPhoto" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="top">
                                                <ItemTemplate>
                                                    <a rel="slidePhoto" class="cboxElement" target="_blank" href="<%# DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() +"725x408" +DataBinder.Eval( Container.DataItem, "OriginalImgPath" ).ToString() %>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ).ToString() %></b>" />
                                                    <img alt="<%# DataBinder.Eval( Container.DataItem, "ImageCategory" ).ToString() %>" border="0" style="margin: 0px 45px 10px 0px; cursor: pointer;" src="<%# DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() +"144x81" +DataBinder.Eval( Container.DataItem, "OriginalImgPath" ).ToString() %>" title="Click to view larger photo" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                    </div>
                </div>
                <div class="grid-4">
                        <div class="bg-white">
                            <div>
                                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                            </div>
                                <div class="content-box-shadow content-inner-block-10 margin-top10">
                                    <h2>Quick Research</h2>
                                    <Qr:QuickResearch ID="qrQuickResearch" runat="server" />
                                </div>
                                <div class="content-box-shadow content-inner-block-10 margin-top10">
                                    <uc:TipsAndAdvices ID="ucTipsAndAdvices" runat="server" />
                                </div>
                                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 600, 10, 10, false, 4) %>
                        </div>
                    </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
    <script   src="/static/src/jquery.colorbox.js"  type="text/javascript"></script>
    <link rel="stylesheet" href="/static/css/colorbox.css" type="text/css" >
    <script language="javascript" type="text/javascript">
        Common.showCityPopup = false;
        doNotShowAskTheExpert = false;
        $(document).ready(function () {
            $("a[rel='slidePhoto']").colorbox();
        });
    </script>
</body>
</html>








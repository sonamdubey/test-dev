<%@ Page Trace="false" Debug="false" Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Editorial.PitStop" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Register TagPrefix="Vspl" TagName="RepeaterPager" Src="/Controls/CommonPager.ascx" %>
<%@ Register TagPrefix="uc" TagName="SubscriptionControl" Src="/Controls/SubscriptionControl.ascx" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%	
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 19;
        Title = "PitStop";
        Description = "Pit Stop";
        Keywords = "social,pitstop";
        Revisit = "5";
        DocumentState = "Static";
        canonical = "https://www.carwale.com/pitstop/";
        altUrl = "https://www.carwale.com/m/pitstop/";
        prevPageUrl = prevUrl;
        nextPageUrl = nextUrl;
        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
        Ad300BTF = true;
        Ad600Bottom = true;
    %>
    <script language="c#" runat="server">
        private bool Ad300BTF , Ad600Bottom ;
    </script>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            <% if (Ad300BTF == true)
               { %>googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads()); <% } %>
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            <% if (Ad600Bottom == true)
               { %>googletag.defineSlot('/7590/CarWale_ReviewsNews/CarWale_ReviewsNews_News/CarWale_ReviewsNews_News_300x600', [[300, 600], [300, 250], [160, 600], [120, 600], [120, 240]], 'div-gpt-ad-1361344117940-2').addService(googletag.pubads()); <% } %>
            googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
   
    <style>
.news-sprite{background:url(https://img.carwale.com/images/news-sprite.png) no-repeat;display:inline-block;position:relative}
.v-views-icon{background-position:0 -70px;top:6px}
.v-views-icon{width:16px;height:15px;margin-right:5px}
    </style>
    <link rel="stylesheet" href="/static/css/colorbox.css" type="text/css" >
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/reviews-news/">Reviews & News</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Pit Stop</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10 special-skin-text"><%= NewsTitle %></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="container">
                <div class="grid-8">
                    
                    <asp:Repeater ID="rptPitstop" runat="server" EnableViewState="false">
                        <ItemTemplate>
                            <div class="content-box-shadow content-inner-block-10 margin-bottom20">
                                <div id='post-<%# DataBinder.Eval(Container.DataItem,"BasicId") %>'>
                                    <h2 class="splh2 font16">
                                        <a class="news-title" href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>"
                                            rel="bookmark" title="<%# DataBinder.Eval(Container.DataItem,"Title") %>">
                                            <%# DataBinder.Eval(Container.DataItem,"Title") %>
                                        </a>
                                    </h2>
                                    <div class="f-small font11 margin-bottom10 margin-top10 font12" style="padding: 4px 0 0 0;">
                                        <abbr><%# DataBinder.Eval(Container.DataItem,"DisplayDate", "{0:f}") %></abbr>
                                        by 
								                    <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
                                        <div class="rightfloat Margin-10 margin-top10"><a href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>#fb-comments"><%# DataBinder.Eval(Container.DataItem,"FacebookCommentCount").ToString() != "" ?DataBinder.Eval(Container.DataItem,"FacebookCommentCount"):'0' %> Comment(s)</a></div>
                                    </div>                                    
                                    
                                    <div class="clear" style="border-top: 1px dotted #f0f0f0; margin-top10"></div>
                                    <div class="margin-top10 margin-bottom10">
                                        <%# DataBinder.Eval(Container.DataItem, "HostUrl")+ DataBinder.Eval(Container.DataItem,"OriginalImgUrl").ToString() != "" ?"<a class='cbBox' href='" + DataBinder.Eval(Container.DataItem, "HostUrl") + "664x374" + DataBinder.Eval(Container.DataItem,"OriginalImgUrl") + "'> <img class='lazy' data-original='" + DataBinder.Eval(Container.DataItem, "HostUrl") + "174x98" + DataBinder.Eval(Container.DataItem,"OriginalImgUrl") +"' title='" + DataBinder.Eval(Container.DataItem,"Title") + "' alt='" + DataBinder.Eval(Container.DataItem,"Title") + "' src='https://imgd.aeplcdn.com/0x0/statics/grey.gif' align='right' border='0' /></a>" : "" %>
                                        <%# DataBinder.Eval(Container.DataItem, "Description") %>
                                        <div class="clear"></div>
                                    </div>
                                    <p><a href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>">Read the rest of this entry &raquo;</a></p>
                                    <p class="postmetadata margin-top10">
                                        <a class="redirect-rt rightfloat" href="<%# DataBinder.Eval(Container.DataItem,"ArticleUrl") %>#addcomment">Leave a Comment</a>
                                        <span class="news-sprite v-views-icon"></span><%# DataBinder.Eval(Container.DataItem,"Views") %> views
                                    </p>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%--  <div class="footerStrip" id="divStrip" align="right" Visible="false" runat="server"></div>	--%>
                    <div class="margin-bottom20" id="divStrip" align="right">
                        <Vspl:RepeaterPager ID="pagerDetails" Visible="true" runat="server" align="right"></Vspl:RepeaterPager>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="grid-4">
                    <!-- #include file="/includes/sidebar.aspx" -->
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1361344117940", 300, 600, 0, 20, false, 2) %>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->

        <script  type="text/javascript"  src="/static/src/jquery.colorbox.js" ></script>   
        <script type="text/javascript">            
            $("a[rel='slide']").colorbox({ width: "700px", height: "500px" });
        </script>
        <script type="text/javascript">
            Common.showCityPopup = false;
            doNotShowAskTheExpert = false;
            $(document).ready(function () {
                $("a.cbBox").colorbox({ rel: "nofollow" })
            });            
        </script>                
    </form>
</body>
</html>

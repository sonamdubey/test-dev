<%@ Page Trace="false" Debug="false" Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Authors.AuthorDetails" %>

<%@ Register TagPrefix="wn" TagName="NewsRightWidget" Src="/Controls/NewsRightWidget.ascx" %>
<%@ Register TagPrefix="wv" TagName="PopularVideoWidget" Src="/Controls/PopularVideoWidget.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 16;
        Title = authorName;
        Description = "Author details.";
        Keywords = "news, car news, auto news, latest car news, indian car news, car news of india , author";
        Revisit = "5";
        DocumentState = "Static";
        altUrl = altURL;
        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <link rel="stylesheet" href="/static/css/author.css" type="text/css" >
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90], [960, 60], [970, 60]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>300x600', [[120, 240], [120, 600], [160, 600], [300, 250], [300, 600]], 'div-gpt-ad-<%= AdId %>-9').addService(googletag.pubads());
        googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");

        //googletag.pubads().enableSyncRendering();
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
    </script>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <!-- #include file="/includes/header.aspx" -->
    <section class="container">
        <div class="grid-12 padding-bottom10">
            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
        </div>
    </section>
    <div class="clear"></div>
    <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
        <div class="container">
            <div class="grid-12 ">
                <div class="breadcrumb">
                    <ul class="special-skin-text">
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a title="Authors" href="/authors/">Authors</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><%=authorName %></li>
                    </ul>
                </div>
                <h1 class="font30 text-black special-skin-text"><%=authorName %>, <%=designation %></h1>
                <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
             <div class="container ">
                <!-- Left side code starts here-->
                <div class="grid-8">
                    <div class="content-inner-block-10 content-box-shadow margin-bottom20">
                        <!--Author content starts here-->
                        <div class="author-box">
                            <p>
                                <img align="left" src="https://<%=hostUrl %>/<%=profileImage %>/<%=imageName %>">
                                <%=fullDescription %>
                            </p>
                            <div class="clear"></div>
                            <div class="email-box">
                                <p>Email id: <a href="mailto:<%=emailId %>" target="_blank"><%=emailId %></a></p>
                            </div>
                            <div class="social-icon-box">
                                <ul>
                                    <li><span>Catch me on:</span></li>
                                    <%if (facebookProfile != "")
                                        { %>
                                    <li><a href="<%=facebookProfile %>" class="author-sprite f-large-icon" target="_blank"></a></li>
                                    <%} %>
                                    <%if (googlePlusProfile != "")
                                        { %>
                                    <li><a href="<%=googlePlusProfile %>" class="author-sprite g-large-icon" target="_blank"></a></li>
                                    <%} %>
                                    <%if (linkedInProfile != "")
                                        { %>
                                    <li><a href="<%=linkedInProfile %>" class="author-sprite in-large-icon" target="_blank"></a></li>
                                    <%} %>
                                    <%if (twitterProfile != "")
                                        { %>
                                    <li><a href="<%=twitterProfile %>" class="author-sprite t-large-icon" target="_blank"></a></li>
                                    <%} %>
                                </ul>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <!--Author content ends here-->

                        <%if (rptExpertReview.Items.Count > 0)
                            { %>
                        
                            <!--Expert Reviews code starts here-->
                            <h2>Expert Reviews</h2>
                            <div class="author-bullets">
                                <ul>
                                    <asp:Repeater ID="rptExpertReview" runat="server">
                                        <ItemTemplate>
                                            <li><a href="/<%# FormURL(((Carwale.Entity.Author.ExpertReviews)Container.DataItem).CategoryId.ToString(),((Carwale.Entity.Author.ExpertReviews)Container.DataItem).MakeName,((Carwale.Entity.Author.ExpertReviews)Container.DataItem).MaskingName,((Carwale.Entity.Author.ExpertReviews)Container.DataItem).Url,((Carwale.Entity.Author.ExpertReviews)Container.DataItem).BasicId.ToString()) %>"><%#((Carwale.Entity.Author.ExpertReviews)Container.DataItem).Title %></a></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <!--Expert Reviews code ends here-->
                        
                        <%} %>

                        <%if (rptNewsList.Items.Count > 0)
                            { %>
                        <%if (rptExpertReview.Items.Count == 0)
                            { %>
                        <div class="author-highlight first">
                            <%} %>
                            <%else
                            {%>
                            <div class="author-highlight">
                                <%} %>
                                <!--News code starts here-->
                                <div>
                                    <h2>News</h2>
                                    <div class="author-bullets">
                                        <ul>
                                            <asp:Repeater ID="rptNewsList" runat="server">
                                                <ItemTemplate>
                                                    <li><a href="<%# Eval("Url") %>"> <%#((Carwale.Entity.Author.NewsEntity)Container.DataItem).Title %></a></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                                <!--News code ends here-->
                            </div>
                            <%} %>
                        </div>
                        <div class="content-box-shadow content-inner-block-10">
                            <h2>Other Authors</h2>
                                <div class="jcarousel-wrapper" style="width: 622px;">
                                    <div class="jcarousel authors-gallery">
                                        <ul>
                                            <asp:Repeater ID="rptOtherAuthors" runat="server">
                                            <ItemTemplate>
                                            <li class="card" style="width:187px;">
                                                <div class="front">
                                                    <div class="contentWrapper">
                                                        <div class="imageWrapper">
                                                            <a href="/authors/<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).MaskingName %>">
                                                                <img src="https://<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).HostUrl %>/<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).ProfileImage %>/<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).ImageName %>" alt="<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).AuthorName %>" title="<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).AuthorName %>">
                                                            </a>
                                                        </div>
                                                        <div class="carDescWrapper">
                                                            <div class="carTitle margin-bottom20">
                                                                <h3><a href="/authors/<%#((Carwale.Entity.Author.AuthorList)Container.DataItem).MaskingName %>"><%#((Carwale.Entity.Author.AuthorList)Container.DataItem).AuthorName %></a></h3>
                                                            </div>
                                                            <div class="margin-bottom20 font14 text-light-grey"><span><%#((Carwale.Entity.Author.AuthorList)Container.DataItem).Designation%></span></div>                                                            
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </ul>
                                    </div>
                                    <span class="jcarousel-control-left"><a href="#" class="cwsprite jcarousel-control-prev"></a></span>
                                    <span class="jcarousel-control-right"><a href="#" class="cwsprite jcarousel-control-next"></a></span>
                                </div>
                        </div>
                        <!--Other Authors code starts here -->                        
                        <!-- Left side code ends here -->
                </div>
                </div>
                <!-- right side code starts here -->
                <div class="grid-4">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                    <div class="content-box-shadow margin-bottom20">
                    <wn:NewsRightWidget ID="ctrlNewsRightWidget" runat="server" />
                    </div>
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 0, 20, false, 1) %>
                    <wv:PopularVideoWidget ID="ctrlPopularVideoWidget" runat="server" />
                    <div class="clear"></div>
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 600, 20, 0, false, 9) %>
                </div>
                <div class="clear"></div>
                <!-- right side code ends here- -->
            </div>
        </div>        
    </section>
    <div class="clear"></div>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    
    <style>
        .jcarousel-wrapper .authors-gallery ul li { height: 175px; }
    </style>
    <script type="text/javascript">
        $(document).ready(function (e) {
            //for popular and Upcoming cars tabs
            $("#cars-tabs li h2").click(function () {
                $(".cars-tabs-data").hide();
                $(".cars-tabs-data").eq($(this).parent().index()).show();
                $("#cars-tabs li h2").removeClass("active");
                $(this).addClass("active");
            });
            $("#news-tabs li h2").click(function () {
                $(".news-tab-data").hide();
                $(".news-tab-data").eq($(this).parent().index()).show();
                $("#news-tabs li h2").removeClass("active");
                $(this).addClass("active");
            });
            //for other author carousel
            //var carousel = $('#mainSlider').data('jcarousel');
            $("#authorCarousel").jcarousel({
                scroll: 2,
                auto: 0,
                animation: 800,
                wrap: "circular",
                initCallback: initCallbackUCR, buttonNextHTML: null, buttonPrevHTML: null
            });

            function initCallbackUCR(carousel) {
                $('#list_carousel_widget_prev').bind('click', function () {
                    carousel.prev();
                    return false;
                });

                $('#list_carousel_widget_next').bind('click', function () {
                    carousel.next();
                    return false;
                });
            };
        });
    </script>
</body>
</html>

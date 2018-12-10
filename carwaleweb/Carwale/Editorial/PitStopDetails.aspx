<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Editorial.PitStopDetails" Trace="false" Debug="false" %>

<%@ Register TagPrefix="uc" TagName="SubscriptionControl" Src="/Controls/SubscriptionControl.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
    
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 19;
        Title = title + "-CarWale";
        Description = "Pit Stop";
        Keywords = "";
        Revisit = "5";
        DocumentState = "Static";
        AdId = "1396440332273";
    AdPath          = "/1017752/ReviewsNews_";
    AdTargeting     =  tag;
    fbImgPath       = ImagePath;
    fbTitle         = Title;
    canonical = "https://www.carwale.com" + url;
    altUrl = "https://www.carwale.com/m" + url;
    %>
    <script language="c#" runat="server">
        private string AdTargeting, fbImgPath, fbTitle;
        private bool Ad300BTF = false, Ad600Bottom = false;  // variables for google ad script
    </script>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
                <% if (Ad300BTF == true) { %>googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads()); <% } %>
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
    <meta name="twitter:card" content="summary_large_image">
    <meta name="twitter:site" content="@CarWale">
    <meta name="twitter:title" content="<%=pitStopDetails.Title %>">
    <meta name="twitter:description" content="<%=pitStopDetails.Description%>">
    <meta name="twitter:creator" content="@CarWale">
    <meta name="twitter:image" content="<%=Carwale.Utility.ImageSizes.CreateImageUrl(pitStopDetails.HostUrl,Carwale.Utility.ImageSizes._642X361,pitStopDetails.OriginalImgUrl) %>">
    <meta property="og:title" content="<%=pitStopDetails.Title %>" />
    <meta property="og:type" content="article" />
    <meta property="og:url" content="<%="https://www.carwale.com" + pitStopDetails.ArticleUrl %>" />
    <meta property="og:image" content="<%=Carwale.Utility.ImageSizes.CreateImageUrl(HostUrl,Carwale.Utility.ImageSizes._642X361,pitStopDetails.OriginalImgUrl) %>" />
    <meta property="og:description" content="<%=pitStopDetails.Description%>" />
    <meta property="og:site_name" content="CarWale" />
    <meta property="article:published_time" content="<%=pitStopDetails.DisplayDate.ToString("s") %>" />
    <meta property="article:section" content="Car News" />
    <meta property="article:tag" content="<%=string.Join(",",pitStopDetails.TagsList.ToArray()) %>" />
    <meta property="fb:admins" content="154881297559" />
    <meta property="fb:pages" content="154881297559" />
    <link rel="stylesheet" type="text/css" href="https://st.aeplcdn.com/v2/css/cw-news.css?2016052105455" />
    <script src="https://cdn.topsy.com/topsy.js?20160419032055" type="text/javascript"></script>
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
                    <div class="breadcrumb">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New Cars</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/reviews-news/">Reviews & News</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a title="/Pit Stop/" href="/pitstop/">Pit Stop</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= title%></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10 special-skin-text"><%= title%></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
            </div>

            <div class="container padding-top10">
                <div class="grid-8">
                    <div class="content-box-shadow content-inner-block-10 margin-bottom10">
                        <div id="post-<%= pitstopId%>">
                            
                            <div class="f-small" style="padding: 4px 0 0 0;">
                                <abbr><%= displayDate%></abbr>
                                by <%if (!string.IsNullOrEmpty(authorMaskingName))
                                     { %> <a href="/authors/<%= authorMaskingName %>"><%=authorName%> </a><%}
                                     else
                                     { %> <%=authorName%> <%} %></div>
                            <div class="clear"></div>
                            <ul class="social">
                                <li>
                                    <fb:like href="https://carwale.com<%= url%>" send="false" layout="button_count" width="80" show_faces="false"></fb:like>
                                </li>
                                <li>
                                    <iframe allowtransparency="true" frameborder="0" scrolling="no" src="https://platform.twitter.com/widgets/tweet_button.html?text=<%=title %>&url=https://www.carwale.com<%=url %>&counturl=https://www.carwale.com<%=url %>&via=CarWale" style="width: 110px; height: 40px;"></iframe>
                                </li>
                                <li>
                                    <div class="g_plus">
                                        <!-- Place this tag where you want the +1 button to render -->
                                        <g:plusone size="medium" href="https://www.carwale.com<%= url%> count="true"></g:plusone>
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

                            <div class="rightfloat"><a style="font-size: 13px;" href="<%=url %>#fb-comments"><%= FbCommentCount != "0" ?FbCommentCount: "Be the first to " %><%= FbCommentCount != "0" ?"comment(s)": "comment" %></a></div>
                            <div class="clear" style="border-top: 1px dotted #f0f0f0;"></div>
                            <div class="margin-top10">
                                <%= OriginalImgUrl != "" ? (MainImgCaption != "" ? "<div style='text-align:center;'><img style='margin-top:10px; border:none; background-color:#fff;' title='" + title + "' alt='" + title + "' class='margin-top10 size-thumbnail img-border-news' src='" + HostUrl + "566x318" + OriginalImgUrl + "' border='0' /></div><div style='background-color:#f2f2f2; font-size:11px; color:#666;  text-align:left;'>" + MainImgCaption + "</div>" : "<div style='text-align:center;'><img alt='loading' class='margin-top10 size-thumbnail img-border-news' src='" + HostUrl + "642x361" + OriginalImgUrl + "' border='0' /></div>")  : "" %>
                                <%= content %>
                                <div class="clear"></div>
                            </div>
                            <p class="postmetadata hide">
                                <span class="redirect-rt">
                                    <fb:comments-count href='https://<%=Request.ServerVariables["HTTP_HOST"] %><%=url %>'></fb:comments-count>
                                    Comments</span>
                                <div class="clear"></div>
                            </p>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <% if (prevId != "0")
                       { %>
                    <div class="content-inner-block-10 leftfloat">
                        <p class="margin-bottom5"><a href="<%=prevNewsUrl %>" style="text-decoration: none;"><span class="prev-arrow"></span><b>Previous Article</b></a></p>
                        <p class="margin-bottom10"><a href="<%=prevNewsUrl %>" style="color: #666; text-decoration: underline;"><%=prevNewsTitle %></a></p>
                    </div>
                    <%} %>
                    <% if (nextId != "0")
                       { %>
                    <div class="content-inner-block-10 rightfloat">
                        <p class="margin-bottom5 text-right"><a href="<%=nextNewsUrl %>" style="text-decoration: none;"><b>Next Article</b> <span class="next-arrow"></span></a></p>
                        <p class="margin-bottom10"><a href="<%=nextNewsUrl %>" style="color: #666; text-decoration: underline;"><%=nextNewsTitle %></a></p>
                    </div>
                    <% } %>
                    <div class="clear"></div>
                    <div class="content-box-shadow content-inner-block-10 margin-bottom10">
                        <div id="fb-comments" class="clear">
                            <div class="fb-comments" data-href="https://<%=Request.ServerVariables["HTTP_HOST"] %><%=url %>" data-num-posts="2" data-width="620"></div>
                        </div>
                    </div>
                </div>
                <div class="grid-4">
                    <!-- #include file="/includes/sidebar.aspx" -->
                </div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <!-- Fb comments script -->
        <script>
            Common.showCityPopup = false;
            doNotShowAskTheExpert = false;
            (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id)) return;
                js = d.createElement(s); js.id = id;
                js.src = "//connect.facebook.net/en_US/sdk.js";
                fjs.parentNode.insertBefore(js, fjs);
            }(document, 'script', 'facebook-jssdk'));

            $(document).ready(function () {
                window.fbAsyncInit = function () {
                    FB.init({
                        appId: FACEBOOKAPPID,
                        cookie: true,  // enable cookies to allow the server to access 
                        // the session
                        xfbml: true,  // parse social plugins on this page
                        version: 'v2.2' // use version 2.2
                    });
                }

                window.fbAsyncInit();
            });
        </script>
        <!--END Fb comments script -->
        <script type="text/javascript">
            function entersocialcount(likecount, commentcount, url) {
                var basicid = url.split('-')[0].split('/')[4];
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/CarwaleAjax.AjaxNews,Carwale.ashx",
                    data: '{"likeCount":"' + likecount + '","commentCount":"' + commentcount + '","Id":"' + basicid + '","categoryid":"' + 12 + '"}',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("X-AjaxPro-Method", "SocialPluginCount")
                    },
                    success: function (response) {
                    }
                });
            };
                $(window).load(function () {
                    try {
                        var basicId = window.location.pathname.split('-').pop().split('/')[0];
                        if (window.localStorage && basicId != undefined) {
                            if (localStorage.BasicId != undefined && localStorage.BasicId.split('|').indexOf(basicId) == -1)
                                localStorage.BasicId += "|" + basicId;
                            else localStorage.BasicId = basicId;
                        }
                    }
                      catch (e) {
                        console.log(e);
                    }
                }); 
        </script>
    </form>
</body>
</html>

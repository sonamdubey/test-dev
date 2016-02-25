<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.m.videos.video" %>
<!DOCTYPE html>
<%@ Register Src="~/m/controls/SimilarVideos.ascx" TagName="SimilarVideos" TagPrefix="BW" %>
<html>
<head>
    <%   
        title = metaTitle;
        description = metaDesc;
        keywords = metaKeywords;
        canonical = string.Format("http://www.bikewale.com/bike-videos/{0}-{1}/", videoModel.VideoTitleUrl, videoModel.BasicId);
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        .padding20 { padding:15px 20px; }
        #embedVideo { max-width:476px; width:100%; height:180px; margin:0 auto; }
        #embedVideo iframe { width:100%; }
        .bottom-shadow { -webkit-box-shadow:0 2px 2px #ccc; -moz-box-shadow:0 2px 2px #ccc; box-shadow:0 2px 2px #ccc; }
        .text-default { color:#4d5057; }
        .text-xlight-grey { color:#a8afb3; }
        .line-height18 { line-height:1.8; }
        .video-views-count-container { min-width:138px; }
        .video-views-icon { width:17px; height:13px; background-position:-59px -303px; }
        .video-likes-icon { width:15px; height:15px; background-position:-63px -322px; position:relative; top:2px; }
        .border-light-right { border-right:1px solid #e2e2e2; }
        .border-light-top { border-top:1px solid #e2e2e2; }
        .social-wrapper li { width:38px; height:24px; margin-right:10px; float:left; }
        .social-wrapper a { width:100%; height:100%; display:block; }
        .whatsapp-container { background: -webkit-linear-gradient(to bottom, #49c633, #40ad2c); background: -o-linear-gradient(to bottom, #49c633, #40ad2c); background: -moz-linear-gradient(to bottom, #49c633, #40ad2c); background: linear-gradient(to bottom, #49c633, #40ad2c); }
        .fb-container { background: -webkit-linear-gradient(to bottom, #5171ba, #3b599c); background: -o-linear-gradient(to bottom, #5171ba, #3b599c); background: -moz-linear-gradient(to bottom, #5171ba, #3b599c); background: linear-gradient(to bottom, #5171ba, #3b599c); }
        .tweet-container { background: -webkit-linear-gradient(to bottom, #28c0ff, #1babe7); background: -o-linear-gradient(to bottom, #28c0ff, #1babe7); background: -moz-linear-gradient(to bottom, #28c0ff, #1babe7); background: linear-gradient(to bottom, #28c0ff, #1babe7); }
        .gplus-container { background: -webkit-linear-gradient(to bottom, #ff533f, #dd2a16); background: -o-linear-gradient(to bottom, #ff533f, #dd2a16); background: -moz-linear-gradient(to bottom, #ff533f, #dd2a16); background: linear-gradient(to bottom, #ff533f, #dd2a16); }
        .mail-container { background: -webkit-linear-gradient(to bottom, #c6c6c6, #acacac); background: -o-linear-gradient(to bottom, #c6c6c6, #acacac); background: -moz-linear-gradient(to bottom, #c6c6c6, #acacac); background: linear-gradient(to bottom, #c6c6c6, #acacac); }
        .social-icons-sprite { background: url(/m/images/social-icons-sprite.png) no-repeat; display: inline-block; }
        .whatsapp-icon, .fb-icon, .tweet-icon, .gplus-icon, .mail-icon { height:14px; position:relative; top:4px; }
        .whatsapp-icon { width:12px; background-position:0 0; }
        .fb-icon { width:8px; background-position:-22px 0; }
        .tweet-icon { width:16px; background-position:-39px 0; }
        .gplus-icon { width:14px; background-position:-65px 0; }
        .mail-icon { width:16px; background-position:-89px 0; }
        .swiper-slide.video-carousel-content { width:270px; min-height:290px; background:#fff; border:1px solid #e2e2e2; }
        .swiper-slide img { width:100%; }
        .video-carousel-image { width:100%; height:157px; display: table; overflow: hidden; text-align: center; position:relative; }
        .video-carousel-image a { width:100%; display: table-cell; vertical-align: middle; }
        .video-carousel-desc { padding:10px; }
        .more-videos-link { display:block; margin-top:15px; margin-bottom:15px; }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% if(videoModel!= null) { %>
        <section class="bg-white">
            <div class="container bottom-shadow">
                <div id="embedVideo">
                    <iframe height="180" src="<%=videoModel.VideoUrl %>&autoplay=1" frameborder="0" allowfullscreen></iframe>
                </div>
                <div class="padding20">
                    <h1 class="font18"><%=videoModel.VideoTitle %></h1>
                    <p class="font12 margin-top10 margin-bottom10 text-xlight-grey"><%=videoModel.DisplayDate %></p>
                    <p class="font14 text-light-grey line-height18 margin-bottom10"><%=videoModel.Description %></p>
                    <div class="video-views-count-container font14 leftfloat padding-right10 border-light-right">
                            <span class="bwmsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default"><%=videoModel.Views %></span>
                    </div>
                    <div class="video-views-count-container font14 leftfloat padding-left10 padding-right10">
                        <span class="bwmsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default"><%=videoModel.Likes %></span>
                    </div>
                    <div class="clear"></div>
                    <div class=""></div>
                    <p class="padding-top15 margin-top20 margin-bottom15 font14 text-light-grey border-light-top">Share this story</p>
                    <ul class="social-wrapper">
                        <li class="whatsapp-container rounded-corner2 text-center">
                            <a href=""><span class="social-icons-sprite whatsapp-icon"></span></a>
                        </li>
                        <li class="fb-container rounded-corner2 text-center">
                            <a href=""><span class="social-icons-sprite fb-icon"></span></a>
                        </li>
                        <li class="tweet-container rounded-corner2 text-center">
                            <a href=""><span class="social-icons-sprite tweet-icon"></span></a>
                        </li>
                        <li class="gplus-container rounded-corner2 text-center">
                            <a href=""><span class="social-icons-sprite gplus-icon"></span></a>
                        </li>
                        <li class="mail-container rounded-corner2 text-center">
                            <a href=""><span class="social-icons-sprite mail-icon"></span></a>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
        </section>

        <section class="margin-bottom30 <%= (ctrlSimilarVideos.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
           <BW:SimilarVideos ID="ctrlSimilarVideos" runat="server" />      
        </section>
        <% } %>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->    
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.Videos.Video" %>

<!DOCTYPE html>
<%@ Register Src="~/m/controls/SimilarVideos.ascx" TagName="SimilarVideos" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/m/controls/GenericBikeInfoControl.ascx" %>
<html>
<head>
    <%   
        title = metaTitle;
        description = metaDesc;
        keywords = metaKeywords;
        canonical = string.Format("https://www.bikewale.com/bike-videos/{0}-{1}/", videoModel.VideoTitleUrl, videoModel.BasicId);
    %>
    <meta property="og:title" content="<%=metaTitle%>" />
    <meta property="og:image" content="<%=string.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg", videoModel.VideoId)%>" />
    <!-- #include file="/includes/headscript_mobile.aspx" -->

    <style type="text/css">
        @charset "utf-8";.padding20{padding:15px 20px}#embedVideo{max-width:476px;width:100%;height:180px;margin:0 auto}#embedVideo iframe{width:100%}.bottom-shadow{-webkit-box-shadow:0 2px 2px #ccc;-moz-box-shadow:0 2px 2px #ccc;box-shadow:0 2px 2px #ccc}.text-default{color:#4d5057}.text-xlight-grey{color:#a8afb3}.line-height18{line-height:1.8}.video-views-count-container{min-width:138px}.border-light-right{border-right:1px solid #e2e2e2}.border-light-top{border-top:1px solid #e2e2e2}.social-wrapper li{width:38px;height:24px;margin-right:10px;float:left}.social-wrapper a{width:100%;height:100%;display:block}.whatsapp-container{background:-webkit-linear-gradient(to bottom, #49c633, #40ad2c);background:-o-linear-gradient(to bottom, #49c633, #40ad2c);background:-moz-linear-gradient(to bottom, #49c633, #40ad2c);background:linear-gradient(to bottom, #49c633, #40ad2c)}.fb-container{background:-webkit-linear-gradient(to bottom, #5171ba, #3b599c);background:-o-linear-gradient(to bottom, #5171ba, #3b599c);background:-moz-linear-gradient(to bottom, #5171ba, #3b599c);background:linear-gradient(to bottom, #5171ba, #3b599c)}.tweet-container{background:-webkit-linear-gradient(to bottom, #28c0ff, #1babe7);background:-o-linear-gradient(to bottom, #28c0ff, #1babe7);background:-moz-linear-gradient(to bottom, #28c0ff, #1babe7);background:linear-gradient(to bottom, #28c0ff, #1babe7)}.gplus-container{background:-webkit-linear-gradient(to bottom, #ff533f, #dd2a16);background:-o-linear-gradient(to bottom, #ff533f, #dd2a16);background:-moz-linear-gradient(to bottom, #ff533f, #dd2a16);background:linear-gradient(to bottom, #ff533f, #dd2a16)}.mail-container{background:-webkit-linear-gradient(to bottom, #c6c6c6, #acacac);background:-o-linear-gradient(to bottom, #c6c6c6, #acacac);background:-moz-linear-gradient(to bottom, #c6c6c6, #acacac);background:linear-gradient(to bottom, #c6c6c6, #acacac)}.social-icons-sprite{background:url(https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/social-icons-sprite.png) no-repeat;display:inline-block}.whatsapp-icon,.fb-icon,.tweet-icon,.gplus-icon,.mail-icon{height:14px;position:relative;top:4px}.whatsapp-icon{width:12px;background-position:0 0}.fb-icon{width:8px;background-position:-22px 0}.tweet-icon{width:16px;background-position:-39px 0}.gplus-icon{width:14px;background-position:-65px 0}.mail-icon{width:16px;background-position:-89px 0}.swiper-slide.video-carousel-content{width:270px;min-height:290px;background:#fff;border:1px solid #e2e2e2}.swiper-slide img{width:100%}.video-carousel-image{width:100%;height:157px;display:table;overflow:hidden;text-align:center;position:relative}.video-carousel-image a{width:100%;display:table-cell;vertical-align:middle}.video-carousel-desc{padding:10px}.more-videos-link{display:block;margin-top:15px;margin-bottom:15px}.video-views-count-container{min-width:90px}.powerdrift-sub-btn{position:relative;top:-2px}
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <% if (videoModel != null)
           { %>
        <section class="bg-white">
            <div class="container bottom-shadow">
                <div id="embedVideo">
                    <iframe height="180" src="<%=videoModel.VideoUrl %>&autoplay=1" frameborder="0" allowfullscreen></iframe>
                </div>
                <div class="padding20">
                    <h1 class="font18"><%=videoModel.VideoTitle %></h1>
                    <p class="font12 margin-top10 margin-bottom10 text-xlight-grey"><%=videoModel.DisplayDate %></p>
                    <div class="font14 text-light-grey line-height18 margin-bottom10"><%=videoModel.Description %></div>
                    <div class="video-views-count-container font14 leftfloat border-light-right">
                        <span class="bwmsprite video-views-icon margin-right5"></span><span class="text-default comma"><%=videoModel.Views %></span>
                    </div>
                    <div class="video-views-count-container font14 leftfloat padding-left10 border-light-right">
                        <span class="bwmsprite video-likes-icon margin-right5"></span><span class="text-default comma"><%=videoModel.Likes %></span>
                    </div>
                    <div class="font14 leftfloat padding-left10 powerdrift-sub-btn">
                        <script src="https://apis.google.com/js/platform.js"></script>
                        <div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="default" data-count="hidden"></div>
                    </div>
                    <div class="clear"></div>
                    <div class=""></div>
                 
                    <p class="padding-top15 margin-top20 margin-bottom15 font14 text-light-grey border-light-top">Share this story</p>
                    <ul class="social-wrapper">
                        <li class="whatsapp-container rounded-corner2 text-center share-btn" data-attr="wp">
                            <span data-text="share this video" data-link="www.google.com" class="social-icons-sprite whatsapp-icon"></span>
                        </li>
                        <li class="fb-container rounded-corner2 text-center share-btn" data-attr="fb">
                            <span class="social-icons-sprite fb-icon"></span>
                        </li>
                        <li class="tweet-container rounded-corner2 text-center share-btn" data-attr="tw">
                            <span class="social-icons-sprite tweet-icon"></span>
                        </li>
                        <li class="gplus-container rounded-corner2 text-center  share-btn" data-attr="gp">
                            <span class="social-icons-sprite gplus-icon"></span>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
              <section>
                 <div class="padding-15-20 section-bottom-margin margin-bottom10">
                     <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />
                 </div>
             </section>
        </section>

        <section class="margin-bottom30 <%= (ctrlSimilarVideos.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
            <BW:SimilarVideos ID="ctrlSimilarVideos" runat="server" />
        </section>
        <% } %>
        <script type="text/javascript">
            $('.share-btn').click(function () {
                var str = $(this).attr('data-attr');
                var cururl = window.location.href;
                switch (str) {
                    case 'fb':
                        url = 'https://www.facebook.com/sharer/sharer.php?u=';
                        break;
                    case 'tw':
                        url = 'https://twitter.com/home?status=';
                        break;
                    case 'gp':
                        url = 'https://plus.google.com/share?url=';
                        break;
                    case 'wp':
                        var text = document.getElementsByTagName("title")[0].innerHTML;
                        var message = encodeURIComponent(text) + " - " + encodeURIComponent(cururl);
                        var whatsapp_url = "whatsapp://send?text=" + message;
                        url = whatsapp_url;
                        window.open(url, '_blank');
                        return;
                }
                url += cururl;
                window.open(url, '_blank');
            });
            $('.comma').each(function (i, obj) {
                var y = formatPrice($(this).html());
                if (y != null)
                    $(this).html(y);
            });
            function formatPrice(x) { try { x = x.toString(); var lastThree = x.substring(x.length - 3); var otherNumbers = x.substring(0, x.length - 3); if (otherNumbers != '') lastThree = ',' + lastThree; var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree; return res; } catch (err) { } }
        </script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    </form>
</body>
</html>

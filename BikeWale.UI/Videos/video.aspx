<%@ Page Language="C#" Inherits="Bikewale.Videos.video" AutoEventWireup="false" EnableViewState="false" %>
<!DOCTYPE html>
<%@ Register Src="~/controls/SimilarVideos.ascx" TagName="SimilarVideos" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/controls/GenericBikeInfoControl.ascx" %>
<html>
<head>
    <%   
        title = metaTitle;
        description = metaDesc;
        keywords = metaKeywords;
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        canonical = string.Format("https://www.bikewale.com/bike-videos/{0}-{1}/", videoModel.VideoTitleUrl, videoModel.BasicId);
        alternate = string.Format("https://www.bikewale.com/m/bike-videos/{0}-{1}/", videoModel.VideoTitleUrl, videoModel.BasicId);
    %>
     <!-- #include file="/includes/headscript.aspx" -->
  
    <style type="text/css">
        #embedVideo iframe { width:934px; height:527px; }.video-views-counts { min-width:140px; }.border-light-right { border-right:1px solid #e2e2e2; }.video-social-wrapper { position:fixed; left:0; top:230px; width:54px; height:162px; overflow:hidden; background:#e2e2e2; }.video-social-wrapper li { height:54px; font-size:11px; padding-top:8px; padding-bottom:8px; }.video-social-wrapper a { display:block; color:#fff; text-align:center; }.video-social-wrapper a:hover { text-decoration:none; }.video-social-wrapper span { display:block; margin:0 auto;}.video-social-wrapper span.fa { font-size:25px; }.fb-counter { background:#3b5998; } .tw-counter { background:#00aced; } .gp-counter { background:#dd4b39; }.related-video-jcarousel li { height:312px; border: 1px solid #e2e2e2; padding:20px; }.videocarousel-image-wrapper { width:271px; height:153px; margin-bottom:15px; overflow:hidden; text-align:center; }.videocarousel-image-wrapper a { width:100%; height:100%; display:block; background:url('https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif') no-repeat center center; }.videocarousel-image-wrapper img { width:100%; height:100%; }.more-videos-link { width:200px; display:block; margin:5px auto 25px; }.margin-top8{margin-top:8px;}
    </style>
    <link href="<%=  + staticUrl  %>/css/jquery.floating-social-share.min.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <% if(videoModel!= null) { %>
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-top15 margin-bottom10">
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/"><span itemprop="title">Home</span></a></li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span><a href="/bike-videos/"><span itemprop="title">Bike Videos</span></a></li>
                            <% if(!String.IsNullOrEmpty(videoModel.SubCatName)) {%>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span><a href="<%= Bikewale.Utility.UrlFormatter.VideoByCategoryPageUrl(videoModel.SubCatName, videoModel.SubCatId) %>"><span itemprop="title"><%=videoModel.SubCatName %></span></a></li>
                            <% } %>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span><%=videoModel.VideoTitle %></li>
                        </ul>
                    </div>
                    <h1 class="font26 margin-bottom5"><%=videoModel.VideoTitle%></h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <div id="embedVideo" class="margin-bottom15">
                            <iframe width="934" height="527" src="<%=videoModel.VideoUrl %>&autoplay=1" frameborder="0" allowfullscreen></iframe>
                        </div>
                        <div class="font14 text-light-grey margin-bottom10"><%=videoModel.Description %></div>
                        <p class="video-views-counts border-light-right font14 leftfloat padding-right40">
                            <span class="bwsprite video-views-icon margin-right5"></span><span class="text-light-grey margin-right5">Views:</span><span class="text-default comma"><%=videoModel.Views %></span>
                        </p>
                        <p class="video-views-counts padding-left30 font14 leftfloat border-light-right">
                            <span class="bwsprite video-likes-icon margin-right5"></span><span class="text-light-grey margin-right5">Likes:</span><span class="text-default comma"><%=videoModel.Likes %></span>
                        </p>
                        <div class="leftfloat powerdrift-subscribe padding-left30">
                            <script src="https://apis.google.com/js/platform.js"></script>
                            <div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="default" data-theme="dark" data-count="hidden"></div>
                        </div>
                        <p class="rightfloat text-light-grey font12"><%=videoModel.DisplayDate %></p>
                        <p class="clear"></p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <BW:GenericBikeInfo runat="server" ID="ctrlGenericBikeInfo" />
        <section class="margin-bottom30 <%= (ctrlSimilarVideos.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
         <BW:SimilarVideos ID="ctrlSimilarVideos" runat="server" />              
        </section>
        <% } %>
        <script type="text/javascript">
            $(function () {
                try{
                    $("body").floatingSocialShare();
                    $('.comma').each(function (i, obj) {
                        var y = formatPrice($(this).html());
                        if (y != null)
                            $(this).html(y);
                    });
                }catch(err){}
            });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
    <script type="text/javascript" src="<%= staticUrl %>/src/jquery.floating-social-share.min.js?<%= staticFileVersion %>">"></script>
</body>
</html>
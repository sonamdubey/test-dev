<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Videos.Default" EnableViewState="false" %>
<%@ Register Src="~/m/controls/VideosByCategory.ascx" TagName="ByCategory" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ExpertReviewsVideos.ascx" TagName="ExpertReview" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        .video-jumbotron { width:100%; height:180px; display:block; position:relative; overflow:hidden; background:#fff; }.video-jumbotron a { margin:0 auto; width:320px; height:180px; display:block; background:url('http://img.aeplcdn.com/bikewaleimg/images/circleloader.gif') no-repeat center center; text-align:center; }.video-jumbotron img { width:320px; position:relative; top:-32px; }.video-jumbotron span { width:100%; position: absolute; left:0; bottom:0; text-align:left; font-size: 14px; font-weight:bold; color: #fff; padding: 10px; background: linear-gradient(to bottom, rgba(0,0,0,0), rgba(5,0,0,0.7)); }.video-jumbotron span:hover { text-decoration:underline; }.video-jumbotron-list { padding:20px; overflow:hidden; background:#fff; }.bottom-shadow { -webkit-box-shadow:0 2px 2px #ccc; -moz-box-shadow:0 2px 2px #ccc; box-shadow:0 2px 2px #ccc; }.powerdrift-banner { background:#222 url('http://img.aeplcdn.com/bikewaleimg/m/images/m-powerdrift-banner.jpg') no-repeat center; height:109px; }.powerdrift-banner.container { width:87%; }.powerdrift-subscribe { margin-left:5px; margin-right:5px; padding:5px 5px 0; }.margin-top3 { margin-top:3px; }.text-default { color:#4d5057; }.text-xlight-grey { color:#a8afb3; }.line-height17 { line-height: 1.7; }.video-jumbotron-list li { width:100%; border-top:1px solid #e2e2e2; margin-top:20px; padding-top:20px; }.video-jumbotron-list li:first-child { border-top:none; margin-top:0; padding-top:0; }.video-jumbotron-list a { display:inline-block; vertical-align:middle; }.video-jumbotron-list .jumbotron-list-image { width:100px; height:55px; overflow:hidden; background:url('http://img.aeplcdn.com/bikewaleimg/images/circleloader.gif') no-repeat center center; text-align:center; }.video-jumbotron-list .jumbotron-list-image img { width:100%; position:relative; top:-10px; }.jumbotron-list-title { width:52%; }.swiper-slide.video-carousel-content { width:270px; min-height:290px; background:#fff; border:1px solid #e2e2e2; }.swiper-slide img { width:100%; height:157px; }.video-carousel-image { width:100%; height:157px; display: block; overflow: hidden; text-align: center; position:relative; }.video-carousel-image a { width:100%; height:100%; display: block;}.video-carousel-desc { padding:10px; }.border-light-right { border-right:1px solid #e2e2e2; }.more-videos-link { width:200px;display:block; margin:15px auto;}.swiper-slide.reviews-carousel-content { width:240px; min-height:305px; background:#fff; }.swiper-slide.reviews-carousel-content .video-carousel-image { height:135px; }.reviews-carousel-content img { border: 1px solid #e2e2e2;position: relative;top: -9px;} #expertReviewsWrapper .video-carousel-image a { height:135px; }#expertReviewsWrapper .more-videos-link { margin-bottom:0; padding-bottom:20px; }#expertReviewsWrapper .swiper-wrapper { position:relative; left:20px; }#expertReviewsWrapper .swiper-slide { margin-right:20px !important; }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container">
                <div class="video-jumbotron">
                   <a href="/m<%= Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(ctrlVideosLandingFirst.VideoTitleUrl,ctrlVideosLandingFirst.BasicId.ToString()) %>" >
                        <img class="lazy" data-original="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",ctrlVideosLandingFirst.VideoId)  %>" alt="<%= ctrlVideosLandingFirst.VideoTitle  %>" title="<%= ctrlVideosLandingFirst.VideoTitle  %>" src="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",ctrlVideosLandingFirst.VideoId)  %>" border="0" />
                        <span><%= ctrlVideosLandingFirst.VideoTitle  %></span>
                    </a>
                </div>
                <ul class="video-jumbotron-list bottom-shadow margin-bottom20">
                    <asp:Repeater ID="rptLandingVideos" runat="server">
                            <ItemTemplate>
                    <li>
                        <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="jumbotron-list-image margin-right20">
                            <img class="lazy" data-original="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" border="0" />

                        </a>
                        <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="jumbotron-list-title font14 text-default"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>
                    </li>
                       </ItemTemplate>
                        </asp:Repeater> 
                </ul>
            </div>
        </section>

        <section>
            <div class="powerdrift-banner container font14">
                <p class="text-bold text-white padding-top10 margin-left20 padding-bottom10">Reviews, Specials, Underground, Launch Alerts & a whole lot more...</p>
                <div class="bg-white powerdrift-subscribe">
                    <p class="font14 leftfloat margin-left10 margin-top3">PowerDrift</p>
                    <div class="rightfloat">
                        <script src="https://apis.google.com/js/platform.js"></script>
                        <div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="default" data-count="default"></div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
        </section>
               

        <% if (ctrlExpertReview.FetchedRecordsCount > 0) {%>
        <BW:ExpertReview runat="server" ID="ctrlExpertReview" /> 
        <% } %> 

         <% if (ctrlFirstRide.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlFirstRide" /> 
        <% } %> 
        
        <% if (ctrlLaunchAlert.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlLaunchAlert" />
        <% } %> 

        <% if (ctrlFirstLook.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlFirstLook" /> 
        <% } %> 

        <% if (ctrlPowerDriftBlockBuster.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlPowerDriftBlockBuster" />
        <% } %> 
        
        <% if (ctrlMotorSports.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlMotorSports" />
        <% } %> 

         <% if (ctrlPowerDriftSpecials.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlPowerDriftSpecials" />
        <% } %> 
        
        <% if (ctrlTopMusic.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlTopMusic" />
        <% } %> 

        <% if (ctrlMiscellaneous.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlMiscellaneous" />
        <% } %> 


        <script type="text/javascript">
            $(document).ready(function () {
                $("img.lazy").lazyload();
            });
        </script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    </form>
</body>
</html>

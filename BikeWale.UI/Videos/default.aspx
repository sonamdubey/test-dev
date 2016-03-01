<%@ Page Language="C#" Inherits="Bikewale.Videos.Default" AutoEventWireup="false" EnableViewState="false" Trace="false" %>
<%@ Import namespace="Bikewale.Utility.StringExtention" %>
<%@ Register TagPrefix="BikeWale" TagName="video" Src="/controls/VideoCarousel.ascx" %>

<%@ Register Src="~/controls/VideoByCategory.ascx" TagName="ByCategory" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviewsVideos.ascx" TagName="ExpertReview" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <%  
        title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
        description ="Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters - features, performance, price, fuel economy, handling and more.";
        canonical = "http://www.bikewale.com/bike-videos/";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        #videoJumbotron .grid-8 { padding:20px 0 20px 20px; }#videoJumbotron .grid-4 { padding:20px 10px; }.main-video-container { width:624px; height:350px; display:block; position:relative; background:url('http://img.aeplcdn.com/bikewaleimg/images/loader.gif') no-repeat center center; text-align:center; overflow:hidden; }.main-video-container img { width:100%; position:relative; top:-59px; }.main-video-container span { position: absolute; left:0; bottom:0; text-align:left; font-size: 22px; width: 624px; color: #fff; padding: 20px; background: linear-gradient(to bottom, rgba(0,0,0,0), rgba(5,0,0,0.7)); }.main-video-container span:hover { text-decoration:underline; }#videoJumbotron ul { border-left:1px solid #e2e2e2; padding-left:14px; }#videoJumbotron li { width:280px; height:auto; margin-top:20px; padding-top:20px; border-top:1px solid #e2e2e2; }#videoJumbotron li:first-child { margin-top:0; padding-top:0; border-top:none; }.sidebar-video-image { width:100px; height:55px; overflow:hidden; margin-right:15px; display:inline-block; vertical-align:middle; background:url('http://img.aeplcdn.com/bikewaleimg/images/loader.gif') no-repeat center center; text-align:center; }.sidebar-video-title { width:160px; height:auto; display:inline-block; vertical-align:middle; }.sidebar-video-title:hover { color:#4d5057; }.sidebar-video-image img { width:100%; position:relative; top:-10px; }.powerdrift-banner { background:url('http://img.aeplcdn.com/bikewaleimg/images/d-powerdrift-banner.jpg') no-repeat center; height:129px; }.powerdrift-banner h3 { line-height:1.7; }.margin-top35 { margin-top:35px; }.powerdrift-subscribe { padding:15px 15px 10px; margin-top:25px; margin-right:25px; background:#fff; }.firstride-jcarousel li { height:312px; border: 1px solid #e2e2e2; padding:20px; }.videocarousel-image-wrapper { width:271px; height:153px; margin-bottom:15px; overflow:hidden; text-align:center; }.videocarousel-image-wrapper a, .reviews-image-wrapper a { width:100%; height:100%; display:block; overflow:hidden; background:url('http://img.aeplcdn.com/bikewaleimg/images/loader.gif') no-repeat center center; }.videocarousel-image-wrapper img, .reviews-image-wrapper img { width:100%; }.reviews-image-wrapper img { position:relative; top:-43px; }.border-light-right { border-right:1px solid #e2e2e2; }.more-videos-link { width:200px; display:block; margin:5px auto 25px; }.reviews-image-wrapper { width:458px; height:258px; margin-bottom:15px; border:1px solid #e2e2e2; overflow:hidden; text-align:center; }
    </style>
    <%
        isAd970x90Shown = false;
         %>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-top15 margin-bottom10">
                        <ul>
                            <li><a href="/"><span>Home</span></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Videos</li>
                        </ul>
                    </div>
                    <h1 class="font26 margin-bottom5">Videos</h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        
        <section>
    <div id="videoJumbotron" class="container">
        <div class="grid-12">
            <div class="content-box-shadow">
                <div class="grid-8">
                    <a href="<%= Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(ctrlVideosLandingFirst.VideoTitleUrl,ctrlVideosLandingFirst.BasicId.ToString()) %>" class="main-video-container">
                        <img class="lazy" data-original="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",ctrlVideosLandingFirst.VideoId)  %>" alt="<%= ctrlVideosLandingFirst.VideoTitle  %>" title="<%= ctrlVideosLandingFirst.VideoTitle  %>" src="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",ctrlVideosLandingFirst.VideoId)  %>" border="0" />
                        <span><%= ctrlVideosLandingFirst.VideoTitle  %></span>
                    </a>
                </div>
                <div class="grid-4">
                    <ul> 
                        <asp:Repeater ID="rptLandingVideos" runat="server">
                            <ItemTemplate>

                                <li>
                                    <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="sidebar-video-image">
                                        <img class="lazy" data-original="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" border="0" /></a>
                                    <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" class="sidebar-video-title font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString().Truncate(35) %></a>
                                </li>

                            </ItemTemplate>
                        </asp:Repeater> 
                    </ul>
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
    </div>
</section>  

        <section>
            <div class="container margin-top20 powerdrift-banner">
                <div class="grid-12">
                    <div class="leftfloat margin-left25 margin-top35">
                        <h3 class="text-white">Reviews, Specials, Underground, Launch Alerts &<br />a whole lot more...</h3>
                    </div>
                    <div class="rightfloat powerdrift-subscribe">
                        <script src="https://apis.google.com/js/platform.js"></script>
                        <div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="full" data-count="hidden"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <% if (ctrlFirstRide.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlFirstRide" /> 
        <% } %>         

        <% if (ctrlExpertReview.FetchedRecordsCount > 0) {%>
        <BW:ExpertReview runat="server" ID="ctrlExpertReview" /> 
        <% } %> 


        <% if (ctrlLaunchAlert.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlLaunchAlert" />
        <% } %> 


        <% if (ctrlMiscellaneous.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlMiscellaneous" />
        <% } %> 


        <% if (ctrlTopMusic.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlTopMusic" />
        <% } %> 


        <% if (ctrlDoItYourself.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlDoItYourself" />
        <% } %> 

        <script type="text/javascript">
            $(document).ready(function () { $("img.lazy").lazyload(); });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Videos.Default" EnableViewState="false" %>
<%@ Register Src="~/m/controls/VideosByCategory.ascx" TagName="ByCategory" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ExpertReviewsVideos.ascx" TagName="ExpertReview" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/video.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
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
                        <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="font14 text-default padding-left20"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>
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
            $(document).ready(function () {
                $("img.lazy").lazyload();
            });
        </script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
    </form>
</body>
</html>

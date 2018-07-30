<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Videos.Default" EnableViewState="false" %>

<%@ Register Src="~/m/controls/VideosByCategory.ascx" TagName="ByCategory" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/ExpertReviewsVideos.ascx" TagName="ExpertReview" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>

    <%   
        title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
        description = "Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters - features, performance, price, fuel economy, handling and more.";
        keywords = "bike videos, video reviews, expert video reviews, road test videos, bike comparison videos";
        canonical = "https://www.bikewale.com/bike-videos/";
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
     <link href="<%= staticUrl  %>/m/css/videos/bwm-videos-landing.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

</head>
<body class="bg-light-grey page-type-landing">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container">
                <div class="video-jumbotron">
                    <a href="/m<%= Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(ctrlVideosLandingFirst.VideoTitleUrl,ctrlVideosLandingFirst.BasicId.ToString()) %>">
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


        <% if (ctrlExpertReview.FetchedRecordsCount > 0)
           {%>
        <BW:ExpertReview runat="server" ID="ctrlExpertReview" />
        <% } %>

        <% if (ctrlFirstRide.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlFirstRide" />
        <% } %>

        <% if (ctrlLaunchAlert.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlLaunchAlert" />
        <% } %>

        <% if (ctrlFirstLook.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlFirstLook" />
        <% } %>
        <% if (objVideo != null && objVideo.TopMakeList != null)
           {  %>
        <section>
             <h2 class="text-center padding-top10 padding-bottom15">Browse videos by brands</h2>
            <div class="container bg-white box-shadow card-bottom-margin padding-top25 padding-bottom20 collapsible-brand-content margin-bottom25">
                <div id="brand-type-container" class="brand-type-container">
                    <ul class="text-center">
                        <%foreach (var bikebrand in objVideo.TopMakeList)
                          {%>
                        <li>
                            <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(bikebrand.MaskingName,string.Empty) %>" title="<%=bikebrand.MakeName %> bikes videos">
                                <span class="brand-type">
                                    <span class="lazy brandlogosprite brand-<%=bikebrand.MakeId %>"></span>
                                </span>
                                <span class="brand-type-title"><%=bikebrand.MakeName %></span>
                            </a>
                        </li>
                        <%} %>
                    </ul>
                    <%if (objVideo.OtherMakeList != null && objVideo.OtherMakeList.Count()>0)
                      { %>
                    <ul class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                        <%foreach (var bikebrand in objVideo.OtherMakeList)
                          {%>
                        <li>
                            <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(bikebrand.MaskingName,string.Empty) %>" title="<%=bikebrand.MakeName %> bikes videos">
                                <span class="brand-type">
                                    <span class="lazy brandlogosprite brand-<%=bikebrand.MakeId %>"></span>
                                </span>
                                <span class="brand-type-title"><%=bikebrand.MakeName %></span>
                            </a>
                        </li>
                        <%} %>
                    </ul>

                </div>
                <div class="view-all-btn-container">
                    <a href="javascript:void(0)" rel="nofollow" class="view-brandType btn view-all-target-btn rotate-arrow"><span class="btn-label">View more brands</span><span class="bwmsprite teal-right"></span></a>
                </div>
                <%} %>
            </div>
        </section>
        <%} %>

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

        <% if (ctrlTopMusic.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlTopMusic" />
        <% } %>

        <% if (ctrlMiscellaneous.FetchedRecordsCount > 0)
           {%>
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

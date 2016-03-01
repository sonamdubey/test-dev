<%@ Page Language="C#" Inherits="Bikewale.Videos.Default" AutoEventWireup="false" EnableViewState="false" Trace="false" %>

<%@ Import Namespace="Bikewale.Utility.StringExtention" %>
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
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/video.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css" />
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
                        <h3 class="text-white">Reviews, Specials, Underground, Launch Alerts &<br />
                            a whole lot more...</h3>
                    </div>
                    <div class="rightfloat powerdrift-subscribe">
                        <script src="https://apis.google.com/js/platform.js"></script>
                        <div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="full" data-count="hidden"></div>
                    </div>
                </div>
                <div class="clear"></div>
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

        <% if (ctrlLaunchAlert.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlLaunchAlert" />
        <% } %> 

        <% if (ctrlFirstLook.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlFirstLook" />
        <% } %>


        <% if (ctrlPDBlockbuster.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlPDBlockbuster" />
        <% } %>


        <% if (ctrlMotorSports.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlMotorSports" />
        <% } %>          


        <% if (ctrlPDSpecials.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlPDSpecials" />
        <% } %>
        

        <% if (ctrlTopMusic.FetchedRecordsCount > 0)
           {%>
        <BW:ByCategory runat="server" ID="ctrlTopMusic" />
        <% } %>

         <% if (ctrlMiscellaneous.FetchedRecordsCount > 0) {%>
        <BW:ByCategory runat="server" ID="ctrlMiscellaneous" />
        <% } %> 

        <script type="text/javascript">
            $(document).ready(function () { $("img.lazy").lazyload(); });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
</body>
</html>

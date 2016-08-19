<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewVideosControl" %>
<h2 class="padding-right10 padding-left10"><%= WidgetTitle %> Videos</h2>
<div class="model-updates-videos-container" id="ctrlVideos">
    <!-- Videos data code starts here-->
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate>
            <div class="margin-bottom20">
                <div class="grid-4">
                    <div class="model-preview-image-container">
                        <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>">
                            <img class="lazy" data-original="<%#String.Format("http://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" alt="" src="" />
                            <span class="play-icon-wrapper">
                                <span class="bwsprite video-play-icon"></span>
                            </span>
                        </a>
                    </div>
                </div>
                <div class="grid-8">
                    <a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="article-target-link"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>
                    <div class="article-stats-left-grid">
                        <span class="bwsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString(),"MMM dd, yyyy") %></span>
                    </div>
                    <div class="article-stats-right-grid">
                        <span class="bwsprite review-sm-lgt-grey"></span>
                        <span class="article-stats-content"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                    </div>
                    <p class="margin-top12 line-height17">
                        <!-- desc -->
                    </p>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="padding-left10">
        <a title="<%= linkTitle %>" href="<%=MoreVideoUrl%>"  class="bw-ga" c="Model_Page" a="View_all_videos_link_cliked" v="myBikeName">View all videos<span class="bwsprite blue-right-arrow-icon"></span></a>        
    </div>
</div>
<!-- Ends here-->


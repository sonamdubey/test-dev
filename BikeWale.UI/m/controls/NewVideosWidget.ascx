<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewVideosWidget" %>
<div class="model-expert-review-container">                    
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate> 
        <div class="margin-bottom20">
            <div class="review-image-wrapper">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="video-image-thumbnail">
                    <img class="lazy" data-original="<%#String.Format("https://img.youtube.com/vi/{0}/mqdefault.jpg",DataBinder.Eval(Container.DataItem,"VideoId")) %>"
                        alt="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="" border="0" />
                   
                    <span class="play-icon-wrapper">
                        <span class="bwmsprite video-play-icon"></span>
                    </span>
                </a>
            </div>
            <div class="review-heading-wrapper">
                <a href="/m<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" 
                    class="font14 target-link" title="<%#DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %>" ><%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem,"VideoTitle").ToString(), 44) %></a>
                <div class="grid-7 alpha padding-right5">
                    <span class="bwmsprite calender-grey-sm-icon"></span>
                    <span class="article-stats-content"><%# Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(DataBinder.Eval(Container.DataItem,"DisplayDate")), string.Format("dd MMM yyyy")) %></span>
                </div>
                <div class="grid-5 alpha omega">
                    <span class="bwmsprite views-grey-sm-icon"></span>
                    <span class="article-stats-content video-view-count"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span>
                </div>
                <div class="clear"></div>
            </div>
            <p class="margin-top10">
                <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "Description").ToString(),180) %>
            </p>
        </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<div class="view-all-btn-container">
    <a title="<%= linkTitle %>" href="<%=MoreVideoUrl%>" class="bw-ga btn view-all-target-btn" data-cat="Model_Page" data-act="View_all_videos_link_cliked" data-var="myBikeName">View all videos<span class="bwmsprite teal-right"></span></a>
</div>
